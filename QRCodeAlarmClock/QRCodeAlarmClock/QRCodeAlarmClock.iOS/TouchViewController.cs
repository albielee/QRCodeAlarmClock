using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using CoreGraphics;
using System.CodeDom.Compiler;

namespace QRCodeAlarmClock.iOS
{
    public class TouchViewController : UILabel
    {
        #region Private Variables
        private bool imageHighlighted = false;
        private bool touchStartedInside = true;
        #endregion

        #region Constructors
        public TouchViewController(IntPtr handle) : base(handle)
        {
            Text = "TESTING";
        }
        #endregion


        #region Override Methods
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            // Get the current touch
            UITouch touch = touches.AnyObject as UITouch;
            if (touch != null)
            {

            }
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);
            // get the touch
            UITouch touch = touches.AnyObject as UITouch;
            if (touch != null)
            {
                // check to see if the touch started in the drag me image
                if (touchStartedInside)
                {
                    // move the shape
                    //float offsetX = touch.PreviousLocationInView(View).X - touch.LocationInView(View).X;
                    //float offsetY = touch.PreviousLocationInView(View).Y - touch.LocationInView(View).Y;
                    //DragImage.Frame = new RectangleF(new PointF(DragImage.Frame.X - offsetX, DragImage.Frame.Y - offsetY), DragImage.Frame.Size);
                }
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            // reset our tracking flags
            touchStartedInside = false;
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            // get the touch
            UITouch touch = touches.AnyObject as UITouch;
            if (touch != null)
            {
                //==== IMAGE TOUCH
                //if (TouchImage.Frame.Contains(touch.LocationInView(TouchView)))
                //{
                //    TouchImage.Image = UIImage.FromBundle("TouchMe.png");
                //    TouchStatus.Text = "Touches Ended";
                //}
            }
            // reset our tracking flags
            touchStartedInside = false;
        }
        #endregion
    }
}