using System.Numerics;
using System.Reflection;

namespace Exercise01.Exceptions;

public sealed class InvalidTypeException : Exception
{
    public InvalidTypeException(MemberInfo type)
        : base($"Expected type of {nameof(Int32)} or {nameof(BigInteger)} but got {type.Name}")
    {
    }
}