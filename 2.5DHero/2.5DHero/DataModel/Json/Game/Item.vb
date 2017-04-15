Imports System.Runtime.Serialization

Namespace DataModel.Json.Game

    ''' <summary>
    ''' The data model of an item.
    ''' </summary>
    <DataContract>
    Public Class ItemModel

        Inherits JsonDataModel

        <DataMember>
        Public Name As String

        <DataMember>
        Public PluralName As String

        <DataMember>
        Public Price As Integer

        <DataMember>
        Public Id As Integer

        <DataMember>
        Public Texture As TextureSourceModel

        <DataMember>
        Public Description As String

        <DataMember>
        Public ScriptBinding As String

        <DataMember>
        Public InventoryDefinition As InventoryDefinitionModel

        <DataMember>
        Public Usage As ItemUsageModel

        <DataMember>
        Public Classification As ItemClassificationModel

        <DataMember>
        Public Berry As BerryModel

        <DataMember>
        Public Megastone As MegastoneModel

        <DataMember>
        Public Plate As PlateModel

        <DataMember>
        Public TechnicalMachine As TechnicalMachineModel

        <DataContract>
        Public Class TechnicalMachineModel

            <DataMember>
            Public MoveId As Integer

            <DataMember>
            Public IsTM As Boolean

        End Class

        <DataContract>
        Public Class PlateModel

            <DataMember(Name:="Type")>
            Private TypeStr As String

            Public Property Type() As Element
                Get
                    Return New Element(TypeStr)
                End Get
                Set(value As Element)
                    TypeStr = value.ToString()
                End Set
            End Property

        End Class

        <DataContract>
        Public Class MegastoneModel

            <DataMember>
            Public MegaPokemonNumber As Integer

        End Class

        <DataContract>
        Public Class BerryModel

            Inherits JsonDataModel

            <DataMember>
            Public PhaseTime As Integer

            <DataMember>
            Public Size As String

            <DataMember>
            Public Firmness As String

            <DataMember>
            Public MinBerries As Integer

            <DataMember>
            Public MaxBerries As Integer

            <DataMember(Name:="Flavour")>
            Private FlavourStr As String

            Public Property Flavour() As Items.Berry.Flavours
                Get
                    Return ConvertStringToEnum(Of Items.Berry.Flavours)(FlavourStr)
                End Get
                Set(value As Items.Berry.Flavours)
                    FlavourStr = value.ToString()
                End Set
            End Property

            <DataMember(Name:="Type")>
            Private TypeStr As String

            Public Property Type() As Element
                Get
                    Return New Element(TypeStr)
                End Get
                Set(value As Element)
                    TypeStr = value.ToString()
                End Set
            End Property

            <DataMember>
            Public Power As Integer

        End Class

        <DataContract>
        Public Class ItemClassificationModel

            <DataMember>
            Public IsBall As Boolean

            <DataMember>
            Public IsBerry As Boolean

            <DataMember>
            Public IsHealingItem As Boolean

            <DataMember>
            Public IsMail As Boolean

            <DataMember>
            Public IsMegastone As Boolean

            <DataMember>
            Public IsPlate As Boolean

        End Class

        <DataContract>
        Public Class ItemUsageModel

            <DataMember>
            Public InBattle As Boolean

            <DataMember>
            Public Overworld As Boolean

            <DataMember>
            Public Trade As Boolean

            <DataMember>
            Public Held As Boolean

            <DataMember>
            Public Toss As Boolean

        End Class

        <DataContract>
        Public Class InventoryDefinitionModel

            Inherits JsonDataModel

            <DataMember(Name:="Type")>
            Private TypeStr As String

            Public Property Type() As Item.ItemTypes
                Get
                    Return ConvertStringToEnum(Of Item.ItemTypes)(TypeStr)
                End Get
                Set(value As Item.ItemTypes)
                    TypeStr = value.ToString()
                End Set
            End Property

            <DataMember>
            Public MaxStack As Integer

            <DataMember>
            Public SortValue As Integer

        End Class

    End Class

End Namespace