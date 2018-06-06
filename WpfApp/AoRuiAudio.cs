using Oraycn.MCapture;
using Oraycn.MFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class AoRuiAudio
    {
        const int PLAYSYNC = 1;
        const int PLAYLOOP = 2;

        private IMicrophoneCapturer microphoneCapturer;
        private AudioFileMaker videoFileMaker;

        public AoRuiAudio()
        {
            //MCapture、MFile授权
            Oraycn.MCapture.GlobalUtil.SetAuthorizedUser("FreeUser", "");
            Oraycn.MFile.GlobalUtil.SetAuthorizedUser("FreeUser", "");
            ////初始化语音识别
            //SpeechRecognition = new Baidu.Aip.Speech.Asr(API_KEY, SECRET_KEY);
            ////初始化语音合成
            //SpeechSynthesis = new Baidu.Aip.Speech.Tts(API_KEY, SECRET_KEY);
        }

        /// <summary>
        /// 播放wav文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="PlayMode">播放模式，默认为异步播放，1为同步播放，2为循环播放</param>
        public void PlayWavSound(string filePath,int PlayMode = 0)
        {
            SoundPlayer soundPlayer = new SoundPlayer(filePath);
            if (PlayMode == PLAYSYNC)
            {
                soundPlayer.PlaySync();
            }
            else if(PlayMode == PLAYLOOP)
            {
                soundPlayer.PlayLooping();
            }
            else
            {
                soundPlayer.Play();
            }
        }


        /// <summary>
        /// 初始化声音收集器
        /// </summary>
        /// <param name="outFileName">输出MP3文件名</param>
        /// <param name="microphoneIndex">第几个麦克风，默认为第一个（0）</param>
        public void InitialCapturer(string outFileName, int microphoneIndex = 0)
        {
            microphoneCapturer = CapturerFactory.CreateMicrophoneCapturer(microphoneIndex);
            microphoneCapturer.AudioCaptured += new ESBasic.CbGeneric<byte[]>(
                    delegate (byte[] audioData)
                    {
                        //收集数据
                        videoFileMaker.AddAudioFrame(audioData);
                    }
                );
            // audioPlayer = PlayerFactory.CreateAudioPlayer(0, 16000, 1, 16, 2);

            if (videoFileMaker == null)
            {
                videoFileMaker = new AudioFileMaker();
                Console.WriteLine(outFileName);
                videoFileMaker.Initialize(outFileName, 16000, 1);//初始化
            }
        }

        /// <summary>
        /// 打开麦克风，开始录音
        /// </summary>
        public void OpenMircoPhone()
        {
            microphoneCapturer.Start();
        }

        /// <summary>
        /// 关闭麦克风，释放资源
        /// </summary>
        public void CloseMircoPhone()
        {
            if (microphoneCapturer != null)
            {
                microphoneCapturer.Stop();
                microphoneCapturer.Dispose();
                microphoneCapturer = null;
            }
            if (videoFileMaker != null)
            {
                videoFileMaker.Close(true);
                videoFileMaker.Dispose();
                videoFileMaker = null;
            }
        }
    }
}
