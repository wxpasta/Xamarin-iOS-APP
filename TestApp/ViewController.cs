using System;
using UIKit;
using Network;
using Foundation;

namespace xxxApp
{
    public partial class ViewController : UIViewController
    {

        
        
        public ViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            //NodeModel(int parentId, int nodeId, string name, int depth, bool expand)

            NetworkHelper network = NetworkHelper.Instance();

            NSMutableDictionary dic = new NSMutableDictionary();
            dic.SetValueForKey((NSString)"sap", (NSString)"client_id");
            dic.SetValueForKey((NSString)"secret", (NSString)"client_secret");
            dic.SetValueForKey((NSString)"value123", (NSString)"key321");
            dic.SetValueForKey((NSString)"testValue", (NSString)"testKey");
            string nameUlr = network.GroupGetString("www.baidu.com",dic);
            
        }


        


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        
    }
}