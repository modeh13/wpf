using System;
using System.IO;
using System.Text;

namespace DrawingToolLibrary.Utils
{
   internal class Canvas
   {
      #region Attributes - Properties
      public int Width { get; protected set; }
      public int Height { get; protected set; }
      private string[,] data;
      public string[,] Data {
         get { return data; }
         set
         {
            data = value;
            IsCreated = true;
            Height = data.GetLength(0);
            Width = data.GetLength(1);
         }
      }

      public bool IsCreated { get; private set; }

      public string OutputFile { get; set; }
      #endregion

      #region Constructors
      public Canvas(string outputFolderPath)
      {
         OutputFile = Path.Combine(outputFolderPath, $"{Constants.OUTPUT_FILENAME}_{DateTime.Now.ToString("yyyyMMdd_hhmm_ss")}.txt");
      } 
      #endregion

      #region Methods
      public void PrintOutPut()
      {
         if (IsCreated)
         {
            string directoryPath = Path.GetDirectoryName(OutputFile);

            //Validate if directory exists.
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            using (FileStream fs = new FileStream(OutputFile, FileMode.Append, FileAccess.Write))
            {
               using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
               {
                  StringBuilder newLine;
                  sw.WriteLine(new string('-', Width + 2));

                  for (int i = 0; i < Data.GetLength(0); i++)
                  {
                     newLine = new StringBuilder();
                     newLine.Append("|");
                     for (int j = 0; j < Data.GetLength(1); j++)
                     {
                        newLine.Append(string.IsNullOrEmpty(Data[i, j]) ? " " : Data[i, j]);
                     }
                     newLine.Append("|");
                     sw.WriteLine(newLine);
                  }

                  sw.WriteLine(new string('-', Width + 2));
               }
            }
         }         
      }

      public bool ExistPoint(Point point)
      {
         return (point.X >= Constants.CANVAS_OFFSET && point.X <= Width) && (point.Y >= Constants.CANVAS_OFFSET && point.Y <= Height);
      }
      #endregion
   }
}