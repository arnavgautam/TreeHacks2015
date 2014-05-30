function Get-ScriptDirectory {
$Invocation = (Get-Variable MyInvocation -Scope 1).Value
Split-Path $Invocation.MyCommand.Path
}

$FunctionToRegister = Join-Path (Get-ScriptDirectory) "setup\Invoke-AzureEnvironmentSetup.ps1"
. "$FunctionToRegister"

#### Primary Environment
$EnvironmentSubscriptionName = "BUILD Web Workload 1 -- Cory | Jon"
$EnvironmentPrimaryLocation = "West US"
$StorageEnvironmentLocation = "West US"
$EnvironmentWebSites = @{"clipmeme"=$EnvironmentPrimaryLocation}
$EnvironmentStagingSites = @{"clipmeme"=$EnvironmentPrimaryLocation}
$EnvironmentStorageAccount = "clipmemestorage"
$StorageContainers = @('uploads','memes')

$PublishSettingsFile = Join-Path (Get-ScriptDirectory) "setup\assets\publishSettings\primary.publishsettings"

Invoke-AzureEnvironmentSetup -EnvironmentSubscriptionName $EnvironmentSubscriptionName `
                             -EnvironmentPrimaryLocation $EnvironmentPrimaryLocation `
                             -StorageEnvironmentLocation $StorageEnvironmentLocation `
                             -EnvironmentWebSites $EnvironmentWebSites `
                             -EnvironmentStagingSites $EnvironmentStagingSites `
                             -EnvironmentStorageAccount $EnvironmentStorageAccount `
                             -StorageContainers $StorageContainers `
							 -PublishSettingsFile $PublishSettingsFile