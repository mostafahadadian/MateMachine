
using MateMachine;

var converter = CurrencyConverter.Instance;

converter.ClearConfiguration();
converter.UpdateConfiguration(new List<Tuple<string, string, double>>
{
    Tuple.Create("USD", "CAD", 1.34),
    Tuple.Create("CAD", "GBP", 0.58),
    Tuple.Create("USD", "EUR", 0.86)
});

double amountInEUR = converter.Convert("CAD", "EUR", 100);
Console.WriteLine($"100 CAD is {amountInEUR} EUR");

Console.ReadKey();
