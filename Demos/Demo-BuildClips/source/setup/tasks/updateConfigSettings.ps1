param($settingsFile, $key, $value, $JsFormat = $false)

function Replace
{
	param($text, $parameterName, $parameterValue)

	if ($parameterValue -ne $null)
	{
		if ($jsFormat)
		{
			return [Regex]::Replace($text, ("{0}: '(.)*'" -f $parameterName), ("{0}: '{1}'" -f $parameterName, $parameterValue))
		}
		else
		{
			return [Regex]::Replace($text, ("{0} = `"(.)*`"" -f $parameterName), ("{0} = `"{1}`"" -f $parameterName, $parameterValue))
		}
	}
	else
	{
		return $text
	}
}

$settingsText = [String]::Join([Environment]::NewLine, (Get-Content -Path $settingsFile))

$settingsText = Replace $settingsText $key $value

Set-Content -Path $settingsFile -Value $settingsText