using System;
using System.Threading;

class Program
{
    static object[] forks = new object[5];
    
    static void Main()
    {
        for (int i = 0; i < 5; i++)
        {
            forks[i] = new object();
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
            
            lock (forks[left])
            {
                Console.WriteLine($"Философ {id} взял левую вилку {left}");
                Thread.Sleep(100);
                
                lock (forks[right])
                {
                    Console.WriteLine($"Философ {id} взял правую вилку {right}");
                    Console.WriteLine($"Философ {id} кушоет");
                    Thread.Sleep(800);
                    Console.WriteLine($"Философ {id} положил правую вилку {right}");
                }
                Console.WriteLine($"Философ {id} положил левую вилку {left}");
            }
        }
    }
}