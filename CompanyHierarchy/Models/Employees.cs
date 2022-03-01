using CompanyHierarchy.Exceptions;

namespace CompanyHierarchy.Models;

internal sealed class Employees
{
    private readonly bool CEOExists;
    private readonly IDictionary<string, (string? managerId, long salary)> employees;

    public Employees(string csvString)
    {
        if (string.IsNullOrWhiteSpace(csvString))
            throw new CsvStringIsNullOrEmptyException();

        employees = new Dictionary<string, (string? employeeId, long salary)>();

        var csvEntries = SplitCsvString(csvString);

        foreach (var (employeeId, managerId, salary) in csvEntries)
        {
            var employeeIsCEO = string.IsNullOrWhiteSpace(managerId);

            if (employeeIsCEO && CEOExists)
                throw new CEOAlreadyExistsException();

            // make sure the employee is not manager of himself
            if (!employeeIsCEO && employeeId == managerId || IsCircularReference(employeeId, managerId!))
                throw new CircularReferenceException(employeeId, managerId!);

            if (employees.ContainsKey(employeeId))
                throw new EmployeeAlreadyHasManagerException(employeeId);

            if (employeeIsCEO)
                CEOExists = true;

            employees.Add(employeeId, (managerId, salary));
        }
    }

    internal long GetSalaryBudgetForManager(string managerId)
    {
        if (string.IsNullOrWhiteSpace(managerId))
            throw new ArgumentException("Parameter 'managerId' cannot be null or whitespace");

        // all managers are employees
        if (!employees.ContainsKey(managerId))
            throw new ManagerNotFoundException(managerId);

        var salaryBudget = CalculateSalaryOfDirectAndIndirectEmployees(managerId);

        return salaryBudget;
    }

    private long CalculateSalaryOfDirectAndIndirectEmployees(string managerId)
    {
        var totalSalary = employees[managerId].salary;

        var employeesUnderManager = employees
            .Where(x => x.Value.managerId == managerId)
            .Select(x => x.Key)
            .ToArray();

        if (!employeesUnderManager.Any())
            return totalSalary;

        totalSalary += employeesUnderManager.Sum(CalculateSalaryOfDirectAndIndirectEmployees);

        return totalSalary;
    }

    private static IEnumerable<(string, string?, long)> SplitCsvString(string csvString)
    {
        var entries = csvString
            .Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(row =>
            {
                var data = row.Split(',');

                var managerId = string.IsNullOrWhiteSpace(data[1]) ? null : data[1];

                var employeeId = data[0];

                if (!long.TryParse(data[2], out var salary))
                    throw new InvalidIntegerException(data[0]);

                return (employeeId, managerId, salary);
            });

        return entries;
    }

    private bool IsCircularReference(string employeeId, string? managerId)
    {
        managerId ??= string.Empty;

        if (employees.ContainsKey(employeeId))
            return employees[employeeId].managerId == managerId;

        if (employees.ContainsKey(managerId))
            return employees[managerId].managerId == employeeId;

        return false;
    }
}