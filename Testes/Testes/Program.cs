using System;
using System.Globalization;

namespace Testes
{
    class Program
    {
        static void Main(string[] args)
        {
            double number;
            number = double.Parse(Console.ReadLine(),CultureInfo.InvariantCulture);
            Console.WriteLine(number);
            Console.ReadLine();
        }
    }
}
