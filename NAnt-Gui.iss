; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "NAnt-Gui"
#define MyAppVerName "NAnt-Gui 1.3.2"
#define MyAppPublisher "Colin Svingen"
#define MyAppURL "http://www.swoogan.com/nantgui.html"
#define MyAppExeName "NAnt-Gui.exe"

#define NAntDir = "nant-0.85-rc3"
#define NAntContribDir = "nantcontrib-0.85-rc3"

[Setup]
AppName={#MyAppName}
AppVerName={#MyAppVerName}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=true
OutputDir=installer
OutputBaseFilename=NAnt-Gui-1.3.2
Compression=lzma
SolidCompression=true
ShowLanguageDialog=yes
ChangesAssociations=true
LicenseFile=License.txt
AppCopyright=2005 Colin Svingen
AppVersion=1.3.2
AppID={{5A46EB86-CC66-403A-9789-E7D7413C20D2}
AppContact=nantgui@swoogan.com
UninstallDisplayIcon={app}\src\Ant.ico

[Tasks]
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons}
Name: quicklaunchicon; Description: {cm:CreateQuickLaunchIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: unchecked
Name: assocbuild; GroupDescription: Associate with file types; Description: Associate with .build files
Name: assocnant; GroupDescription: Associate with file types; Description: Associate with .nant files; Flags: unchecked
Name: envpath; GroupDescription: Set environment variables; Description: Add bin directory to path

[Files]
; Binary
Source: NAnt-Gui\bin\Release\NAnt-Gui.exe; DestDir: {app}\bin; Flags: ignoreversion; Components: bin
Source: NAnt-Gui\bin\Release\NAnt-Gui.exe.config; DestDir: {app}\bin; Flags: ignoreversion; Components: bin
Source: NAnt-Gui.Core\bin\Release\NAnt-Gui.Core.dll; DestDir: {app}\bin; Flags: ignoreversion; Components: bin
; Libraries
Source: ThirdParty Libraries\*; DestDir: {app}\bin; Flags: ignoreversion; Components: bin

; Source
Source: NAnt-Gui.sln; DestDir: {app}\src; Flags: ignoreversion; Components: src
Source: NAnt-Gui.build; DestDir: {app}\src; Flags: ignoreversion; Components: src
Source: Ant.ico; DestDir: {app}\src; Flags: ignoreversion; Components: src
Source: License.txt; DestDir: {app}; Flags: ignoreversion
Source: License.txt; DestDir: {app}\src; Flags: ignoreversion; Components: src
Source: Bugs.txt; DestDir: {app}; Flags: ignoreversion
Source: Bugs.txt; DestDir: {app}\src; Flags: ignoreversion; Components: src
Source: Todo.txt; DestDir: {app}; Flags: ignoreversion
Source: Todo.txt; DestDir: {app}\src; Flags: ignoreversion; Components: src
Source: NAnt-Gui.iss; DestDir: {app}\src; Flags: ignoreversion; Components: src
Source: ThirdParty Libraries\*; DestDir: {app}\src\ThirdParty Libraries; Flags: ignoreversion; Components: src
; NAnt-Gui.Unittests
Source: NAnt-Gui.Unittests\*.cs; DestDir: {app}\src\Nant-Gui.Unittests; Flags: ignoreversion; Components: src
Source: NAnt-Gui.Unittests\*.resx; DestDir: {app}\src\Nant-Gui.Unittests; Flags: ignoreversion skipifsourcedoesntexist; Components: src
Source: NAnt-Gui.Unittests\Nant-Gui.Unittests.csproj; DestDir: {app}\src\NAnt-Gui.Unittests; Flags: ignoreversion; Components: src
; NAnt-Gui.Core
Source: NAnt-Gui.Core\Images\*; DestDir: {app}\src\Nant-Gui.Core\Images; Flags: ignoreversion; Components: src
Source: NAnt-Gui.Core\*.cs; DestDir: {app}\src\Nant-Gui.Core; Flags: ignoreversion; Components: src
Source: NAnt-Gui.Core\*.resx; DestDir: {app}\src\Nant-Gui.Core; Flags: ignoreversion; Components: src
Source: NAnt-Gui.Core\NAnt\*.cs; DestDir: {app}\src\Nant-Gui.Core\NAnt; Flags: ignoreversion; Components: src
Source: NAnt-Gui.Core\NAnt\*.resx; DestDir: {app}\src\Nant-Gui.Core\NAnt; Flags: ignoreversion skipifsourcedoesntexist; Components: src
Source: NAnt-Gui.Core\Nant-Gui.Core.csproj; DestDir: {app}\src\NAnt-Gui.Core; Flags: ignoreversion; Components: src
; NAnt-Gui
Source: NAnt-Gui\app.config; DestDir: {app}\src\Nant-Gui; Flags: ignoreversion; Components: src
Source: NAnt-Gui\*.cs; DestDir: {app}\src\Nant-Gui; Flags: ignoreversion; Components: src
Source: NAnt-Gui\NAnt-Gui.csproj; DestDir: {app}\src\Nant-Gui; Flags: ignoreversion; Components: src
; NAnt/NAnt-Contrib
Source: C:\Program Files\{#NAntDir}\bin\*; DestDir: {app}\bin; Flags: ignoreversion recursesubdirs; Components: bin\nant
Source: C:\Program Files\{#NAntDir}\examples\*; DestDir: {app}\examples; Flags: ignoreversion recursesubdirs; Components: examples
Source: C:\Program Files\{#NAntDir}\doc\*; DestDir: {app}\nant-docs; Flags: ignoreversion recursesubdirs; Components: docs
Source: C:\Program Files\{#NAntContribDir}\bin\*; DestDir: {app}\bin; Flags: ignoreversion recursesubdirs; Components: bin\contrib
Source: C:\Program Files\{#NAntContribDir}\doc\*; DestDir: {app}\nantcontrib-docs; Flags: ignoreversion recursesubdirs; Components: docs

; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: {group}\{#MyAppName}; Filename: {app}\bin\{#MyAppExeName}
Name: {userdesktop}\{#MyAppName}; Filename: {app}\bin\{#MyAppExeName}; Tasks: desktopicon
Name: {userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}; Filename: {app}\bin\{#MyAppExeName}; Tasks: quicklaunchicon
Name: {group}\Documentation\NAnt-SDK Help; Filename: {app}\nant-docs\sdk\NAnt-SDK.chm; Components: docs
Name: {group}\Documentation\NAnt Documentation; Filename: {app}\nant-docs\help\index.html; Components: docs
Name: {group}\Documentation\NAnt Contrib Docs; Filename: {app}\nantcontrib-docs\help\index.html; Components: docs
Name: {group}\{cm:UninstallProgram, {#MyAppName}}; Filename: {uninstallexe}

[Run]
Filename: {app}\bin\{#MyAppExeName}; Description: {cm:LaunchProgram,{#MyAppName}}; Flags: nowait postinstall skipifsilent

[Dirs]
Name: {app}\src; Components: src
Name: {app}\bin; Components: bin

[Registry]
Root: HKCR; Subkey: .build; ValueType: string; ValueName: ; ValueData: NAntBuildFile; Flags: uninsdeletekey; Tasks: assocbuild
Root: HKCR; Subkey: NAntBuildFile; ValueType: string; ValueName: ; ValueData: NAnt Build File; Flags: uninsdeletekey; Tasks: assocbuild
Root: HKCR; Subkey: NAntBuildFile\DefaultIcon; ValueType: string; ValueName: ; ValueData: {app}\bin\{#MyAppExeName},0; Flags: uninsdeletevalue; Tasks: assocbuild
Root: HKCR; Subkey: NAntBuildFile\shell\Open\command; ValueType: string; ValueData: """{app}\bin\{#MyAppExeName}"" -f:""%1"""; Flags: uninsdeletevalue; Tasks: assocbuild
Root: HKCR; Subkey: NAntBuildFile\shell\Edit\command; ValueType: string; ValueData: """{sys}\notepad.exe"" ""%1"""; Flags: uninsdeletevalue; Tasks: assocbuild

Root: HKCR; Subkey: .nant; ValueType: string; ValueName: ; ValueData: NAntNAntFile; Flags: uninsdeletekey; Tasks: assocnant
Root: HKCR; Subkey: NAntNAntFile; ValueType: string; ValueName: ; ValueData: NAnt Build File; Flags: uninsdeletekey; Tasks: assocnant
Root: HKCR; Subkey: NAntNAntFile\DefaultIcon; ValueType: string; ValueName: ; ValueData: {app}\bin\{#MyAppExeName},0; Flags: uninsdeletevalue; Tasks: assocnant
Root: HKCR; Subkey: NAntNAntFile\shell\Open\command; ValueType: string; ValueData: """{app}\bin\{#MyAppExeName}"" -f:""%1"""; Flags: uninsdeletevalue; Tasks: assocnant
Root: HKCR; Subkey: NAntNAntFile\shell\Edit\command; ValueType: string; ValueData: """{sys}\notepad.exe"" ""%1"""; Tasks: assocnant

Root: HKLM; Subkey: SYSTEM\CurrentControlSet\Control\Session Manager\Environment; ValueType: string; ValueName: Path; ValueData: "{code:GetENVPath};{app}\bin"; Tasks: envpath; Check: IfNotInPath

[Components]
Name: bin; Description: Executable files; Types: custom compact full
Name: bin\nant; Description: NAnt 0.85-rc3; Types: custom compact full
Name: bin\contrib; Description: NAnt Contrib 0.85-rc3; Types: full custom compact
Name: examples; Description: Example NAnt build files; Types: full
Name: docs; Description: NAnt documentation files; Types: custom full
Name: src; Description: Source code files; Types: full

[Code]
function GetENVPath(Param: string) : string;
var
	Path: string;
begin
	if RegQueryStringValue(HKLM, 'SYSTEM\CurrentControlSet\Control\Session Manager\Environment', 'Path', Path) then
		Result := Path
	else
		Result := '';
end;

function IfNotInPath() : Boolean;
var
	EnvPath: string;
	Path: string;
begin
	RegQueryStringValue(HKLM, 'SYSTEM\CurrentControlSet\Control\Session Manager\Environment', 'Path', EnvPath);
	Path := ExpandConstant('{app}') + '\bin';
	Result := Pos(Path, EnvPath) = 0;
end;
