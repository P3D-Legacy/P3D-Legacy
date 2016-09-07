'Namespace Attacks.Normal

'    Public Class StruggleAttack

'        Inherits Attack

'        Public Sub New()
'            MyBase.New(165, "Struggle", 1, 50, -1, 0, Categories.Physical, ContestCategories.Cool, AttackCategories.Damage, New Element(Element.Types.Normal), 0, "Used only if all PP are exhausted.")

'            Me.CanBeCopiedByMirrorMove = False
'            Me.UseNotAffected = False
'            Me.UseNotEffective = False
'            Me.UseSuperEffective = False
'        End Sub

'        Public Overrides Sub DidDamage(ByVal Battle As Battle, ByVal target As String, ByVal damage As Integer)
'            Dim recoil As Integer = CInt(Math.Ceiling((damage * 25) / 100))

'            Dim p As Pokemon = Nothing
'            If target = "1" Then
'                p = Battle.GetOwnPokemon()
'            ElseIf target = "0" Then
'                p = Battle.GetOppPokemon()
'            End If

'            Battle.AddBattleStep("DrainHP", target & "|" & recoil)
'            Battle.AddBattleStep("Message", p.GetDisplayName() & " is damaged~by recoil!")
'        End Sub

'    End Class

'End Namespace