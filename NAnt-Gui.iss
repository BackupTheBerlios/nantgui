; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "NAnt-Gui"
#define MyAppPublisher "Colin Svingen"
#define MyAppURL "http://www.swoogan.com/nantgui.html"
#define MyAppExeName "NAnt-Gui.exe"

#define NAnt = "Tools\nant-0.85-rc3"
#define NAntContrib = "Tools\nantcontrib-0.85-rc3"
#define NUnit2Report = "Tools\NUnit2Report-1.2.3"

#define AppVersion = GetStringFileInfo("NAnt-Gui\bin\Release\NAnt-Gui.exe", "FileVersion")


[Setup]
AppName={#MyAppName}
AppVerName={#MyAppName} {#AppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=true
OutputDir=installer
OutputBaseFilename={#MyAppName}-{#AppVersion}
Compression=lzma
SolidCompression=true
ShowLanguageDialog=yes
ChangesAssociations=true
LicenseFile=License.txt
AppCopyright=2005 Colin Svingen
AppVersion={#AppVersion}
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
; ### Binary ###
; Documents
Source: License.txt; DestDir: {app}; Flags: ignoreversion
Source: Install.txt; DestDir: {app}; Flags: ignoreversion
Source: Todo.txt; DestDir: {app}; Flags: ignoreversion
Source: Bugs.txt; DestDir: {app}; Flags: ignoreversion
; NAnt-Gui
Source: NAnt-Gui\bin\Release\NAnt-Gui.exe; DestDir: {app}\bin; Flags: ignoreversion; Components: bin
Source: NAnt-Gui\bin\Release\NAnt-Gui.exe.config; DestDir: {app}\bin; Flags: ignoreversion; Components: bin
Source: NAnt-Gui\MainFormDocking.config; DestDir: {app}\bin; Flags: ignoreversion onlyifdoesntexist; Components: bin
Source: NAnt-Gui.Core\bin\Release\NAnt-Gui.Core.dll; DestDir: {app}\bin; Flags: ignoreversion; Components: bin
Source: NAnt-Gui.Framework\bin\Release\NAnt-Gui.Framework.dll; DestDir: {app}\bin; Flags: ignoreversion; Components: bin
Source: NAnt-Gui.NAnt\bin\Release\NAnt-Gui.NAnt.dll; DestDir: {app}\bin; Flags: ignoreversion; Components: bin

; Libraries
Source: ThirdParty Libraries\*; DestDir: {app}\bin; Flags: ignoreversion; Components: bin
; NAnt/NAnt-Contrib
Source: {#NAnt}\bin\*; DestDir: {app}\bin; Flags: ignoreversion recursesubdirs; Components: bin\nant
Source: {#NAnt}\examples\*; DestDir: {app}\examples; Flags: ignoreversion recursesubdirs; Components: examples
Source: {#NAnt}\doc\*; DestDir: {app}\nant-docs; Flags: ignoreversion recursesubdirs; Components: docs
Source: {#NAntContrib}\bin\*; DestDir: {app}\bin; Flags: ignoreversion recursesubdirs; Components: bin\contrib
Source: {#NAntContrib}\doc\*; DestDir: {app}\nantcontrib-docs; Flags: ignoreversion recursesubdirs; Components: docs
; NUnit2Report
Source: {#NUnit2Report}\*; DestDir: {app}\bin; Flags: ignoreversion recursesubdirs; Components: bin\nunit2report
; InnoSetupTask
Source: Nant.InnoSetup\bin\Release\NAnt.InnoSetupTasks.dll; DestDir: {app}\bin; Flags: ignoreversion; Components: bin\innosetup

; ### Source ###
Source: NAnt-Gui.sln; DestDir: {app}\src; Flags: ignoreversion; Components: src
Source: NAnt-Gui.build; DestDir: {app}\src; Flags: ignoreversion; Components: src
Source: Ant.ico; DestDir: {app}\src; Flags: ignoreversion; Components: src
Source: License.txt; DestDir: {app}\src; Flags: ignoreversion; Components: src
Source: Install.txt; DestDir: {app}\src; Flags: ignoreversion; Components: src
Source: Bugs.txt; DestDir: {app}\src; Flags: ignoreversion; Components: src
Source: Todo.txt; DestDir: {app}\src; Flags: ignoreversion; Components: src
Source: NAnt-Gui.iss; DestDir: {app}\src; Flags: ignoreversion; Components: src
Source: build_number.txt; DestDir: {app}\src; Flags: ignoreversion; Components: src
; Libraries
Source: ThirdParty Libraries\*; DestDir: {app}\src\ThirdParty Libraries; Flags: ignoreversion; Components: src
; NAnt-Gui
Source: NAnt-Gui\*.config; DestDir: {app}\src\Nant-Gui; Flags: ignoreversion; Components: src
Source: NAnt-Gui\*.cs; DestDir: {app}\src\Nant-Gui; Flags: ignoreversion; Components: src
Source: NAnt-Gui\*.csproj; DestDir: {app}\src\Nant-Gui; Flags: ignoreversion; Components: src
Source: NAnt-Gui\*.build; DestDir: {app}\src\Nant-Gui; Flags: ignoreversion; Components: src
; NAnt-Gui.Unittests
Source: NAnt-Gui.Unittests\*.cs; DestDir: {app}\src\Nant-Gui.Unittests; Flags: ignoreversion; Components: src
Source: NAnt-Gui.Unittests\*.resx; DestDir: {app}\src\Nant-Gui.Unittests; Flags: ignoreversion skipifsourcedoesntexist; Components: src
Source: NAnt-Gui.Unittests\*.csproj; DestDir: {app}\src\NAnt-Gui.Unittests; Flags: ignoreversion; Components: src
Source: NAnt-Gui.Unittests\*.build; DestDir: {app}\src\NAnt-Gui.Unittests; Flags: ignoreversion; Components: src
; NAnt-Gui.Core
Source: NAnt-Gui.Core\Images\*; DestDir: {app}\src\Nant-Gui.Core\Images; Flags: ignoreversion; Components: src
Source: NAnt-Gui.Core\*.cs; DestDir: {app}\src\Nant-Gui.Core; Flags: ignoreversion recursesubdirs; Components: src
Source: NAnt-Gui.Core\*.resx; DestDir: {app}\src\Nant-Gui.Core; Flags: ignoreversion skipifsourcedoesntexist recursesubdirs; Components: src
Source: NAnt-Gui.Core\*.csproj; DestDir: {app}\src\NAnt-Gui.Core; Flags: ignoreversion; Components: src
Source: NAnt-Gui.Core\*.build; DestDir: {app}\src\NAnt-Gui.Core; Flags: ignoreversion; Components: src
; NAnt-Gui.Framework
Source: NAnt-Gui.Framework\*.cs; DestDir: {app}\src\Nant-Gui.Framework; Flags: ignoreversion; Components: src
Source: NAnt-Gui.Framework\*.resx; DestDir: {app}\src\Nant-Gui.Framework; Flags: ignoreversion skipifsourcedoesntexist; Components: src
Source: NAnt-Gui.Framework\*.csproj; DestDir: {app}\src\NAnt-Gui.Framework; Flags: ignoreversion; Components: src
Source: NAnt-Gui.Framework\*.build; DestDir: {app}\src\NAnt-Gui.Framework; Flags: ignoreversion; Components: src
; NAnt-Gui.NAnt
Source: NAnt-Gui.NAnt\*.cs; DestDir: {app}\src\Nant-Gui.NAnt; Flags: ignoreversion; Components: src
Source: NAnt-Gui.NAnt\*.resx; DestDir: {app}\src\Nant-Gui.NAnt; Flags: ignoreversion skipifsourcedoesntexist; Components: src
Source: NAnt-Gui.NAnt\*.csproj; DestDir: {app}\src\NAnt-Gui.NAnt; Flags: ignoreversion; Components: src
Source: NAnt-Gui.NAnt\*.build; DestDir: {app}\src\NAnt-Gui.NAnt; Flags: ignoreversion; Components: src
; NAnt-Gui.MSBuild
Source: NAnt-Gui.MSBuild\*.cs; DestDir: {app}\src\Nant-Gui.MSBuild; Flags: ignoreversion; Components: src
Source: NAnt-Gui.MSBuild\*.resx; DestDir: {app}\src\Nant-Gui.MSBuild; Flags: ignoreversion skipifsourcedoesntexist; Components: src
Source: NAnt-Gui.MSBuild\*.csproj; DestDir: {app}\src\NAnt-Gui.MSBuild; Flags: ignoreversion; Components: src
Source: NAnt-Gui.MSBuild\*.build; DestDir: {app}\src\NAnt-Gui.MSBuild; Flags: ignoreversion; Components: src
; NAnt.InnoSetup.Tasks
Source: NAnt.InnoSetup\*.cs; DestDir: {app}\src\NAnt.InnoSetup; Flags: ignoreversion; Components: src
Source: NAnt.InnoSetup\*.csproj; DestDir: {app}\src\NAnt.InnoSetup; Flags: ignoreversion; Components: src
Source: NAnt.InnoSetup\Sample.iss; DestDir: {app}\src\NAnt.InnoSetup; Flags: ignoreversion skipifsourcedoesntexist; Components: src
Source: NAnt.InnoSetup\Sample.xml; DestDir: {app}\src\NAnt.InnoSetup; Flags: ignoreversion; Components: src
Source: NAnt.InnoSetup\Tasks\*.cs; DestDir: {app}\src\NAnt.InnoSetup\Tasks; Flags: ignoreversion; Components: src
; Tools
Source: Tools\*; DestDir: {app}\src\Tools; Flags: ignoreversion recursesubdirs; Components: src

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
Name: bin\innosetup; Description: NAnt InnoSetup Task; Types: full
Name: bin\nunit2report; Description: NAnt NUnit2 Report Task; Types: full
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
