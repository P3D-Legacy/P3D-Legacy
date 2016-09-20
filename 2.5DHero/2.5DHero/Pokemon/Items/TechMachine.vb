Namespace Items

    Public MustInherit Class TechMachine

        Inherits Item

        Public Attack As BattleSystem.Attack
        Public IsTM As Boolean = True
        Public TechID As Integer = 0
        Public HiddenID As Integer = 0

        'Special tags:
        Public CanTeachAlways As Boolean = False
        Public CanTeachWhenFullyEvolved As Boolean = False
        Public CanTeachWhenGender As Boolean = False

        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeHold As Boolean = False
        Public Overrides ReadOnly Property CanBeTraded As Boolean = True
        Public Overrides ReadOnly Property CanBeTossed As Boolean = True
        Public Overrides ReadOnly Property SortValue As Integer
        Public Overrides ReadOnly Property Description As String
        Public Overrides ReadOnly Property ItemType As ItemTypes = ItemTypes.Machines
        Public Overrides ReadOnly Property PokeDollarPrice As Integer

        Public Sub New(ByVal IsTM As Boolean, ByVal Price As Integer, ByVal AttackID As Integer)
            Attack = BattleSystem.Attack.GetAttackByID(AttackID)
            Me.IsTM = IsTM
            TechID = ID - 190
            HiddenID = ID - 242
            PokeDollarPrice = Price

            If Me.IsTM = False Then
                _CanBeTraded = False
                _CanBeTossed = False

                'Sets the sortvalue to something very low so the HMs will always be in first place:
                _SortValue = -100000 + ID
            Else
                _SortValue = ID
            End If

            _Description = "Teaches """ & Attack.Name & """ to a Pokémon."

            SetTextureRectangle()
        End Sub

        Private Sub SetTextureRectangle()
            Dim r As New Rectangle(144, 168, 24, 24)

            Select Case Attack.Type.Type
                Case Element.Types.Blank, Element.Types.Normal
                    r = New Rectangle(144, 168, 24, 24)
                Case Element.Types.Bug
                    r = New Rectangle(24, 192, 24, 24)
                Case Element.Types.Dark
                    r = New Rectangle(384, 168, 24, 24)
                Case Element.Types.Dragon
                    r = New Rectangle(408, 168, 24, 24)
                Case Element.Types.Electric
                    r = New Rectangle(288, 168, 24, 24)
                Case Element.Types.Fairy
                    r = New Rectangle(72, 264, 24, 24)
                Case Element.Types.Fighting
                    r = New Rectangle(168, 168, 24, 24)
                Case Element.Types.Fire
                    r = New Rectangle(360, 168, 24, 24)
                Case Element.Types.Flying
                    r = New Rectangle(0, 192, 24, 24)
                Case Element.Types.Ghost
                    r = New Rectangle(480, 168, 24, 24)
                Case Element.Types.Grass
                    r = New Rectangle(336, 168, 24, 24)
                Case Element.Types.Ground
                    r = New Rectangle(456, 168, 24, 24)
                Case Element.Types.Ice
                    r = New Rectangle(312, 168, 24, 24)
                Case Element.Types.Poison
                    r = New Rectangle(264, 168, 24, 24)
                Case Element.Types.Psychic
                    r = New Rectangle(216, 168, 24, 24)
                Case Element.Types.Rock
                    r = New Rectangle(240, 168, 24, 24)
                Case Element.Types.Steel
                    r = New Rectangle(432, 168, 24, 24)
                Case Element.Types.Water
                    r = New Rectangle(192, 168, 24, 24)
            End Select

            _textureRectangle = r
        End Sub

        Public Overrides Sub Use()
            SoundManager.PlaySound("PC\pc_logon", False)
            SetScreen(New ChoosePokemonScreen(CurrentScreen, Me, AddressOf UseOnPokemon, "Teach " & Attack.Name, True))
            CType(CurrentScreen, ChoosePokemonScreen).SetupLearnAttack(Attack, 1, Me)
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)
            Dim a As BattleSystem.Attack = Attack
            Dim t As String = CanTeach(p)

            If t = "" Then
                If p.Attacks.Count = 4 Then
                    SetScreen(New LearnAttackScreen(CurrentScreen, p, a, ID))

                    Return True
                Else
                    If IsTM = True And Core.Player.DifficultyMode > 0 Then
                        Core.Player.Inventory.RemoveItem(ID, 1)
                    End If
                    p.Attacks.Add(BattleSystem.Attack.GetAttackByID(a.ID))

                    SoundManager.PlaySound("success_small", False)
                    Screen.TextBox.Show(p.GetDisplayName() & " learned~" & a.Name & "!", {}, False, False)
                    PlayerStatistics.Track("TMs/HMs used", 1)

                    Return True
                End If
            Else
                Screen.TextBox.Show(t, {}, False, False)

                Return False
            End If
        End Function

        Public Function CanTeach(ByVal p As Pokemon) As String
            If p.IsEgg() = True Then
                Return "Egg cannot learn~" & Attack.Name & "!"
            End If

            For Each knowAttack As BattleSystem.Attack In p.Attacks
                If knowAttack.ID = Attack.ID Then
                    Return p.GetDisplayName() & " already~knows " & Attack.Name & "."
                End If
            Next

            If p.Machines.Contains(Attack.ID) = True Then
                Return ""
            End If

            For Each learnAttack As BattleSystem.Attack In p.AttackLearns.Values
                If learnAttack.ID = Attack.ID Then
                    Return ""
                End If
            Next

            If CanTeachAlways = True Then
                If p.Machines.Count > 0 Then
                    Return ""
                End If
            End If

            If CanTeachWhenFullyEvolved = True Then
                If p.IsFullyEvolved() = True And p.Machines.Count > 0 Then
                    Return ""
                End If
            End If

            If CanTeachWhenGender = True Then
                If p.Gender <> Pokemon.Genders.Genderless And p.Machines.Count > 0 Then
                    Return ""
                End If
            End If

            If p.CanLearnAllMachines = True Then
                Return ""
            End If

            Return p.GetDisplayName() & " cannot learn~" & Attack.Name & "!"
        End Function

    End Class

End Namespace