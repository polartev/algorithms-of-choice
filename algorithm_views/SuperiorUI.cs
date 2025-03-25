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
    class SuperiorUI : AlgorithmView
    {
        private Superior superiorModel;

        public SuperiorUI(AlgorithmModel algorithmModel, StackPanel leftPanel, StackPanel rightPanel) : base(algorithmModel, leftPanel, rightPanel)
        {
            superiorModel = (Superior)algorithmModel;
        }

        protected override void InitializeDataGrid(DataGrid dataGrid, int columns = 3, int rows = 0)
        {
            superiorModel.dataGrids.Clear();

            base.InitializeDataGrid(dataGrid, columns, rows);

            for (int row = 0; row < superiorModel.totalExperts; row++)
                for (int column = 0; column < superiorModel.totalAlternatives; column++)
                    superiorModel.dataGrids[0][row].Values[column].onUpdateEvent += BuildRightSide;
        }

        private void ChangeTotalExpert(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.Name == dataGrids[0].Name + "_button_plus1")
                superiorModel.totalExperts++;
            else
                superiorModel.totalExperts--;

            InitializeDataGrid(dataGrids[0], superiorModel.totalAlternatives, superiorModel.totalExperts);
        }

        private void ChangeTotalAlternative(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.Name == dataGrids[0].Name + "_button_minus2")
                superiorModel.totalAlternatives--;
            else
                superiorModel.totalAlternatives++;

            InitializeDataGrid(dataGrids[0], superiorModel.totalAlternatives, superiorModel.totalExperts);
        }

        public override void BuildLeftSide()
        {
            base.BuildLeftSide();

            dataGrids.Add(GenerateDataGrid());

            dataGrids[0].Margin = new Thickness(5, 5, 20, 20);
            InitializeDataGrid(dataGrids[0], superiorModel.totalAlternatives, superiorModel.totalExperts);

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
                superiorModel.Calculate();

                base.BuildRightSide();

                rightPanel.Children.Add(new TextBlock
                {
                    Text = "Модифікована матриця переваги:",
                    Background = Brushes.White,
                    Margin = new Thickness(20, 20, 0, 5),
                    FontSize = 16,
                });

                for (int row = 0; row < superiorModel.totalExperts; row++)
                {
                    string text = "";
                    for (int column = 0; column < superiorModel.totalAlternatives; column++)
                        text += $"\t{Math.Round(superiorModel.converted[row][column]).ToString()}";
                    rightPanel.Children.Add(new TextBlock
                    {
                        Text = text,
                        Margin = new Thickness(24, 4, 0, 0),
                        FontSize = 15,
                    });
                }

                rightPanel.Children.Add(new TextBlock
                {
                    Text = "Сумарні оцінки переваги:",
                    Background = Brushes.White,
                    Margin = new Thickness(20, 20, 0, 5),
                    FontSize = 16,
                });

                for (int grade = 0; grade < superiorModel.totalAlternatives; grade++)
                    rightPanel.Children.Add(new TextBlock
                    {
                        Text = "K [" + (grade + 1).ToString() + "] = " + Math.Round(superiorModel.grade[grade], 2).ToString(),
                        Margin = new Thickness(24, 4, 0, 0),
                        FontSize = 15,
                    });

                rightPanel.Children.Add(new TextBlock
                {
                    Text = "Шукані ваги цілей:",
                    Background = Brushes.White,
                    Margin = new Thickness(20, 20, 0, 5),
                    FontSize = 16,
                });

                for (int weight = 0; weight < superiorModel.totalAlternatives; weight++)
                    rightPanel.Children.Add(new TextBlock
                    {
                        Text = "W [" + (weight + 1).ToString() + "] = " + Math.Round(superiorModel.weight[weight], 2).ToString(),
                        Margin = new Thickness(24, 4, 0, 0),
                        FontSize = 15,
                    });

                rightPanel.Children.Add(new TextBlock
                {
                    Text = "Найкращою альтернативою є: A[" + (superiorModel.weight.IndexOf(superiorModel.weight.Max()) + 1).ToString() + "]",
                    Background = Brushes.White,
                    Margin = new Thickness(20, 20, 0, 0),
                    FontSize = 16,
                });

                double sum = superiorModel.weight.Sum();
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
