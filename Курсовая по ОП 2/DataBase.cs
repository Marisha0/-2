using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Windows.Forms;

namespace Курсовая_по_ОП_2
{
    public class Database
    {
        private readonly string dataSource;
        private SQLiteConnection conn;

        private static string GenMD5Hash(string user0, string password0)
        { //получение хэша пароля
            byte[] passwordBytes = Encoding.UTF8.GetBytes(user0.ToUpper() + password0);
            byte[] res = new MD5CryptoServiceProvider().ComputeHash(passwordBytes);
            return Convert.ToBase64String(res);
        }

        Hash Hash = new Hash();
        public Database(string dataSource)
        {
            this.dataSource = dataSource;
            conn = new SQLiteConnection(dataSource);
        }
        public bool InitializeDatabase()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                SQLiteCommand cmd = conn.CreateCommand();
                string sql_command = "CREATE TABLE IF NOT EXISTS users("
                + "id INTEGER PRIMARY KEY AUTOINCREMENT, "
                + "login TEXT, "
                + "password TEXT, "
                + "role TEXT)";
                cmd.CommandText = sql_command;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }


        public bool CreateUser(string username, string password)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = string.Format("INSERT INTO users (login, password)"
                + " VALUES ('{0}', '{1}')",
                username.ToUpper(), GenMD5Hash(username, password));
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }


        public bool CheckUser(string username)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = string.Format("SELECT login, password, role"
                    + " FROM users"
                    + " where login = '{0}'",
                    username.ToUpper());

                return cmd.ExecuteScalar() != null;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        public bool UserAuth(string username, string password)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = string.Format("SELECT login, password, role"
                + " FROM users"
                + " where login = '{0}' AND"
                + " password = '{1}'",
                username.ToUpper(), GenMD5Hash(username, password));
                return cmd.ExecuteScalar() != null;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        public void CloseDB()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }
}