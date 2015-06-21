Imports System
Imports System.Net
Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Collections.Generic
Imports System.Math

' TODO: abilitare sistemare "Form1.Load"

Public Class MainForm
    Const APP_NAME = "{0AGHJ15F-G95D-06F4-KM90-G9XZI5DON37D}_is1"
    Const URL = "https://api.github.com/repos/Bl00d-Kirito/HSK-Cremisi_Portals_Demo/releases/latest"
    Const VERSION_FILE_NAME = "VERSION"
    Const _Unknown = -2
    Const _Over = -1
    Const _Equal = 0
    Const _Under = 1
    Const _Check = 2
    Const _Update = 3
    Const _Downloading = 4

    Dim currentPath As String = Directory.GetCurrentDirectory()
    Dim currentVer As String
    Dim latestVer As String
    Dim downloadLink As String
    Dim data As String
    Dim jsonObject As JObject
    Dim items As List(Of JToken)
    Dim jsonReader As StreamReader
    Dim haveNew As Integer
    Dim WithEvents download As WebClient = New WebClient
    Dim downloadStatus As Integer
    Dim downloadSuccesfull As Boolean = False

    Private Sub MainForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        My.Application.SaveMySettingsOnExit = True

        If My.Settings.ignoreUpdate Then
            Console.WriteLine("Ignoring update...")
            Process.Start(currentPath & "\Game.exe")
            Application.Exit()
            ckbIgnore.Checked = True
        End If
        changeDownloadStatus(_Check)
        Dim checkResult As Integer = checkUpdate(True)

        If checkResult = _Under And downloadStatus <> _Downloading Then
            Dim userRes As MsgBoxResult = MsgBox("Una nuova versione è disponibile (" & latestVer & ") vuoi scaricarla?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Nuova versione disponibile")
            If userRes = MsgBoxResult.Yes Then
                downloadUpdate()
            End If
        ElseIf checkResult <> _Update And downloadStatus <> _Downloading
            Process.Start(currentPath & "\Game.exe")
            Application.Exit()
        End If
    End Sub

    Private Sub Form1_Close(sender As Object, e As EventArgs) Handles MyBase.Closing
        checkbeforeClose()
    End Sub

    Private Function checkUpdate(silent As Boolean)
        currentVer = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" & APP_NAME, "DisplayVersion", Nothing)
        If currentVer Is Nothing Then
            Console.WriteLine("Can't read the version key from Windows registry")
            currentVer = "---"
            Try
                Dim fileVerReader As New StreamReader(VERSION_FILE_NAME)
                currentVer = fileVerReader.ReadToEnd()
            Catch ex As IOException
                Console.WriteLine("Can't find file " & VERSION_FILE_NAME & ": " & ex.Message)
                MsgBox("Impossibile leggere la versione attuale del gioco", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Versione del gioco sconosciuta")
            End Try
            If My.Settings.installationAlert Then
                Dim mUserResponse As MsgBoxResult = MsgBox("Il gioco non è installato correttamente, questo potrebbe compromettere il funzionamento del sistema d'aggiornamento, è consigliabile re-installarlo (i dati di gioco non andranno persi)" & vbCrLf & "Vuoi scaricarlo e installarlo? (No = Non visualizzare più questo messaggio)", MsgBoxStyle.Information + MsgBoxStyle.YesNoCancel, "Installazione non corretta")
                If mUserResponse = MsgBoxResult.Yes Then
                    My.Settings.installationAlert = False
                    If checkUpdate(False) <> _Unknown Then
                        downloadUpdate()
                    End If
                    My.Settings.installationAlert = True
                ElseIf mUserResponse = MsgBoxResult.No Then
                    My.Settings.installationAlert = False
                End If
            End If
        End If
        Console.WriteLine("Current version: " & currentVer)
        lblCurrentVer.Text = currentVer

        latestVer = "---"
        Try
            Dim request As HttpWebRequest = HttpWebRequest.Create(URL)
            request.Proxy = Nothing
            request.UserAgent = "Test"

            Dim response As HttpWebResponse = request.GetResponse
            Dim responseStream As System.IO.Stream = response.GetResponseStream

            Dim streamReader As New System.IO.StreamReader(responseStream)
            data = streamReader.ReadToEnd
            streamReader.Close()
        Catch ex As WebException
            Console.WriteLine(ex.Message)
            If Not silent Then
                MsgBox("Impossibile connettersi al server" & vbCrLf & "Controlla se la connessione a internet è attiva" & vbCrLf & ex.Message, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Controllo aggiornamenti fallito")
            End If
            Return _Unknown
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            If Not silent Then
                MsgBox("Impossibile controllare gli aggiornamenti" & vbCrLf & ex.Message, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Controllo aggiornamenti fallito")
            End If
            Return _Unknown
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

        haveNew = _Unknown
        If Not currentVer Is Nothing And Not latestVer Is Nothing Then
            haveNew = checkVersion(currentVer, latestVer)
            If haveNew = _Under Then
                Console.WriteLine("New version available: " & latestVer & "(> " & currentVer & ")")
                If downloadStatus <> _Downloading Then
                    changeDownloadStatus(_Update)
                End If
            ElseIf haveNew = _Over Then
                Console.WriteLine("Not-available version: " & latestVer & "(< " & currentVer & ")")
            ElseIf haveNew = _Equal Then
                Console.WriteLine("Updated version: " & latestVer & "(= " & currentVer & ")")
            End If
        End If
        Return haveNew
    End Function

    Private Sub downloadUpdate()
        Try
            lblInfo.Text = "Avvio download"
            download.DownloadFileAsync(New Uri(downloadLink), "install.exe")
            changeDownloadStatus(_Downloading)
            downloadSuccesfull = True
        Catch ex As WebException
            Console.WriteLine(ex.Message)
            MsgBox("Errore nel download del file: " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
        End Try
    End Sub

    Private Sub changeDownloadStatus(status As Integer)
        downloadStatus = status
        Select Case status
            Case _Check
                btnUpdate.Text = "Controlla aggiornamenti"
            Case _Update
                btnUpdate.Text = "Scarica aggiornamento"
            Case _Downloading
                btnUpdate.Text = "Annulla aggiornamento"
        End Select
    End Sub

    Private Sub download_DownloadProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs) Handles download.DownloadProgressChanged
        If downloadStatus = _Downloading Then
            downloadBar.Value = e.ProgressPercentage
            lblInfo.Text = e.ProgressPercentage & "% - Scaricato " & Round(CDbl(e.BytesReceived) / 1048576, 1) & " di " & Round(CDbl(e.TotalBytesToReceive) / 1048576, 1) & " MB"
        End If
    End Sub

    Private Sub download_DownloadFileCompleted() Handles download.DownloadFileCompleted
        If downloadSuccesfull Then
            Try
                Process.Start(currentPath & "\install.exe")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
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
        If downloadStatus = _Downloading Then
            lblInfo.Text = "Annullamento download"
            downloadSuccesfull = False
            download.CancelAsync()
            download.Dispose()
            changeDownloadStatus(_Update)
            Threading.Thread.Sleep(400)
            My.Computer.FileSystem.DeleteFile(currentPath & "\install.exe")
            downloadBar.Value = 0
            lblInfo.Text = "Pronto"
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Select Case downloadStatus
            Case _Check
                checkUpdate(False)

                If downloadStatus = _Update Then
                    Dim userRes As MsgBoxResult = MsgBox("Una nuova versione è disponibile (" & latestVer & ") vuoi scaricarla?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Nuova versione disponibile")
                    If userRes = MsgBoxResult.Yes Then
                        downloadUpdate()
                    End If
                End If
            Case _Update
                btnUpdate.Text = "Scarica aggiornamento"
                If checkUpdate(False) = _Under Then
                    downloadUpdate()
                Else
                    changeDownloadStatus(_Check)
                End If
            Case _Downloading
                lblInfo.Text = "Annullamento download"
                downloadSuccesfull = False
                download.CancelAsync()
                download.Dispose()
                changeDownloadStatus(_Update)
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
        If ckbIgnore.Checked = True Then
            Dim mConfirm As MsgBoxResult = MsgBox("ATTENZIONE: abilitando questa opzione non sarà più possibile accedere all'updater, quindi l'unico modo per disabilitarla sarà modificare manualmente il file di configurazione" & vbCrLf & "Sei sicuro di volerla abilitare?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Attenzione")
            If mConfirm = MsgBoxResult.No Then
                ckbIgnore.Checked = False
            End If
        End If
    End Sub
End Class
