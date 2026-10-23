'由于包含加解密等安全信息，本文件中的部分代码已被删除

Friend Module ModSecret

    '标注 PCL 的不同分支，仅用于替换标记
    Public Const VersionBranchMain As String = "OpenSource"
    '在开源版的注册表与常规版的注册表隔离，以防数据冲突
    Public Const RegFolder As String = "PCLDebug"
    '用于微软登录的 ClientId
    Public OAuthClientId As String = If(Environment.GetEnvironmentVariable("PCL_MS_CLIENT_ID"), "")
    'CurseForge API Key
    Public CurseForgeAPIKey As String = If(Environment.GetEnvironmentVariable("PCL_CURSEFORGE_API_KEY"), "")
    '用于匿名数据收集的腾讯云日志服务上报 URL
    Public Const ClsBaseUrl As String = ""

#Region "网络鉴权"

    Friend Function SecretCdnSign(UrlWithMark As String) As String
        If Not UrlWithMark.EndsWithF("{CDN}") Then Return UrlWithMark
        Return UrlWithMark.Replace("{CDN}", "").Replace(" ", "%20")
    End Function

    Friend Sub SecretHeadersSign(Url As String, ByRef Req As HttpRequestMessage, Optional SimulateBrowserHeaders As Boolean = False)
        If Not Req.Headers.UserAgent.Any Then
            If Url.Contains("baidupcs.com") OrElse Url.Contains("baidu.com") Then
                Req.Headers.Add("User-Agent", "LogStatistic")
            ElseIf SimulateBrowserHeaders Then
                Req.Headers.Add("User-Agent", $"PCL2/{VersionBaseName}.{CInt(BuildType)} Mozilla/5.0 AppleWebKit/537.36 Chrome/63.0.3239.132 Safari/537.36")
            Else
                Req.Headers.Add("User-Agent", $"PCL2/{VersionBaseName}.{CInt(BuildType)}")
            End If
        End If
        If Not SimulateBrowserHeaders Then Req.Headers.Add("Referer", $"http://{VersionCode}.open.pcl2.server/")
        If Url.Contains("api.curseforge.com") OrElse Url.Contains("forgecdn.net") Then Req.Headers.Add("x-api-key", CurseForgeAPIKey)
    End Sub

#End Region

