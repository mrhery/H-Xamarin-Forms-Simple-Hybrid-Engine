using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HXFSimpleHybrid
{
    public static class Config
    {
        public static string port = "45100";

        public static class DB
        {
            public static string File = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "hxfhybrid.db.db3");
            public static SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.SharedCache;
        }

        public static class Permissions
        {
            public static bool Camera       = false;
            public static bool Microphone   = false;
        }
    }
}
