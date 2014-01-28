reg add "HKEY_CLASSES_ROOT\MIME\Database\Content Type\application/json" /v CLSID /t REG_SZ /d "{25336920-03F9-11cf-8FD0-00AA00686F13}" /f; 
reg add "HKEY_CLASSES_ROOT\MIME\Database\Content Type\application/json" /v Encoding /t REG_DWORD /d 0x08000000 /f
reg add "HKEY_CLASSES_ROOT\MIME\Database\Content Type\text/json" /v CLSID /t REG_SZ /d "{25336920-03F9-11cf-8FD0-00AA00686F13}" /f; 
reg add "HKEY_CLASSES_ROOT\MIME\Database\Content Type\text/json" /v Encoding /t REG_DWORD /d 0x08000000 /f
write-host "JSON Visualization enabled in Internet Explorer"