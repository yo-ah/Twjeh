using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplicationTracker.Services
{
    internal static class Helper
    {
        public static void ListEnum<T>() where T: struct, Enum
        {
            var constants = Enum.GetNames<T>();

            for (int i = 0; i < constants.Length; i++)
            {
                Console.WriteLine($"\t- ({i + 1}) {constants[i]}");
            }
        }

        public static void PreviousMenu(string msg, Action action)
        {
            Console.Write($"{msg}Press any key to return to previous menu: ");
            Console.ReadKey();
            action();
        }

        public static T UpdateEntityData<T>(string msg, Func<T> action)
        {
            Console.Write(msg);
            return action();
        }

    }
}
