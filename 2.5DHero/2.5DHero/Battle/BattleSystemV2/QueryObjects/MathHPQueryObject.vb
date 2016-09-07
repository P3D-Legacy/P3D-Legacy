Namespace BattleSystem

    Public Class MathHPQueryObject

        Inherits QueryObject

#Region "PVPData"
        Dim Current As Integer = 0
        Dim Max As Integer = 0
        Dim Damage As Integer = 0
#End Region

        Private _target As Integer
        Private _amount As Integer = 1000
        Private _delay As Single = 1.0F

        Private _startamount As Integer = 1000

        Private _position As Vector2

        Public Sub New(ByVal Current As Integer, ByVal Max As Integer, ByVal Damage As Integer, ByVal position As Vector2)
            MyBase.New(QueryTypes.MathHP)

            Me.Current = Current
            Me.Max = Max
            Me.Damage = Damage

            _amount = CInt((1000 / Max) * Current)
            _startamount = _amount

            Dim endState As Integer = Current - Damage
            If endState < 0 Then
                endState = 0
            End If

            _target = CInt((1000 / Max) * endState)
            _position = position
        End Sub

        Public Overrides Sub Update(BV2Screen As BattleScreen)
            If _target < _amount Then
                _amount -= 8
                If _amount < _target Then
                    _amount = _target
                End If
                Particles.Add(New Particle(New Vector2(CSng((_amount / 1000) * 300 + _position.X), _position.Y + Core.Random.Next(0, 34)), GetColor()))

                Shake = New Vector2(Shake.X + (Core.Random.Next(0, 3) - 1), Shake.Y + (Core.Random.Next(0, 3) - 1))
            End If
            If _target > _amount Then
                _amount += 8
                If _amount > _target Then
                    _amount = _target
                End If
            End If
            If _target = _amount Then
                If _delay > 0.0F Then
                    Me._delay -= 0.01F
                    If _delay <= 0.0F Then
                        _delay = 0.0F
                    End If
                    If Controls.Accept(True, True) = True Then
                        _delay = 0.0F
                    End If
                End If
            End If
        End Sub

        Private Particles As New List(Of Particle)
        Private Shake As Vector2 = New Vector2(0, 0)

        Public Overrides Sub Draw(BV2Screen As BattleScreen)
            Canvas.DrawRectangle(New Rectangle(CInt(_position.X + Shake.X), CInt(_position.Y + Shake.Y), 300, 30), Color.White)
            If _amount >= _target Then
                Canvas.DrawScrollBar(New Vector2(_position.X + Shake.X, _position.Y + Shake.Y), 1000, _startamount, 0, New Size(300, 30), True, New Color(0, 0, 0, 0), Color.DarkGray)
            End If
            Canvas.DrawScrollBar(New Vector2(_position.X + Shake.X, _position.Y + Shake.Y), 1000, _amount, 0, New Size(300, 30), True, New Color(0, 0, 0, 0), GetColor())
            Canvas.DrawBorder(4, New Rectangle(CInt(_position.X - 2 + Shake.X), CInt(_position.Y - 2 + Shake.Y), 304, 34), Color.Gray)

            For i = 0 To Particles.Count - 1
                If i <= Particles.Count - 1 Then
                    Dim p As Particle = Particles(i)
                    p.Draw(Shake)
                    If p.CanBeRemoved = True Then
                        Particles.RemoveAt(i)
                        i -= 1
                    End If
                End If
            Next
        End Sub

        Private Function GetColor() As Color
            Dim percent As Integer = CInt((_amount / 1000) * 100)

            If percent > 75 Then
                Return New Color(64, 191, 0)
            ElseIf percent <= 75 And percent > 50 Then
                Return New Color(157, 191, 0)
            ElseIf percent <= 50 And percent > 25 Then
                Return New Color(222, 191, 0)
            Else
                Return New Color(192, 63, 0)
            End If

        End Function

        Public Overrides ReadOnly Property IsReady As Boolean
            Get
                If _amount = _target Then
                    If _delay = 0.0F Then
                        Return True
                    End If
                End If
                Return False
            End Get
        End Property

        Class Particle

            Public Delay As Single = 0.0F
            Public Position As Vector2
            Public Color As Color

            Public CanBeRemoved As Boolean = False

            Public Sub New(ByVal position As Vector2, ByVal Color As Color)
                Me.Position = position
                Me.Color = Color
                Me.Delay = 0.15F
            End Sub

            Public Sub Draw(ByVal offset As Vector2)
                Me.Delay -= 0.01F
                If Me.Delay <= 0.0F Then
                    Me.Delay = 0.0F
                    Me.CanBeRemoved = True
                End If
                Canvas.DrawRectangle(New Rectangle(CInt(Position.X + offset.X), CInt(Position.Y + offset.Y), 4, 4), Color)
            End Sub

        End Class

        Public Overrides Function NeedForPVPData() As Boolean
            Return True
        End Function

        Public Shared Shadows Function FromString(ByVal input As String) As QueryObject
            Dim d() As String = input.Split(CChar("|"))
            Return New MathHPQueryObject(CInt(d(0)), CInt(d(1)), CInt(d(2)), New Vector2(CSng(STSE(d(3))), CSng(STSE(d(4)))))
        End Function

        Public Overrides Function ToString() As String
            Dim s As String = Me.Current.ToString() & "|" &
                Me.Max.ToString() & "|" &
                Me.Damage.ToString() & "|" &
                SEST(Me._position.X.ToString()) & "|" & SEST(Me._position.Y.ToString())

            Return "{MATHHP|" & s & "}"
        End Function

        Private Shared Function STSE(ByVal s As String) As String
            Return s.Replace(".", GameController.DecSeparator)
        End Function

        Private Shared Function SEST(ByVal s As String) As String
            Return s.Replace(GameController.DecSeparator, ".")
        End Function

    End Class

End Namespace