$headerFile = 'DemoHeader.md'
$segmentSuffix= 'segment'
$destinationFile = 'demo.md'

Write-Host 'Reading Header File...'
$headerContent = (Get-Content $headerFile) -join [Environment]::NewLine

Write-Host 'Processing Segments...'
for ($index = 1; $index -le 5; $index++)
{
	$segmentFile = $segmentSuffix + $index + '.md'
	$segmentContent = (Get-Content $segmentFile) -join [Environment]::NewLine
	$replaceTag = '\$\$\$segment' + $index		
	$headerContent = $headerContent -replace $replaceTag, $segmentContent
}

Write-Host 'Writing Demo.MD output file...'
Set-Content $destinationFile $headerContent
