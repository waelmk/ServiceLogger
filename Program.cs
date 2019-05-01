using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MyWinService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ScheduledWinService()
            };

            // On est en mode intéractif et débogage ?
            if (Environment.UserInteractive && System.Diagnostics.Debugger.IsAttached)
            {

                new ScheduledWinService().start();

            }
            else
            {
                // Exécute les services normalement
                ServiceBase.Run(ServicesToRun);
            }

        }
        static void RunInteractiveServices(ServiceBase[] servicesToRun)
        {
            Console.WriteLine("Démarrage des services en mode intéractif.");


            // Récupération de la méthode a exécuter sur chaque service pour le démarrer
            MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (ServiceBase service in servicesToRun)
            {
                Console.Write("Démarrage de {0} ... ", service.ServiceName);
                onStartMethod.Invoke(service, new object[] { new string[] { } });
                Console.WriteLine("Démarré");
            }

        }
    }
}
