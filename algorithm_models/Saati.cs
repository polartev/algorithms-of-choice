using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Controls;

namespace Labs_1_3.algorithms
{
    class Saati : AlgorithmModel
    {
        public List<double> price = new List<double>();
        public List<double> weight = new List<double>();


        public Saati()
        {
            algorithmName = "Алгоритм Сааті";
        }

        public void Calculate()
        {
            price = CalculatePrice();
            weight = CalculateWeight();
        }

        private List<double> CalculatePrice()
        {
            List<double> result = new List<double>();
            for (int row = 0; row < totalAlternatives; row++) // calculations of Sum (but actualy operation is * )
            {
                double Sum = 1;
                for (int column = 0; column < totalAlternatives; column++)
                {
                    var cell = dataGrids[0][row].Values[column].Value;
                    if (cell.IndexOf('/') == -1)
                        Sum = Sum * Convert.ToDouble(cell);
                    else
                        Sum = Sum * ConvertToTenDrob(cell);
                }
                Sum = Math.Pow(Sum, 1.0 / totalAlternatives);
                result.Add(Sum);
            }
            return result;
        }

        private List<double> CalculateWeight()
        {
            List<double> result = new List<double> ();

            for (int item =  0; item < totalAlternatives; item++)
                result.Add(price[item] / price.Sum());

            return result;
        }

        public string CheckExpertsGrades() // check at right values of counting
        {
            double SLS = 0;
            List<double> R = new List<double>();

            switch (totalAlternatives.ToString())
            {
                case "3":
                    SLS = 0.58;
                    break;
                case "4":
                    SLS = 0.90;
                    break;
                case "5":
                    SLS = 1.12;
                    break;
                case "6":
                    SLS = 1.24;
                    break;
                case "7":
                    SLS = 1.32;
                    break;
                case "8":
                    SLS = 1.41;
                    break;
                case "9":
                    SLS = 1.45;
                    break;
                case "10":
                    SLS = 1.49;
                    break;
            } // підбір СлС

            for (int row = 0; row < totalAlternatives; row++)
            {
                double Sum = 0;
                for (int column = 0; column < totalAlternatives; column++)
                {
                    var cell = dataGrids[0][column].Values[row].Value;
                    if (cell.IndexOf('/') == -1)
                        Sum = Sum + Convert.ToDouble(cell);
                    else
                        Sum = Sum + ConvertToTenDrob(cell);
                }
                R.Add(Sum);
            }

            double L = 0;

            for (int i = 0; i < totalAlternatives; i++)
                L += R[i] * weight[i];

            double IS = (L - totalAlternatives) / (totalAlternatives - 1);
            double OS = Math.Round(IS / SLS, 3);

            if (OS > 0.2)
                return "Відношення узгодженості перевищує " + OS + " > 0.2, потрібне уточнення матриці парних порівнянь";
            else 
                return "Відношення узгодженості не перевищує " + OS + " < 0.2, уточнення матриці парних порівнянь не потрібне";
        }

        public void FillDataGrid()
        {
            for (int row = 0; row < totalAlternatives; row++)
                for (int column = row; column < row+1; column++)
                    dataGrids[0][row].Values[column].Value = "1";

            for (int row = 0; row < totalAlternatives; row++)
                for (int column = 0; column < row; column++)
                    dataGrids[0][row].Values[column].Value = OppositeTenDrobe(dataGrids[0][column].Values[row].Value);
        }

        private double ConvertToTenDrob(string RightDrob) // розрахунок введення у таблиці значень виду: 1/5
        {
            double TenDrob;

            string[] operand = RightDrob.Split('/');
            return TenDrob = Convert.ToDouble(operand[0]) / Convert.ToDouble(operand[1]);
        }

        private string OppositeTenDrobe(string RightDrobe) 
        {
            if (RightDrobe == string.Empty)
                return string.Empty;

            string result;

            if (RightDrobe.IndexOf('/') == -1)
                result = $"1/{RightDrobe}";
            else
            {
                string[] operand;
                operand = RightDrobe.Split("/");
                result = operand[1];
            }

            return result;
        }
    }
}
