Public Class EggCreator

    Const PUFFER As Integer = 40
    Const MINCOLOR As Integer = 100
    Const GRAYSCALERANGE As Integer = 2

    Public Shared Function CreateEggSprite(ByVal Pokemon As Pokemon, ByVal DefaultEggSprite As Texture2D, ByVal EggTemplate As Texture2D) As Texture2D
        'Colors:
        'Light center: 0,255,255 (get from middle color + light)
        'Darker center: 255,0,0 (get from middle color)
        'Light dots: 0,255,0
        'Darker dots: 255,255,0

        Dim egg As Texture2D = EggTemplate
        Dim sprite As Texture2D = Pokemon.GetMenuTexture(False)

        Dim arr As List(Of Color) = GetColors2(sprite, New Rectangle(0, 0, sprite.Width, sprite.Height), PUFFER).ToList()

        If arr.Count < 2 Then
            Return DefaultEggSprite
        Else
            While arr.Count < 4
                For i = 0 To arr.Count - 1
                    If arr.Count < 4 Then
                        arr.Add(arr(i))
                    End If
                Next
            End While
        End If

        While arr.Count > 4
            arr.RemoveAt(arr.Count - 1)
        End While

        arr = (From c As Color In arr Order By (CInt(c.R) + CInt(c.G) + CInt(c.B)) Descending).ToList()

        Dim inputColors() As Color = {New Color(0, 255, 255), New Color(255, 0, 0), New Color(0, 255, 0), New Color(255, 255, 0)}
        egg = egg.ReplaceColors(inputColors, arr.ToArray())

        Return egg
    End Function

#Region "EggColorAlgorithm"
    
    Private Shared Function GetColors2(ByVal tex As Texture2D, ByVal rect As Rectangle, ByVal puffer As Integer) As Color()
        Dim data As Color() = New Color(rect.Width * rect.Height - 1) {}

        If data.Length = 0 Then
            Return {Color.White}
        End If

        tex.GetData(Of Color)(0, rect, data, 0, data.Length)

        Dim cDic As New Dictionary(Of Color, Integer)

        For Each c As Color In data
            If c.A <> 0 Then
                If c.R <> c.G Or c.R <> c.B Or c.G <> c.B Then
                    If CInt(c.R) + CInt(c.G) + CInt(c.B) >= MINCOLOR Then
                        If IsGrayScale(c) = False Then
                            Dim cc As Color = IsNear2(c, cDic.Keys.ToArray(), puffer)
                            If cDic.ContainsKey(cc) = False Then
                                cDic.Add(cc, 1)
                            Else
                                cDic(cc) += 1
                            End If
                        End If
                    End If
                End If
            End If
        Next

        Dim l As New List(Of KeyValuePair(Of Color, Integer))
        l.AddRange(cDic)

        l = l.OrderBy(Function(x) x.Value).ToList()

        Dim returnList As New List(Of Color)

        For i = 0 To 3
            If l.Count - 1 >= i Then
                Dim addID As Integer = (l.Count - 1) - i
                returnList.Add(l(addID).Key)
            End If
        Next

        Return returnList.ToArray()
    End Function

    Private Shared Function IsNear2(ByVal c As Color, ByVal cArr As Color(), ByVal puffer As Integer) As Color
        For Each cc As Color In cArr
            If Math.Abs(CInt(c.R) - CInt(cc.R)) <= puffer And Math.Abs(CInt(c.G) - CInt(cc.G)) <= puffer And Math.Abs(CInt(c.B) - CInt(cc.B)) <= puffer Then
                Return cc
            End If
        Next
        Return c
    End Function

    Private Shared Function IsGrayScale(ByVal c As Color) As Boolean
        Dim range As Integer = GRAYSCALERANGE

        Dim v As Double = (CInt(c.R) + (c.G) + CInt(c.B)) / 3
        Dim maxR As Double = Math.Abs(v - c.R)
        Dim maxG As Double = Math.Abs(v - c.G)
        Dim maxB As Double = Math.Abs(v - c.B)

        Dim max As Double = 0
        If maxR > max Then
            max = maxR
        End If
        If maxG > max Then
            max = maxG
        End If
        If maxB > max Then
            max = maxB
        End If

        If max <= range Then
            Return True
        End If
        Return False
    End Function

#End Region

End Class