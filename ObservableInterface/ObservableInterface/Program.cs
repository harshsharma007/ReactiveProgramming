using System;

namespace ObservableInterface
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var observer = new ConsoleIntegerProducer())
            using (var consumer1 = observer.Subscribe(new IntegerConsumer(2)))
            using (var consumer2 = observer.Subscribe(new IntegerConsumer(3)))
            using (var consumer3 = observer.Subscribe(new IntegerConsumer(5)))
                observer.Wait();

            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
