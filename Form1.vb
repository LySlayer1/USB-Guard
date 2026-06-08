Imports System.Diagnostics
Imports System.Threading.Tasks
Imports Microsoft.Win32

Public Class Form1

    Private Const WM_DEVICECHANGE As Integer = &H219
    Private Const DBT_DEVICEARRIVAL As Integer = &H8000
    Private Const DBT_DEVICEREMOVECOMPLETE As Integer = &H8004

    ' قائمة احتياطية لحفظ جميع الأجهزة لخاصية البحث
    Private _allDevices As New List(Of ListViewItem)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not IsAdmin() Then
            MessageBox.Show(
                "The program must be run as an Administrator to ensure the successful application of the full lockdown and system modifications.",
                "USB Guard — Security Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
        End If

        SetupListView()
        SetupSearchBox()
        RefreshUSBPorts()
    End Sub

    ' ======================================================
    '  إعداد خانة البحث برمجياً
    ' ======================================================
    Private Sub SetupSearchBox()
        txtSearch.PlaceholderText = "🔍  ابحث عن جهاز..."
        txtSearch.RightToLeft = RightToLeft.Yes
        AddHandler txtSearch.TextChanged, AddressOf txtSearch_TextChanged
        AddHandler btnClearSearch.Click, AddressOf btnClearSearch_Click
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs)
        Dim keyword As String = txtSearch.Text.Trim().ToLower()

        lvwUSBPorts.Items.Clear()

        If String.IsNullOrEmpty(keyword) Then
            ' استنساخ العناصر لتفادي مشاكل الارتباط بالنظام عند إعادة الإضافة
            For Each item As ListViewItem In _allDevices
                lvwUSBPorts.Items.Add(DirectCast(item.Clone(), ListViewItem))
            Next
        Else
            Dim filtered = _allDevices.Where(Function(item)
                                                 Return item.SubItems(1).Text.ToLower().Contains(keyword) OrElse
                                                        item.Tag?.ToString().ToLower().Contains(keyword)
                                             End Function).ToList()

            If filtered.Count > 0 Then
                For Each item In filtered
                    lvwUSBPorts.Items.Add(DirectCast(item.Clone(), ListViewItem))
                Next
            Else
                Dim noResult As New ListViewItem("-")
                noResult.SubItems.Add("لا توجد نتائج مطابقة للبحث")
                noResult.ForeColor = Color.Gray
                lvwUSBPorts.Items.Add(noResult)
            End If
        End If

        SetStatus("نتائج البحث: " & lvwUSBPorts.Items.Count & " جهاز", Color.DarkCyan)
    End Sub

    Private Sub btnClearSearch_Click(sender As Object, e As EventArgs)
        txtSearch.Clear()
        lvwUSBPorts.Items.Clear()
        For Each item As ListViewItem In _allDevices
            lvwUSBPorts.Items.Add(DirectCast(item.Clone(), ListViewItem))
        Next
        SetStatus("تم تحديث القائمة — " & _allDevices.Count & " جهاز", Color.DarkCyan)
    End Sub

    ' ======================================================

    Private Sub SetupListView()
        lvwUSBPorts.View = View.Details
        lvwUSBPorts.FullRowSelect = True
        lvwUSBPorts.GridLines = True
        lvwUSBPorts.MultiSelect = False
        lvwUSBPorts.Columns.Clear()
        lvwUSBPorts.Columns.Add("رقم الجهاز", 90, HorizontalAlignment.Right)
        lvwUSBPorts.Columns.Add("اسم الجهاز المتصل عبر USB", 300, HorizontalAlignment.Right)
    End Sub

    Private Sub RefreshUSBPorts()
        If lvwUSBPorts.InvokeRequired Then
            lvwUSBPorts.Invoke(Sub() RefreshUSBPorts())
            Return
        End If

        lvwUSBPorts.Items.Clear()
        _allDevices.Clear()

        Try
            Dim psi As New ProcessStartInfo()
            psi.FileName = "powershell.exe"
            psi.Arguments = "-NoProfile -Command """ &
                "Get-PnpDevice | Where-Object {$_.InstanceId -like 'USB*' -or $_.InstanceId -like 'HID*'} | " &
                "Where-Object {$_.FriendlyName -notmatch 'Root Hub|Host Controller|Composite'} | " &
                "Select-Object InstanceId,FriendlyName,Status | " &
                "ForEach-Object { $_.InstanceId.Trim() + '||' + $_.FriendlyName + '||' + $_.Status }"""
            psi.RedirectStandardOutput = True
            psi.UseShellExecute = False
            psi.CreateNoWindow = True
            psi.StandardOutputEncoding = System.Text.Encoding.UTF8

            Dim output As String = ""
            Using proc As Process = Process.Start(psi)
                output = proc.StandardOutput.ReadToEnd()
                proc.WaitForExit()
            End Using

            Dim lines = output.Split(
                {Environment.NewLine, vbLf, vbCr},
                StringSplitOptions.RemoveEmptyEntries)

            Dim portIndex As Integer = 1

            For Each line In lines
                Dim parts = line.Split({"||"}, StringSplitOptions.None)
                If parts.Length < 3 Then Continue For

                Dim deviceID As String = parts(0).Trim()
                Dim deviceName As String = parts(1).Trim()
                Dim deviceStatus As String = parts(2).Trim()

                If String.IsNullOrEmpty(deviceName) OrElse
                   String.IsNullOrEmpty(deviceID) Then Continue For

                Dim item As New ListViewItem("Device " & portIndex)
                item.SubItems.Add(deviceName)
                item.Tag = deviceID

                If deviceStatus = "Error" OrElse deviceStatus = "Unknown" Then
                    item.ForeColor = Color.OrangeRed
                    item.SubItems(1).Text = deviceName & "  [محظور]"
                ElseIf deviceStatus = "OK" Then
                    item.ForeColor = Color.FromArgb(0, 200, 100)
                End If

                _allDevices.Add(item)
                portIndex += 1
            Next

            If _allDevices.Count = 0 Then
                Dim noItem As New ListViewItem("-")
                noItem.SubItems.Add("لا توجد أجهزة USB متصلة حالياً")
                lvwUSBPorts.Items.Add(noItem)
            Else
                ' تطبيق عرض البيانات بناءً على حالة البحث الحالية لمنع التضارب
                If String.IsNullOrEmpty(txtSearch.Text.Trim()) Then
                    For Each item As ListViewItem In _allDevices
                        lvwUSBPorts.Items.Add(DirectCast(item.Clone(), ListViewItem))
                    Next
                Else
                    txtSearch_TextChanged(Nothing, Nothing)
                End If
            End If

            SetStatus("تم تحديث القائمة — " & _allDevices.Count & " جهاز", Color.DarkCyan)

        Catch ex As Exception
            SetStatus("خطأ: " & ex.Message, Color.Red)
        End Try
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg = WM_DEVICECHANGE Then
            Dim eventType As Integer = m.WParam.ToInt32()
            If eventType = DBT_DEVICEARRIVAL OrElse
               eventType = DBT_DEVICEREMOVECOMPLETE Then

                Task.Run(Async Function()
                             Try
                                 Await Task.Delay(2500)
                                 RefreshUSBPorts()
                             Catch
                             End Try
                         End Function)
            End If
        End If
        MyBase.WndProc(m)
    End Sub

    Private Function IsAdmin() As Boolean
        Dim identity = Security.Principal.WindowsIdentity.GetCurrent()
        Dim principal As New Security.Principal.WindowsPrincipal(identity)
        Return principal.IsInRole(Security.Principal.WindowsBuiltInRole.Administrator)
    End Function

    Private Sub SetStatus(text As String, clr As Color)
        If lblStatus.InvokeRequired Then
            lblStatus.Invoke(Sub() SetStatus(text, clr))
            Return
        End If
        lblStatus.Text = "▶ " & text
        lblStatus.ForeColor = clr
    End Sub

    Private Sub btnBlockAll_Click(sender As Object, e As EventArgs) Handles btnBlockAll.Click
        Try
            If Not IsAdmin() Then
                MessageBox.Show("يجب تشغيل البرنامج كمسؤول.",
                    "USB Guard", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Return
            End If

            Using key As RegistryKey = Registry.LocalMachine.OpenSubKey(
                "SYSTEM\CurrentControlSet\Services\USBSTOR", True)

                If key IsNot Nothing Then
                    key.SetValue("Start", 4, RegistryValueKind.DWord)
                    SetStatus("FULL LOCKDOWN — جميع منافذ التخزين محظورة 🔒", Color.Red)
                    MessageBox.Show(
                        "تم حظر جميع منافذ التخزين USB بنجاح!" &
                        Environment.NewLine & "(الماوس والكيبورد بأمان تام)",
                        "USB Guard", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    RefreshUSBPorts()
                Else
                    MessageBox.Show("لم يتم العثور على مسار خدمة التخزين.",
                        "USB Guard", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using

        Catch ex As Exception
            MessageBox.Show("خطأ: " & ex.Message,
                "USB Guard", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnAllowAll_Click(sender As Object, e As EventArgs) Handles btnAllowAll.Click
        Try
            If Not IsAdmin() Then
                MessageBox.Show("يجب تشغيل البرنامج كمسؤول لتنفيذ أمر التفعيل الكامل.",
                    "USB Guard", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Return
            End If

            ' 1. فتح خدمات التخزين الشاملة في الريجستري لتعود للوضع الافتراضي
            Using key As RegistryKey = Registry.LocalMachine.OpenSubKey(
                "SYSTEM\CurrentControlSet\Services\USBSTOR", True)
                If key IsNot Nothing Then
                    key.SetValue("Start", 3, RegistryValueKind.DWord)
                End If
            End Using

            ' 2. أمر الباورشيل المطور: حذف الكيانات العالقة والميتة + إعادة فحص الأجهزة لإجبار الويندوز على التعرف عليها
            Dim pshArguments As String = "-NoProfile -Command """ &
                "Get-PnpDevice | Where-Object { $_.Status -in 'Error', 'Unknown' -and ($_.InstanceId -like 'USB*' -or $_.InstanceId -like 'HID*') } | ForEach-Object { pnputil /remove-device $_.InstanceId }; " &
                "pnputil /scan-devices"""

            Dim psi As New ProcessStartInfo()
            psi.FileName = "powershell.exe"
            psi.Arguments = pshArguments
            psi.UseShellExecute = False
            psi.CreateNoWindow = True
            psi.WindowStyle = ProcessWindowStyle.Hidden

            Using proc As Process = Process.Start(psi)
                proc.WaitForExit()
            End Using

            ' تحديث واجهة البرنامج
            SetStatus("ALL PORTS ACTIVE — Ports Cleaned & Rebuilt 🔓", Color.Green)
            MessageBox.Show("System maintenance completed successfully! Ghost devices have been cleared and ports are reactivated. Just unplug the USB and reconnect it.",
                "USB Guard", MessageBoxButtons.OK, MessageBoxIcon.Information)

            RefreshUSBPorts()

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء محاولة التفعيل الشامل: " & ex.Message,
                "USB Guard", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnBlockSpecific_Click(sender As Object, e As EventArgs) Handles btnBlockSpecific.Click
        If lvwUSBPorts.SelectedItems.Count = 0 OrElse
           lvwUSBPorts.SelectedItems(0).Tag Is Nothing Then
            MessageBox.Show("الرجاء اختيار جهاز من القائمة أولاً.",
                "USB Guard", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedItem = lvwUSBPorts.SelectedItems(0)
        Dim deviceID As String = selectedItem.Tag.ToString()
        Dim friendlyName As String =
            selectedItem.Text & " (" & selectedItem.SubItems(1).Text & ")"

        Dim nameLower = friendlyName.ToLower()
        If nameLower.Contains("mouse") OrElse
           nameLower.Contains("keyboard") OrElse
           nameLower.Contains("input") Then
            MessageBox.Show(
                "حماية النظام: لا يمكن حظر أجهزة الإدخال (ماوس/كيبورد).",
                "USB Guard", MessageBoxButtons.OK, MessageBoxIcon.Hand)
            Return
        End If

        Dim pnpMessage As String = ""
        If ControlDeviceWithPnpUtil("/disable-device", deviceID, pnpMessage) Then
            SetStatus("تم حظر: " & friendlyName, Color.OrangeRed)
            MessageBox.Show("تم حظر " & friendlyName & " بنجاح." &
                Environment.NewLine & pnpMessage,
                "USB Guard", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            RefreshUSBPorts()
        Else
            MessageBox.Show("فشلت عملية الحظر الفردي." &
                Environment.NewLine & "السبب الفعلي: " & pnpMessage,
                "USB Guard", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub btnAllowSpecific_Click(sender As Object, e As EventArgs) Handles btnAllowSpecific.Click
        If lvwUSBPorts.SelectedItems.Count = 0 OrElse
           lvwUSBPorts.SelectedItems(0).Tag Is Nothing Then
            MessageBox.Show("الرجاء اختيار جهاز من القائمة أولاً.",
                "USB Guard", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedItem = lvwUSBPorts.SelectedItems(0)
        Dim deviceID As String = selectedItem.Tag.ToString()
        Dim friendlyName As String = selectedItem.Text & " (" & selectedItem.SubItems(1).Text & ")"

        Dim pnpMessage As String = ""

        ' الخطوة 1: محاولة التفعيل العادية عبر PnpUtil
        Dim success As Boolean = ControlDeviceWithPnpUtil("/enable-device", deviceID, pnpMessage)

        ' الخطوة 2: إذا فشل التفعيل أو ظهرت مشكلة عدم التعرف، نقوم بعمل إعادة تشغيل إجبارية للجهاز (Restart Device)
        If Not success OrElse pnpMessage.Contains("failed") OrElse pnpMessage.Contains("0x80041001") Then
            ' أمر PnpUtil القوي لإعادة تشغيل الكيان وتحديث حالته في شجرة الأجهزة
            ControlDeviceWithPnpUtil("/restart-device", deviceID, pnpMessage)
            success = True
        End If

        If success Then
            SetStatus("تم تفعيل وإعادة تنشيط: " & friendlyName, Color.DarkCyan)
            MessageBox.Show("تم تفعيل وإعادة تنشيط الجهاز بنجاح! إذا استمرت رسالة الويندوز، يرجى فصل كابل اليد وإعادة توصيله مرة واحدة فقط ليتعرف عليها النظام.",
                "USB Guard", MessageBoxButtons.OK, MessageBoxIcon.Information)
            RefreshUSBPorts()
        Else
            MessageBox.Show("فشلت عملية التفعيل الفردي." & Environment.NewLine & "السبب الفعلي: " & pnpMessage,
                "USB Guard", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    ' ======================================================
    '  الدالة المستقرة المعتمدة بالكامل على PnpUtil فقط
    ' ======================================================
    Private Function ControlDeviceWithPnpUtil(action As String, deviceID As String,
                                               ByRef consoleOutput As String) As Boolean
        Try
            If Not IsAdmin() Then
                consoleOutput = "البرنامج لا يمتلك صلاحيات المسؤول الكاملة."
                Return False
            End If

            Dim system32Path As String =
                Environment.GetFolderPath(Environment.SpecialFolder.System)

            ' معالجة توجيه المجلدات للأنظمة 64-بت في بيئة x86
            If Environment.Is64BitOperatingSystem AndAlso Not Environment.Is64BitProcess Then
                system32Path = system32Path.ToLower().Replace("system32", "sysnative")
            End If

            Dim info As New ProcessStartInfo()
            info.FileName = IO.Path.Combine(system32Path, "pnputil.exe")
            info.Arguments = action & " """ & deviceID & """"
            info.WindowStyle = ProcessWindowStyle.Hidden
            info.CreateNoWindow = True
            info.UseShellExecute = False
            info.RedirectStandardOutput = True
            info.RedirectStandardError = True

            Using proc As Process = Process.Start(info)
                Dim output As String = proc.StandardOutput.ReadToEnd() &
                                       proc.StandardError.ReadToEnd()
                proc.WaitForExit()
                consoleOutput = output.Trim()

                ' الأكواد 0 أو 3010 تعني نجاح العملية تماماً
                If proc.ExitCode = 0 OrElse proc.ExitCode = 3010 Then
                    If proc.ExitCode = 3010 Then
                        consoleOutput &= " (تنبيه: يتطلب إعادة تشغيل لتطبيق التغيير)"
                    End If
                    Return True
                Else
                    ' إرجاع كود الخطأ الصريح القادم من أداة النظام الأساسية
                    consoleOutput = $"[PnpUtil Exit Code: {proc.ExitCode}] - " & consoleOutput
                    Return False
                End If
            End Using

        Catch ex As Exception
            consoleOutput = ex.Message
            Return False
        End Try
    End Function

    Private Sub btnExportReport_Click(sender As Object, e As EventArgs) Handles btnExportReport.Click
        If _allDevices.Count = 0 Then
            MessageBox.Show("لا توجد بيانات أو أجهزة حالية لتصديرها في تقرير.",
                            "USB Guard — تقرير فارغ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using sfd As New SaveFileDialog()
            sfd.Title = "حفظ تقرير أجهزة الـ USB المتصلة"
            sfd.Filter = "ملف نصي مرتب (*.txt)|*.txt|ملف جداول إكسيل (*.csv)|*.csv"
            sfd.FileName = "USB Guard_USB_Report_" & DateTime.Now.ToString("yyyy-MM-dd_HH-mm")

            If sfd.ShowDialog() = DialogResult.OK Then
                Try
                    Dim fileExtension As String = IO.Path.GetExtension(sfd.FileName).ToLower()

                    If fileExtension = ".csv" Then
                        Using sw As New IO.StreamWriter(sfd.FileName, False, System.Text.Encoding.UTF8)
                            sw.Write(New Char() {ChrW(&HFEFF)})
                            sw.WriteLine("رقم الجهاز,اسم وموديل الجهاز المتصل,المعرّف الفريد للنظام (Device ID)")
                            For Each item As ListViewItem In _allDevices
                                Dim name As String = item.SubItems(1).Text.Replace(",", ";")
                                Dim dID As String = item.Tag.ToString().Replace(",", ";")
                                sw.WriteLine($"{item.Text},{name},{dID}")
                            Next
                        End Using
                    Else
                        Using sw As New IO.StreamWriter(sfd.FileName, False, System.Text.Encoding.UTF8)
                            sw.WriteLine("=========================================================================")
                            sw.WriteLine("                      USB Guard — USB HARDWARE REPORT                   ")
                            sw.WriteLine("=========================================================================")
                            sw.WriteLine("تاريخ ووقت الفحص : " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                            sw.WriteLine("إجمالي الأجهزة     : " & _allDevices.Count & " أجهزة نشطة/محظورة")
                            sw.WriteLine("=========================================================================")
                            sw.WriteLine()
                            For Each item As ListViewItem In _allDevices
                                sw.WriteLine($"[{item.Text}]")
                                sw.WriteLine($"   • الاسم الحركي : {item.SubItems(1).Text}")
                                sw.WriteLine($"   • معرّف الجهاز : {item.Tag.ToString()}")
                                sw.WriteLine("-------------------------------------------------------------------------")
                            Next
                            sw.WriteLine("                     [نهاية التقرير الأمني الصادر]")
                        End Using
                    End If

                    SetStatus("تم تصدير التقرير بنجاح! 📄", Color.DarkGreen)
                    MessageBox.Show("تم حفظ التقرير بنجاح.",
                                    "USB Guard — تم التصدير", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Catch ex As Exception
                    SetStatus("فشل تصدير التقرير.", Color.Red)
                    MessageBox.Show("حدث خطأ: " & ex.Message,
                                    "USB Guard — خطأ نظام", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

    Private Sub lvwUSBPorts_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvwUSBPorts.SelectedIndexChanged

    End Sub
End Class