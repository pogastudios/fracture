using Fracture.Client.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fracture.Client.Helpers
{
    public static class FirstRunHelper
    {

        public static async Task RunFirstTimeSetup()
        {
            EnsureCurDir("python");

            var proc = Process.Start("python", "get-pip.py");
            await proc.WaitForExitAsync();

            proc.Dispose();
            GlobalAppData.IsCurDirSet = false;

            EnsureCurDir();

            proc = Process.Start("pip", "install -U demucs PySoundFile");
            await proc.WaitForExitAsync();

            proc.Dispose();

            proc = Process.Start("pip", "install ffmpeg");
            await proc.WaitForExitAsync();

            proc.Dispose();
        }

        public static void EnsureCurDir(string location = @"python\Scripts")
        {
            if(!GlobalAppData.IsCurDirSet)
            {
                Directory.SetCurrentDirectory(Path.Combine(GlobalAppData.InitCurrentDir, location));
                GlobalAppData.IsCurDirSet = true;
            }
        }
    }
}
