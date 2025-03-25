using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Labs_1_3.algorithms
{
    class RangUI : AlgorithmView
    {
        private Rang rangModel;

        public RangUI(AlgorithmModel algorithmModel, StackPanel leftPanel, StackPanel rightPanel) : base(algorithmModel, leftPanel, rightPanel) 
        {
            rangModel = (Rang)algorithmModel;
        }

        protected override void InitializeDataGrid(DataGrid dataGrid, int columns = 3, int rows = 0)
        {
            rangModel.dataGrids.Clear();

            base.InitializeDataGrid(dataGrid, columns, rows);

            for (int row = 0; row < rangModel.totalExperts; row++)
                for (int column = 0; column < rangModel.totalAlternatives; column++)
                    rangModel.dataGrids[0][row].Values[column].onUpdateEvent += BuildRightSide;
        }

        private void ChangeTotalExpert(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.Name == dataGrids[0].Name + "_button_plus1")
                rangModel.totalExperts++;
            else
                rangModel.totalExperts--;

            InitializeDataGrid(dataGrids[0], rangModel.totalAlternatives, rangModel.totalExperts);
        }

        private void ChangeTotalAlternative(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.Name == dataGrids[0].Name + "_button_minus2")
                rangModel.totalAlternatives--;
            else
                rangModel.totalAlternatives++;

            InitializeDataGrid(dataGrids[0], rangModel.totalAlternatives, rangModel.totalExperts);
        }

        public override void BuildLeftSide()
        {
            base.BuildLeftSide();

            dataGrids.Add(GenerateDataGrid());

            dataGrids[0].Margin = new Thickness(5, 5, 20, 20);
            InitializeDataGrid(dataGrids[0], rangModel.totalAlternatives, rangModel.totalExperts);

            DockPanel rowPanel = new DockPanel
            {
                LastChildFill = true
            };

            Image minusIcon1 = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/resources/images/minus_icon.png"))
            };
            Button button_minus1 = new Button
            {
                Name = dataGrids[0].Name + "_button_minus1",
                Content = minusIcon1,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(59, 5, 0, 0),
                MaxHeight = 24,
                MaxWidth = 24
            };
            DockPanel.SetDock(button_minus1, Dock.Top);
            button_minus1.Click += ChangeTotalExpert;
            rowPanel.Children.Add(button_minus1);

            Image plusIcon1 = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/resources/images/plus_icon.png"))
            };
            Button button_plus1 = new Button
            {
                Name = dataGrids[0].Name + "_button_plus1",
                Content = plusIcon1,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(59, 1, 0, 0),
                MaxHeight = 24,
                MaxWidth = 24
            };
            DockPanel.SetDock(button_plus1, Dock.Top);
            button_plus1.Click += ChangeTotalExpert;
            rowPanel.Children.Add(button_plus1);

            Image minusIcon2 = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/resources/images/minus_icon.png"))
            };
            Button button_minus2 = new Button
            {
                Name = dataGrids[0].Name + "_button_minus2",
                Content = minusIcon2,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(5, 5.5, 0, 0),
                MaxHeight = 24,
                MaxWidth = 24
            };
            button_minus2.Click += ChangeTotalAlternative;
            rowPanel.Children.Add(button_minus2);

            Image plusIcon2 = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/resources/images/plus_icon.png"))
            };
            Button button_plus2 = new Button
            {
                Name = dataGrids[0].Name + "_button_plus2",
                Content = plusIcon2,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(1, 5.5, 0, 0),
                MaxHeight = 24,
                MaxWidth = 24
            };
            button_plus2.Click += ChangeTotalAlternative;
            rowPanel.Children.Add(button_plus2);

            rowPanel.Children.Add(dataGrids[0]);
            leftPanel.Children.Add(rowPanel);
        }

        public override void BuildRightSide()
        {
            rightPanel.Children.Clear();
            try
            {
                rangModel.Calculate();

                base.BuildRightSide();

                rightPanel.Children.Add(new TextBlock
                {
                    Text = "Матриця нормованих оцінок:",
                    Background = Brushes.White,
                    Margin = new Thickness(20, 20, 0, 5),
                    FontSize = 16,
                });

                for (int row = 0; row < rangModel.totalExperts; row++)
                {
                    string text = "";
                    for (int column = 0; column < rangModel.totalAlternatives; column++)
                        text += $"\t{rangModel.normalized[row][column]}";
                    rightPanel.Children.Add(new TextBlock
                    {
                        Text = text,
                        Margin = new Thickness(24, 4, 0, 0),
                        FontSize = 15,
                    });
                }

                rightPanel.Children.Add(new TextBlock
                {
                    Text = "Шукані ваги цілей:",
                    Background = Brushes.White,
                    Margin = new Thickness(20, 20, 0, 5),
                    FontSize = 16,
                });

                for (int weight = 0; weight < rangModel.totalAlternatives; weight++)
                    rightPanel.Children.Add(new TextBlock
                    {
                        Text = "W [" + (weight + 1).ToString() + "] = " + Math.Round(rangModel.weight[weight], 3).ToString(),
                        Margin = new Thickness(24, 4, 0, 0),
                        FontSize = 15,
                    });

                rightPanel.Children.Add(new TextBlock
                {
                    Text = "Найкращою альтернативою є: A[" + (rangModel.weight.IndexOf(rangModel.weight.Max()) + 1).ToString() + "]",
                    Background = Brushes.White,
                    Margin = new Thickness(20, 20, 0, 0),
                    FontSize = 16,
                });

                double sum = rangModel.weight.Sum();
                string check = $"Де сума всіх Wi = {Math.Round(sum, 2)}. ";
                double epsilon = 1e-10;

                if (Math.Abs(sum - 1.0) < epsilon)
                    check += "Таким чином все пораховано вірно.";
                else
                    check += "Щось пішло не так.";

                rightPanel.Children.Add(new TextBlock
                {
                    Text = check,
                    Background = Brushes.White,
                    Margin = new Thickness(20, 0, 0, 0),
                    FontSize = 16,
                });
            }
            catch { }
        }
    }
}
