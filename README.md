# FullStackEngineer-Projects
Contents of the Repository

Angular-UI - Angular UI Codebase 
Angular-UI-Dist - Angular UI Published package 
Database - DDL Script to setup the database 
WebAPI - Middle layer service build on ASP.Net WebAPI

Database Setup

Open Microsoft SQL Developer Managment Studio.
Login into to the default database.
Execute the .sql script from Database\DDL.sql.
Verfiy the new database ProjectManager from the object explorer.
Web API Setup

Open the solution from WebAPI\ProjectManager.sln.
Update the connectionString in ProjectManger\web.cofig with your database server details.
Build th solution.
Start the ProjectManager project without Debugging
Anular Code Setup

Open the folder Angular-UI in Visual Studio Code.
Open the Integrated Terminal.
Run the command npm install -g -f @angular/cli
Run the command npm install -f
Run ng serve --open to test the application from Visual Studio Code.

Deployment Step for Web API: 
1. Give proper permissions to the Webapi published code. 
2. Host the RestService_Published in IIS. 
3. Update the connectionString details in Web.config with your database details.

Angular UI Setup:
1. Give proper permissions to the Angular UI published code. 
2. Host the Angular_Frontend_Dist in IIS. 
3. Update the URL for web api service in the below files by replacing http://localhost:50830/ with service hosted IIS URL \
a. main-es2015.42dd3198b3ac27452948.js 
b. main-es5.7bbe4589883ae6a86c23.js