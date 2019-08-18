# MovieTime


## Requirements to build and run locally: 
* .net core 2.2 sdk
* sql server or sql server express
* ssms or azure data studio 
* npm

## Process:
* Install sql server and start new sql server instance
* Deploy MovieTimeDb project to database on your desired sql server instance
* change connection string in MovieTime/Appsettings.json for MovieTimeDb 
* if building and running through command line: "dotnet build" "dotnet run" in the MovieTime web project
* If running through visual studio: normal build and debug with iisexpress or create new iis site/app pool
