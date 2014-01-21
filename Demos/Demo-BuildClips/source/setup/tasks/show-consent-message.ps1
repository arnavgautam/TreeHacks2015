param([switch]$ResetAzure,[switch]$ResetLocal,[switch]$SetupAzure,[switch]$SetupLocal,[switch]$CleanupLocal,[switch]$CleanupAzure,[switch]$SetupDeployment,[switch]$CleanupDeployment)

if ($SetupLocal.IsPresent) {
    Write-Warning "This script will setup your machine by performing the following tasks:"
    Write-Host ""
    Write-Host "1. Run the Dependency Checker"
    Write-Host "2. Set powershell execution policy to unrestricted"
    Write-Host "3. Copy the Begin solution to the working directory"
    Write-Host "4. Copy the Begin solution of Segment 2 to the working directory"
    Write-Host "5. Copy the Begin solution of Segment 5 to the working directory"
    Write-Host "6. Copy assets code to the working directory"
    Write-Host "7. Update the Web config settings for the Begin solution"
    Write-Host "8. Update the Web config settings for the Begin solution for Segment 2"
    Write-Host "9. Update the App config settings for the Begin solution"
    Write-Host "10. Reset Azure Compute Emulator and Dev Storage" -ForegroundColor Yellow
    Write-Host "11. Configure IIS Express Web Site"
    Write-Host "12. Upload sample videos to the local Web Site"
    Write-Host "13. Install the code snippets for the demo"
    Write-Host "14. Open Manual Reset steps file"
    Write-Host ""
}

if ($ResetLocal.IsPresent) {
    Write-Warning "This script will reset your machine by performing the following tasks:"
    Write-Host ""
    Write-Host "1. Remove the working directory for the demo"
    Write-Host "2. Remove the code snippets for the demo"
    Write-Host "3. Drop local database"
    Write-Host "4. Remove IIS Express Web Site"
    Write-Host "5. Run the Dependency Checker"
    Write-Host "6. Set powershell execution policy to unrestricted"
    Write-Host "7. Copy the Begin solution to the working directory"
    Write-Host "8. Copy the Begin solution of Segment 2 to the working directory"
    Write-Host "9. Copy assets code to the working directory"
    Write-Host "10. Update the Web config settings for the Begin solution"
    Write-Host "11. Update the Web config settings for the Begin solution for Segment 2"
    Write-Host "12. Update the App config settings for the Begin solution"
    Write-Host "13. Reset Azure Compute Emulator and Dev Storage" -ForegroundColor Yellow
    Write-Host "14. Configure the IIS Express Web Site"
    Write-Host "15. Upload sample videos to the local Web Site"
    Write-Host "16. Install the code snippets for the demo"
    Write-Host "17. Open Manual Reset steps file"
    Write-Host ""
}

if ($CleanupLocal.IsPresent) {
    Write-Warning "This script will cleanup your machine by performing the following tasks:"
    Write-Host ""
    Write-Host "1. Remove the working directory for the demo"
    Write-Host "2. Remove the code snippets for the demo"
    Write-Host "3. Drop the local database"
    Write-Host "4. Remove the IIS Express Web Site"
    Write-Host ""
}

if ($SetupAzure.IsPresent) {
    Write-Warning "This script will set up your Azure account by performing the following tasks:"
    Write-Host ""
    #TBC
}

if ($ResetAzure.IsPresent) {
    Write-Warning "This script will reset your Azure account by performing the following tasks:"
    Write-Host ""
    Write-Host "1. Remove the SQL server associated to the Web site (created during the demo)" -ForegroundColor Red
    Write-Host ""
}

if ($CleanupAzure.IsPresent) {
    Write-Warning "This script will clean up your Azure account by performing the following tasks:"
    Write-Host ""
    Write-Host "1. Remove the SQL server associated to the Web site (created during the demo)" -ForegroundColor Red
    Write-Host ""
}

if ($SetupDeployment.IsPresent) {
    Write-Warning "This script will setup the deployment for Segment 5 by performing the following tasks:"
    Write-Host ""
    Write-Host "1. Create a storage account for diagnostics."
    Write-Host "2. Create a SQL server and a SQL database in that server."
    Write-Host "3. Add a SQL server firewall rule to accept all the ip range."
    Write-Host "4. Create a cloud service."
    Write-Host "5. Copy Begin solution for Segment 5 to working directory"
    Write-Host "6. Update Web.config for solution of Segment 5"
    Write-Host "7. Update App.config for solution of Segment 5"
    Write-Host "8. Update Web.Release.config for solution of Segment 5"
    Write-Host "9. Update ServiceConfiguration.Cloud.cscfg for solution of Segment 5"
    Write-Host "10. Update ServiceDefinition.csdef for solution of Segment 5"
    Write-Host "11. Build the solution to generate the package to deploy."
    Write-Host "12. Create the deployment with the configured solution."
    Write-Host ""
}

if ($CleanupDeployment.IsPresent) {
    Write-Warning "This script will clean up your Azure account by performing the following tasks:"
    Write-Host ""
    Write-Host "1. Remove the storage account for diagnostics created by the setup.deployment script." -ForegroundColor Red
    Write-Host "2. Remove the SQL server created by the setup.deployment script." -ForegroundColor Red
    Write-Host "3. Remove the deployment created by the setup.deployment script." -ForegroundColor Red
    Write-Host "4. Remove the cloud service created by the setup.deployment script." -ForegroundColor Red
    Write-Host "5. Remove begin solution for Segment 5 from working directory."
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


