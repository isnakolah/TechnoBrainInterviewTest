namespace CompanyHierarchy.Exceptions;

internal sealed class CircularReferenceException : Exception
{
    public CircularReferenceException(string employeeId1, string employeeId2)
        : base($"Employee: {employeeId1} and {employeeId2} are in a cyclic reference.")
    {
    }
}