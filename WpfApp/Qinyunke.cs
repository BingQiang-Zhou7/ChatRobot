using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Qinyunke
    {
        private WebClient webClient = null;

        //初始化
        public Qinyunke()
        {
            webClient = new WebClient();
            webClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据  
        }

        //与机器人聊天
        public string ChatWithRobot(string str)
        {
            Byte[] pageData = webClient.DownloadData("http://api.qingyunke.com/api.php?key=free&appid=0&msg=" + str);
            //string pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句         
            string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句  
            string[] strList = pageHtml.Split('\"');
            //foreach (var item in strList)
            //{
            //    Console.WriteLine(item);
            //}
            //提取返回的信息
            string msg = strList[strList.Length - 2];//.Substring(0, strList[strList.Length - 2].Length - 1)
                                                     //ChatArea.AppendText("我：" + DateTime.Now.ToString() + "\n" + TextArea.Text + "\n")
            //Console.WriteLine(msg);
            //处理换行
            String[] strs = msg.Split("{br}".ToCharArray());
            msg = "";
            foreach (var item in strs)
            {
                if(! item.Equals(""))
                msg += item+"\r\n";
            }
            return msg;
        }
        public void ReleaseResource()
        {
            if(webClient != null)
            {
                webClient.Dispose();
                webClient = null;
            }
        }
    }
}
