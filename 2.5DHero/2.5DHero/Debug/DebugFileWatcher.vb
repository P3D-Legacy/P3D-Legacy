''' <summary>
''' This class watches the maps and scripts folders of the project to automatically
''' copy changed files on map reloads to the bin directory.
''' </summary>
Public Class DebugFileWatcher

    Private Shared _changedFiles As List(Of String)
    Private Shared _watchers As List(Of FileSystemWatcher)
    Private Shared _isWatching As Boolean = False

    Shared Sub New()
        _changedFiles = New List(Of String)()
        _watchers = New List(Of FileSystemWatcher)()
    End Sub

    Public Shared Sub TriggerReload()
        ' copy all changed files
        SyncLock _changedFiles
            Dim projectPath = GetProjectPath()
            Dim targetPath = AppDomain.CurrentDomain.BaseDirectory

            For Each changedFile In _changedFiles
                Dim relativeFile = changedFile.Remove(0, projectPath.Length + 1)
                Dim targetFile = Path.Combine(targetPath, relativeFile)

                File.Copy(changedFile, targetFile, True)
            Next

            ' clear the changed files afterwards
            _changedFiles.Clear()
        End SyncLock
    End Sub

    Private Shared Function GetProjectPath() As String
        Return New DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName
    End Function

    Public Shared Sub StartWatching()
        If Not _isWatching Then
            _isWatching = True

            Dim projectPath = GetProjectPath()

            ' MAPS
            Dim mapsPath = Path.Combine(projectPath, "maps")
            Dim mapsWatcher = New FileSystemWatcher With {
                .Path = mapsPath,
                .NotifyFilter = NotifyFilters.LastWrite,
                .IncludeSubdirectories = True
            }

            AddHandler mapsWatcher.Changed, AddressOf OnChanged
            mapsWatcher.EnableRaisingEvents = True

            _watchers.Add(mapsWatcher)

            ' SCRIPTS
            Dim scriptsPath = Path.Combine(projectPath, "Scripts")
            Dim scriptsWatcher = New FileSystemWatcher With {
                .Path = scriptsPath,
                .NotifyFilter = NotifyFilters.LastWrite,
                .IncludeSubdirectories = True
            }

            AddHandler scriptsWatcher.Changed, AddressOf OnChanged
            scriptsWatcher.EnableRaisingEvents = True

            _watchers.Add(scriptsWatcher)
        End If
    End Sub

    Private Shared Sub OnChanged(source As Object, e As FileSystemEventArgs)
        SyncLock _changedFiles
            Dim file = e.FullPath
            If Not _changedFiles.Contains(file) Then
                Logger.Debug("File changed: " + file)
                _changedFiles.Add(file)
            End If
        End SyncLock
    End Sub

End Class
