using System;
using System.Text.RegularExpressions;

namespace CorrelatingEvents
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var publisher = new NewFileSavedMessagePublisher("C:\\Users\\huny8\\Downloads"))
            using (var correlator = new FileNameMessageCorrelator(ExtractionCorrelationKey))
            {
                publisher.Subscribe(correlator);
                correlator.Subscribe(new NewFileSavedMessageSubscriber());
                Console.ReadLine();
            }
        }

        static string ExtractionCorrelationKey(string arg)
        {
            var match = Regex.Match(arg, @"(FILEID\d{4})");
            if (match.Success)
            {
                return match.Captures[0].Value;
            }
            else
            {
                return null;
            }
        }
    }
}
