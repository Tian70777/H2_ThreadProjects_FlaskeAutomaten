namespace FlaskeAutomaten.Data
{
    public class Beverage
    {
        public BType BType { get; set; }
        public Material Material { get; set; }
        public Size Size { get; set; }
        

        public Beverage(BType type, Material material, Size size)
        {
            BType = type;
            Material = material;
            Size = size;
        }
    }

    public enum BType 
    { 
        Soda, 
        Beer
    }

    public enum Material
    {
        Can,
        Glass, 
        Plastic
    }

    public enum Size
    {
        _25cl, 
        _33cl, 
        _50cl, 
        _125cl,
        _150cl, 
        _200cl
    }
}
