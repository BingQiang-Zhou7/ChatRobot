using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Qinyunke chatClient = null;
        AoRuiAudio aoRuiAudio = null;
        BaiduAI baiduAI = null;

        string filePath = null;

        public MainWindow()
        {
            InitializeComponent();
            #region 判断是否连接网络，未连接网络，退出程序
            if (false == new CheckNetworkAccess().isConnect())
            {
                MessageBox.Show("网络连接出现问题，请稍后再试！");
                App.Current.Shutdown();//关闭程序
            }
            #endregion
            aoRuiAudio = new AoRuiAudio();
            baiduAI = new BaiduAI();
        }
        #region 点击录音图标消息
        private void Record(object sender, MouseButtonEventArgs e)
        {

            //MessageBox.Show("点击了");
            App.IsRecord = App.IsRecord % 2 + 1;
            string path = "/WpfApp1;component/images/" + App.IsRecord + ".png";
            //修改图片
            BitmapImage ima = new BitmapImage(new Uri(path, UriKind.Relative));
            recordImage.Source = ima;
            if (App.IsRecord == 1)
            {
                showRecord.Text = "开始讲话";
                tabItemText.IsEnabled = true;
                
                aoRuiAudio.CloseMircoPhone();
                //TODO 语音识别，发送文本给机器人，返回机器人聊天消息，语音合成，播放wav，输出文本
                filePath = AudioFileFormatChange.Mp3ToWav(filePath);
                string speechRecongniationResult = baiduAI.AsrData(filePath);
                Console.WriteLine(speechRecongniationResult);
                if (null == chatClient)
                {
                    chatClient = new Qinyunke();
                }
                string robotMsg = SendMsgToRobot(speechRecongniationResult,true);
                baiduAI.TtsAndPlaySound(robotMsg, App.Speed, App.Volume, App.Voice, filePath);
            }
            else
            {
                showRecord.Text = "正在讲话";
                tabItemText.IsEnabled = false;//正在说话不可切换到文本聊天
                filePath = "..\\..\\cache\\" + System.DateTime.Now.ToFileTime()+ ".mp3";
                aoRuiAudio.InitialCapturer(filePath);
                aoRuiAudio.OpenMircoPhone();
            }
            //ChatArea.AppendText("点击了");
        }
        #endregion

        #region 发送消息给机器人
        private void SendMessage(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                SendMsgToRobot(TextArea.Text,false);
                TextArea.Text = "";
            }
        }
        #endregion

        #region 点击设置图标消息
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window window = new SettingWindow();
            window.ShowDialog();
            //MessageBox.Show(App.Speed.ToString()+"\t"+App.Voice.ToString()+"\t"+App.Volume.ToString());
        }
        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != chatClient)
            {
                //释放资源
                chatClient.ReleaseResource();
            }
        }


        public string SendMsgToRobot(string text,bool isSpeech)
        {
            if (text.Equals("exit"))
            {
                Application.Current.Shutdown();
            }
            if (true == isSpeech)
            {
                ChatArea.AppendText("我：" + DateTime.Now.ToString() + "\r\n\t【语音消息】" + text + "\r\n");
            }
            else
            {
                ChatArea.AppendText("我：" + DateTime.Now.ToString() + "\r\n" + text + "\r\n");
            }
            string str = text;
            

            if (chatClient == null)
            {
                chatClient = new Qinyunke();
            }
            string msg = chatClient.ChatWithRobot(str);

            ChatArea.AppendText("机器人：" + DateTime.Now.ToString() + "\r\n" + msg + "\r\n");
            ChatArea.ScrollToEnd();
            return msg;
        }
    }
}
