''' <summary>
''' This class watches the maps and scripts folders of the project to automatically
''' copy changed files on map reloads to the bin directory.
''' </summary>
Public Class DebugFileWatcher

    Private Shared _changedFiles As List(Of String)
    Private Shared _watcher As FileSystemWatcher
    Private Shared _isWatching As Boolean = False

    Shared Sub New()
        _changedFiles = New List(Of String)()
    End Sub

    Public Shared Sub TriggerReload()
        ' copy all changed files
        SyncLock _changedFiles
            Dim projectPath = GetProjectPath()
            Dim targetPath = AppDomain.CurrentDomain.BaseDirectory

            For Each changedFile In _changedFiles
                Dim relativeFile = changedFile.Remove(0, projectPath.Length + 1)

                If File.Exists(relativeFile) Then
                    Dim targetFile = Path.Combine(targetPath, relativeFile)
                    File.Copy(changedFile, targetFile, True)
                End If
            Next

            ' clear the changed files afterwards
            _changedFiles.Clear()
        End SyncLock
    End Sub

    Private Shared Function GetProjectPath() As String
        'Return New DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName '' Remove when making builds for others.
        Return New DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).FullName ''Use this one instead in such case.
    End Function

    Public Shared Sub StartWatching()
        If Not _isWatching Then
            _isWatching = True

            Dim projectPath = GetProjectPath()

            Dim contentPath = Path.Combine(projectPath, "Content")
            _watcher = New FileSystemWatcher With {
                .Path = contentPath,
                .NotifyFilter = NotifyFilters.LastWrite Or NotifyFilters.LastAccess,
                .IncludeSubdirectories = True
            }

            AddHandler _watcher.Changed, AddressOf OnChanged
            _watcher.EnableRaisingEvents = True

        End If
    End Sub

    Private Shared Sub OnChanged(source As Object, e As FileSystemEventArgs)
        SyncLock _changedFiles
            Dim file = e.FullPath
            ' if files are edited with certain programs, FileSystemWatcher freaks out.
            ' it replaces the filename with some random sequence of chars.
            If IO.File.Exists(file) Then
                If Not _changedFiles.Contains(file) Then
                    Logger.Debug("File changed: " + file)
                    _changedFiles.Add(file)
                End If
            Else
                ' Distinct trait of these changes are that the filename ends with a tilde.
                ' only thing that can be done is watch the entire folder instead of just a single file...
                If file.EndsWith("~") Then
                    Dim dir = Path.GetDirectoryName(file)
                    Logger.Debug("Single file can't be watched. Watch folder """ + dir + """ instead.")
                    For Each dirFile In Directory.GetFiles(dir)
                        If Not _changedFiles.Contains(dirFile) Then
                            Logger.Debug("File changed: " + dirFile)
                            _changedFiles.Add(dirFile)
                        End If
                    Next
                End If
            End If
        End SyncLock
    End Sub

End Class
