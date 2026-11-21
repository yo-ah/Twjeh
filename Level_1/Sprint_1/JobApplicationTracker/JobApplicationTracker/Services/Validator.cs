using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JobApplicationTracker.Services
{
    internal class Validator
    {        
        // Convert Any Type To Tout Type
        private delegate bool ConvertTo<Tout>(out Tout a);
        public static readonly string EmailPattern = @"^(?:[^@\s]+@[^@\s]+\.[^@\s]+)?$";

        private static Tout TypeConverter<Tout>(string msg, ConvertTo<Tout> converter)
        {
            Tout output;
            Console.Write(msg);

            while (!converter(out output))
            {
                Console.WriteLine($"invalid {typeof(Tout).Name} input");
                Console.Write(msg);
            }
            return output;
        }

        // validate any int input & validates the range also
        public static int IntValidation(string msg, int startRange, int endRange = int.MaxValue)
        {
            int number = TypeConverter(msg, (out int n) => int.TryParse(Console.ReadLine(), out n));

            while (number < startRange || number > endRange)
            {
                Console.WriteLine($"input should be between [{startRange}-{endRange}]");
                number = TypeConverter(msg, (out int n) => int.TryParse(Console.ReadLine(), out n));
            }

            return number;
        }

        public static string StringValidation(string msg, string pattern)
        {
            string output = Console.ReadLine() ?? "";

            while (!Regex.IsMatch(output, pattern))
            {
                    Console.WriteLine($"invalid {msg} value: {output}");
                    Console.Write(msg);
                    output = Console.ReadLine() ?? "";
            }
            return output;
        }
    }
}
