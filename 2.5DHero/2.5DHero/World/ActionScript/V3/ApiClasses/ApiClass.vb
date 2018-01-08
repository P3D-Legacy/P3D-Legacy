Imports Kolben.Adapters
Imports Kolben.Types

Namespace Scripting.V3.ApiClasses

    Friend MustInherit Class ApiClass

        Protected Shared Function EnsureTypeContract(parameters As SObject(), typeContract As Type(), ByRef netObjects As Object(), Optional optionalCount As Integer = 0) As Boolean

            If parameters.Length + optionalCount >= typeContract.Length Then

                netObjects = New Object(parameters.Length - 1) {}
                Dim i = 0

                While i < parameters.Length
                    netObjects(i) = ScriptOutAdapter.Translate(parameters(i))

                    If i < typeContract.Length AndAlso Not typeContract(i) = netObjects(i).GetType() Then
                        Return False
                    End If

                    i += 1
                End While

                Return True
            Else
                netObjects = Nothing
                Return False
            End If

        End Function

    End Class

End Namespace
