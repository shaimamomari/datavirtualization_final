


using DataVirtualization;
using DataVirtualization.Modules;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Windows;
using System.Windows.Shapes;

namespace DataVirtualization
{
    /// <summary>
    /// Demo implementation of IItemsProvider returning dummy customer items after
    /// a pause to simulate network/disk latency.
    /// </summary>
    public class DemoCustomerProvider : IItemsProvider<Customer>, INotifyPropertyChanged
    {
        public static FetchRangeResult shared = new FetchRangeResult();

        private int _countRec;
        public int Count
        {
            get => _countRec;
            set
            {
                if (_countRec != value)
                {
                    _countRec = value;
                    OnPropertyChanged(nameof(Count));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));











        private readonly int _count;
        private readonly int _fetchDelay;

        /// <summary>
        /// Initializes a new instance of the <see cref="DemoCustomerProvider"/> class.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="fetchDelay">The fetch delay.</param>
        public DemoCustomerProvider(int count, int fetchDelay)
        {
            _count = count;
            _fetchDelay = fetchDelay;
        }

        /// <summary>
        /// Fetches the total number of items available.
        /// </summary>
        /// <returns></returns>
        public int FetchCount()
        {
            Trace.WriteLine("FetchCount");
            Thread.Sleep(_fetchDelay);
            return _count;
        }

        /// <summary>
        /// Fetches a range of items.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The number of items to fetch.</param>
        /// <returns></returns>
        public FetchRangeResult FetchRange(int startIndex, int count, ActionsFilter filter)
        {
         

            var data = new List<ActionAccount>();
            var data2 = new List<ActionAccount>();
            int recordCount = 0;
            Trace.WriteLine("FetchRange: " + startIndex + "," + count);
            Thread.Sleep(_fetchDelay);

            List<Customer> list = new List<Customer>();
            List<Customer> list2 = new List<Customer>();
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;


            if (filter.isHistory)
            {

                using (MySqlConnection xConn = new MySqlConnection(connectionString))
                {
                    // Open the connection
                    xConn.Open();

                    string query = @"
    SELECT * FROM 
    ( 
        SELECT
            `actions`.`record_num` AS `record_num`,
            `actions`.`order_id` AS `order_id`,
            `actions`.`client_id` AS `client_id`,
            `actions`.`index` AS `index`,
            `actions`.`broker_code` AS `broker_code`,
            `actions`.`broker_name` AS `broker_name`,
            `actions`.`station_id` AS `station_id`,
            `actions`.`origin` AS `origin`,
            `actions`.`order_type` AS `order_type`,
            `actions`.`order_type_desc` AS `order_type_desc`,
            `actions`.`disclosed_qty` AS `disclosed_qty`,
            `actions`.`ok_date` AS `ok_date`,
            `actions`.`ok_time` AS `ok_time`,
            `actions`.`security_code` AS `security_code`,
            `actions`.`security_symbol` AS `security_symbol`,
            `actions`.`security_name` AS `security_name`,
            `actions`.`side` AS `side`,
            `actions`.`order_class` AS `order_class`,
            `actions`.`status` AS `status`,
            `actions`.`entered_price` AS `entered_price`,
            `actions`.`entered_qty` AS `entered_qty`,
            `actions`.`min_qty` AS `min_qty`,
            `actions`.`disp_qty` AS `disp_qty`,
            `actions`.`validity` AS `validity`,
            `actions`.`validity_date` AS `validity_date`,
            `actions`.`validity_time` AS `validity_time`,
            `actions`.`client` AS `client`,
            `actions`.`client_name` AS `client_name`,
            `actions`.`s_client` AS `s_client`,
            `actions`.`s_client_name` AS `s_client_name`,
            `actions`.`entry_date` AS `entry_date`,
            `actions`.`entry_time` AS `entry_time`,
            `actions`.`exe_price` AS `exe_price`,
            `actions`.`exe_qty` AS `exe_qty`,
            `actions`.`rem_qty` AS `rem_qty`,
            `actions`.`exe_time` AS `exe_time`,
            `actions`.`ctby` AS `ctby`,
            `actions`.`reject_cat` AS `reject_cat`,
            `actions`.`reject_code` AS `reject_code`,
            `actions`.`ACC_BROKER_NO` AS `ACC_BROKER_NO`,
            `actions`.`ACC_NUM` AS `ACC_NUM`,
            `actions`.`ACC_TYPE_ID` AS `ACC_TYPE_ID`,
            `actions`.`ACC_S_ID` AS `ACC_S_ID`,
            `actions`.`ACC_INVESTOR_NO` AS `ACC_INVESTOR_NO`,
            `actions`.`ACC_INVESTOR_NAME` AS `ACC_INVESTOR_NAME`,
            `actions`.`INVS_S_ID` AS `INVS_S_ID`,
            `actions`.`CNTRY_ID` AS `CNTRY_ID`,
            `actions`.`ACC_S_NUM` AS `ACC_S_NUM`,
            `actions`.`ACC_S_NAME_A` AS `ACC_S_NAME_A`,
            `actions`.`ACC_S_NAME_E` AS `ACC_S_NAME_E`,
            `actions`.`ACC_S_CODE` AS `ACC_S_CODE`,
            `actions`.`INVS_S_NUM` AS `INVS_S_NUM`,
            `actions`.`INVS_S_NAME_A` AS `INVS_S_NAME_A`,
            `actions`.`INVS_S_NAME_E` AS `INVS_S_NAME_E`,
            `actions`.`INVS_S_CODE` AS `INVS_S_CODE`,
            `actions`.`ACC_TYPE_NUM` AS `ACC_TYPE_NUM`,
            `actions`.`ACC_TYPE_NAME_A` AS `ACC_TYPE_NAME_A`,
            `actions`.`ACC_TYPE_NAME_E` AS `ACC_TYPE_NAME_E`,
            `actions`.`ACC_TYPE_CODE` AS `ACC_TYPE_CODE`,
            `actions`.`CNTRY_NUM` AS `CNTRY_NUM`,
            `actions`.`CNTRY_NAME_A` AS `CNTRY_NAME_A`,
            `actions`.`CNTRY_NAME_E` AS `CNTRY_NAME_E`,
            `actions`.`CNTRY_NAT_A` AS `CNTRY_NAT_A`,
            `actions`.`CNTRY_NAT_E` AS `CNTRY_NAT_E`,
            `actions`.`source_file_date` AS `source_file_date`,
            `actions`.`GROUP_ID` AS `GROUP_ID`,
            `actions`.`GROUP_NUM` AS `GROUP_NUM`,
            `actions`.`GROUP_NAME_A` AS `GROUP_NAME_A`,
            `actions`.`GROUP_NAME_E` AS `GROUP_NAME_E`,
            `actions`.`sector_id` AS `sector_id`,
            `actions`.`sector_num` AS `sector_num`,
            `actions`.`sector_name_ar` AS `sector_name_ar`,
            `actions`.`sector_name_en` AS `sector_name_en`,
            `actions`.`main_sector_id` AS `main_sector_id`,
            `actions`.`main_sector_num` AS `main_sector_num`,
            `actions`.`main_sector_name_ar` AS `main_sector_name_ar`,
            `actions`.`main_sector_name_en` AS `main_sector_name_en`,
            `actions`.`INVS_TYPE_ID` AS `INVS_TYPE_ID`,
            `actions`.`INVS_TYPE_NUM` AS `INVS_TYPE_NUM`,
            `actions`.`INVS_TYPE_NAME_A` AS `INVS_TYPE_NAME_A`,
            `actions`.`INVS_TYPE_NAME_E` AS `INVS_TYPE_NAME_E`,
            `actions`.`INVS_CAT_ID` AS `INVS_CAT_ID`,
            `actions`.`INVS_CAT_NUM` AS `INVS_CAT_NUM`,
            `actions`.`INVS_CAT_NAME_A` AS `INVS_CAT_NAME_A`,
            `actions`.`INVS_CAT_NAME_E` AS `INVS_CAT_NAME_E`,
            `total_exe_qty_v`.`total_exe_qty` AS `total_exe_qty` 
        FROM ((( 
            SELECT * 
            FROM `actions_history_tmp_v` 
            WHERE(
                `actions_history_tmp_v`.`source_file_date` >= @startDate 
                AND `actions_history_tmp_v`.`source_file_date` <= @endDate
                AND `actions_history_tmp_v`.`order_id` > ''
            )
        )) `actions`
        LEFT JOIN `total_exe_qty_v` 
        ON `actions`.`order_id` = `total_exe_qty_v`.`order_id`
    )) t1 
    ";

                    // Append filtering conditions
                    bool hasFilterConditions = false;
                    if (filter.source_file_date_to_FilterText == "" || filter.source_file_date_to_FilterText == null) filter.source_file_date_to_FilterText = "20700319";
                    if (filter.source_file_date_from_FilterText == "" || filter.source_file_date_from_FilterText == null) filter.source_file_date_from_FilterText = "20000319";

                    if (!string.IsNullOrEmpty(filter.Name) || !string.IsNullOrEmpty(filter.Name) || !string.IsNullOrEmpty(filter.entry_date_from_FilterText) || !string.IsNullOrEmpty(filter.entry_date_to_FilterText))
                    {
                        query += " WHERE ";

                        if (!string.IsNullOrEmpty(filter.Name))
                        {
                            query += " order_id = @filterText ";
                            hasFilterConditions = true;
                        }

                        if (!string.IsNullOrEmpty(filter.status))
                        {
                            if (hasFilterConditions) query += " AND ";
                            query += " status = @filterStatus ";
                        }


                        if (!string.IsNullOrEmpty(filter.entry_date_from_FilterText))
                        {
                            if (hasFilterConditions) query += " AND ";
                            query += " entry_date >= @entry_date_from_FilterText ";
                        }



                        if (!string.IsNullOrEmpty(filter.entry_date_to_FilterText))
                        {
                            if (hasFilterConditions) query += " AND ";
                            query += " entry_date <= @entry_date_to_FilterText ";
                        }


                    }

                    // Append LIMIT and OFFSET if no filters
                    if (!hasFilterConditions)
                    {
                        query += " group by order_id LIMIT @count OFFSET @startIndex;";
                    }

                    // If filters are applied, add LIMIT and OFFSET
                    if (hasFilterConditions)
                    {
                        query += " group by order_id LIMIT @count OFFSET @startIndex;";
                    }




                    MySqlCommand xCmd = new MySqlCommand(query, xConn);

                    xCmd.Parameters.AddWithValue("@startDate", filter.source_file_date_from_FilterText);




                    xCmd.Parameters.AddWithValue("@endDate", filter.source_file_date_to_FilterText);
                    xCmd.Parameters.AddWithValue("@count", count); // count should be a variable
                    xCmd.Parameters.AddWithValue("@startIndex", startIndex); // startIndex should be a variable
                    xCmd.Parameters.AddWithValue("@filterText", filter.Name); // startIndex should be a variable
                    xCmd.Parameters.AddWithValue("@filterStatus", filter.status); // startIndex should be a variable
                    xCmd.Parameters.AddWithValue("@entry_date_from_FilterText", filter.entry_date_from_FilterText); // startIndex should be a variable
                    xCmd.Parameters.AddWithValue("@entry_date_to_FilterText", filter.entry_date_to_FilterText); // startIndex should be a variable



                    using (MySqlDataReader xRdr = xCmd.ExecuteReader())
                    {
                        for (int i = startIndex; i < startIndex + count; i++)
                        {
                            ActionAccount account = new ActionAccount(); // Initialize the ActionAccount object
                            if (xRdr.Read())
                            {
                                account.record_num = xRdr.GetInt32("record_num").ToString();
                                account.order_id = xRdr.GetString("order_id");
                                account.entry_date = xRdr.GetString("entry_date");



                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("client_id")))
                                {
                                    account.Client_id = xRdr.GetString("client_id");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.Client_id = null; // or set a default value
                                }




                                account.status = xRdr.GetString("status");
                            

                                var validityDateColumn = xRdr["validity_date"];

                                account.validity_date = (validityDateColumn != DBNull.Value) ? xRdr.GetString(xRdr.GetOrdinal("validity_date")) : null;
                                account.broker_code = xRdr.GetString("broker_code");



                               // account.index = xRdr.GetInt32("index").ToString();
                                var indexColumn = xRdr["index"];

                                account.index = (indexColumn != DBNull.Value) ? xRdr.GetInt32(xRdr.GetOrdinal("index")).ToString() : "null";


                                account.broker_name = xRdr.GetString("broker_name");
                                



                                var stationIdColumn = xRdr["station_id"];

                                account.station_id = (stationIdColumn != DBNull.Value) ? xRdr.GetString(xRdr.GetOrdinal("station_id")) : null;




                                account.security_code = xRdr.GetString("security_code");
                                account.security_name = xRdr.GetString("security_name");
                                account.side = xRdr.GetString("side");
                                account.order_class = xRdr.GetString("order_class");
                                account.entered_price = xRdr.GetDouble("entered_price").ToString();
                                account.entered_qty = xRdr.GetInt32("entered_qty").ToString();
                                //account.min_qty = xRdr.GetInt32("min_qty").ToString();


                                var minQtyColumn = xRdr["min_qty"];

                                account.min_qty = (minQtyColumn != DBNull.Value) ? xRdr.GetInt32(xRdr.GetOrdinal("min_qty")).ToString() : "null";




                                account.disp_qty = xRdr.GetInt32("disp_qty").ToString();
                                account.validity = xRdr.GetString("validity");
                                object validityTime = xRdr.GetValue(xRdr.GetOrdinal("validity_time"));
                                account.validity_time = validityTime != DBNull.Value ? (string)validityTime : null;




                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("ACC_NUM")))
                                {
                                    account.ACC_NUM = xRdr.GetInt32("ACC_NUM").ToString();
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.ACC_NUM = null; // or set a default value
                                }



                                account.client_name = xRdr.GetString("client_name");




                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("CNTRY_NAT_A")))
                                {
                                    account.CNTRY_NAT_A = xRdr.GetString("CNTRY_NAT_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.CNTRY_NAT_A = null; // or set a default value
                                }







                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("ACC_INVESTOR_NO")))
                                {
                                    account.ACC_INVESTOR_NO = xRdr.GetString("ACC_INVESTOR_NO");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.ACC_INVESTOR_NO = null; // or set a default value
                                }







                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("Acc_S_Name_A")))
                                {
                                    account.account_state = xRdr.GetString("Acc_S_Name_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.account_state = null; // or set a default value
                                }









                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("GROUP_NAME_A")))
                                {
                                    account.GROUP_NAME_A = xRdr.GetString("GROUP_NAME_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.GROUP_NAME_A = null; // or set a default value
                                }





                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("main_sector_name_ar")))
                                {
                                    account.main_sector_name_ar = xRdr.GetString("main_sector_name_ar");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.main_sector_name_ar = null; // or set a default value
                                }



                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("sector_name_ar")))
                                {
                                    account.sector_name_ar = xRdr.GetString("sector_name_ar");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.sector_name_ar = null; // or set a default value
                                }








                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("Invs_Type_Name_A")))
                                {
                                    account.investor_type = xRdr.GetString("Invs_Type_Name_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.investor_type = null; // or set a default value
                                }





                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("Invs_Cat_Name_A")))
                                {
                                    account.investor_cat = xRdr.GetString("Invs_Cat_Name_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.investor_cat = null; // or set a default value
                                }




                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("Invs_S_Name_A")))
                                {
                                    account.investor_state = xRdr.GetString("Invs_S_Name_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.investor_state = null; // or set a default value
                                }



                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("Acc_Type_Name_A")))
                                {
                                    account.account_type = xRdr.GetString("Acc_Type_Name_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.account_type = null; // or set a default value
                                }




                                


                                var entryTimeColumn = xRdr["entry_time"];

                                account.entry_time = (entryTimeColumn != DBNull.Value) ? xRdr.GetString(xRdr.GetOrdinal("entry_time")) : "null";



                                account.exe_price = xRdr.GetDouble("exe_price").ToString();
                                account.exe_qty = xRdr.GetInt32("exe_qty").ToString();

                                account.rem_qty = xRdr.GetInt32("rem_qty").ToString();
                                account.exe_time = xRdr.GetString("exe_time");
                                account.source_file_date = xRdr.GetInt32("source_file_date").ToString();
                                account.origin = xRdr.GetString("origin");
                                



                                var orderTypeDescColumn = xRdr["Order_Type_Desc"];

                                account.order_type = (orderTypeDescColumn != DBNull.Value) ? xRdr.GetString(xRdr.GetOrdinal("Order_Type_Desc")) : "null";



                                var disclosedQtyColumn = xRdr["disclosed_qty"];

                                account.disclosed_qty = (disclosedQtyColumn != DBNull.Value) ? xRdr.GetInt64(xRdr.GetOrdinal("disclosed_qty")).ToString() : "null";



                                


                                data.Add(account); // Add the populated account to the list
                            }

                            Customer customer = new Customer
                            {
                                Id = i + 1,
                                record_num = account.record_num,
                                Name = account.order_id,
                                entry_date = account.entry_date,
                                Client_id = account.Client_id,
                                status = account.status,
                                validity_date = account.validity_date,
                                broker_code = account.broker_code,

                                index = account.index,
                                broker_name = account.broker_name,
                                station_id = account.station_id,




                                security_code = account.security_code,
                                security_name = account.security_name,
                                side = account.side,
                                order_class = account.order_class,
                                entered_price = account.entered_price,
                                entered_qty = account.entered_qty,
                                min_qty = account.min_qty,
                                disp_qty = account.disp_qty,
                                validity = account.validity,
                                validity_time = account.validity_time,
                                ACC_NUM = account.ACC_NUM,
                                client_name = account.client_name,
                                CNTRY_NAT_A = account.CNTRY_NAT_A,
                                ACC_INVESTOR_NO = account.ACC_INVESTOR_NO,
                                account_state = account.account_state,
                                GROUP_NAME_A = account.GROUP_NAME_A,
                                main_sector_name_ar = account.main_sector_name_ar,
                                sector_name_ar = account.sector_name_ar,
                                investor_type = account.investor_type,
                                investor_cat = account.investor_cat,
                                investor_state = account.investor_state,
                                account_type = account.account_type,
                                entry_time = account.entry_time,
                                exe_price = account.exe_price,
                                exe_qty = account.exe_qty,
                                total_exe_qty_v = account.total_exe_qty_v,
                                rem_qty = account.rem_qty,
                                exe_time = account.exe_time,
                                source_file_date = account.source_file_date,
                                origin = account.origin,
                                order_type = account.order_type,
                                disclosed_qty = account.disclosed_qty




                            };
                            list.Add(customer);
                        }
                    }
                }



                using (MySqlConnection xConn = new MySqlConnection(connectionString))
                {
                    // Open the connection
                    xConn.Open();

                    string query = @"
    SELECT * FROM 
    ( 
        SELECT
            `actions`.`record_num` AS `record_num`,
            `actions`.`order_id` AS `order_id`,
            `actions`.`client_id` AS `client_id`,
            `actions`.`index` AS `index`,
            `actions`.`broker_code` AS `broker_code`,
            `actions`.`broker_name` AS `broker_name`,
            `actions`.`station_id` AS `station_id`,
            `actions`.`origin` AS `origin`,
            `actions`.`order_type` AS `order_type`,
            `actions`.`order_type_desc` AS `order_type_desc`,
            `actions`.`disclosed_qty` AS `disclosed_qty`,
            `actions`.`ok_date` AS `ok_date`,
            `actions`.`ok_time` AS `ok_time`,
            `actions`.`security_code` AS `security_code`,
            `actions`.`security_symbol` AS `security_symbol`,
            `actions`.`security_name` AS `security_name`,
            `actions`.`side` AS `side`,
            `actions`.`order_class` AS `order_class`,
            `actions`.`status` AS `status`,
            `actions`.`entered_price` AS `entered_price`,
            `actions`.`entered_qty` AS `entered_qty`,
            `actions`.`min_qty` AS `min_qty`,
            `actions`.`disp_qty` AS `disp_qty`,
            `actions`.`validity` AS `validity`,
            `actions`.`validity_date` AS `validity_date`,
            `actions`.`validity_time` AS `validity_time`,
            `actions`.`client` AS `client`,
            `actions`.`client_name` AS `client_name`,
            `actions`.`s_client` AS `s_client`,
            `actions`.`s_client_name` AS `s_client_name`,
            `actions`.`entry_date` AS `entry_date`,
            `actions`.`entry_time` AS `entry_time`,
            `actions`.`exe_price` AS `exe_price`,
            `actions`.`exe_qty` AS `exe_qty`,
            `actions`.`rem_qty` AS `rem_qty`,
            `actions`.`exe_time` AS `exe_time`,
            `actions`.`ctby` AS `ctby`,
            `actions`.`reject_cat` AS `reject_cat`,
            `actions`.`reject_code` AS `reject_code`,
            `actions`.`ACC_BROKER_NO` AS `ACC_BROKER_NO`,
            `actions`.`ACC_NUM` AS `ACC_NUM`,
            `actions`.`ACC_TYPE_ID` AS `ACC_TYPE_ID`,
            `actions`.`ACC_S_ID` AS `ACC_S_ID`,
            `actions`.`ACC_INVESTOR_NO` AS `ACC_INVESTOR_NO`,
            `actions`.`ACC_INVESTOR_NAME` AS `ACC_INVESTOR_NAME`,
            `actions`.`INVS_S_ID` AS `INVS_S_ID`,
            `actions`.`CNTRY_ID` AS `CNTRY_ID`,
            `actions`.`ACC_S_NUM` AS `ACC_S_NUM`,
            `actions`.`ACC_S_NAME_A` AS `ACC_S_NAME_A`,
            `actions`.`ACC_S_NAME_E` AS `ACC_S_NAME_E`,
            `actions`.`ACC_S_CODE` AS `ACC_S_CODE`,
            `actions`.`INVS_S_NUM` AS `INVS_S_NUM`,
            `actions`.`INVS_S_NAME_A` AS `INVS_S_NAME_A`,
            `actions`.`INVS_S_NAME_E` AS `INVS_S_NAME_E`,
            `actions`.`INVS_S_CODE` AS `INVS_S_CODE`,
            `actions`.`ACC_TYPE_NUM` AS `ACC_TYPE_NUM`,
            `actions`.`ACC_TYPE_NAME_A` AS `ACC_TYPE_NAME_A`,
            `actions`.`ACC_TYPE_NAME_E` AS `ACC_TYPE_NAME_E`,
            `actions`.`ACC_TYPE_CODE` AS `ACC_TYPE_CODE`,
            `actions`.`CNTRY_NUM` AS `CNTRY_NUM`,
            `actions`.`CNTRY_NAME_A` AS `CNTRY_NAME_A`,
            `actions`.`CNTRY_NAME_E` AS `CNTRY_NAME_E`,
            `actions`.`CNTRY_NAT_A` AS `CNTRY_NAT_A`,
            `actions`.`CNTRY_NAT_E` AS `CNTRY_NAT_E`,
            `actions`.`source_file_date` AS `source_file_date`,
            `actions`.`GROUP_ID` AS `GROUP_ID`,
            `actions`.`GROUP_NUM` AS `GROUP_NUM`,
            `actions`.`GROUP_NAME_A` AS `GROUP_NAME_A`,
            `actions`.`GROUP_NAME_E` AS `GROUP_NAME_E`,
            `actions`.`sector_id` AS `sector_id`,
            `actions`.`sector_num` AS `sector_num`,
            `actions`.`sector_name_ar` AS `sector_name_ar`,
            `actions`.`sector_name_en` AS `sector_name_en`,
            `actions`.`main_sector_id` AS `main_sector_id`,
            `actions`.`main_sector_num` AS `main_sector_num`,
            `actions`.`main_sector_name_ar` AS `main_sector_name_ar`,
            `actions`.`main_sector_name_en` AS `main_sector_name_en`,
            `actions`.`INVS_TYPE_ID` AS `INVS_TYPE_ID`,
            `actions`.`INVS_TYPE_NUM` AS `INVS_TYPE_NUM`,
            `actions`.`INVS_TYPE_NAME_A` AS `INVS_TYPE_NAME_A`,
            `actions`.`INVS_TYPE_NAME_E` AS `INVS_TYPE_NAME_E`,
            `actions`.`INVS_CAT_ID` AS `INVS_CAT_ID`,
            `actions`.`INVS_CAT_NUM` AS `INVS_CAT_NUM`,
            `actions`.`INVS_CAT_NAME_A` AS `INVS_CAT_NAME_A`,
            `actions`.`INVS_CAT_NAME_E` AS `INVS_CAT_NAME_E`,
            `total_exe_qty_v`.`total_exe_qty` AS `total_exe_qty` 
        FROM ((( 
            SELECT * 
            FROM `actions_history_tmp_v` 
            WHERE(
                `actions_history_tmp_v`.`source_file_date` >= @startDate 
                AND `actions_history_tmp_v`.`source_file_date` <= @endDate
                AND `actions_history_tmp_v`.`order_id` > ''
            )
        )) `actions`
        LEFT JOIN `total_exe_qty_v` 
        ON `actions`.`order_id` = `total_exe_qty_v`.`order_id`
    )) t1 
    ";

                    // Append filtering conditions
                    bool hasFilterConditions = false;
                    if (filter.source_file_date_to_FilterText == "" || filter.source_file_date_to_FilterText == null) filter.source_file_date_to_FilterText = "20700319";
                    if (filter.source_file_date_from_FilterText == "" || filter.source_file_date_from_FilterText == null) filter.source_file_date_from_FilterText = "20000319";

                    if (!string.IsNullOrEmpty(filter.Name) || !string.IsNullOrEmpty(filter.Name) || !string.IsNullOrEmpty(filter.entry_date_from_FilterText) || !string.IsNullOrEmpty(filter.entry_date_to_FilterText))
                    {
                        query += " WHERE ";

                        if (!string.IsNullOrEmpty(filter.Name))
                        {
                            query += " order_id = @filterText ";
                            hasFilterConditions = true;
                        }

                        if (!string.IsNullOrEmpty(filter.status))
                        {
                            if (hasFilterConditions) query += " AND ";
                            query += " status = @filterStatus ";
                        }


                        if (!string.IsNullOrEmpty(filter.entry_date_from_FilterText))
                        {
                            if (hasFilterConditions) query += " AND ";
                            query += " entry_date >= @entry_date_from_FilterText ";
                        }



                        if (!string.IsNullOrEmpty(filter.entry_date_to_FilterText))
                        {
                            if (hasFilterConditions) query += " AND ";
                            query += " entry_date <= @entry_date_to_FilterText ";
                        }


                    }

                    // Append LIMIT and OFFSET if no filters
                    if (!hasFilterConditions)
                    {
                        query += "  LIMIT @count OFFSET @startIndex;";
                    }

                    // If filters are applied, add LIMIT and OFFSET
                    if (hasFilterConditions)
                    {
                        query += "  LIMIT @count OFFSET @startIndex;";
                    }




                    MySqlCommand xCmd = new MySqlCommand("set net_write_timeout=99999; set net_read_timeout=99999;" + query, xConn);
                    xCmd.CommandTimeout = 1000000; // Increase to 300 seconds
                    xCmd.Parameters.AddWithValue("@startDate", filter.source_file_date_from_FilterText);




                    xCmd.Parameters.AddWithValue("@endDate", filter.source_file_date_to_FilterText);
                    xCmd.Parameters.AddWithValue("@count", count); // count should be a variable
                    xCmd.Parameters.AddWithValue("@startIndex", startIndex); // startIndex should be a variable
                    xCmd.Parameters.AddWithValue("@filterText", filter.Name); // startIndex should be a variable
                    xCmd.Parameters.AddWithValue("@filterStatus", filter.status); // startIndex should be a variable
                    xCmd.Parameters.AddWithValue("@entry_date_from_FilterText", filter.entry_date_from_FilterText); // startIndex should be a variable
                    xCmd.Parameters.AddWithValue("@entry_date_to_FilterText", filter.entry_date_to_FilterText); // startIndex should be a variable


                    //shaima
                    try
                    {
                        using (MySqlDataReader xRdr = xCmd.ExecuteReader())
                        {

                            ActionAccount account = new ActionAccount(); // Initialize the ActionAccount object
                            while (xRdr.Read())
                            {
                                account.record_num = xRdr.GetInt32("record_num").ToString();
                                account.order_id = xRdr.GetString("order_id");
                                account.entry_date = xRdr.GetString("entry_date");



                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("client_id")))
                                {
                                    account.Client_id = xRdr.GetString("client_id");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.Client_id = null; // or set a default value
                                }




                                account.status = xRdr.GetString("status");
                                var validityDateColumn = xRdr["validity_date"];

                                account.validity_date = (validityDateColumn != DBNull.Value) ? xRdr.GetString(xRdr.GetOrdinal("validity_date")) : null;
                                account.broker_code = xRdr.GetString("broker_code");



                                // account.index = xRdr.GetInt32("index").ToString();
                                var indexColumn = xRdr["index"];

                                account.index = (indexColumn != DBNull.Value) ? xRdr.GetInt32(xRdr.GetOrdinal("index")).ToString() : "null";

                                account.broker_name = xRdr.GetString("broker_name");
                                var stationIdColumn = xRdr["station_id"];

                                account.station_id = (stationIdColumn != DBNull.Value) ? xRdr.GetString(xRdr.GetOrdinal("station_id")) : null;

                                account.security_code = xRdr.GetString("security_code");
                                account.security_name = xRdr.GetString("security_name");
                                account.side = xRdr.GetString("side");
                                account.order_class = xRdr.GetString("order_class");
                                account.entered_price = xRdr.GetDouble("entered_price").ToString();
                                account.entered_qty = xRdr.GetInt32("entered_qty").ToString();
                                var minQtyColumn = xRdr["min_qty"];

                                account.min_qty = (minQtyColumn != DBNull.Value) ? xRdr.GetInt32(xRdr.GetOrdinal("min_qty")).ToString() : "null";

                                account.disp_qty = xRdr.GetInt32("disp_qty").ToString();
                                account.validity = xRdr.GetString("validity");
                                object validityTime = xRdr.GetValue(xRdr.GetOrdinal("validity_time"));
                                account.validity_time = validityTime != DBNull.Value ? (string)validityTime : null;




                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("ACC_NUM")))
                                {
                                    account.ACC_NUM = xRdr.GetInt32("ACC_NUM").ToString();
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.ACC_NUM = null; // or set a default value
                                }



                                account.client_name = xRdr.GetString("client_name");




                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("CNTRY_NAT_A")))
                                {
                                    account.CNTRY_NAT_A = xRdr.GetString("CNTRY_NAT_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.CNTRY_NAT_A = null; // or set a default value
                                }







                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("ACC_INVESTOR_NO")))
                                {
                                    account.ACC_INVESTOR_NO = xRdr.GetString("ACC_INVESTOR_NO");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.ACC_INVESTOR_NO = null; // or set a default value
                                }







                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("Acc_S_Name_A")))
                                {
                                    account.account_state = xRdr.GetString("Acc_S_Name_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.account_state = null; // or set a default value
                                }









                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("GROUP_NAME_A")))
                                {
                                    account.GROUP_NAME_A = xRdr.GetString("GROUP_NAME_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.GROUP_NAME_A = null; // or set a default value
                                }





                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("main_sector_name_ar")))
                                {
                                    account.main_sector_name_ar = xRdr.GetString("main_sector_name_ar");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.main_sector_name_ar = null; // or set a default value
                                }



                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("sector_name_ar")))
                                {
                                    account.sector_name_ar = xRdr.GetString("sector_name_ar");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.sector_name_ar = null; // or set a default value
                                }








                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("Invs_Type_Name_A")))
                                {
                                    account.investor_type = xRdr.GetString("Invs_Type_Name_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.investor_type = null; // or set a default value
                                }





                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("Invs_Cat_Name_A")))
                                {
                                    account.investor_cat = xRdr.GetString("Invs_Cat_Name_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.investor_cat = null; // or set a default value
                                }




                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("Invs_S_Name_A")))
                                {
                                    account.investor_state = xRdr.GetString("Invs_S_Name_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.investor_state = null; // or set a default value
                                }



                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("Acc_Type_Name_A")))
                                {
                                    account.account_type = xRdr.GetString("Acc_Type_Name_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.account_type = null; // or set a default value
                                }





                                var entryTimeColumn = xRdr["entry_time"];

                                account.entry_time = (entryTimeColumn != DBNull.Value) ? xRdr.GetString(xRdr.GetOrdinal("entry_time")) : "null";
                                account.exe_price = xRdr.GetDouble("exe_price").ToString();
                                account.exe_qty = xRdr.GetInt32("exe_qty").ToString();

                                account.rem_qty = xRdr.GetInt32("rem_qty").ToString();
                                account.exe_time = xRdr.GetString("exe_time");
                                account.source_file_date = xRdr.GetInt32("source_file_date").ToString();
                                account.origin = xRdr.GetString("origin");
                                var orderTypeDescColumn = xRdr["Order_Type_Desc"];

                                account.order_type = (orderTypeDescColumn != DBNull.Value) ? xRdr.GetString(xRdr.GetOrdinal("Order_Type_Desc")) : "null";

                                var disclosedQtyColumn = xRdr["disclosed_qty"];

                                account.disclosed_qty = (disclosedQtyColumn != DBNull.Value) ? xRdr.GetInt64(xRdr.GetOrdinal("disclosed_qty")).ToString() : "null";



                                data2.Add(account); // Add the populated account to the list



                                Customer customer = new Customer
                                {

                                    record_num = account.record_num,
                                    Name = account.order_id,
                                    entry_date = account.entry_date,
                                    Client_id = account.Client_id,
                                    status = account.status,
                                    validity_date = account.validity_date,
                                    broker_code = account.broker_code,

                                    index = account.index,
                                    broker_name = account.broker_name,
                                    station_id = account.station_id,




                                    security_code = account.security_code,
                                    security_name = account.security_name,
                                    side = account.side,
                                    order_class = account.order_class,
                                    entered_price = account.entered_price,
                                    entered_qty = account.entered_qty,
                                    min_qty = account.min_qty,
                                    disp_qty = account.disp_qty,
                                    validity = account.validity,
                                    validity_time = account.validity_time,
                                    ACC_NUM = account.ACC_NUM,
                                    client_name = account.client_name,
                                    CNTRY_NAT_A = account.CNTRY_NAT_A,
                                    ACC_INVESTOR_NO = account.ACC_INVESTOR_NO,
                                    account_state = account.account_state,
                                    GROUP_NAME_A = account.GROUP_NAME_A,
                                    main_sector_name_ar = account.main_sector_name_ar,
                                    sector_name_ar = account.sector_name_ar,
                                    investor_type = account.investor_type,
                                    investor_cat = account.investor_cat,
                                    investor_state = account.investor_state,
                                    account_type = account.account_type,
                                    entry_time = account.entry_time,
                                    exe_price = account.exe_price,
                                    exe_qty = account.exe_qty,
                                    total_exe_qty_v = account.total_exe_qty_v,
                                    rem_qty = account.rem_qty,
                                    exe_time = account.exe_time,
                                    source_file_date = account.source_file_date,
                                    origin = account.origin,
                                    order_type = account.order_type,
                                    disclosed_qty = account.disclosed_qty




                                };
                                list2.Add(customer);




                            }



                        }
                    }
                    catch(Exception ex ){

                        var xx = ex;

                    }
                }

                recordCount = GetCount2( filter, connectionString);
            }

            else
            {


                /**********************************************************************************************************************************************************************************************/

                using (MySqlConnection xConn = new MySqlConnection(connectionString))
                {
                    // Open the connection
                    xConn.Open();

                    string query = @"
    select * from actions_accounts_gtc_gtd_v where 1 = 1 ";



                    if (filter.isToday)
                    {
                        query += " AND  record_num  >= 0";


                    }

                    // Append filtering conditions
                    bool hasFilterConditions = false;
                    if (filter.source_file_date_to_FilterText == "" || filter.source_file_date_to_FilterText == null) filter.source_file_date_to_FilterText = "20700319";
                    if (filter.source_file_date_from_FilterText == "" || filter.source_file_date_from_FilterText == null) filter.source_file_date_from_FilterText = "20000319";

                    if (!string.IsNullOrEmpty(filter.Name) || !string.IsNullOrEmpty(filter.Name) || !string.IsNullOrEmpty(filter.entry_date_from_FilterText) || !string.IsNullOrEmpty(filter.entry_date_to_FilterText))
                    {


                        if (!string.IsNullOrEmpty(filter.Name))
                        {
                            query += " AND ";
                            query += " order_id = @filterText ";
                            hasFilterConditions = true;
                        }

                        if (!string.IsNullOrEmpty(filter.status))
                        {
                            query += " AND ";
                            query += " status = @filterStatus ";
                        }


                        if (!string.IsNullOrEmpty(filter.entry_date_from_FilterText))
                        {
                            query += " AND ";
                            query += " entry_date >= @entry_date_from_FilterText ";
                        }



                        if (!string.IsNullOrEmpty(filter.entry_date_to_FilterText))
                        {
                            query += " AND ";
                            query += " entry_date <= @entry_date_to_FilterText ";
                        }


                    }

                    // Append LIMIT and OFFSET if no filters
                    if (!hasFilterConditions)
                    {
                        query += " group by order_id LIMIT @count OFFSET @startIndex;";
                    }

                    // If filters are applied, add LIMIT and OFFSET
                    if (hasFilterConditions)
                    {
                        query += " group by order_id LIMIT @count OFFSET @startIndex;";
                    }




                    MySqlCommand xCmd = new MySqlCommand(query, xConn);

                    xCmd.Parameters.AddWithValue("@startDate", filter.source_file_date_from_FilterText);




                    xCmd.Parameters.AddWithValue("@endDate", filter.source_file_date_to_FilterText);
                    xCmd.Parameters.AddWithValue("@count", count); // count should be a variable
                    xCmd.Parameters.AddWithValue("@startIndex", startIndex); // startIndex should be a variable
                    xCmd.Parameters.AddWithValue("@filterText", filter.Name); // startIndex should be a variable
                    xCmd.Parameters.AddWithValue("@filterStatus", filter.status); // startIndex should be a variable
                    xCmd.Parameters.AddWithValue("@entry_date_from_FilterText", filter.entry_date_from_FilterText); // startIndex should be a variable
                    xCmd.Parameters.AddWithValue("@entry_date_to_FilterText", filter.entry_date_to_FilterText); // startIndex should be a variable



                    using (MySqlDataReader xRdr = xCmd.ExecuteReader())
                    {
                        for (int i = startIndex; i < startIndex + count; i++)
                        {
                            ActionAccount account = new ActionAccount(); // Initialize the ActionAccount object
                            if (xRdr.Read())
                            {
                                account.record_num = xRdr.GetInt32("record_num").ToString();
                                account.order_id = xRdr.GetString("order_id");
                                account.entry_date = xRdr.GetString("entry_date");



                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("client_id")))
                                {
                                    account.Client_id = xRdr.GetString("client_id");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.Client_id = null; // or set a default value
                                }




                                account.status = xRdr.GetString("status");
                                var validityDateColumn = xRdr["validity_date"];

                                account.validity_date = (validityDateColumn != DBNull.Value) ? xRdr.GetString(xRdr.GetOrdinal("validity_date")) : null;
                                account.broker_code = xRdr.GetString("broker_code");





                                account.broker_name = xRdr.GetString("broker_name");
                                var stationIdColumn = xRdr["station_id"];

                                account.station_id = (stationIdColumn != DBNull.Value) ? xRdr.GetString(xRdr.GetOrdinal("station_id")) : null;

                                account.security_code = xRdr.GetString("security_code");
                                account.security_name = xRdr.GetString("security_name");
                                account.side = xRdr.GetString("side");
                                account.order_class = xRdr.GetString("order_class");
                                account.entered_price = xRdr.GetDouble("entered_price").ToString();
                                account.entered_qty = xRdr.GetInt32("entered_qty").ToString();
                                var minQtyColumn = xRdr["min_qty"];

                                account.min_qty = (minQtyColumn != DBNull.Value) ? xRdr.GetInt32(xRdr.GetOrdinal("min_qty")).ToString() : "null";
                                account.disp_qty = xRdr.GetInt32("disp_qty").ToString();
                                account.validity = xRdr.GetString("validity");
                                account.validity_time = xRdr.GetString("validity_time");




                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("ACC_NUM")))
                                {
                                    account.ACC_NUM = xRdr.GetInt32("ACC_NUM").ToString();
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.ACC_NUM = null; // or set a default value
                                }



                                account.client_name = xRdr.GetString("client_name");




                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("CNTRY_NAT_A")))
                                {
                                    account.CNTRY_NAT_A = xRdr.GetString("CNTRY_NAT_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.CNTRY_NAT_A = null; // or set a default value
                                }







                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("ACC_INVESTOR_NO")))
                                {
                                    account.ACC_INVESTOR_NO = xRdr.GetString("ACC_INVESTOR_NO");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.ACC_INVESTOR_NO = null; // or set a default value
                                }







                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("Acc_S_Name_A")))
                                {
                                    account.account_state = xRdr.GetString("Acc_S_Name_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.account_state = null; // or set a default value
                                }









                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("GROUP_NAME_A")))
                                {
                                    account.GROUP_NAME_A = xRdr.GetString("GROUP_NAME_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.GROUP_NAME_A = null; // or set a default value
                                }





                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("main_sector_name_ar")))
                                {
                                    account.main_sector_name_ar = xRdr.GetString("main_sector_name_ar");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.main_sector_name_ar = null; // or set a default value
                                }



                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("sector_name_ar")))
                                {
                                    account.sector_name_ar = xRdr.GetString("sector_name_ar");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.sector_name_ar = null; // or set a default value
                                }








                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("Invs_Type_Name_A")))
                                {
                                    account.investor_type = xRdr.GetString("Invs_Type_Name_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.investor_type = null; // or set a default value
                                }





                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("Invs_Cat_Name_A")))
                                {
                                    account.investor_cat = xRdr.GetString("Invs_Cat_Name_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.investor_cat = null; // or set a default value
                                }




                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("Invs_S_Name_A")))
                                {
                                    account.investor_state = xRdr.GetString("Invs_S_Name_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.investor_state = null; // or set a default value
                                }



                                if (!xRdr.IsDBNull(xRdr.GetOrdinal("Acc_Type_Name_A")))
                                {
                                    account.account_type = xRdr.GetString("Acc_Type_Name_A");
                                    // Use the clientId as needed
                                }
                                else
                                {
                                    // Handle the NULL case as needed
                                    account.account_type = null; // or set a default value
                                }





                                var entryTimeColumn = xRdr["entry_time"];

                                account.entry_time = (entryTimeColumn != DBNull.Value) ? xRdr.GetString(xRdr.GetOrdinal("entry_time")) : "null";
                                account.exe_price = xRdr.GetDouble("exe_price").ToString();
                                account.exe_qty = xRdr.GetInt32("exe_qty").ToString();

                                account.rem_qty = xRdr.GetInt32("rem_qty").ToString();
                                account.exe_time = xRdr.GetString("exe_time");
                                //account.source_file_date = xRdr.GetInt32("source_file_date").ToString();
                                account.origin = xRdr.GetString("origin");
                                account.order_type = xRdr.IsDBNull(xRdr.GetOrdinal("Order_Type_Desc")) ? null : xRdr.GetString(xRdr.GetOrdinal("Order_Type_Desc"));
                                var disclosedQtyColumn = xRdr["disclosed_qty"];

                                account.disclosed_qty = (disclosedQtyColumn != DBNull.Value) ? xRdr.GetInt64(xRdr.GetOrdinal("disclosed_qty")).ToString() : "null";



                                data.Add(account); // Add the populated account to the list
                            }

                            Customer customer = new Customer
                            {
                                Id = i + 1,
                                record_num = account.record_num,
                                Name = account.order_id,
                                entry_date = account.entry_date,
                                Client_id = account.Client_id,
                                status = account.status,
                                validity_date = account.validity_date,
                                broker_code = account.broker_code,

                                index = account.index,
                                broker_name = account.broker_name,
                                station_id = account.station_id,




                                security_code = account.security_code,
                                security_name = account.security_name,
                                side = account.side,
                                order_class = account.order_class,
                                entered_price = account.entered_price,
                                entered_qty = account.entered_qty,
                                min_qty = account.min_qty,
                                disp_qty = account.disp_qty,
                                validity = account.validity,
                                validity_time = account.validity_time,
                                ACC_NUM = account.ACC_NUM,
                                client_name = account.client_name,
                                CNTRY_NAT_A = account.CNTRY_NAT_A,
                                ACC_INVESTOR_NO = account.ACC_INVESTOR_NO,
                                account_state = account.account_state,
                                GROUP_NAME_A = account.GROUP_NAME_A,
                                main_sector_name_ar = account.main_sector_name_ar,
                                sector_name_ar = account.sector_name_ar,
                                investor_type = account.investor_type,
                                investor_cat = account.investor_cat,
                                investor_state = account.investor_state,
                                account_type = account.account_type,
                                entry_time = account.entry_time,
                                exe_price = account.exe_price,
                                exe_qty = account.exe_qty,
                                total_exe_qty_v = account.total_exe_qty_v,
                                rem_qty = account.rem_qty,
                                exe_time = account.exe_time,
                                source_file_date = account.source_file_date,
                                origin = account.origin,
                                order_type = account.order_type,
                                disclosed_qty = account.disclosed_qty




                            };
                            list.Add(customer);
                        }
                    }
                }



                using (MySqlConnection xConn = new MySqlConnection(connectionString))
                {
                    // Open the connection
                    xConn.Open();

                    string query = @"
    select * from actions_accounts_gtc_gtd_v where 1 = 1 ";
                    if (filter.isToday)
                    {
                        query += " AND  record_num  >= 0";


                    }

                    // Append filtering conditions
                    bool hasFilterConditions = false;
                    if (filter.source_file_date_to_FilterText == "" || filter.source_file_date_to_FilterText == null) filter.source_file_date_to_FilterText = "20700319";
                    if (filter.source_file_date_from_FilterText == "" || filter.source_file_date_from_FilterText == null) filter.source_file_date_from_FilterText = "20000319";

                    if (!string.IsNullOrEmpty(filter.Name) || !string.IsNullOrEmpty(filter.Name) || !string.IsNullOrEmpty(filter.entry_date_from_FilterText) || !string.IsNullOrEmpty(filter.entry_date_to_FilterText))
                    {


                        if (!string.IsNullOrEmpty(filter.Name))
                        {
                            query += " AND ";
                            query += " order_id = @filterText ";
                            hasFilterConditions = true;
                        }

                        if (!string.IsNullOrEmpty(filter.status))
                        {
                            query += " AND ";
                            query += " status = @filterStatus ";
                        }


                        if (!string.IsNullOrEmpty(filter.entry_date_from_FilterText))
                        {
                            query += " AND ";
                            query += " entry_date >= @entry_date_from_FilterText ";
                        }



                        if (!string.IsNullOrEmpty(filter.entry_date_to_FilterText))
                        {
                            query += " AND ";
                            query += " entry_date <= @entry_date_to_FilterText ";
                        }


                    }

                    // Append LIMIT and OFFSET if no filters
                    if (!hasFilterConditions)
                    {
                        query += "  LIMIT @count OFFSET @startIndex;";
                    }

                    // If filters are applied, add LIMIT and OFFSET
                    if (hasFilterConditions)
                    {
                        query += "  LIMIT @count OFFSET @startIndex;";
                    }




                    MySqlCommand xCmd = new MySqlCommand(query, xConn);

                    xCmd.Parameters.AddWithValue("@startDate", filter.source_file_date_from_FilterText);




                    xCmd.Parameters.AddWithValue("@endDate", filter.source_file_date_to_FilterText);
                    xCmd.Parameters.AddWithValue("@count", count); // count should be a variable
                    xCmd.Parameters.AddWithValue("@startIndex", startIndex); // startIndex should be a variable
                    xCmd.Parameters.AddWithValue("@filterText", filter.Name); // startIndex should be a variable
                    xCmd.Parameters.AddWithValue("@filterStatus", filter.status); // startIndex should be a variable
                    xCmd.Parameters.AddWithValue("@entry_date_from_FilterText", filter.entry_date_from_FilterText); // startIndex should be a variable
                    xCmd.Parameters.AddWithValue("@entry_date_to_FilterText", filter.entry_date_to_FilterText); // startIndex should be a variable



                    using (MySqlDataReader xRdr = xCmd.ExecuteReader())
                    {

                        ActionAccount account = new ActionAccount(); // Initialize the ActionAccount object
                        while (xRdr.Read())
                        {
                            account.record_num = xRdr.GetInt32("record_num").ToString();
                            account.order_id = xRdr.GetString("order_id");
                            account.entry_date = xRdr.GetString("entry_date");



                            if (!xRdr.IsDBNull(xRdr.GetOrdinal("client_id")))
                            {
                                account.Client_id = xRdr.GetString("client_id");
                                // Use the clientId as needed
                            }
                            else
                            {
                                // Handle the NULL case as needed
                                account.Client_id = null; // or set a default value
                            }




                            account.status = xRdr.GetString("status");
                            var validityDateColumn = xRdr["validity_date"];

                            account.validity_date = (validityDateColumn != DBNull.Value) ? xRdr.GetString(xRdr.GetOrdinal("validity_date")) : null;
                            account.broker_code = xRdr.GetString("broker_code");





                            account.broker_name = xRdr.GetString("broker_name");
                            var stationIdColumn = xRdr["station_id"];

                            account.station_id = (stationIdColumn != DBNull.Value) ? xRdr.GetString(xRdr.GetOrdinal("station_id")) : null;

                            account.security_code = xRdr.GetString("security_code");
                            account.security_name = xRdr.GetString("security_name");
                            account.side = xRdr.GetString("side");
                            account.order_class = xRdr.GetString("order_class");
                            account.entered_price = xRdr.GetDouble("entered_price").ToString();
                            account.entered_qty = xRdr.GetInt32("entered_qty").ToString();
                            var minQtyColumn = xRdr["min_qty"];

                            account.min_qty = (minQtyColumn != DBNull.Value) ? xRdr.GetInt32(xRdr.GetOrdinal("min_qty")).ToString() : "null";
                            account.disp_qty = xRdr.GetInt32("disp_qty").ToString();
                            account.validity = xRdr.GetString("validity");
                            account.validity_time = xRdr.GetString("validity_time");




                            if (!xRdr.IsDBNull(xRdr.GetOrdinal("ACC_NUM")))
                            {
                                account.ACC_NUM = xRdr.GetInt32("ACC_NUM").ToString();
                                // Use the clientId as needed
                            }
                            else
                            {
                                // Handle the NULL case as needed
                                account.ACC_NUM = null; // or set a default value
                            }



                            account.client_name = xRdr.GetString("client_name");




                            if (!xRdr.IsDBNull(xRdr.GetOrdinal("CNTRY_NAT_A")))
                            {
                                account.CNTRY_NAT_A = xRdr.GetString("CNTRY_NAT_A");
                                // Use the clientId as needed
                            }
                            else
                            {
                                // Handle the NULL case as needed
                                account.CNTRY_NAT_A = null; // or set a default value
                            }







                            if (!xRdr.IsDBNull(xRdr.GetOrdinal("ACC_INVESTOR_NO")))
                            {
                                account.ACC_INVESTOR_NO = xRdr.GetString("ACC_INVESTOR_NO");
                                // Use the clientId as needed
                            }
                            else
                            {
                                // Handle the NULL case as needed
                                account.ACC_INVESTOR_NO = null; // or set a default value
                            }







                            if (!xRdr.IsDBNull(xRdr.GetOrdinal("Acc_S_Name_A")))
                            {
                                account.account_state = xRdr.GetString("Acc_S_Name_A");
                                // Use the clientId as needed
                            }
                            else
                            {
                                // Handle the NULL case as needed
                                account.account_state = null; // or set a default value
                            }









                            if (!xRdr.IsDBNull(xRdr.GetOrdinal("GROUP_NAME_A")))
                            {
                                account.GROUP_NAME_A = xRdr.GetString("GROUP_NAME_A");
                                // Use the clientId as needed
                            }
                            else
                            {
                                // Handle the NULL case as needed
                                account.GROUP_NAME_A = null; // or set a default value
                            }





                            if (!xRdr.IsDBNull(xRdr.GetOrdinal("main_sector_name_ar")))
                            {
                                account.main_sector_name_ar = xRdr.GetString("main_sector_name_ar");
                                // Use the clientId as needed
                            }
                            else
                            {
                                // Handle the NULL case as needed
                                account.main_sector_name_ar = null; // or set a default value
                            }



                            if (!xRdr.IsDBNull(xRdr.GetOrdinal("sector_name_ar")))
                            {
                                account.sector_name_ar = xRdr.GetString("sector_name_ar");
                                // Use the clientId as needed
                            }
                            else
                            {
                                // Handle the NULL case as needed
                                account.sector_name_ar = null; // or set a default value
                            }








                            if (!xRdr.IsDBNull(xRdr.GetOrdinal("Invs_Type_Name_A")))
                            {
                                account.investor_type = xRdr.GetString("Invs_Type_Name_A");
                                // Use the clientId as needed
                            }
                            else
                            {
                                // Handle the NULL case as needed
                                account.investor_type = null; // or set a default value
                            }





                            if (!xRdr.IsDBNull(xRdr.GetOrdinal("Invs_Cat_Name_A")))
                            {
                                account.investor_cat = xRdr.GetString("Invs_Cat_Name_A");
                                // Use the clientId as needed
                            }
                            else
                            {
                                // Handle the NULL case as needed
                                account.investor_cat = null; // or set a default value
                            }




                            if (!xRdr.IsDBNull(xRdr.GetOrdinal("Invs_S_Name_A")))
                            {
                                account.investor_state = xRdr.GetString("Invs_S_Name_A");
                                // Use the clientId as needed
                            }
                            else
                            {
                                // Handle the NULL case as needed
                                account.investor_state = null; // or set a default value
                            }



                            if (!xRdr.IsDBNull(xRdr.GetOrdinal("Acc_Type_Name_A")))
                            {
                                account.account_type = xRdr.GetString("Acc_Type_Name_A");
                                // Use the clientId as needed
                            }
                            else
                            {
                                // Handle the NULL case as needed
                                account.account_type = null; // or set a default value
                            }





                            var entryTimeColumn = xRdr["entry_time"];

                            account.entry_time = (entryTimeColumn != DBNull.Value) ? xRdr.GetString(xRdr.GetOrdinal("entry_time")) : "null";
                            account.exe_price = xRdr.GetDouble("exe_price").ToString();
                            account.exe_qty = xRdr.GetInt32("exe_qty").ToString();

                            account.rem_qty = xRdr.GetInt32("rem_qty").ToString();
                            account.exe_time = xRdr.GetString("exe_time");
                            //account.source_file_date = xRdr.GetInt32("source_file_date").ToString();
                            account.origin = xRdr.GetString("origin");
                            account.order_type = xRdr.IsDBNull(xRdr.GetOrdinal("Order_Type_Desc")) ? null : xRdr.GetString(xRdr.GetOrdinal("Order_Type_Desc"));
                            var disclosedQtyColumn = xRdr["disclosed_qty"];

                            account.disclosed_qty = (disclosedQtyColumn != DBNull.Value) ? xRdr.GetInt64(xRdr.GetOrdinal("disclosed_qty")).ToString() : "null";



                            data2.Add(account); // Add the populated account to the list



                            Customer customer = new Customer
                            {

                                record_num = account.record_num,
                                Name = account.order_id,
                                entry_date = account.entry_date,
                                Client_id = account.Client_id,
                                status = account.status,
                                validity_date = account.validity_date,
                                broker_code = account.broker_code,

                                index = account.index,
                                broker_name = account.broker_name,
                                station_id = account.station_id,




                                security_code = account.security_code,
                                security_name = account.security_name,
                                side = account.side,
                                order_class = account.order_class,
                                entered_price = account.entered_price,
                                entered_qty = account.entered_qty,
                                min_qty = account.min_qty,
                                disp_qty = account.disp_qty,
                                validity = account.validity,
                                validity_time = account.validity_time,
                                ACC_NUM = account.ACC_NUM,
                                client_name = account.client_name,
                                CNTRY_NAT_A = account.CNTRY_NAT_A,
                                ACC_INVESTOR_NO = account.ACC_INVESTOR_NO,
                                account_state = account.account_state,
                                GROUP_NAME_A = account.GROUP_NAME_A,
                                main_sector_name_ar = account.main_sector_name_ar,
                                sector_name_ar = account.sector_name_ar,
                                investor_type = account.investor_type,
                                investor_cat = account.investor_cat,
                                investor_state = account.investor_state,
                                account_type = account.account_type,
                                entry_time = account.entry_time,
                                exe_price = account.exe_price,
                                exe_qty = account.exe_qty,
                                total_exe_qty_v = account.total_exe_qty_v,
                                rem_qty = account.rem_qty,
                                exe_time = account.exe_time,
                                source_file_date = account.source_file_date,
                                origin = account.origin,
                                order_type = account.order_type,
                                disclosed_qty = account.disclosed_qty




                            };
                            list2.Add(customer);




                        }



                    }
                }

                recordCount = GetCount( filter, connectionString);








                /*****************************************************************************************************************************************************************************************************/
            }













            shared.GroupedRecords = list;
            shared.Count = recordCount; // Update the shared Count to trigger binding
            shared.AllRecords = list2;
            Count = recordCount; // This will trigger the binding update

            return shared;
        }

        public static int GetCount2( ActionsFilter filter, string connectionString)
        {
            int recordCount;
            using (MySqlConnection xConn = new MySqlConnection(connectionString))
            {
                // Open the connection
                xConn.Open();

                string query = @"
    select count(*) as records_num from ( SELECT *  FROM 
    ( 
        SELECT
            `actions`.`record_num` AS `record_num`,
            `actions`.`order_id` AS `order_id`,
            `actions`.`client_id` AS `client_id`,
            `actions`.`index` AS `index`,
            `actions`.`broker_code` AS `broker_code`,
            `actions`.`broker_name` AS `broker_name`,
            `actions`.`station_id` AS `station_id`,
            `actions`.`origin` AS `origin`,
            `actions`.`order_type` AS `order_type`,
            `actions`.`order_type_desc` AS `order_type_desc`,
            `actions`.`disclosed_qty` AS `disclosed_qty`,
            `actions`.`ok_date` AS `ok_date`,
            `actions`.`ok_time` AS `ok_time`,
            `actions`.`security_code` AS `security_code`,
            `actions`.`security_symbol` AS `security_symbol`,
            `actions`.`security_name` AS `security_name`,
            `actions`.`side` AS `side`,
            `actions`.`order_class` AS `order_class`,
            `actions`.`status` AS `status`,
            `actions`.`entered_price` AS `entered_price`,
            `actions`.`entered_qty` AS `entered_qty`,
            `actions`.`min_qty` AS `min_qty`,
            `actions`.`disp_qty` AS `disp_qty`,
            `actions`.`validity` AS `validity`,
            `actions`.`validity_date` AS `validity_date`,
            `actions`.`validity_time` AS `validity_time`,
            `actions`.`client` AS `client`,
            `actions`.`client_name` AS `client_name`,
            `actions`.`s_client` AS `s_client`,
            `actions`.`s_client_name` AS `s_client_name`,
            `actions`.`entry_date` AS `entry_date`,
            `actions`.`entry_time` AS `entry_time`,
            `actions`.`exe_price` AS `exe_price`,
            `actions`.`exe_qty` AS `exe_qty`,
            `actions`.`rem_qty` AS `rem_qty`,
            `actions`.`exe_time` AS `exe_time`,
            `actions`.`ctby` AS `ctby`,
            `actions`.`reject_cat` AS `reject_cat`,
            `actions`.`reject_code` AS `reject_code`,
            `actions`.`ACC_BROKER_NO` AS `ACC_BROKER_NO`,
            `actions`.`ACC_NUM` AS `ACC_NUM`,
            `actions`.`ACC_TYPE_ID` AS `ACC_TYPE_ID`,
            `actions`.`ACC_S_ID` AS `ACC_S_ID`,
            `actions`.`ACC_INVESTOR_NO` AS `ACC_INVESTOR_NO`,
            `actions`.`ACC_INVESTOR_NAME` AS `ACC_INVESTOR_NAME`,
            `actions`.`INVS_S_ID` AS `INVS_S_ID`,
            `actions`.`CNTRY_ID` AS `CNTRY_ID`,
            `actions`.`ACC_S_NUM` AS `ACC_S_NUM`,
            `actions`.`ACC_S_NAME_A` AS `ACC_S_NAME_A`,
            `actions`.`ACC_S_NAME_E` AS `ACC_S_NAME_E`,
            `actions`.`ACC_S_CODE` AS `ACC_S_CODE`,
            `actions`.`INVS_S_NUM` AS `INVS_S_NUM`,
            `actions`.`INVS_S_NAME_A` AS `INVS_S_NAME_A`,
            `actions`.`INVS_S_NAME_E` AS `INVS_S_NAME_E`,
            `actions`.`INVS_S_CODE` AS `INVS_S_CODE`,
            `actions`.`ACC_TYPE_NUM` AS `ACC_TYPE_NUM`,
            `actions`.`ACC_TYPE_NAME_A` AS `ACC_TYPE_NAME_A`,
            `actions`.`ACC_TYPE_NAME_E` AS `ACC_TYPE_NAME_E`,
            `actions`.`ACC_TYPE_CODE` AS `ACC_TYPE_CODE`,
            `actions`.`CNTRY_NUM` AS `CNTRY_NUM`,
            `actions`.`CNTRY_NAME_A` AS `CNTRY_NAME_A`,
            `actions`.`CNTRY_NAME_E` AS `CNTRY_NAME_E`,
            `actions`.`CNTRY_NAT_A` AS `CNTRY_NAT_A`,
            `actions`.`CNTRY_NAT_E` AS `CNTRY_NAT_E`,
            `actions`.`source_file_date` AS `source_file_date`,
            `actions`.`GROUP_ID` AS `GROUP_ID`,
            `actions`.`GROUP_NUM` AS `GROUP_NUM`,
            `actions`.`GROUP_NAME_A` AS `GROUP_NAME_A`,
            `actions`.`GROUP_NAME_E` AS `GROUP_NAME_E`,
            `actions`.`sector_id` AS `sector_id`,
            `actions`.`sector_num` AS `sector_num`,
            `actions`.`sector_name_ar` AS `sector_name_ar`,
            `actions`.`sector_name_en` AS `sector_name_en`,
            `actions`.`main_sector_id` AS `main_sector_id`,
            `actions`.`main_sector_num` AS `main_sector_num`,
            `actions`.`main_sector_name_ar` AS `main_sector_name_ar`,
            `actions`.`main_sector_name_en` AS `main_sector_name_en`,
            `actions`.`INVS_TYPE_ID` AS `INVS_TYPE_ID`,
            `actions`.`INVS_TYPE_NUM` AS `INVS_TYPE_NUM`,
            `actions`.`INVS_TYPE_NAME_A` AS `INVS_TYPE_NAME_A`,
            `actions`.`INVS_TYPE_NAME_E` AS `INVS_TYPE_NAME_E`,
            `actions`.`INVS_CAT_ID` AS `INVS_CAT_ID`,
            `actions`.`INVS_CAT_NUM` AS `INVS_CAT_NUM`,
            `actions`.`INVS_CAT_NAME_A` AS `INVS_CAT_NAME_A`,
            `actions`.`INVS_CAT_NAME_E` AS `INVS_CAT_NAME_E`,
            `total_exe_qty_v`.`total_exe_qty` AS `total_exe_qty` 
        FROM ((( 
            SELECT * 
            FROM `actions_history_tmp_v` 
            WHERE(
                `actions_history_tmp_v`.`source_file_date` >= @startDate 
                AND `actions_history_tmp_v`.`source_file_date` <= @endDate
                AND `actions_history_tmp_v`.`order_id` > ''
            )
        )) `actions`
        LEFT JOIN `total_exe_qty_v` 
        ON `actions`.`order_id` = `total_exe_qty_v`.`order_id`
    )) t1 
    ";

                // Append filtering conditions
                bool hasFilterConditions = false;
                if (filter.source_file_date_to_FilterText == "" || filter.source_file_date_to_FilterText == null) filter.source_file_date_to_FilterText = "20700319";
                if (filter.source_file_date_from_FilterText == "" || filter.source_file_date_from_FilterText == null) filter.source_file_date_from_FilterText = "20000319";

                if (!string.IsNullOrEmpty(filter.Name) || !string.IsNullOrEmpty(filter.Name) || !string.IsNullOrEmpty(filter.entry_date_from_FilterText) || !string.IsNullOrEmpty(filter.entry_date_to_FilterText))
                {
                    query += " WHERE ";

                    if (!string.IsNullOrEmpty(filter.Name))
                    {
                        query += " order_id = @filterText ";
                        hasFilterConditions = true;
                    }

                    if (!string.IsNullOrEmpty(filter.status))
                    {
                        if (hasFilterConditions) query += " AND ";
                        query += " status = @filterStatus ";
                    }


                    if (!string.IsNullOrEmpty(filter.entry_date_from_FilterText))
                    {
                        if (hasFilterConditions) query += " AND ";
                        query += " entry_date >= @entry_date_from_FilterText ";
                    }



                    if (!string.IsNullOrEmpty(filter.entry_date_to_FilterText))
                    {
                        if (hasFilterConditions) query += " AND ";
                        query += " entry_date <= @entry_date_to_FilterText ";
                    }


                }

                // Append LIMIT and OFFSET if no filters
                if (!hasFilterConditions)
                {
                    query += "  group by order_id ) as xx ; ";
                }

                // If filters are applied, add LIMIT and OFFSET
                if (hasFilterConditions)
                {
                    query += "  group by order_id ) as xx ; ";
                }




                MySqlCommand xCmd = new MySqlCommand(query, xConn);

                xCmd.Parameters.AddWithValue("@startDate", filter.source_file_date_from_FilterText);




                xCmd.Parameters.AddWithValue("@endDate", filter.source_file_date_to_FilterText);
    
                xCmd.Parameters.AddWithValue("@startIndex", 0); // startIndex should be a variable
                xCmd.Parameters.AddWithValue("@filterText", filter.Name); // startIndex should be a variable
                xCmd.Parameters.AddWithValue("@filterStatus", filter.status); // startIndex should be a variable
                xCmd.Parameters.AddWithValue("@entry_date_from_FilterText", filter.entry_date_from_FilterText); // startIndex should be a variable
                xCmd.Parameters.AddWithValue("@entry_date_to_FilterText", filter.entry_date_to_FilterText); // startIndex should be a variable





                recordCount = Convert.ToInt32(xCmd.ExecuteScalar());




            }

            return recordCount;
        }

        private static int GetCount( ActionsFilter filter, string connectionString)
        {
            int recordCount;
            using (MySqlConnection xConn = new MySqlConnection(connectionString))
            {
                // Open the connection
                xConn.Open();

                string query = @"  select  count(*)  from (
    select count(*) from actions_accounts_gtc_gtd_v where 1 = 1  ";

                if (filter.isToday)
                {
                    query += " AND  record_num  >= 0";


                }


                // Append filtering conditions
                bool hasFilterConditions = false;
                if (filter.source_file_date_to_FilterText == "" || filter.source_file_date_to_FilterText == null) filter.source_file_date_to_FilterText = "20700319";
                if (filter.source_file_date_from_FilterText == "" || filter.source_file_date_from_FilterText == null) filter.source_file_date_from_FilterText = "20000319";

                if (!string.IsNullOrEmpty(filter.Name) || !string.IsNullOrEmpty(filter.Name) || !string.IsNullOrEmpty(filter.entry_date_from_FilterText) || !string.IsNullOrEmpty(filter.entry_date_to_FilterText))
                {


                    if (!string.IsNullOrEmpty(filter.Name))
                    {
                        query += " AND ";
                        query += " order_id = @filterText ";
                        hasFilterConditions = true;
                    }

                    if (!string.IsNullOrEmpty(filter.status))
                    {
                        query += " AND ";
                        query += " status = @filterStatus ";
                    }


                    if (!string.IsNullOrEmpty(filter.entry_date_from_FilterText))
                    {
                        query += " AND ";
                        query += " entry_date >= @entry_date_from_FilterText ";
                    }



                    if (!string.IsNullOrEmpty(filter.entry_date_to_FilterText))
                    {
                        query += " AND ";
                        query += " entry_date <= @entry_date_to_FilterText ";
                    }


                }

                // Append LIMIT and OFFSET if no filters
                if (!hasFilterConditions)
                {
                    query += "  group by order_id ) as xx ; ";
                }

                // If filters are applied, add LIMIT and OFFSET
                if (hasFilterConditions)
                {
                    query += "  group by order_id ) as xx ; ";
                }




                MySqlCommand xCmd = new MySqlCommand(query, xConn);

                xCmd.Parameters.AddWithValue("@startDate", filter.source_file_date_from_FilterText);




                xCmd.Parameters.AddWithValue("@endDate", filter.source_file_date_to_FilterText);
                xCmd.Parameters.AddWithValue("@startIndex", 0); // startIndex should be a variable
                xCmd.Parameters.AddWithValue("@filterText", filter.Name); // startIndex should be a variable
                xCmd.Parameters.AddWithValue("@filterStatus", filter.status); // startIndex should be a variable
                xCmd.Parameters.AddWithValue("@entry_date_from_FilterText", filter.entry_date_from_FilterText); // startIndex should be a variable
                xCmd.Parameters.AddWithValue("@entry_date_to_FilterText", filter.entry_date_to_FilterText); // startIndex should be a variable





                recordCount = Convert.ToInt32(xCmd.ExecuteScalar());




            }

            return recordCount;
        }
    }
}

