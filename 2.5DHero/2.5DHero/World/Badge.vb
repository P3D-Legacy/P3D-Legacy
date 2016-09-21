''' <summary>
''' This class handles all actions regarding badge loading and displaying.
''' </summary>
Public Class Badge

    Public Enum HMMoves
        Surf
        Cut
        Strength
        Flash
        Fly
        Whirlpool
        Waterfall
        Ride
        Dive
        RockClimb
    End Enum

    Private Shared Badges As New List(Of BadgeDeclaration)

    ''' <summary>
    ''' This describes one badge loaded by a GameMode with ID, Name, Texture and Arguments
    ''' </summary>
    Private Class BadgeDeclaration

        Public ID As Integer = 0
        Public Name As String = ""
        Public LevelCap As Integer = -1
        Public HMs As New List(Of HMMoves)
        Public TextureRectangle As Rectangle = New Rectangle(0, 0, 50, 50)
        Public TexturePath As String = "GUI\Badges"
        Public Region As String = "Johto"

        Public Sub New(ByVal inputData As String)
            Dim data() As String = inputData.Split(CChar("|"))

            Me.ID = CInt(data(0))
            Me.Name = data(1)

            If data.Count > 2 Then
                For i = 2 To data.Count - 1
                    Dim argName As String = data(i).Remove(data(i).IndexOf("="))
                    Dim argData As String = data(i).Remove(0, data(i).IndexOf("=") + 1)

                    Select Case argName.ToLower()
                        Case "level"
                            Me.LevelCap = CInt(argData)
                        Case "hm"
                            Dim hms() As String = argData.Split(CChar(","))

                            For Each hm As String In hms
                                Select Case hm.ToLower()
                                    Case "surf"
                                        Me.HMs.Add(HMMoves.Surf)
                                    Case "cut"
                                        Me.HMs.Add(HMMoves.Cut)
                                    Case "strength"
                                        Me.HMs.Add(HMMoves.Strength)
                                    Case "flash"
                                        Me.HMs.Add(HMMoves.Flash)
                                    Case "fly"
                                        Me.HMs.Add(HMMoves.Fly)
                                    Case "whirlpool"
                                        Me.HMs.Add(HMMoves.Whirlpool)
                                    Case "waterfall"
                                        Me.HMs.Add(HMMoves.Waterfall)
                                    Case "ride"
                                        Me.HMs.Add(HMMoves.Ride)
                                    Case "dive"
                                        Me.HMs.Add(HMMoves.Dive)
                                    Case "rockclimb"
                                        Me.HMs.Add(HMMoves.RockClimb)
                                End Select
                            Next
                        Case "texture"
                            Dim texData() As String = argData.Split(CChar(","))
                            Me.TexturePath = texData(0)
                            Me.TextureRectangle = New Rectangle(CInt(texData(1)), CInt(texData(2)), CInt(texData(3)), CInt(texData(4)))
                        Case "region"
                            Me.Region = argData
                    End Select
                Next
            End If
        End Sub

    End Class

    ''' <summary>
    ''' Loads the badges. Only use after the GameMode got set.
    ''' </summary>
    Public Shared Sub Load()
        Badges.Clear()

        Dim file As String = GameModeManager.GetContentFilePath("Data\badges.dat")
        Security.FileValidation.CheckFileValid(file, False, "Badge.vb")
        Dim data() As String = System.IO.File.ReadAllLines(file)
        For Each line As String In data
            If line.Contains("|") = True Then
                Badges.Add(New BadgeDeclaration(line))
            End If
        Next
    End Sub

#Region "GetFunctions"

    ''' <summary>
    ''' Gets the badge name.
    ''' </summary>
    ''' <param name="ID">The ID of the badge.</param>
    Public Shared Function GetBadgeName(ByVal ID As Integer) As String
        For Each b As BadgeDeclaration In Badges
            If b.ID = ID Then
                Return b.Name
            End If
        Next
        Return "Plain"
    End Function

    ''' <summary>
    ''' Gets the badge texture.
    ''' </summary>
    ''' <param name="ID">The ID of the badge.</param>
    Public Shared Function GetBadgeTexture(ByVal ID As Integer) As Texture2D
        For Each b As BadgeDeclaration In Badges
            If b.ID = ID Then
                Return TextureManager.GetTexture(b.TexturePath, b.TextureRectangle, "")
            End If
        Next
        Return TextureManager.GetTexture("GUI\Badges", New Rectangle(0, 0, 50, 50), "")
    End Function

    ''' <summary>
    ''' Gets the highest level cap the player can use traded Pokémon on.
    ''' </summary>
    Public Shared Function GetLevelCap() As Integer
        Dim trainerBadges As List(Of Integer) = Core.Player.Badges
        Dim highestCap As Integer = 10
        For Each b As BadgeDeclaration In Badges
            If b.LevelCap > highestCap And trainerBadges.Contains(b.ID) = True Then
                highestCap = b.LevelCap
            End If
        Next
        Return highestCap
    End Function

    ''' <summary>
    ''' Checks if the player is able to perform a certain HM move.
    ''' </summary>
    ''' <param name="HM">The HM move the player tries to use.</param>
    Public Shared Function CanUseHMMove(ByVal HM As HMMoves) As Boolean
        Dim trainerBadges As List(Of Integer) = Core.Player.Badges
        For Each b As BadgeDeclaration In Badges
            If b.HMs.Contains(HM) = True And trainerBadges.Contains(b.ID) = True Or b.ID = 0 Then
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' Gets the region.
    ''' </summary>
    ''' <param name="index">The index of the region in the badges enumeration.</param>
    Public Shared Function GetRegion(ByVal index As Integer) As String
        Dim regions As New List(Of String)
        For Each b As BadgeDeclaration In Badges
            If regions.Any(Function(m As String) m.ToLowerInvariant() = b.Region.ToLowerInvariant()) Then
                regions.Add(b.Region)
            End If
        Next
        If regions.Count - 1 >= index Then
            Return regions(index)
        Else
            Return "Johto"
        End If
    End Function

    ''' <summary>
    ''' Gets the amount of badges in a certain region.
    ''' </summary>
    ''' <param name="region">The region to count the badges.</param>
    Public Shared Function GetBadgesCount(ByVal region As String) As Integer
        Dim c As Integer = 0
        For Each b As BadgeDeclaration In Badges
            If b.Region.ToLower() = region.ToLower() Then
                c += 1
            End If
        Next
        Return c
    End Function

    ''' <summary>
    ''' Returns the amount of regions that exists in total.
    ''' </summary>
    Public Shared Function GetRegionCount() As Integer
        Dim regions As New List(Of String)
        For Each b As BadgeDeclaration In Badges
            If regions.Any(Function(m As String) m.ToLowerInvariant() = b.Region.ToLowerInvariant()) Then
                regions.Add(b.Region)
            End If
        Next
        Return regions.Count
    End Function

    ''' <summary>
    ''' Gets the ID of a badge.
    ''' </summary>
    ''' <param name="region">The region this badge is from.</param>
    ''' <param name="index">The index of this badge.</param>
    Public Shared Function GetBadgeID(ByVal region As String, ByVal index As Integer) As Integer
        Dim cBadges As New List(Of BadgeDeclaration)
        For Each b As BadgeDeclaration In Badges
            If b.Region.ToLower() = region.ToLower() Then
                cBadges.Add(b)
            End If
        Next
        If cBadges.Count - 1 >= index Then
            Return cBadges(index).ID
        Else
            Return 1
        End If
    End Function

    ''' <summary>
    ''' Checks if the player has a certain badge.
    ''' </summary>
    ''' <param name="BadgeID">The Badge ID to check for.</param>
    Public Shared Function PlayerHasBadge(ByVal BadgeID As Integer) As Boolean
        Return Core.Player.Badges.Contains(BadgeID)
    End Function

#End Region

End Class
