If you want to view this file with its intended text formatting, it is recommended that you view it in your git repository, download a markdown editor or use one online.
  - (online) https://dillinger.io
  - https://www.typora.io/

## Prerequisites

Install the following applications:
  - Node.js (v12+) (https://nodejs.org/en/download/)
  - Visual Studio Code (https://code.visualstudio.com/download)
  - Visual Studio 2019 IDE (https://visualstudio.microsoft.com/vs/community/)
  - Azure Storage Emulator: (https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator)

Optional:
  - Azure Storage Explorer (https://azure.microsoft.com/en-us/features/storage-explorer/)
  - Microsoft SQL Server Management Studio 18
(https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15)
  - Sourcetree graphic interface for Git (https://www.sourcetreeapp.com)

## Project files
###### If you are planning on interacting with the existing repository or just want to have a quick access to the latest updates - create a complete local copy of the repository by cloning it to your device.
   ##### Copy the Clone URL
  - Open Azure DevOps > Projects > XplicityApplication 
  (or navigate to https://dev.azure.com/xplicity/_git/XplicityApplication)
  - Repos (left sidebar) > Files > Clone (at the top right corner of the dashboard) > Copy Clone URL to clipboard.

   ##### Create a local repository  
  - Open Sourcetree > File > Clone / New... > Paste copied Clone URL into **Source Path / URL** field > Browse for a desired Destination Path > Clone.
 
   ##### Basic Git operations
  - **Fetch**. Downloads contents from a remote repository, allows you to see the newest changes, but doesn't merge them with your local repository.
  - **Pull**. Merges the retrieved branch into your local repository.
Read about other operations here: https://confluence.lsstcorp.org/display/LDMDG/Basic+Git+Operations

# Run the application
## Start Azure Storage Emulator
In Windows Start Menu type ```Microsoft Azure Storage Emulator``` and launch Microsoft Azure Storage Emulator application.
Alternatively you can run it using using any terminal. For example, to start Azure Storage Emulator in PowerShell (or in Visual Studio's Package Manager Console) type:
```powershell
 & "C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe" start
```

## Run the application (Easy mode)
 * Open the solution (**.sln** file in the root of the repository) using Visual Studio.
 * Build the solution (**F6** while in Visual Studio)
 * Update database - open package manager console in Visual Studio and type **Update-Database** and enter
 * Change the debugging profile to Kestrel - there's a dropdown on the debug button in the toolbar (by default it's set to 'IIS Express'), choose 'Xplicity_Holidays' from the dropdown
 * Run the application (**F5** or the Debug button in the toolbar)

## Running front-end and back-end separately
### Run front-end files
The very first time you open Visual Studio Code, you should:
open Terminal (in the tools bar) > New Terminal > type **npm install -g @angular/cli** > click Enter.
You can also do this using Command Prompt or Windows Powershell.

**ClientApp** - the directory where your Angular project file resides. It should contain **src** and **e2e** folders. The full path should look like this: **..\XplicityApplication\Xplicity Holidays\ClientApp**. You will need to paste the full path everywhere **ClientApp** is mentioned.

##### To run the project:
Type in Terminal
```sh
$ cd 'ClientApp'
$ npm install
$ ng serve 
```
  - **cd** changes the current directory. If there is ever an error indicating missing project file, type **cd 'ClientApp'** (full path).
  - **npm install** is necessary every time the project is updated from remote sources (in case new libraries were added by fellow developers). After its first-time execution **node-modules** folder should appear in **ClientApp**, containing all of the necessary libraries.
  - **ng serve** will compile and run the front end of your application.
  - (for developers) to open the code, you could either type **. code** or go to File > Open Folder > search for **ClientApp** directory > Open.
  - don't forget to add typewriter apostrophes (') on both sides of 'ClientApp'. This is to prevent you from getting an error (e.g. if there is a space symbol in your directory name).

Open your browser > in the address bar type the RootUrl (default: https://localhost:4200) > click Enter.
Where to find the RootUrl in case it was changed? Go to **XplicityApplication\Xplicity Holidays** (the back-end directory of the application), open **appsettings.json** file, it should be under **AppSettings**.

Once the page loads, you should see the log-in form, but no interactivity.

#### A few important notes
  - Every time the project is opened anew, it should be run with **ng serve**. If you haven't closed the project, it will be compiled every time a new change has been saved.
  - It is possible that some time in the future you will encounter this error - "Port 44374 is already in use".
Open cmd or PoweShell > type **netstat -a -o -n** > find the line which contains **44374** > copy the last number > **taskkill /F /PID** (last number) > the process will be terminated
```sh
$  netstat -a -o -n
$  TCP    127.0.0.1:44374         0.0.0.0:0              LISTENING       51912
$  taskkill /F /PID 51912
```

## Prepare back-end project
Go to **XplicityApplication\Xplicity Holidays** (the back-end directory of the application), double-click on **Xplicity Holidays.sln**. The project will be opened in Visual Studio 2019.

#### Prepare the database
In Visual Studio 2019 click on View > Other Windows > Package Manager Console > type **Update-Database** (this will apply any pending migrations to the database) > click Enter.
If there are any errors, you can delete the Migrations folder, then in the console type **Add-Migrations Initial**.

Before running the project, it's important to have some data in the database. 
Open **Microsoft SQL Server Management Studio 18** and connect with these settings:
 - Server type: Database Engine
 - Server name: localhost\sqlexpress (check **appsettings.json** file to see if nothing changed, should be **localhost\\SQLEXPRESS**)
 - Authentication: Windows Authentication

   ##### Where are the newly created tables and how to manage their data?
Open the Server Management Studio > check out its Object Explorer window > expand Databases > check to see if there is a catalog entitled **XplicityHolidays**.
If you expand it, you can find its tables, see their Design and edit Rows.

#### Initial admin data 
Initial admin is created when you run update-database command and start your project.
Email: gamma.holidays@gmail.com
Password: password

## Important when running front-end and back-end separately
Open **Startup.cs** and find configuration for single page application (Spa) at the bottom. By default it's set to *UseAngularCliServer* - you need to comment that line and uncomment the next one - *UseProxyToSpaDevelopmentServer(spaUrl)*.
This will link your *ng serve'd* instance with your asp.net core application.

## Run back-end files
After all of the instructions above have been followed, go back to Visual Studio 2019 where the back-end project is open. Press **CTRL+F5** keys at the same time, the project will be compiled and built. Now that the log-in form in the browser is interactive, you can log-in with admin data.

