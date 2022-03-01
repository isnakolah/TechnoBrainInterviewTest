namespace CompanyHierarchy.Exceptions;

internal sealed class EmployeeNotManagerException : Exception
{
    public EmployeeNotManagerException(string employeeId)
        : base($"Employee with id {employeeId} is not a manager")
    {
    }
}