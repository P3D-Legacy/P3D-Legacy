''' <summary>
''' A class to handle entity lighting.
''' </summary>
Public Class Lighting

#Region "Enums"

#End Region

#Region "Fields and Constants"

#End Region

#Region "Properties"

#End Region

#Region "Delegates"

#End Region

#Region "Constructors"

#End Region

#Region "Methods"

    ''' <summary>
    ''' Updates the lighting values of a BasicEffect instance.
    ''' </summary>
    ''' <param name="refEffect">A reference to the BasicEffect that should receive the lighting update.</param>
    ''' <param name="ForceLighting">Checks, if the lighting update on the effect should be forced.</param>
    Public Shared Sub UpdateLighting(ByRef refEffect As BasicEffect, Optional ByVal ForceLighting As Boolean = False)
        If Core.GameOptions.LightingEnabled = True Or ForceLighting = True Then 'Only update the lighting if either the currently loaded level instance allows this, or it's getting forced.
            'Set default parameters:
            refEffect.LightingEnabled = True 'Enable lighting (gets disabled later, if not used)
            refEffect.PreferPerPixelLighting = True 'Yes. Please.
            refEffect.SpecularPower = 2000.0F

            'LightType results:
            '0 = Night
            '1 = Morning
            '2 = day
            '3 = evening
            'everything above = no lighting

            Select Case GetLightingType()
                Case 0 'Night
                    refEffect.AmbientLightColor = New Vector3(0.8F)

                    refEffect.DirectionalLight0.DiffuseColor = New Vector3(0.4F, 0.4F, 0.6F)
                    refEffect.DirectionalLight0.Direction = Vector3.Normalize(New Vector3(-1.0F, 0.0F, 1.0F))
                    refEffect.DirectionalLight0.SpecularColor = New Vector3(0.0F)
                    refEffect.DirectionalLight0.Enabled = True
                Case 1 'Morning
                    refEffect.AmbientLightColor = New Vector3(0.7F)

                    refEffect.DirectionalLight0.DiffuseColor = Color.Orange.ToVector3()
                    refEffect.DirectionalLight0.Direction = Vector3.Normalize(New Vector3(1.0F, -1.0F, 1.0F))
                    refEffect.DirectionalLight0.SpecularColor = New Vector3(0.0F)
                    refEffect.DirectionalLight0.Enabled = True
                Case 2 'Day
                    refEffect.AmbientLightColor = New Vector3(1.0F)

                    refEffect.DirectionalLight0.DiffuseColor = New Vector3(-0.3F)
                    refEffect.DirectionalLight0.Direction = Vector3.Normalize(New Vector3(1.0F, 1.0F, 1.0F))
                    refEffect.DirectionalLight0.SpecularColor = New Vector3(0.0F)
                    refEffect.DirectionalLight0.Enabled = True
                Case 3 'Evening
                    refEffect.AmbientLightColor = New Vector3(1.0F)

                    refEffect.DirectionalLight0.DiffuseColor = New Vector3(-0.45F)
                    refEffect.DirectionalLight0.Direction = Vector3.Normalize(New Vector3(1.0F, 0.0F, 1.0F))
                    refEffect.DirectionalLight0.SpecularColor = New Vector3(0.0F)
                    refEffect.DirectionalLight0.Enabled = True
                Case Else 'Disable lighting on the effect
                    refEffect.LightingEnabled = False
            End Select
        Else
            'Disable lighting if the effect isn't supposed to be lighted.
            refEffect.LightingEnabled = False
        End If
    End Sub

    Public Shared Function GetLightingType() As Integer
        Dim LightType As Integer = CInt(World.GetTime()) 'Determine default lighting type by the world time.

        'Level's lighttype values:
        '0 = get lighting from the current time of day
        '1 = disable lighting
        '2 = always night
        '3 = always morning
        '4 = always day
        '5 = always evening

        If Screen.Level.LightingType = 1 Then 'If the level lighting type is 1, disable lighting (set index to 99)
            LightType = 99
        End If
        If Screen.Level.LightingType > 1 And Screen.Level.LightingType < 6 Then 'If the level's lighting type is 2, 3, 4 or 5, set to respective LightType (set time of day)
            LightType = Screen.Level.LightingType - 2
        End If

        Return LightType
    End Function

    Public Shared Function GetLightingColor(ByVal LightType As Integer) As Vector3
        Select Case LightType
            Case 0
                Return New Vector3(0.4F, 0.4F, 0.6F)
            Case 1
                Return Color.Orange.ToVector3()
            Case 2
                Return New Vector3(-0.3F)
            Case 3
                Return New Vector3(-0.45F)
        End Select
    End Function

#End Region

End Class
