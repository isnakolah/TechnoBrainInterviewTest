namespace CompanyHierarchy.Exceptions;

internal sealed class CsvStringIsNullOrEmptyException : Exception
{
    public CsvStringIsNullOrEmptyException()
        : base("Csv string cannot be null or empty.")
    {
    }    
}