Namespace BattleSystem

    Public Class ToggleEntityQueryObject

        Inherits QueryObject

#Region "PVPData"
        Dim own As Boolean = False
#End Region

        Public Enum BattleEntities
            OwnPokemon = 0
            OppPokemon = 1
        End Enum

        Dim _entity As BattleEntities = BattleEntities.OwnPokemon
        Dim _done As Boolean = False
        Dim _toggleMode As Integer = 0
        Dim _newTexture As String = ""
        Dim ChangeType As Integer = 0

        Public Sub New(ByVal own As Boolean, ByVal newModel As String, ByVal ownModelID As Integer, ownNPCID As Integer, ByVal oppModelID As Integer, ByVal oppNPCID As Integer)
            MyBase.New(QueryTypes.ToggleEntity)

            Me.own = own
            If own = True Then
                Me._entity = BattleEntities.OwnPokemon
            Else
                Me._entity = BattleEntities.OppPokemon
            End If

            Me._newTexture = newModel
            Me.ChangeType = 2

            SetupIDs(ownModelID, ownNPCID, oppModelID, oppNPCID)
        End Sub

        Public Sub New(ByVal own As Boolean, ByVal Entity As BattleEntities, ByVal newTexture As String, ByVal ownModelID As Integer, ownNPCID As Integer, ByVal oppModelID As Integer, ByVal oppNPCID As Integer)
            MyBase.New(QueryTypes.ToggleEntity)

            Me._entity = Entity
            If own = False Then
                Me._entity = CType((Not CBool(CInt(Me._entity))).ToNumberString(), BattleEntities)
            End If

            Me.ChangeType = 1
            Me._newTexture = newTexture

            SetupIDs(ownModelID, ownNPCID, oppModelID, oppNPCID)
        End Sub

        Public Sub New(ByVal own As Boolean, ByVal Entity As BattleEntities, ByVal toggleMode As Integer, ByVal ownModelID As Integer, ownNPCID As Integer, ByVal oppModelID As Integer, ByVal oppNPCID As Integer)
            MyBase.New(QueryTypes.ToggleEntity)

            Me._entity = Entity
            If own = False Then
                Me._entity = CType((Not CBool(CInt(Me._entity))).ToNumberString(), BattleEntities)
            End If

            Me.ChangeType = 0
            Me._toggleMode = toggleMode

            SetupIDs(ownModelID, ownNPCID, oppModelID, oppNPCID)
        End Sub

        Private changedIDs As Boolean = False
        Private ownModelID As Integer = -1
        Private ownNPCID As Integer = -1
        Private oppModelID As Integer = -1
        Private oppNPCID As Integer = -1

        Private Sub SetupIDs(ByVal ownModelID As Integer, ownNPCID As Integer, ByVal oppModelID As Integer, ByVal oppNPCID As Integer)
            Me.ownModelID = ownModelID
            Me.ownNPCID = ownNPCID
            Me.oppModelID = oppModelID
            Me.oppNPCID = oppNPCID
        End Sub

        Public Overrides Sub Update(BV2Screen As BattleScreen)
            If changedIDs = False Then
                changedIDs = True

                If Me.ownModelID > -1 Then
                    BV2Screen.OwnPokemonModel.ID = ownModelID
                End If
                If Me.ownNPCID > -1 Then
                    BV2Screen.OwnPokemonNPC.ID = ownNPCID
                End If
                If Me.oppModelID > -1 Then
                    BV2Screen.OppPokemonModel.ID = oppModelID
                End If
                If Me.oppNPCID > -1 Then
                    BV2Screen.OppPokemonNPC.ID = oppNPCID
                End If
            End If

            Select Case Me.ChangeType
                Case 0
                    Select Case Me._entity
                        Case BattleEntities.OwnPokemon
                            If BV2Screen.OwnPokemonNPC.ID = 1 Then
                                BV2Screen.OwnPokemonNPC.Visible = GetVisible(BV2Screen.OwnPokemonNPC.Visible)
                                BV2Screen.OwnPokemonModel.Visible = False
                            Else
                                BV2Screen.OwnPokemonModel.Visible = GetVisible(BV2Screen.OwnPokemonModel.Visible)
                                BV2Screen.OwnPokemonNPC.Visible = False
                            End If
                        Case BattleEntities.OppPokemon
                            If BV2Screen.OppPokemonNPC.ID = 1 Then
                                BV2Screen.OppPokemonNPC.Visible = GetVisible(BV2Screen.OppPokemonNPC.Visible)
                                BV2Screen.OppPokemonModel.Visible = False
                            Else
                                BV2Screen.OppPokemonModel.Visible = GetVisible(BV2Screen.OppPokemonModel.Visible)
                                BV2Screen.OppPokemonNPC.Visible = False
                            End If
                    End Select
                Case 1
                    Select Case Me._entity
                        Case BattleEntities.OwnPokemon
                            BV2Screen.OwnPokemonNPC.SetupSprite(_newTexture, "", False)
                        Case BattleEntities.OppPokemon
                            BV2Screen.OppPokemonNPC.SetupSprite(_newTexture, "", False)
                    End Select
                Case 2
                    Select Case Me._entity
                        Case BattleEntities.OwnPokemon
                            BV2Screen.OwnPokemonModel.LoadModel(_newTexture)
                        Case BattleEntities.OppPokemon
                            BV2Screen.OppPokemonModel.LoadModel(_newTexture)
                    End Select
            End Select
            _done = True
        End Sub

        Private Function GetVisible(ByVal input As Boolean) As Boolean
            Dim output As Boolean = input

            Select Case Me._toggleMode
                Case 0
                    output = Not output
                Case 1
                    output = True
                Case 2
                    output = False
            End Select

            Return output
        End Function

        Public Overrides ReadOnly Property IsReady As Boolean
            Get
                Return _done
            End Get
        End Property

        Public Overrides Function NeedForPVPData() As Boolean
            Return True
        End Function

        Public Shared Shadows Function FromString(ByVal input As String) As QueryObject
            Dim d() As String = input.Split(CChar("|"))

            Select Case CInt(d(0))
                Case 0
                    Return New ToggleEntityQueryObject(CBool(d(1)), CType(CInt(d(2)), BattleEntities), CInt(d(3)), CInt(d(4)), CInt(d(5)), CInt(d(6)), CInt(d(7)))
                Case 1
                    Return New ToggleEntityQueryObject(CBool(d(1)), CType(CInt(d(2)), BattleEntities), CStr(d(3)), CInt(d(4)), CInt(d(5)), CInt(d(6)), CInt(d(7)))
                Case 2
                    Return New ToggleEntityQueryObject(CBool(d(1)), CStr(d(2)), CInt(d(3)), CInt(d(4)), CInt(d(5)), CInt(d(6)))
            End Select

            Return Nothing
        End Function

        Public Overrides Function ToString() As String
            Dim s As String = Me.ChangeType.ToString() & "|"

            Select Case Me.ChangeType
                Case 0
                    s &= "0|" & CInt(Me._entity).ToString() & "|" & Me._toggleMode.ToString() & "|" & Me.ownModelID.ToString() & "|" & Me.ownNPCID.ToString() & "|" & Me.oppModelID.ToString() & "|" & Me.oppNPCID.ToString()
                Case 1
                    s &= "0|" & CInt(Me._entity).ToString() & "|" & Me._newTexture & "|" & Me.ownModelID.ToString() & "|" & Me.ownNPCID.ToString() & "|" & Me.oppModelID.ToString() & "|" & Me.oppNPCID.ToString()
                Case 2
                    s &= (Not Me.own).ToNumberString() & "|" & Me._newTexture & "|" & Me.ownModelID.ToString() & "|" & Me.ownNPCID.ToString() & "|" & Me.oppModelID.ToString() & "|" & Me.oppNPCID.ToString()
            End Select

            Return "{TOGGLEENTITY|" & s & "}"
        End Function

    End Class

End Namespace