Public Class PageOtherAbout

    Private Shadows IsLoaded As Boolean = False
    Private Sub PageOtherAbout_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        '重复加载部分
        PanBack.ScrollToHome()

        '非重复加载部分
        If IsLoaded Then Return
        IsLoaded = True

        ' 这里改成往你的新控件里写文本（已合规）
        CustomVersionText.Text = "当前版本：" & VersionDisplay & "（" & VersionCode & "） | PCL (Aris修改版)"

        If BuildType = BuildTypes.Debug Then
            BtnDonateDonate.Visibility = Visibility.Collapsed
            BtnDonateOutput.Visibility = Visibility.Visible
        End If

    End Sub

    ' 处理 Hyperlink 的点击事件
    Private Sub Hyperlink_RequestNavigate(sender As Object, e As System.Windows.Navigation.RequestNavigateEventArgs)
        OpenWebsite(e.Uri.AbsoluteUri)
        e.Handled = True
    End Sub

    Public Shared Sub CopyIdentify() Handles BtnDonateCopy.Click
        ClipboardSet(Identify)
    End Sub
    Private Sub BtnDonateCodeInput_Click() Handles BtnDonateInput.Click
        InputPotatoCode(False)
    End Sub
    Private Sub BtnDonateOutput_Click(sender As Object, e As EventArgs) Handles BtnDonateOutput.Click
        GeneratePotatoCode()
    End Sub
    Private Sub BtnAboutUpdate_Click(sender As Object, e As MouseButtonEventArgs) Handles BtnAboutUpdate.Click
        UpdateCheckByButton()
    End Sub

End Class