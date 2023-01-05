using AVFoundation;
using CoreGraphics;
using CoreMedia;
using CoreVideo;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace QRCodeAlarmClock.iOS
{
    public class OutputRecorder : AVCaptureVideoDataOutputSampleBufferDelegate
    {
        #region Computed Properties
        public UIImageView DisplayView { get; set; }
        #endregion

        #region Constructors
        public OutputRecorder()
        {

        }
        #endregion

        #region Private Methods
        private UIImage GetImageFromSampleBuffer(CMSampleBuffer sampleBuffer)
        {

            // Get a pixel buffer from the sample buffer
            using (var pixelBuffer = sampleBuffer.GetImageBuffer() as CVPixelBuffer)
            {
                // Lock the base address
                pixelBuffer.Lock(CVOptionFlags.None);

                // Prepare to decode buffer
                var flags = CGBitmapFlags.PremultipliedFirst | CGBitmapFlags.ByteOrder32Little;

                // Decode buffer - Create a new colorspace
                using (var cs = CGColorSpace.CreateDeviceRGB())
                {

                    // Create new context from buffer
                    using (var context = new CGBitmapContext(pixelBuffer.BaseAddress,
                        pixelBuffer.Width,
                        pixelBuffer.Height,
                        8,
                        pixelBuffer.BytesPerRow,
                        cs,
                        (CGImageAlphaInfo)flags))
                    {

                        // Get the image from the context
                        using (var cgImage = context.ToImage())
                        {

                            // Unlock and return image
                            pixelBuffer.Unlock(CVOptionFlags.None);
                            return UIImage.FromImage(cgImage);
                        }
                    }
                }
            }
        }
        #endregion

        #region Override Methods
        public override void DidOutputSampleBuffer(AVCaptureOutput captureOutput, CMSampleBuffer sampleBuffer, AVCaptureConnection connection)
        {
            // Trap all errors
            try
            {
                // Grab an image from the buffer
                var image = GetImageFromSampleBuffer(sampleBuffer);

                // Display the image
                if (DisplayView != null)
                {
                    DisplayView.BeginInvokeOnMainThread(() => {
                        // Set the image
                        if (DisplayView.Image != null) DisplayView.Image.Dispose();
                        DisplayView.Image = image;

                        // Rotate image to the correct display orientation
                        DisplayView.Transform = CGAffineTransform.MakeRotation((float)Math.PI / 2);
                    });
                }

                // IMPORTANT: You must release the buffer because AVFoundation has a fixed number
                // of buffers and will stop delivering frames if it runs out.
                sampleBuffer.Dispose();
            }
            catch (Exception e)
            {
                // Report error
                Console.WriteLine("Error sampling buffer: {0}", e.Message);
            }
        }
        #endregion
    }
}