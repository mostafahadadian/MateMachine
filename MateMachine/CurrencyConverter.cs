using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private static CurrencyConverter _instance;
        private CurrencyConverter()
        {
            _rates = new ConcurrentDictionary<string, Dictionary<string, double>>();
        }
        public static CurrencyConverter Instance { 
            get 
            { 
                if (_instance == null)
                    _instance = new CurrencyConverter();
                return _instance;
            
            } 
        }
        private ConcurrentDictionary<string, Dictionary<string, double>> _rates; 
        public void ClearConfiguration()
        {
           _rates.Clear();
        }

        public double Convert(string fromCurrency, string toCurrency, double amount)
        {
            if (fromCurrency == toCurrency)
            {
                return amount;
            }
            var conversionPath = FindOptimizePath(fromCurrency, toCurrency);
            if (conversionPath == null)
            {
                throw new Exception($"Path NotFound  {fromCurrency} => {toCurrency}");
            }

            double result = amount;
            for (int i = 0; i < conversionPath.Count - 1; i++)
            {
                string from = conversionPath[i];
                string to = conversionPath[i + 1];
                result *= _rates[from][to];
            }

            return result;
        }

        public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
        {
            foreach (var item in conversionRates)
            {
                if (!_rates.ContainsKey(item.Item1))
                {
                    _rates[item.Item1] = new Dictionary<string, double>();
                }
                _rates[item.Item1][item.Item2] = item.Item3;
                if (!_rates.ContainsKey(item.Item2))
                {
                    _rates[item.Item2] = new Dictionary<string, double>();
                }
                _rates[item.Item2][item.Item1] = 1 / item.Item3;
            }        
        }

        private List<string> FindOptimizePath(string fromCurrency, string toCurrency)
        {
            var visited = new HashSet<string>();
            var queue = new Queue<(string, List<string>)>();
            queue.Enqueue((fromCurrency, new List<string> { fromCurrency }));

            while (queue.Count > 0)
            {
                var (currentCurrency, path) = queue.Dequeue();

                if (currentCurrency == toCurrency)
                    return path;

                visited.Add(currentCurrency);

                if (_rates.ContainsKey(currentCurrency))
                {
                    foreach (var nextCurrency in _rates[currentCurrency].Keys)
                    {
                        if (!visited.Contains(nextCurrency))
                        {
                            var newPath = new List<string>(path) { nextCurrency };
                            queue.Enqueue((nextCurrency, newPath));
                        }
                    }
                }
            }

            return null; 
        }
    }
}
