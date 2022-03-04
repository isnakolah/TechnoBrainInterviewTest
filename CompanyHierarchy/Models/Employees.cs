using CompanyHierarchy.Exceptions;

namespace CompanyHierarchy.Models;

internal sealed class Employees
{
    private record Employee(string Id, string? ManagerId, long Salary, HashSet<string> Subordinates)
    {
        public bool IsCEO() => ManagerId is null;
    }

    private readonly IDictionary<string, Employee> employees;

    public Employees(string csvString)
    {
        if (string.IsNullOrWhiteSpace(csvString))
            throw new CsvStringIsNullOrEmptyException();

        employees = new Dictionary<string, Employee>();

        var newEmployees = SplitCsvString(csvString);

        foreach (var newEmployee in newEmployees)
        {
            if (newEmployee.IsCEO() && CEOExists())
                throw new CEOAlreadyExistsException();

            var (id, managerId, _, _) = newEmployee;

            if (id == managerId || IsCircularReference(newEmployee))
                throw new CircularReferenceException(id, managerId!);

            if (employees.Values.Any(x => x.Subordinates.Contains(id)))
                throw new EmployeeAlreadyHasManagerException(id);

            if (!employees.ContainsKey(id))
                employees.Add(id, newEmployee);

            else if (employees[id] is {Id: "", Salary: 0, ManagerId: ""})
                employees[id] = newEmployee with {Subordinates = employees[id].Subordinates};

            if (!newEmployee.IsCEO() && employees.ContainsKey(managerId!))
                employees[managerId!].Subordinates.Add(id);

            else if (!newEmployee.IsCEO() && !employees.ContainsKey(managerId!))
                employees.Add(managerId!, new Employee(string.Empty, string.Empty, 0, new HashSet<string> {id}));
        }
    }

    public long GetSalaryBudgetForManager(string managerId)
    {
        if (string.IsNullOrWhiteSpace(managerId))
            throw new ArgumentException("Parameter 'managerId' cannot be null or whitespace");

        if (!employees.ContainsKey(managerId))
            throw new ManagerNotFoundException(managerId);

        var salaryBudget = CalculateSalaryOfDirectAndIndirectEmployees(managerId);

        return salaryBudget;
    }

    private bool CEOExists()
    {
        return employees.Values.Any(x => x.IsCEO());
    }

    private long CalculateSalaryOfDirectAndIndirectEmployees(string managerId)
    {
        var totalSalary = employees[managerId].Salary;

        var subordinates = employees[managerId].Subordinates;

        if (!subordinates.Any()) 
            return totalSalary;

        totalSalary += subordinates.Sum(CalculateSalaryOfDirectAndIndirectEmployees);

        return totalSalary;
    }

    private static IEnumerable<Employee> SplitCsvString(string csvString)
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

                return new Employee(employeeId, managerId, salary, new HashSet<string>());
            });

        return entries;
    }

    private bool IsCircularReference(Employee employee)
    {
        if (employee.IsCEO())
            return false;

        var (employeeId, managerId, _, _) = employee;

        if (employees.ContainsKey(employeeId) && employees[employeeId] is not {ManagerId: "", Salary: 0})
            return employees[employeeId].ManagerId == managerId;

        if (employees.ContainsKey(managerId!) && employees[managerId!] is not {ManagerId: "", Salary: 0})
            return employees[managerId!].ManagerId == employeeId;

        return false;
    }
}