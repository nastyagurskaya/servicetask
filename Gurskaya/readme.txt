To install service: InstallUtil.exe "FILEDIRECTORY"\FileWatcherService.exe
To start service: sc start FileWatcherService "LOGDIRECTORY" "FILETOWATCHDIRECTORY" example: sc start FileWatcherService D:\ hashCRC2.txt

To stop service:sc stop
To uninstall service: InstallUtil.exe /u "FILEDIRECTORY"\FileWatcherService.exe