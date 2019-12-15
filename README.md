# SessionMonitor Windows Service
Tool for finding out who's logged into a Windows server

## Install SessionMonitor Windows Service onto a server via Powershell
 - Upgrade Powershell to a version that supports necessary Cmdlets:
   ```Powershell
   PS iex "& { $(irm https://aka.ms/install-powershell.ps1) } -UseMSI"
   ```
 - Install service:
   ```Powershell
   PS New-Service -Name "SessionMonitor" -BinaryPathName "C:\CodeDeploy\SessionMonitor.WindowsSvc\SessionMonitor.WindowsSvc.exe"
   ```
 - Uninstall service:
   ```Powershell
   PS Remove-Service -Name "SessionMonitor"
   ```

## Control SessionMonitor Windows Service on a remote server via CMD
 - Connect to remote server:
   ```
   C:\> net use \\WIN-ADGLFHBJRGG P4ssw0rd /user:WIN-ADGLFHBJRGG\administrator`
   ```
 - See if service is running on remote server:
   ```
   C:\> SC \\WIN-ADGLFHBJRGG Query SessionMonitor
   ```
 - Stop service on remote server:
   ```
   C:\> SC \\WIN-ADGLFHBJRGG Stop SessionMonitor
   ```
 - Start service on remote server:
   ```
   C:\> SC \\WIN-ADGLFHBJRGG Start SessionMonitor
   ```

Admin2
UserX2019
