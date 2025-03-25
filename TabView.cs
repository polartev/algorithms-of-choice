using Labs_1_3.algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Labs_1_3
{
    class TabView
    {
        private event RoutedEventHandler? deleteTab;

        public event RoutedEventHandler DeleteTab
        {
            add
            {
                if(deleteTab == null)
                    deleteTab += value;
            }
            remove => deleteTab -= value;
        }

        private TabModel tabModel;

        private AlgorithmView algorithmView;

        private TabControl tabControl;

        private TabItem? tabItem;
        private StackPanel? headerPanel;
        private TextBlock? headerText;
        private Button? headerButton;

        private Grid? workLayout;
        private ScrollViewer? scrollViewerLeft;
        private StackPanel? leftPanel;
        private GridSplitter? gridSplitter;
        private ScrollViewer? scrollViewerRight;
        private StackPanel? rightPanel;

        public TabView(TabControl tabControl, TabModel tabModel)
        {
            this.tabControl = tabControl;
            this.tabModel = tabModel;
        }

        public void InitializeComponent()
        {
            buildTabItem();
            algorithmViewInitialize();
        }

        private void algorithmViewInitialize()
        {
            switch (tabModel.algorithmModel.algorithmName)
            {
                case "Алгоритм Сааті":
                    algorithmView = new SaatiUI(tabModel.algorithmModel, leftPanel, rightPanel);
                    break;
                case "Алгоритм парних порівнянь":
                    algorithmView = new PairComparisonUI(tabModel.algorithmModel, leftPanel, rightPanel);
                    break;
                case "Метод зважування оцінок":
                    algorithmView = new WeightUI(tabModel.algorithmModel, leftPanel, rightPanel);
                    break;
                case "Метод переваги":
                    algorithmView = new SuperiorUI(tabModel.algorithmModel, leftPanel, rightPanel);
                    break;
                case "Метод ранга":
                    algorithmView = new RangUI(tabModel.algorithmModel, leftPanel, rightPanel);
                    break;
                default: throw new ArgumentException("Невідомий алгоритм!");
            }
            algorithmView.BuildLeftSide();
        }

        private void buildTabItem()
        {
            int index = 1;
            string tabName = "";
            do
            {
                tabName = "_file" + index.ToString();
                index++;
            }
            while (tabControl.Items.OfType<TabItem>().Any(item => ((TabItem)item).Name == tabName));

            tabItem = new TabItem { Name = tabName };
            tabModel.itemName = tabItem.Name;

            headerPanel = new StackPanel { Orientation = Orientation.Horizontal };

            headerText = new TextBlock { Text = tabItem.Name, Margin = new Thickness(3) };

            headerButton = new Button { MaxHeight = 16, MaxWidth = 16 };
            headerButton.Click += deleteTab;

            Image closeIcon = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/resources/images/cross_icon.png"))
            };

            headerButton.Content = closeIcon;

            headerPanel.Children.Add(headerText);
            headerPanel.Children.Add(headerButton);

            tabItem.Header = headerPanel;

            workLayout = new Grid { Background = new SolidColorBrush(Color.FromRgb(177, 174, 174)) };
            workLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) } );
            workLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto } );
            workLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) } );

            scrollViewerLeft = new ScrollViewer();
            Grid.SetColumn(scrollViewerLeft, 0);
            Panel.SetZIndex(scrollViewerLeft, 1);

            leftPanel = new StackPanel { Name = "leftSide" };
            scrollViewerLeft.Content = leftPanel;

            gridSplitter = new GridSplitter 
            {
                Width = 3,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Stretch,
                ShowsPreview = false
            };
            Grid.SetColumn(gridSplitter, 1);

            scrollViewerRight = new ScrollViewer();
            Grid.SetColumn(scrollViewerRight, 2);
            Panel.SetZIndex(scrollViewerRight, 0);

            rightPanel = new StackPanel { Name = "rightSide" };
            scrollViewerRight.Content = rightPanel;

            workLayout.Children.Add(scrollViewerLeft);
            workLayout.Children.Add(gridSplitter);
            workLayout.Children.Add(scrollViewerRight);

            tabItem.Content = workLayout;
            tabControl.Items.Add(tabItem);
        }
    }
}
