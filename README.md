# SAPConsoleApp

Pre-Reqs:

 - You will need a copy of the SAP Connector for Microsoft.NET from SAP(Requires an S-User account).
 - Create a `PrivateSettings.config` file within the SAPConsoleApp folder and define your custom values like the ones below.

PrivateSettings.config:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<appSettings>
  <add key="AppServerHost" value="sap.example.com"/>
  <add key="SystemNumber" value="XX"/>
  <add key="Client" value="XXX"/>
  <add key="SystemID" value="XXX"/>
  <add key="User" value="username"/>
  <add key="Password" value="password"/>
  <add key="SncQOP" value="X"/>
  <add key="SncMode" value="X"/>
  <add key="SncPartnerName" value="X"/>
  <add key="SncMyName" value="X"/>
  <add key="SncLibraryPath" value="C:\\sap\\libs\\sapcryptolib\\sapcrypto.dll"/>
</appSettings>
```