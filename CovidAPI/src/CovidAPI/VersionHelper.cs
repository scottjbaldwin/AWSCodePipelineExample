using System.Linq;
using System.Reflection;

namespace CovidAPI
{
    public static class VersionHelper
    {
        public static string InformationalVersion
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                var assemblyVersion = assembly.GetCustomAttributes<AssemblyInformationalVersionAttribute>().ToList()[0].InformationalVersion;

                return assemblyVersion;
            }
        }

        public static string AssemblyName
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Name;
            }
        }
    } 
}