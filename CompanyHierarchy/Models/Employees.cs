using CompanyHierarchy.Exceptions;

namespace CompanyHierarchy.Models;

internal sealed class Employees
{
    private readonly bool CEOExists;
    private readonly IDictionary<string, (string? managerId, double salary)> employees;

    public Employees(string csvString)
    {
        if (string.IsNullOrWhiteSpace(csvString))
            throw new CsvStringIsNullOrEmptyException();

        employees = new Dictionary<string, (string? employeeId, double salary)>();

        var csvEntries = SplitCsvString(csvString);

        foreach (var (employeeId, managerId, salary) in csvEntries)
        {
            var employeeIsCEO = managerId is null;

            if (employeeIsCEO && CEOExists)
                throw new CEOAlreadyExistsException();

            if (!employeeIsCEO && employeeId == managerId || IsCircularReference(employeeId, managerId!))
                throw new CircularReferenceException(employeeId, managerId!);

            if (!employees.ContainsKey(employeeId))
                throw new EmployeeAlreadyHasManagerException(employeeId);

            if (employeeIsCEO)
                CEOExists = true;

            if (!int.TryParse(salary, out var salaryAsInt))
                throw new InvalidIntegerException(employeeId);

            employees.Add(employeeId, (managerId, salaryAsInt));
        }
    }

    internal long GetSalaryBudgetForManger(string managerId)
    {
        // get sum of all employees reporting directly or indirectly to the manager, plus the salary of the manager

        throw new NotImplementedException();
    }

    private static IEnumerable<(string, string?, string)> SplitCsvString(string csvString)
    {
        return new[]
        {
            ("Employee1", "Employee2", "500"),
            ("Employee2", "Employee3", "300"),
            ("Employee3", null, "800")
        };
    }
    
    private bool IsCircularReference(string employeeId, string managerId)
    {
        // If employee exists
        if (employees.ContainsKey(employeeId))
            return employees[employeeId].managerId == managerId;
            
        if (employees.ContainsKey(managerId))
            return employees[managerId].managerId == employeeId;

        return false;
    }
}