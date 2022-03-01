using CompanyHierarchy.Exceptions;
using CompanyHierarchy.Models;

namespace CompanyHierarchy.Tests.Unit;

public class EmployeesTests
{
    private const string validCsvString1 = "\"Employee4\",\"Employee2\",500 \n" +
                                           "\"Employee3\",\"Employee4\",830 \n" +
                                           "\"Employee1\",\",1000\n" +
                                           "\"Employee5\",\"Employee4\",400";

    private const string validCsvString2 = "\"Employee1\",\"Employee5\",500 \n" +
                                           "\"Employee4\",\",1300\n" +
                                           "\"Employee3\",\"Employee1\",830 \n" +
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
        result.Should().ThrowExactly<CsvStringIsNullOrEmptyException>();
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
    public void EmployeesConstructor_ShouldThrowInvalidIntegerException_IfSalaryIsInvalidInteger(string csvString, string invalidEmployeeSalaryId)
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
        var csvString = "\"Employee1\",\"Employee5\",500 \n" +
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


    [Fact]
    public void EmployeesConstructor_ShouldThrowCEOAlreadyExistsException_IfCEOAlreadyExists()
    
    {
        // Arrange
        var csvString = "\"Employee1\",\"Employee5\",500 \n" +
                        "\"Employee4\",,1300\n" +
                        "\"Employee3\",,830 \n" +
                        "\"Employee5\",\"Employee4\",400";

        // Act
        var result = () => new Employees(csvString);
        
        // Assert
        result.Should().ThrowExactly<CEOAlreadyExistsException>();
    }

    [Fact]
    public void EmployeesConstructor_ShouldThrowCircularReferenceException_IfCircularReferencesExists()
    {
        // Arrange
        var csvString = "\"Employee1\",\"Employee5\",500 \n" +
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

    [Fact]
    public void GetSalaryBudgetForManager_ShouldReturnSalaryBudget_IfCsvStringValid()
    {
        // Arrange
        var csvString = "\"Employee1\",\"Employee5\",500 \n" +
                        "\"Employee4\",\"Employee2\",1300\n" +
                        "\"Employee3\",\"Employee1\",830 \n" +
                        "\"Employee5\",\"Employee4\",400";
        
        // Act
        var result = new Employees(csvString).GetSalaryBudgetForManager("Employee1");
        
        // Assert
        result.Should().Be(1300);
    }
}