using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Labs_1_3.algorithms
{
    class PairComparisonUI : AlgorithmView
    {
        private PairComparison pairCompModel;

        private StackPanel gridChangePanel;

        public PairComparisonUI(AlgorithmModel algorithmModel, StackPanel leftPanel, StackPanel rightPanel) : base(algorithmModel, leftPanel, rightPanel) 
        {
            pairCompModel = (PairComparison)algorithmModel;
        }

        protected override void InitializeDataGrid(DataGrid dataGrid, int columns = 3, int rows = 0)
        {
            base.InitializeDataGrid(dataGrid, columns, rows);
            int current = Convert.ToInt32(Regex.Match(dataGrid.Name, @"\d+").Value);

            for (int row = 0; row < pairCompModel.totalAlternatives; row++)
                for (int column = 0; column < pairCompModel.totalAlternatives; column++)
                {
                    pairCompModel.dataGrids[current][row].Values[column].onUpdateEvent += pairCompModel.FillDataGrid;
                    pairCompModel.dataGrids[current][row].Values[column].onUpdateEvent += BuildRightSide;
                }
        }

        private void ChangeTotalExpert(object sender, RoutedEventArgs e)
        {
            leftPanel.Children.Remove(gridChangePanel);

            Button button = (Button)sender;

            if (button.Name == "button_add")
            {
                pairCompModel.totalExperts++;
                dataGrids.Add(GenerateDataGrid());
                dataGrids.Last().Margin = new Thickness(55, 20, 20, 0);
                InitializeDataGrid(dataGrids.Last(), pairCompModel.totalAlternatives);
                leftPanel.Children.Add(dataGrids.Last());

            }
            else if (pairCompModel.totalExperts > 2)
            {
                pairCompModel.totalExperts--;
                leftPanel.Children.Remove(leftPanel.Children[leftPanel.Children.Count - 1]);
                pairCompModel.dataGrids.Remove(pairCompModel.dataGrids.Last());
                dataGrids.Remove(dataGrids.Last());
            }
            leftPanel.Children.Add(gridChangePanel);
        }

        private void ChangeTotalAlternative(object sender, RoutedEventArgs e)
        {
            pairCompModel.dataGrids.Clear();

            Button button = (Button)sender;

            if (button.Name == dataGrids[0].Name + "_button_minus")
                pairCompModel.totalAlternatives--;
            else
                pairCompModel.totalAlternatives++;

            for (int grid = 0; grid < pairCompModel.totalExperts; grid++)
                InitializeDataGrid(dataGrids[grid], pairCompModel.totalAlternatives);
        }

        public override void BuildLeftSide()
        {
            base.BuildLeftSide();

            dataGrids.Add(GenerateDataGrid());

            dataGrids.Last().Margin = new Thickness(5, 20, 20, 0);
            InitializeDataGrid(dataGrids.Last());

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

            rowPanel.Children.Add(dataGrids.Last());
            leftPanel.Children.Add(rowPanel);

            dataGrids.Add(GenerateDataGrid());

            dataGrids.Last().Margin = new Thickness(55, 20, 20, 0);
            InitializeDataGrid(dataGrids.Last());
            leftPanel.Children.Add(dataGrids.Last());

            gridChangePanel = new StackPanel();

            Image removeIcon = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/resources/images/minus_icon.png"))
            };
            Button removeButton = new Button
            {
                Name = "button_remove",
                Content = removeIcon,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 0),
                MaxHeight = 46,
                MaxWidth = 46
            };
            removeButton.Click += ChangeTotalExpert;

            Image addIcon = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/resources/images/plus_icon.png"))
            };
            Button addButton = new Button
            {
                Name = "button_add",
                Content = addIcon,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 2, 0, 20),
                MaxHeight = 46,
                MaxWidth = 46
            };
            addButton.Click += ChangeTotalExpert;

            gridChangePanel.Children.Add(removeButton);
            gridChangePanel.Children.Add(addButton);
            leftPanel.Children.Add(gridChangePanel);
        }

        public override void BuildRightSide()
        {
            rightPanel.Children.Clear();
            try
            {
                pairCompModel.Calculate();

                base.BuildRightSide();

                rightPanel.Children.Add(new TextBlock
                {
                    Text = "Думки кожного з експертів:",
                    Background = Brushes.White,
                    Margin = new Thickness(20, 20, 0, 5),
                    FontSize = 16,
                });

                for (int grid = 0; grid < pairCompModel.totalExperts; grid++)
                {
                    string text = "";
                    for (int grade = 0; grade < pairCompModel.totalAlternatives; grade++)
                        text += $"\t{Math.Round(pairCompModel.grade[grid][grade], 1).ToString()}";

                    rightPanel.Children.Add(new TextBlock
                    {
                        Text = "E [" + (grid + 1).ToString() + "]: " + text,
                        Margin = new Thickness(24, 4, 0, 0),
                        FontSize = 15,
                    });
                }

                rightPanel.Children.Add(new TextBlock
                {
                    Text = "Узагальнені оцінки переваг альтернатив:",
                    Background = Brushes.White,
                    Margin = new Thickness(20, 20, 0, 5),
                    FontSize = 16,
                });

                for (int general = 0; general < pairCompModel.totalAlternatives; general++)
                    rightPanel.Children.Add(new TextBlock
                    {
                        Text = "C [" + (general + 1).ToString() + "] = " + Math.Round(pairCompModel.general[general], 1).ToString(),
                        Margin = new Thickness(24, 4, 0, 0),
                        FontSize = 15,
                    });

                rightPanel.Children.Add(new TextBlock
                {
                    Text = "Сума всіх оцінок:",
                    Background = Brushes.White,
                    Margin = new Thickness(20, 20, 0, 5),
                    FontSize = 16,
                });

                rightPanel.Children.Add(new TextBlock
                {
                    Text = "C = " + Math.Round(pairCompModel.general.Sum(), 2).ToString(),
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

                for (int weight = 0; weight < pairCompModel.totalAlternatives; weight++)
                    rightPanel.Children.Add(new TextBlock
                    {
                        Text = "V [" + (weight + 1).ToString() + "] = " + Math.Round(pairCompModel.weight[weight], 2).ToString(),
                        Margin = new Thickness(24, 4, 0, 0),
                        FontSize = 15,
                    });

                rightPanel.Children.Add(new TextBlock
                {
                    Text = "Найкращою альтернативою є: A[" + (pairCompModel.weight.IndexOf(pairCompModel.weight.Max()) + 1).ToString() + "]",
                    Background = Brushes.White,
                    Margin = new Thickness(20, 20, 0, 0),
                    FontSize = 16,
                });
            }
            catch { }
        }
    }
}