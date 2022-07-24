# Suit Alteration Manager
## Technologies Adopted
* Database -> Microsoft SQL Server
* Backend -> .NET6

# Structure
## Database
 this folder contains the Database project.
You can publish it in a runnig server by right click on the project and then on "Publish...".

It will create a new database (create if not exists, update otherwise) and then automatically execute a SQL script that insert a demo user.

**NOTE:** this user is the one that you can use to authenticate and retrieve the JWT token that is **MANDATORY** to consume the APIs.

Following the credentials of the user : 

* email --> admin@mail.it
* psw --> demo

If you need a SQL server instance to run the project locally you can create a new docker container in your local PC by using the file 'docker-compose-sql.yml' in the root folder : 

You can run 
`docker compose -f .\docker-compose-sql.yml up` inside the root directory.

## Backend
this folder contains the whole structure of the project.

Use VisualStudio to run the api; alternatively you can use the command `dotnet run ` inside the directory of the api project.

It contains an AzureFunction to consume a message in a Azure Service Bus Topic.

In order to test it locally you can start it in debug and write manually a message in the topic (ie. from the Azure Portal).