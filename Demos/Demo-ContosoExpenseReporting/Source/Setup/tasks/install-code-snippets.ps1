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

if($CSharpSnippets) {
	Copy-Item "$CSharpSnippets\*.snippet" "$myCSharpSnippetsLocation" -force
	}
if($htmlSnippets) {
	Copy-Item "$htmlSnippets\*.snippet" "$myHtmlSnippetsLocation" -force
	}
if($xmlSnippets) {
	Copy-Item "$xmlSnippets\*.snippet" "$myXmlSnippetsLocation" -force
	}
write-host "Installing Code Snippets done!"