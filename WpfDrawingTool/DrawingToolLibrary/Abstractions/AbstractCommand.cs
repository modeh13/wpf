using DrawingToolLibrary.Utils;

namespace DrawingToolLibrary.Abstractions
{
   internal abstract class AbstractCommand
   {      
      public string Name { get; protected set; }
      public string Line { get; protected set; }
      public string Letter { get; protected set; }
      public char Character { get; protected set; }
      public Canvas Canvas { get; set; }
      public static string Regex = string.Empty;

      public virtual void Execute() { }
   }
}