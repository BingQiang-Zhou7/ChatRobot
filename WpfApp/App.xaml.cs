using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        //语速
        private static int speed = 5;
        //音量
        private static int volume = 5;
        //声音（音色）
        private static int voice = 0;
        //是否正在讲话
        private static int isRecord = 1;

        public static int Speed { get => speed; set => speed = value; }
        public static int Volume { get => volume; set => volume = value; }
        public static int Voice { get => voice; set => voice = value; }

        public static int IsRecord { get => isRecord; set => isRecord = value; }
    }

    //TODO 录音待测试、语音合成、语音识别 待测试
    //TODO 封装百度AI库
}
