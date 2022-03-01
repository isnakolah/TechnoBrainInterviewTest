namespace CompanyHierarchy.Exceptions;

internal sealed class ManagerNotFoundException : Exception
{
    public ManagerNotFoundException(string managerId)
        : base($"Manager with id {managerId} was not found")
    {
    }
}