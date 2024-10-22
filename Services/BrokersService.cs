using DataVirtualization.Modules;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVirtualization.Services
{
    public class BrokersService
    {
        public IEnumerable<Broker> ListBrokers()
        {
            ObservableCollection<Broker> xReVal = new ObservableCollection<Broker>();
            DataTable xDT = new DataTable();
            MySqlConnection xConn = null;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
                xConn = new MySqlConnection(connectionString);
                xConn.Open();

                StringBuilder xSql = new StringBuilder();
                xSql.Append("SELECT * FROM broker;");
                MySqlCommand xCmd = new MySqlCommand(xSql.ToString(), xConn);
                using (MySqlDataReader xRdr = xCmd.ExecuteReader())
                {
                    if (xRdr.FieldCount > 0)
                    {
                        xDT.Load(xRdr);
                        foreach (DataRow item in xDT.Rows)
                        {
                            var xBroker = new Broker
                            {
                                Code = item["code"].Nvl(""),
                                Arabic_Name = item["arabic_name"].Nvl(""),
                                English_Name = item["english_name"].Nvl(""),
                                Market_Cap = item["market_cap"].Nvl(0)
                            };
                            xReVal.Add(xBroker);
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


    }
}
