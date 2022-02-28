namespace CompanyHierarchy.Exceptions;

internal sealed class EmployeeAlreadyHasManagerException : Exception
{
    public EmployeeAlreadyHasManagerException(string employeeId)
        : base($"Employee with id {employeeId} already has a reporting manager.")
    {
    }
}
