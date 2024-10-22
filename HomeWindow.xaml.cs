using DataVirtualization.Modules;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        public HomeWindow()
        {
            InitializeComponent();

            // Make the window full screen
            this.WindowState = WindowState.Maximized;    // Maximizes the window


        }
        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Code to handle saving
        }

        private void RestartMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Code to handle restart
        }
       // private TabItem actionsTab;
        /* private DemoWindow actionsWindow;
         private void ActionsMenuItem_Click(object sender, RoutedEventArgs e)
         {
             // Check if the window is already open
             if (actionsWindow == null || !actionsWindow.IsVisible)
             {
                 // Create a new instance of DemoWindow
                 actionsWindow = new DemoWindow();

                 // Set the title of the new window
                 actionsWindow.Title = "Actions";

                 // Set the size and other properties as needed (optional)
                 actionsWindow.Width = 800;
                 actionsWindow.Height = 600;

                 // Show the window
                 actionsWindow.Show();

                 // Optionally, handle the closing event if needed to nullify the reference when closed
                 actionsWindow.Closed += (s, args) => actionsWindow = null;
             }
             else
             {
                 // If the window already exists, bring it to the foreground
                 actionsWindow.Activate();
             }
         }*/

        private TabItem actionsTab;

        private void ActionsMenuItem_Click(object sender, RoutedEventArgs e)
        {


            if (sender is MenuItem menuItem)
            {
                string actionType = menuItem.Tag.ToString();

                switch (actionType)
                {
                    case "NewTab":
                        // Logic for opening a new tab
                        OpenNewTab();
                        break;

                    case "NewWindow":
                        // Logic for opening a new window
                        OpenNewWindow();
                        break;
                }
            }





         


               


            }

        private void OpenNewWindow()
        {

            // Create a new DemoWindowHost to display the UserControl
            DemoWindowHost demoWindowHost = new DemoWindowHost();

            // Optionally, set DataContext or any other properties
            DemoCustomerProvider customerProvider = new DemoCustomerProvider(30, 30);
            demoWindowHost.DataContext = new AsyncVirtualizingCollection<Customer>(customerProvider, 10, 30 * 1000, new ActionsFilter());

            // Show the window
            demoWindowHost.Show();
        }
    

        private void OpenNewTab()
        {
            // Check if the tab already exists
            // Check if the tab already exists
            if (actionsTab == null)
            {
                // Create a new TabItem
                actionsTab = new TabItem();

                // Create a StackPanel for the header
                StackPanel headerPanel = new StackPanel { Orientation = Orientation.Horizontal };

                // Create the TextBlock for the tab title
                TextBlock tabHeaderText = new TextBlock { Text = "Actions" };

                // Create the close button
                Button closeButton = new Button
                {
                    Content = "X",
                    Width = 20,
                    Height = 20,
                    Margin = new Thickness(5, 0, 0, 0)
                };
                closeButton.Click += CloseButton_Click; // Subscribe to the close button click event

                // Add the title and close button to the header
                headerPanel.Children.Add(tabHeaderText);
                headerPanel.Children.Add(closeButton);

                // Set the header of the TabItem to the StackPanel
                actionsTab.Header = headerPanel;

                // Load DemoWindow's content and place it inside the new TabItem
                DemoWindow demoWindow = new DemoWindow();
                DemoCustomerProvider customerProvider = new DemoCustomerProvider(30, 30);
                // demoWindow.DataContext = new AsyncVirtualizingCollection<Customer>(customerProvider, 10, 30 * 1000, new ActionsFilter());

                // Use a Frame to embed the DemoWindow content inside the tab
                Frame frame = new Frame { Content = demoWindow };
                actionsTab.Content = frame;

                // Add the new TabItem to the TabControl
                MainTabControl.Items.Add(actionsTab);

                // Optionally, make the new tab the selected tab
                MainTabControl.SelectedItem = actionsTab;
            }
            else
            {
                // If it exists, make it the selected tab
                MainTabControl.SelectedItem = actionsTab;
            }
        }

        // Event handler for the close button
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove the tab
            if (actionsTab != null)
            {
                MainTabControl.Items.Remove(actionsTab);
                actionsTab = null; // Clear the reference
            }
        }

        private void ActiveClientsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Code to handle showing Active Clients content
        }



    }
}



/*   private TabItem actionsTab;

        private void ActionsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Check if the tab already exists
            if (actionsTab == null)
            {
                // Create a new TabItem
                actionsTab = new TabItem();

                // Create a StackPanel for the header
                StackPanel headerPanel = new StackPanel { Orientation = Orientation.Horizontal };

                // Create the TextBlock for the tab title
                TextBlock tabHeaderText = new TextBlock { Text = "Actions" };

                // Create the close button
                Button closeButton = new Button
                {
                    Content = "X",
                    Width = 20,
                    Height = 20,
                    Margin = new Thickness(5, 0, 0, 0)
                };
                closeButton.Click += CloseButton_Click; // Subscribe to the close button click event

                // Add the title and close button to the header
                headerPanel.Children.Add(tabHeaderText);
                headerPanel.Children.Add(closeButton);

                // Set the header of the TabItem to the StackPanel
                actionsTab.Header = headerPanel;

                // Load DemoWindow's content and place it inside the new TabItem
                DemoWindow demoWindow = new DemoWindow();
                actionsTab.Content = demoWindow.Content;


       
                DemoCustomerProvider customerProvider = new DemoCustomerProvider(30, 30);
  
        


                demoWindow.DataContext  = new AsyncVirtualizingCollection<Customer>(customerProvider, 10, 30 * 1000, new ActionsFilter());


                // Add the new TabItem to the TabControl
                MainTabControl.Items.Add(actionsTab);

                // Optionally, make the new tab the selected tab
                MainTabControl.SelectedItem = actionsTab;
            }
            else
            {
                // If it exists, make it the selected tab
                MainTabControl.SelectedItem = actionsTab;
            }
        }*/