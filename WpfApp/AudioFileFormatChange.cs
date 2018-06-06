using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class AudioFileFormatChange
    {
        public static string Mp3ToWav(string filePath)
        {
            Mp3FileReader reader = new Mp3FileReader(filePath);
            filePath = filePath.Substring(0, filePath.Length - 4) + ".wav";
            Console.WriteLine(filePath);
            WaveFileWriter.CreateWaveFile(filePath, reader);
            reader.Close();
            return filePath;
        }
    }
}
