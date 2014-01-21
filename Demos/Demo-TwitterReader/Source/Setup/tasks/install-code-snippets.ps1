Param([string] $CSharpSnippets,
	[string] $htmlSnippets,
	[string] $xmlSnippets)
	
write-host "========= Installing Code Snippets ... ========="
[string] $documentsFolder = [Environment]::GetFolderPath("MyDocuments")
if (-NOT (test-path "$documentsFolder"))
{
    $documentsFolder = "$env:UserProfile\Documents";
}
[string] $myCSharpSnippetsLocation = "$documentsFolder\Visual Studio 2012\Code Snippets\Visual C#\My Code Snippets"
[string] $myHtmlSnippetsLocation = "$documentsFolder\Visual Studio 2012\Code Snippets\Visual Web Developer\My HTML Snippets"
[string] $myXmlSnippetsLocation = "$documentsFolder\Visual Studio 2012\Code Snippets\XML\My Xml Snippets"

if($CSharpSnippets -AND (test-path $CSharpSnippets))
{
    if (-NOT (test-path "$myCSharpSnippetsLocation"))
    {
        New-Item -ItemType directory "$myCSharpSnippetsLocation" -Force
    }

    Copy-Item "$CSharpSnippets\*.snippet" "$myCSharpSnippetsLocation" -force
}

if($htmlSnippets -AND (test-path $htmlSnippets))
{
    if (-NOT (test-path "$myHtmlSnippetsLocation"))
    {
        New-Item -ItemType directory "$myHtmlSnippetsLocation" -Force
    }

    Copy-Item "$htmlSnippets\*.snippet" "$myHtmlSnippetsLocation" -force
}

if($xmlSnippets -AND (test-path $xmlSnippets))
{
    if (-NOT (test-path "$myXmlSnippetsLocation"))
    {
        New-Item -ItemType directory "$myXmlSnippetsLocation" -Force
    }    
    
    Copy-Item "$xmlSnippets\*.snippet" "$myXmlSnippetsLocation" -force
}

write-host "Installing Code Snippets done!"