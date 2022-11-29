''' <summary>
''' A class to handle entity lighting.
''' </summary>
Public Class Lighting

    Shared LightingColorTexture As Texture2D = TextureManager.GetTexture("SkyDomeResource\LightingColors")
    Shared FogColorTexture As Texture2D = TextureManager.GetTexture("SkyDomeResource\FogColors")

#Region "Methods"
    Public Shared Function GetEnvironmentColor(TextureType As Integer) As Vector3 '0 = Directional light, 1 = Ambient light, 2 = Fog color
        Dim ColorTexture As Texture2D
        Dim EnvironmentColor As Vector3 = Nothing

        Dim x As Integer = 0
        Dim y As Integer = 0

        Select Case TextureType
            Case 0
                x = Screen.Level.DayTime - 1
                If x >= 0 AndAlso x <= 3 Then
                    ColorTexture = LightingColorTexture
                    Dim ColorData(0) As Color
                    ColorTexture.GetData(0, New Rectangle(x, 0, 1, 1), ColorData, 0, 1)

                    Dim DarkOrBrightData(0) As Color
                    ColorTexture.GetData(0, New Rectangle(x, 1, 1, 1), DarkOrBrightData, 0, 1)
                    EnvironmentColor = ColorData(0).ToVector3
                    If DarkOrBrightData(0) = Color.Black Then
                        EnvironmentColor = New Vector3(0.0F) - EnvironmentColor
                    End If
                Else
                    EnvironmentColor = New Vector3(0.0F)
                End If
            Case 1
                x = Screen.Level.DayTime - 1
                If x >= 0 AndAlso x <= 3 Then
                    ColorTexture = LightingColorTexture

                    Dim ColorData(0) As Color
                    ColorTexture.GetData(0, New Rectangle(x, 2, 1, 1), ColorData, 0, 1)

                    Dim DarkOrBrightData(0) As Color
                    ColorTexture.GetData(0, New Rectangle(x, 3, 1, 1), DarkOrBrightData, 0, 1)

                    EnvironmentColor = ColorData(0).ToVector3
                    If DarkOrBrightData(0) = Color.Black Then
                        EnvironmentColor = New Vector3(0.0F) - EnvironmentColor
                    End If

                Else
                    EnvironmentColor = New Vector3(0.0F)
                End If
            Case 2
                ColorTexture = FogColorTexture
                Select Case Screen.Level.EnvironmentType
                    Case 0
                        x = GetLightingType()
                        If x = 99 Then
                            x = Screen.Level.DayTime - 1
                        End If
                        If x > 2 Then
                            x = 0
                            y += 1
                        End If
                    Case 1
                        x = 1
                        y = 1
                    Case 2
                        x = 2
                        y = 1
                    Case 3
                        x = 0
                        y = 2
                    Case 4
                        x = 1
                        y = 2
                    Case 5
                        x = 2
                        y = 2
                End Select
                Dim ColorData(0) As Color
                ColorTexture.GetData(0, New Rectangle(x, y, 1, 1), ColorData, 0, 1)
                EnvironmentColor = ColorData(0).ToVector3
        End Select
        Return EnvironmentColor
    End Function
    ''' <summary>
    ''' Updates the lighting values of a BasicEffect instance.
    ''' </summary>
    ''' <param name="refEffect">A reference to the BasicEffect that should receive the lighting update.</param>
    ''' <param name="ForceLighting">Checks, if the lighting update on the effect should be forced.</param>
    Public Shared Sub UpdateLighting(ByRef refEffect As BasicEffect, Optional ByVal ForceLighting As Boolean = False)
        If Core.GameOptions.LightingEnabled = True OrElse ForceLighting = True Then ' Only update the lighting if either the currently loaded level instance allows this, or it's getting forced.
            ' Set default parameters:
            refEffect.LightingEnabled = True ' Enable lighting (gets disabled later, if not used)
            refEffect.PreferPerPixelLighting = True ' Yes. Please.
            refEffect.SpecularPower = 2000.0F

            ' LightType results:
            ' 0 = Night
            ' 1 = Morning
            ' 2 = Day
            ' 3 = Evening
            ' Anything higher than 3 = No Lighting

            Select Case GetLightingType()

                Case 0 ' Night
                    refEffect.AmbientLightColor = GetEnvironmentColor(1)

                    refEffect.DirectionalLight0.DiffuseColor = GetEnvironmentColor(0)
                    refEffect.DirectionalLight0.Direction = Vector3.Normalize(New Vector3(1.0F, 1.0F, -1.0F))
                    refEffect.DirectionalLight0.SpecularColor = New Vector3(0.0F)
                    refEffect.DirectionalLight0.Enabled = True
                Case 1 ' Morning
                    refEffect.AmbientLightColor = GetEnvironmentColor(1)

                    refEffect.DirectionalLight0.DiffuseColor = GetEnvironmentColor(0)
                    refEffect.DirectionalLight0.Direction = Vector3.Normalize(New Vector3(-1.0F, 0.0F, 1.0F))
                    refEffect.DirectionalLight0.SpecularColor = New Vector3(0.0F)
                    refEffect.DirectionalLight0.Enabled = True
                Case 2 ' Day
                    refEffect.AmbientLightColor = GetEnvironmentColor(1)

                    refEffect.DirectionalLight0.DiffuseColor = GetEnvironmentColor(0)
                    refEffect.DirectionalLight0.Direction = Vector3.Normalize(New Vector3(-1.0F, 0.0F, 1.0F))
                    refEffect.DirectionalLight0.SpecularColor = New Vector3(0.0F)
                    refEffect.DirectionalLight0.Enabled = True
                Case 3 ' Evening
                    refEffect.AmbientLightColor = GetEnvironmentColor(1)

                    refEffect.DirectionalLight0.DiffuseColor = GetEnvironmentColor(0)
                    refEffect.DirectionalLight0.Direction = Vector3.Normalize(New Vector3(1.0F, 1.0F, -1.0F))
                    refEffect.DirectionalLight0.SpecularColor = New Vector3(0.0F)
                    refEffect.DirectionalLight0.Enabled = True
                Case Else  'Disable lighting on the effect
                    refEffect.LightingEnabled = False
            End Select
        Else
            ' Disable lighting if the effect isn't supposed to have light.
            refEffect.LightingEnabled = False
        End If
    End Sub

    Public Shared Function GetLightingType() As Integer
        Dim LightType As Integer = CInt(World.GetTime()) ' Determine default lighting type by the world time.

        ' Level's lighttype values:
        ' 0 = Get lighting from the current time of day.
        ' 1 = Disable lighting
        ' 2 = Always Night
        ' 3 = Always Morning
        ' 4 = Always Day
        ' 5 = Always Evening

        If Screen.Level.EnvironmentType = World.EnvironmentTypes.Outside Then
            Select Case Screen.Level.DayTime
                Case 1
                    LightType = 0
                Case 2
                    LightType = 1
                Case 3
                    LightType = 2
                Case 4
                    LightType = 3
            End Select
        End If
        If Screen.Level.LightingType = 1 Then ' If the level lighting type is 1, disable lighting (set index to 99).
            LightType = 99
        End If
        If Screen.Level.LightingType > 1 AndAlso Screen.Level.LightingType < 6 Then ' If the level's lighting type is 2, 3, 4 or 5, set to the respective LightType (set time of day).
            LightType = Screen.Level.LightingType - 2
        End If
        If Screen.Level.LightingType = 6 AndAlso Screen.Level.EnvironmentType = 1 OrElse Screen.Level.LightingType > 6 Then
            LightType = 99
        End If

        Return LightType
    End Function

#End Region

End Class