Public Class MainWindow3
   Private Sub btnOpenMenu_Click(sender As Object, e As RoutedEventArgs)
      btnCloseMenu.Visibility = Visibility.Visible
      btnOpenMenu.Visibility = Visibility.Collapsed
   End Sub

   Private Sub btnCloseMenu_Click(sender As Object, e As RoutedEventArgs)
      btnOpenMenu.Visibility = Visibility.Visible
      btnCloseMenu.Visibility = Visibility.Collapsed
   End Sub
End Class
