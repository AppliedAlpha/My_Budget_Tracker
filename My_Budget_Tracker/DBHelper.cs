using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace My_Budget_Tracker
{
    public class DBHelper
    {
        // Connection Object
        private static MySqlConnection conn = null;
        public static string DBConnString {
            get;
            private set;
        }
        public static bool bDBConnCheck = false;
        private static int errorBoxCount = 0;

        // Constructor
        public DBHelper() { }
        public static MySqlConnection DBConn
        {
            get
            {
                if (!bDBConnCheck) return null;
                return conn;
            }
        }

        // DB Connection
        public static bool ConnectToDB(String pwd)
        {
            if (conn == null)
            {
                // args: Server Name, DB Name, Auth
                DBConnString = String.Format("server={0};uid={1};pwd={2};database={3};charset=utf8 ;", "localhost", "aria", pwd, "budget_tracker");
                conn = new MySqlConnection(DBConnString);
            }
            try
            {
                if (!IsDBConnected)
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open) bDBConnCheck = true;
                    else
                    {
                        conn = null;
                        bDBConnCheck = false;
                    }
                }
            }
            catch (MySqlException mse)
            {
                errorBoxCount++;
                if (errorBoxCount == 1) MessageBox.Show(mse.Message, "DBHelper - ConnectToDB()");
                return false;
            }
            return true;
        }

        // Check DB Open State
        public static bool IsDBConnected
        {
            get
            {
                if (conn.State != System.Data.ConnectionState.Open) return false;
                else return true;
            }
        }

        // DB Closing
        public static void Close()
        {
            if (IsDBConnected) DBConn.Close();
        }
    }
}
