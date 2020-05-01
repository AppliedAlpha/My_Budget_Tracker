using MySql.Data.MySqlClient;
using System;
using System.Collections;
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
        private DataTable dataTable = new DataTable();
        private static MySqlConnection conn = null;
        public static String DBConnString;
        public static bool bDBConnCheck = false;
        public IEnumerable<String> in_items = new String[] { "월급", "용돈", "이자", "잔액조정", "기타" };
        public IEnumerable<String> out_items = new String[] { "식비", "주거/통신", "생활용품", "의복/미용", "건강/문화", "교육", "교통", "회비", "이자", "저축", "기타" };

        public MainWindow()
        {
            InitializeComponent();
            login.Click += _login;
            _in.Click += inoutClick;
            _out.Click += inoutClick;
            add.Click += _add;
            delete.Click += _delete;
        }

        private void _add(object sender, RoutedEventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    int total = 0;
                    MySqlDataAdapter adapter = new MySqlDataAdapter("INSERT INTO `budget_tracker`.`budget_form` " +
                        "(`record_date`, `price`, `description`, `usage`, `total`) VALUES " +
                        "('" + DateTime.Today.ToString("d") + "', '" + price.Text + "', '" + description.Text + "', '" +
                        type_detail.Text + "', '" + total + "');", conn);
                    try
                    {
                        adapter.Fill(dataTable);
                        dataGrid.ItemsSource = dataTable.DefaultView;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void _delete(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void inoutClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_in.IsChecked.Value)
                {
                    type_detail.ItemsSource = in_items;
                }
                else type_detail.ItemsSource = out_items;
            }
            catch (InvalidOperationException ioe)
            {
                MessageBox.Show(ioe.Message + "\n값이 제대로 할당되지 않았습니다.");
            }
            catch (NotImplementedException nie)
            {
                MessageBox.Show(nie.Message + "\n아무 것도 안 고를 순 없습니다.");
            }
        }

        private void _login(Object sender, EventArgs e)
        {
            if (pwd.Text.Length == 0)
            {
                MessageBox.Show("패스워드를 입력해주세요.");
                return;
            }
            if (conn == null)
            {
                // args: Server Name, DB Name, Auth
                DBConnString = String.Format("server={0};uid={1};pwd={2};database={3};charset=utf8 ;", "localhost", "aria", pwd.Text, "budget_tracker");
                conn = new MySqlConnection(DBConnString);
            }
            try
            {
                conn.Open();
                bDBConnCheck = true;
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM budget_form ORDER BY id DESC LIMIT 1000", conn);
                try
                {
                    adapter.Fill(dataTable);
                    dataGrid.ItemsSource = dataTable.DefaultView;
                    pwd.Text = "";
                    MessageBox.Show("정상적으로 로그인되었습니다.", "MySQL Connection");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "DataGrid Load Error");
                    conn = null;
                }
            }
            catch (MySqlException mse)
            {
                MessageBox.Show(mse.Message, "DB Connection을 확인해주세요.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류가 발생했습니다.");
            }
        }
    }
}
