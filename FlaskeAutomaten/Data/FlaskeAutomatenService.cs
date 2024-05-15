using FlaskeAutomaten.Controller;

namespace FlaskeAutomaten.Data
{
    public class FlaskeAutomatenService
    {
        private Producer producer;
        private Splitter? splitter;
        private BeerConsumer beerConsumer;
        private SodaConsumer sodaConsumer;
        private ProductionBuffer productionBuffer = new ProductionBuffer(20);
        public event Action<string> OnLog;

        public FlaskeAutomatenService()
        {
            // Initialize the components
            
            ProductionBuffer productionBuffer = new ProductionBuffer(1500);
            BeerBuffer beerBuffer = new BeerBuffer(150);
            SodaBuffer sodaBuffer = new SodaBuffer(150);
            producer = new Producer(productionBuffer);
            beerConsumer = new BeerConsumer(beerBuffer);
            sodaConsumer = new SodaConsumer(sodaBuffer);
            splitter = new Splitter(productionBuffer, beerBuffer, sodaBuffer);
           

            // Subscribe to the events
            producer.OnLog += Log;
            splitter.OnLog += Log;
            beerConsumer.OnLog += Log;
            sodaConsumer.OnLog += Log;
            sodaBuffer.OnLog += Log;
            beerBuffer.OnLog += Log;
            productionBuffer.OnLog += Log;
        }

        
        private void Log(string message)
        {
            // Raise the OnLog event with the message
            OnLog?.Invoke(message);


        }
        public void StartProcess()
        {
            Task.Run(() => producer.Produce());
            Task.Run(() => splitter.Split());
            Task.Run(() => beerConsumer.Consume());
            Task.Run(() => sodaConsumer.Consume());
            //Beverage b = new Beverage(BType.Beer, Material.Can, Size._50cl);
            //Soda s2 = (Soda)b;
        }
    }
}

