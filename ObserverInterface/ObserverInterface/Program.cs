using System;

namespace ObserverInterface
{
    public class Program
    {
        static void Main(string[] args)
        {
            var consumer = new ConsoleTextConsumer();

            while (true)
            {
                Console.WriteLine("Write some text and press Enter to send message. Press Enter to exit.");
                var input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    consumer.OnCompleted();
                    Console.WriteLine("Task completed. Any further message will generate an error");
                }
                else
                {
                    consumer.OnNext(input);
                }
            }
        }
    }
}
