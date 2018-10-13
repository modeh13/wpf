using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using DrawingToolLibrary.Utils;
using System.Text.RegularExpressions;
using DrawingToolLibrary.Abstractions;

namespace DrawingToolLibrary
{
   public class DrawingTool
   {
      #region Attributes - Properties
      private Canvas Canvas;
      private Queue<AbstractCommand> CommandList;
      private string InputPath;
      public string OutPutPath { get; }
      public delegate void DelDrawingCommand(object sender, DrawingCommandArgs e);
      public event DelDrawingCommand DrawingCommandExecuted;
      #endregion

      #region Constructors
      public DrawingTool() { }

      public DrawingTool(string inputPath, string outputPath)
      {
         InputPath = inputPath;
         OutPutPath = outputPath;
         Canvas = new Canvas(OutPutPath);
      }
      #endregion

      #region Methods
      public void Draw()
      {
         if (File.Exists(InputPath))
         {
            string commandLine;
            TypeRegex typeRegex;
            MethodInfo methodInfo;
            AbstractCommand drawingCommand;            
            CommandList = new Queue<AbstractCommand>();
            List<TypeRegex> commandRegex = DrawingToolUtil.GetCommandRegex();
            
            //Reading input file
            using (FileStream fs = new FileStream(InputPath, FileMode.Open, FileAccess.Read))
            {
               using (StreamReader sr = new StreamReader(fs))
               {
                  while (!sr.EndOfStream)
                  {
                     drawingCommand = null;
                     commandLine = sr.ReadLine().Trim();
                     typeRegex = commandRegex.First(x => new Regex(x.Regex).Match(commandLine).Success);

                     //It's a valid command
                     if (typeRegex != null) {
                        methodInfo = typeRegex.Type.GetMethod(Constants.COMMAND_CREATE_INSTANCE, BindingFlags.Public | BindingFlags.Static);
                        drawingCommand = methodInfo?.Invoke(null, new object[] { commandLine, Canvas }) as AbstractCommand;
                        if (drawingCommand != null) CommandList.Enqueue(drawingCommand);
                     }
                  }
               }
            }            

            //Executes commands
            if (CommandList.Any())
            {
               while (CommandList.Count > 0)
               {
                  drawingCommand = CommandList.Dequeue();
                  drawingCommand.Execute();                  
                  DrawingCommandExecuted?.Invoke(this, new DrawingCommandArgs(drawingCommand.Name, drawingCommand.Line));                  
               }
            }
            else {
               throw new ArgumentException("Valid commands not found.");
            }
         }
         else
         {
            throw new FileNotFoundException("Input file not found.", InputPath);
         }
      }
      #endregion
   }
}