namespace CompanyHierarchy.Exceptions;

internal sealed class CEOAlreadyExistsException : Exception
{
    public CEOAlreadyExistsException()
        : base("CEO already exists!")
    {
    }
}