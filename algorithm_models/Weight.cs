using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace Labs_1_3.algorithms
{
    class Weight : AlgorithmModel
    {
        public List<double> prof = new List<double>();
        public List<double> relative = new List<double>();
        public List<double> weight = new List<double>();

        public Weight()
        {
            algorithmName = "Метод зважування оцінок";
        }

        public void Calculate()
        {
            relative = CalculateRealtive();
            weight = CalculateWeight();
        }

        private List<double> CalculateRealtive()
        {
            List<double> result = new List<double>();

            double sum = prof.Sum();

            foreach (var item in prof)
                result.Add(item / sum);

            return result;
        }

        private List<double> CalculateWeight()
        {
            List<double> result = new List<double>();

            for (int column = 0; column < totalAlternatives; column++)              
            {
                double Sum = 0;
                for (int row = 0; row < totalExperts; row++)
                {
                    var cell = Convert.ToDouble(dataGrids[0][row].Values[column].Value);
                    var relative = this.relative[row];

                    Sum += relative * cell;
                }
                result.Add(Sum);
            }

            return result;
        }
    }
}
