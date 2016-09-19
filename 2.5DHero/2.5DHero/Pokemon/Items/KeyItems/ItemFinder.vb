Namespace Items.KeyItems

    <Item(55, "Itemfinder")>
    Public Class ItemFinder

        Inherits Item

        Public Sub New()
            MyBase.New("Itemfinder", 138, ItemTypes.KeyItems, 55, 1, 0, New Rectangle(192, 48, 24, 24), "It checks for unseen items in the area and makes noise and lights when it finds something.")

            Me._canBeUsed = True
            Me._canBeUsedInBattle = False
            Me._canBeTraded = False
            Me._canBeHold = False
        End Sub

        Public Overrides Sub Use()
            Dim found As Boolean = False

            For Each e As Entity In Screen.Level.Entities
                If e.EntityID = "ItemObject" Then
                    Dim i As ItemObject = CType(e, ItemObject)
                    If i.IsHiddenItem() = True Then
                        i.Opacity = 1.0F
                        i.Visible = True
                        found = True
                    End If
                End If
            Next

            While Core.CurrentScreen.Identification <> Screen.Identifications.OverworldScreen
                Core.SetScreen(Core.CurrentScreen.PreScreen)
            End While

            If found = False Then
                Screen.TextBox.Show("No hidden items.", {}, True, False)
            Else
                SoundManager.PlaySound("itemfinder", False)
            End If
        End Sub

    End Class

End Namespace
