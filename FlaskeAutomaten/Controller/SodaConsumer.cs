using FlaskeAutomaten.Data;
using System.Text.Json;

namespace FlaskeAutomaten.Controller
{
    public class SodaConsumer : Consumer<Beverage>
    {
        public event Action<string> OnLog;
        private SodaBuffer sodaBuffer;
        public SodaConsumer(SodaBuffer buffer) : base(buffer)
        {
            this.sodaBuffer = buffer;
        }

        public override void Consume()
        {
            while (true)
            {
                var sodas = sodaBuffer.SodaList;
                int consumed =1;

                if (sodas.Count > 0)
                {
                    for (int i = sodas.Count - 1; i >= 0; i--)
                    {
                        // remove consumed item from list
                        OnLog?.Invoke($"Consumed {sodas[i].BType} {sodas[i].Material} {sodas[i].Size}");
                        sodas.RemoveAt(i);
                        consumed++;
                    }
                    
                    LogBeverages(sodas);
                    OnLog?.Invoke($"Consumed {consumed} sodas");
                }
                else
                {
                    OnLog?.Invoke("Soda buffer is empty, waiting...");
                    Task.Delay(3000).Wait();

                }
            }
        }


        private void LogBeverages(List<Beverage> sodas)
        {
            var logFilePath = $@"C:\{DateTime.Now:yyyyMMdd_HHmmss}_ProducerLog.json";
            foreach (var soda in sodas)
            {
                var logEntry = new
                {
                    TimeStamp = DateTime.Now,
                    soda.BType,
                    soda.Material,
                    soda.Size
                };
                //File.AppendAllText(logFilePath, JsonSerializer.Serialize(logEntry) + Environment.NewLine);
            }
        }
    }
}
