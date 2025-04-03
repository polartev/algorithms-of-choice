using Labs_1_3.algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Labs_1_3
{
    class TabModel
    {
        private AlgorithmModel? AlgorithmModel;
        public AlgorithmModel? algorithmModel 
        { 
            get => AlgorithmModel;
            set
            {
                if (AlgorithmModel == null)
                    AlgorithmModel = value;
            } 
        }

        private string ItemName = ""; 
        public string itemName { get => ItemName; set => ItemName = value; }

        public TabModel(string? algorithmName)
        {
            setAlgorithm(algorithmName);
        }

        private void setAlgorithm(string? algorithmName) //can be changed on: algorithmModel = new AlgorithmModel();
        {
            switch (algorithmName)
            {
                case "Алгоритм Сааті":
                    algorithmModel = new Saati();
                    break;
                case "Алгоритм парних порівнянь":
                    algorithmModel = new PairComparison();
                    break;
                case "Метод зважування оцінок":
                    algorithmModel = new Weight();
                    break;
                case "Метод переваги":
                    algorithmModel = new Superior();
                    break;
                case "Метод ранга":
                    algorithmModel = new Rang();
                    break;
                default: throw new ArgumentException("Невідомий алгоритм!");
            }
        }


    }
}
