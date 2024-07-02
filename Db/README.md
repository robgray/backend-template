# README #
This readme is to provide some guidance and use of this console application.
The main purpose of this app is to be a script runner which is invoked by Octopus Deploy as part of the CI/CD pipeline to ensure the database is consistent and up to date.    
    
However, it can easily be run manually (ideal for building up a new local database).    


### Dry Run ###
You can add a command line flag (_PreviewReportPath_) which will generate an HTML report named **UpgradeReport.html**. This will show the scripts which it would have run, had it been run without that flag e.g.    

```C:\YourDirectory>Vald.TeleHab.Library.DatabaseUpdater.exe --PreviewReportPath="C:\updater"``` 

This is effectively a dry run.

It is also used as a deployment step on Octopus, to create an artifact for the scripts that will be run on that deployment.

### How it Works ###
DbUp adds new table SchemaVersions to the database (if it does not already exist).  

For scripts in the DeploymentScripts directory, it inserts the names of the scripts which it has executed into that table . This is how it tracks the state of the database. So, if it finds a script in that table, it will not execute it again.  

But there are times when it would be required to always run a script or set of scripts. An example is a post-deployment script to refresh all the views. Alternatively, a script to rebuild all the indexes and regenerate stats. You don’t want to write a new script for each deployment. For these scenarios, add the scripts in the BeforeDeploymentScripts and PostDeploymentScripts directories.  
  
The scripts in all folders start from 0001 e.g. ```0001_<TicketNumber>-AddColumnYToTableX.sql```  
So, each folder has it's own 0001 script.  
  


### Flags ###
These are the command line flags which you can pass to the application.  
1. connectionString - this is injected by Octopus, where it is stored as a Variable
2. previewReportPath - instead of actually running the scripts, it creates an html report of those scripts which would be run
3. timeout - this enables you to bump up the timeout if you do not want to risk having a massive script fail
4. reportName - if you wanted to name it something other than the default UpgradeReport.html
5. firstRunOnExistingDb - for an edge case when replacing EF Migrations for an existing database. See below.


### Edge Case - Replacing Migrations ###
You may find yourself in a scenario where you have an existing database, complete with data which has been managed using EF Migrations.    
The reason for replacing this is so that a new developer can just run this application on their machine and the dev database will be brought up to date. Also, having EF migrations manage the database evolution to a point, then have DbUp take it over would be a maintenance burden.    
So, the idea is to use DbUp as if it has always managed the database. But we don't want to run the historical scripts against the existing database.1   
To enable this a flag called **firstRunOnExistingDb** was added. This will:
1. create ensure that the **SchemaVersions** table has been created; and
2. insert the names of the existing scripts at cutover so that those scripts won't be run against the existing database. 

### Examples ###    
A normal run scenario:    
```C:\YourDirectory>Vald.TeleHab.Library.DatabaseUpdater.exe --timeout="400" --connectionString="Server=servername,1433;Initial Catalog=somedb;Persist Security Info=False;User ID=DbUser;Password=complexpassword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3200;"``` 

In a Dev environment, where you are getting the connectionstring from UserSecrets inside the app:    
```C:\YourDirectory>Vald.TeleHab.Library.DatabaseUpdater.exe``` 

Edge case where replacing EF Migrations:    
```C:\YourDirectory>Vald.TeleHab.Library.DatabaseUpdater.exe --firstRunOnExistingDb="true" --connectionString="Server=servername,1433;Initial Catalog=somedb;Persist Security Info=False;User ID=DbUser;Password=complexpassword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3200;"```

Create HTML report of the scripts which would be run:     
```C:\YourDirectory>Vald.TeleHab.Library.DatabaseUpdater.exe --previewReportPath="D:\Artifacts" --connectionString="Server=servername,1433;Initial Catalog=somedb;Persist Security Info=False;User ID=DbUser;Password=complexpassword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3200;"```