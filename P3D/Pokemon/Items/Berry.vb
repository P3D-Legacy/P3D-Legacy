Namespace Items

    Public MustInherit Class Berry

        Inherits MedicineItem

        Public Enum Flavours
            Spicy
            Dry
            Sweet
            Bitter
            Sour
        End Enum

        Public PhaseTime As Integer

        Public Size As String
        Public Firmness As String
        Public BerryIndex As Integer
        Public minBerries As Integer
        Public maxBerries As Integer

        Public Spicy As Integer = 0
        Public Dry As Integer = 0
        Public Sweet As Integer = 0
        Public Bitter As Integer = 0
        Public Sour As Integer = 0

        Public WinterGrow As Integer = 0
        Public SpringGrow As Integer = 3
        Public SummerGrow As Integer = 2
        Public FallGrow As Integer = 1

        Public Type As Element.Types
        Public Power As Integer = 60

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property FlingDamage As Integer = 10
        Public Overrides ReadOnly Property ItemType As ItemTypes = ItemTypes.Plants
        Public Overrides ReadOnly Property SortValue As Integer
        Public Overrides ReadOnly Property Description As String

        Public Sub New(ByVal PhaseTime As Integer, ByVal Description As String, ByVal Size As String, ByVal Firmness As String, ByVal minBerries As Integer, ByVal maxBerries As Integer)
            SortValue = ID - 1999

            Me.PhaseTime = PhaseTime
            Me.Size = Size
            Me.Firmness = Firmness
            BerryIndex = Me.ID - 2000
            Me.minBerries = minBerries
            Me.maxBerries = maxBerries

            Dim x As Integer = BerryIndex * 128
            Dim y As Integer = 0
            While x >= 512
                x -= 512
                y += 32
            End While

            _Description = Description
            _textureSource = "Textures\Berries"
            _textureRectangle = New Rectangle(x, y, 32, 32)
        End Sub

        Public ReadOnly Property Flavour() As Flavours
            Get
                Dim returnFlavour As Flavours = Flavours.Spicy
                Dim highestFlavour As Integer = Spicy

                If Dry > highestFlavour Then
                    highestFlavour = Dry
                    returnFlavour = Flavours.Dry
                End If
                If Sweet > highestFlavour Then
                    highestFlavour = Sweet
                    returnFlavour = Flavours.Sweet
                End If
                If Bitter > highestFlavour Then
                    highestFlavour = Bitter
                    returnFlavour = Flavours.Bitter
                End If
                If Sour > highestFlavour Then
                    highestFlavour = Sour
                    returnFlavour = Flavours.Sour
                End If

                Return returnFlavour
            End Get
        End Property

        ''' <summary>
        ''' Returns if a Pokémon likes this berry based on its flavour.
        ''' </summary>
        ''' <param name="p">The Pokémon to test this berry for.</param>
        Public Function PokemonLikes(ByVal p As Pokemon) As Boolean
            Select Case p.Nature
                Case Pokemon.Natures.Lonely
                    If Flavour = Flavours.Spicy Then
                        Return True
                    ElseIf Flavour = Flavours.Sour Then
                        Return False
                    End If
                Case Pokemon.Natures.Adamant
                    If Flavour = Flavours.Spicy Then
                        Return True
                    ElseIf Flavour = Flavours.Dry Then
                        Return False
                    End If
                Case Pokemon.Natures.Naughty
                    If Flavour = Flavours.Spicy Then
                        Return True
                    ElseIf Flavour = Flavours.Bitter Then
                        Return False
                    End If
                Case Pokemon.Natures.Brave
                    If Flavour = Flavours.Spicy Then
                        Return True
                    ElseIf Flavour = Flavours.Sweet Then
                        Return False
                    End If
                Case Pokemon.Natures.Bold
                    If Flavour = Flavours.Sour Then
                        Return True
                    ElseIf Flavour = Flavours.Spicy Then
                        Return False
                    End If
                Case Pokemon.Natures.Impish
                    If Flavour = Flavours.Sour Then
                        Return True
                    ElseIf Flavour = Flavours.Dry Then
                        Return False
                    End If
                Case Pokemon.Natures.Lax
                    If Flavour = Flavours.Sour Then
                        Return True
                    ElseIf Flavour = Flavours.Bitter Then
                        Return False
                    End If
                Case Pokemon.Natures.Relaxed
                    If Flavour = Flavours.Sour Then
                        Return True
                    ElseIf Flavour = Flavours.Sweet Then
                        Return False
                    End If
                Case Pokemon.Natures.Modest
                    If Flavour = Flavours.Dry Then
                        Return True
                    ElseIf Flavour = Flavours.Spicy Then
                        Return False
                    End If
                Case Pokemon.Natures.Mild
                    If Flavour = Flavours.Dry Then
                        Return True
                    ElseIf Flavour = Flavours.Sour Then
                        Return False
                    End If
                Case Pokemon.Natures.Rash
                    If Flavour = Flavours.Dry Then
                        Return True
                    ElseIf Flavour = Flavours.Bitter Then
                        Return False
                    End If
                Case Pokemon.Natures.Quiet
                    If Flavour = Flavours.Dry Then
                        Return True
                    ElseIf Flavour = Flavours.Sweet Then
                        Return False
                    End If
                Case Pokemon.Natures.Calm
                    If Flavour = Flavours.Bitter Then
                        Return True
                    ElseIf Flavour = Flavours.Spicy Then
                        Return False
                    End If
                Case Pokemon.Natures.Gentle
                    If Flavour = Flavours.Bitter Then
                        Return True
                    ElseIf Flavour = Flavours.Sour Then
                        Return False
                    End If
                Case Pokemon.Natures.Careful
                    If Flavour = Flavours.Bitter Then
                        Return True
                    ElseIf Flavour = Flavours.Dry Then
                        Return False
                    End If
                Case Pokemon.Natures.Sassy
                    If Flavour = Flavours.Bitter Then
                        Return True
                    ElseIf Flavour = Flavours.Sweet Then
                        Return False
                    End If
                Case Pokemon.Natures.Timid
                    If Flavour = Flavours.Sweet Then
                        Return True
                    ElseIf Flavour = Flavours.Spicy Then
                        Return False
                    End If
                Case Pokemon.Natures.Hasty
                    If Flavour = Flavours.Sweet Then
                        Return True
                    ElseIf Flavour = Flavours.Sour Then
                        Return False
                    End If
                Case Pokemon.Natures.Jolly
                    If Flavour = Flavours.Sweet Then
                        Return True
                    ElseIf Flavour = Flavours.Dry Then
                        Return False
                    End If
                Case Pokemon.Natures.Naive
                    If Flavour = Flavours.Sweet Then
                        Return True
                    ElseIf Flavour = Flavours.Bitter Then
                        Return False
                    End If
                Case Else
                    Return True
            End Select
            Return True
        End Function

    End Class

End Namespace