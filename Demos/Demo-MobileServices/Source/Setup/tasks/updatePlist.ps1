Param([string] $azureSettingsFile, [string] $plistPath)

[xml]$plist = Get-Content $plistPath;
[xml]$configuration = Get-Content $azureSettingsFile;

$elements = $plist.SelectNodes("plist/dict").ChildNodes

for ($i=0; $i -le ($elements.Count-1) ; $i = $i + 2)
{
    $key = $elements[$i]."#text";
    if ($configuration.configuration.clientSettings[$key]."#text" -ne $null){
        $value = $configuration.configuration.clientSettings[$key]."#text"
        $elements[$i+1].set_InnerXML($value);
    }
}

$plist.Save($plistPath);


# We need to remove the [] added when file is saved.
# Reopen the file
$content = Get-Content $plistPath;
$content[1] = "<!DOCTYPE plist PUBLIC `"-//Apple//DTD PLIST 1.0//EN`" `"http://www.apple.com/DTDs/PropertyList-1.0.dtd`">";
$content | Set-Content  $plistPath
