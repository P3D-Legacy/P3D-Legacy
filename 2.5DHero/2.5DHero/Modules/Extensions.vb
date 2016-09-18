Imports System.Runtime.CompilerServices

Module Extensions

    <Extension()>
    Public Function IsNumericList(Of T)(ByVal l As List(Of T)) As Boolean
        For Each O As T In l
            If IsNumeric(O.ToString().Replace(".", GameController.DecSeparator)) = False Then
                Return False
            End If
        Next
        Return True
    End Function

    <Extension()>
    Public Function IsBooleanicList(Of T)(ByVal l As List(Of T)) As Boolean
        For Each O As T In l
            If O.ToString() <> "0" And O.ToString() <> "1" And O.ToString().ToLower() <> "true" And O.ToString().ToLower() <> "false" And GetType(Boolean) <> O.GetType Then
                Return False
            End If
        Next
        Return True
    End Function

    <Extension()>
    Public Function DeleteDuplicates(Of T)(ByVal l As List(Of T)) As List(Of T)
        Dim copyList As New List(Of T)
        For Each lI As T In l
            If copyList.Contains(lI) = False Then
                copyList.Add(lI)
            End If
        Next
        Return copyList
    End Function

    <Extension()>
    Public Function ContainsToLower(Of T)(ByVal l As List(Of T), ByVal s As String) As Boolean
        For Each Item As T In l
            If Item.ToString().ToLower() = s.ToLower() Then
                Return True
            End If
        Next
        Return False
    End Function

    <Extension()>
    Public Sub Move(Of T)(ByRef l As List(Of T), ByVal moveItemIndex As Integer, ByVal destinationIndex As Integer)
        Dim i As T = l(moveItemIndex)
        l.RemoveAt(moveItemIndex)
        l.Insert(destinationIndex, i)
    End Sub

    <Extension()>
    Public Function Copy(ByVal t As Texture2D) As Texture2D
        Dim newT As Texture2D = New Texture2D(Core.GraphicsDevice, t.Width, t.Height)

        Dim cArr(newT.Width * newT.Height - 1) As Color
        t.GetData(cArr)

        newT.SetData(cArr)

        Return newT
    End Function

    <Extension()>
    Public Function GetSplit(ByVal fullString As String, ByVal valueIndex As Integer, ByVal seperator As String) As String
        If fullString.Contains(seperator) = False Then
            Return fullString
        Else
            Dim parts() As String = fullString.Split({seperator}, StringSplitOptions.None)
            If parts.Count - 1 >= valueIndex Then
                Return parts(valueIndex)
            Else
                Return fullString
            End If
        End If
    End Function

    <Extension()>
    Public Function GetSplit(ByVal fullString As String, ByVal valueIndex As Integer) As String
        Return GetSplit(fullString, valueIndex, ",")
    End Function

    <Extension()>
    Public Function SetSplit(ByVal fullString As String, ByVal valueIndex As Integer, ByVal newValue As String, ByVal seperator As String, ByVal replace As Boolean) As String
        Dim s() As String = fullString.Split({seperator}, System.StringSplitOptions.None)

        fullString = ""

        For x = 0 To s.Count - 1
            If x = valueIndex Then
                If replace = True Then
                    fullString &= newValue
                Else
                    fullString &= newValue & seperator & s(x)
                End If
            Else
                fullString &= s(x)
            End If

            If x <> s.Count - 1 Then
                fullString &= seperator
            End If
        Next

        Return fullString
    End Function

    <Extension()>
    Public Function SetSplit(ByVal fullString As String, ByVal valueIndex As Integer, ByVal newValue As String, ByVal replace As Boolean) As String
        Return SetSplit(fullString, valueIndex, newValue, ",", replace)
    End Function

    <Extension()>
    Public Function SetSplit(ByVal fullString As String, ByVal valueIndex As Integer, ByVal newValue As String, ByVal seperator As String) As String
        Return SetSplit(fullString, valueIndex, newValue, seperator, True)
    End Function

    <Extension()>
    Public Function SetSplit(ByVal fullString As String, ByVal valueIndex As Integer, ByVal newValue As String) As String
        Return SetSplit(fullString, valueIndex, newValue, ",", True)
    End Function

    <Extension()>
    Public Function RemoveSplit(ByVal fullString As String, ByVal valueIndex As Integer, ByVal separator As String) As String
        If fullString.Contains(separator) = False Then
            Return ""
        End If

        Dim s() As String = fullString.Split(CChar(separator))

        Dim rString As String = ""

        For x = 0 To s.Count - 1
            If x <> valueIndex Then
                If rString <> "" Then
                    rString &= separator
                End If

                rString &= s(x)
            End If
        Next

        Return rString
    End Function

    <Extension()>
    Public Function CountSplits(ByVal fullString As String, ByVal seperator As String) As Integer
        Dim i As Integer = 0
        If fullString.Contains(seperator) = True Then
            For Each c As Char In fullString
                If c = CChar(seperator) Then
                    i += 1
                End If
            Next
        End If
        Return i + 1
    End Function

    <Extension()>
    Public Function CountSplits(ByVal fullString As String) As Integer
        Return CountSplits(fullString, ",")
    End Function

    <Extension()>
    Public Function CountSeperators(ByVal fullstring As String, ByVal seperator As String) As Integer
        Dim i As Integer = 0
        If fullstring.Contains(seperator) = True Then
            For Each c As Char In fullstring
                If c = CChar(seperator) Then
                    i += 1
                End If
            Next
        End If
        Return i
    End Function

    <Extension()>
    Public Function CountSeperators(ByVal fullstring As String) As Integer
        Return CountSeperators(fullstring, ",")
    End Function

    <Extension()>
    Public Sub Print(ByVal s As String)
        Logger.Debug(s)
    End Sub

    <Extension()>
    Public Sub Print(ByVal i As Integer)
        Logger.Debug(i.ToString())
    End Sub

    <Extension()>
    Public Sub Print(ByVal l As Long)
        Logger.Debug(l.ToString())
    End Sub

    <Extension()>
    Public Sub Print(ByVal s As Single)
        Logger.Debug(s.ToString())
    End Sub

    <Extension()>
    Public Sub Print(Of T)(ByVal Array() As T)
        Dim s As String = "{"
        For i = 0 To Array.Length - 1
            If i <> 0 Then
                s &= ", "
            End If

            s &= Array(i).ToString()
        Next
        s &= "}"
        Logger.Debug(s)
    End Sub

    <Extension()>
    Public Function ArrayToString(Of T)(ByVal Array() As T, Optional ByVal NewLine As Boolean = False) As String
        If NewLine = True Then
            Dim s As String = ""
            For i = 0 To Array.Length - 1
                If i <> 0 Then
                    s &= vbNewLine
                End If

                s &= Array(i).ToString()
            Next
            Return s
        Else
            Dim s As String = "{"
            For i = 0 To Array.Length - 1
                If i <> 0 Then
                    s &= ","
                End If

                s &= Array(i).ToString()
            Next
            s &= "}"
            Return s
        End If
    End Function

    <Extension()>
    Public Function ToNumberString(ByVal bool As Boolean) As String
        If bool = True Then
            Return "1"
        Else
            Return "0"
        End If
    End Function

    <Extension()>
    Public Function ToArray(ByVal s As String, ByVal Seperator As String) As String()
        Return s.Replace(vbNewLine, Seperator).Split(CChar(Seperator))
    End Function

    <Extension()>
    Public Function ToList(ByVal s As String, ByVal Seperator As String) As List(Of String)
        Return s.Replace(vbNewLine, Seperator).Split(CChar(Seperator)).ToList()
    End Function

    <Extension()>
    Public Function ToPositive(ByVal i As Integer) As Integer
        If i < 0 Then
            i *= -1
        End If
        Return i
    End Function

    <Extension()>
    Public Function ToNegative(ByVal i As Integer) As Integer
        If i > 0 Then
            i *= -1
        End If
        Return i
    End Function

    <Extension()>
    Public Function Clamp(ByVal i As Integer, ByVal min As Integer, ByVal max As Integer) As Integer
        If i > max Then
            i = max
        End If
        If i < min Then
            i = min
        End If
        Return i
    End Function

    <Extension()>
    Public Function Clamp(ByVal s As Single, ByVal min As Single, ByVal max As Single) As Single
        If s > max Then
            s = max
        End If
        If s < min Then
            s = min
        End If
        Return s
    End Function

    <Extension()>
    Public Function Clamp(ByVal d As Decimal, ByVal min As Decimal, ByVal max As Decimal) As Decimal
        If d > max Then
            d = max
        End If
        If d < min Then
            d = min
        End If
        Return d
    End Function

    <Extension()>
    Public Function Clamp(ByVal d As Double, ByVal min As Double, ByVal max As Double) As Double
        If d > max Then
            d = max
        End If
        If d < min Then
            d = min
        End If
        Return d
    End Function

    <Extension()>
    Public Function CropStringToWidth(ByVal s As String, ByVal font As SpriteFont, ByVal scale As Single, ByVal width As Integer) As String
        Dim fulltext As String = s

        If (font.MeasureString(fulltext).X * scale) <= width Then
            Return fulltext
        Else
            If fulltext.Contains(" ") = False Then
                Dim newText As String = ""
                While fulltext.Length > 0
                    If (font.MeasureString(newText & fulltext(0).ToString()).X * scale) > width Then
                        newText &= vbNewLine
                        newText &= fulltext(0).ToString()
                        fulltext.Remove(0, 1)
                    Else
                        newText &= fulltext(0).ToString()
                        fulltext.Remove(0, 1)
                    End If
                End While
                Return newText
            End If
        End If

        Dim output As String = ""
        Dim currentLine As String = ""
        Dim currentWord As String = ""

        While fulltext.Length > 0
            If fulltext.StartsWith(vbNewLine) = True Then
                If currentLine <> "" Then
                    currentLine &= " "
                End If
                currentLine &= currentWord
                output &= currentLine & vbNewLine
                currentLine = ""
                currentWord = ""
                fulltext = fulltext.Remove(0, 2)
            ElseIf fulltext.StartsWith(" ") = True Then
                If currentLine <> "" Then
                    currentLine &= " "
                End If
                currentLine &= currentWord
                currentWord = ""
                fulltext = fulltext.Remove(0, 1)
            Else
                currentWord &= fulltext(0)
                If (font.MeasureString(currentLine & currentWord).X * scale) >= width Then
                    If currentLine = "" Then
                        output &= currentWord & vbNewLine
                        currentWord = ""
                        currentLine = ""
                    Else
                        output &= currentLine & vbNewLine
                        currentLine = ""
                    End If
                End If
                fulltext = fulltext.Remove(0, 1)
            End If
        End While

        If currentWord <> "" Then
            If currentLine <> "" Then
                currentLine &= " "
            End If
            currentLine &= currentWord
        End If
        If currentLine <> "" Then
            output &= currentLine
        End If

        Return output
    End Function

    <Extension()>
    Public Function CropStringToWidth(ByVal s As String, ByVal font As SpriteFont, ByVal width As Integer) As String
        Return CropStringToWidth(s, font, 1.0F, width)
    End Function

    <Extension()>
    Public Function ToColor(ByVal v As Vector3) As Color
        Return New Color(CInt(v.X * 255), CInt(v.Y * 255), CInt(v.Z * 255))
    End Function

    <Extension()>
    Public Function ReplaceColors(ByVal t As Texture2D, ByVal InputColors() As Color, ByVal OutputColors() As Color) As Texture2D
        Dim newTexture As Texture2D = New Texture2D(Core.GraphicsDevice, t.Width, t.Height)

        If InputColors.Length = OutputColors.Length And InputColors.Length > 0 Then
            Dim Data(t.Width * t.Height - 1) As Color
            Dim newData As New List(Of Color)
            t.GetData(0, Nothing, Data, 0, t.Width * t.Height)

            For i = 0 To Data.Length - 1
                Dim c As Color = Data(i)
                If InputColors.Contains(c) = True Then
                    For iC = 0 To InputColors.Length - 1
                        If InputColors(iC) = c Then
                            c = OutputColors(iC)
                            Exit For
                        End If
                    Next
                End If
                newData.Add(c)
            Next

            newTexture.SetData(newData.ToArray())
        Else
            newTexture = t
        End If

        Return newTexture
    End Function

    <Extension()>
    Public Function xRoot(ByVal root As Integer, ByVal number As Double) As Double
        Dim powered As Double = 1 / root

        Dim returnNumber As Double = Math.Pow(number, powered)

        Return returnNumber
    End Function

    <Extension()>
    Public Function SplitAtNewline(ByVal s As String) As String()
        Return s.Split({Environment.NewLine}, StringSplitOptions.None)
    End Function

    <Extension()>
    Public Function Split(ByVal s As String, ByVal splitAt As String) As String()
        Return s.Split({splitAt}, System.StringSplitOptions.None)
    End Function

    <Extension()>
    Public Function Randomize(Of T)(ByVal L As List(Of T)) As List(Of T)
        Return Randomize(Of T)(L.ToArray()).ToList()
    End Function

    <Extension()>
    Public Function Randomize(Of T)(ByVal L As T()) As T()
        Dim r As New System.Random()
        Dim temp As T
        Dim indexRand As Integer
        Dim indexLast As Integer = L.Count - 1
        For index As Integer = 0 To indexLast
            indexRand = r.Next(index, indexLast)
            temp = L(indexRand)
            L(indexRand) = L(index)
            L(index) = temp
        Next index
        Return L
    End Function

    Public Function GetRandomChance(ByVal chances As List(Of Integer)) As Integer
        Dim totalNumber As Integer = 0
        For Each c As Integer In chances
            totalNumber += c
        Next

        Dim r As Integer = Core.Random.Next(0, totalNumber + 1)

        Dim x As Integer = 0
        For i = 0 To chances.Count - 1
            x += chances(i)
            If r <= x Then
                Return i
            End If
        Next

        Return -1
    End Function

    <Extension()>
    Public Function ProjectPoint(ByVal v As Vector3, ByVal View As Matrix, ByVal Projection As Matrix) As Vector2
        Dim mat As Matrix = Matrix.Identity * View * Projection

        Dim v4 As Vector4 = Vector4.Transform(v, mat)

        Return New Vector2(CSng(((v4.X / v4.W + 1) * (windowSize.Width / 2))), CSng(((1 - v4.Y / v4.W) * (windowSize.Height / 2))))
    End Function

    <Extension()>
    Public Function ToInteger(ByVal s As Single) As Integer
        Return CInt(s)
    End Function

    ''' <summary>
    ''' Inverts the Color.
    ''' </summary>
    ''' <param name="c"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function Invert(ByVal c As Color) As Color
        Return New Color(255 - c.R, 255 - c.G, 255 - c.B, c.A)
    End Function

    <Extension()>
    Public Function ReplaceDecSeparator(ByVal s As String) As String
        Return s.Replace(GameController.DecSeparator, ".")
    End Function

    <Extension()>
    Public Function InsertDecSeparator(ByVal s As String) As String
        Return s.Replace(".", GameController.DecSeparator)
    End Function

    ''' <summary>
    ''' Converts a System.Drawing.Color into a Xna.Framework.Color.
    ''' </summary>
    ''' <param name="c"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function ToXNA(ByVal c As Drawing.Color) As Color
        Return New Color(c.R, c.G, c.B, c.A)
    End Function

    ''' <summary>
    ''' Converts a Xna.Framework.Color into a System.Drawing.Color.
    ''' </summary>
    ''' <param name="c"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function ToDrawing(ByVal c As Color) As Drawing.Color
        Return Drawing.Color.FromArgb(c.R, c.G, c.B, c.A)
    End Function

End Module