#Region "主题"

    Public Color1 As New MyColor(52, 61, 74)
    Public Color2 As New MyColor(11, 91, 203)
    Public Color3 As New MyColor(19, 112, 243)
    Public Color4 As New MyColor(72, 144, 245)
    Public Color5 As New MyColor(150, 192, 249)
    Public Color6 As New MyColor(213, 230, 253)
    Public Color7 As New MyColor(222, 236, 253)
    Public Color8 As New MyColor(234, 242, 254)
    Public ColorBg0 As New MyColor(150, 192, 249)
    Public ColorBg1 As New MyColor(190, Color7)
    Public ColorGray1 As New MyColor(64, 64, 64)
    Public ColorGray2 As New MyColor(115, 115, 115)
    Public ColorGray3 As New MyColor(140, 140, 140)
    Public ColorGray4 As New MyColor(166, 166, 166)
    Public ColorGray5 As New MyColor(204, 204, 204)
    Public ColorGray6 As New MyColor(235, 235, 235)
    Public ColorGray7 As New MyColor(240, 240, 240)
    Public ColorGray8 As New MyColor(245, 245, 245)
    Public ColorSemiTransparent As New MyColor(1, Color8)

    Public ThemeNow As Integer = -1
    Public ColorHue As Integer = 210, ColorSat As Integer = 85, ColorLightAdjust As Integer = 0, ColorHueTopbarDelta As OneOf(Of Integer, Integer()) = 0
    Public ThemeDontClick As Integer = 0

    Public Sub ThemeRefresh(Optional NewTheme As Integer = -1)
        Try
            If ThemeNow = NewTheme AndAlso NewTheme >= 0 Then Return
            If NewTheme >= 0 Then ThemeNow = NewTheme

            Color1 = New MyColor().FromHSL2(ColorHue, ColorSat * 0.2, 25 + ColorLightAdjust * 0.3)
            Color2 = New MyColor().FromHSL2(ColorHue, ColorSat, 45 + ColorLightAdjust)
            Color3 = New MyColor().FromHSL2(ColorHue, ColorSat, 55 + ColorLightAdjust)
            Color4 = New MyColor().FromHSL2(ColorHue, ColorSat, 65 + ColorLightAdjust)
            Color5 = New MyColor().FromHSL2(ColorHue, ColorSat, 80 + ColorLightAdjust * 0.4)
            Color6 = New MyColor().FromHSL2(ColorHue, ColorSat, 91 + ColorLightAdjust * 0.1)
            Color7 = New MyColor().FromHSL2(ColorHue, ColorSat, 95)
            Color8 = New MyColor().FromHSL2(ColorHue, ColorSat, 97)
            ColorBg0 = Color4 * 0.4 + Color5 * 0.4 + ColorGray4 * 0.2
            ColorBg1 = New MyColor(190, Color7)

            ColorSemiTransparent = New MyColor(1, Color8)
            Application.Current.Resources("ColorBrush1") = New SolidColorBrush(Color1)
            Application.Current.Resources("ColorBrush2") = New SolidColorBrush(Color2)
            Application.Current.Resources("ColorBrush3") = New SolidColorBrush(Color3)
            Application.Current.Resources("ColorBrush4") = New SolidColorBrush(Color4)
            Application.Current.Resources("ColorBrush5") = New SolidColorBrush(Color5)
            Application.Current.Resources("ColorBrush6") = New SolidColorBrush(Color6)
            Application.Current.Resources("ColorBrush7") = New SolidColorBrush(Color7)
            Application.Current.Resources("ColorBrush8") = New SolidColorBrush(Color8)
            Application.Current.Resources("ColorBrushBg0") = New SolidColorBrush(ColorBg0)
            Application.Current.Resources("ColorBrushBg1") = New SolidColorBrush(ColorBg1)
            Application.Current.Resources("ColorObject1") = CType(Color1, Color)
            Application.Current.Resources("ColorObject2") = CType(Color2, Color)
            Application.Current.Resources("ColorObject3") = CType(Color3, Color)
            Application.Current.Resources("ColorObject4") = CType(Color4, Color)
            Application.Current.Resources("ColorObject5") = CType(Color5, Color)
            Application.Current.Resources("ColorObject6") = CType(Color6, Color)
            Application.Current.Resources("ColorObject7") = CType(Color7, Color)
            Application.Current.Resources("ColorObject8") = CType(Color8, Color)
            Application.Current.Resources("ColorObjectBg0") = CType(ColorBg0, Color)
            Application.Current.Resources("ColorObjectBg1") = CType(ColorBg1, Color)
            ThemeRefreshMain()
        Catch ex As Exception
            Logger.Error(ex, "刷新主题颜色失败", LogBehavior.Toast)
        End Try
    End Sub

    Public Sub ThemeRefreshMain()
        RunInUi(
        Sub()
            If Not FrmMain.IsLoaded Then Return
            Dim Brush = New LinearGradientBrush With {.EndPoint = New Point(1, 0), .StartPoint = New Point(0, 0)}
            Dim Deltas = ColorHueTopbarDelta.Switch(Function(d) New Integer() {-d, 0, d}, Function(d) d)
            Brush.GradientStops.Add(New GradientStop With {.Offset = 0, .Color = New MyColor().FromHSL2(ColorHue + Deltas(0), ColorSat, 48 + ColorLightAdjust)})
            Brush.GradientStops.Add(New GradientStop With {.Offset = 0.5, .Color = New MyColor().FromHSL2(ColorHue + Deltas(1), ColorSat, 54 + ColorLightAdjust)})
            Brush.GradientStops.Add(New GradientStop With {.Offset = 1, .Color = New MyColor().FromHSL2(ColorHue + Deltas(2), ColorSat, 48 + ColorLightAdjust)})
            FrmMain.PanTitle.Background = Brush
            FrmMain.PanTitle.Background.Freeze()
            If Settings.Get(Of Boolean)("UiBackgroundColorful") Then
                Brush = New LinearGradientBrush With {.EndPoint = New Point(0.1, 1), .StartPoint = New Point(0.9, 0)}
                Brush.GradientStops.Add(New GradientStop With {.Offset = -0.1, .Color = New MyColor().FromHSL2(ColorHue - 15, ColorSat * 0.8, 91)})
                Brush.GradientStops.Add(New GradientStop With {.Offset = 0.4, .Color = New MyColor().FromHSL2(ColorHue, ColorSat * 0.8, 91)})
                Brush.GradientStops.Add(New GradientStop With {.Offset = 1.1, .Color = New MyColor().FromHSL2(ColorHue + 15, ColorSat * 0.8, 91)})
                FrmMain.PanForm.Background = Brush
            Else
                FrmMain.PanForm.Background = New MyColor(245, 245, 245)
            End If
            FrmMain.PanForm.Background.Freeze()
        End Sub)
    End Sub

    Friend Sub ThemeCheckAll(EffectSetup As Boolean)
    End Sub
    Friend Function ThemeCheckOne(Id As Integer) As Boolean
        Return True
    End Function
    Friend Function ThemeUnlock(Id As Integer, Optional ShowDoubleHint As Boolean = True, Optional UnlockHint As String = Nothing) As Boolean
        Return False
    End Function

