using FlaskeAutomaten.Data;
using System.Text.Json;

namespace FlaskeAutomaten.Controller
{
    public class BeerConsumer : Consumer<Beverage>
    {
        public event Action<string> OnLog;
        private BeerBuffer beerBuffer;

        public BeerConsumer(BeerBuffer buffer) : base(buffer)
        {
            this.beerBuffer = buffer;
        }

        public override void Consume()
        {
            while (true)
            {
                var beers = beerBuffer.BeerList;
                int consumed = 1;

                if (beers.Count > 0)
                {
                    for (int i = beers.Count - 1; i >= 0; i--)
                    {
                        // remove consumed item from list
                        OnLog?.Invoke($"Consumed {beers[i].BType} {beers[i].Material} {beers[i].Size}");
                        beers.RemoveAt(i);
                        consumed++;
                    }
                    LogBeverages(beers);
                    OnLog?.Invoke($"Consumed {consumed} beers");
                }
                else
                {
                    OnLog?.Invoke("Beer buffer is empty, waiting...");
                    Task.Delay(3000).Wait();
                }
            }
        }

        private void LogBeverages(List<Beverage> beers)
        {
            var logFilePath = $@"C:\{DateTime.Now:yyyyMMdd_HHmmss}_ProducerLog.json";
            foreach (var beer in beers)
            {
                var logEntry = new
                {
                    TimeStamp = DateTime.Now,
                    beer.BType,
                    beer.Material,
                    beer.Size
                };
                //File.AppendAllText(logFilePath, JsonSerializer.Serialize(logEntry) + Environment.NewLine);
            }
        }
    }
}
