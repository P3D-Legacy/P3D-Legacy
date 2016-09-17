Namespace Scripting.V3.Prototypes

    Friend Class TypeContract

        Public Shared Function Ensure(objects As Object(), typeContract As Type()) As Boolean
            If objects.Length <> typeContract.Length Then
                Return False
            End If

            Return Not typeContract.Where(Function(t As Type, i As Integer)
                                              Return objects(i) IsNot Nothing AndAlso objects(i).GetType() <> t
                                          End Function).Any()
        End Function

    End Class

End Namespace
