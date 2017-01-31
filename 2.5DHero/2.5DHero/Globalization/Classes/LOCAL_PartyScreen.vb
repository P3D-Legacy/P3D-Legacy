Namespace Globalization.Classes

    Public Class LOCAL_PartyScreen

        Inherits Translation

        Private Const C_LV_TEXT As String = "Lv. {0}"

        Private Const C_MENU_SUMMARY As String = "Summary"
        Private Const C_MENU_SELECT As String = "Select"
        Private Const C_MENU_SWITCH As String = "Switch"
        Private Const C_MENU_ITEM As String = "Item"
        Private Const C_MENU_BACK As String = "Back"
        Private Const C_MENU_FIELDMOVE As String = "Field Move"

        Private Const C_MENU_ITEM_GIVE As String = "Give"
        Private Const C_MENU_ITEM_TAKE As String = "Take"

        Private Const C_MENU_FIELDMOVE_FLY As String = "Fly"
        Private Const C_MENU_FIELDMOVE_RIDE As String = "Ride"
        Private Const C_MENU_FIELDMOVE_FLASH As String = "Flash"
        Private Const C_MENU_FIELDMOVE_CUT As String = "Cut"
        Private Const C_MENU_FIELDMOVE_TELEPORT As String = "Teleport"
        Private Const C_MENU_FIELDMOVE_DIG As String = "Dig"

        Private Const C_MESSAGE_FIELDMOVE_ERROR As String = "You cannot use {0} here."
        Private Const C_MESSAGE_MAILTAKEN As String = "The Mail was taken to your inbox on your PC."
        Private Const C_MESSAGE_ITEMTAKEN As String = "Taken {0} from {1}."

        Private Const C_MESSAGE_SWITCH_ITEM_MAIL As String = "Gave {0} to {1} and took the Mail to the PC."
        Private Const C_MESSAGE_SWITCH_ITEM As String = "Switched {0}'s {1} with a {2}."
        Private Const C_MESSAGE_GIVE_ITEM As String = "Gave {0} a {1}."
        Private Const C_MESSAGE_GIVE_ITEM_ERROR As String = "{0} cannot be given to a Pokémon."

        ''' <summary>
        ''' The message displayed when giving an item to a Pokémon.
        ''' </summary>
        Public ReadOnly Property MESSAGE_GIVE_ITEM_ERROR(ByVal itemName As String) As String
            Get
                Return GetTranslation(NameOf(C_MESSAGE_GIVE_ITEM_ERROR), {itemName})
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
        ''' A message that displays when the player switches the item on a Pokémon.
        ''' </summary>
        Public ReadOnly Property MESSAGE_SWITCH_ITEM(ByVal pokemonName As String, ByVal newItemName As String, ByVal oldItemName As String) As String
            Get
                Return GetTranslation(NameOf(C_MESSAGE_SWITCH_ITEM), {pokemonName, newItemName, oldItemName})
            End Get
        End Property

        ''' <summary>
        ''' A message that displays when the player switches the mail from a Pokémon with another item.
        ''' </summary>
        Public ReadOnly Property MESSAGE_SWITCH_ITEM_MAIL(ByVal itemName As String, ByVal pokemonName As String) As String
            Get
                Return GetTranslation(NameOf(C_MESSAGE_SWITCH_ITEM_MAIL), {itemName, pokemonName})
            End Get
        End Property

        ''' <summary>
        ''' A message that displays when the player took mail from a Pokémon.
        ''' </summary>
        Public ReadOnly Property MESSAGE_MAILTAKEN As String
            Get
                Return GetTranslation(NameOf(C_MESSAGE_MAILTAKEN))
            End Get
        End Property

        ''' <summary>
        ''' A message that displays when the player took an item from a Pokémon.
        ''' </summary>
        Public ReadOnly Property MESSAGE_ITEMTAKEN(ByVal itemName As String, ByVal pokemonName As String) As String
            Get
                Return GetTranslation(NameOf(C_MESSAGE_ITEMTAKEN), {itemName, pokemonName})
            End Get
        End Property

        ''' <summary>
        ''' A message that displays when the player cannot fly from the current position.
        ''' </summary>
        Public ReadOnly Property MESSAGE_FIELDMOVE_ERROR(ByVal fieldmove As String) As String
            Get
                Return GetTranslation(NameOf(C_MESSAGE_FIELDMOVE_ERROR), {fieldmove})
            End Get
        End Property

        ''' <summary>
        ''' The level literal.
        ''' </summary>
        Public ReadOnly Property LV_TEXT(ByVal level As String) As String
            Get
                Return GetTranslation(NameOf(C_LV_TEXT), {level})
            End Get
        End Property

        ''' <summary>
        ''' Menu entry, summary.
        ''' </summary>
        Public ReadOnly Property MENU_SUMMARY As String
            Get
                Return GetTranslation(NameOf(C_MENU_SUMMARY))
            End Get
        End Property

        ''' <summary>
        ''' Menu entry, select.
        ''' </summary>
        Public ReadOnly Property MENU_SELECT As String
            Get
                Return GetTranslation(NameOf(C_MENU_SELECT))
            End Get
        End Property

        ''' <summary>
        ''' Menu entry, switch.
        ''' </summary>
        Public ReadOnly Property MENU_SWITCH As String
            Get
                Return GetTranslation(NameOf(C_MENU_SWITCH))
            End Get
        End Property

        ''' <summary>
        ''' Menu entry, item.
        ''' </summary>
        Public ReadOnly Property MENU_ITEM As String
            Get
                Return GetTranslation(NameOf(C_MENU_ITEM))
            End Get
        End Property

        ''' <summary>
        ''' Menu entry, back.
        ''' </summary>
        Public ReadOnly Property MENU_BACK As String
            Get
                Return GetTranslation(NameOf(C_MENU_BACK))
            End Get
        End Property

        ''' <summary>
        ''' Menu entry, field move.
        ''' </summary>
        Public ReadOnly Property MENU_FIELDMOVE As String
            Get
                Return GetTranslation(NameOf(C_MENU_FIELDMOVE))
            End Get
        End Property

        ''' <summary>
        ''' Menu entry, give.
        ''' </summary>
        Public ReadOnly Property MENU_ITEM_GIVE As String
            Get
                Return GetTranslation(NameOf(C_MENU_ITEM_GIVE))
            End Get
        End Property

        ''' <summary>
        ''' Menu entry, take.
        ''' </summary>
        Public ReadOnly Property MENU_ITEM_TAKE As String
            Get
                Return GetTranslation(NameOf(C_MENU_ITEM_TAKE))
            End Get
        End Property

        ''' <summary>
        ''' Menu entry, fly.
        ''' </summary>
        Public ReadOnly Property MENU_FIELDMOVE_FLY As String
            Get
                Return GetTranslation(NameOf(C_MENU_FIELDMOVE_FLY))
            End Get
        End Property

        ''' <summary>
        ''' Menu entry, ride.
        ''' </summary>
        Public ReadOnly Property MENU_FIELDMOVE_RIDE As String
            Get
                Return GetTranslation(NameOf(C_MENU_FIELDMOVE_RIDE))
            End Get
        End Property

        ''' <summary>
        ''' Menu entry, flash.
        ''' </summary>
        Public ReadOnly Property MENU_FIELDMOVE_FLASH As String
            Get
                Return GetTranslation(NameOf(C_MENU_FIELDMOVE_FLASH))
            End Get
        End Property

        ''' <summary>
        ''' Menu entry, cut.
        ''' </summary>
        Public ReadOnly Property MENU_FIELDMOVE_CUT As String
            Get
                Return GetTranslation(NameOf(C_MENU_FIELDMOVE_CUT))
            End Get
        End Property

        ''' <summary>
        ''' Menu entry, teleport.
        ''' </summary>
        Public ReadOnly Property MENU_FIELDMOVE_TELEPORT As String
            Get
                Return GetTranslation(NameOf(C_MENU_FIELDMOVE_TELEPORT))
            End Get
        End Property

        ''' <summary>
        ''' Menu entry, dig.
        ''' </summary>
        Public ReadOnly Property MENU_FIELDMOVE_DIG As String
            Get
                Return GetTranslation(NameOf(C_MENU_FIELDMOVE_DIG))
            End Get
        End Property

    End Class

End Namespace
