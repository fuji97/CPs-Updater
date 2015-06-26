<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.lblStaticCurrentVer = New System.Windows.Forms.Label()
        Me.lblCurrentVer = New System.Windows.Forms.Label()
        Me.lblStaticLatestVer = New System.Windows.Forms.Label()
        Me.lblLatestVersion = New System.Windows.Forms.Label()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.ckbIgnore = New System.Windows.Forms.CheckBox()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.downloadBar = New System.Windows.Forms.ProgressBar()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.infoPanel = New System.Windows.Forms.Panel()
        Me.grbVersion = New System.Windows.Forms.GroupBox()
        Me.radioIncremental = New System.Windows.Forms.RadioButton()
        Me.radioSetup = New System.Windows.Forms.RadioButton()
        Me.lblIncrementalStatus = New System.Windows.Forms.Label()
        Me.infoPanel.SuspendLayout()
        Me.grbVersion.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblStaticCurrentVer
        '
        Me.lblStaticCurrentVer.AutoSize = True
        Me.lblStaticCurrentVer.Location = New System.Drawing.Point(13, 13)
        Me.lblStaticCurrentVer.Name = "lblStaticCurrentVer"
        Me.lblStaticCurrentVer.Size = New System.Drawing.Size(86, 13)
        Me.lblStaticCurrentVer.TabIndex = 0
        Me.lblStaticCurrentVer.Text = "Versione attuale:"
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
        'lblStaticLatestVer
        '
        Me.lblStaticLatestVer.AutoSize = True
        Me.lblStaticLatestVer.Location = New System.Drawing.Point(176, 13)
        Me.lblStaticLatestVer.Name = "lblStaticLatestVer"
        Me.lblStaticLatestVer.Size = New System.Drawing.Size(82, 13)
        Me.lblStaticLatestVer.TabIndex = 2
        Me.lblStaticLatestVer.Text = "Ultima versione:"
        '
        'lblLatestVersion
        '
        Me.lblLatestVersion.AutoSize = True
        Me.lblLatestVersion.BackColor = System.Drawing.Color.LemonChiffon
        Me.lblLatestVersion.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLatestVersion.Location = New System.Drawing.Point(260, 12)
        Me.lblLatestVersion.Name = "lblLatestVersion"
        Me.lblLatestVersion.Size = New System.Drawing.Size(22, 13)
        Me.lblLatestVersion.TabIndex = 3
        Me.lblLatestVersion.Text = "---"
        '
        'lblInfo
        '
        Me.lblInfo.AutoSize = True
        Me.lblInfo.BackColor = System.Drawing.SystemColors.ControlLight
        Me.lblInfo.Location = New System.Drawing.Point(4, 4)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(38, 13)
        Me.lblInfo.TabIndex = 4
        Me.lblInfo.Text = "Pronto"
        '
        'ckbIgnore
        '
        Me.ckbIgnore.AutoSize = True
        Me.ckbIgnore.Location = New System.Drawing.Point(134, 94)
        Me.ckbIgnore.Name = "ckbIgnore"
        Me.ckbIgnore.Size = New System.Drawing.Size(179, 17)
        Me.ckbIgnore.TabIndex = 5
        Me.ckbIgnore.Text = "Ignora gli aggiornamenti in futuro"
        Me.ckbIgnore.UseVisualStyleBackColor = True
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(221, 124)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(75, 23)
        Me.btnStart.TabIndex = 6
        Me.btnStart.Text = "Avvia gioco"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'downloadBar
        '
        Me.downloadBar.Location = New System.Drawing.Point(12, 153)
        Me.downloadBar.Name = "downloadBar"
        Me.downloadBar.Size = New System.Drawing.Size(322, 19)
        Me.downloadBar.TabIndex = 7
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(44, 124)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(138, 23)
        Me.btnUpdate.TabIndex = 8
        Me.btnUpdate.Text = "Controlla aggiornamenti"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'infoPanel
        '
        Me.infoPanel.BackColor = System.Drawing.SystemColors.ControlLight
        Me.infoPanel.Controls.Add(Me.lblInfo)
        Me.infoPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.infoPanel.Location = New System.Drawing.Point(0, 184)
        Me.infoPanel.Name = "infoPanel"
        Me.infoPanel.Size = New System.Drawing.Size(346, 21)
        Me.infoPanel.TabIndex = 9
        '
        'grbVersion
        '
        Me.grbVersion.Controls.Add(Me.radioIncremental)
        Me.grbVersion.Controls.Add(Me.radioSetup)
        Me.grbVersion.Location = New System.Drawing.Point(7, 34)
        Me.grbVersion.Name = "grbVersion"
        Me.grbVersion.Size = New System.Drawing.Size(103, 77)
        Me.grbVersion.TabIndex = 10
        Me.grbVersion.TabStop = False
        Me.grbVersion.Text = "Versione"
        '
        'radioIncremental
        '
        Me.radioIncremental.AutoSize = True
        Me.radioIncremental.Enabled = False
        Me.radioIncremental.Location = New System.Drawing.Point(9, 45)
        Me.radioIncremental.Name = "radioIncremental"
        Me.radioIncremental.Size = New System.Drawing.Size(80, 17)
        Me.radioIncremental.TabIndex = 1
        Me.radioIncremental.TabStop = True
        Me.radioIncremental.Text = "Incremental"
        Me.radioIncremental.UseVisualStyleBackColor = True
        '
        'radioSetup
        '
        Me.radioSetup.AutoSize = True
        Me.radioSetup.Enabled = False
        Me.radioSetup.Location = New System.Drawing.Point(9, 22)
        Me.radioSetup.Name = "radioSetup"
        Me.radioSetup.Size = New System.Drawing.Size(53, 17)
        Me.radioSetup.TabIndex = 0
        Me.radioSetup.TabStop = True
        Me.radioSetup.Text = "Setup"
        Me.radioSetup.UseVisualStyleBackColor = True
        '
        'lblIncrementalStatus
        '
        Me.lblIncrementalStatus.AutoSize = True
        Me.lblIncrementalStatus.Location = New System.Drawing.Point(131, 50)
        Me.lblIncrementalStatus.Name = "lblIncrementalStatus"
        Me.lblIncrementalStatus.Size = New System.Drawing.Size(0, 13)
        Me.lblIncrementalStatus.TabIndex = 2
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(346, 205)
        Me.Controls.Add(Me.lblIncrementalStatus)
        Me.Controls.Add(Me.grbVersion)
        Me.Controls.Add(Me.infoPanel)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.downloadBar)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.ckbIgnore)
        Me.Controls.Add(Me.lblLatestVersion)
        Me.Controls.Add(Me.lblStaticLatestVer)
        Me.Controls.Add(Me.lblCurrentVer)
        Me.Controls.Add(Me.lblStaticCurrentVer)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainForm"
        Me.Text = "CPs Updater"
        Me.infoPanel.ResumeLayout(False)
        Me.infoPanel.PerformLayout()
        Me.grbVersion.ResumeLayout(False)
        Me.grbVersion.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblStaticCurrentVer As Label
    Friend WithEvents lblCurrentVer As Label
    Friend WithEvents lblStaticLatestVer As Label
    Friend WithEvents lblLatestVersion As Label
    Friend WithEvents lblInfo As Label
    Friend WithEvents ckbIgnore As CheckBox
    Friend WithEvents btnStart As Button
    Friend WithEvents downloadBar As ProgressBar
    Friend WithEvents btnUpdate As Button
    Friend WithEvents infoPanel As Panel
    Friend WithEvents grbVersion As GroupBox
    Friend WithEvents radioIncremental As RadioButton
    Friend WithEvents radioSetup As RadioButton
    Friend WithEvents lblIncrementalStatus As Label
End Class
