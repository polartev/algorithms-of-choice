using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Labs_1_3.algorithms
{
    class SaatiUI : AlgorithmView
    {
        private Saati saatiModel;

        public SaatiUI(AlgorithmModel algorithmModel, StackPanel leftPanel, StackPanel rightPanel) : base(algorithmModel, leftPanel, rightPanel) 
        {
            saatiModel = (Saati)algorithmModel;
        }

        protected override void InitializeDataGrid(DataGrid dataGrid, int columns = 3, int rows = 0)
        {
            saatiModel.dataGrids.Clear();

            base.InitializeDataGrid(dataGrid, columns, rows);

            if (saatiModel.totalAlternatives > 10)
                InitializeDataGrid(dataGrid, 10);
            else
                for (int row = 0; row < saatiModel.totalAlternatives; row++)
                    for (int column = 0; column < saatiModel.totalAlternatives; column++)
                    {
                        saatiModel.dataGrids[0][row].Values[column].onUpdateEvent += saatiModel.FillDataGrid;
                        saatiModel.dataGrids[0][row].Values[column].onUpdateEvent += BuildRightSide;
                    }
        }

        private void ChangeTotalAlternative(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.Name == dataGrids[0].Name + "_button_minus")
                InitializeDataGrid(dataGrids[0], saatiModel.totalAlternatives - 1);
            else
                InitializeDataGrid(dataGrids[0], saatiModel.totalAlternatives + 1);
        }

        public override void BuildLeftSide()
        {
            base.BuildLeftSide();

            dataGrids.Add(GenerateDataGrid());

            dataGrids[0].Margin = new Thickness(5, 20, 20, 0);
            InitializeDataGrid(dataGrids[0]);

            DockPanel rowPanel = new DockPanel 
            {
                LastChildFill = true
            };

            Image minusIcon = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/resources/images/minus_icon.png"))
            };
            Button button_minus = new Button
            {
                Name = dataGrids[0].Name + "_button_minus",
                Content = minusIcon,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(1, 20, 0, 0),
                MaxHeight = 24,
                MaxWidth = 24
            };
            button_minus.Click += ChangeTotalAlternative;
            rowPanel.Children.Add(button_minus);

            Image plusIcon = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/resources/images/plus_icon.png"))
            };
            Button button_plus = new Button
            {
                Name = dataGrids[0].Name + "_button_plus",
                Content = plusIcon,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(1, 20, 0, 0),
                MaxHeight = 24,
                MaxWidth = 24
            }; 
            button_plus.Click += ChangeTotalAlternative;
            rowPanel.Children.Add(button_plus);

            rowPanel.Children.Add(dataGrids[0]);
            leftPanel.Children.Add(rowPanel);
        }

        public override void BuildRightSide()
        {
            rightPanel.Children.Clear();
            try
            {
                saatiModel.Calculate();

                base.BuildRightSide();

                rightPanel.Children.Add(new TextBlock
                {
                    Text = "Ціни альтернатив:",
                    Background = Brushes.White,
                    Margin = new Thickness(20, 20, 0, 5),
                    FontSize = 16,
                });

                for(int price = 0; price < saatiModel.totalAlternatives; price++)
                    rightPanel.Children.Add(new TextBlock
                    {
                        Text = "C [" + (price + 1).ToString() + "] = " + Math.Round(saatiModel.price[price], 2).ToString(),
                        Margin = new Thickness(24, 4, 0, 0),
                        FontSize = 15,
                    });
                
                rightPanel.Children.Add(new TextBlock
                {
                    Text = "Сума цін альтернатив:",
                    Background = Brushes.White,
                    Margin = new Thickness(20, 20, 0, 5),
                    FontSize = 16,
                });

                rightPanel.Children.Add(new TextBlock
                {
                    Text = "C = " + Math.Round(saatiModel.price.Sum(), 2).ToString(),
                    Margin = new Thickness(24, 4, 0, 0),
                    FontSize = 15,
                });

                rightPanel.Children.Add(new TextBlock
                {
                    Text = "Ваги альтернатив:",
                    Background = Brushes.White,
                    Margin = new Thickness(20, 20, 0, 5),
                    FontSize = 16,
                });

                for(int weight = 0; weight < saatiModel.totalAlternatives; weight++)
                    rightPanel.Children.Add(new TextBlock
                    {
                        Text = "V [" + (weight + 1).ToString() + "] = " + Math.Round(saatiModel.weight[weight], 3).ToString(),
                        Margin = new Thickness(24, 4, 0, 0),
                        FontSize = 15,
                    });

                rightPanel.Children.Add(new TextBlock
                {
                    Text = "Найкращою альтернативою є: A[" + (saatiModel.weight.IndexOf(saatiModel.weight.Max()) + 1).ToString() + "]",
                    Background = Brushes.White,
                    Margin = new Thickness(20, 20, 0, 0),
                    FontSize = 16,
                });

                rightPanel.Children.Add(new TextBlock
                {
                    Text = saatiModel.CheckExpertsGrades(),
                    Background = Brushes.White,
                    Margin = new Thickness(20, 0, 0, 5),
                    FontSize = 16,
                });
            }
            catch { }
        }
    }
}
