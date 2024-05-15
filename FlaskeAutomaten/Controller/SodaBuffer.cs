using FlaskeAutomaten.Data;

namespace FlaskeAutomaten.Controller
{
    public class SodaBuffer : Buffer<Beverage>
    {

        public List<Beverage> SodaList { get; set; }

        public SodaBuffer( int maxSize) : base(maxSize)
        {
            // set max size as 100
            this.maxSize = maxSize;
            SodaList = new List<Beverage>();
        }

       
    }
}
