using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Labs_1_3.algorithms
{
    class PairComparison : AlgorithmModel
    {
        public List<List<double>> grade = new List<List<double>>();
        public List<double> general = new List<double>();
        public List<double> weight = new List<double>();

        public PairComparison()
        {
            algorithmName = "Алгоритм парних порівнянь";
        }

        public void Calculate()
        {
            grade = CalculateGrade();
            general = CalculateGeneral();
            weight = CalculateWeight();
        }

        private List<List<double>> CalculateGrade()
        {
            List<List<double>> result = new List<List<double>>();

            for (int grid = 0; grid < totalExperts; grid++)
            {
                result.Add(new List<double>());

                for (int row = 0; row < totalAlternatives; row++)
                {
                    double Sum = 0;

                    for (int column = 0; column < totalAlternatives; column++)
                    {
                        var cell = dataGrids[grid][row].Values[column].Value;

                        if (double.TryParse(cell, out double cellValue))
                            Sum += cellValue;
                    }

                    result[grid].Add(Sum);
                }
            }

            return result;
        }

        private List<double> CalculateGeneral() 
        {
            List<double> result = new List<double>();
                        
            for (int grade = 0; grade < totalAlternatives; grade++)
            {
                double Sum = 0.0;
                for (int grid = 0; grid < totalExperts; grid++)
                    Sum += this.grade[grid][grade];
                result.Add(Sum);
            }

            return result;
        }

        private List<double> CalculateWeight() 
        {
            List<double> result = new List<double>();

            for (int general = 0; general < totalAlternatives; general++)
                result.Add(this.general[general]/this.general.Sum());

            return result;
        }

        public void FillDataGrid()
        {
            for (int grid = 0; grid < totalExperts; grid++)
            {
                for (int row = 0; row < totalAlternatives; row++)
                    for (int column = row; column < row + 1; column++)
                        dataGrids[grid][row].Values[column].Value = "0";

                for (int row = 0; row < totalAlternatives; row++)
                    for (int column = 0; column < row; column++)
                        dataGrids[grid][row].Values[column].Value = CalculateOppositeValue(dataGrids[grid][column].Values[row].Value);
            }
        }

        private string CalculateOppositeValue(string firstSide)
        {
            if (firstSide == string.Empty || Convert.ToDouble(firstSide) < 0.0 || Convert.ToDouble(firstSide) > 1.0)
                return string.Empty;

            return Convert.ToString(Math.Round(1.0 - Convert.ToDouble(firstSide), 1));
        }
    }
}