#End Region

#Region "更新"

    Friend IsUpdateWaitingRestart As Boolean = False
    Public Sub UpdateRestart(TriggerRestartAndByEnd As Boolean)
    End Sub
    Friend Sub DownloadLatestPCL(Optional LoaderToSyncProgress As LoaderBase = Nothing)
    End Sub

    Friend Sub UpdateCheckByButton()
        Dim FetchTask As New LoaderTask(Of String, JObject)("获取最新版本信息",
        Sub(Task As LoaderTask(Of String, JObject))
            Dim JsonStr = NetRequestByClient("https://api.github.com/repos/4444ji/PCL_Aris/releases/latest", 
                                              SimulateBrowserHeaders:=True, Timeout:=10000)
            Task.Output = JsonStr.DeserializeJson()
        End Sub)

        Dim Loader As New LoaderCombo(Of String)("检查零食版更新", New List(Of LoaderBase) From {FetchTask})
        
        Loader.OnStateChanged =
        Sub(CurrentLoader)
            If CurrentLoader.State = LoadState.Finished Then
                Dim Json = CType(FetchTask.Output, JObject)
                If Json Is Nothing Then Return
                
                Dim LatestVersion As String = Json("tag_name")?.ToString()
                Dim Body As String = Json("body")?.ToString()
                Dim ExeUrl As String = Nothing
                
                Dim Assets = Json("assets")
                If Assets IsNot Nothing Then
                    For Each asset In Assets
                        If asset("name").ToString().EndsWith(".exe") Then
                            ExeUrl = asset("browser_download_url").ToString()
                            Exit For
                        End If
                    Next
                End If

                ' ================= 核心：双重校验（版本号 + 哈希） =================
                Dim TargetHash As String = Nothing
                If Body IsNot Nothing AndAlso Body.Contains("SHA256:") Then
                    TargetHash = Body.AfterFirst("SHA256:").Split({vbCrLf, vbLf, " "}, StringSplitOptions.RemoveEmptyEntries)(0).Trim()
                End If

                Dim IsLatestVersion As Boolean = False

                ' 1. 版本号校验
                If LatestVersion = "v2.14-alpha" Then ' 这里请根据你最新的 Tag 修改
                    ' 2. 如果存在目标哈希，则进行哈希校验
                    If Not String.IsNullOrEmpty(TargetHash) Then
                        Dim CurrentHash As String = CryptographyUtils.ComputeFileHash(PathExe, CryptographyUtils.HashMethod.Sha256)
                        If CurrentHash.Equals(TargetHash, StringComparison.OrdinalIgnoreCase) Then
                            IsLatestVersion = True
                        End If
                    Else
                        ' 如果发布者忘记写哈希，只能退化为仅校验版本号
                        IsLatestVersion = True
                    End If
                End If

                If IsLatestVersion Then
                    RunInUi(Sub() Hint("当前已是最新版本（版本号与哈希均一致）！", HintType.Green))
                    Return
                End If
                ' =================================================================

                Dim Msg As String = $"发现新版本：{LatestVersion}{vbCrLf}{vbCrLf}更新内容：{vbCrLf}{Body}{vbCrLf}{vbCrLf}是否立即更新并重启？"
                
                If MyMsgBox(Msg, "发现新版本", "立即更新", "暂不更新") = 1 Then
                    If Not String.IsNullOrEmpty(ExeUrl) Then
                        Dim TempExe = PathTemp & "Update.exe"
                        Hint("正在下载更新，请稍候……", HintType.Blue)
                        
                        RunInNewThread(
                        Sub()
                            Try
                                NetDownloadByLoader(ExeUrl, TempExe)
                                RunInUi(Sub() UpdateReplace(Process.GetCurrentProcess().Id, PathExe, TempExe, True))
                            Catch ex As Exception
                                RunInUi(Sub() Hint("下载更新失败：" & ex.Message, HintType.Red))
                            End Try
                        End Sub, "UpdateDownload")
                    Else
                        Hint("未找到更新文件！请检查 Release 是否上传了 .exe 附件。", HintType.Red)
                    End If
                End If
            ElseIf CurrentLoader.State = LoadState.Failed Then
                RunInUi(Sub() Hint("检查更新失败：" & CurrentLoader.Error.Message, HintType.Red))
            End If
        End Sub

        Loader.Start()
        LoaderTaskbarAdd(Loader)
        FrmMain.BtnExtraDownload.ShowRefresh()
        FrmMain.BtnExtraDownload.Ribble()
    End Sub

    Public Sub UpdateReplace(ProcessId As Integer, OldFileName As String, NewFileName As String, TriggerRestart As Boolean)
        Try
            Dim OldBackup = OldFileName & ".old"
            Try
                If FileUtils.Exists(OldBackup) Then FileUtils.Delete(OldBackup)
            Catch
            End Try
            
            FileUtils.Move(OldFileName, OldBackup)
            FileUtils.Move(NewFileName, OldFileName)
            If TriggerRestart Then StartProcess(OldFileName, "")
            FormMain.EndProgramForce(ProcessReturnValues.TaskDone)
        Catch ex As Exception
            Logger.Error(ex, "PCL 热更新替换失败")
            MyMsgBox("由于文件被占用或权限不足，自动替换失败。请手动将新版本覆盖到当前目录。", "更新失败", IsWarn:=True)
        End Try
    End Sub

