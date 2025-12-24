using System;
using System.Collections.Concurrent;
using System.Threading;

class Program
{
    static BlockingCollection<int> buffer = new BlockingCollection<int>(5);
    
    static void Main()
    {
        Thread producer1 = new Thread(Producer);
        Thread producer2 = new Thread(Producer);
        Thread consumer1 = new Thread(Consumer);
        Thread consumer2 = new Thread(Consumer);
        
        producer1.Start(1);
        producer2.Start(2);
        consumer1.Start(1);
        consumer2.Start(2);
        
        Thread.Sleep(8000);
        buffer.CompleteAdding();
        Console.WriteLine("Конец работы");
    }
    
    static void Producer(object id)
    {
        Random rnd = new Random();
        for (int i = 1; i <= 10; i++)
        {
            int item = rnd.Next(100, 1000);
            buffer.Add(item);
            Console.WriteLine($"Производитель {id} добавил: {item} (в буфере: {buffer.Count})");
            Thread.Sleep(rnd.Next(300, 800));
        }
    }
    
    static void Consumer(object id)
    {
        while (!buffer.IsCompleted)
        {
            try
            {
                int item = buffer.Take();
                Console.WriteLine($"Потребитель {id} взял: {item} (в буфере: {buffer.Count})");
                Thread.Sleep(600);
            }
            catch { }
        }
    }
}