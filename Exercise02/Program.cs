using Exercise01.Extensions;
using Exercise02.Exceptions;

Console.WriteLine("Hello, welcome to the number to string converter.");

Console.WriteLine("Write the number separating them with commas like: 12,000,000");

while (true)
{
    try
    {
        Console.Write("Number to convert: ");

        var userInput = string.Join("", Console.ReadLine()?.Split(',') ?? Array.Empty<string>());
        
        // seperate int and biginterger numbers, if number is greater than 2 billion, make biginterger

        if (!int.TryParse(userInput, out var num))
            throw new InvalidInputException();

        Console.WriteLine(num.Towards());
    }
    catch (IOException ex)
    {
        Console.WriteLine(ex.Message);
        break;
    }
    catch (InvalidInputException)
    {
        Console.WriteLine("Your input is not valid. Try again.");        
    }
}