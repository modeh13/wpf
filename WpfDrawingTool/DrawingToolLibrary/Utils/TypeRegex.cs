using System;

namespace DrawingToolLibrary.Utils
{
   internal class TypeRegex
   {
      public Type Type { get; set; }
      public string Regex { get; set; }

      public TypeRegex(Type type, string regex)
      {
         Type = type;
         Regex = regex;
      }
   }
}