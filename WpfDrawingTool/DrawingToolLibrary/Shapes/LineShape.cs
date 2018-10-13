using DrawingToolLibrary.Interfaces;
using DrawingToolLibrary.Utils;

namespace DrawingToolLibrary.Shapes
{
   internal class LineShape : IShape
   {
      public enum LineOrientation : byte
      {
         Vertical = 1,
         Horizontal = 2,
         Unknown = 3
      }

      #region Attributes - Properties
      public Canvas Canvas { get; set; }

      public Point P1 { get; set; }
      public Point P2 { get; set; }
      public LineOrientation Orientation { get; }
      
      #endregion

      #region Constructors
      public LineShape(Point p1, Point p2, Canvas canvas)
      {
         P1 = p1;
         P2 = p2;
         Canvas = canvas;

         if (P1.X == P2.X) Orientation = LineOrientation.Vertical;
         else if (P1.Y == P2.Y) Orientation = LineOrientation.Horizontal;
         else Orientation = LineOrientation.Unknown;
      }
      #endregion

      #region Methods
      public void Draw(string character)
      {
         switch (Orientation)
         {
            case LineOrientation.Horizontal:
               if (P1.Y >= Constants.CANVAS_OFFSET && P1.Y <= Canvas.Height)
               {
                  for (int j = Constants.CANVAS_OFFSET; j <= Canvas.Width; j++)
                  {
                     if (j >= P1.X && j <= P2.X)
                     {
                        Canvas.Data[P1.Y - Constants.CANVAS_OFFSET, j - Constants.CANVAS_OFFSET] = character;
                     }
                  }
               }
               break;

            case LineOrientation.Vertical:
               if (P1.X >= Constants.CANVAS_OFFSET && P1.X <= Canvas.Width)
               {
                  for (int i = Constants.CANVAS_OFFSET; i <= Canvas.Height; i++)
                  {
                     if (i >= P1.Y && i <= P2.Y)
                     {
                        Canvas.Data[i - Constants.CANVAS_OFFSET, P1.X - Constants.CANVAS_OFFSET] = character;
                     }
                  }
               }
               break;

            case LineOrientation.Unknown:
               break;
         }
      } 
      #endregion
   }
}