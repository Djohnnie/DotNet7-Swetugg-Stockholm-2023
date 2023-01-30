using System.Numerics;
using static System.Console;



WriteLine($"{CalculateFactorial((byte)5)}");

WriteLine($"{CalculateFactorial(5)}");

WriteLine($"{CalculateFactorial(5U)}");

WriteLine($"{CalculateFactorial(5L)}");

WriteLine($"{CalculateFactorial(5UL)}");

WriteLine($"{CalculateFactorial(5.0F)}");

WriteLine($"{CalculateFactorial(5.0D)}");

WriteLine($"{CalculateFactorial(5.0M)}");









static TNumber CalculateFactorial<TNumber>(TNumber value) where TNumber : INumber<TNumber>
{
    if (value == TNumber.Zero || value == TNumber.One)
    {
        return TNumber.One;
    }

    if (value == TNumber.One + TNumber.One)
    {
        return value;
    }

    return value * CalculateFactorial(value - TNumber.One);
}