using System;
using Topshelf;

namespace SaavorMissingOrderPrint
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x=>{
                x.Service<ServiceInitiated>(s=>
                {
                    s.ConstructUsing(ServiceInitiated => new ServiceInitiated());
                    s.WhenStarted(ServiceInitiated => ServiceInitiated.Start());
                    s.WhenStopped(ServiceInitiated => ServiceInitiated.Stop());
                });

                x.RunAsLocalSystem();
                x.SetServiceName("SaavorPOSMissingOrderPrint");
                x.SetDisplayName("SaavorPOSMissingOrderPrint");
                x.SetDescription("This service is designed to get the POS orders that have not been printed yet");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
