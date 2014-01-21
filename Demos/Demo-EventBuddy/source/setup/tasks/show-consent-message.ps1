param([switch]$ResetAzure,[switch]$ResetLocal,[switch]$SetupAzure,[switch]$SetupLocal,[switch]$CleanupLocal,[switch]$CleanupAzure)

if ($SetupLocal.IsPresent) {
    Write-Host "This demo does not require any setup step in your local machine" -ForegroundColor Green
    Write-Host ""
}

if ($ResetLocal.IsPresent) {
    Write-Warning "This script will setup your machine by performing the following tasks:"
    Write-Host ""
	Write-Host "- Install Demo Toolkit"
	Write-Host "- Close existing instances of Visual Studio"
	Write-Host "- Close existing instances of Internet Explorer"
	Write-Host "- Remove current working directory if exists"
	Write-Host "- Create new working directory (as specified in Config.Local.xml)"
	Write-Host "- Copy assets code to working directory"
	Write-Host "- Update Mobile Services settings in the Windows Phone 8 application"
	Write-Host "- Uninstall Windows 8 application if installed"
	Write-Host "- Build Windows 8 application"
	Write-Host "- Install Windows 8 application"
	Write-Host "- Install Code Snippets for the Windows 8 application"
	Write-Host "- Install Internet Explorer snippets in the favorites bar"
	Write-Host "- Copy Visual Studio .suo to Begin solution"
	Write-Host "- Start Visual Studio with the Windows 8 application (if using Professional or higher edition)"
	Write-Host "- Start Visual Studio with the Windows Phone 8 application (if using Windows Phone Emulator with Professional or higher edition of Visual Studio)"
	Write-Host "- Open Internet Explorer logged in the Windows Azure Management Portal"
}

if ($CleanupLocal.IsPresent) {
    Write-Host "This demo does not require any cleanup step in your local machine" -ForegroundColor Green
    Write-Host ""
}

if ($SetupAzure.IsPresent) {
    Write-Host "This demo does not require any setup step in Windows Azure" -ForegroundColor Green
    Write-Host ""
}

if ($ResetAzure.IsPresent) {
    Write-Warning "This script will setup Windows Azure by performing the following tasks:"
    Write-Host ""
	Write-Host "- Install Demo Toolkit"
	Write-Host "- Clean up Channel table"
}

if ($CleanupAzure.IsPresent) {
    Write-Host "This demo does not require any cleanup step in Windows Azure" -ForegroundColor Green
    Write-Host ""
}

Write-Host ""

$title = ""
$message = "Are you sure you want to continue?"

$yes = New-Object System.Management.Automation.Host.ChoiceDescription "&Yes"
$no = New-Object System.Management.Automation.Host.ChoiceDescription "&No"
$options = [System.Management.Automation.Host.ChoiceDescription[]]($yes, $no)
$confirmation = $host.ui.PromptForChoice($title, $message, $options, 1)

if ($confirmation -eq 0) {
    exit 0
}
else {
    exit 1
}


