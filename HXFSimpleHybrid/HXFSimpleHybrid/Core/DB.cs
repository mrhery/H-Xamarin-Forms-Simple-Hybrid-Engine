using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXFSimpleHybrid.Core
{
    public class DB
    {
        public DB(string _table)
        {
            table = _table;
        }

        static SQLiteConnection _instance;
        static string table;
        public static bool started = false;

        public static SQLiteConnection Init()
        {
            if (!started)
            {
                _instance = new SQLiteConnection(Config.DB.File, Config.DB.Flags);
                // _instance.DropTable<Tables.Users>();
                //_instance.CreateTable<Tables.Users>();

                started = true;
            }

            return _instance;
        }

        public static SQLiteConnection Conn(string _table = "")
        {
            if (!string.IsNullOrEmpty(_table))
            {
                table = _table;
            }

            if (started)
            {
                return _instance;
            }
            else
            {
                throw new Exception("Letak DB.Init() kat App.xaml.cs laaa badolll.");
            }
        }

        public List<dynamic> GetBy(Dictionary<string, string> ps = null)
        {
            if (ps == null)
            {
                throw new Exception("DB GetBy need parameter in GetBy mehtod ()");
            }
            else
            {
                var db = Init();

                string sql = "SELECT * FROM " + table + " WHERE ";
                string sqlwhere = "";
                foreach (var p in ps)
                {
                    if (string.IsNullOrEmpty(sqlwhere))
                    {
                        sqlwhere += " AND ";
                    }

                    sqlwhere += p.Key + @"='" + p.Value + @"'";
                }

                sql += sqlwhere;

                return db.Query<dynamic>(sql);
            }
        }
    }
}
