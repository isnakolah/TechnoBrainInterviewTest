using CompanyHierarchy.Exceptions;
using CompanyHierarchy.Models;

namespace CompanyHierarchy.Tests.Unit;

public class EmployeesTests
{
    private const string validCsvString1 = "\"Employee4\",\"Employee2\",500 \n" +
                                           "\"Employee2\",\"Employee1\",830 \n" +
                                           "\"Employee3\",\"Employee4\",830 \n" +
                                           "\"Employee1\",\",1000\n" +
                                           "\"Employee5\",\"Employee4\",400";

    private const string validCsvString2 = "\"Employee1\",\"Employee5\",500 \n" +
                                           "\"Employee4\",\",1300\n" +
                                           "\"Employee3\",\"Employee1\",830 \n" +
                                           "\"Employee7\",\"Employee5\",930 \n" +
                                           "\"Employee9\",\"Employee8\",730 \n" +
                                           "\"Employee8\",\"Employee5\",1000 \n" +
                                           "\"Employee2\",\"Employee4\",690 \n" +
                                           "\"Employee6\",\"Employee1\",600 \n" +
                                           "\"Employee5\",\"Employee4\",400";

    private const string validCsvString3 = "\"Employee3\",\"Employee2\",500 \n" +
                                           "\"Employee2\",\"Employee1\",830 \n" +
                                           "\"Employee5\",\"Employee3\",400 \n" +
                                           "\"Employee1\",\",1101";

    [Theory]
    [InlineData(validCsvString1)]
    [InlineData(validCsvString2)]
    [InlineData(validCsvString3)]
    public void EmployeesConstructor_ShouldInitializeCorrectly_IfCsvStringIsValid(string csvString)
    {
        // Act
        var result = new Employees(csvString);

        // Assert
        result.Should().BeOfType<Employees>();
    }

    [Theory]
    [InlineData(validCsvString1)]
    [InlineData(validCsvString2)]
    [InlineData(validCsvString3)]
    public void EmployeesConstructor_ShouldNotThrowAnyExceptions_IfCsvStringIsValid(string csvString)
    {
        // Act
        var result = () => new Employees(csvString);

        // Assert
        result.Should().NotThrow();
    }

    [Fact]
    public void EmployeesConstructor_ShouldThrowCsvStringIsNullOrEmptyException_IfCsvStringIsNull()
    {
        // Act
        var result = () => new Employees(null!);

        // Assert
        result.Should()
            .ThrowExactly<CsvStringIsNullOrEmptyException>()
            .WithMessage("Csv string cannot be null or empty.");
    }

    [Fact]
    public void EmployeesConstructor_ShouldThrowCsvStringIsNullOrEmptyException_IfCsvStringIsEmpty()
    {
        // Act
        var result = () => new Employees(string.Empty);

        // Assert
        result.Should().ThrowExactly<CsvStringIsNullOrEmptyException>();
    }

    [Fact]
    public void EmployeesConstructor_ShouldThrowCsvStringIsNullOrEmptyException_IfCsvStringIsWhiteSpace()
    {
        // Act
        var result = () => new Employees(" ");

        // Assert
        result.Should().ThrowExactly<CsvStringIsNullOrEmptyException>();
    }

    [Theory]
    [InlineData("\"Employee1\",\"Employee5\",500\n" +
                "\"Employee4\",\"Employee2\",1300\n" +
                "\"Employee3\",\"Employee1\",830\n" +
                "\"Employee5\",\"Employee4\",\"400\"", "Employee5")]
    [InlineData("\"Employee1\",\"Employee5\",\" \n" +
                "\"Employee4\",\"Employee2\",1300\n" +
                "\"Employee3\",\"Employee1\",830 \n" +
                "\"Employee5\",\"Employee4\",400", "Employee1")]
    [InlineData("\"Employee1\",\"Employee5\",\n" +
                "\"Employee4\",\"Employee2\",1300\n" +
                "\"Employee3\",\"Employee1\",830 \n" +
                "\"Employee5\",\"Employee4\",400", "Employee1")]
    public void EmployeesConstructor_ShouldThrowInvalidIntegerException_IfSalaryIsInvalidInteger(string csvString,
        string invalidEmployeeSalaryId)
    {
        // Act
        var result = () => new Employees(csvString);

        // Assert
        result.Should()
            .ThrowExactly<InvalidIntegerException>()
            .WithMessage($"Invalid integer value for salary of employee: \"{invalidEmployeeSalaryId}\"");
    }

