Employee Data Lookup Demo
=================

The following is a sample application extension that can be used in a GlobalAction "Call Assembly" Workflow node.
This is a quick and useful way to pull data from a SQL database using a document's index data in a GlobalAction workflow.
Use this method if you want:
- the SQL query to be statically set within the assembly
- the ability to return a set of single values

The assembly takes in any given number of Worklfow Property values, which represent 
the names of the SQL columns we will be pulling data from.

The assembly performs the following steps:
1) A static SQL query is set within the Assembly code.
2) For each Property that is passed to the Assembly code, run the SQL Query. Each Property value is the name of 
   a SQL column that we are selecting data from.
3) For each result that is returned from the Query, the corresponding Property value is updated to be the 
   query's returned value.

Key Notes:
- The method is called "RunCallAssembly." Any assembly that is being referenced by the Call Assembly node must have a 
  method with this name. This is the method that will execute when the assembly is loaded.
- The method accepts and returns a Dictionary<string, string> object. The Input dictionary is the list of all 
  workflow Properties that have been passed to Call Assembly. The Output dictionary only needs to return the list 
  of properties that are being updated with their new values.