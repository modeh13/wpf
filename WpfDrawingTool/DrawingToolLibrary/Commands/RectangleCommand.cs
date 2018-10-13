using DrawingToolLibrary.Abstractions;
using DrawingToolLibrary.Interfaces;
using DrawingToolLibrary.Shapes;
using DrawingToolLibrary.Utils;
using System;
using System.Text.RegularExpressions;

namespace DrawingToolLibrary.Commands
{
   internal class RectangleCommand : AbstractCommand
   {
      #region Attributes - Properties      
      //Only positive numbers: ^R\s+(\d{1,3})\s+(\d{1,3})\s+(\d{1,3})\s+(\d{1,3})$
      //Negative numbers included: ^R\s+(-?\d+)\s+(-?\d+)\s+(-?\d+)\s+(-?\d+)$
      public static new string Regex = @"^R\s+(-?\d+)\s+(-?\d+)\s+(-?\d+)\s+(-?\d+)$";

      public Point P1 { get; }
      public Point P2 { get; }     

      private IShape LeftLine;
      private IShape RightLine;
      private IShape TopLine;
      private IShape BottomLine;
      #endregion

      #region Constructors
      public RectangleCommand(string commandLine, int x1, int y1, int x2, int y2, Canvas canvas)
      {
         Name = "Create Rectangle";
         Line = commandLine;
         Letter = "R";
         Character = 'x';
         Canvas = canvas;

         P1 = new Point(x1, y1);
         P2 = new Point(x2, y2);         
         LeftLine = new LineShape(new Point(x1, y1), new Point(x1, y2), canvas);
         RightLine = new LineShape(new Point(x2, y1), new Point(x2, y2), canvas);
         TopLine = new LineShape(new Point(x1, y1), new Point(x2, y1), canvas);
         BottomLine = new LineShape(new Point(x1, y2), new Point(x2, y2), canvas);
      }
      #endregion

      #region Methods
      public static RectangleCommand CreateInstance(string commandLine, Canvas canvas)
      {
         RectangleCommand canvasCommand = null;
         Regex regex = new Regex(Regex);
         Match match = regex.Match(commandLine);

         if (match.Success)
         {
            int x1 = Convert.ToInt32(match.Groups[1].Value);
            int y1 = Convert.ToInt32(match.Groups[2].Value);
            int x2 = Convert.ToInt32(match.Groups[3].Value);
            int y2 = Convert.ToInt32(match.Groups[4].Value);

            //Validate if Coordinate X1 is more bigger that coordinate X2 (Valid rectangle)
            if (x1 < x2 && y1 < y2)
            {
               canvasCommand = new RectangleCommand(commandLine, x1, y1, x2, y2, canvas);
            }
         }

         return canvasCommand;
      }

      public override void Execute()
      {
         if (Canvas?.IsCreated != null)
         {
            LeftLine?.Draw(Character.ToString());
            RightLine?.Draw(Character.ToString());
            TopLine?.Draw(Character.ToString());
            BottomLine?.Draw(Character.ToString());
            Canvas.PrintOutPut();
         }
      } 
      #endregion
   }
}