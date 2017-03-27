Namespace GameModes.Maps.EntityProperties

    Module EntityPropertyFactoryModule

#Region "Property Name section"

        ' /// All constants in this section have to be lower case as they get compared case insensitive. \\\

        'This is defined here because it's used in multiple properties.
        'Do not define an extra class for this, it's using a normal DataStorage.
        Public Const PROPERTY_NAME_SPEED As String = "speed"

        Public Const PROPERTY_NAME_FACECAMERA As String = "facecamera"

        Public Const PROPERTY_NAME_STAIRS As String = "isstairs"
        Public Const PROPERTY_NAME_SIGN As String = "issign"
        Public Const PROPERTY_NAME_WARP As String = "iswarp"
        Public Const PROPERTY_NAME_MOVABLE As String = "ismovable"
        Public Const PROPERTY_NAME_STEP As String = "isstep"
        Public Const PROPERTY_NAME_CUTTREE As String = "iscuttree"
        Public Const PROPERTY_NAME_WATER As String = "iswater"
        Public Const PROPERTY_NAME_GRASS As String = "isgrass"
        Public Const PROPERTY_NAME_LOAMYSOIL As String = "isloamysoil"
        Public Const PROPERTY_NAME_ITEM As String = "isitem"
        Public Const PROPERTY_NAME_SCRIPTTRIGGER As String = "isscripttrigger"
        Public Const PROPERTY_NAME_SPINNING As String = "isspinning"
        Public Const PROPERTY_NAME_APRICORN As String = "isapricorn"
        Public Const PROPERTY_NAME_HEADBUTTTREE As String = "isheadbutttree"
        Public Const PROPERTY_NAME_SMASHROCK As String = "issmashrock"
        Public Const PROPERTY_NAME_STRENGTHROCK As String = "isstrengthrock"
        Public Const PROPERTY_NAME_WATERFALL As String = "iswaterfall"
        Public Const PROPERTY_NAME_WHIRLPOOL As String = "iswhirlpool"
        Public Const PROPERTY_NAME_STRENGTHTRIGGER As String = "isstrengthtrigger"
        Public Const PROPERTY_NAME_ROTATIONTILE As String = "isrotationtile"
        Public Const PROPERTY_NAME_DIVETILE As String = "isdivetile"
        Public Const PROPERTY_NAME_ROCKCLIMB As String = "isrockclimb"

#End Region

        ''' <summary>
        ''' A struct to easily move constructor params around.
        ''' </summary>
        Public Structure EntityPropertyDataCreationStruct

            Public Name As String
            Public Data As String
            Public Parent As Entity

        End Structure

        ''' <summary>
        ''' A factory method to create entity properties.
        ''' </summary>
        Public Function EntityPropertyFactory(ByVal parent As Entity, ByVal dataModel As DataModel.Json.Game.EntityModel.PropertyDataModel) As EntityProperty

            Dim prop As EntityProperty
            Dim params As New EntityPropertyDataCreationStruct() With
                {
                    .Parent = parent,
                    .Data = dataModel.Value,
                    .Name = dataModel.Name
                }

            Select Case dataModel.Name.ToLower(System.Globalization.CultureInfo.InvariantCulture)
                Case PROPERTY_NAME_MOVABLE
                    prop = New MovableEntityProperty(params)
                Case PROPERTY_NAME_FACECAMERA
                    prop = New FaceCameraEntityProperty(params)
                Case Else
                    prop = New DataStorageEntityProperty(params)
            End Select

            Return prop

        End Function

    End Module

End Namespace
