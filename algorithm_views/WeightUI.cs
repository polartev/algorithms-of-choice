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
    class WeightUI : AlgorithmView
    {
        private Weight weightModel;

        private List<Grid> profGrid = new List<Grid>();

        private List<TextBox> textBoxes = new List<TextBox>();

        public WeightUI(AlgorithmModel algorithmModel, StackPanel leftPanel, StackPanel rightPanel) : base(algorithmModel, leftPanel, rightPanel)
        {
            weightModel = (Weight)algorithmModel;
        }

        protected override void InitializeDataGrid(DataGrid dataGrid, int columns = 3, int rows = 0)
        {
            weightModel.dataGrids.Clear();

            base.InitializeDataGrid(dataGrid, columns, rows);

            for (int row = 0; row < weightModel.totalExperts; row++)
                for (int column = 0; column < weightModel.totalAlternatives; column++)
                    weightModel.dataGrids[0][row].Values[column].onUpdateEvent += BuildRightSide;
        }

        private void ChangeTotalExpert(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.Name == dataGrids[0].Name + "_button_plus1")
                weightModel.totalExperts++;
            else
                weightModel.totalExperts--;

            InitializeDataGrid(dataGrids[0], weightModel.totalAlternatives, weightModel.totalExperts);
            GenerateProfInput(weightModel.totalExperts);
        }

        private void ChangeTotalAlternative(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.Name == dataGrids[0].Name + "_button_minus2")
                weightModel.totalAlternatives--;
            else
                weightModel.totalAlternatives++;

            InitializeDataGrid(dataGrids[0], weightModel.totalAlternatives, weightModel.totalExperts);
        }

        private List<Grid> GenerateProfInput(int count)
        {
            foreach(var element in profGrid)
                leftPanel.Children.Remove(element);

            profGrid.Clear();
            textBoxes.Clear();

            for (int grid = 0; grid < count; grid++)
            {
                profGrid.Add(new Grid());
                profGrid[grid] = new Grid { Margin = new Thickness(59, 0, 20, 0) };
                profGrid[grid].ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                profGrid[grid].ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                TextBlock label = new TextBlock
                {
                    Text = $"R [{grid + 1}]: ",
                    Background = Brushes.White,
                    VerticalAlignment = VerticalAlignment.Center,
                    Padding = new Thickness(5, 0, 0, 0),
                    Margin = new Thickness(1)
                };
                Grid.SetColumn(label, 0);
                profGrid[grid].Children.Add(label);

                TextBox textBox = new TextBox
                {
                    Margin = new Thickness(1),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    MinWidth = 32
                    
                };
                textBox.TextChanged += (s, e) => BuildRightSide();
                Grid.SetColumn(textBox, 1);

                textBoxes.Add(textBox);

                profGrid[grid].Children.Add(textBox);
            }

            for (int grid = 0; grid < count; grid++)
                leftPanel.Children.Add(profGrid[grid]);
            return profGrid;
        }

        public override void BuildLeftSide()
        {
            base.BuildLeftSide();

            dataGrids.Add(GenerateDataGrid());

            dataGrids[0].Margin = new Thickness(5, 5, 20, 20);
            InitializeDataGrid(dataGrids[0], weightModel.totalAlternatives, weightModel.totalExperts);

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

            GenerateProfInput(weightModel.totalExperts);
        }

        public override void BuildRightSide()
        {
            rightPanel.Children.Clear();
            try
            {
                weightModel.prof.Clear();
                foreach (var item in textBoxes)
                    weightModel.prof.Add(Convert.ToDouble(item.Text));

                weightModel.Calculate();

                base.BuildRightSide();

                rightPanel.Children.Add(new TextBlock
                {
                    Text = "Відносні оцінки компетентності експертів:",
                    Background = Brushes.White,
                    Margin = new Thickness(20, 20, 0, 5),
                    FontSize = 16,
                });

                for (int relative = 0; relative < weightModel.totalExperts; relative++)
                    rightPanel.Children.Add(new TextBlock
                    {
                        Text = "R [" + (relative + 1).ToString() + "] = " + Math.Round(weightModel.relative[relative], 2).ToString(),
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

                for (int weight = 0; weight < weightModel.totalAlternatives; weight++)
                    rightPanel.Children.Add(new TextBlock
                    {
                        Text = "W [" + (weight + 1).ToString() + "] = " + Math.Round(weightModel.weight[weight], 2).ToString(),
                        Margin = new Thickness(24, 4, 0, 0),
                        FontSize = 15,
                    });

                rightPanel.Children.Add(new TextBlock
                {
                    Text = "Найкращою альтернативою є: A[" + (weightModel.weight.IndexOf(weightModel.weight.Max()) + 1).ToString() + "]",
                    Background = Brushes.White,
                    Margin = new Thickness(20, 20, 0, 0),
                    FontSize = 16,
                });

                double sum = weightModel.weight.Sum();
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
