using DrawingToolLibrary.Abstractions;
using DrawingToolLibrary.Interfaces;
using DrawingToolLibrary.Shapes;
using DrawingToolLibrary.Utils;
using System;
using System.Text.RegularExpressions;

namespace DrawingToolLibrary.Commands
{
   internal class LineCommand : AbstractCommand
   {
      #region Attributes - Properties
      //Only positive numbers: ^L\s+(\d{1,3})\s+(\d{1,3})\s+(\d{1,3})\s+(\d{1,3})$
      //Negative numbers included: ^L\s+(-?\d+)\s+(-?\d+)\s+(-?\d+)\s+(-?\d+)$
      public static new string Regex = @"^L\s+(-?\d+)\s+(-?\d+)\s+(-?\d+)\s+(-?\d+)$";      

      private IShape LineShape;
      public Point P1 { get; }
      public Point P2 { get; }      
      #endregion

      #region Constructors
      public LineCommand(string commandLine, int x1, int y1, int x2, int y2, Canvas canvas)
      {
         Name = "Create Line";
         Line = commandLine;
         Letter = "L";
         Character = 'x';
         Canvas = canvas;
         P1 = new Point(x1, y1);
         P2 = new Point(x2, y2);         
         LineShape = new LineShape(P1, P2, canvas);
      }
      #endregion

      #region Methods
      public static LineCommand CreateInstance(string commandLine, Canvas canvas)
      {
         LineCommand canvasCommand = null;
         Regex regex = new Regex(Regex);
         Match match = regex.Match(commandLine);

         if (match.Success)
         {
            int tempX1 = Convert.ToInt32(match.Groups[1].Value);
            int tempY1 = Convert.ToInt32(match.Groups[2].Value);
            int tempX2 = Convert.ToInt32(match.Groups[3].Value);
            int tempY2 = Convert.ToInt32(match.Groups[4].Value);

            //Validate if it's a vertical or horizontal line.
            if (tempX1 == tempX2 || tempY1 == tempY2) {
               int x1 = 0, y1 = 0, x2 = 0, y2 = 0;

               //Vertical
               if (tempX1 == tempX2) {
                  //Top -> Bottom
                  if(tempY1 <= tempY2)
                  {
                     x1 = tempX1;
                     y1 = tempY1;
                     x2 = tempX2;
                     y2 = tempY2;
                  }
                  else {
                     x1 = tempX2;
                     y1 = tempY2;
                     x2 = tempX1;
                     y2 = tempY1;
                  }
               }

               //Horizontal
               if (tempY1 == tempY2)
               {
                  //Left -> Right
                  if (tempX1 <= tempX2)
                  {
                     x1 = tempX1;
                     y1 = tempY1;
                     x2 = tempX2;
                     y2 = tempY2;
                  }
                  else
                  {
                     x1 = tempX2;
                     y1 = tempY2;
                     x2 = tempX1;
                     y2 = tempY1;
                  }
               }

               canvasCommand = new LineCommand(commandLine, x1, y1, x2, y2, canvas);
            }
         }

         return canvasCommand;
      }
      public override void Execute()
      {
         if (Canvas.IsCreated)
         {
            LineShape?.Draw(Character.ToString());
            Canvas.PrintOutPut();
         }
      } 
      #endregion
   }
}