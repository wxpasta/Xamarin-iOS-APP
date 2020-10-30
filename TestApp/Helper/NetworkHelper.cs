using System;
using System.IO;
using System.Net;
using System.Text;
using Foundation;

namespace Network
{


    public class NetworkHelper
    {
        public delegate void NetworkHelperResponse(NSDictionary dicData, NSHttpUrlResponse response, NSError error);
        public event NetworkHelperResponse NetworkHelperRDelegate;   //声明事件

        //public  void BJCompletionHandler (double duration, NetworkHelperResponse completionHandler);

        // 定义一个静态变量来保存类的实例
        private static NetworkHelper uniqueInstance;

        // 定义私有构造函数，使外界不能创建该类实例
        private NetworkHelper()
        {

        }

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static NetworkHelper Instance()
        {
            // 如果类的实例不存在则创建，否则直接返回
            if (uniqueInstance == null)
            {
                uniqueInstance = new NetworkHelper();
            }
            return uniqueInstance;
        }

        public void POST(string urlString, NSDictionary param)
        {
            NSError err;
            // 格式化字典
            NSData dataParam = NSJsonSerialization.Serialize(param, NSJsonWritingOptions.PrettyPrinted, out err);

            NSString urlstring = new NSString(urlString);
            // 请求配置
            NSMutableUrlRequest request = new NSMutableUrlRequest(new NSUrl(urlstring))
            {
                HttpMethod = "POST",
                Body = dataParam,
                TimeoutInterval = 15.0
            };
            PubliceRequestMethod(request);
        }

        public void GET(string urlString, NSDictionary param)
        {
            // 拼接urlstring
            string groupGetStr = GroupGetString(urlString, param);

            NSString urlstring = new NSString(groupGetStr);
            // 请求配置
            NSMutableUrlRequest request = new NSMutableUrlRequest(new NSUrl(urlstring))
            {
                HttpMethod = "GET",
                TimeoutInterval = 15.0
            };
            PubliceRequestMethod(request);
        }

        private void PubliceRequestMethod(NSMutableUrlRequest request)
        {
            NSError err;
            // 设置header
            NSMutableDictionary header = new NSMutableDictionary();
            header.SetValueForKey((NSString)"application/json; charset=utf-8", (NSString)"Content-Type");
            request.Headers = header;

            NSUrlSession session = NSUrlSession.FromConfiguration(NSUrlSessionConfiguration.DefaultSessionConfiguration, (INSUrlSessionDelegate)this, NSOperationQueue.CurrentQueue);

            NSUrlSessionDataTask task = session.CreateDataTask(request, (data, response, error) =>
            {
                if (error != null)
                {
                    Console.WriteLine(error.Description);
                }
                else
                {
                    NSHttpUrlResponse resp = (NSHttpUrlResponse)response;
                    NSDictionary dicData = (NSDictionary)NSJsonSerialization.Deserialize(data, NSJsonReadingOptions.MutableLeaves, out err);
                    if (dicData == null)
                    {
                        return;
                    }
                    NSObject code = dicData.ValueForKey((NSString)"code");
                    if (resp.StatusCode == 200 && code.ToString().Equals("0"))
                    {
                        NetworkHelperRDelegate(dicData, resp, null);
                        Console.WriteLine(resp.Description);
                    }
                    else
                    {
                        NetworkHelperRDelegate(dicData, resp, err);
                        string message = dicData.ValueForKey((NSString)"message").ToString();
                        Console.WriteLine(dicData.Description);
                    }
                }

            });
            task.Resume();
        }

        public string GroupGetString(string url, NSDictionary dic)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            if (dic.Count > 0)
            {
                builder.Append("?");
                int i = 0;
                foreach (var item in dic)
                {
                    if (i > 0)
                        builder.Append("&");

                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
            }
            return builder.ToString();
        }
    }
}
