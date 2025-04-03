using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs_1_3.algorithms
{
    class Rang : AlgorithmModel
    {
        public List<List<string>> normalized = new List<List<string>>();
        public List<double> weight = new List<double>();

        public Rang()
        {
            algorithmName = "Метод ранга";
        }

        public void Calculate()
        {
            normalized = CalculateNormalized();
            weight = CalculateWeight();
        }

        private List<List<string>> CalculateNormalized()
        {
            List<List<string>> result = new List<List<string>>();

            for (int row = 0; row < totalExperts; row++)
            {
                result.Add(new List<string>());
                double Sum = dataGrids[0][row].Values.Sum(num => Convert.ToDouble(num.Value));
                for (int column = 0; column < totalAlternatives; column++)
                {
                    var cell = dataGrids[0][row].Values[column].Value;
                    result[row].Add($"{cell}/{Math.Round(Sum).ToString()}");
                }
                
            }

            return result;
        }

        private List<double> CalculateWeight()
        {
            var result = new List<double>();

            for (int column = 0; column < totalAlternatives; column++)                
            {
                double Sum = 0.0;
                for (int row = 0; row < totalExperts; row++)
                {
                    var cell = ConvertToTenDrob(normalized[row][column]);
                    Sum += cell;
                }
                result.Add(Sum/totalExperts);
            }

            return result;
        }

        private double ConvertToTenDrob(string RightDrob) // розрахунок введення у таблиці значень виду: 1/5
        {
            double TenDrob;

            string[] operand = RightDrob.Split('/');
            return TenDrob = Convert.ToDouble(operand[0]) / Convert.ToDouble(operand[1]);
        }
    }
}
