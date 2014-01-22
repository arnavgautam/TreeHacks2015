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
    Copy-Item "$CSharpSnippets\*.snippet" "$myCSharpSnippetsLocation" -force -recurse
}

if($htmlSnippets -AND (test-path $htmlSnippets))
{
    Copy-Item "$htmlSnippets\*.snippet" "$myHtmlSnippetsLocation" -force -recurse
}

if($xmlSnippets -AND (test-path $xmlSnippets))
{
    Copy-Item "$xmlSnippets\*.snippet" "$myXmlSnippetsLocation" -force -recurse
}
write-host "Installing Code Snippets done!"