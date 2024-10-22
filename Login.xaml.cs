using DataVirtualization.Modules;
using DataVirtualization.Utilies;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Shapes;

namespace DataVirtualization
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the username and password from the input fields
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;



            IsUserAuthorised(UsernameTextBox.Text.Nvl(""), CryptorEngine.Encrypt(PasswordBox.Password.Nvl(""), true));
            // Simple validation logic
            if (IsUserAuthorised(UsernameTextBox.Text.Nvl(""), CryptorEngine.Encrypt(PasswordBox.Password.Nvl(""), true))!=0)
            {


                // Open a new window or navigate to the next screen
                HomeWindow mainWindow = new HomeWindow();
                mainWindow.Show();
                this.Close(); // Close the login window
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public int IsUserAuthorised(string vUserName, string vPassword)
        {
            int xReVal = 0;
            DataTable xDT = new DataTable();
            MySqlConnection xConn = null;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

                xConn = new MySqlConnection(connectionString);
                xConn.Open();

                StringBuilder xSql = new StringBuilder();

                xSql.AppendFormat(" SET sql_mode=(SELECT REPLACE(@@sql_mode,'ONLY_FULL_GROUP_BY','')); SET GLOBAL sql_mode=(SELECT REPLACE(@@sql_mode,'ONLY_FULL_GROUP_BY',''));");

                MySqlCommand xCmd = new MySqlCommand(xSql.ToString(), xConn);
                xCmd.ExecuteNonQuery();



                xSql.Clear();
                xSql.Append("SELECT id, last_modified_date FROM user WHERE user_name = @UserName AND password = @Password;");
                xCmd = new MySqlCommand(xSql.ToString(), xConn);
                xCmd.Parameters.AddWithValue("@UserName", vUserName);
                xCmd.Parameters.AddWithValue("@Password", vPassword);








                using (MySqlDataReader xRdr = xCmd.ExecuteReader())
                {
                    DataSet xDataSet = new DataSet();
                    xDataSet.Tables.Add(xDT);
                    xDataSet.EnforceConstraints = false;
                    xDT.Load(xRdr);

                    foreach (DataRow item in xDT.Rows)
                    {
                        xReVal = item["id"].Nvl(0);
                
                    }


                    xSql.Clear();
                    if (xReVal != 0)
                    {
                        //Autharized
                        xSql.Append(" INSERT INTO user_log");
                        xSql.Append(" VALUES (");
                        xSql.Append(" 0,");
                        xSql.AppendFormat(" {0},", xReVal);
                        xSql.AppendFormat(" {0},", DateTime.Now.ToString("yyyyMMdd"));
                        xSql.AppendFormat(" {0},", DateTime.Now.ToString("HHmmss"));
                        xSql.AppendFormat(" '{0}')", "Autharized");
                        xCmd = new MySqlCommand(xSql.ToString(), xConn);
                        bool xIsLogSave = false;
                        using (MySqlDataReader xRdr2 = xCmd.ExecuteReader())
                        {
                            if (xRdr2.RecordsAffected > 0)
                            {
                                xIsLogSave = true;
                               
                            }
                        }
                    }
                    else
                    {
                        //Not Autharized
                        xSql.Append(" INSERT INTO user_log");
                        xSql.Append(" VALUES (");
                        xSql.Append(" 0,");
                        xSql.AppendFormat(" {0},", xReVal);
                        xSql.AppendFormat(" {0},", DateTime.Now.ToString("yyyyMMdd"));
                        xSql.AppendFormat(" {0},", DateTime.Now.ToString("HHmmss"));
                        xSql.AppendFormat(" 'Not Autharized and UserName is {0} and password is {1}')", vUserName, vPassword);
                        xCmd = new MySqlCommand(xSql.ToString(), xConn);
                        bool xIsLogSave = false;
                        using (MySqlDataReader xRdr2 = xCmd.ExecuteReader())
                        {
                            if (xRdr2.RecordsAffected > 0)
                            {
                                xIsLogSave = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (xConn != null && xConn.State == ConnectionState.Open)
                {
                    xConn.Close();
                }
            }
            return xReVal;
        }
        //**************************************************************
    }
}
