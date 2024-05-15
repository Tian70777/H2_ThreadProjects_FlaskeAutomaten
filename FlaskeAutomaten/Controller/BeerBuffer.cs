using FlaskeAutomaten.Data;
using System.Threading;

namespace FlaskeAutomaten.Controller
{
    public class BeerBuffer : Buffer<Beverage>
    {
        public List<Beverage> BeerList { get; set; }
        public BeerBuffer(int maxSize) : base( maxSize)
        {
            // set max size as 100
            this.maxSize = maxSize;
            BeerList = new List<Beverage>();
        }
    }
}


