using System;
using UIKit;
using Foundation;
using CoreGraphics;

namespace Tools
{
    public class ImageTool
    {
        public static readonly object padlock = new object();
        private static ImageTool instance = null;

        public static ImageTool SharedInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ImageTool();
                    }
                    return instance;
                }
            }
        }

        public NSData ScreenshotImageData()
        {
            CGSize imageSize = new CGSize();
            UIInterfaceOrientation orientation = UIApplication.SharedApplication.StatusBarOrientation;
            if (UIInterfaceOrientation.Portrait == orientation)
            {
                imageSize = UIScreen.MainScreen.Bounds.Size;
            }
            else
            {
                imageSize = new CGSize(UIScreen.MainScreen.Bounds.Size.Height, UIScreen.MainScreen.Bounds.Size.Width);
            }

            
            UIGraphics.BeginImageContextWithOptions(imageSize, false, 0);
            
            CGContext context = UIGraphics.GetCurrentContext();
            foreach (var window in UIApplication.SharedApplication.Windows)
            {
                context.SaveState();
                context.TranslateCTM(window.Center.X,window.Center.Y);
                context.ConcatCTM(window.Transform);
                context.TranslateCTM(-window.Bounds.Size.Width * window.Layer.AnchorPoint.X, -window.Bounds.Size.Height * window.Layer.AnchorPoint.Y);
                if (orientation == UIInterfaceOrientation.LandscapeLeft)
                {
                    context.RotateCTM((nfloat)1.57079632679489661923132169163975144);
                    context.TranslateCTM(0,-imageSize.Width);
                }
                else if (orientation == UIInterfaceOrientation.LandscapeRight)
                {
                    context.RotateCTM((nfloat)1.57079632679489661923132169163975144);
                    context.TranslateCTM(0, -imageSize.Height);
                }
                else if (orientation == UIInterfaceOrientation.PortraitUpsideDown)
                {
                    context.RotateCTM((nfloat)3.14159265358979323846264338327950288);
                    context.TranslateCTM(-imageSize.Width, -imageSize.Height);
                }

                if (window.RespondsToSelector(new ObjCRuntime.Selector("window.DrawViewHierarchy")))
                {
                    window.DrawViewHierarchy(window.Bounds, true);
                }
                else
                {
                    window.Layer.RenderInContext(context);
                }

                context.RestoreState();
            }
            UIImage image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            // AsPNG() == UIImagePNGRepresentation(image);
            NSData imgData = image.AsPNG();
            return imgData;

        }

    }
}
