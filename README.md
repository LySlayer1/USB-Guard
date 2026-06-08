# USB-Guard
USB Guard is a high-privilege Windows endpoint security utility built in VB.NET for real-time USB monitoring and restriction. It mitigates risks from data exfiltration and hardware attacks (like BadUSB) through real-time subsystem interception, global registry lockdowns, and granular driver-level hardware isolation.

Technical Sophistication:
* **Asynchronous Hardware Auditing:** Implements asynchronous execution processes (`Task.Run`) to interface with the Windows PnP subsystem via PowerShell (`Get-PnpDevice`), gathering granular hardware intelligence without causing UI thread blocking.
* **Low-Level Subsystem Monitoring:** Intercepts system-level hardware state changes instantaneously by handling OS-native window messages (`WM_DEVICECHANGE`).
* **Dual-Layer Mitigation Matrix:**
    * **Global Enforcement:** Hardens the operating system instantly by modifying core registry components (`USBSTOR` driver start parameters) to halt mass storage provisioning while preserving active HIDs (Mice/Keyboards).
    * **Granular Driver Isolation:** Leverages the Windows Deployment Image Servicing engine (`pnputil.exe`) via elevated 64-bit environment mapping (`sysnative`) to disable specific untrusted hardware threads using their unique Hardware Instance IDs.
* **Forensic Compliance Engine:** Features a built-in auditing module that generates structured forensic logs exported into `.txt` formats or standard `.csv` spreadsheets with UTF-8 BOM integration for immediate enterprise reporting.
* 
