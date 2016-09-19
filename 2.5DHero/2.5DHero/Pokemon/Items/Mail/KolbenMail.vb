Namespace Items.Mail

    <Item(336, "Kolben Mail")>
    Public Class KolbenMail

        Inherits MailItem

        Public Sub New()
            MyBase.New("Kolben Mail", 2674, 336, New Rectangle(360, 480, 24, 24), "Stationery featuring a print of the Kolben Logo. It is given to Pokémon with a special meaning.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = False
            Me._canBeHold = False
        End Sub

    End Class

End Namespace
