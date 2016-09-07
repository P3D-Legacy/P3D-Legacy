Namespace Items

    ''' <summary>
    ''' This class enables content creators to create own items with own images, functions and stats.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class FileItem

        Inherits Item

        Private _isValid As Boolean = True
        Private _scriptBinding As String = ""

        ''' <summary>
        ''' Returns the item of the FileItem class.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Item() As Item
            Get
                Return Me
            End Get
        End Property

        ''' <summary>
        ''' Returns if the FileItem is generated in a valid state.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsValid() As Boolean
            Get
                Return Me._isValid
            End Get
        End Property

        ''' <summary>
        ''' This is the script binding that represents the script that gets executed once the item gets used.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ScriptBinding() As String
            Get
                Return Me._scriptBinding
            End Get
            Set(value As String)
                Me._scriptBinding = value
            End Set
        End Property

        ''' <summary>
        ''' Iniztializes a new FileItem class.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal data As String)
            data = data.Remove(0, 1)
            data = data.Remove(data.Length - 1, 1)

            Dim dataArr() As String = data.Split(CChar("|"))

            Dim ID As Integer = 0
            Dim Name As String = ""
            Dim Price As Integer = 0
            Dim ItemType As Item.ItemTypes = net.Pokemon3D.Game.Item.ItemTypes.Standard
            Dim CatchMultiplier As Single = 0.0F
            Dim SortValue As Integer = 0
            Dim Description As String = ""
            Dim TextureRectangle As Rectangle = Nothing
            Dim TextureSource As String = ""
            Dim ScriptBinding As String = ""

            If dataArr.Count = 11 Then
                If IsNumeric(dataArr(0)) = True Then
                    ID = CInt(dataArr(0))
                Else
                    Logger.Log(Logger.LogTypes.ErrorMessage, "FileItem.vb: The field for ""ID"" did not have the correct data type (int).")
                    Me._isValid = False
                End If

                If dataArr(1) <> "" Then
                    Name = dataArr(1)
                Else
                    Logger.Log(Logger.LogTypes.ErrorMessage, "FileItem.vb: The field for ""Name"" cannot be empty.")
                    Me._isValid = False
                End If

                If IsNumeric(dataArr(2)) = True Then
                    Price = CInt(dataArr(2))
                Else
                    Logger.Log(Logger.LogTypes.ErrorMessage, "FileItem.vb: The field for ""Price"" did not have the correct data type (int).")
                    Me._isValid = False
                End If

                Select Case dataArr(3).ToLower()
                    Case "standard"
                        ItemType = net.Pokemon3D.Game.Item.ItemTypes.Standard
                    Case "medicine"
                        ItemType = net.Pokemon3D.Game.Item.ItemTypes.Medicine
                    Case "plants"
                        ItemType = net.Pokemon3D.Game.Item.ItemTypes.Plants
                    Case "pokeballs", "pokéballs"
                        ItemType = net.Pokemon3D.Game.Item.ItemTypes.Pokéballs
                    Case "machines"
                        ItemType = net.Pokemon3D.Game.Item.ItemTypes.Machines
                    Case "keyitems", "keyitem"
                        ItemType = net.Pokemon3D.Game.Item.ItemTypes.KeyItems
                    Case "mail"
                        ItemType = net.Pokemon3D.Game.Item.ItemTypes.Mail
                    Case "battleitems"
                        ItemType = net.Pokemon3D.Game.Item.ItemTypes.BattleItems
                    Case Else
                        Logger.Log(Logger.LogTypes.ErrorMessage, "FileItem.vb: The field for ""ItemType"" did not contain an ItemType data type.")
                        Me._isValid = False
                End Select

                If IsNumeric(dataArr(4).Replace(".", GameController.DecSeparator)) = True Then
                    CatchMultiplier = CSng(dataArr(4).Replace(".", GameController.DecSeparator))
                Else
                    Logger.Log(Logger.LogTypes.ErrorMessage, "FileItem.vb: The field for ""CatchMultiplier"" did not have the correct data type (sng).")
                    Me._isValid = False
                End If

                If IsNumeric(dataArr(5)) = True Then
                    SortValue = CInt(dataArr(5))
                Else
                    Logger.Log(Logger.LogTypes.ErrorMessage, "FileItem.vb: The field for ""SortValue"" did not have the correct data type (int).")
                    Me._isValid = False
                End If

                If dataArr(6) <> "" Then
                    Description = dataArr(6)
                Else
                    Logger.Log(Logger.LogTypes.ErrorMessage, "FileItem.vb: The field for ""Description"" cannot be empty.")
                    Me._isValid = False
                End If

                If dataArr(7).CountSeperators(",") = 3 Then
                    Dim texData() As String = dataArr(7).Split(CChar(","))
                    If texData.ToList().IsNumericList() = True Then
                        TextureRectangle = New Rectangle(CInt(texData(0)), CInt(texData(1)), CInt(texData(2)), CInt(texData(3)))
                    Else
                        Logger.Log(Logger.LogTypes.ErrorMessage, "FileItem.vb: The field for ""TextureRectangle"" contains invalid data.")
                        Me._isValid = False
                    End If
                Else
                    Logger.Log(Logger.LogTypes.ErrorMessage, "FileItem.vb: The field for ""TextureRectangle"" contains invalid data.")
                    Me._isValid = False
                End If

                TextureSource = dataArr(8)

                If dataArr(9) <> "" Then
                    ScriptBinding = dataArr(9)
                Else
                    Logger.Log(Logger.LogTypes.ErrorMessage, "FileItem.vb: The field for ""ScriptBinding"" cannot be empty.")
                    Me._isValid = False
                End If

                If dataArr(10).CountSeperators(",") = 3 Then
                    Dim attData() As String = dataArr(10).Split(CChar(","))
                    If attData.ToList().IsBooleanicList() = True Then
                        Me._canBeUsed = CBool(attData(0))
                        Me._canBeTraded = CBool(attData(1))
                        Me._canBeHold = CBool(attData(2))
                        Me._isBall = CBool(attData(3))
                        If Me._isBall = True Then
                            Me._canBeUsedInBattle = True
                        End If
                    Else
                        Logger.Log(Logger.LogTypes.ErrorMessage, "FileItem.vb: The field for ""ItemAttributes"" contains invalid data.")
                        Me._isValid = False
                    End If
                Else
                    Logger.Log(Logger.LogTypes.ErrorMessage, "FileItem.vb: The field for ""ItemAttributes"" contains invalid data.")
                    Me._isValid = False
                End If
            Else
                Logger.Log(Logger.LogTypes.ErrorMessage, "FileItem.vb: The array length (" & dataArr.Count & ") was smaller or greater than expected (11).")
                Me._isValid = False
            End If

            If Me._isValid = True Then
                Me.Initialize(Name, Price, ItemType, ID, CatchMultiplier, SortValue, TextureRectangle, Description)
                Me._scriptBinding = ScriptBinding

                If TextureSource.ToLower() <> "items\itemsheet" And TextureSource <> "" Then
                    Me._texture = TextureManager.GetTexture(TextureSource, TextureRectangle, "")
                End If
            End If
        End Sub

        Public Overrides Sub Use()
            Dim s As Screen = Core.CurrentScreen
            While s.Identification <> Screen.Identifications.OverworldScreen And Not s.PreScreen Is Nothing
                s = s.PreScreen
            End While

            If s.Identification = Screen.Identifications.OverworldScreen Then
                Core.SetScreen(s)
                CType(s, OverworldScreen).ActionScript.StartScript(Me._scriptBinding, 0)
            Else
                Logger.Log(Logger.LogTypes.Warning, "FileItem.vb: The Item """ & Me.Name & """ created by the GameMode """ & GameModeManager.ActiveGameMode.Name & """ was used in a bad environment.")
            End If
        End Sub

    End Class

End Namespace