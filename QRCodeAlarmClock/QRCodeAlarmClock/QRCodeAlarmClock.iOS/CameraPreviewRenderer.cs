using System;
using AVFoundation;
using Foundation;
using QRCodeAlarmClock;
using QRCodeAlarmClock.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CameraPreview), typeof(CameraPreviewRenderer))]
namespace QRCodeAlarmClock.iOS
{
    public class CameraOutput : IAVCapturePhotoCaptureDelegate
    {
       
        public IntPtr Handle => this.Handle;

        public void DidFinishProcessingPhoto(AVCapturePhotoOutput output,
                    AVCapturePhoto photo, Foundation.NSError error)
        {
            using (NSData imageData = photo.FileDataRepresentation)
            {
                Byte[] myByteArray = new Byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));

                QRCodeDetector detector = new QRCodeDetector();
                string code = detector.ScanImage(myByteArray);
                if (code != null)
                {

                }
            }
        }

        public void DidFinishProcessingPhoto(Foundation.NSError error)
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class CameraPreviewRenderer : ViewRenderer<CameraPreview, UICameraPreview>, IAVCapturePhotoCaptureDelegate
    {
        UICameraPreview uiCameraPreview;
        private AVCapturePhotoOutput photoOutput = new AVCapturePhotoOutput();

        protected override void OnElementChanged(ElementChangedEventArgs<CameraPreview> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe
                uiCameraPreview.Tapped -= OnCameraPreviewTapped;
            }
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    uiCameraPreview = new UICameraPreview(e.NewElement.Camera);
                    SetNativeControl(uiCameraPreview);
                }

                // Subscribe
                uiCameraPreview.Tapped += OnCameraPreviewTapped;
            }
        }


        void OnCameraPreviewTapped(object sender, EventArgs e)
            {
            AVCaptureSession captureSession = uiCameraPreview.CaptureSession;

            //AVCaptureDevice captureDevice = AVCaptureDevice.GetDefaultDevice(AVMediaTypes.Video);
            //NSError inputError = new NSError();
            //AVCaptureDeviceInput input = new AVCaptureDeviceInput(captureDevice, out inputError);
            //captureSession.AddInput(input);

            captureSession.AddOutput(photoOutput);

            AVCapturePhotoSettings settings = AVCapturePhotoSettings.Create();

            NSError error = new NSError();
            AVCapturePhoto photo = null;
            CameraOutput co = new CameraOutput();
            
            photoOutput.CapturePhoto(settings, co);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Control.CaptureSession.Dispose();
                Control.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}