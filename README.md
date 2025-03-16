# PBX API Control

PBX API Control is a simple example  gRPC API implementation for the 3CX PBX system written in C#.
---

## Configuration

### Additional Configuration File

```json
{
  "AllowedHosts": "*",
  "DefaultCulture": "en-US",
  "WhitelistedIPs": ["127.0.0.1"],
  "Kestrel": {
    "EndpointDefaults": {
      "Url": "http://127.0.0.1:2838",
      "Protocols": "Http2"
    }
  },
  "Jwt": {
    "Key": "0358a93d3fcf35a970bc7a62a3445be47d9443ac09852bc251b457ae4c7b3def",
    "Issuer": "pac",
    "Audience": "onvoip.ru"
  }
}
```

### PBXAPIConfig.cs Configuration Changes
If the configuration file path differs from the default, it must be adjusted depending on the operating system:

```csharp
if (OperatingSystem.IsWindows())
{
    return @"C:\ProgramData\3CX\Bin\3CXPhoneSystem.ini";
}
else if (OperatingSystem.IsLinux())
{
    return "/var/lib/3cxpbx/Bin/3CXPhoneSystem.ini";
}
```

---

## 📄 gRPC Protocols

Files in the `Protos/` folder define methods for managing:
- **Calls** (`call.proto`)
- **Contacts** (`contact.proto`)
- **Extensions** (`extension.proto`)
- **IVR** (`ivr.proto`)
- **Queues** (`queue.proto`)
- **Ring Groups** (`ring-group.proto`)
- **SQL Queries** (`sql.proto`)

---

## Installation

### 🖥️ Windows
1. Download and install [Microsoft .NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0).
2. Build the project:

```bash
dotnet build pbx-call-control.csproj
```

### 🐧 Linux (Debian 12)
1. Install .NET SDK 8.0:

```bash
wget https://packages.microsoft.com/config/debian/10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update \
    && sudo apt-get install -y apt-transport-https \
    && sudo apt-get update \
    && sudo apt-get install -y dotnet-sdk-8.0
```

2. **Before compiling**, edit the `WebAPICore.csproj` file:

Remove the line:
```xml
<Private>false</Private>
```

And modify the path:
```xml
<ItemGroup>
    <Reference Include="3cxpscomcpp2">
        <HintPath>/usr/lib/3cxpbx/3cxpscomcpp2.dll</HintPath>
        <Private>false</Private>
    </Reference>
</ItemGroup>
```

3. Run the following command to publish the project:

```bash
dotnet publish -c Release
```

---

## Authentication
To enable IP and token restriction:
1. Uncomment the following line in `Program.cs`:

```csharp
// app.UseMiddleware<AuthMiddleware>(); // Add authentication middleware
```

2. Obtain a token using the `GenerateToken` method.