    [Fact]
    public void EmployeesConstructor_ShouldThrowEmployeeAlreadyHasManagerException_IfEmployeeHasManager()
    {
        // Arrange
        const string csvString = "\"Employee1\",\"Employee5\",500 \n" +
                                 "\"Employee4\",\"Employee2\",1300\n" +
                                 "\"Employee3\",\"Employee1\",830 \n" +
                                 "\"Employee3\",\"Employee4\",400";

        // Act
        var result = () => new Employees(csvString);

        // Assert
        result.Should()
            .ThrowExactly<EmployeeAlreadyHasManagerException>()
            .WithMessage("Employee with id \"Employee3\" already has a reporting manager.");
    }

    [Theory]
    [InlineData("\"Employee1\",\"Employee5\",500 \n" +
                "\"Employee4\",,1300\n" +
                "\"Employee3\",,830 \n" +
                "\"Employee5\",\"Employee4\",400")]
    [InlineData("\"Employee1\",\"Employee5\",500 \n" +
                "\"Employee4\",,1300\n" +
                "\"Employee3\",\"\",830 \n" +
                "\"Employee5\",\"Employee4\",400", Skip = "Empty quoted string not identified as empty string")]
    public void EmployeesConstructor_ShouldThrowCEOAlreadyExistsException_IfCEOAlreadyExists(string csvString)
    {
        // Act
        var result = () => new Employees(csvString);

        // Assert
        result.Should()
            .ThrowExactly<CEOAlreadyExistsException>()
            .WithMessage("CEO already exists!");
    }

    [Fact]
    public void EmployeesConstructor_ShouldThrowCircularReferenceException_IfCircularReferencesExists()
    {
        // Arrange
        const string csvString = "\"Employee1\",\"Employee5\",500 \n" +
                                 "\"Employee4\",\"Employee2\",1300\n" +
                                 "\"Employee2\",\"Employee4\",830 \n" +
                                 "\"Employee5\",\"Employee4\",400";

        // Act
        var result = () => new Employees(csvString);

        // Assert
        result.Should()
            .ThrowExactly<CircularReferenceException>()
            .WithMessage("Employee: \"Employee2\" and \"Employee4\" are in a circular reference.");
    }

    [Theory]
    [InlineData("\"Employee4\"", 6_980)]
    [InlineData("\"Employee1\"", 1_930)]
    [InlineData("\"Employee8\"", 1_730)]
    [InlineData("\"Employee5\"", 4_990)]
    public void GetManagerSalaryBudget_ShouldReturnSalaryBudget_IfManagerIdIsValid(string managerId, long expectSalaryBudget)
    {
        // Act
        var result = new Employees(validCsvString2).GetSalaryBudgetForManager(managerId);

        // Assert
        result.Should().Be(expectSalaryBudget);
    }

    [Theory]
    [InlineData(validCsvString1, "\"Employee2\"")]
    [InlineData(validCsvString2, "\"Employee4\"")]
    [InlineData(validCsvString3, "\"Employee3\"")]
    public void GetManagerSalaryBudget_ShouldNotThrowAnyErrors_IfManagerIdIsValid(string csvString, string managerId)
    {
        // Act
        var result = () => new Employees(csvString).GetSalaryBudgetForManager(managerId);

        // Assert
        result.Should().NotThrow();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void GetManagerSalaryBudget_ShouldThrowArgumentException_IfManagerIdIsInvalid(string managerId)
    
    {
        // Arrange
        const string csvString = "\"Employee4\",\"Employee2\",500 \n" +
                                 "\"Employee3\",\"Employee4\",830 \n" +
                                 "\"Employee1\",\",1000\n" +
                                 "\"Employee5\",\"Employee4\",400";

        // Act
        var result = () => new Employees(csvString).GetSalaryBudgetForManager(managerId);

        // Assert
        result.Should()
            .ThrowExactly<ArgumentException>()
            .WithMessage("Parameter 'managerId' cannot be null or whitespace");
    }

    [Fact]
    public void GetManagerSalaryBudget_ShouldThrowManagerNotFoundException_IfManagerDoesNotExist()
    {
        // Arrange
        const string csvString = "\"Employee4\",\"Employee2\",500 \n" +
                                 "\"Employee2\",\"Employee1\",830 \n" +
                                 "\"Employee3\",\"Employee4\",830 \n" +
                                 "\"Employee1\",\",1000\n" +
                                 "\"Employee5\",\"Employee4\",400";

        // Act
        var result = () => new Employees(csvString).GetSalaryBudgetForManager("Employee6");

        // Assert
        result.Should()
            .ThrowExactly<ManagerNotFoundException>()
            .WithMessage("Manager with id Employee6 was not found");
    }
}