public class FetchRangeResult : INotifyPropertyChanged
{
    private int _count;

    public IList<Customer> GroupedRecords { get; set; }

    public int Count
    {
        get => _count;
        set
        {
            if (_count != value)
            {
                _count = value;
                OnPropertyChanged(nameof(Count)); // Notify property changed
            }
        }
    }

    public IList<Customer> AllRecords { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class ActionAccount
{
    // Define properties for ActionAccount
    public string record_num { get; set; }
    public string order_id { get; set; }
    public string entry_date { get; set; }
    public string Client_id { get; set; }
    public string status { get; set; }
    public string validity_date { get; set; }
    public string broker_code { get; set; }


    public string index { get; set; }
    public string broker_name { get; set; }
    public string station_id { get; set; }

    public string security_code { get; set; }
    public string security_name { get; set; }
    public string side { get; set; }
    public string order_class { get; set; }
    public string entered_price { get; set; }
    public string entered_qty { get; set; }
    public string min_qty { get; set; }
    public string disp_qty { get; set; }
    public string validity { get; set; }
    public string validity_time { get; set; }
    public string ACC_NUM { get; set; }
    public string client_name { get; set; }
    public string CNTRY_NAT_A { get; set; }

    public string ACC_INVESTOR_NO { get; set; }
    public string account_state { get; set; }
    public string GROUP_NAME_A { get; set; }
    public string main_sector_name_ar { get; set; }
    public string sector_name_ar { get; set; }
    public string investor_type { get; set; }
    public string investor_cat { get; set; }
    public string investor_state { get; set; }
    public string account_type { get; set; }
    public string entry_time { get; set; }
    public string exe_price { get; set; }
    public string exe_qty { get; set; }
    public string total_exe_qty_v { get; set; }
    public string rem_qty { get; set; }
    public string exe_time { get; set; }
    public string source_file_date { get; set; }
    public string origin { get; set; }
    public string order_type { get; set; }
    public string disclosed_qty { get; set; }
    // Add other properties as necessary


}