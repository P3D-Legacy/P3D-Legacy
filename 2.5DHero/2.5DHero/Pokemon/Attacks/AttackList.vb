Imports System.Reflection

Namespace BattleSystem

    Public NotInheritable Class AttackList
        Public Shared ReadOnly Attacks As Func(Of Attack)()

        Shared Sub New()
            Attacks = New AttackEnum().SomethingConstructorArray(Of Attack)(Assembly.GetAssembly(GetType(AttackList)))
        End Sub
    End Class
End Namespace
