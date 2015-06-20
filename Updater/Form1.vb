Imports System
Imports System.Net
Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Collections.Generic

Public Class Form1

    Const appName = "{0AGHJ15F-G95D-06F4-KM90-G9XZI5DON37D}_is1"
    Const url = "https://api.github.com/repos/Bl00d-Kirito/HSK-Cremisi_Portals_Demo/releases/latest"
    Dim currentVer As String
    Dim latestVer As String
    Dim data As String
    Dim jsonObject As JObject
    Dim items As List(Of JToken)
    Dim jsonReader As StreamReader
    Dim haveNew As Integer

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
                End If
            Next
        End If
        If Not currentVer Is Nothing And Not latestVer Is Nothing Then
            haveNew = checkVersion(currentVer, latestVer)
            If haveNew = 1 Then
                Console.WriteLine("New version avaiable: " & latestVer & "(> " & currentVer & ")")
            ElseIf haveNew = -1 Then
                Console.WriteLine("Not-avaiable version: " & latestVer & "(< " & currentVer & ")")
            ElseIf haveNew = 0 Then
                Console.WriteLine("Updated version: " & latestVer & "(= " & currentVer & ")")
            End If
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
End Class
