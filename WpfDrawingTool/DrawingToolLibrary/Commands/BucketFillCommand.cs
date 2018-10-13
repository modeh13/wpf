using DrawingToolLibrary.Abstractions;
using DrawingToolLibrary.Utils;
using System;
using System.Text.RegularExpressions;

namespace DrawingToolLibrary.Commands
{
   internal class BucketFillCommand : AbstractCommand
   {
      #region Attributes - Properties
      //Only positive numbers: ^B\s+(\d{1,3})\s+(\d{1,3})\s+(.)$
      //Negative numbers included: ^B\s+(-?\d+)\s+(-?\d+)\s+(.)$
      public static new string Regex = @"^B\s+(-?\d+)\s+(-?\d+)\s+(.)$";

      public Point Point { get; }      
      #endregion

      #region Constructors
      public BucketFillCommand(string commandLine, int x, int y, char colour, Canvas canvas)
      {
         Name = "Bucket Fill";
         Line = commandLine;
         Letter = "B";
         Character = colour;
         Canvas = canvas;

         Point = new Point(x, y);
      }
      #endregion

      #region Methods
      public static BucketFillCommand CreateInstance(string commandLine, Canvas canvas)
      {
         BucketFillCommand canvasCommand = null;
         Regex regex = new Regex(Regex);
         Match match = regex.Match(commandLine);

         if (match.Success)
         {
            int x = Convert.ToInt32(match.Groups[1].Value);
            int y = Convert.ToInt32(match.Groups[2].Value);
            string character = match.Groups[3].Value;

            //Validate that the point is within the range
            if (x >= 0 && y >= 0)
            {
               canvasCommand = new BucketFillCommand(commandLine, x, y, char.Parse(character), canvas);
            }
         }

         return canvasCommand;
      }

      private void Fill(Point point, string character)
      {
         if (Canvas.ExistPoint(point))
         {
            if (Canvas.Data[point.Y - Constants.CANVAS_OFFSET, point.X - Constants.CANVAS_OFFSET] == character)
            {
               Canvas.Data[point.Y - Constants.CANVAS_OFFSET, point.X - Constants.CANVAS_OFFSET] = Character.ToString();

               Point pointLeft = new Point(point.X - 1, point.Y);
               Point pointRight = new Point(point.X + 1, point.Y);
               Point pointTop = new Point(point.X, point.Y - 1);
               Point pointBottom = new Point(point.X, point.Y + 1);

               Fill(pointLeft, character);
               Fill(pointRight, character);
               Fill(pointTop, character);
               Fill(pointBottom, character);
            }            
         }
      }

      public override void Execute()
      {
         if (Canvas.IsCreated)
         {
            if (Canvas.ExistPoint(Point))
            {               
               Fill(Point, Canvas.Data[Point.Y - Constants.CANVAS_OFFSET, Point.X - Constants.CANVAS_OFFSET]);
               Canvas.PrintOutPut();
            }  
         }
      } 
      #endregion
   }
}