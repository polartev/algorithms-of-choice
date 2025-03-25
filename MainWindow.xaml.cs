using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Labs_1_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<(TabModel, TabView)> tabs = new List<(TabModel, TabView)>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void createTab(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem menuItem = (MenuItem)sender;

                TabModel tabModel = new TabModel(menuItem.Header.ToString());

                TabView tabView = new TabView(tabControl, tabModel);
                tabView.DeleteTab += deleteTab;
                tabView.InitializeComponent();

                tabs.Add((tabModel, tabView));
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Creation Tab Error!", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void selectTab(object sender, SelectionChangedEventArgs e)
        {
            TabItem tabItem = (TabItem)tabControl.SelectedItem;
            if (tabItem != null)
            {
                var selectedModel = tabs.FindAll(tab => tab.Item1.itemName == tabItem.Name);
                var algorithmModel = selectedModel.First().Item1.algorithmModel;
                if (algorithmModel != null)
                {
                    string? algorithmName = algorithmModel.algorithmName;
                    if (algorithmName != null)
                        currentAlgorithm.Text = algorithmName;
                    else currentAlgorithm.Text = "...";
                }
            }
            else currentAlgorithm.Text = "...";
        }

        private void deleteTab(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            TabItem tabItem = (TabItem)((StackPanel)button.Parent).Parent;
            tabs.RemoveAll(tab => tab.Item1.itemName == tabItem.Name);
            tabControl.Items.Remove(tabItem);
        }
    }
}