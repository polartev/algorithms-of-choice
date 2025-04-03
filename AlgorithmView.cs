using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Labs_1_3
{
    internal class AlgorithmView
    {
        private AlgorithmModel algorithmModel;

        protected StackPanel leftPanel;
        protected StackPanel rightPanel;

        protected List<DataGrid> dataGrids = new List<DataGrid>();

        public AlgorithmView(AlgorithmModel algorithmModel, StackPanel leftPanel, StackPanel rightPanel)
        {
            this.algorithmModel = algorithmModel;
            this.rightPanel = rightPanel;
            this.leftPanel = leftPanel;
        }

        public virtual void BuildLeftSide()
        {

        }

        public virtual void BuildRightSide()
        {

        }

        protected DataGrid GenerateDataGrid()
        {
            string name = "dataGrid" + dataGrids.Count.ToString();
            DataGrid dataGrid = new DataGrid 
            { 
                Name = name,
                AutoGenerateColumns = false,
                CanUserReorderColumns = false,
                CanUserAddRows = false,
                CanUserDeleteRows = false,
                CanUserSortColumns = false,
                Margin = new Thickness(20) 
            };
            return dataGrid;
        }

        protected virtual void InitializeDataGrid(DataGrid dataGrid, int columns = 3, int rows = 0)
        {
            dataGrid.Columns.Clear();
            
            string rowHeader = "A";
            algorithmModel.totalAlternatives = columns;
            columns = algorithmModel.totalAlternatives;
            if (rows == 0)
                rows = columns;
            else
            {
                rowHeader = "E";
                algorithmModel.totalExperts = rows;
                rows = algorithmModel.totalExperts;
            }

            DataGridTextColumn headerColumn = new DataGridTextColumn
            {
                Header = "",
                Binding = new Binding("Header"),
                IsReadOnly = true
            };
            dataGrid.Columns.Add(headerColumn);

            for (int column = 0; column < columns; column++)
            {
                DataGridTextColumn textColumn = new DataGridTextColumn
                {
                    Header = $"A{column + 1}",
                    Binding = new Binding($"Values[{column}].Value")
                };
                dataGrid.Columns.Add(textColumn);
            }

            var dataRows = new List<DataRow>();
            for (int row = 0; row < rows; row++)
            {
                var dataRow = new DataRow
                {
                    Header = $"{rowHeader}{row + 1}",
                    Values = new ObservableCollection<NotifyString>()
                };

                for (int column = 0; column < columns; column++)
                {
                    dataRow.Values.Add(new NotifyString { Value = string.Empty });
                }

                dataRows.Add(dataRow);
            }
            algorithmModel.dataGrids.Add(dataRows);
            dataGrid.ItemsSource = dataRows;
        }
    }
}
