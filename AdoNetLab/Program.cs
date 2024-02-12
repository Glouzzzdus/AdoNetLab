using DbOperations;

internal class Program
{
    private static void Main(string[] args)
    {
        var sqlDatabaseAccess = new SqlDatabaseAccess("Server=(local);Database=AdoNetLab;Integrated Security=true;");
        var dbOperations = new DbOperations.DbOperations(sqlDatabaseAccess);
    }
}