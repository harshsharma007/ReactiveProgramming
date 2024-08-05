using System;

namespace SourcingEvents
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Watching for new files");

            using (var publisher = new NewFileSavedMessagePublisher("C:\\Users\\huny8\\Downloads"))
            using (var subscriber = publisher.Subscribe(new NewFileSavedMessageSubscriber()))
            {
                Console.WriteLine("Please RETURN to exit");
                Console.ReadLine();
            }
        }
    }
}
