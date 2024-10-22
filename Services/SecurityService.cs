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
    public class SecurityService
    {
        public IEnumerable<Security> ListSecurity()
        {
            ObservableCollection<Security> xReVal = new ObservableCollection<Security>();
            DataTable xDT = new DataTable();
            MySqlConnection xConn = null;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
                xConn = new MySqlConnection(connectionString);
                xConn.Open();

                StringBuilder xSql = new StringBuilder();
                xSql.Append("SELECT * FROM security ORDER BY symbol;");
                MySqlCommand xCmd = new MySqlCommand(xSql.ToString(), xConn);
                using (MySqlDataReader xRdr = xCmd.ExecuteReader())
                {
                    if (xRdr.FieldCount > 0)
                    {
                        xDT.Load(xRdr);
                        foreach (DataRow item in xDT.Rows)
                        {
                            Security xSecurity = new Security
                            {
                                Code = item["code"].Nvl(""),
                                Arabic_Name = item["arabic_name"].Nvl(""),
                                English_Name = item["english_name"].Nvl(""),
                                Symbol = item["symbol"].Nvl(""),
                                Group = item["group"].Nvl(0),
                                Sector = item["sector"].Nvl(""),
                                Close_Price = item["close_price"].Nvl(0),
                                Index_Sample = item["index_sample"].Nvl(0),
                                Isin_Code = item["isin_code"].Nvl(""),
                                Total_Shares = item["total_shares"].Nvl(0),
                                Factor_100 = item["factor_100"].Nvl(0),
                                General_Base_Index = item["general_base_index"].Nvl(0)








                            };
                            xReVal.Add(xSecurity);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Handle exception as needed
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
