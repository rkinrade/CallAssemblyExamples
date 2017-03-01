SQL Select Query Demo
======================

The following is a sample application extension that can be used in a GlobalAction "Call Assembly" Workflow node.

The current binary can be found here:  http://www.square-9.com/dlls/SQLSelectQuery.dll

Usage of the binary will require the dll be placed in your server's GetSmart folder for GlobalAction, or your server's GetSmart\CaptureServices\GlobalCapture_1 folder for GlobalCapture.  You will also need to add an XML config file to 
map property id's to the correct fields.  PropertyMapping.xml should be formatted as follows, where the property ID reflects the correct element in the workflow and the WorkflowID matches the value you are passing as a Process Field value:

```xml
<PropertyMapping>
  <PropertyMap>
    <WorkflowID>SAMPLE</WorkflowID>
    <ConnectionString>1</ConnectionString>
    <SqlStatement>2</SqlStatement>
    <ReturnValue>3</ReturnValue>
  </PropertyMap>
  <PropertyMap>
    <WorkflowID>eF37bs64D</WorkflowID>
    <ConnectionString>4</ConnectionString>
    <SqlStatement>5</SqlStatement>
    <ReturnValue>6</ReturnValue>
  </PropertyMap>
</PropertyMapping>
```
In order for the DLL to know which IDs to use for the ConnectionString, SQLStatement, and ReturnValue you must pass the WorkflowID to the assembly. This is accomplished by creating a process field in your GlobalAction or GlobalCapture workflow and setting the value of that process field to WorkflowID=SAMPLE. The left side of the equal sign is Case Sensitive. The right side of the equal sign is not. So WorkflowID=Sample will match and give you the correct values from PropertyMapping.xml, but workflowid=SAMPLE will not.

If the system cannot find the WorkflowID in the process fields the batch will error out and tell you that the WorkflowID was not found in the Workflow Fields. If the WorkflowID is not found in the PropertyMapping.xml file, the batch will error and let you know that it was not found in PropertyMapping.xml.

This is a quick and useful way to pull data from a SQL database using a document's index data in a GlobalAction workflow.
Use this method if you want:

- the SQL query to be dynamically set within the GlobalAction or GlobalCapture Workflow
- the ability to return a single value

The assembly takes in 2 Workflow Property values, a SQL Connection String, and a SQL SELECT query, and 
returns the SQL query's result as the third Property's value.

To support 

The assembly performs the following steps:
1) The Input Dictionary object is used to set the WorkflowID
2) The WorkflowID is used to set Property 1 as the SQL Connection string; Property 2 is set as the SQL Query (for WorkflowID=SAMPLE).
4) The SQL query is executed.
5) If a result is returned from the query, it is added to the Output Dictionary object with ID 3 and returned (for WorkflowID=SAMPLE). If no result is returned from the query, the value "No Data" is added to the Output Dictionary object with ID 3 and returned (for WorkflowID=SAMPLE). Lastly, if the WorkflowID is missing from either the Input Dictionary object or PropertyMapping.xml an error message is added to the Output Dictionary object with ID -1 and returned.

Key Notes:
- The method is called "RunCallAssembly." Any assembly that is being referenced by the Call Assembly node must have a 
  method with this name. This is the method that will execute when the assembly is loaded.
- The method accepts and returns a Dictionary<string, string> object. The Input dictionary is the list of all 
  workflow Properties that have been passed to Call Assembly. The Output dictionary only needs to return the list 
  of properties that are being updated with their new values.
- There must be a workflow Property passed to the Call Assembly that has a value of WorkflowID=IDValue. The left side of the equal sign is case sensitive, while the right side of the equal sign is not case sensitive.
