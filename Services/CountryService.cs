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
    public class CountryService
    {
        public ObservableCollection<Countries> ListCountries()
        {
            ObservableCollection<Countries> countriesCollection = new ObservableCollection<Countries>();
            DataTable dataTable = new DataTable();
            MySqlConnection connection = null;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
                connection = new MySqlConnection(connectionString);
                connection.Open();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT * FROM countries WHERE ENABLED_FLAG = 1 ORDER BY CNTRY_NUM;");
                MySqlCommand command = new MySqlCommand(sql.ToString(), connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.FieldCount > 0)
                    {
                        dataTable.Load(reader);
                        foreach (DataRow item in dataTable.Rows)
                        {
                            Countries country = new Countries
                            {
                                Cntry_Id = item["CNTRY_ID"].Nvl(0),
                                Cntry_Cat_Id = item["CNTRY_CAT_ID"].Nvl(0),
                                Cntry_Num = item["CNTRY_NUM"].Nvl(0),
                                Cntry_Name_A = item["CNTRY_NAME_A"].Nvl(""),
                                Cntry_Name_E = item["CNTRY_NAME_E"].Nvl(""),
                                Cntry_Nat_A = item["CNTRY_NAT_A"].Nvl(""),
                                Cntry_Nat_E = item["CNTRY_NAT_E"].Nvl("")
                            };
                            countriesCollection.Add(country);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw; // Consider logging or handling the exception
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return countriesCollection;
        }
    }
}
