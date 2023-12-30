
PS C:\repos\Band Tree> dotnet new webapi -n BandTree.Server
The template "ASP.NET Core Web API" was created successfully.

Processing post-creation actions...
Restoring C:\repos\Band Tree\BandTree.Server\BandTree.Server.csproj:
  Determining projects to restore...
  Restored C:\repos\Band Tree\BandTree.Server\BandTree.Server.csproj (in 478 ms).
Restore succeeded.


PS C:\repos\Band Tree> dir


    Directory: C:\repos\Band Tree


Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
d-----         9/27/2023   8:24 PM                BandTree.Server
-a----         9/27/2023   8:23 PM           1404 Band Tree Doc - Shortcut.lnk


PS C:\repos\Band Tree> cd .\BandTree.Server\

PS C:\repos\Band Tree\BandTree.Server> dotnet add package EdgeDB.Net.Driver -Source https://www.myget.org/F/edgedb-net/api/v3/index.json
Unrecognized command or argument '-Source'
Unrecognized command or argument 'https://www.myget.org/F/edgedb-net/api/v3/index.json'
Description:
  Add a NuGet package reference to the project.

Usage:
  dotnet add [<PROJECT>] package <PACKAGE_NAME> [options]

Arguments:
  <PROJECT>       The project file to operate on. If a file is not specified, the command will search the current
                  directory for one. [default: C:\repos\Band Tree\BandTree.Server\]
  <PACKAGE_NAME>  The package reference to add.

Options:
  -v, --version <VERSION>            The version of the package to add.
  -f, --framework <FRAMEWORK>        Add the reference only when targeting a specific framework.
  -n, --no-restore                   Add the reference without performing restore preview and compatibility check.
  -s, --source <SOURCE>              The NuGet package source to use during the restore.
  --package-directory <PACKAGE_DIR>  The directory to restore packages to.
  --interactive                      Allows the command to stop and wait for user input or action (for example to
                                     complete authentication).
  --prerelease                       Allows prerelease packages to be installed.
  -?, -h, --help                     Show command line help.


PS C:\repos\Band Tree\BandTree.Server> dotnet add package EdgeDB.Net.Driver --source https://www.myget.org/F/edgedb-net/api/v3/index.json
  Determining projects to restore...
  Writing C:\Users\John Comienzo\AppData\Local\Temp\tmpC56C.tmp
info : X.509 certificate chain validation will use the default trust store selected by .NET for code signing.
info : X.509 certificate chain validation will use the default trust store selected by .NET for timestamping.
info : Adding PackageReference for package 'EdgeDB.Net.Driver' into project 'C:\repos\Band Tree\BandTree.Server\BandTree.Server.csproj'.
info :   GET https://www.myget.org/F/edgedb-net/api/v3/registration1/edgedb.net.driver/index.json
info :   OK https://www.myget.org/F/edgedb-net/api/v3/registration1/edgedb.net.driver/index.json 428ms
info : Restoring packages for C:\repos\Band Tree\BandTree.Server\BandTree.Server.csproj...
info : Package 'EdgeDB.Net.Driver' is compatible with all the specified frameworks in project 'C:\repos\Band Tree\BandTree.Server\BandTree.Server.csproj'.
info : PackageReference for package 'EdgeDB.Net.Driver' version '1.2.3' added to file 'C:\repos\Band Tree\BandTree.Server\BandTree.Server.csproj'.
info : Writing assets file to disk. Path: C:\repos\Band Tree\BandTree.Server\obj\project.assets.json
log  : Restored C:\repos\Band Tree\BandTree.Server\BandTree.Server.csproj (in 104 ms).
PS C:\repos\Band Tree\BandTree.Server> dir


    Directory: C:\repos\Band Tree\BandTree.Server


Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
d-----         9/27/2023   8:24 PM                Controllers                                                           d-----         9/27/2023   8:26 PM                obj                                                                   d-----         9/27/2023   8:24 PM                Properties                                                            -a----         9/27/2023   8:24 PM            127 appsettings.Development.json
-a----         9/27/2023   8:24 PM            151 appsettings.json
-a----         9/27/2023   8:26 PM            479 BandTree.Server.csproj
-a----         9/27/2023   8:24 PM            557 Program.cs
-a----         9/27/2023   8:24 PM            264 WeatherForecast.cs


