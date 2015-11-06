using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace MayTinh {
    public class item {
        public string ID { get; set; }
        public string phepTinh { get; set; }
        public string ketQua { get; set; }
        public string thoiGian { get; set; }
    }
    class DataBaseHistory {
        SQLiteConnection connect;
        public DataBaseHistory() {
            if (!System.IO.File.Exists(@"History.sqlite")) {
                SQLiteConnection.CreateFile(@"History.sqlite");
            }
            connect =
                new SQLiteConnection(@"Data Source=History.sqlite;Version=3;");
            connect.Open();
            
            string sql = @"create table if not exists history(
                            ID INTEGER PRIMARY KEY AUTOINCREMENT,
                            phepTinh nvarchar(500),
                            ketQua nvarchar(20),
                            thoiGian nvarchar(100)
                            )";
            SQLiteCommand command = new SQLiteCommand(sql, connect);
            command.ExecuteNonQuery();
        }
        public void insertOneRow(string phepTinh, string ketQua) {
            string time = DateTime.Now.ToString();
            string sql = 
                string.Format("insert into history(phepTinh,ketQua,thoiGian) values(\"{0}\",\"{1}\",\"{2}\")", phepTinh, ketQua, time);
            SQLiteCommand command = new SQLiteCommand(sql, connect);
            command.ExecuteNonQuery();
        }
        public List<item> showHistory() {
            string sql = @"select * from history
                            order by ID desc";
            SQLiteCommand command = new SQLiteCommand(sql, connect);
            SQLiteDataReader reader = command.ExecuteReader();
            List<item> items = new List<item>();
            while (reader.Read()) {
                items.Add(new item() {
                    ID = reader["ID"].ToString(),
                    phepTinh = reader["phepTinh"].ToString(),
                    ketQua = reader["ketQua"].ToString(),
                    thoiGian = reader["thoiGian"].ToString()
                });
            }
            return items;
        }
        public void deleteAllHistory() {
            string sql = @"delete from history";
            SQLiteCommand command = new SQLiteCommand(sql, connect);
            command.ExecuteNonQuery();
        }
    }
}
