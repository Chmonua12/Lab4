using System;
using System.Threading;
using System.Threading.Tasks;

class PhilosophersTests
{
    public static void RunTests()
    {
        Console.WriteLine("Тестирование кушающих философов");
        
        TestDeadlockVersion();
        TestNoDeadlockVersion();
    }
    
    static void TestDeadlockVersion()
    {
        Console.WriteLine("Версия с deadlock");
        
        try
        {
            var task = Task.Run(() => RunDeadlockSimulation(3));
            
            if (task.Wait(TimeSpan.FromSeconds(5)))
            {
                Console.WriteLine("ОШИБКА: Deadlock не произошел");
            }
            else
            {
                Console.WriteLine("УСПЕХ: Deadlock обнаружен");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Исключение: {ex.Message}");
        }
    }
    
    static void TestNoDeadlockVersion()
    {
        Console.WriteLine("Версия без deadlock");
        
        try
        {
            var task = Task.Run(() => RunNoDeadlockSimulation(5));
            
            if (task.Wait(TimeSpan.FromSeconds(10)))
            {
                Console.WriteLine("Программа завершилась без deadlock");
            }
            else
            {
                Console.WriteLine("Программа зависла");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Исключение: {ex.Message}");
        }
    }
    
    static void RunDeadlockSimulation(int seconds)
    {
        object lock1 = new object();
        object lock2 = new object();
        
        var t1 = new Thread(() =>
        {
            lock (lock1)
            {
                Thread.Sleep(100);
                lock (lock2) { }
            }
        });
        
        var t2 = new Thread(() =>
        {
            lock (lock2)
            {
                Thread.Sleep(100);
                lock (lock1) { }
            }
        });
        
        t1.Start();
        t2.Start();
        
        Thread.Sleep(seconds * 1000);
    }
    
    static void RunNoDeadlockSimulation(int seconds)
    {
        SemaphoreSlim semaphore = new SemaphoreSlim(2, 2);
        
        for (int i = 0; i < 5; i++)
        {
            int id = i;
            new Thread(() =>
            {
                semaphore.Wait();
                Thread.Sleep(500);
                semaphore.Release();
            }).Start();
        }
        
        Thread.Sleep(seconds * 1000);
    }
}