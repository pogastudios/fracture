using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fracture.Client.Data
{
    public static class GlobalAppData
    {
        public static string InitCurrentDir { get; set; }

        public static bool IsSetupComplete
        {
            get => File.Exists("firstRun.info");
            set => File.WriteAllText("firstRun.info", "true");
        }
        public static bool IsCurDirSet { get; set; }
    }
}
