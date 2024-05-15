using FlaskeAutomaten.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;

namespace FlaskeAutomaten.Controller
{
    public class Producer
    {
        public event Action<string> OnLog; 
        private ProductionBuffer productionBuffer;
        private int totalProduced = 0;
        private const int totalItemsToProduce = 1500;
        private string logFilePath = $@"C:\{DateTime.Now:yyyyMMdd_HHmmss}_producerLog.json";

        public Producer(ProductionBuffer productionBuffer)
        {
            this.productionBuffer = productionBuffer;
        }

        public async Task Produce()
        {
            while (totalProduced < totalItemsToProduce)
            {
                do
                {
                    var items = GenerateBeverages(productionBuffer.maxSize - productionBuffer.itemCount);

                    // add all items to the buffer
                    productionBuffer.Push(items);

                    foreach (var item in items)
                    {
                        LogBeverage(item);
                        OnLog?.Invoke("Producing " + item.BType + " " + item.Material + item.Size);
                    }

                    totalProduced += items.Count;
                    OnLog?.Invoke($"Produced {items.Count} items, {totalProduced} in total");
                } while (productionBuffer.itemCount < productionBuffer.maxSize);

                if (productionBuffer.itemCount < productionBuffer.maxSize)
                {
                    await Task.Delay(5000); // Wait for the buffer to have space
                }


            //if (productionBuffer.itemCount < productionBuffer.maxSize)
            //{
            //    var items = GenerateBeverages(productionBuffer.maxSize - productionBuffer.itemCount);

                    //    // add all items to the buffer
                    //    productionBuffer.Push(items);

                    //    foreach (var item in items)
                    //    {
                    //        LogBeverage(item);
                    //        OnLog?.Invoke("Producing " + item.BType + " " + item.Material + item.Size);
                    //    }
                    //    totalProduced += items.Count;
                    //    OnLog?.Invoke($"Produced {items.Count} items, {totalProduced} in total");
                    //}
                    //else
                    //{
                    //    await Task.Delay(5000); // Wait for the buffer to have space
                    //}
           }
         }
        
        

        public List<Beverage> GenerateBeverages(int amount)
        {
            var beverages = new List<Beverage>();
            Random rand = new Random();
            int count = 0;

            for (int i = 0; i < amount; i++)
            {
                BType type = (BType)rand.Next(0, 2); // Cast to Type enum
                Material material = (Material)rand.Next(0, 3); // Cast to Material enum
                Size size = (Size)rand.Next(0, 5); // Cast to Size enum
                beverages.Add(new Beverage(type, material, size));
                count++;
                OnLog?.Invoke("Producing " + type + " " + material + size + ". Count: " + count);
            }
            return beverages;
        }

        private void LogBeverage(Beverage beverage)
        {
            var logEntry = new
            {
                TimeStamp = DateTime.Now,
                beverage.BType,
                beverage.Material,
                beverage.Size
            };
            File.AppendAllText(logFilePath, JsonSerializer.Serialize(logEntry) + Environment.NewLine);
        }



    }
}
