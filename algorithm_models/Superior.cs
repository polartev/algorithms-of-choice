using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Labs_1_3.algorithms
{
    class Superior : AlgorithmModel
    {
        public List<List<double>> converted = new List<List<double>>();
        public List<double> grade = new List<double>();
        public List<double> weight = new List<double>();

        public Superior()
        {
            algorithmName = "Метод переваги";
        }

        public void Calculate()
        {
            converted = CalculateConverted();
            grade = CalculateGrade();
            weight = CalculateWeight();
        }

        private List<List<double>> CalculateConverted()
        {
            List<List<double>> result = new List<List<double>>();

            for (int row = 0; row < totalExperts; row++) 
            {
                result.Add(new List<double>());
                for(int column = 0; column < totalAlternatives; column++)
                {
                    var cell = Convert.ToDouble(dataGrids[0][row].Values[column].Value);

                    if (cell < 0 || cell > totalAlternatives)
                        throw new Exception();

                    result[row].Add(totalAlternatives - cell);
                }
            }

            return result;
        }

        private List<double> CalculateGrade()
        {
            List<double> result = new List<double>();

            for (int column = 0; column < totalAlternatives; column++)   
            {
                double Sum = 0;
                for (int row = 0; row < totalExperts; row++)
                {
                    var cell = converted[row][column];
                    Sum += cell;
                }
                result.Add(Sum);
            }

            return result;
        }

        private List<double> CalculateWeight()
        {
            List<double> result = new List<double>();

            double sum = grade.Sum();

            foreach (var item in grade)
                result.Add(item / sum);

            return result;
        }
    }
}
