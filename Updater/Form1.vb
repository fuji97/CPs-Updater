Imports System
Imports System.Net
Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Collections.Generic
Imports System.Math

' TODO: Icona, nome applicazione, compilazione prorietà, controllo eccezione no internet, abilitare "ignore update", sistemare "Form1.Load", Aggiungere controllo versione via file, controllare errore "dialogResult" (evidenziato verde nella dichiarazione)

Public Class Form1
    Const appName = "{0AGHJ15F-G95D-06F4-KM90-G9XZI5DON37D}_is1"
    Const url = "https://api.github.com/repos/Bl00d-Kirito/HSK-Cremisi_Portals_Demo/releases/latest"
    Const Unknown = -2
    Const Over = -1
    Const Equal = 0
    Const Under = 1
    Const Check = 2
    Const Update = 3
    Const Downloading = 4
    Dim currentPath As String = Directory.GetCurrentDirectory()
    Dim currentVer As String
    Dim latestVer As String
    Dim downloadLink As String
    Dim data As String
    Dim jsonObject As JObject
    Dim items As List(Of JToken)
    Dim jsonReader As StreamReader
    Dim haveNew As Integer
    Dim dialogResult As MsgBoxResult
    Dim WithEvents download As WebClient = New WebClient
    Dim downloadStatus As Integer
    Dim downloadSuccesfull As Boolean = False

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        My.Application.SaveMySettingsOnExit = True

        If My.Settings.ignoreUpdate Then
            Console.WriteLine("Ignoring update...")
            ckbIgnore.Checked = True
        End If
        changeDownloadStatus(Check)
        Dim checkResult As Integer = checkUpdate()

        If checkResult = Under Then
            dialogResult = MsgBox("Una nuova versione è disponibile (" & latestVer & ") vuoi scaricarla?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Nuova versione disponibile")
            If dialogResult = MsgBoxResult.Yes Then
                downloadUpdate()
            End If
        ElseIf checkResult = Equal
            Process.Start(currentPath & "\Game.exe")
            Application.Exit()
        End If
    End Sub

    Private Function checkUpdate()
        currentVer = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" & appName, "DisplayVersion", Nothing)
        If currentVer Is Nothing Then
            ' TODO
            currentVer = "---"
        End If
        Console.WriteLine("Current version: " & currentVer)
        lblCurrentVer.Text = currentVer

        latestVer = "---"
        Try
            Dim request As HttpWebRequest = HttpWebRequest.Create(url)
            request.Proxy = Nothing
            request.UserAgent = "Test"

            Dim response As HttpWebResponse = request.GetResponse
            Dim responseStream As System.IO.Stream = response.GetResponseStream

            Dim streamReader As New System.IO.StreamReader(responseStream)
            data = streamReader.ReadToEnd
            streamReader.Close()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            MsgBox("Impossibile controllare gli aggiornamenti" & vbCrLf & ex.Message, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Aggiornamento fallito")
        End Try

        If Not data Is Nothing Then
            Dim ser As JObject = JObject.Parse(data)
            Dim jData As List(Of JToken) = ser.Children().ToList

            For Each item As JProperty In jData
                item.CreateReader()
                If item.Name = "tag_name" Then
                    latestVer = item.Value.Value(Of String)
                    Console.WriteLine("Latest version: " & latestVer)
                    lblLatestVersion.Text = latestVer
                ElseIf item.Name = "assets" Then
                    For Each assets As JObject In item.Value
                        downloadLink = assets("browser_download_url")
                        Console.WriteLine("Download URL: " & downloadLink)
                    Next
                End If
            Next
        End If

        currentVer = "v1"

        haveNew = Unknown
        If Not currentVer Is Nothing And Not latestVer Is Nothing Then
            haveNew = checkVersion(currentVer, latestVer)
            If haveNew = 1 Then
                Console.WriteLine("New version avaiable: " & latestVer & "(> " & currentVer & ")")
                changeDownloadStatus(Update)
            ElseIf haveNew = -1 Then
                Console.WriteLine("Not-avaiable version: " & latestVer & "(< " & currentVer & ")")
            ElseIf haveNew = 0 Then
                Console.WriteLine("Updated version: " & latestVer & "(= " & currentVer & ")")
            End If
        End If
        Return haveNew
    End Function

    Private Sub downloadUpdate()
        Try
            lblInfo.Text = "Avvio download"
            download.DownloadFileAsync(New Uri(downloadLink), "install.exe")
            changeDownloadStatus(Downloading)
            downloadSuccesfull = True
        Catch ex As WebException
            Console.WriteLine(ex.Message)
            MsgBox("Errore nel download del file: " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
        Finally
        End Try
    End Sub

    Private Sub changeDownloadStatus(status As Integer)
        downloadStatus = status
        Select Case status
            Case Check
                btnUpdate.Text = "Controlla aggiornamenti"
            Case Update
                btnUpdate.Text = "Scarica aggiornamento"
            Case Downloading
                btnUpdate.Text = "Annulla aggiornamento"
        End Select
    End Sub

    Private Sub download_DownloadProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs) Handles download.DownloadProgressChanged
        If downloadStatus = Downloading Then
            downloadBar.Value = e.ProgressPercentage
            lblInfo.Text = e.ProgressPercentage & "% - Scaricato " & Round(CDbl(e.BytesReceived) / 1048576, 1) & " di " & Round(CDbl(e.TotalBytesToReceive) / 1048576, 1) & " MB"
        End If
    End Sub

    Private Sub download_DownloadFileCompleted() Handles download.DownloadFileCompleted
        If downloadSuccesfull Then
            Process.Start(currentPath & "\install.exe")
            Application.Exit()
        End If
    End Sub

    Private Function checkVersion(prev As String, last As String)
        Dim prevArray As List(Of String) = New List(Of String)
        Dim lastArray As List(Of String) = New List(Of String)
        Dim prevValue As Integer
        Dim lastValue As Integer
        Dim haveNew As Integer = 0
        Dim i As Integer = 0
        prevArray.AddRange(Split(prev, "."))
        lastArray.AddRange(Split(last, "."))
        prevArray(0) = prevArray(0).Trim("v")
        lastArray(0) = lastArray(0).Trim("v")

        Do While haveNew = 0
            Try
                prevValue = CInt(prevArray(i))
            Catch ex As Exception
                prevValue = 0
            End Try
            Try
                lastValue = CInt(lastArray(i))
            Catch ex As Exception
                lastValue = 0
            End Try
            If prevValue > lastValue Then
                haveNew = -1
            ElseIf prevValue < lastValue Then
                haveNew = 1
            End If
            If i > prevArray.Count - 1 And i > lastArray.Count - 1 Then
                Exit Do
            End If
            i = i + 1
        Loop
        Return haveNew
    End Function

    Private Sub checkbeforeClose()
        If downloadStatus = Downloading Then
            lblInfo.Text = "Annullamento download"
            downloadSuccesfull = False
            download.CancelAsync()
            download.Dispose()
            changeDownloadStatus(Update)
            Threading.Thread.Sleep(400)
            My.Computer.FileSystem.DeleteFile(currentPath & "\install.exe")
            downloadBar.Value = 0
            lblInfo.Text = "Pronto"
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Select Case downloadStatus
            Case Check
                checkUpdate()

                If downloadStatus = Update Then
                    dialogResult = MsgBox("Una nuova versione è disponibile (" & latestVer & ") vuoi scaricarla?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Nuova versione disponibile")
                    If dialogResult = MsgBoxResult.Yes Then
                        downloadUpdate()
                    End If
                End If
            Case Update
                btnUpdate.Text = "Scarica aggiornamento"
                downloadUpdate()
            Case Downloading
                lblInfo.Text = "Annullamento download"
                downloadSuccesfull = False
                download.CancelAsync()
                download.Dispose()
                changeDownloadStatus(Update)
                Threading.Thread.Sleep(400)
                My.Computer.FileSystem.DeleteFile(currentPath & "\install.exe")
                downloadBar.Value = 0
                lblInfo.Text = "Pronto"
        End Select
    End Sub

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        checkbeforeClose()
        Process.Start(currentPath & "\Game.exe")
        Application.Exit()
    End Sub

    Private Sub ckbIgnore_CheckedChanged(sender As Object, e As EventArgs) Handles ckbIgnore.CheckedChanged
        My.Settings.ignoreUpdate = ckbIgnore.Checked
    End Sub
End Class
