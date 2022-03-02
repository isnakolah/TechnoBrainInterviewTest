using System.Numerics;
using Exercise01.Services;

namespace Exercise01.Extensions;

public static class NumberExtensions
{
    public static string Towards(this int num) => TowardsService.Towards(num);

    public static string Towards(this BigInteger num) => TowardsService.Towards(num);
}