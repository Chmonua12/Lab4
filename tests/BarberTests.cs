using System;
using System.Collections.Generic;
using System.Threading;

class BarberTests
{
    public static void RunTests()
    {
        Console.WriteLine("Тестирование парикмахера");
        
        TestBarberWithQueueLimit();
        TestBarberWithoutClients();
    }
    
    static void TestBarberWithQueueLimit()
    {
        Console.WriteLine("Очередь с ограничением");
        
        try
        {
            int servedClients = 0;
            int maxQueueSize = 3;
            Queue<int> queue = new Queue<int>();
            SemaphoreSlim customers = new SemaphoreSlim(0);
            SemaphoreSlim barberReady = new SemaphoreSlim(0);
            object queueLock = new object();
            
            var barber = new Thread(() =>
            {
                while (true)
                {
                    customers.Wait();
                    
                    int client;
                    lock (queueLock)
                    {
                        client = queue.Dequeue();
                    }
                    
                    barberReady.Release();
                    servedClients++;
                    Thread.Sleep(200);
                }
            });
            barber.Start();
            
            for (int i = 1; i <= 5; i++)
            {
                lock (queueLock)
                {
                    if (queue.Count < maxQueueSize)
                    {
                        queue.Enqueue(i);
                        customers.Release();
                    }
                }
                Thread.Sleep(100);
            }
            
            Thread.Sleep(1000);
            Console.WriteLine($"Обслужено клиентов: {servedClients}");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
    
    static void TestBarberWithoutClients()
    {
        Console.WriteLine("Парикмахер без клиентов");
        
        try
        {
            bool barberSlept = false;
            SemaphoreSlim customers = new SemaphoreSlim(0);
            
            var barber = new Thread(() =>
            {
                if (!customers.Wait(1000))
                {
                    barberSlept = true;
                }
            });
            
            barber.Start();
            barber.Join();
            
            if (barberSlept)
            {
                Console.WriteLine("Парикмахер корректно заснул");
            }
            else
            {
                Console.WriteLine("Парикмахер не спит");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
