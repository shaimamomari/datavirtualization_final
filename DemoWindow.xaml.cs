using DataVirtualization.Modules;
using DataVirtualization.Services;
using DataVirtualization.Utilies;
using Nest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace DataVirtualization
{


    /// <summary>
    /// Interaction logic for DemoWindow.xaml
    /// </summary>
    public partial class DemoWindow : UserControl
    {
        private ObservableCollection<Broker> brokers;
        private ObservableCollection<Security> securityCodes;
        public List<string> UniqueBrokerCodes { get; set; }
        private ObservableCollection<Countries> nationality;

        private string idFilterText = string.Empty;
        private string nameFilterText = string.Empty;
        private string entry_date_from_FilterText = string.Empty;
        private string entry_date_to_FilterText = string.Empty;


        private string source_file_date_from_FilterText = string.Empty;
        private string source_file_date_to_FilterText = string.Empty;
        private CountryService countryService;
        private SecurityService securityService;
        private BrokersService brokersService;
        /// <summary>
        /// Initializes a new instance of the <see cref="DemoWindow"/> class.
        /// </summary>
        public DemoWindow()
        {
            InitializeComponent();
            this.DataContext = DemoCustomerProvider.shared;
            InitializeServices();
            LoadLists();
            SetupDataBindings();
            DemoCustomerProvider.shared.PropertyChanged += Shared_PropertyChanged;
            SetupTimer();
        }


        private void InitializeServices()
        {
            countryService = new CountryService();
            securityService = new SecurityService();
            brokersService = new BrokersService();
        }

        private void LoadLists()
        {
            brokers = CollectionLoader.LoadCollection<Broker>(brokersService.ListBrokers);
            securityCodes = CollectionLoader.LoadCollection<Security>(securityService.ListSecurity);
            nationality = CollectionLoader.LoadCollection<Countries>(countryService.ListCountries);
            UniqueBrokerCodes = brokers.Select(b => b.Code).Distinct().ToList();
        }

        private void SetupDataBindings()
        {
            DataContext = this;
        }

        private void SetupTimer()
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (sender, e) => UpdateMemoryUsage();
            timer.Start();
        }

        private void UpdateMemoryUsage()
        {
            tbMemory.Text = $"{GC.GetTotalMemory(true) / 1024.0 / 1024.0:0.00} MB";
        }

        private void Shared_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FetchRangeResult.Count))
            {
                UpdateResultCount();
            }
        }

        private void UpdateResultCount()
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                tbResultCount.Text = DemoCustomerProvider.shared.Count.ToString();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    tbResultCount.Text = DemoCustomerProvider.shared.Count.ToString();
                });
            }
        }


        private void OnFilterComboBoxSelectionChanged_SecurityCode(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BrokerCodeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateComboBox(sender as ComboBox, brokers.Select(b => b.Code).Distinct().ToList());
        }

        private void SecurityCodeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateComboBox(sender as ComboBox, securityCodes.Select(s => s.Code).Distinct().ToList());
        }

        private void PopulateComboBox(ComboBox comboBox, List<string> items)
        {
            if (comboBox != null)
            {
                comboBox.ItemsSource = items;
            }
        }



        private void OnFilterSelectionChanged_Broker_Code(object sender, SelectionChangedEventArgs e)
        {

        }


        private void timer_Tick(object sender, EventArgs e)
        {
            tbMemory.Text = string.Format("{0:0.00} MB", GC.GetTotalMemory(true) / 1024.0 / 1024.0);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (chkHistory.IsChecked == true)
            {
                ActionsFilter filter = fillFilters();
               // int numItems = int.Parse(tbNumItems.Text);
                int fetchDelay = int.Parse(tbFetchDelay.Text);
                int pageSize = DemoCustomerProvider.GetCount2(filter, ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString);

                int numItems = pageSize;
                DemoCustomerProvider customerProvider = new DemoCustomerProvider(numItems, fetchDelay); //SHAIMA
              

               int pageTimeout = 30;
  
                DataContext = new AsyncVirtualizingCollection<Customer>(customerProvider, pageSize, pageTimeout * 1000, filter);
            }

            else if (chkActiveActions.IsChecked==true) {


                ActionsFilter filter = fillFilters();
                int pageSize = DemoCustomerProvider.GetCount2(filter, ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString);

                int numItems = pageSize;
                int fetchDelay = int.Parse(tbFetchDelay.Text);
                DemoCustomerProvider customerProvider = new DemoCustomerProvider(numItems, fetchDelay);
             
                int pageTimeout = 30;
                DataContext = new AsyncVirtualizingCollection<Customer>(customerProvider, pageSize, pageTimeout * 1000, filter);

            }




        }


        private void OnStatusFilterTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            idFilterText = textBox.Text;
            ApplyFilters();
        }

        private void OnNameFilterTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            nameFilterText = textBox.Text;
            ApplyFilters();
        }

        private void chkHistory_Checked(object sender, RoutedEventArgs e)
        {
            spHistoryOptions.Visibility = Visibility.Visible;
            chkActiveActions.IsChecked = false;
        }

        private void chkHistory_Unchecked(object sender, RoutedEventArgs e)
        {
            spHistoryOptions.Visibility = Visibility.Collapsed;
        }




        private void chkActiveActions_Checked(object sender, RoutedEventArgs e)
        {
            if (chkHistory!=null)
            chkHistory.IsChecked = false;

            if (chkHistory != null)
                chkToday.Visibility = Visibility.Visible;
            if (chkCurrentYear != null)
                chkCurrentYear.Visibility = Visibility.Visible;
        }

        private void chkActiveActions_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkHistory != null)
                chkToday.Visibility = Visibility.Collapsed;
            if (chkCurrentYear != null)
                chkCurrentYear.Visibility = Visibility.Collapsed;
        }




        private void chkToday_Checked(object sender, RoutedEventArgs e)
        {
            if (chkCurrentYear != null)
                chkCurrentYear.IsChecked = false;
        }

        private void chkToday_Unchecked(object sender, RoutedEventArgs e)
        {
          
        }


        private void chkCurrentYear_Checked(object sender, RoutedEventArgs e)
        {
            chkToday.IsChecked = false;
        }

        private void chkCurrentYear_Unchecked(object sender, RoutedEventArgs e)
        {

        }






        private void ApplyFilters()
        {
            ActionsFilter filter = fillFilters();
            FetchFilteredData(filter);
        }

        private void OnStatusFilterChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                idFilterText = selectedItem.Tag.ToString();
                ApplyFilters();
            }
        }

        private async void FetchFilteredData(ActionsFilter filter)
        {
            int pageSize = DemoCustomerProvider.GetCount2(filter, ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString);

            int numItems = pageSize;
            int fetchDelay = int.Parse(tbFetchDelay.Text);
            DemoCustomerProvider customerProvider = new DemoCustomerProvider(numItems, fetchDelay);

          
            int pageTimeout = 30;

            DataContext = new AsyncVirtualizingCollection<Customer>(customerProvider, pageSize, pageTimeout * 1000, filter);

        }


        private void DateIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowPopup(sender as Image, "FromDatePopup", "ToDatePopup");
        }

       

        private void ToDateIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            if (image != null)
            {
                // Find the parent StackPanel
                var stackPanel = FindAncestor<StackPanel>(image);
                if (stackPanel != null)
                {
                    // Find the Popup in the StackPanel's children
                    var popup = FindChild<Popup>(stackPanel, "ToDatePopup");
                    if (popup != null)
                    {
                        popup.IsOpen = true;
                    }
                }
            }
        }

        private void FromDateCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var calendar = sender as System.Windows.Controls.Calendar;
            var selectedDate = calendar?.SelectedDate;

            if (selectedDate.HasValue)
            {
                // Get the Popup from the Tag property of the Calendar
                var popup = calendar.Tag as Popup;

                if (popup != null)
                {
                    // Find the StackPanel that contains the Calendar
                    var stackPanel = FindAncestor<StackPanel>(popup);
                    if (stackPanel != null)
                    {
                        // Find the TextBox for From Date
                        var textBox = FindChild<TextBox>(stackPanel, "FromDateTextBox");
                        if (textBox != null)
                        {
                            textBox.Text = selectedDate.Value.ToString("yyyy/MM/dd");

                            entry_date_from_FilterText = textBox.Text;



                            ApplyFilters();
                        }
                    }

                    // Close the Popup
                    popup.IsOpen = false;
                }
            }
        }

        private void ToDateCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var calendar = sender as System.Windows.Controls.Calendar;
            var selectedDate = calendar?.SelectedDate;

            if (selectedDate.HasValue)
            {
                // Get the Popup from the Tag property of the Calendar
                var popup = calendar.Tag as Popup; // This should work since you set the Tag in XAML

                if (popup != null)
                {
                    // Find the StackPanel that contains the Calendar
                    var stackPanel = FindAncestor<StackPanel>(popup);
                    if (stackPanel != null)
                    {
                        // Find the TextBox for To Date
                        var textBox = FindChild<TextBox>(stackPanel, "ToDateTextBox");
                        if (textBox != null)
                        {
                            textBox.Text = selectedDate.Value.ToString("yyyy/MM/dd");

                            entry_date_to_FilterText = textBox.Text;



                            ApplyFilters();
                        }
                    }

                    // Close the Popup
                    popup.IsOpen = false;
                }
            }
        }



        private void FromDateIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            if (image != null)
            {
                // Find the parent StackPanel
                var stackPanel = FindAncestor<StackPanel>(image);
                if (stackPanel != null)
                {
                    // Find the Popup in the StackPanel's children
                    var popup = FindChild<Popup>(stackPanel, "FromDatePopup");
                    if (popup != null)
                    {
                        popup.IsOpen = true;
                    }
                }
            }
        }


        private void ValidityFromDateIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
        private void ValidityToDateIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ValidityFromDateCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ValidityToDateCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FromDate_source_file_Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {

            //shaima
            var calendar = sender as System.Windows.Controls.Calendar;
            var selectedDate = calendar?.SelectedDate;

            if (selectedDate.HasValue)
            {
                // Get the Popup from the Tag property of the Calendar
                var popup = calendar.Tag as Popup;

                if (popup != null)
                {
                    // Find the StackPanel that contains the Calendar
                    var stackPanel = FindAncestor<StackPanel>(popup);
                    if (stackPanel != null)
                    {
                        // Find the TextBox for From Date
                        var textBox = FindChild<TextBox>(stackPanel, "source_file_FromDateTextBox");
                        if (textBox != null)
                        {
                            textBox.Text = selectedDate.Value.ToString("yyyy/MM/dd");

                            source_file_date_from_FilterText = selectedDate.Value.ToString(Constants.dateFormat);



                            // ApplyFilters();
                        }
                    }

                    // Close the Popup
                    popup.IsOpen = false;
                }
            }
        }

        private void ToDate_source_file_Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {

            //shaima
            var calendar = sender as System.Windows.Controls.Calendar;
            var selectedDate = calendar?.SelectedDate;

            if (selectedDate.HasValue)
            {
                // Get the Popup from the Tag property of the Calendar
                var popup = calendar.Tag as Popup; // This should work since you set the Tag in XAML

                if (popup != null)
                {
                    // Find the StackPanel that contains the Calendar
                    var stackPanel = FindAncestor<StackPanel>(popup);
                    if (stackPanel != null)
                    {
                        // Find the TextBox for To Date
                        var textBox = FindChild<TextBox>(stackPanel, "source_file_ToDateTextBox");
                        if (textBox != null)
                        {
                            textBox.Text = selectedDate.Value.ToString("yyyy/MM/dd");

                            source_file_date_to_FilterText = selectedDate.Value.ToString(Constants.dateFormat);



                            // ApplyFilters();
                        }
                    }

                    // Close the Popup
                    popup.IsOpen = false;
                }
            }
        }

        private void source_file_FromDateIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            if (image != null)
            {
                // Find the parent StackPanel
                var stackPanel = FindAncestor<StackPanel>(image);
                if (stackPanel != null)
                {
                    // Find the Popup in the StackPanel's children
                    var popup = FindChild<Popup>(stackPanel, "source_file_FromDatePopup");
                    if (popup != null)
                    {
                        popup.IsOpen = true;
                    }
                }
            }
        }

        private void source_file_ToDateIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            if (image != null)
            {
                // Find the parent StackPanel
                var stackPanel = FindAncestor<StackPanel>(image);
                if (stackPanel != null)
                {
                    // Find the Popup in the StackPanel's children
                    var popup = FindChild<Popup>(stackPanel, "source_file_ToDatePopup");
                    if (popup != null)
                    {
                        popup.IsOpen = true;
                    }
                }
            }
        }

        // Helper method to find the ancestor of a specific type
        private T FindAncestor<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            while (parentObject != null)
            {
                if (parentObject is T parent)
                {
                    return parent; // Return the found ancestor
                }
                parentObject = VisualTreeHelper.GetParent(parentObject);
            }
            return null; // No ancestor found
        }

        // Helper method to find a child of a specific type by name
        private T FindChild<T>(DependencyObject parent, string childName) where T : FrameworkElement
        {
            // Confirm parent is valid.
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the specified type, continue down the tree
                if (child is T childType)
                {
                    // If the child's name matches, return it
                    if (!string.IsNullOrEmpty(childName))
                    {
                        if ((child as FrameworkElement)?.Name == childName)
                        {
                            foundChild = childType;
                            break;
                        }
                    }
                    else
                    {
                        foundChild = childType;
                        break;
                    }
                }

                // Recursively drill down the tree
                foundChild = FindChild<T>(child, childName);
                if (foundChild != null) break;
            }

            return foundChild;
        }

        private void OnFilterTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            string filterText = textBox.Text;
            int pageSize = DemoCustomerProvider.GetCount2(new ActionsFilter(), ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString);

            int numItems = pageSize;



            int fetchDelay = int.Parse(tbFetchDelay.Text);
            DemoCustomerProvider customerProvider = new DemoCustomerProvider(numItems, fetchDelay);
           
            int pageTimeout = 30;

            DataContext = new AsyncVirtualizingCollection<Customer>(customerProvider, pageSize, 
                pageTimeout * 1000, new ActionsFilter());

        }


        private void OnFilterSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox?.SelectedItem is ComboBoxItem selectedItem)
            {
                int selectedValue = (int)selectedItem.Tag;
            }
        }

        private void OnFilterSelectionChanged_Notionality(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NotionalityComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var uniquenationality = nationality.Select(b => b.Cntry_Nat_A).Distinct().ToList();
                comboBox.ItemsSource = uniquenationality;
            }
        }


        private void dg1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ActionsFilter filter = new ActionsFilter();
            var selectedItem = lvStyle.SelectedItem as Customer;

            if (selectedItem != null)
            {
                string chosenName = selectedItem.Name;

                lvStyle2.ItemsSource = DemoCustomerProvider.shared.AllRecords
                   .Where(customer => customer.Name == chosenName).ToList();

            }

        }


        #region UTILIES

        private ActionsFilter fillFilters()
        {
            ActionsFilter filter = new ActionsFilter();
            filter.status = idFilterText;
            filter.entry_date_from_FilterText = entry_date_from_FilterText;
            filter.entry_date_to_FilterText = entry_date_to_FilterText;
            filter.Name = nameFilterText;
            filter.source_file_date_from_FilterText = source_file_date_from_FilterText;
            filter.source_file_date_to_FilterText = source_file_date_to_FilterText;
            filter.isHistory = (bool)chkHistory.IsChecked;


            filter.isToday = (bool)chkToday.IsChecked;


            return filter;
        }


        private void ShowPopup(Image image, string fromPopupName, string toPopupName)
        {
            if (image == null) return;

            var stackPanel = FindAncestor<StackPanel>(image);
            if (stackPanel == null) return;

            var popup = FindChild<Popup>(stackPanel, fromPopupName) ?? FindChild<Popup>(stackPanel, toPopupName);
            popup?.SetCurrentValue(Popup.IsOpenProperty, true);
        }

        #endregion

    }
}
