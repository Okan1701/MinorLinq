# Info
MinorLinq is a modular and extendable LINQ-to-SQL ORM that it capable of translating LINQ queries to SQL and executing it on the database. It also deserializes the results into a collection of C# objects. The project uses .NET Core 3.1

# Setting up
The project contains the library itself and a small demo to show the basic functionality. In order to run the program, do the following steps:
* In the root folder, open the MinorLinq folder (the one that contains the .csproj file)
* Download all the dependancies using `dotnet restore`
* Run the code via `dotnet run`