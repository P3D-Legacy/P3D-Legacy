Namespace Scripting.V3.Prototypes

    Friend Class TypeContract

        Public Shared Function Ensure(objects As Object(), typeContract As Type, Optional optionalCount As Integer = 0) As Boolean
            Return Ensure(objects, {typeContract}, optionalCount)
        End Function

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

        Public Shared Function Ensure(objects As Object(), typeContract As Type()(), Optional optionalCount As Integer = 0) As Boolean
            If optionalCount > typeContract.Length Then
                Throw New ArgumentOutOfRangeException(NameOf(optionalCount))
            End If

            If objects.Length + optionalCount < typeContract.Length Then
                Return False
            End If

            Return Not typeContract.Where(Function(types As Type(), i As Integer)
                                              If types Is Nothing OrElse types.Length = 0 Then
                                                  Return False
                                              End If

                                              If objects.Length <= i Then
                                                  Return i < typeContract.Length - optionalCount
                                              End If

                                              If objects(i) IsNot Nothing Then
                                                  For Each t As Type In types
                                                      If objects(i).GetType() = t Then
                                                          Return False
                                                      End If
                                                  Next
                                                  Return True
                                              Else
                                                  Return False
                                              End If
                                          End Function).Any()
        End Function

        Private Shared _numberBuffer As Type()

        Public Shared ReadOnly Property Number() As Type()
            Get

                If _numberBuffer Is Nothing Then
                    _numberBuffer = {GetType(Double), GetType(Integer)}
                End If

                Return _numberBuffer

            End Get
        End Property

    End Class

End Namespace
