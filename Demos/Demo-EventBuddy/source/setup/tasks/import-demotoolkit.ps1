# Import Demo Toolkit Module
[string] $myDocumentsFolder = [Environment]::GetFolderPath(“MyDocuments”)
if (-NOT (test-path "$myDocumentsFolder"))
{
	$myDocumentsFolder = "$env:UserProfile\Documents";
}
Import-Module "$myDocumentsFolder\WindowsPowerShell\Modules\DemoToolkit\DemoToolkit.psd1" -WarningAction SilentlyContinue
