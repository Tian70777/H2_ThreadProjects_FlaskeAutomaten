using FlaskeAutomaten.Data;

namespace FlaskeAutomaten.Controller
{
    public class Splitter
    {
        public event Action<string> OnLog;
        private ProductionBuffer productionBuffer;
        private BeerBuffer beerBuffer;
        private SodaBuffer sodaBuffer;

        public Splitter(ProductionBuffer productionBuffer, BeerBuffer beerBuffer, SodaBuffer sodaBuffer)
        {
            this.productionBuffer = productionBuffer;
            this.beerBuffer = beerBuffer;
            this.sodaBuffer = sodaBuffer;
        }

        public void Split()
        {
            while (true)
            {
                List<Beverage> myList = productionBuffer.PullAll();
                if (myList.Count > 0)
                {
                    foreach (var beverage in myList)
                    {
                        if (beverage.BType == BType.Soda && sodaBuffer.itemCount < sodaBuffer.maxSize )
                        {
                            sodaBuffer.SodaList.Add(beverage);
                            sodaBuffer.itemCount++;
                            OnLog?.Invoke(beverage.BType + " " + beverage.Material + " " + beverage.Size + " added to soda buffer, total account:" + sodaBuffer.itemCount);
                        }
                        else if (beverage.BType == BType.Soda)
                        {
                            OnLog?.Invoke("Soda buffer is full, cannot add more sodas.");
                        }
                        else if (beverage.BType == BType.Beer && beerBuffer.itemCount < beerBuffer.maxSize )
                        {
                            beerBuffer.BeerList.Add(beverage);
                            beerBuffer.itemCount++;
                            OnLog?.Invoke(beverage.BType + " " + beverage.Material + " " + beverage.Size + " added to beer buffer, total account: " + beerBuffer.itemCount);
                        }
                        else if (beverage.BType == BType.Beer)
                        {
                            OnLog?.Invoke("Beer buffer is full, cannot add more beers.");
                        }
                    }
                }
                else
                {
                    OnLog?.Invoke("Production buffer is empty, waiting...");
                    Task.Delay(10000).Wait();
                }
            }
        }
    }
}
