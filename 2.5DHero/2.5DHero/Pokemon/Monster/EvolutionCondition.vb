Public Class EvolutionCondition

    Public Enum ConditionTypes
        Level
        Friendship
        Item
        HoldItem
        Place
        Trade
        Gender
        AtkDef
        DefAtk
        DefEqualsAtk
        Move
        DayTime
        InParty
        InPartyType
        Weather
    End Enum

    Public Structure Condition
        Dim ConditionType As ConditionTypes
        Dim Argument As String
    End Structure

    Public Trigger As EvolutionTrigger
    Public Evolution As Integer = 0
    Public Conditions As New List(Of Condition)

    Public Sub SetEvolution(ByVal evolution As Integer)
        Me.Evolution = evolution
    End Sub

    Public Sub AddCondition(ByVal type As String, ByVal arg As String)
        Dim c As New Condition
        c.Argument = arg

        Select Case type.ToLower()
            Case "level"
                c.ConditionType = ConditionTypes.Level
            Case "item"
                c.ConditionType = ConditionTypes.Item
            Case "holditem"
                c.ConditionType = ConditionTypes.HoldItem
            Case "location", "place"
                c.ConditionType = ConditionTypes.Place
            Case "friendship"
                c.ConditionType = ConditionTypes.Friendship
            Case "trade"
                c.ConditionType = ConditionTypes.Trade
            Case "move"
                c.ConditionType = ConditionTypes.Move
            Case "gender"
                c.ConditionType = ConditionTypes.Gender
            Case "atkdef"
                c.ConditionType = ConditionTypes.AtkDef
            Case "defatk"
                c.ConditionType = ConditionTypes.DefAtk
            Case "defequalsatk"
                c.ConditionType = ConditionTypes.DefEqualsAtk
            Case "daytime"
                c.ConditionType = ConditionTypes.DayTime
            Case "inparty"
                c.ConditionType = ConditionTypes.InParty
            Case "inpartytype"
                c.ConditionType = ConditionTypes.InPartyType
            Case "weather"
                c.ConditionType = ConditionTypes.Weather
        End Select
        Me.Conditions.Add(c)
    End Sub

    Public Sub SetTrigger(ByVal trigger As String)
        Select Case trigger.ToLower()
            Case "none", ""
                Me.Trigger = EvolutionTrigger.None
            Case "level", "levelup"
                Me.Trigger = EvolutionTrigger.LevelUp
            Case "trade", "trading"
                Me.Trigger = EvolutionTrigger.Trading
            Case "item", "itemuse"
                Me.Trigger = EvolutionTrigger.ItemUse
        End Select
    End Sub

    Public ReadOnly Property Count() As Integer
        Get
            Return Conditions.Count
        End Get
    End Property

    Public Enum EvolutionTrigger
        None
        LevelUp
        Trading
        ItemUse
    End Enum

    ''' <summary>
    ''' Returns the evolution of a Pokémon. Returns 0 if not successful
    ''' </summary>
    ''' <param name="p">The Pokémon to get the evolution from.</param>
    ''' <param name="trigger">The trigger that triggered the evolution.</param>
    ''' <param name="arg">An argument (for example Item ID)</param>
    Public Shared Function EvolutionNumber(ByVal p As Pokemon, ByVal trigger As EvolutionTrigger, ByVal arg As String) As Integer
        Dim e As EvolutionCondition = GetEvolutionCondition(p, trigger, arg)
        If e Is Nothing Then
            Return 0
        Else
            Return e.Evolution
        End If
    End Function

    Public Shared Function GetEvolutionCondition(ByVal p As Pokemon, ByVal trigger As EvolutionTrigger, ByVal arg As String) As EvolutionCondition
        If trigger = EvolutionTrigger.LevelUp Or trigger = EvolutionTrigger.Trading Then
            If Not p.Item Is Nothing Then
                If p.Item.ID = 112 Then
                    Return Nothing
                End If
            End If
        End If

        Dim possibleEvolutions As New List(Of EvolutionCondition)

        For Each e As EvolutionCondition In p.EvolutionConditions
            Dim canEvolve As Boolean = True

            If trigger <> e.Trigger Then
                canEvolve = False
            End If

            If canEvolve = True Then
                For Each c As Condition In e.Conditions
                    Select Case c.ConditionType
                        Case ConditionTypes.AtkDef
                            If p.Attack <= p.Defense Then
                                canEvolve = False
                            End If
                        Case ConditionTypes.DayTime
                            Dim daytimes As List(Of String) = c.Argument.Split(CChar(";")).ToList()

                            If daytimes.Contains(CStr(CInt(World.GetTime()))) = False Then
                                canEvolve = False
                            End If
                        Case ConditionTypes.DefAtk
                            If p.Defense <= p.Attack Then
                                canEvolve = False
                            End If
                        Case ConditionTypes.DefEqualsAtk
                            If p.Attack <> p.Defense Then
                                canEvolve = False
                            End If
                        Case ConditionTypes.Friendship
                            If p.Friendship < CInt(c.Argument) Then
                                canEvolve = False
                            End If
                        Case ConditionTypes.Gender
                            If CInt(p.Gender) <> CInt(c.Argument) Then
                                canEvolve = False
                            End If
                        Case ConditionTypes.HoldItem
                            If p.Item Is Nothing Then
                                canEvolve = False
                            Else
                                If p.Item.ID <> CInt(c.Argument) Then
                                    canEvolve = False
                                    'ElseIf c.Trigger = EvolutionTrigger.Trading Then
                                    'REMOVE HELD ITEM CHECK
                                End If
                            End If
                        Case ConditionTypes.InParty
                            Dim isInParty As Boolean = False
                            For Each pokemon As Pokemon In Core.Player.Pokemons
                                If pokemon.Number = CInt(c.Argument) Then
                                    isInParty = True
                                    Exit For
                                End If
                            Next
                            If isInParty = False Then
                                canEvolve = False
                            End If
                        Case ConditionTypes.InPartyType
                            Dim isInParty As Boolean = False
                            For Each pokemon As Pokemon In Core.Player.Pokemons
                                If pokemon.IsType(New Element(c.Argument).Type) = True Then
                                    isInParty = True
                                    Exit For
                                End If
                            Next
                            If isInParty = False Then
                                canEvolve = False
                            End If
                        Case ConditionTypes.Item
                            If CInt(arg) <> CInt(c.Argument) Then
                                canEvolve = False
                            End If
                        Case ConditionTypes.Level
                            If p.Level < CInt(c.Argument) Then
                                canEvolve = False
                            End If
                        Case ConditionTypes.Move
                            Dim hasattack As Boolean = False
                            For Each a As BattleSystem.Attack In p.Attacks
                                If a.ID = CInt(c.Argument) Then
                                    hasattack = True
                                    Exit For
                                End If
                            Next
                            If hasattack = False Then
                                canEvolve = False
                            End If
                        Case ConditionTypes.Place
                            If Screen.Level.MapName.ToLower() <> c.Argument.ToLower() Then
                                canEvolve = False
                            End If
                        Case ConditionTypes.Trade
                            If StringHelper.IsNumeric(c.Argument) Then
                                If CInt(c.Argument) > 0 Then
                                    If CInt(c.Argument) <> CInt(arg) Then
                                        canEvolve = False
                                    End If
                                End If
                            End If
                        Case ConditionTypes.Weather
                            If World.GetCurrentRegionWeather().ToString.ToLower <> c.Argument.ToLower Then
                                canEvolve = False
                            End If
                    End Select
                Next
            End If

            If canEvolve = True Then
                possibleEvolutions.Add(e)
            End If
        Next

        'Assuming there is never more than one possible evolution for trading + held item
        If possibleEvolutions.Count > 0 Then
            Return possibleEvolutions(Core.Random.Next(0, possibleEvolutions.Count))
        End If

        Return Nothing
    End Function

End Class
