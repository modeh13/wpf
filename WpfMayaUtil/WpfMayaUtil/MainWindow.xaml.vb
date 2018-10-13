Imports System.IO
Imports System.Xml

Class MainWindow

   Private notificationsPath As String = "C:\mint\files\Sabre\Notifications"
   Private lvwItems As List(Of FileItem)

   Private Sub mitSabreNotification_Click(sender As Object, e As RoutedEventArgs)

      Dim directory As New DirectoryInfo(notificationsPath)

      If directory.Exists Then
         Dim fileList As FileInfo() = directory.GetFiles("*.xml")

         If fileList.Length > 0 Then
            lvwItems = New List(Of FileItem)

            Dim fileItem As FileItem

            For Each file As FileInfo In fileList
               fileItem = New FileItem
               With fileItem
                  .FileName = file.Name
                  .FileDate = file.CreationTime.ToString("yyyy-MM-dd HH:mm:tt")
                  .FileStatus = FileItem.FileStatusEnum.Pending
               End With

               lvwItems.Add(fileItem)
            Next

            lvwFiles.ItemsSource = lvwItems

            MessageBox.Show($"The folder exists !! {fileList.Length}")
         End If
      End If
   End Sub

   Private Sub btnStartProcess_Click(sender As Object, e As RoutedEventArgs)
      If Not IsNothing(lvwItems) And lvwItems.Count > 0 Then
         Dim fileContent As String
         Dim xmlDoc As XmlDocument
         Dim xmlNodes As XmlNodeList
         Dim xmlNode As XmlNode


         For Each fileItem In lvwItems
            fileContent = File.ReadAllText(Path.Combine(notificationsPath, fileItem.FileName))

            If Not String.IsNullOrEmpty(fileContent) Then
               Dim newFolderPath As String
               xmlDoc = New XmlDocument()
               xmlDoc.LoadXml(fileContent)
               xmlNodes = xmlDoc.GetElementsByTagName("Event")

               If xmlNodes.Count > 0 Then
                  xmlNode = xmlNodes.Item(0)

                  'Create a directory with the SABRE Notification name
                  newFolderPath = Path.Combine(notificationsPath, xmlNode.InnerText.Trim())

                  If Not Directory.Exists(newFolderPath) Then
                     Directory.CreateDirectory(newFolderPath)
                  End If

                  File.Move(Path.Combine(notificationsPath, fileItem.FileName), Path.Combine(newFolderPath, fileItem.FileName))
               End If
            End If

            fileItem.FileStatus = FileItem.FileStatusEnum.Processed
         Next
      End If
   End Sub

   Private Function GetLineFile(filePath As String, patternToSearch As String) As String
      Dim line As String = String.Empty

      If File.Exists(filePath) Then
         line = File.ReadAllLines(filePath).First(Function(x) x.Contains(patternToSearch))
      End If

      Return line
   End Function


   Private Sub mitGetPieceLog_Click(sender As Object, e As RoutedEventArgs)
      If Not String.IsNullOrEmpty(txtPatternToSearch.Text.Trim()) Then
         Const pathLog As String = "C:\Users\german.rairez\Desktop\Azul XML Files\20180823"
         Const logFile As String = "azul_test.log"
         Dim fullNameLog As String = Path.Combine(pathLog, logFile)
         Dim outPathLog As String = Path.Combine(pathLog, "OutPut")
         Dim patternToSearch As String = txtPatternToSearch.Text.Trim()

         If File.Exists(fullNameLog) Then
            Dim separatorChar As Char = "|"
            Dim firstLine As String = GetLineFile(fullNameLog, patternToSearch)
            Dim startPattern As String = IIf(firstLine.Length > 10, firstLine.Substring(0, 9), "")
            Dim lineSplit As String() = firstLine.Split(separatorChar)
            Dim processIdentifier As String = String.Empty

            If lineSplit.Length > 3 Then
               processIdentifier = $"{lineSplit(2)}{separatorChar}{lineSplit(3)}"
            End If


            Dim line As String = String.Empty
            Dim lines As List(Of String) = New List(Of String)()
            'lines = File.ReadAllLines(fullNameLog).Where(Function(x) x.Contains(processIdentifier)).ToList

            Using sr As StreamReader = New StreamReader(fullNameLog, Text.Encoding.UTF8)
               Dim previousLineAdded = False

               While Not sr.EndOfStream
                  line = sr.ReadLine()

                  If line.Contains(processIdentifier) Or (Not line.StartsWith(startPattern) And previousLineAdded) Then
                     lines.Add(line)
                     previousLineAdded = True
                  Else
                     previousLineAdded = False
                  End If
               End While
            End Using

            If Not Directory.Exists(outPathLog) Then
               Directory.CreateDirectory(outPathLog)
            End If

            Dim fullNameNewLog As String = Path.Combine(outPathLog, $"Log_{DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")}.log")

            If File.Exists(fullNameNewLog) Then
               File.Delete(fullNameNewLog)
            End If

            Using fs As New FileStream(fullNameNewLog, FileMode.Create)
               Using sw As New StreamWriter(fs, Text.Encoding.UTF8)
                  For Each lineFile As String In lines
                     sw.WriteLine(lineFile)
                  Next
               End Using
            End Using

            MessageBox.Show($"Process was finished !!")
         End If
      End If
   End Sub
End Class

Class FileItem

   Public Enum FileStatusEnum As Byte
      Pending = 1
      Processed = 2
   End Enum


   Private _fileName As String
   Public Property FileName() As String
      Get
         Return _fileName
      End Get
      Set(ByVal value As String)
         _fileName = value
      End Set
   End Property

   Private _filedate As String
   Public Property FileDate() As String
      Get
         Return _filedate
      End Get
      Set(ByVal value As String)
         _filedate = value
      End Set
   End Property

   Private _fileStatus As FileStatusEnum
   Public Property FileStatus() As FileStatusEnum
      Get
         Return _fileStatus
      End Get
      Set(ByVal value As FileStatusEnum)
         _fileStatus = value
      End Set
   End Property

End Class