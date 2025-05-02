using System.Configuration;
using System.Collections.Specialized;

namespace InsightEngine.Program

{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            SystemInfo si = new SystemInfo();
            si.getProcessorInfo();
            Form1 form = new Form1();
            form.Text = ConfigurationManager.AppSettings["AppName"];
            Application.Run(form);
        }
    }
}