using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;

namespace SQLSelectQuery
{
    public class SQLSelectQuery
    {
        //The Assembly that is referenced from the "Call Assembly" Workflow Node must have a method called "RunCallAssembly."
        //The method must accept and return a Dictionary<string, string> Object.
        //The Dictionary Key is the Workflow Property ID and the Value is the Workflow Property Value.
        public Dictionary<string, string> RunCallAssembly(Dictionary<string, string> Input)
        {
            //Declare the Output variable that will be used to collect/return our processed data.
            Dictionary<string, string> Output = new Dictionary<string, string>();

            //property names and their ids are stored in an xml file. the name is the element, the id has to match the dictionary key.
            String mappingfile = "PropertyMapping.xml";

            if (File.Exists(mappingfile))
            {
                //we create an object to deserialize the xml to. 
                PropertyMapping propertyMap = new PropertyMapping();
                propertyMap = propertyMap.Deserialize(mappingfile);

                //Now, for each Workflow Property Value that we have in our Input, perform some sort of processing with that data.
                //In this example, we are taking a SQL Connection string and a SQL query, then running the SQL query to get a value.
                SqlConnection connection = new SqlConnection();
                SqlCommand command = new SqlCommand();

                //the dictionary's key value for the connectionstring
                connection.ConnectionString = @Input[propertyMap.ConnectionString];  //1 represents the ID of the property that contains the connection string.

                //the dictionary's key value for the SQL SELECT Query.
                String sqlQuery = Input[propertyMap.SqlStatement];
                //these values are populated by the Call Assembly workflow node
                sqlQuery = sqlQuery.Replace("#ARCHIVEID#", Input["ARCHIVEID"]);
                sqlQuery = sqlQuery.Replace("#DOCID#", Input["DOCUMENTID"]);
                sqlQuery = sqlQuery.Replace("#DATABASEID#", Input["DATABASEID"]);

                command.CommandText = sqlQuery;

                if (connection.ConnectionString != String.Empty && command.CommandText != String.Empty)
                {
                    connection.Open();
                    command.Connection = connection;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader[0] != null)
                            {
                                //The result that is returned from the SQL query is added to our return Dictionary object as Property id and Value.
                                Output.Add(propertyMap.ReturnValue, reader[0].ToString());
                            }
                            else
                            {
                                Output.Add(propertyMap.ReturnValue, "No data");
                            }
                        }
                    }
                    connection.Close();
                }
            }
            else
            {
                Output.Add("-1", "PropertyMapping.xml not found.");
            }
            //Finally, return our Output Dictionary Object that will be used set the new Values of each Workflow Property.
            //It is only necessary to return the Property ID's and Values of the Properties that are updated.
            return Output;
           
        }

        private String giveMeSpace(String path)
        {
            return path.Replace("%20", " ");
        }
    }

    /// <summary>
    /// create an object that will match the xml file
    /// </summary>
    public class PropertyMapping
    {
        public String ConnectionString { get; set; }
        public String SqlStatement { get; set; }
        public String ReturnValue { get; set; }

        public void Serialize(String file, PropertyMapping propertyMap)
        {
            System.Xml.Serialization.XmlSerializer xs
               = new System.Xml.Serialization.XmlSerializer(propertyMap.GetType());
            StreamWriter writer = File.CreateText(file);
            xs.Serialize(writer, propertyMap);
            writer.Flush();
            writer.Close();
        }

        public PropertyMapping Deserialize(string file)
        {
            System.Xml.Serialization.XmlSerializer xs
               = new System.Xml.Serialization.XmlSerializer(
                  typeof(PropertyMapping));
            StreamReader reader = File.OpenText(file);
            PropertyMapping propertyMap = (PropertyMapping)xs.Deserialize(reader);
            reader.Close();
            return propertyMap;
        }
    }
}
