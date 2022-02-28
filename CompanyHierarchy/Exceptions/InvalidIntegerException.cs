namespace CompanyHierarchy.Exceptions;

internal sealed  class InvalidIntegerException : Exception
{
    public InvalidIntegerException(string employeeId)
        : base($"Invalid integer value for salary of employee: {employeeId}")
    {
    }
}