#End Region

#Region "联网配置"

    Public ServerConfig As JObject

    Public ServerLoader As New LoaderTask(Of Integer, Integer)("PCL 配置更新", 
    Sub()
        Try
            Dim JsonStr = NetRequestByClient("https://api.github.com/repos/4444ji/PCL_Aris/releases/latest", 
                                              SimulateBrowserHeaders:=True, Timeout:=8000)
            Dim Json = JsonStr.DeserializeJson()
            Dim LatestVersion = Json("tag_name")?.ToString()
            Dim Body = Json("body")?.ToString()
            
            ' ================= 双重校验（版本号 + 哈希） =================
            Dim TargetHash As String = Nothing
            If Body IsNot Nothing AndAlso Body.Contains("SHA256:") Then
                TargetHash = Body.AfterFirst("SHA256:").Split({vbCrLf, vbLf, " "}, StringSplitOptions.RemoveEmptyEntries)(0).Trim()
            End If

            Dim IsLatestVersion As Boolean = False
            If LatestVersion = "v2.14-alpha" Then
                If Not String.IsNullOrEmpty(TargetHash) Then
                    Dim CurrentHash As String = CryptographyUtils.ComputeFileHash(PathExe, CryptographyUtils.HashMethod.Sha256)
                    If CurrentHash.Equals(TargetHash, StringComparison.OrdinalIgnoreCase) Then
                        IsLatestVersion = True
                    End If
                Else
                    IsLatestVersion = True
                End If
            End If

            If Not IsLatestVersion Then
                RunInUi(Sub() Hint("发现零食版新版本：" & LatestVersion & "，请前往关于或百宝箱检查更新！", HintType.Green))
                Logger.Info($"后台检测到新版本：{LatestVersion}")
            End If
        Catch
            Logger.Info("后台更新检测失败（静默）")
        End Try
    End Sub, Priority:=ThreadPriority.BelowNormal) With {
        .ReloadTimeout = 1000 * 60 * 60
    }

#End Region

#Region "赞助等级"
    ' 务必保持官方原样，不要修改

    Public ReadOnly Property CurrentRank As DonationRank
        Get
            Return DonationRank.None
        End Get
    End Property

    Public Sub InputPotatoCode(IsUpdating As Boolean)
    End Sub
    Friend Sub GeneratePotatoCode()
    End Sub

    Friend Function GetIdentify() As String
        Return "0000-0000-0000-0000"
    End Function

#End Region

End Module