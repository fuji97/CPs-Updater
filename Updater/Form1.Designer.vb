<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lblText1 = New System.Windows.Forms.Label()
        Me.lblCurrentVer = New System.Windows.Forms.Label()
        Me.lblText2 = New System.Windows.Forms.Label()
        Me.lblLatestVersion = New System.Windows.Forms.Label()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.ckbIgnore = New System.Windows.Forms.CheckBox()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.downloadBar = New System.Windows.Forms.ProgressBar()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblText1
        '
        Me.lblText1.AutoSize = True
        Me.lblText1.Location = New System.Drawing.Point(13, 13)
        Me.lblText1.Name = "lblText1"
        Me.lblText1.Size = New System.Drawing.Size(86, 13)
        Me.lblText1.TabIndex = 0
        Me.lblText1.Text = "Versione attuale:"
        '
        'lblCurrentVer
        '
        Me.lblCurrentVer.AutoSize = True
        Me.lblCurrentVer.BackColor = System.Drawing.Color.LemonChiffon
        Me.lblCurrentVer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentVer.Location = New System.Drawing.Point(102, 13)
        Me.lblCurrentVer.Name = "lblCurrentVer"
        Me.lblCurrentVer.Size = New System.Drawing.Size(22, 13)
        Me.lblCurrentVer.TabIndex = 1
        Me.lblCurrentVer.Text = "---"
        '
        'lblText2
        '
        Me.lblText2.AutoSize = True
        Me.lblText2.Location = New System.Drawing.Point(164, 13)
        Me.lblText2.Name = "lblText2"
        Me.lblText2.Size = New System.Drawing.Size(82, 13)
        Me.lblText2.TabIndex = 2
        Me.lblText2.Text = "Ultima versione:"
        '
        'lblLatestVersion
        '
        Me.lblLatestVersion.AutoSize = True
        Me.lblLatestVersion.BackColor = System.Drawing.Color.LemonChiffon
        Me.lblLatestVersion.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLatestVersion.Location = New System.Drawing.Point(248, 12)
        Me.lblLatestVersion.Name = "lblLatestVersion"
        Me.lblLatestVersion.Size = New System.Drawing.Size(22, 13)
        Me.lblLatestVersion.TabIndex = 3
        Me.lblLatestVersion.Text = "---"
        '
        'lblInfo
        '
        Me.lblInfo.AutoSize = True
        Me.lblInfo.Location = New System.Drawing.Point(3, 117)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(38, 13)
        Me.lblInfo.TabIndex = 4
        Me.lblInfo.Text = "Pronto"
        '
        'ckbIgnore
        '
        Me.ckbIgnore.AutoSize = True
        Me.ckbIgnore.Location = New System.Drawing.Point(16, 46)
        Me.ckbIgnore.Name = "ckbIgnore"
        Me.ckbIgnore.Size = New System.Drawing.Size(179, 17)
        Me.ckbIgnore.TabIndex = 5
        Me.ckbIgnore.Text = "Ignora gli aggiornamenti in futuro"
        Me.ckbIgnore.UseVisualStyleBackColor = True
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(314, 8)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(75, 23)
        Me.btnStart.TabIndex = 6
        Me.btnStart.Text = "Avvia gioco"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'downloadBar
        '
        Me.downloadBar.Location = New System.Drawing.Point(13, 85)
        Me.downloadBar.Name = "downloadBar"
        Me.downloadBar.Size = New System.Drawing.Size(376, 23)
        Me.downloadBar.TabIndex = 7
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(251, 56)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(138, 23)
        Me.btnUpdate.TabIndex = 8
        Me.btnUpdate.Text = "Controlla aggiornamenti"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(401, 133)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.downloadBar)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.ckbIgnore)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.lblLatestVersion)
        Me.Controls.Add(Me.lblText2)
        Me.Controls.Add(Me.lblCurrentVer)
        Me.Controls.Add(Me.lblText1)
        Me.Name = "Form1"
        Me.Text = "CPs updater"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblText1 As Label
    Friend WithEvents lblCurrentVer As Label
    Friend WithEvents lblText2 As Label
    Friend WithEvents lblLatestVersion As Label
    Friend WithEvents lblInfo As Label
    Friend WithEvents ckbIgnore As CheckBox
    Friend WithEvents btnStart As Button
    Friend WithEvents downloadBar As ProgressBar
    Friend WithEvents btnUpdate As Button
End Class
