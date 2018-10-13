using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using DrawingToolLibrary;

namespace WpfDrawingTool
{
   internal class Log {
      public string Date { get; set; }
      public string Description { get; set; }
      public string Status { get; set; }

      public Log(string date, string description, string status) {
         Date = date;
         Description = description;
         Status = status;
      }
   }

   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      #region Attributes - Properties
      public const string DATETIME_FULL_FORMAT = "yyyy-MM-dd hh:mm:ss tt";
      private string InputFilePath;
      private string OutputFolderPath;
      private bool UsedTestCase = false;
      private DrawingTool DrawingTool;
      ObservableCollection<Log> LogList;
      private BackgroundWorker BackgroundWorker;
      #endregion

      #region Constructors
      public MainWindow()
      {
         InitializeComponent();
         BackgroundWorker = new BackgroundWorker();
         BackgroundWorker.WorkerReportsProgress = true;         
         BackgroundWorker.DoWork += BackgroundWorker_DoWork;         
         BackgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;

         LogList = new ObservableCollection<Log>();
         AddLogMessage(new Log(DateTime.Now.ToString(DATETIME_FULL_FORMAT), $"{ Title } started.", "WARN"));
         lvwLog.ItemsSource = LogList;         
      }
      #endregion

      #region Methods
      private void AddLogMessage(Log log )
      {
         LogList.Insert(0, log);
      } 
      #endregion

      #region Events
      private void btnInputFile_Click(object sender, RoutedEventArgs e)
      {
         var ofd = new System.Windows.Forms.OpenFileDialog() { Filter = "Text files (*.txt)|*.txt" };
         System.Windows.Forms.DialogResult result = ofd.ShowDialog();

         if (result == System.Windows.Forms.DialogResult.OK) {
            InputFilePath = ofd.FileName;
            txtInputFile.Text = InputFilePath;
         }
      }

      private void btnOutputFolder_Click(object sender, RoutedEventArgs e)
      {
         using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
         {
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
               OutputFolderPath = dialog.SelectedPath;
               txtOutputFolder.Text = OutputFolderPath;
            }
         }
      }

      private void btnStart_Click(object sender, RoutedEventArgs e)
      {
         bool isValid = true;

         if (!UsedTestCase)
         {
            AddLogMessage(new Log(DateTime.Now.ToString(DATETIME_FULL_FORMAT), "Validating inputs...", "INFO"));

            if (string.IsNullOrEmpty(InputFilePath))
            {               
               AddLogMessage(new Log(DateTime.Now.ToString(DATETIME_FULL_FORMAT), "You must select an input file.", "WARN"));
               isValid = false;
            }
            if (string.IsNullOrEmpty(OutputFolderPath))
            {
               AddLogMessage(new Log(DateTime.Now.ToString(DATETIME_FULL_FORMAT), "You must select an output folder.", "WARN"));
               isValid = false;
            }
            if (!File.Exists(InputFilePath))
            {
               AddLogMessage(new Log(DateTime.Now.ToString(DATETIME_FULL_FORMAT), "Input file not found.", "ERROR"));
               isValid = false;
            }            
         }
         else {
            InputFilePath = @"C:\Users\german.rairez\Downloads\DrawingTool\input.txt";
            OutputFolderPath = @"C: \Users\german.rairez\Downloads\DrawingTool";
         }

         if (isValid && !BackgroundWorker.IsBusy) {
            stkControlsPanel.IsEnabled = false;
            BackgroundWorker.RunWorkerAsync();
         }         
      }

      private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         if (e.Error != null)
         {
            AddLogMessage(new Log(DateTime.Now.ToString(DATETIME_FULL_FORMAT), e.Error.Message, "ERROR"));
         }

         AddLogMessage(new Log(DateTime.Now.ToString(DATETIME_FULL_FORMAT), "Process Completed.", "INFO"));
         stkControlsPanel.IsEnabled = true;
      }

      private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
      {
         DrawingTool = new DrawingTool(InputFilePath, OutputFolderPath);
         DrawingTool.DrawingCommandExecuted += (senderDrawing, eDrawing) =>
         {            
            Dispatcher.Invoke(() => {
               AddLogMessage(new Log(DateTime.Now.ToString(DATETIME_FULL_FORMAT), $"Command {eDrawing.CommandName} - {eDrawing.CommandLine} was executed.", "INFO"));
            });
         };
         DrawingTool.Draw();
         //Thread.Sleep(5000);
      }
      #endregion
   }
}