using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static Queue<int> queue = new Queue<int>();
    static SemaphoreSlim customers = new SemaphoreSlim(0);
    static SemaphoreSlim barber = new SemaphoreSlim(0);
    static SemaphoreSlim access = new SemaphoreSlim(1);
    static int maxSeats = 3;
    static int nextCustomer = 1;
    
    static void Main()
    {
        Thread barberThread = new Thread(Barber);
        Thread customerThread = new Thread(Customers);
        
        barberThread.Start();
        customerThread.Start();
        
        Thread.Sleep(15000);
        Console.WriteLine("Конец работы");
    }
    
    static void Barber()
    {
        while (true)
        {
            customers.Wait();
            access.Wait();
            
            int customer = queue.Dequeue();
            Console.WriteLine($"Парикмахер стрижет {customer}");
            
            access.Release();
            barber.Release();
            
            Thread.Sleep(1500);
            Console.WriteLine($"Парикмахер закончил стричь {customer}");
        }
    }
    
    static void Customers()
    {
        while (true)
        {
            Thread.Sleep(800);
            
            access.Wait();
            if (queue.Count < maxSeats)
            {
                queue.Enqueue(nextCustomer);
                Console.WriteLine($"Клиент {nextCustomer} занял место в очереди. Очередь: {queue.Count}");
                customers.Release();
                nextCustomer++;
            }
            else
            {
                Console.WriteLine($"Клиент {nextCustomer} ушёл");
                nextCustomer++;
            }
            access.Release();
        }
    }
}