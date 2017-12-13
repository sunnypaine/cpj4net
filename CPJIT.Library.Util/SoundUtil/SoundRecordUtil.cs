using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace CPJIT.Library.Util.SoundUtil
{
    /// <summary>
    /// 录音工具。该工具借用win32 API实现，如果要录制高品质音频，请使用其它方式，例如Microsoft DirectX。
    /// </summary>
    public class SoundRecordUtil
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendString", CharSet = CharSet.Auto)]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        public static void StartRecord()
        {
            mciSendString("set wave bitpersample 8", "", 0, 0);

            mciSendString("set wave samplespersec 20000", "", 0, 0);
            mciSendString("set wave channels 2", "", 0, 0);
            mciSendString("set wave format tag pcm", "", 0, 0);
            mciSendString("open new type WAVEAudio alias movie", "", 0, 0);

            mciSendString("record movie", "", 0, 0);
        }

        public static void StopRecord(string filePath)
        {
            mciSendString("stop movie", "", 0, 0);
            mciSendString("save movie " + filePath, "", 0, 0);
            mciSendString("close movie", "", 0, 0);
        }
    }
}
