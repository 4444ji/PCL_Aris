Public Class PageOtherTest

    ' ========== 原有空壳方法（保留，避免调用方报错） ==========

    Public Shared Sub RubbishClear()
        Hint("为便于维护，开源内容中不包含百宝箱功能……")
    End Sub

    Public Shared Sub MemoryOptimize(ShowHint As Boolean)
        If ShowHint Then Hint("为便于维护，开源内容中不包含百宝箱功能……")
    End Sub

    Public Shared Sub MemoryOptimizeInternal(ShowHint As Boolean)
        If ShowHint Then Hint("为便于维护，开源内容中不包含百宝箱功能……")
    End Sub

    Public Shared Function GetRandomCave() As String
        Return "为便于维护，开源内容中不包含百宝箱功能……"
    End Function

    Public Shared Function GetRandomHint() As String
        Return "为便于维护，开源内容中不包含百宝箱功能……"
    End Function

    Public Shared Function GetRandomPresetHint() As String
        Return "为便于维护，开源内容中不包含百宝箱功能……"
    End Function

    ' ========== 自定义下载 ==========

    Public Shared Sub StartCustomDownload(Url As String, FileName As String, Optional Folder As String = Nothing)
        Try
            If String.IsNullOrWhiteSpace(Url) Then
                Hint("请输入下载地址", HintType.Red)
                Return
            End If
            
            ' 如果没填文件名，就自动从 URL 提取
            If String.IsNullOrWhiteSpace(FileName) Then
                FileName = PathUtils.GetLastPart(Url)
                If String.IsNullOrWhiteSpace(FileName) Then FileName = "下载文件_" & GetUuid()
            End If
            
            ' 如果没选保存目录，默认保存到桌面
            If String.IsNullOrWhiteSpace(Folder) Then
                Folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            End If
            
            DirectoryUtils.Create(Folder)
            Dim TargetPath As String = Path.Combine(Folder, FileName)

            ' 使用 PCL 的 LoaderDownload 组件
            Dim NewDownloadTask As New LoaderDownload("自定义下载 " & FileName,
                New List(Of NetFile) From {New NetFile({Url}, TargetPath)})
            
            ' 必须包装在 LoaderCombo 里才能加入任务栏
            Dim NewComboTask As New LoaderCombo(Of String)("自定义下载 " & FileName,
                New List(Of LoaderBase) From {NewDownloadTask})
            
            NewComboTask.Start()
            LoaderTaskbarAdd(NewComboTask)
            
            Hint("已开始下载：" & FileName, HintType.Green)
            FrmMain.BtnExtraDownload.ShowRefresh() ' 刷新右上角的下载小图标
            FrmMain.BtnExtraDownload.Ribble()      ' 让图标抖一下
        Catch ex As Exception
            Hint("下载失败：" & ex.Message, HintType.Red)
        End Try
    End Sub

    ' ========== 网站探活 ==========

    Public Shared Sub CheckWebsite(Url As String)
        If String.IsNullOrWhiteSpace(Url) Then
            Hint("请输入网址", HintType.Red)
            Return
        End If
        Url = Url.Trim()
        If Not Url.StartsWithF("http", True) Then Url = "https://" & Url

        RunInNewThread(
        Sub()
            Dim StartTime = GetTimeMs()
            Try
                Dim Result = NetRequestByClient(Url, Timeout:=10000, MakeLog:=False)
                Dim Cost = GetTimeMs() - StartTime
                RunInUi(
                Sub()
                    Hint($"网站存活！用时 {Cost} ms，返回 {If(Result Is Nothing, 0, Result.Length)} 字符", HintType.Green)
                End Sub)
            Catch ex As HttpRequestCodeException
                Dim Cost = GetTimeMs() - StartTime
                Dim Code = CInt(ex.StatusCode)
                RunInUi(
                Sub()
                    Hint($"网站有响应，但返回 HTTP {Code}（{Cost} ms）", HintType.Blue)
                End Sub)
            Catch ex As Exception
                Dim Cost = GetTimeMs() - StartTime
                RunInUi(
                Sub()
                    Hint($"网站无响应（{Cost} ms）：{ex.Message}", HintType.Red)
                End Sub)
            End Try
        End Sub, "CheckWebsite")
    End Sub

    ' ========== 今日人品 ==========

    Public Shared Sub Jrrp()
        Dim Seed As String = Date.Now.ToString("yyyyMMdd") & Identify
        Dim Hash As Long = 0
        For Each c As Char In Seed
            Hash = (Hash * 31 + AscW(c)) And &H7FFFFFFFL
        Next
        Dim Value As Integer = CInt(Hash Mod 101)

        ' 尝试在界面上显示结果
        Dim CurrentPage = TryCast(FrmMain.PageRight, PageOtherTest)
        If CurrentPage IsNot Nothing Then
            CurrentPage.LabJrrpResult.Text = "正在测算……"
        End If

        If Value = 100 Then
            If CurrentPage IsNot Nothing Then
                CurrentPage.LabJrrpResult.Text = "今日人品：100！传说中的欧皇就是你！（原版这里会解锁隐藏主题，咱没有）"
            Else
                Hint("今日人品：100！传说中的欧皇就是你！", HintType.Green)
            End If
            Return
        End If

        If Value = 0 Then
            If CurrentPage IsNot Nothing Then
                RunInNewThread(
                Sub()
                    RunInUi(Sub() CurrentPage.LabJrrpResult.Text = "今日人品：0…… 请阅读以下附加条款：" & vbCrLf & "1. 本结果由随机算法生成，不构成任何运势、投资、抽卡建议。")
                    Thread.Sleep(800)
                    RunInUi(Sub() CurrentPage.LabJrrpResult.Text &= vbCrLf & "2. 若因相信本结果导致任何损失，包括但不限于：抽卡全歪、挖矿掉岩浆、被苦力怕炸家……")
                    Thread.Sleep(800)
                    RunInUi(Sub() CurrentPage.LabJrrpResult.Text &= vbCrLf & "3. 本项目及编译者概不负责。最终解释权归龙腾猫跃所有。")
                End Sub, "JrrpDisclaimer")
            Else
                Hint("今日人品：0…… 今天建议别抽卡。", HintType.Red)
            End If
            Return
        End If

        Dim Texts As String()
        Select Case Value
            Case 1 To 20 : Texts = {"大凶，宜睡觉", "建议摸鱼", "今日不宜启动 Minecraft"}
            Case 21 To 50 : Texts = {"平平淡淡才是真", "普普通通的一天", "不好不坏，凑合过"}
            Case 51 To 80 : Texts = {"小吉，运气不错", "适合挖矿", "可能会遇到友善的苦力怕", "还行啦，还行啦"}
            Case 81 To 99 : Texts = {"大吉！宜抽卡", "欧气侧漏", "今天你说了算"}
            Case Else : Texts = {"平平淡淡才是真"}
        End Select
        
        Dim Pick = Texts(RandomInteger(0, Texts.Length - 1))
        
        If CurrentPage IsNot Nothing Then
            CurrentPage.LabJrrpResult.Text = $"今日人品：{Value} —— {Pick}"
        Else
            Hint($"今日人品：{Value} —— {Pick}", HintType.Blue)
        End If
    End Sub

    ' ========== 界面事件 ==========

    Private Sub BtnCheck_Click(sender As Object, e As EventArgs) Handles BtnCheck.Click
        CheckWebsite(TextUrl.Text)
    End Sub

    Private Sub BtnSelectFolder_Click(sender As Object, e As EventArgs) Handles BtnSelectFolder.Click
        ' 补上第二个参数 False（非多选）
        Dim Selected = Dialogs.SelectFolder("选择文件保存目录", False)
        If Selected IsNot Nothing AndAlso Selected.Any() Then
            TextDlFolder.Text = Selected.First()
        End If
    End Sub

    Private Sub BtnStartDownload_Click(sender As Object, e As EventArgs) Handles BtnStartDownload.Click
        StartCustomDownload(TextDlUrl.Text, TextDlName.Text, TextDlFolder.Text)
    End Sub

    Private Sub BtnJrrp_Click(sender As Object, e As EventArgs) Handles BtnJrrp.Click
        Jrrp()
    End Sub

    Private Sub BtnMemoryOptimize_Click(sender As Object, e As EventArgs) Handles BtnMemoryOptimize.Click
        Try
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
            Hint("内存优化完成", HintType.Green)
        Catch ex As Exception
            Hint("优化失败：" & ex.Message, HintType.Red)
        End Try
    End Sub

End Class