PS C:\repos\Band Tree\BandTree.Server> cat .\BandTree.Server.csproj
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net7.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="EdgeDB.Net.Driver" Version="1.2.3" />
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="7.0.10" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
  </ItemGroup>

</Project>
PS C:\repos\Band Tree\BandTree.Server> iwr https://ps1.edgedb.com -useb | iex
Downloading installer...

Welcome to EdgeDB!

This will install the official EdgeDB command-line tools.

The edgedb binary will be placed in the user bin directory located at:
C:\Users\John Comienzo\AppData\Roaming\edgedb\bin

┌──────────────────────┬───────────────────────────────────────────────────┐
│ Installation Path    │ C:\Users\John Comienzo\AppData\Roaming\edgedb\bin │
│ Modify PATH Variable │ no                                                │
└──────────────────────┴───────────────────────────────────────────────────┘
1) Proceed with installation (default)
2) Customize installation
3) Cancel installation
1
The EdgeDB command-line tool is now installed!
To initialize a new project, run:
edgedb project init
PS C:\repos\Band Tree\BandTree.Server> edgedb project init
No `edgedb.toml` found in `\\?\C:\repos\Band Tree\BandTree.Server` or above
Do you want to initialize a new project? [Y/n]
> Y
Specify the name of EdgeDB instance to use with this project [default: BandTree]:
> BandTree
Checking EdgeDB versions...
Timeout expired to fetch https://packages.edgedb.com/archive/.jsonindexes/x86_64-unknown-linux-gnu.json. Common reasons are:
  1. Internet connectivity is slow
  2. A firewall is blocking internet access to this resource
Specify the version of EdgeDB to use with this project [default: 3.4]:
> 3.4
┌─────────────────────┬────────────────────────────────────────────────────┐
│ Project directory   │ \\?\C:\repos\Band Tree\BandTree.Server             │
│ Project config      │ \\?\C:\repos\Band Tree\BandTree.Server\edgedb.toml │
│ Schema dir (empty)  │ \\?\C:\repos\Band Tree\BandTree.Server\dbschema    │
│ Installation method │ WSL                                                │
│ Version             │ 3.4+301ba34                                        │
│ Instance name       │ BandTree                                           │
└─────────────────────┴────────────────────────────────────────────────────┘
Downloading package...
00:00:02 [====================] 39.00 MiB/39.00 MiB 15.85 MiB/s | ETA: 0s
Successfully installed 3.4+301ba34
Initializing EdgeDB instance...
[edgedb] CRITICAL 12233 2023-09-27T07:17:33.522 postgres: the database system is starting up
Applying migrations...
Everything is up to date. Revision initial
Project initialized.
To connect to BandTree, run `edgedb`
PS C:\repos\Band Tree\BandTree.Server> edgedb ui
[wsl] base
[wsl] edbjwskeys.pem
[wsl] edbtlscert.pem
[wsl] edbprivkey.pem
[wsl] pg_multixact
[wsl] instance_info.json
[wsl] pg_stat
[wsl] pg_dynshmem
[wsl] PG_VERSION
[wsl] postgresql.conf
[wsl] pg_logical
[wsl] global
[wsl] pg_notify
[wsl] pg_replslot
[wsl] pg_serial
[wsl] pg_snapshots
[wsl] pg_commit_ts
[wsl] pg_stat_tmp
[wsl] pg_subtrans
[wsl] pg_tblspc
[wsl] pg_twophase
[wsl] pg_wal
[wsl] pg_hba.conf
[wsl] pg_xact
[wsl] postgresql.auto.conf
[wsl] pg_ident.conf
[wsl] postmaster.opts
[wsl] postmaster.pid
Opening URL in browser:
http://localhost:10702/ui?authToken=edbt_eyJ0eXAiOiJKV1QiLCJhbGciOiJFUzI1NiJ9.eyJlZGdlZGIuc2VydmVyLmFueV9yb2xlIjp0cnVlfQ.GfeO-dp8BQwrlx_D36wMCjIjf61oXMVectgYur6D8ITwg6G3vLcHp--jqPYvv5UBBR0Ew5ir8npR3eYFdsgD1A
PS C:\repos\Band Tree\BandTree.Server>

