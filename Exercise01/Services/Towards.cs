using System.Numerics;
using System.Runtime.CompilerServices;
using Exercise01.Exceptions;

[assembly: InternalsVisibleTo("Exercise01.Tests.Unit")]
namespace Exercise01.Services;

internal static class TowardsService
{
    private static readonly Dictionary<string, string> numWords = new()
    {
        {"0", ""},
        {"1", "one"},
        {"2", "two"},
        {"3", "three"},
        {"4", "four"},
        {"5", "five"},
        {"6", "six"},
        {"7", "seven"},
        {"8", "eight"},
        {"9", "nine"},
        {"10", "ten"},
        {"11", "eleven"},
        {"12", "twelve"},
        {"13", "thirteen"},
        {"14", "fourteen"},
        {"15", "fifteen"},
        {"16", "sixteen"},
        {"17", "seventeen"},
        {"18", "eighteen"},
        {"19", "nineteen"},
        {"20", "twenty"},
        {"30", "thirty"},
        {"40", "forty"},
        {"50", "fifty"},
        {"60", "sixty"},
        {"70", "seventy"},
        {"80", "eighty"},
        {"90", "ninety"},
        {"100", "hundred"},
    };

    private static string[] illions = new[]
    {
        "",
        "thousand",
        "million",
        "billion",
        "trillion",
        "quadrillion",
        "quintillion",
        "sextillion",
        "septillion",
        "octillion",
        "nonillion",
        "decillion",
        "undecillion",
        "duodecillion",
        "tredecillion",
        "quattuordecillion",
    };

    internal static string Towards<T>(T num)
        where T : struct, IComparable, IComparable<T>, IEquatable<T>, ISpanFormattable, IFormattable
    {
        var numType = num.GetType();
        
        if (!(numType == typeof(int) || numType == typeof(BigInteger)))
            throw new InvalidTypeException(numType);
        
        var numString = ToWords(0, num.ToString());

        return numString.TrimEnd();
    }
    
    
    private static string ToWords(int pos, string numStr)
    {
        var threeDigits = numStr.Length >= 3 ? numStr[^3..]: numStr;

        if (numStr.Length < 3)
            return GetWords(pos, threeDigits);
        
        var numberAsString = GetWords(pos, threeDigits) + ToWords(pos+1, numStr[..3]);

        return numberAsString;
    }

    private static string GetWords(int pos, string numStr)
    {
        var hundreds = numStr.Length > 2 ? numStr[0] : '0';

        var tens = numStr.Length >= 2 ? numStr[..2] : numStr;

        var and = hundreds != '0' ? "and" : "";

        return numWords[hundreds.ToString()] + and + GetTensWording(tens) + " " + illions[pos];
    }

    private static string GetTensWording(string tens)
    {
        if (tens == "")
            return "";
        
        if (tens.Length == 1)
            return numWords[tens];
        
        if (tens[0] == '1')
            return numWords[tens];
        
        return numWords[$"{tens[0]}0"] + " " + numWords[tens[1].ToString()];
    }
}