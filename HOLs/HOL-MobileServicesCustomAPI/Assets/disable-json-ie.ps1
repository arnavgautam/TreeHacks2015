reg add "HKEY_CLASSES_ROOT\MIME\Database\Content Type\application/json" /v CLSID /f
reg delete "HKEY_CLASSES_ROOT\MIME\Database\Content Type\application/json" /v Encoding /f
reg add "HKEY_CLASSES_ROOT\MIME\Database\Content Type\text/json" /v CLSID /f
reg delete "HKEY_CLASSES_ROOT\MIME\Database\Content Type\text/json" /v Encoding /f
write-host "JSON Visualization disabled in Internet Explorer"