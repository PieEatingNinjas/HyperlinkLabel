
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace HyperlinkLabelControl.iOS.Controls
{
    public class HyperlinkUIView : UITextView
    {
        public HyperlinkUIView()
        {
            Selectable = true;
            Editable = false;
            BackgroundColor = UIColor.Clear;
        }

        public override bool CanBecomeFirstResponder => false;

        public override bool GestureRecognizerShouldBegin(UIGestureRecognizer gestureRecognizer)
        {
            //Preventing standard actions on UITextView that are triggered after long press
            if (gestureRecognizer is UILongPressGestureRecognizer longpress 
                && longpress.MinimumPressDuration == .5)
                return false;

            return true;
        }

        public override bool CanPerform(Selector action, NSObject withSender) => false;

        public override void LayoutSubviews()
        {
            //Make the TextView as large as its content
            base.LayoutSubviews();
            var x = new CGSize(this.Frame.Size.Width, double.MaxValue);

            var fits = SizeThatFits(x);

            var frame = Frame;

            frame.Size = fits;
        }
    }
}