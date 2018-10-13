using System;

namespace DrawingToolLibrary.Utils
{
   public class DrawingCommandArgs : EventArgs
   {
      public string CommandName { get; set; }
      public string CommandLine { get; set; }

      public DrawingCommandArgs(string commandName, string commandLine)
      {
         CommandName = commandName;
         CommandLine = commandLine;
      }
   }
}