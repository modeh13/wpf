using DrawingToolLibrary.Abstractions;
using DrawingToolLibrary.Utils;
using System;
using System.Text.RegularExpressions;

namespace DrawingToolLibrary.Commands
{
   internal class CanvasCommand : AbstractCommand
   {
      #region Attributes - Properties      
      public static new string Regex = @"^C\s+(\d+)\s+(\d+)$";

      public int Width { get; private set; }
      public int Height { get; private set; }
      #endregion

      #region Constructors
      public CanvasCommand(string commandLine, int width, int height, Canvas canvas)
      {
         Name = "Create Canvas";
         Line = commandLine;
         Letter = "C";         
         Canvas = canvas;
         Width = width;
         Height = height;         
      }
      #endregion

      #region Methods
      public static CanvasCommand CreateInstance(string commandLine, Canvas canvas)
      {
         CanvasCommand canvasCommand = null;
         Regex regex = new Regex(Regex);
         Match match = regex.Match(commandLine);

         if (match.Success)
         {
            int width = Convert.ToInt32(match.Groups[1].Value);
            int height = Convert.ToInt32(match.Groups[2].Value);

            if (width > 0 && height > 0) {
               canvasCommand = new CanvasCommand(commandLine, width, height, canvas);
            }            
         }

         return canvasCommand;
      }

      public override void Execute()
      {
         if (Canvas.Data == null)
         {
            Canvas.Data = new string[Height, Width];
            Canvas.PrintOutPut();
         }
      } 
      #endregion
   }
}