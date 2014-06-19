class microsoft-sysinternals {
  $taskmgr = 'HKLM\Software\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\Taskmgr.exe'
  $procexp = 'c:\sysinternals\procexp.exe'

  exec { 'download-sysinternals':
    command  => template('microsoft-sysinternals/downloadsysinternals.ps1'),
    creates  => $procexp,
    provider => powershell,
  }
  registry_key { taskmgr:
    name    => $taskmgr, 
    ensure  => present,
    require => Exec['download-sysinternals'],
  }
  registry_value { taskmgr:
    name   => "$taskmgr\\Debugger",
    ensure => present,
    type   => string,
    data   => $procexp,
  }
}
