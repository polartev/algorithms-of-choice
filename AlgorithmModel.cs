using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Labs_1_3
{
    internal class AlgorithmModel
    {
        protected string AlgorithmName = "";
        public string algorithmName { get => AlgorithmName; init => AlgorithmName = value; }

        protected int TotalAlternatives = 3;
        public int totalAlternatives
        {
            get => TotalAlternatives;
            set 
            {
                if (value > 2)
                    TotalAlternatives = value;
            } 
        }

        protected int TotalExperts = 2;
        public int totalExperts
        {
            get => TotalExperts;
            set
            {
                if (value > 1)
                    TotalExperts = value;
            }
        }

        public List<List<DataRow>> dataGrids = new List<List<DataRow>>();
    }

    internal class DataRow : INotifyPropertyChanged
    {
        private string header;
        public string Header
        {
            get => header;
            set
            {
                if (header != value)
                {
                    header = value;
                }
            }
        }

        private ObservableCollection<NotifyString> values;
        public ObservableCollection<NotifyString> Values
        {
            get => values;
            set
            {
                if (values != value)
                {
                    values = value;
                    OnPropertyChanged(nameof(Values));
                }
            }
        }

        public DataRow()
        {
            Values = new ObservableCollection<NotifyString>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class NotifyString : INotifyPropertyChanged
    {
        private string _value;
        public string Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    onUpdateEvent?.Invoke();
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event Action onUpdateEvent;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
