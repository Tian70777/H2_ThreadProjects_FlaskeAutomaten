using FlaskeAutomaten.Data;

namespace FlaskeAutomaten.Controller
{
    public abstract class Consumer<T> where T : Beverage
    {
        public event Action<string> OnLog;

        protected Buffer<T> buffer;

        public Consumer(Buffer<T> buffer)
        {
            this.buffer = buffer;
        }

        public abstract void Consume();

    }
}
