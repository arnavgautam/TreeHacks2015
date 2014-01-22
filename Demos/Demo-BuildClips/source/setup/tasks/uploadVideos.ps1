Param([string] $videosDir, [string] $apiUrl)

Foreach ($file in Get-ChildItem $videosDir)
{
    Write-Host "Uploading"$file.Name"..."

    $request = [System.Net.HttpWebRequest]::Create($apiUrl)
    $boundary = [System.Guid]::NewGuid().ToString()
    
    $request.ContentType = [System.String]::Format("multipart/form-data; boundary={0}", $boundary)
    $request.Method = "POST"

    #Build contents for Post
    $header = [System.String]::Format("--{0}", $boundary)
    $footer = [System.String]::Format("{0}--", $header)

    $encoding = [System.Text.Encoding]::UTF8
    $postDataStream = New-Object System.IO.MemoryStream

    #video
    $video = [System.IO.File]::ReadAllBytes($file.FullName)
    $data = [System.String]::Format("`r`n{0}`r`nContent-Disposition: form-data; name=""videoFile""; filename=""{1}""`r`nContent-Type: video/mp4`r`n`r`n", $header, $file.Name)
    $postDataStream.Write($encoding.GetBytes($data), 0, $encoding.GetByteCount($data))
    $postDataStream.Write($video, 0, $video.Length)
    
    #title
    $data = [System.String]::Format("`r`n{0}`r`nContent-Disposition: form-data; name=""title""`r`n`r`n{1}", $header, $file.BaseName.Replace("_", " "))
    $postDataStream.Write($encoding.GetBytes($data), 0, $encoding.GetByteCount($data))

    #description
    $data = [System.String]::Format("`r`n{0}`r`nContent-Disposition: form-data; name=""description""`r`n`r`n{1}", $header, $file.BaseName.Replace("_", " "))
    $postDataStream.Write($encoding.GetBytes($data), 0, $encoding.GetByteCount($data))

    #footer
    $data = [System.String]::Format("`r`n{0}`r`n", $footer)
    $postDataStream.Write($encoding.GetBytes($data), 0, $encoding.GetByteCount($data))
 
    #send request
    $postDataStream.Position = 0
    $postDataBytes = New-Object byte[] $postDataStream.Length
    $postDataStream.Read($postDataBytes, 0, $postDataBytes.Length) | Out-Null
    $postDataStream.Close()
    $request.ContentLength = $postDataBytes.Length
    $requestStream = $request.GetRequestStream()
    $requestStream.Write($postDataBytes, 0, $postDataBytes.Length)
    $requestStream.Close()
    $response = $request.GetResponse()
}


