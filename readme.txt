This is simple Cron expression parser console application.
The console app parse cron expression (passed via args) and print an explanation to each field in the expression on console/terminal 

The application is compiled against .NET 6

compilation pre-requisities
---------------------------
.NET 6 SDK should be installed.

Usage steps
-----------
1) Open "CronParser.App" project folder in terminal 
2) Generate release build using following command "dotnet build --configuration Release" and assure that there are no compilation errors.
3) navigate to "bin\Release\net6.0" sub-folder.
4) launch the console app via "CronParser.App.exe" and pass cron expression as argument to EXE.
	ex: CronParser.App.exe */15 0 1,15 * 1-5 /usr/bin/find