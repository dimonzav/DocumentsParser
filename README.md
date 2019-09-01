# Documents Parser

Documents Parser is a Console Application using command line parameters to run a few methods to parse specified log files.

To run the application:

1. Clone the repository "$ git clone https://github.com/dimonzav/DocumentsParser.git".

2. Open the project solution in Visual Studio and check connection string in "appsettings.json" under "DataAccess" project.

3. Build the solution.

4. Run the application:
- go to folder where solution is build;
- run from command line "dotnet DocumentsParser.dll" and check the available options list.

5. There are two methods available to run the parser from:
- Run document parser using Parallel.ForEach method;
- Run document parser using BlockingCollection with running two parallel tasks.

6. For using Parallel.ForEach run "dotnet DocumentsParser.dll a -f "path/to/folder/with/logs".

7. For using BlockingCollection run "dotnet DocumentsParser.dll b -f "path/to/folder/with/logs".

8. By default parsed logs from specific files do not save to database. To enable it use option "-s" and value "true".

- "dotnet DocumentsParser.dll a -f "path/to/folder/with/logs -s true"
