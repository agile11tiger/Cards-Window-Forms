using System;
using System.Collections.Generic;
using System.Linq;

namespace DurakLibrary.HostServer
{
    public static class Utils
    {
        public static void FillTypeList<T>(AppDomain domain, List<T> result)
        {
            Type[] types = (
                from domainAssembly in domain.GetAssemblies()                   // Get the referenced assemblies
                from assemblyType in domainAssembly.GetTypes()                  // Get all types in assembly
                where typeof(T).IsAssignableFrom(assemblyType)                  // Check to see if the type is a game rule
                where assemblyType.GetConstructor(Type.EmptyTypes) != null      // Make sure there is an empty constructor
                select assemblyType).ToArray();                                 // Convert IEnumerable to array
            
            for (int index = 0; index < types.Length; index++)
                result.Add((T)Activator.CreateInstance(types[index]));
        }
        
        public static bool CanChangeType(object value, Type conversionType)
        {
            var convertible = value as IConvertible;

            if (conversionType == null)
                return false;
            else if (value == null)
                return false;
            else if (convertible == null)
                return false;
            else
                return true;
        }
    }
}
