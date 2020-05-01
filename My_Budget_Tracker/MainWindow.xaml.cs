using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace My_Budget_Tracker
{
    public partial class MainWindow : Window
    {
        private DataTable dataTable = null;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                lock (DBHelper.DBConn)
                {
                    if (!DBHelper.IsDBConnected)
                    {
                        MessageBox.Show("DB 연결을 확인하세요.");
                        return;
                    }
                    else
                    {
                        // DB Connection Established
                        MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM budget_form", DBHelper.DBConn);
                        dataTable = new DataTable();
                        try
                        {
                            adapter.Fill(dataTable);
                            dataGrid.ItemsSource = dataTable.DefaultView;
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message, "DataGrid_Load Error");
                        }
                        DBHelper.Close();
                    }
                }
            }
            catch (ArgumentNullException ane)
            {
                MessageBox.Show(ane.Message, "DataGrid_Load Error");
            }
        }
    }
}
