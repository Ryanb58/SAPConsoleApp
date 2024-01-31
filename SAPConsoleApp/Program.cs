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
                    RfcConfigParameters parms = new RfcConfigParameters();

                    /* Default connection details */

                    parms.Add(RfcConfigParameters.PoolSize, ConfigurationManager.AppSettings.Get("PoolSize"));
                    parms.Add(RfcConfigParameters.Language, ConfigurationManager.AppSettings.Get("Language"));

                    /* SAP connection details */
                    parms.Add(RfcConfigParameters.AppServerHost, ConfigurationManager.AppSettings.Get("AppServerHost"));
                    parms.Add(RfcConfigParameters.SystemNumber, ConfigurationManager.AppSettings.Get("SystemNumber"));
                    parms.Add(RfcConfigParameters.Client, ConfigurationManager.AppSettings.Get("Client"));
                    parms.Add(RfcConfigParameters.SystemID, ConfigurationManager.AppSettings.Get("SystemID"));

                    /* SAP username & password Auth */
                    parms.Add(RfcConfigParameters.User, ConfigurationManager.AppSettings.Get("User"));
                    parms.Add(RfcConfigParameters.Password, ConfigurationManager.AppSettings.Get("Password"));

                    /* SAP SNC Auth */
                    parms.Add(RfcConfigParameters.SncQOP, ConfigurationManager.AppSettings.Get("SncQOP"));
                    parms.Add(RfcConfigParameters.SncMode, ConfigurationManager.AppSettings.Get("SncMode"));
                    parms.Add(RfcConfigParameters.SncPartnerName, ConfigurationManager.AppSettings.Get("SncPartnerName")); /* SAP Subject name on the server itself. */
                    parms.Add(RfcConfigParameters.SncMyName, ConfigurationManager.AppSettings.Get("SncMyName")); /* SAP certificate subject name of the end user. */
                    parms.Add(RfcConfigParameters.SncLibraryPath, ConfigurationManager.AppSettings.Get("SncLibraryPath"));

                    //parms.Add(RfcConfigParameters.Trace, "2");

                    return parms;
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
