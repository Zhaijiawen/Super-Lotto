using Super_Lotto.DataSources.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super_Lotto.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IList<HistoryData> historysField;
        public IList<HistoryData> Historys
        {
            get
            {
                if (historysField == null)
                {
                    historysField = new List<HistoryData>();
                }
                return historysField;
            }
            set
            {
                if (historysField != value)
                {
                    historysField = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Historys)));
                    }
                }
            }
        }

        private IList<string> foreacastNumbersField;
        public IList<string> ForeacastNumbers
        {
            get
            {
                if (foreacastNumbersField == null)
                {
                    foreacastNumbersField = new List<string>();
                }
                return foreacastNumbersField;
            }
            set
            {
                if (foreacastNumbersField != value)
                {
                    foreacastNumbersField = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ForeacastNumbers)));
                    }
                }
            }
        }

        private int calculationCountField = 30;
        public int CalculationCount
        {
            get
            {
                return calculationCountField;
            }
            set
            {
                if (calculationCountField != value)
                {
                    calculationCountField = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(CalculationCount)));
                    }
                }
            }
        }

        private int forecastCountField = 10;
        public int ForecastCount
        {
            get
            {
                return forecastCountField;
            }
            set
            {
                if (forecastCountField != value)
                {
                    forecastCountField = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ForecastCount)));
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
