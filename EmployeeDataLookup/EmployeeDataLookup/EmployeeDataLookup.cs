using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;

namespace EmployeeDataLookup
{
    public class EmployeeDataLookup
    {
        //The Assembly that is referenced from the "Call Assembly" Workflow Node must have a method called "RunCallAssembly."
        //The method must accept and return a Dictionary<string, string> Object.
        //The Dictionary Key is the Workflow Property ID and the Value is the Workflow Property Value.
        public Dictionary<string, string> RunCallAssembly(Dictionary<string, string> Input)
        {
            //Now, for each Workflow Property Value that we have in our Input, perform some sort of processing with that data.
            //In this example, we are taking a SQL Connection string and a SQL query, then running the SQL query to get a value.
            SqlConnection connection = new SqlConnection();
            SqlCommand command = new SqlCommand();

            //Set the SQL Connection String
            connection.ConnectionString = @"Data Source=(local)\GETSMART;Initial Catalog=Employees;Integrated Security=SSPI;MultipleActiveResultSets=true;";

            //Declare the Output variable that will be used to collect/return our processed data.
            Dictionary<string, string> Output = new Dictionary<string, string>();

            //Run the SQL Query for each Property that is passed to the Assembly. For this example, each Property is a SQL column name that we are pulling data from.
            foreach (KeyValuePair<string, string> Property in Input)
            {
                connection.Open();
                command.CommandText = "SELECT TOP 1 " + Property.Value + " FROM [Employees].[dbo].[Employee_Data] ORDER BY [Votes] DESC";
                command.Connection = connection;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader[0] != null)
                        {
                            //The result that is returned from the SQL query is added to our return Dictionary object as the Property's new Value.
                            Output.Add(Property.Key, reader[0].ToString());
                        }
                    }
                }
                connection.Close();
            }

            //Finally, return our Output Dictionary Object that will be used set the new Values of each Workflow Property.
            //It is only necessary to return the Property ID's and Values of the Properties that are updated.
            return Output;
        }
    }
}
