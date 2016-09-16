Imports System.Threading
Imports Pokemon3D.Scripting.Adapters
Imports Pokemon3D.Scripting.Types

Namespace Scripting.V3.ApiClasses

    Friend MustInherit Class ApiClass

        Protected Shared Function EnsureTypeContract(parameters As SObject(), typeContract As Type(), netObjects As Object()) As Boolean

            If parameters.Length >= typeContract.Length Then

                netObjects = New Object(parameters.Length - 1) {}
                Dim i = 0

                While i < parameters.Length
                    netObjects(i) = ScriptOutAdapter.Translate(parameters(i))

                    If i < typeContract.Length AndAlso Not typeContract(i) = netObjects(i).GetType() Then
                        Return False
                    End If
                End While

                Return True
            Else
                netObjects = Nothing
                Return False
            End If

        End Function

        Protected Shared Sub BlockThreadUntilCondition(condition As Func(Of Boolean))
            SpinWait.SpinUntil(condition)
        End Sub

    End Class

End Namespace
