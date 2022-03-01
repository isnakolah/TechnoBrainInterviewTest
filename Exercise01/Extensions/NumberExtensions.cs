using System.Numerics;

namespace Exercise01.Extensions;

public static class NumberExtensions
{
    public static string Towards(this int num)
    {
        return num.ToString();
    }

    public static string Towards(this BigInteger num)
    {
        return num.ToString();
    }
}