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
    Public Const VERSION_FILE_NAME = "VERSION"
    Public Const _Unknown = -2
    Public Const _Over = -1
    Public Const _Equal = 0
    Public Const _Under = 1
    Public Const _Check = 2
    Public Const _Update = 3
    Public Const _Downloading = 4

    Dim fileArray As List(Of downlodableFile) = New List(Of downlodableFile)

    Dim currentPath As String = Directory.GetCurrentDirectory()
    Dim currentVer As String
    Dim latestVer As String
    Dim downloadLink As String
    Dim incremetalVer As String
    Dim data As String
    Dim jsonObject As JObject
    Dim items As List(Of JToken)
    Dim jsonReader As StreamReader
    Dim haveNew As Integer
    Dim WithEvents download As WebClient = New WebClient
    Dim downloadStatus As Integer
    Dim downloadSuccesfull As Boolean = False ' To Remove
    Dim WithEvents selectedFile As downlodableFile

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        My.Application.SaveMySettingsOnExit = True

        If My.Settings.ignoreUpdate Then
            Console.WriteLine("Ignoring update...")
            Process.Start(currentPath & "\Game.exe")
            Application.Exit()
            ckbIgnore.Checked = True
        End If
        changeDownloadStatus(_Check)
        Dim checkResult As Integer = checkUpdate(True)


        If checkResult <> _Under Then
            Try
                Process.Start(currentPath & "\Game.exe")
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            Application.Exit()
        End If
    End Sub

    Private Sub MainForm_Close(sender As Object, e As EventArgs) Handles MyBase.Closing
        checkBeforeClose()
    End Sub

    Public Function checkUpdate(silent As Boolean)
        btnUpdate.Enabled = False
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
                MsgBox("Il gioco non è installato correttamente, questo potrebbe compromettere il funzionamento del sistema d'aggiornamento, è consigliabile re-installarlo", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Installazione non corretta")
            End If
        End If
        Console.WriteLine("Current version: " & currentVer)
        lblCurrentVer.Text = currentVer

        radioSetup.Enabled = False
        radioSetup.Checked = False
        radioIncremental.Enabled = False
        radioIncremental.Checked = False

        latestVer = "---"
        Try
            Dim request As HttpWebRequest = HttpWebRequest.Create(URL)
            request.Proxy = Nothing
            request.UserAgent = "CPs Updater v1.1"

            Dim response As HttpWebResponse = request.GetResponse
            Dim responseStream As System.IO.Stream = response.GetResponseStream

            Dim streamReader As New System.IO.StreamReader(responseStream)
            data = streamReader.ReadToEnd
            response.Dispose()
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

        fileArray.Clear()

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
                    lblIncrementalStatus.Text = "In questa versione non è ancora presente" & vbCrLf & "alcun aggiornamento incrementale"
                    For Each assets As JObject In item.Value
                        downloadLink = assets("browser_download_url")
                        Console.WriteLine("Download URL: " & downloadLink)
                        Dim newDownloadableFile As downlodableFile = New downlodableFile(Me, assets, latestVer)
                        Console.WriteLine("New Downloadable File created As " & newDownloadableFile.type)
                        fileArray.Add(newDownloadableFile)
                        Select Case newDownloadableFile.type
                            Case "Setup"
                                radioSetup.Enabled = True
                            Case "Incremental"
                                If newDownloadableFile.checkCompatibility(currentVer) Then
                                    radioIncremental.Enabled = True
                                    lblIncrementalStatus.Text = "E' disponibile un aggiornamento" & vbCrLf & "incrementale per la tua versione!"
                                Else
                                    lblIncrementalStatus.Text = "Non è presente alcun aggiornamento" & vbCrLf & "incrementale compatibile con la tua versione"
                                End If
                        End Select
                    Next
                    If radioSetup.Enabled And radioIncremental.Enabled Then   ' bad, very bad control
                        radioIncremental.Checked = True
                    ElseIf radioSetup.Enabled
                        radioSetup.Checked = True
                    ElseIf radioIncremental.Enabled
                        radioIncremental.Checked = True
                    End If
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
        btnUpdate.Enabled = True
        Return haveNew
    End Function

    Public Sub downloadUpdate()
        Try
            lblInfo.Text = "Avvio download"
            btnUpdate.Enabled = False
            Dim typeChoosed As String = ""
            If radioSetup.Checked Then
                typeChoosed = "Setup"
            ElseIf radioIncremental.Checked
                typeChoosed = "Incremental"
            End If
            For Each file As downlodableFile In fileArray
                If file.type = typeChoosed And (typeChoosed <> "Incremental" Or file.compatibleVersion = currentVer) Then
                    selectedFile = file
                    file.startDownload()
                    radioSetup.Enabled = False
                    radioIncremental.Enabled = False
                    Exit For
                End If
            Next
        Catch ex As WebException
            Console.WriteLine(ex.Message)
            MsgBox("Errore nel download del file: " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
        Finally
            lblInfo.Text = "Pronto"
            btnUpdate.Enabled = True
        End Try
    End Sub

    Public Sub changeDownloadStatus(status As Integer)
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

    Private Sub download_DownloadProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs) Handles selectedFile.DownloadProgressChanged
        If downloadStatus = _Downloading Then
            downloadBar.Value = e.ProgressPercentage
            lblInfo.Text = selectedFile.type & " - " & e.ProgressPercentage & "% - Scaricato " & Round(CDbl(e.BytesReceived) / 1048576, 1) & " di " & Round(CDbl(e.TotalBytesToReceive) / 1048576, 1) & " MB"
        End If
    End Sub

    Private Sub download_DownloadFileCompleted() Handles selectedFile.DownloadFileCompleted
        If selectedFile.isDownloadSuccessfull Then
            Dim userResponse As MsgBoxResult = MsgBox("Il download è stato completato, vuoi installarlo?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Download completato")
            If userResponse = MsgBoxResult.Yes Then
                Try
                    Process.Start(currentPath & "\install.exe")
                    Application.Exit()
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                End Try
            Else
                changeDownloadStatus(_Check)
                radioIncremental.Enabled = False
                radioIncremental.Checked = False
                radioSetup.Enabled = False
                radioSetup.Checked = False
            End If
        End If
    End Sub

    Public Function checkVersion(prev As String, last As String)
        Dim prevArray As List(Of String) = New List(Of String)
        Dim lastArray As List(Of String) = New List(Of String)
        Dim prevValue As Integer
        Dim lastValue As Integer
        Dim haveNew As Integer = 0
        Dim i As Integer = 0
        Dim preVersionType As String = "Stable"     ' Unused
        Dim lastVersionType As String = "Stable"    ' Unused
        For Each splitted As String In Split(prev, ".")
            prevArray.AddRange(Split(splitted, "-"))
        Next
        For Each splitted As String In Split(last, ".")
            lastArray.AddRange(Split(splitted, "-"))
        Next
        prevArray(0) = prevArray(0).Trim("v")
        lastArray(0) = lastArray(0).Trim("v")

        Do While haveNew = 0
            Try
                prevValue = CInt(prevArray(i))
            Catch ex As InvalidCastException
                preVersionType = prevArray(i)
                prevValue = 0
            Catch ex As Exception
                prevValue = 0
            End Try
            Try
                lastValue = CInt(lastArray(i))
            Catch ex As InvalidCastException
                preVersionType = prevArray(i)
                prevValue = 0
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

    Public Sub checkBeforeClose()
        If downloadStatus = _Downloading Then
            lblInfo.Text = "Annullamento download"
            btnUpdate.Enabled = False
            For Each downloadableUpdate In fileArray
                Select Case downloadableUpdate.type
                    Case "Setup"
                        radioSetup.Enabled = True
                    Case "Incremental"
                        radioIncremental.Enabled = True
                End Select
            Next
            ' Select Case selectedFile.type
            'Case "Setup"
            'radioSetup.Checked = True
            'End Select
            selectedFile.stopDownload(currentPath)
            changeDownloadStatus(_Update)
            downloadBar.Value = 0
            lblInfo.Text = "Pronto"
            btnUpdate.Enabled = True
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Select Case downloadStatus
            Case _Check
                checkUpdate(False)
            Case _Update
                btnUpdate.Text = "Scarica aggiornamento"
                ' If checkUpdate(False) = _Under Then
                downloadUpdate()
            'Else
            'changeDownloadStatus(_Check)
            'End If
            Case _Downloading
                checkBeforeClose()
        End Select
    End Sub

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        checkBeforeClose()
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
