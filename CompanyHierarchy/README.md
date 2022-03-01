## About

This is a C# .NET console application with a Employees model class with logic for handling

## Running it

- Follow the [solutions readme](https://github.com/isnakolah/TechnoBrainInterviewTest/) for initial setup.

- On your favorite terminal and navigate to parent directory(same level as solution file)
  - Run command `dotnet test --project ./CompanyHierarchy.Tests.Unit/CompanyHierarchy.Tests.Unit.csproj` to run the unit tests for this project
  - Run command `dotnet run --project ./CompanyHierarchy/ComponayHierachy.csproj` to run the program

## Limitations

- Empty strings in the csv file wrapped with another pair of quotations so the string.IsNullOrWhitespace reads this as not an empty string. This results in multiple CEOs being added. Could be fixed by removing the strings
- No logic to check if managerial chain reaches the CEO
- Double quotations in the strings