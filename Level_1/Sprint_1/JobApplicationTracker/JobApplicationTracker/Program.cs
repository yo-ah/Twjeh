using JobApplicationTracker.Models;

namespace JobApplicationTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var c = new Company("a");
            var d = new Company("ac");
            Console.WriteLine(c);
            Console.WriteLine(d);
        }
    }
}
