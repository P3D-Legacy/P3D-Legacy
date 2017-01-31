Namespace Globalization.Classes

    ''' <summary>
    ''' The translation provider class for the inventory UI.
    ''' </summary>
    Public Class LOCAL_InventoryScreen

        Inherits Translation

        Private Const C_INFO_ITEM_OPTION_USE As String = "Use"
        Private Const C_INFO_ITEM_OPTION_GIVE As String = "Give"
        Private Const C_INFO_ITEM_OPTION_TOSS As String = "Toss"
        Private Const C_INFO_ITEM_OPTION_SELECT As String = "Select"

        Private Const C_MESSAGE_GIVE_ITEM As String = "Gave {0} a {1}."
        Private Const C_MESSAGE_SWITCH_ITEM As String = "Switched {0}'s {1} with a {2}."
        Private Const C_MESSAGE_EGG_ERROR As String = "Eggs cannot hold items."

        Private Const C_TECH_MACHINE_TITLE As String = "Technical Machine"
        Private Const C_HIDDEN_MACHINE_TITLE As String = "Hidden Machine"
        Private Const C_STANDARD_ITEM_TITLE As String = "{0} Item"
        Private Const C_KEYITEM_TITLE As String = "Key Item"
        Private Const C_POKEBALL_TITLE As String = "Poké Ball"
        Private Const C_PLANT_TITLE As String = "Plant"
        Private Const C_BATTLEITEM_TITLE As String = "Battle Item"

        ''' <summary>
        ''' The title for the technical machine item category.
        ''' </summary>
        Public ReadOnly Property TECH_MACHINE_TITLE(ByVal itemCategory As String) As String
            Get
                Return GetTranslation(NameOf(C_TECH_MACHINE_TITLE), {itemCategory})
            End Get
        End Property

        ''' <summary>
        ''' The title for hidden machines in the technical item category.
        ''' </summary>
        Public ReadOnly Property HIDDEN_MACHINE_TITLE(ByVal itemCategory As String) As String
            Get
                Return GetTranslation(NameOf(C_HIDDEN_MACHINE_TITLE), {itemCategory})
            End Get
        End Property

        ''' <summary>
        ''' The title for standard items.
        ''' </summary>
        Public ReadOnly Property STANDARD_ITEM_TITLE(ByVal itemCategory As String) As String
            Get
                Return GetTranslation(NameOf(C_STANDARD_ITEM_TITLE), {itemCategory})
            End Get
        End Property

        ''' <summary>
        ''' The title for key items.
        ''' </summary>
        Public ReadOnly Property KEYITEM_TITLE(ByVal itemCategory As String) As String
            Get
                Return GetTranslation(NameOf(C_KEYITEM_TITLE), {itemCategory})
            End Get
        End Property

        ''' <summary>
        ''' The title for Pokéball items.
        ''' </summary>
        Public ReadOnly Property POKEBALL_TITLE(ByVal itemCategory As String) As String
            Get
                Return GetTranslation(NameOf(C_POKEBALL_TITLE), {itemCategory})
            End Get
        End Property

        ''' <summary>
        ''' The title for plant items.
        ''' </summary>
        Public ReadOnly Property PLANT_TITLE(ByVal itemCategory As String) As String
            Get
                Return GetTranslation(NameOf(C_PLANT_TITLE), {itemCategory})
            End Get
        End Property

        ''' <summary>
        ''' The title for battle items.
        ''' </summary>
        Public ReadOnly Property BATTLEITEM_TITLE(ByVal itemCategory As String) As String
            Get
                Return GetTranslation(NameOf(C_BATTLEITEM_TITLE), {itemCategory})
            End Get
        End Property

        ''' <summary>
        ''' Info panel, use option
        ''' </summary>
        Public ReadOnly Property INFO_ITEM_OPTION_USE As String
            Get
                Return GetTranslation(NameOf(C_INFO_ITEM_OPTION_USE))
            End Get
        End Property

        ''' <summary>
        ''' Info panel, give option
        ''' </summary>
        Public ReadOnly Property INFO_ITEM_OPTION_GIVE As String
            Get
                Return GetTranslation(NameOf(C_INFO_ITEM_OPTION_GIVE))
            End Get
        End Property

        ''' <summary>
        ''' Info panel, toss option
        ''' </summary>
        Public ReadOnly Property INFO_ITEM_OPTION_TOSS As String
            Get
                Return GetTranslation(NameOf(C_INFO_ITEM_OPTION_TOSS))
            End Get
        End Property

        ''' <summary>
        ''' Info panel, select option.
        ''' </summary>
        Public ReadOnly Property INFO_ITEM_OPTION_SELECT As String
            Get
                Return GetTranslation(NameOf(C_INFO_ITEM_OPTION_SELECT))
            End Get
        End Property

        ''' <summary>
        ''' The message displayed when giving an item to a Pokémon.
        ''' </summary>
        Public ReadOnly Property MESSAGE_GIVE_ITEM(ByVal pokemonName As String, ByVal itemName As String) As String
            Get
                Return GetTranslation(NameOf(C_MESSAGE_GIVE_ITEM), {pokemonName, itemName})
            End Get
        End Property

        ''' <summary>
        ''' The message displayed when switching an item for a Pokémon.
        ''' </summary>
        Public ReadOnly Property MESSAGE_SWITCH_ITEM(ByVal pokemonName As String, ByVal preItemName As String, ByVal itemName As String) As String
            Get
                Return GetTranslation(NameOf(C_MESSAGE_SWITCH_ITEM), {pokemonName, preItemName, itemName})
            End Get
        End Property

        ''' <summary>
        ''' Message for when giving an item to an egg
        ''' </summary>
        Public ReadOnly Property MESSAGE_EGG_ERROR As String
            Get
                Return GetTranslation(NameOf(C_MESSAGE_EGG_ERROR))
            End Get
        End Property

    End Class

End Namespace
