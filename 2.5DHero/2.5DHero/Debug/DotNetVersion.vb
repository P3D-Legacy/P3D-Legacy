''' <summary>
''' A class to supply .Net installation information.
''' </summary>
Public Class DotNetVersion

    ''' <summary>
    ''' Returns .Net installation information.
    ''' </summary>
    Public Shared Function GetInstalled() As String
        Dim output As String = ""

        Try
            Using ndpKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry32).OpenSubKey("SOFTWARE\Microsoft\NET Framework Setup\NDP\")
                For Each versionKeyName As String In ndpKey.GetSubKeyNames()
                    If versionKeyName.StartsWith("v") Then
                        Dim versionKey As Microsoft.Win32.RegistryKey = ndpKey.OpenSubKey(versionKeyName)
                        Dim name As String = DirectCast(versionKey.GetValue("Version", ""), String)
                        Dim sp As String = versionKey.GetValue("SP", "").ToString()
                        Dim install As String = versionKey.GetValue("Install", "").ToString()
                        If install = "" Then
                            'no install info, ust be later
                            output &= versionKeyName & "  " & name & Environment.NewLine
                        Else
                            If sp <> "" AndAlso install = "1" Then
                                output &= versionKeyName & "  " & name & "  SP" & sp & Environment.NewLine
                            End If
                        End If
                        If name <> "" Then
                            Continue For
                        End If
                        For Each subKeyName As String In versionKey.GetSubKeyNames()
                            Dim subKey As Microsoft.Win32.RegistryKey = versionKey.OpenSubKey(subKeyName)
                            name = DirectCast(subKey.GetValue("Version", ""), String)
                            If name <> "" Then
                                sp = subKey.GetValue("SP", "").ToString()
                            End If
                            install = subKey.GetValue("Install", "").ToString()
                            If install = "" Then
                                'no install info, ust be later
                                output &= versionKeyName & "  " & name & Environment.NewLine
                            Else
                                If sp <> "" AndAlso install = "1" Then
                                    output &= "  " & subKeyName & "  " & name & "  SP" & sp & Environment.NewLine
                                ElseIf install = "1" Then
                                    output &= "  " & subKeyName & "  " & name & Environment.NewLine
                                End If
                            End If
                        Next
                    End If
                Next
            End Using
        Catch ex As Exception
            output &= "Error getting .Net installation information."
        End Try

        Return output
    End Function

End Class