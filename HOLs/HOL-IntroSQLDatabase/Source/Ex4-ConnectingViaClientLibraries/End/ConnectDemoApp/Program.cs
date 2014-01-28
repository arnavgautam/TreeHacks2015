using System;

namespace ConnectDemoApp
{
    public class Program
    {
        /// <summary>
        /// Connection Variables. Please replace the values with those discussed in the lab.
        /// </summary>
        private static string userName = "[USERNAME]";
        private static string password = "[PASSWORD]";
        private static string datasource = "[SERVER].database.windows.net";
        private static string databaseName = "HoLTestDB";

        public static void Main(string[] args)
        {
            // Invoke the ADO.NET connection demo
            Console.WriteLine("Starting the ADO.NET Connection Demo..");
            AdoConnectionDemo demo1 = new AdoConnectionDemo();
            demo1.ConnectToSQLDatabase(userName, password, datasource, databaseName);
            Console.WriteLine("Demo Complete.. Press any key");
            Console.ReadKey();

            // Invoke the ODBC connection demo
            Console.WriteLine("Starting the ODBC Connection Demo..");
            OdbcConnectionDemo demo2 = new OdbcConnectionDemo();
            demo2.ConnectToSQLDatabase(userName, password, datasource, databaseName);
            Console.WriteLine("Demo Complete.. Press any key");
            Console.ReadKey();

            // Invoke the OleDB connection demo
            Console.WriteLine("Starting the OLEDB Connection Demo..");
            OleDbConnectionDemo demo3 = new OleDbConnectionDemo();
            demo3.ConnectToSQLDatabase(userName, password, datasource, databaseName);
            Console.WriteLine("Demo Complete.. Press any key");
            Console.ReadKey();

            // Invoke the EF connection demo
            Console.WriteLine("Starting the Linq to SQL Connection Demo..");
            EFConnectionDemo demo4 = new EFConnectionDemo();
            demo4.ConnectToSQLDatabase();
            Console.WriteLine("Demo Complete.. Press any key");
            Console.ReadKey();
        }
    }
}
