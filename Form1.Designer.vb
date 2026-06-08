<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.grpGlobalMode = New System.Windows.Forms.GroupBox()
        Me.btnBlockAll = New System.Windows.Forms.Button()
        Me.btnAllowAll = New System.Windows.Forms.Button()
        Me.grpSpecificMode = New System.Windows.Forms.GroupBox()
        Me.btnClearSearch = New System.Windows.Forms.Button()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.btnExportReport = New System.Windows.Forms.Button()
        Me.lvwUSBPorts = New System.Windows.Forms.ListView()
        Me.btnBlockSpecific = New System.Windows.Forms.Button()
        Me.btnAllowSpecific = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpGlobalMode.SuspendLayout()
        Me.grpSpecificMode.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblStatus
        '
        Me.lblStatus.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.lblStatus.ForeColor = System.Drawing.Color.Navy
        Me.lblStatus.Location = New System.Drawing.Point(33, 22)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(409, 23)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "SYSTEM STATUS: INITIALIZED"
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpGlobalMode
        '
        Me.grpGlobalMode.Controls.Add(Me.btnBlockAll)
        Me.grpGlobalMode.Controls.Add(Me.btnAllowAll)
        Me.grpGlobalMode.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.grpGlobalMode.Location = New System.Drawing.Point(12, 48)
        Me.grpGlobalMode.Name = "grpGlobalMode"
        Me.grpGlobalMode.Size = New System.Drawing.Size(459, 130)
        Me.grpGlobalMode.TabIndex = 1
        Me.grpGlobalMode.TabStop = False
        Me.grpGlobalMode.Text = " التحكم الشامل "
        '
        'btnBlockAll
        '
        Me.btnBlockAll.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.btnBlockAll.Location = New System.Drawing.Point(30, 25)
        Me.btnBlockAll.Name = "btnBlockAll"
        Me.btnBlockAll.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.btnBlockAll.Size = New System.Drawing.Size(393, 32)
        Me.btnBlockAll.TabIndex = 0
        Me.btnBlockAll.Text = "حظر جميع منافذ USB"
        Me.btnBlockAll.UseVisualStyleBackColor = True
        '
        'btnAllowAll
        '
        Me.btnAllowAll.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnAllowAll.Location = New System.Drawing.Point(30, 68)
        Me.btnAllowAll.Name = "btnAllowAll"
        Me.btnAllowAll.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.btnAllowAll.Size = New System.Drawing.Size(393, 32)
        Me.btnAllowAll.TabIndex = 1
        Me.btnAllowAll.Text = "تفعيل جميع منافذ USB"
        Me.btnAllowAll.UseVisualStyleBackColor = True
        '
        'grpSpecificMode
        '
        Me.grpSpecificMode.Controls.Add(Me.btnClearSearch)
        Me.grpSpecificMode.Controls.Add(Me.txtSearch)
        Me.grpSpecificMode.Controls.Add(Me.btnExportReport)
        Me.grpSpecificMode.Controls.Add(Me.lvwUSBPorts)
        Me.grpSpecificMode.Controls.Add(Me.btnBlockSpecific)
        Me.grpSpecificMode.Controls.Add(Me.btnAllowSpecific)
        Me.grpSpecificMode.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.grpSpecificMode.Location = New System.Drawing.Point(12, 178)
        Me.grpSpecificMode.Name = "grpSpecificMode"
        Me.grpSpecificMode.Size = New System.Drawing.Size(459, 237)
        Me.grpSpecificMode.TabIndex = 2
        Me.grpSpecificMode.TabStop = False
        Me.grpSpecificMode.Text = " التحكم بالمنافذ المتصلة حالياً "
        '
        'btnClearSearch
        '
        Me.btnClearSearch.Location = New System.Drawing.Point(20, 17)
        Me.btnClearSearch.Name = "btnClearSearch"
        Me.btnClearSearch.Size = New System.Drawing.Size(85, 23)
        Me.btnClearSearch.TabIndex = 5
        Me.btnClearSearch.Text = "إلغاء البحث 🔄"
        Me.btnClearSearch.UseVisualStyleBackColor = True
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(111, 17)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(325, 23)
        Me.txtSearch.TabIndex = 4
        '
        'btnExportReport
        '
        Me.btnExportReport.BackColor = System.Drawing.Color.Ivory
        Me.btnExportReport.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnExportReport.Location = New System.Drawing.Point(361, 157)
        Me.btnExportReport.Name = "btnExportReport"
        Me.btnExportReport.Size = New System.Drawing.Size(75, 23)
        Me.btnExportReport.TabIndex = 3
        Me.btnExportReport.Text = "Report"
        Me.btnExportReport.UseVisualStyleBackColor = False
        '
        'lvwUSBPorts
        '
        Me.lvwUSBPorts.FullRowSelect = True
        Me.lvwUSBPorts.GridLines = True
        Me.lvwUSBPorts.Location = New System.Drawing.Point(20, 41)
        Me.lvwUSBPorts.MultiSelect = False
        Me.lvwUSBPorts.Name = "lvwUSBPorts"
        Me.lvwUSBPorts.Size = New System.Drawing.Size(416, 113)
        Me.lvwUSBPorts.TabIndex = 0
        Me.lvwUSBPorts.UseCompatibleStateImageBehavior = False
        Me.lvwUSBPorts.View = System.Windows.Forms.View.Details
        '
        'btnBlockSpecific
        '
        Me.btnBlockSpecific.BackColor = System.Drawing.Color.Red
        Me.btnBlockSpecific.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.btnBlockSpecific.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.btnBlockSpecific.Location = New System.Drawing.Point(59, 189)
        Me.btnBlockSpecific.Name = "btnBlockSpecific"
        Me.btnBlockSpecific.Size = New System.Drawing.Size(155, 32)
        Me.btnBlockSpecific.TabIndex = 1
        Me.btnBlockSpecific.Text = "🔒 حظر المنفذ"
        Me.btnBlockSpecific.UseVisualStyleBackColor = False
        '
        'btnAllowSpecific
        '
        Me.btnAllowSpecific.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnAllowSpecific.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnAllowSpecific.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.btnAllowSpecific.Location = New System.Drawing.Point(240, 189)
        Me.btnAllowSpecific.Name = "btnAllowSpecific"
        Me.btnAllowSpecific.Size = New System.Drawing.Size(155, 32)
        Me.btnAllowSpecific.TabIndex = 2
        Me.btnAllowSpecific.Text = "🔓 تفعيل المنفذ"
        Me.btnAllowSpecific.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Green
        Me.Label1.Location = New System.Drawing.Point(148, 418)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(167, 15)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Developer by Asaed Dughman"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(504, 459)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.grpSpecificMode)
        Me.Controls.Add(Me.grpGlobalMode)
        Me.Controls.Add(Me.lblStatus)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "USB Guard - USB Control"
        Me.grpGlobalMode.ResumeLayout(False)
        Me.grpSpecificMode.ResumeLayout(False)
        Me.grpSpecificMode.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents grpGlobalMode As System.Windows.Forms.GroupBox
    Friend WithEvents btnBlockAll As System.Windows.Forms.Button
    Friend WithEvents btnAllowAll As System.Windows.Forms.Button
    Friend WithEvents grpSpecificMode As System.Windows.Forms.GroupBox
    Friend WithEvents lvwUSBPorts As System.Windows.Forms.ListView
    Friend WithEvents btnBlockSpecific As System.Windows.Forms.Button
    Friend WithEvents btnAllowSpecific As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnExportReport As Button
    Friend WithEvents btnClearSearch As Button
    Friend WithEvents txtSearch As TextBox
End Class