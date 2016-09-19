''' <summary>
''' Represents the Element of a Pokémon or move type.
''' </summary>
Public Class Element

    ''' <summary>
    ''' The Type an Element can be.
    ''' </summary>
    Public Enum Types
        Normal
        Fighting
        Flying
        Poison
        Ground
        Rock
        Bug
        Ghost
        Steel
        Fire
        Water
        Grass
        Electric
        Psychic
        Ice
        Dragon
        Dark
        Fairy
        Shadow
        Blank
    End Enum

    Private _type As Types = Types.Blank

    ''' <summary>
    ''' The Type of this Element.
    ''' </summary>
    Public Property Type As Types
        Get
            Return Me._type
        End Get
        Set(value As Types)
            Me._type = value
        End Set
    End Property

    ''' <summary>
    ''' Creates a new instance of the Element class.
    ''' </summary>
    ''' <param name="TypeID">The ID of the type.</param>
    Public Sub New(ByVal TypeID As Integer)
        Select Case TypeID
            Case 0
                Me._type = Types.Normal
            Case 1
                Me._type = Types.Fighting
            Case 2
                Me._type = Types.Flying
            Case 3
                Me._type = Types.Poison
            Case 4
                Me._type = Types.Ground
            Case 5
                Me._type = Types.Rock
            Case 6
                Me._type = Types.Bug
            Case 7
                Me._type = Types.Ghost
            Case 8
                Me._type = Types.Steel
            Case 9
                Me._type = Types.Fire
            Case 10
                Me._type = Types.Water
            Case 11
                Me._type = Types.Grass
            Case 12
                Me._type = Types.Electric
            Case 13
                Me._type = Types.Psychic
            Case 14
                Me._type = Types.Ice
            Case 15
                Me._type = Types.Dragon
            Case 16
                Me._type = Types.Dark
            Case 17
                Me._type = Types.Blank
            Case 18
                Me._type = Types.Fairy
        End Select
    End Sub

    ''' <summary>
    ''' Creates a new instance of the Element class.
    ''' </summary>
    ''' <param name="Type">The Type as string.</param>
    Public Sub New(ByVal Type As String)
        Select Case Type.ToLower()
            Case "normal"
                Me._type = Types.Normal
            Case "fighting"
                Me._type = Types.Fighting
            Case "flying"
                Me._type = Types.Flying
            Case "poison"
                Me._type = Types.Poison
            Case "ground"
                Me._type = Types.Ground
            Case "rock"
                Me._type = Types.Rock
            Case "bug"
                Me._type = Types.Bug
            Case "ghost"
                Me._type = Types.Ghost
            Case "steel"
                Me._type = Types.Steel
            Case "fire"
                Me._type = Types.Fire
            Case "water"
                Me._type = Types.Water
            Case "grass"
                Me._type = Types.Grass
            Case "electric"
                Me._type = Types.Electric
            Case "psychic"
                Me._type = Types.Psychic
            Case "ice"
                Me._type = Types.Ice
            Case "dragon"
                Me._type = Types.Dragon
            Case "dark"
                Me._type = Types.Dark
            Case "fairy"
                Me._type = Types.Fairy
            Case "shadow"
                Me._type = Types.Shadow
            Case "blank"
                Me._type = Types.Blank
            Case Else
                Me._type = Types.Blank
        End Select
    End Sub

    ''' <summary>
    ''' Creates a new instance of the Element class.
    ''' </summary>
    ''' <param name="Type">The Type to set this Element to.</param>
    Public Sub New(ByVal Type As Types)
        Me._type = Type
    End Sub

    ''' <summary>
    ''' Returns a multiplier which represents the connection between an attacking and a defending element.
    ''' </summary>
    ''' <param name="AttackElement">The attacking element.</param>
    ''' <param name="DefenseElement">The defending element.</param>
    Public Shared Function GetElementMultiplier(ByVal AttackElement As Element, ByVal DefenseElement As Element) As Single
        Dim a As Element = AttackElement
        Dim d As Element = DefenseElement

        If d Is Nothing Or a Is Nothing Then
            Return 1
        End If

        If d._type = Types.Blank Or a._type = Types.Blank Then
            Return 1
        End If

        Select Case a._type
            Case Types.Normal
                Select Case d._type
                    Case Types.Normal
                        Return 1
                    Case Types.Fighting
                        Return 1
                    Case Types.Flying
                        Return 1
                    Case Types.Poison
                        Return 1
                    Case Types.Ground
                        Return 1
                    Case Types.Rock
                        Return 0.5F
                    Case Types.Bug
                        Return 1
                    Case Types.Ghost
                        Return 0
                    Case Types.Steel
                        Return 0.5F
                    Case Types.Fire
                        Return 1
                    Case Types.Water
                        Return 1
                    Case Types.Grass
                        Return 1
                    Case Types.Electric
                        Return 1
                    Case Types.Psychic
                        Return 1
                    Case Types.Ice
                        Return 1
                    Case Types.Dragon
                        Return 1
                    Case Types.Dark
                        Return 1
                    Case Else
                        Return 1
                End Select
            Case Types.Fighting
                Select Case d._type
                    Case Types.Normal
                        Return 2
                    Case Types.Fighting
                        Return 1
                    Case Types.Flying
                        Return 0.5F
                    Case Types.Poison
                        Return 0.5F
                    Case Types.Ground
                        Return 1
                    Case Types.Rock
                        Return 2
                    Case Types.Bug
                        Return 0.5F
                    Case Types.Ghost
                        Return 0
                    Case Types.Steel
                        Return 2
                    Case Types.Fire
                        Return 1
                    Case Types.Water
                        Return 1
                    Case Types.Grass
                        Return 1
                    Case Types.Electric
                        Return 1
                    Case Types.Psychic
                        Return 0.5F
                    Case Types.Ice
                        Return 2
                    Case Types.Dragon
                        Return 1
                    Case Types.Dark
                        Return 2
                    Case Types.Fairy
                        Return 0.5F
                    Case Else
                        Return 1
                End Select
            Case Types.Flying
                Select Case d._type
                    Case Types.Normal
                        Return 1
                    Case Types.Fighting
                        Return 2
                    Case Types.Flying
                        Return 1
                    Case Types.Poison
                        Return 1
                    Case Types.Ground
                        Return 1
                    Case Types.Rock
                        Return 0.5F
                    Case Types.Bug
                        Return 2
                    Case Types.Ghost
                        Return 1
                    Case Types.Steel
                        Return 0.5F
                    Case Types.Fire
                        Return 1
                    Case Types.Water
                        Return 1
                    Case Types.Grass
                        Return 2
                    Case Types.Electric
                        Return 0.5F
                    Case Types.Psychic
                        Return 1
                    Case Types.Ice
                        Return 1
                    Case Types.Dragon
                        Return 1
                    Case Types.Dark
                        Return 1
                    Case Else
                        Return 1
                End Select
            Case Types.Poison
                Select Case d._type
                    Case Types.Normal
                        Return 1
                    Case Types.Fighting
                        Return 1
                    Case Types.Flying
                        Return 1
                    Case Types.Poison
                        Return 0.5F
                    Case Types.Ground
                        Return 0.5F
                    Case Types.Rock
                        Return 0.5F
                    Case Types.Bug
                        Return 1
                    Case Types.Ghost
                        Return 0.5F
                    Case Types.Steel
                        Return 0
                    Case Types.Fire
                        Return 1
                    Case Types.Water
                        Return 1
                    Case Types.Grass
                        Return 2
                    Case Types.Electric
                        Return 1
                    Case Types.Psychic
                        Return 1
                    Case Types.Ice
                        Return 1
                    Case Types.Dragon
                        Return 1
                    Case Types.Dark
                        Return 1
                    Case Types.Fairy
                        Return 2
                    Case Else
                        Return 1
                End Select
            Case Types.Ground
                Select Case d._type
                    Case Types.Normal
                        Return 1
                    Case Types.Fighting
                        Return 1
                    Case Types.Flying
                        Return 0
                    Case Types.Poison
                        Return 2
                    Case Types.Ground
                        Return 1
                    Case Types.Rock
                        Return 2
                    Case Types.Bug
                        Return 0.5F
                    Case Types.Ghost
                        Return 1
                    Case Types.Steel
                        Return 2
                    Case Types.Fire
                        Return 2
                    Case Types.Water
                        Return 1
                    Case Types.Grass
                        Return 0.5F
                    Case Types.Electric
                        Return 2
                    Case Types.Psychic
                        Return 1
                    Case Types.Ice
                        Return 1
                    Case Types.Dragon
                        Return 1
                    Case Types.Dark
                        Return 1
                    Case Else
                        Return 1
                End Select
            Case Types.Rock
                Select Case d._type
                    Case Types.Normal
                        Return 1
                    Case Types.Fighting
                        Return 0.5F
                    Case Types.Flying
                        Return 2
                    Case Types.Poison
                        Return 1
                    Case Types.Ground
                        Return 0.5F
                    Case Types.Rock
                        Return 1
                    Case Types.Bug
                        Return 2
                    Case Types.Ghost
                        Return 1
                    Case Types.Steel
                        Return 0.5F
                    Case Types.Fire
                        Return 2
                    Case Types.Water
                        Return 1
                    Case Types.Grass
                        Return 1
                    Case Types.Electric
                        Return 1
                    Case Types.Psychic
                        Return 1
                    Case Types.Ice
                        Return 2
                    Case Types.Dragon
                        Return 1
                    Case Types.Dark
                        Return 1
                    Case Else
                        Return 1
                End Select
            Case Types.Bug
                Select Case d._type
                    Case Types.Normal
                        Return 1
                    Case Types.Fighting
                        Return 0.5F
                    Case Types.Flying
                        Return 0.5F
                    Case Types.Poison
                        Return 0.5F
                    Case Types.Ground
                        Return 1
                    Case Types.Rock
                        Return 1
                    Case Types.Bug
                        Return 1
                    Case Types.Ghost
                        Return 0.5F
                    Case Types.Steel
                        Return 0.5F
                    Case Types.Fire
                        Return 0.5F
                    Case Types.Water
                        Return 1
                    Case Types.Grass
                        Return 2
                    Case Types.Electric
                        Return 1
                    Case Types.Psychic
                        Return 2
                    Case Types.Ice
                        Return 1
                    Case Types.Dragon
                        Return 1
                    Case Types.Dark
                        Return 2
                    Case Types.Fairy
                        Return 0.5F
                    Case Else
                        Return 1
                End Select
            Case Types.Ghost
                Select Case d._type
                    Case Types.Normal
                        Return 0
                    Case Types.Fighting
                        Return 1
                    Case Types.Flying
                        Return 1
                    Case Types.Poison
                        Return 1
                    Case Types.Ground
                        Return 1
                    Case Types.Rock
                        Return 1
                    Case Types.Bug
                        Return 1
                    Case Types.Ghost
                        Return 2
                    Case Types.Steel
                        Return 1
                    Case Types.Fire
                        Return 1
                    Case Types.Water
                        Return 1
                    Case Types.Grass
                        Return 1
                    Case Types.Electric
                        Return 1
                    Case Types.Psychic
                        Return 2
                    Case Types.Ice
                        Return 1
                    Case Types.Dragon
                        Return 1
                    Case Types.Dark
                        Return 0.5F
                    Case Else
                        Return 1
                End Select
            Case Types.Steel
                Select Case d._type
                    Case Types.Normal
                        Return 1
                    Case Types.Fighting
                        Return 1
                    Case Types.Flying
                        Return 1
                    Case Types.Poison
                        Return 1
                    Case Types.Ground
                        Return 1
                    Case Types.Rock
                        Return 2
                    Case Types.Bug
                        Return 1
                    Case Types.Ghost
                        Return 1
                    Case Types.Steel
                        Return 0.5F
                    Case Types.Fire
                        Return 0.5F
                    Case Types.Water
                        Return 0.5F
                    Case Types.Grass
                        Return 1
                    Case Types.Electric
                        Return 0.5F
                    Case Types.Psychic
                        Return 1
                    Case Types.Ice
                        Return 2
                    Case Types.Dragon
                        Return 1
                    Case Types.Dark
                        Return 1
                    Case Types.Fairy
                        Return 2
                    Case Else
                        Return 1
                End Select
            Case Types.Fire
                Select Case d._type
                    Case Types.Normal
                        Return 1
                    Case Types.Fighting
                        Return 1
                    Case Types.Flying
                        Return 1
                    Case Types.Poison
                        Return 1
                    Case Types.Ground
                        Return 1
                    Case Types.Rock
                        Return 0.5F
                    Case Types.Bug
                        Return 2
                    Case Types.Ghost
                        Return 1
                    Case Types.Steel
                        Return 2
                    Case Types.Fire
                        Return 0.5F
                    Case Types.Water
                        Return 0.5F
                    Case Types.Grass
                        Return 2
                    Case Types.Electric
                        Return 1
                    Case Types.Psychic
                        Return 1
                    Case Types.Ice
                        Return 2
                    Case Types.Dragon
                        Return 0.5F
                    Case Types.Dark
                        Return 1
                    Case Else
                        Return 1
                End Select
            Case Types.Water
                Select Case d._type
                    Case Types.Normal
                        Return 1
                    Case Types.Fighting
                        Return 1
                    Case Types.Flying
                        Return 1
                    Case Types.Poison
                        Return 1
                    Case Types.Ground
                        Return 2
                    Case Types.Rock
                        Return 2
                    Case Types.Bug
                        Return 1
                    Case Types.Ghost
                        Return 1
                    Case Types.Steel
                        Return 1
                    Case Types.Fire
                        Return 2
                    Case Types.Water
                        Return 0.5F
                    Case Types.Grass
                        Return 0.5F
                    Case Types.Electric
                        Return 1
                    Case Types.Psychic
                        Return 1
                    Case Types.Ice
                        Return 1
                    Case Types.Dragon
                        Return 0.5F
                    Case Types.Dark
                        Return 1
                    Case Else
                        Return 1
                End Select
            Case Types.Grass
                Select Case d._type
                    Case Types.Normal
                        Return 1
                    Case Types.Fighting
                        Return 1
                    Case Types.Flying
                        Return 0.5F
                    Case Types.Poison
                        Return 0.5F
                    Case Types.Ground
                        Return 2
                    Case Types.Rock
                        Return 2
                    Case Types.Bug
                        Return 0.5F
                    Case Types.Ghost
                        Return 1
                    Case Types.Steel
                        Return 0.5F
                    Case Types.Fire
                        Return 0.5F
                    Case Types.Water
                        Return 2
                    Case Types.Grass
                        Return 0.5F
                    Case Types.Electric
                        Return 1
                    Case Types.Psychic
                        Return 1
                    Case Types.Ice
                        Return 1
                    Case Types.Dragon
                        Return 0.5F
                    Case Types.Dark
                        Return 1
                    Case Else
                        Return 1
                End Select
            Case Types.Electric
                Select Case d._type
                    Case Types.Normal
                        Return 1
                    Case Types.Fighting
                        Return 1
                    Case Types.Flying
                        Return 2
                    Case Types.Poison
                        Return 1
                    Case Types.Ground
                        Return 0
                    Case Types.Rock
                        Return 1
                    Case Types.Bug
                        Return 1
                    Case Types.Ghost
                        Return 1
                    Case Types.Steel
                        Return 1
                    Case Types.Fire
                        Return 1
                    Case Types.Water
                        Return 2
                    Case Types.Grass
                        Return 0.5F
                    Case Types.Electric
                        Return 0.5F
                    Case Types.Psychic
                        Return 1
                    Case Types.Ice
                        Return 1
                    Case Types.Dragon
                        Return 0.5F
                    Case Types.Dark
                        Return 1
                    Case Else
                        Return 1
                End Select
            Case Types.Psychic
                Select Case d._type
                    Case Types.Normal
                        Return 1
                    Case Types.Fighting
                        Return 2
                    Case Types.Flying
                        Return 1
                    Case Types.Poison
                        Return 2
                    Case Types.Ground
                        Return 1
                    Case Types.Rock
                        Return 1
                    Case Types.Bug
                        Return 1
                    Case Types.Ghost
                        Return 1
                    Case Types.Steel
                        Return 0.5F
                    Case Types.Fire
                        Return 1
                    Case Types.Water
                        Return 1
                    Case Types.Grass
                        Return 1
                    Case Types.Electric
                        Return 1
                    Case Types.Psychic
                        Return 0.5F
                    Case Types.Ice
                        Return 1
                    Case Types.Dragon
                        Return 1
                    Case Types.Dark
                        Return 0
                    Case Else
                        Return 1
                End Select
            Case Types.Ice
                Select Case d._type
                    Case Types.Normal
                        Return 1
                    Case Types.Fighting
                        Return 1
                    Case Types.Flying
                        Return 2
                    Case Types.Poison
                        Return 1
                    Case Types.Ground
                        Return 2
                    Case Types.Rock
                        Return 1
                    Case Types.Bug
                        Return 1
                    Case Types.Ghost
                        Return 1
                    Case Types.Steel
                        Return 0.5F
                    Case Types.Fire
                        Return 0.5F
                    Case Types.Water
                        Return 0.5F
                    Case Types.Grass
                        Return 2
                    Case Types.Electric
                        Return 1
                    Case Types.Psychic
                        Return 1
                    Case Types.Ice
                        Return 0.5F
                    Case Types.Dragon
                        Return 2
                    Case Types.Dark
                        Return 1
                    Case Else
                        Return 1
                End Select
            Case Types.Dragon
                Select Case d._type
                    Case Types.Normal
                        Return 1
                    Case Types.Fighting
                        Return 1
                    Case Types.Flying
                        Return 1
                    Case Types.Poison
                        Return 1
                    Case Types.Ground
                        Return 1
                    Case Types.Rock
                        Return 1
                    Case Types.Bug
                        Return 1
                    Case Types.Ghost
                        Return 1
                    Case Types.Steel
                        Return 0.5F
                    Case Types.Fire
                        Return 1
                    Case Types.Water
                        Return 1
                    Case Types.Grass
                        Return 1
                    Case Types.Electric
                        Return 1
                    Case Types.Psychic
                        Return 1
                    Case Types.Ice
                        Return 1
                    Case Types.Dragon
                        Return 2
                    Case Types.Dark
                        Return 1
                    Case Types.Fairy
                        Return 0
                    Case Else
                        Return 1
                End Select
            Case Types.Dark
                Select Case d._type
                    Case Types.Normal
                        Return 1
                    Case Types.Fighting
                        Return 0.5F
                    Case Types.Flying
                        Return 1
                    Case Types.Poison
                        Return 1
                    Case Types.Ground
                        Return 1
                    Case Types.Rock
                        Return 1
                    Case Types.Bug
                        Return 1
                    Case Types.Ghost
                        Return 2
                    Case Types.Steel
                        Return 1
                    Case Types.Fire
                        Return 1
                    Case Types.Water
                        Return 1
                    Case Types.Grass
                        Return 1
                    Case Types.Electric
                        Return 1
                    Case Types.Psychic
                        Return 2
                    Case Types.Ice
                        Return 1
                    Case Types.Dragon
                        Return 1
                    Case Types.Dark
                        Return 0.5F
                    Case Types.Fairy
                        Return 0.5F
                    Case Else
                        Return 1
                End Select
            Case Types.Fairy
                Select Case d._type
                    Case Types.Fire
                        Return 0.5F
                    Case Types.Fighting
                        Return 2.0F
                    Case Types.Poison
                        Return 0.5F
                    Case Types.Dragon
                        Return 2.0F
                    Case Types.Dark
                        Return 2.0F
                    Case Types.Steel
                        Return 0.5F
                End Select
            Case Types.Shadow
                Select Case d._type
                    Case Types.Shadow
                        Return 0.5F
                    Case Else
                        Return 2
                End Select
            Case Else
                Return 1
        End Select

        Return 1
    End Function

    ''' <summary>
    ''' Returns the rectangle from the texture "GUI\Menus\Types" that represents the Type of this Element.
    ''' </summary>
    Public Function GetElementImage() As Rectangle
        Dim r As New Rectangle(0, 0, 0, 0)

        Select Case Me._type
            Case Types.Normal
                r = New Rectangle(0, 0, 48, 16)
            Case Types.Grass
                r = New Rectangle(0, 16, 48, 16)
            Case Types.Fire
                r = New Rectangle(0, 32, 48, 16)
            Case Types.Water
                r = New Rectangle(0, 48, 48, 16)
            Case Types.Electric
                r = New Rectangle(0, 64, 48, 16)
            Case Types.Ground
                r = New Rectangle(0, 80, 48, 16)
            Case Types.Rock
                r = New Rectangle(0, 96, 48, 16)
            Case Types.Ice
                r = New Rectangle(0, 112, 48, 16)
            Case Types.Steel
                r = New Rectangle(0, 128, 48, 16)
            Case Types.Bug
                r = New Rectangle(48, 0, 48, 16)
            Case Types.Fighting
                r = New Rectangle(48, 16, 48, 16)
            Case Types.Flying
                r = New Rectangle(48, 32, 48, 16)
            Case Types.Poison
                r = New Rectangle(48, 48, 48, 16)
            Case Types.Ghost
                r = New Rectangle(48, 64, 48, 16)
            Case Types.Dark
                r = New Rectangle(48, 80, 48, 16)
            Case Types.Psychic
                r = New Rectangle(48, 96, 48, 16)
            Case Types.Dragon
                r = New Rectangle(48, 128, 48, 16)
            Case Types.Fairy
                r = New Rectangle(96, 48, 48, 16)
            Case Types.Shadow
                r = New Rectangle(96, 64, 48, 16)
            Case Types.Blank
                r = New Rectangle(48, 112, 48, 16)
            Case Else
                r = New Rectangle(48, 112, 48, 16)
        End Select

        Return r
    End Function

    Public Overrides Function ToString() As String
        Select Case Me._type
            Case Types.Blank
                Return "Blank"
            Case Types.Bug
                Return "Bug"
            Case Types.Dark
                Return "Dark"
            Case Types.Dragon
                Return "Dragon"
            Case Types.Electric
                Return "Electric"
            Case Types.Fairy
                Return "Fairy"
            Case Types.Fighting
                Return "Fighting"
            Case Types.Fire
                Return "Fire"
            Case Types.Flying
                Return "Flying"
            Case Types.Ghost
                Return "Ghost"
            Case Types.Grass
                Return "Grass"
            Case Types.Ground
                Return "Ground"
            Case Types.Ice
                Return "Ice"
            Case Types.Normal
                Return "Normal"
            Case Types.Poison
                Return "Poison"
            Case Types.Psychic
                Return "Psychic"
            Case Types.Rock
                Return "Rock"
            Case Types.Shadow
                Return "Shadow"
            Case Types.Steel
                Return "Steel"
            Case Types.Water
                Return "Water"
            Case Else
                Return "Blank"
        End Select
    End Function

    Public Shared Operator =(ByVal Element1 As Element, ByVal Element2 As Element) As Boolean
        Return Element1._type = Element2._type
    End Operator

    Public Shared Operator <>(ByVal Element1 As Element, ByVal Element2 As Element) As Boolean
        Return Element1._type <> Element2._type
    End Operator

End Class
