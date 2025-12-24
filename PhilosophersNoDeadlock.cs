using System;
using System.Threading;

class Program
{
    static SemaphoreSlim[] forks;
    static SemaphoreSlim waiter = new SemaphoreSlim(4, 4);
    
    static void Main()
    {
        forks = new SemaphoreSlim[5];
        for (int i = 0; i < 5; i++)
        {
            forks[i] = new SemaphoreSlim(1, 1);
        }
        
        Thread[] philosophers = new Thread[5];
        for (int i = 0; i < 5; i++)
        {
            int id = i;
            philosophers[i] = new Thread(() => Philosopher(id));
            philosophers[i].Start();
        }
        
        Thread.Sleep(10000);
        Console.WriteLine("конец");
    }
    
    static void Philosopher(int id)
    {
        int left = id;
        int right = (id + 1) % 5;
        
        while (true)
        {
            Console.WriteLine($"Философ {id} думает");
            Thread.Sleep(500);
            
            waiter.Wait();
            forks[left].Wait();
            Console.WriteLine($"Философ {id} взял левую вилку {left}");
            
            forks[right].Wait();
            Console.WriteLine($"Философ {id} взял правую вилку {right}");
            
            Console.WriteLine($"Философ {id} ЕСТ");
            Thread.Sleep(800);
            Console.WriteLine($"Философ {id} положил правую вилку {right}");
            forks[right].Release();
            
            Console.WriteLine($"Философ {id} положил левую вилку {left}");
            forks[left].Release();
            
            waiter.Release();
        }
    }
}