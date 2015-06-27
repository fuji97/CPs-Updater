Imports System
Imports System.Net
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class downlodableFile
    Public ReadOnly name As String
    Public ReadOnly downloadLink As String
    Public ReadOnly type As String
    Public ReadOnly version As String
    Public ReadOnly compatibleVersion As String = ""
    Public Property isDownloading As Boolean = False
    Public Property isDownloadSuccessfull As Boolean = False
    Public ReadOnly updatingDate As DateTime
    Public ReadOnly size As Integer
    Public ReadOnly downloadCount As Integer

    Public Event DownloadProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs)
    Public Event DownloadFileCompleted()

    Dim mainForm As MainForm
    Public WithEvents download As WebClient = New WebClient

    Sub New(ByVal form As MainForm, json As JObject, mVersion As String)
        mainForm = form
        version = mVersion
        name = json("name")
        size = json("size")
        downloadCount = json("download_count")
        downloadLink = json("browser_download_url")
        Console.WriteLine("Name: " & name & " Size: " & size & " Download count: " & downloadCount & vbCrLf & "Download link: " & downloadLink)
        Dim splitext As List(Of String) = New List(Of String)
        Dim swichSplit As List(Of String) = New List(Of String)
        updatingDate = json("updated_at")
        Console.WriteLine("Pubblication date (latest update): " & updatingDate)
        splitext.Clear()
        splitext.AddRange(Split(name, "-"))
        type = splitext(0)
        Dim versionCheck As String = ""
        versionCheck = Split(Replace(name, ".exe", ""), "-", 2)(1)
        Console.WriteLine("Check version string extracted by name: " & versionCheck & " (" & splitext(0) & ")")
        splitext.Clear()
        Select Case type
            Case "Incremental"
                splitext.AddRange(Split(versionCheck, "_"))
                compatibleVersion = splitext(0)
                If version <> splitext(1) Then
                    MsgBox("Errore nelle versioni dell'eseguibile e/o del tag" & vbCr & "Per favore, segnalate questo problema agli sviluppatori", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Errore nel versioning")
                End If
                Console.WriteLine("Compatible version: " & compatibleVersion & " Version: " & version)
            Case "Setup"
                If version <> versionCheck Then
                    MsgBox("Errore nelle versioni dell'eseguibile e/o del tag" & vbCr & "Per favore, segnalate questo problema agli sviluppatori", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Errore nel versioning")
                End If
                Console.WriteLine("Version: " & version)
        End Select
    End Sub

    Public Sub startDownload()
        Try
            Dim request As WebRequest = WebRequest.Create(downloadLink)
            Dim response As WebResponse = request.GetResponse()
            response.Dispose()
            download.DownloadFileAsync(New Uri(downloadLink), "install.exe")
            mainForm.changeDownloadStatus(MainForm._Downloading)
            isDownloadSuccessfull = True
        Catch ex As WebException
            isDownloadSuccessfull = False
            Throw New WebException(ex.Message, ex)
        End Try
    End Sub

    Public Sub stopDownload(currentPath As String)
        isDownloadSuccessfull = False
        download.CancelAsync()
        Threading.Thread.Sleep(400)
        My.Computer.FileSystem.DeleteFile(currentPath & "\install.exe")
    End Sub

    Public Function checkCompatibility(currentVersion As String)
        If type = "Incremental" Then
            If compatibleVersion = currentVersion Then
                Return True
            End If
        Else
            Return False
        End If
    End Function

    Sub download_DownloadProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs) Handles download.DownloadProgressChanged
        RaiseEvent DownloadProgressChanged(sender, e)
    End Sub

    Sub download_DownloadFileCompleted() Handles download.DownloadFileCompleted
        RaiseEvent DownloadFileCompleted()
    End Sub

End Class
