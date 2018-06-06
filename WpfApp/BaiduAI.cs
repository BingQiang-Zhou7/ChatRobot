using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using PersonalInfo;

namespace WpfApp1
{
    public class BaiduAI
    {
        // 设置APPID/AK/SK
        private string API_KEY = BaiduAPIInfo.API_KEY;
        private string SECRET_KEY = BaiduAPIInfo.SECRET_KEY;

        WpfApp1.AoRuiAudio aoRuiAudio = null;

        /// <summary>
        /// 语音合成
        /// </summary>
        /// <param name="text">合成文本（合成文本长度必须小于1024字节，如果本文长度较长）</param>
        /// <param name="outFilePath">输出MP3文件名，不加后缀</param>
        public void TtsAndPlaySound(string text, int speed = 5, int volume = 5, int voices = 0,string outFileName = null)
        {
            var client = new Baidu.Aip.Speech.Tts(API_KEY, SECRET_KEY);
            // 可选参数
            var option = new Dictionary<string, object>()
            {
                {"spd", speed}, // 语速
                {"vol", volume}, // 音量
                {"per", voices}  // 发音人，4：情感度丫丫童声
            };
            var result = client.Synthesis(text, option);

            string filePath = null;
            if (result.ErrorCode == 0)  // 或 result.Success
            {
                if (null == outFileName)
                {
                    //如果合成的文本长于6个字符，前面6个字符作为文件名，小于6个以文本内容作文件名
                    filePath = "..\\..\\cache\\" + text.Substring(0, text.Length > 6 ? 6 : text.Length) + ".mp3";
                }
                else
                {
                    //自定义文件名
                    filePath = outFileName;
                    filePath = filePath.Substring(0, filePath.Length - 4) + ".mp3";
                    Console.WriteLine(filePath);
                }
                File.WriteAllBytes(filePath, result.Data);
            }
            filePath = WpfApp1.AudioFileFormatChange.Mp3ToWav(filePath);
            if(aoRuiAudio == null)
            {
                aoRuiAudio = new WpfApp1.AoRuiAudio();
            }
            aoRuiAudio.PlayWavSound(filePath);
        }

        //语音识别
        public string AsrData(string filePath)
        {
            var client = new Baidu.Aip.Speech.Asr(API_KEY, SECRET_KEY);
            //client.Timeout = 60000;  // 修改超时时间
            var data = File.ReadAllBytes(filePath);
            client.Timeout = 120000; // 若语音较长，建议设置更大的超时时间. ms
            string result = client.Recognize(data, "wav", 16000).ToString();
            int index = result.IndexOf("result");
            string msg = null;
            if (index != -1)
            {
                string[] msgList = result.Split(',');
                foreach (var item in msgList)
                {
                    index = item.IndexOf("result");
                    if (index != -1)
                    {
                        string[] msgs = item.Split('"');
                        msg = msgs[msgs.Length - 2];
                        //Console.WriteLine(msg);
                    }
                }
            }
            else
            {
                string[] msgList = result.Split(',');
                foreach (var item in msgList)
                {
                    index = item.IndexOf("err_msg");
                    if (index != -1)
                    {
                        string[] msgs = item.Split('"');
                        msg = msgs[msgs.Length - 2];
                        //Console.WriteLine(msg);
                    }
                }
            }
            return msg;
        }
    }
}
