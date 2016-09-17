Namespace Scripting.V3.Prototypes

    Friend Class TypeContract

        Public Shared Function Ensure(objects As Object(), typeContract As Type(), Optional optionalCount As Integer = 0) As Boolean
            If optionalCount > typeContract.Length Then
                Throw New ArgumentOutOfRangeException(NameOf(optionalCount))
            End If

            If objects.Length + optionalCount < typeContract.Length Then
                Return False
            End If

            Return Not typeContract.Where(Function(t As Type, i As Integer)
                                              If t Is Nothing Then
                                                  Return False
                                              End If

                                              If objects.Length <= i Then
                                                  Return i < typeContract.Length - optionalCount
                                              End If

                                              Return objects(i) IsNot Nothing AndAlso objects(i).GetType() <> t
                                          End Function).Any()
        End Function

    End Class

End Namespace
