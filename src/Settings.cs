using System;

namespace Centipede
{
    static class Settings
    {
        private static readonly string exportDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/stuff/ld/33/";

        public static class Screenshot
        {
            public const bool SaveAsPng = false;

            public static readonly string Path = exportDir + "screenshots/";
            public const string NameBase = "centipede_";
        }
    }
}