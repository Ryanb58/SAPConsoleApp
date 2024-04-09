using System.Configuration;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SAP.Middleware.Connector;


namespace SAPConsoleApp
{
    internal class Program
    {
        public class MyBackendConfig : IDestinationConfiguration
        {
            public RfcConfigParameters GetParameters(String destinationName)
            {
                if ("PRD_000".Equals(destinationName))
                {
                    RfcConfigParameters cparams = new RfcConfigParameters();

                    /* Default connection details */
                    cparams.Add(RfcConfigParameters.PoolSize, ConfigurationManager.AppSettings.Get("PoolSize"));
                    cparams.Add(RfcConfigParameters.Language, ConfigurationManager.AppSettings.Get("Language"));

                    /* SAP connection details */
                    cparams.Add(RfcConfigParameters.AppServerHost, ConfigurationManager.AppSettings.Get("AppServerHost"));
                    cparams.Add(RfcConfigParameters.SystemNumber, ConfigurationManager.AppSettings.Get("SystemNumber"));
                    cparams.Add(RfcConfigParameters.Client, ConfigurationManager.AppSettings.Get("Client"));
                    cparams.Add(RfcConfigParameters.SystemID, ConfigurationManager.AppSettings.Get("SystemID"));

                    /* SAP username & password Auth */
                    //cparams.Add(RfcConfigParameters.User, ConfigurationManager.AppSettings.Get("User"));
                    //cparams.Add(RfcConfigParameters.Password, ConfigurationManager.AppSettings.Get("Password"));
                    //cparams.Add(RfcConfigParameters.User, "");
                    //cparams.Add(RfcConfigParameters.Password, "");

                    /* SAP SNC Auth */
                    cparams.Add(RfcConfigParameters.SncQOP, ConfigurationManager.AppSettings.Get("SncQOP"));
                    cparams.Add(RfcConfigParameters.SncMode, ConfigurationManager.AppSettings.Get("SncMode"));
                    cparams.Add(RfcConfigParameters.SncPartnerName, ConfigurationManager.AppSettings.Get("SncPartnerName")); /* SAP Subject name on the server itself. */
                    cparams.Add(RfcConfigParameters.SncMyName, ConfigurationManager.AppSettings.Get("SncMyName")); /* SAP certificate subject name of the end user. */
                    cparams.Add(RfcConfigParameters.SncLibraryPath, ConfigurationManager.AppSettings.Get("SncLibraryPath"));
                    cparams.Add(RfcConfigParameters.SncSSO, ConfigurationManager.AppSettings.Get("SncSSO"));
                    cparams.Add(RfcConfigParameters.X509Certificate, ConfigurationManager.AppSettings.Get("X509Certificate"));
                    cparams.Add(RfcConfigParameters.Trace, "2");

                    //RfcTrace.DefaultTraceLevel = RfcTracing.Level2;

                    RfcTrace.TraceDirectory = "C:\\sap\\nco_trace_logs";

                    return cparams;
                }
                else
                {
                    return null;
                }
            }
            // The following two are not used in this example:
            public bool ChangeEventsSupported()
            {
                return false;
            }
            public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;
        }

        static void Main(string[] args)
        {       
            RfcDestinationManager.RegisterDestinationConfiguration(new MyBackendConfig()); 

            // Create the connection to SAP
            RfcDestination destination = RfcDestinationManager.GetDestination("PRD_000");
            destination.Ping();

            // Get the repository and create the function
            RfcRepository repo = null;
            try
            {
                repo = destination.Repository;
            }
            catch (RfcLogonException ex)
            {
                throw ex;
            }

            // Create the function call
            IRfcFunction function = repo.CreateFunction("STFC_CONNECTION");


            // Set the import parameters
            function.SetValue("REQUTEXT", "Hello World :)!");

            // Execute the function
            function.Invoke(destination);

            // Get the export parameters
            string echotext = function.GetString("ECHOTEXT");
            string responseText = function.GetString("RESPTEXT");
            string requtext = function.GetString("REQUTEXT");

            // Print the results
            Console.WriteLine("Echo Text: " + echotext);
            Console.WriteLine("Response Text: " + responseText);
            Console.WriteLine("REQU Text: " + requtext);
            Console.ReadLine();
        }
    }
}
