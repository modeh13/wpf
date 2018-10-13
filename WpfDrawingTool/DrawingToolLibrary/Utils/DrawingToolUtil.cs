using DrawingToolLibrary.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DrawingToolLibrary.Utils
{
   internal static class DrawingToolUtil
   {
      public static IEnumerable<Type> GetAssemblyTypesByNamespace<T>(string _namespace)
      {
         var type = typeof(T);
         return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass && p.Namespace == _namespace);
      }

      public static List<TypeRegex> GetCommandRegex()
      {
         List<TypeRegex> commandRegex = new List<TypeRegex>();
         IEnumerable<Type> typeList = GetAssemblyTypesByNamespace<AbstractCommand>(Constants.COMMAND_NAMESPACE);
         FieldInfo fieldInfo;
         string regex;

         foreach (Type type in typeList)
         {            
            fieldInfo = type.GetField(Constants.COMMAND_REGEX_FIELD, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            regex = fieldInfo?.GetValue(null).ToString();

            if (!string.IsNullOrEmpty(regex))
            {
               commandRegex.Add(new TypeRegex(type, regex));
            }
         }

         return commandRegex;
      }

      public static object GetNewObject(Type type, string commandLine)
      {
         try
         {
            return Activator.CreateInstance(type, new object[] { commandLine });
         }
         catch
         {
            return null;
         }
      }
   }
}