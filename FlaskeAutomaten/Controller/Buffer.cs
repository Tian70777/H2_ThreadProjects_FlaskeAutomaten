using FlaskeAutomaten.Data;

namespace FlaskeAutomaten.Controller
{
    public class Buffer<T> where T : Beverage
    {
        // Event for logging
        public event Action<string> OnLog;
        private Producer producer; // Reference to the producer

        // Queue for the buffer, containing the generic type T, which is a Beverage
        // this queue is protected so that it can be accessed by the subclasses
        protected Queue<T> queue = new Queue<T>();

        // Count of items added and removed
        public int itemCount = 0;

        // Maximum size of the buffer
        public int maxSize;

        // list to store the items entered into the buffer

        public List<T> itemsList;
        public List<T> ItemsList { get; set; }

        //// SemaphoreSlim for thread synchronization
        //private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Constructor to set the maximum size of the buffer
        /// </summary>
        /// <param name="maxSize"></param>
        public Buffer( int maxSize)
        {
            this.maxSize = maxSize;
            itemsList = new List<T>();
           
        }

        /// <summary>
        /// Method to add items to the buffer
        /// </summary>
        /// <param name="items"></param>
        public void Push(List<T> items)
        {
            bool lockTaken = false;
            try
            {
                Monitor.Enter(queue, ref lockTaken);
                
                foreach(var item in items)
                {
                    queue.Enqueue(item);
                    OnLog?.Invoke($"Added {item.GetType().Name} {item.BType} {item.Material} " +
                        $"{item.Size} to buffer...");
                    itemCount++;
                    itemsList.Add(item);

                }
                // Wait for the buffer to have space
                OnLog?.Invoke("Buffer is full, waiting for space...");
                // Notify any waiting threads that the buffer state has changed
                Monitor.Pulse(queue);
                    
              
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(queue); // Release the lock
                }
            }
        }

        /// <summary>
        /// Get all items from a full buffer
        /// </summary>
        /// <returns></returns>
        public List<T> PullAll()
        {
            bool lockTaken = false;
            List<T> items = new List<T>();

            try
            {
                Monitor.Enter(queue, ref lockTaken);
                while(queue.Count > 0)
                {
                    T item = queue.Dequeue();
                    items.Add(item);
                    OnLog?.Invoke($"Removing {item.GetType().Name} from buffer...");
                    itemCount--; // Decrement the count of items removed
                    Thread.Sleep(100);
                }
                // Notify any waiting threads that the buffer state has changed
                Monitor.Pulse(queue);
            }

            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(queue); // Release the lock
                }
            }

            return items;
        }

        ///// <summary>
        ///// Method to add an item to the buffer
        ///// </summary>
        ///// <param name="item"></param>
        //public void Push(T item)
        //{
        //    // Wait until it's safe to enter the critical section
        //    semaphoreSlim.Wait();

        //    try
        //    {
        //        // If the queue is not full, add the item to the queue
        //        if(queue.Count < maxSize)
        //        {
        //            // Add the item to the queue
        //            queue.Enqueue(item);
        //            Console.WriteLine($"Added {item.GetType().Name} to buffer");
        //            return; // Exit the method after successfully adding the item
        //        }
        //        // If the queue is full, wait until there is space in the queue
        //        else
        //        {
        //            Console.WriteLine("Buffer is full, waiting for space...");
        //        }
        //            Thread.Sleep(100);
        //    }
        //    finally
        //    {
        //        // Release the semaphore
        //        semaphoreSlim.Release();
        //    }

        //    // If the buffer is full, pause the current thread for 100 milliseconds before trying again
        //    Thread.Sleep(100);
        //}

        //public void Pull()
        //{

        //    // Wait until it's safe to enter the critical section
        //    semaphoreSlim.Wait();

        //    try
        //    {
        //        // If the queue is not empty, remove the first item from the queue
        //        if(queue.Count > 0)
        //        {
        //            // Remove the first item from the queue
        //            T item = queue.Dequeue();
        //            Console.WriteLine($"Removed {item.GetType().Name} from buffer");
        //            return; // Exit the method after successfully removing the item
        //        }
        //        // If the queue is empty, wait until there is an item in the queue
        //        else
        //        {
        //            Console.WriteLine("Buffer is empty, waiting for items...");
        //        }
        //            Thread.Sleep(100);
        //    }
        //    finally
        //    {
        //        // Release the semaphore
        //        semaphoreSlim.Release();
        //    }

        //    // If the buffer is empty, pause the current thread for 100 milliseconds before trying again
        //    Thread.Sleep(100);
        //}
    }
}
