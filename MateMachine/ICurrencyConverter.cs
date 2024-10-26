using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine
{
    public interface ICurrencyConverter
    {
        void ClearConfiguration();
        void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates);
        double Convert(string fromCurrency, string toCurrency, double amount);
    }
}
