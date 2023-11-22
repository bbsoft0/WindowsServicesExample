# WindowsServiceConsole
Windows Services using TopShelf - Complete usage using Files Converter deployed as service.

Complete solution with all important usages.

## Reasons to use Windows Services

* Run even when no user is logged in
* Start automatically on machine boot
* Run as different users (built-in)
* Start, Stop, Pause, Resume Services
* Auto Restart - failure policy easy to implement
* Dependencies on other services running
* Manage froom remote machines (stop)
* Monitoring via Windows event log
* System integration ( file ingestion and data transfer between systems)
* Health / heartbeat monitoring ( Execute API periodically)

## Service Log On Accounts

*Local System - Privileges on local machine, not recommended for running custom services
*Local Service - Presents as anonymous on network
*Network Service - Machine Account on network
*Local User custom account - User account created on local, permissions can be customized
*Domain User custom account - User account created on domain, permissions can be customized. Network file shares, domain services

For security, always use an account that has the minimum permissions required for the service to do its job

## Service Recovery Options
### Recovery actions
* Restart the service (after n minutes)
* Take no action
* Run a program
* Restart the computer (after n minutes)

### Multiple recovery actions
* First failure
* Second failure
* Subsequent failures
* Reset failure count after n days

## windows service control manager
The windows service control manager - can be seen as services.exe in Task Manager
* sc.exe - can be used on the command line
* maintain database of installed services
* starting, stopping, pausing, resuming of services
* querying installed services and service state
* sending control messages to services
  

## Installation and usage 

1. Create .NETCore console app.
	
2. Right click on references and go to manage NuGet-Packages

3. Download and install Topshelf via NuGet

4. Paste the Code below into your application and include all imports.

5. Switch from “Debug” mode to “Release” and build the application.

6. Run cmd.exe as administrator
   
8. Navigate the console to
```
 .\myConsoleApplication\bin\Release\
```

8. Install the service with
```
myConsoleApplication.exe install
```

10. Run the service without arguments and it runs like console app.
11. Run the service with **action:install** and it will install the service.
12. Run the service with **action:uninstall** and it will uninstall the service.
13. Run the service with **action:start** and it will start the service.
14. Run the service with **action:stop** and it will stop the service.
9. Run the service with **action:pause** and it will pause the service.
9. Run the service with **action:continue** and it will continue the service.
10. Run the service with **username:YOUR_USERNAME**, **password:YOUR_PASSWORD** and **action:install** which installs it for the given account.
11. Run the service with **built-in-account:(NetworkService|LocalService|LocalSystem)** and **action:install** which installs it for the given built in account. Defaults to **LocalSystem**.
12. Run the service with **description:YOUR_DESCRIPTION** and it setup description for the service.
13. Run the service with **display-name:YOUR_DISPLAY_NAME** and it setup Display name for the service.
14. Run the service with **name:YOUR_NAME** and it setup name for the service.
15. Run the service with **start-immediately:(true|false)** to start service immediately after install. Defaults to **true**.

## Example and explanation

in Service class add methods
```
public bool Start() {..}
public bool Pause() {..}
public bool Continue() {..}
public bool Stop() {..}
public bool CustomCommand() {..}
public bool FileCreated() {..}
```

in Program.cs Main :
```
HostFactory.Run(serviceConfig =>
{
    serviceConfig.UseNLog();

    serviceConfig.Service<ConverterService>(serviceInstance =>
    {
        serviceInstance.ConstructUsing(() => new ConverterService());
        serviceInstance.WhenStarted(execute => execute.Start());
        serviceInstance.WhenStopped(execute => execute.Stop());
        serviceInstance.WhenPaused(execute => execute.Pause());
        serviceInstance.WhenContinued(execute => execute.Continue());
        serviceInstance.WhenCustomCommandReceived((execute, hostControl, commandNumber) => execute.CustomCommand(commandNumber));
    });

    serviceConfig.EnableServiceRecovery(recoveryOption =>
    {
        recoveryOption.RestartService(1);   //1 minute delay    
        recoveryOption.RestartComputer(60,"Demo");
        recoveryOption.RunProgram(5, @"C:\\Windows\\System32\\notepad.exe");
        recoveryOption.SetResetPeriod(1);
    });

    serviceConfig.EnablePauseAndContinue();
    serviceConfig.SetServiceName("DemoFileConverterService");
    serviceConfig.SetDisplayName("Demo File Converter Service");
    serviceConfig.SetDescription("This service converts files from lower case to upper case.");
    serviceConfig.StartAutomatically();
});
```

The service uses NLog package to write logs on a text file.

The line who starts the service in auto mode is 
```
serviceConfig.StartAutomatically();
```
    


## License

[MIT](https://github.com/bbsoft0/WindowsServicesExample/blob/master/LICENSE)
