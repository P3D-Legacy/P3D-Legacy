Namespace Security

    Public Class FileValidation

        Shared _validated As Boolean = False
        Shared _valid As Boolean = False

        Const RUNVALIDATION As Boolean = False
        Const EXPECTEDSIZE As Integer = 40193269
        Const METAHASH As String = "QjVCOTU5MEUyQzU0MTA4MjEwNzA5QjBGNjFFMDdDRDg="

        Public Shared ReadOnly Property IsValid(ByVal ForceResult As Boolean) As Boolean
            Get
                If _validated = False Then
                    _validated = True
                    _valid = FilesValid()
                End If

                If GameController.IS_DEBUG_ACTIVE = True And ForceResult = False Then
                    Return True
                Else
                    Return _valid
                End If
            End Get
        End Property

        Private Shared Function FilesValid() As Boolean
            Dim MeasuredSize As Long = 0

            Dim files As New List(Of String)
            Dim paths() As String = {"Content", "maps", "Scripts"}
            Dim includeExt() As String = {".dat", ".poke", ".lua", ".trainer"}

            If RUNVALIDATION = True Then
                Logger.Log(Logger.LogTypes.Debug, "FileValidation.vb: WARNING! FILE VALIDATION IS RUNNING!")
                For Each subFolder As String In paths
                    For Each file As String In System.IO.Directory.GetFiles(GameController.GamePath & "\" & subFolder, "*.*", IO.SearchOption.AllDirectories)
                        If file.Contains("\Content\Localization\") = False Then
                            Dim ext As String = System.IO.Path.GetExtension(file)
                            If includeExt.Contains(ext.ToLower()) = True Then
                                files.Add(file.Remove(0, GameController.GamePath.Length + 1))
                            End If
                        End If
                    Next
                Next

                Dim s As String = ""
                For Each f As String In files
                    Dim i As Long = New System.IO.FileInfo(GameController.GamePath & "\" & f).Length
                    Dim hash As String = GetMD5FromFile(GameController.GamePath & "\" & f)

                    FileDictionary.Add((GameController.GamePath & "\" & f).ToLower(), New ValidationStorage(i, hash))
                    MeasuredSize += i

                    If s <> "" Then
                        s &= ","
                    End If
                    s &= f & ":" & hash
                Next

                System.IO.File.WriteAllText(GameController.GamePath & "\meta", s)
                Logger.Log(Logger.LogTypes.Debug, "FileValidation.vb: Meta created! Expected Size: " & MeasuredSize &
                           "|MetaHash: " & StringObfuscation.Obfuscate(GetMD5FromFile(GameController.GamePath & "\meta")))

                Core.GameInstance.Exit()
            Else
                If System.IO.File.Exists(GameController.GamePath & "\meta") = True Then
                    If GetMD5FromFile(GameController.GamePath & "\meta") = StringObfuscation.DeObfuscate(METAHASH) Then
                        files = System.IO.File.ReadAllText(GameController.GamePath & "\meta").Split(CChar(",")).ToList()
                        Logger.Debug("Meta loaded. Files to check: " & files.Count)
                    Else
                        Logger.Log(Logger.LogTypes.Warning, "FileValidation.vb: Failed to load Meta (Hash incorrect)! File Validation will fail!")
                    End If
                Else
                    Logger.Log(Logger.LogTypes.Warning, "FileValidation.vb: Failed to load Meta (File not found)! File Validation will fail!")
                End If

                For Each f As String In files
                    Dim fileName As String = f.Split(CChar(":"))(0)
                    Dim fileHash As String = f.Split(CChar(":"))(1)

                    If System.IO.File.Exists(GameController.GamePath & "\" & fileName) Then
                        Dim i As Long = New System.IO.FileInfo(GameController.GamePath & "\" & fileName).Length
                        FileDictionary.Add((GameController.GamePath & "\" & fileName).ToLower(), New ValidationStorage(i, fileHash))
                        MeasuredSize += i
                    End If
                Next
            End If

            If MeasuredSize = EXPECTEDSIZE Then
                Return True
            End If
            Return False
        End Function

        Shared FileDictionary As New Dictionary(Of String, ValidationStorage)

        Public Shared Sub CheckFileValid(ByVal file As String, ByVal relativePath As Boolean, ByVal origin As String)
            Dim validationResult As String = ValidateFile(file, relativePath)
            If validationResult <> "" Then
                Logger.Log(Logger.LogTypes.ErrorMessage, "FileValidation.vb: Detected invalid files in a sensitive game environment. Stopping execution...")

                Dim ex As New Exception("The File Validation system detected invalid files in a sensitive game environment.")
                ex.Data.Add("File", file)
                ex.Data.Add("File Validation result", validationResult)
                ex.Data.Add("Call Origin", origin)
                ex.Data.Add("Relative Path", relativePath)

                Throw ex
            End If
        End Sub

        Private Shared Function ValidateFile(ByVal file As String, ByVal relativePath As Boolean) As String
            If Core.Player.IsGamejoltSave = True And GameController.IS_DEBUG_ACTIVE = False Then
                Dim filePath As String = file.Replace("/", "\")
                If relativePath = True Then
                    filePath = GameController.GamePath & "\" & file
                End If
                Dim i As Long = New System.IO.FileInfo(filePath).Length

                If System.IO.File.Exists(filePath) = True Then
                    If FileDictionary.ContainsKey(filePath.ToLower()) = True Then
                        If i <> FileDictionary(filePath.ToLower()).FileSize Then
                            Return "File Validation rendered the file invalid. Array length invalid."
                        Else
                            Dim hash As String = GetMD5FromFile(filePath)
                            If hash <> FileDictionary(filePath.ToLower()).Hash Then
                                Return "File Validation rendered the file invalid. File has been edited."
                            End If
                        End If
                    Else
                        Return "The File Validation system couldn't find the requested file."
                    End If
                End If
            End If
            Return ""
        End Function

        Private Shared Function GetMD5FromFile(ByVal file As String) As String
            Dim MD5 As System.Security.Cryptography.MD5 = System.Security.Cryptography.MD5.Create()
            Dim Hash As Byte()
            Dim sb As New System.Text.StringBuilder()

            Using st As New IO.FileStream(file, IO.FileMode.Open, IO.FileAccess.Read)
                Hash = MD5.ComputeHash(st)
            End Using

            For Each b In Hash
                sb.Append(b.ToString("X2"))
            Next

            Return sb.ToString()
        End Function

        Private Class ValidationStorage

            Public Hash As String = ""
            Public FileSize As Long = 0

            Public Sub New(ByVal FileSize As Long, ByVal Hash As String)
                Me.FileSize = FileSize
                Me.Hash = Hash
            End Sub

            Public Function CheckValidation(ByVal FileSize As Long) As Boolean
                Return (FileSize = Me.FileSize)
            End Function

            Public Function CheckValidation(ByVal FileSize As Long, ByVal Hash As String) As Boolean
                If Me.FileSize = FileSize And Me.Hash = Hash Then
                    Return True
                End If
                Return False
            End Function

        End Class

    End Class

End Namespace