using FlaskeAutomaten.Data;

namespace FlaskeAutomaten.Controller
{
    public class ProductionBuffer : Buffer<Beverage>
    {
        public ProductionBuffer(int maxSize) : base(maxSize)
        {
            // set max size as 1000
            this.maxSize = 100;
        }
    }
}

