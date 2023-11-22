using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace FileConverterService
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
                    recoveryOption.RestartService(1);   //1 miunte delay    
                    recoveryOption.RestartComputer(60,"PS Demo");
                    recoveryOption.RunProgram(5, @"C:\\Windows\\System32\\notepad.exe");
                    recoveryOption.SetResetPeriod(1);
                });

                serviceConfig.EnablePauseAndContinue();
                serviceConfig.SetServiceName("DemoFileConverterService");
                serviceConfig.SetDisplayName("Demo File Converter Service");
                serviceConfig.SetDescription("This service converts files from lower case to upper case.");
                serviceConfig.StartAutomatically();


            });
        }
    }
}
