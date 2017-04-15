Imports System.Runtime.Serialization.Json
Imports System.Runtime.Serialization

Namespace DataModel.Json

    ''' <summary>
    ''' The base data model class.
    ''' </summary>
    <DataContract>
    Public MustInherit Class JsonDataModel

        Protected Sub New()
            'Empty constructor.
        End Sub

        ''' <summary>
        ''' Creates a data model of a specific type.
        ''' </summary>
        ''' <typeparam name="T">The return type of the data model.</typeparam>
        ''' <param name="input">The Json input string.</param>
        ''' <returns></returns>
        Public Shared Function FromString(Of T)(ByVal input As String) As T
            'We create a new Json serializer of the given type and a corresponding memory stream here.
            Dim serializer As New DataContractJsonSerializer(GetType(T))
            Dim memStream As New MemoryStream()

            'Create StreamWriter to the memory stream, which writes the input string to the stream.
            Dim sw As New StreamWriter(memStream)
            sw.Write(input)
            sw.Flush()

            'Reset the stream's position to the beginning:
            memStream.Position = 0

            Try
                'Create and return the object:
                Dim returnObj As T = CType(serializer.ReadObject(memStream), T)
                Return returnObj
            Catch ex As Exception
                'Exception occurs while loading the object due to malformed Json.
                'Throw exception and move up to handler class.
                Throw New JsonDataLoadException(input, GetType(T))
            End Try
        End Function

        ''' <summary>
        ''' Converts a string representation of an enum member into the enum type.
        ''' </summary>
        ''' <typeparam name="TEnum">The enum type.</typeparam>
        ''' <param name="enumMember">The string representation of the enum member.</param>
        ''' <returns></returns>
        Protected Function ConvertStringToEnum(Of TEnum)(ByVal enumMember As String) As TEnum
            Dim names As String() = [Enum].GetNames(GetType(TEnum))
            Dim index As Integer = 0

            If names.Contains(enumMember) Then
                index = Array.IndexOf(names, enumMember)
            End If

            Return CType([Enum].Parse(GetType(TEnum), names(index)), TEnum)
        End Function

        Protected Function ConvertStringArrayToEnumArray(Of TEnum)(ByVal enumMemberArray As String()) As TEnum()
            Dim enumArr As TEnum() = New TEnum(enumMemberArray.Length - 1) {}
            For i = 0 To enumMemberArray.Length - 1
                enumArr(i) = ConvertStringToEnum(Of TEnum)(enumMemberArray(i))
            Next
            Return enumArr
        End Function

        Protected Function ConvertEnumArrayToStringArray(Of TEnum)(ByVal enumArray As TEnum()) As String()
            Dim stringArr As String() = New String(enumArray.Length - 1) {}
            For i = 0 To enumArray.Length - 1
                stringArr(i) = enumArray(i).ToString()
            Next
            Return stringArr
        End Function

        ''' <summary>
        ''' Returns the Json representation of this object.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            'We create a new Json serializer of the given type and a corresponding memory stream here.
            Dim serializer As New DataContractJsonSerializer(Me.GetType())
            Dim memStream As New MemoryStream()

            'Write the data to the stream.
            serializer.WriteObject(memStream, Me)

            'Reset the stream's position to the beginning:
            memStream.Position = 0

            'Create stream reader, read string and return it.
            Dim sr As New StreamReader(memStream)
            Dim returnJson As String = sr.ReadToEnd()

            Return returnJson
        End Function

        ''' <summary>
        ''' Returns the Json representation of this object in multiline format.
        ''' </summary>
        ''' <param name="spaces">The spaces to use for outlining.</param>
        ''' <returns></returns>
        Public Overloads Function ToString(ByVal spaces As String) As String
            Return JsonFormatter.FormatMultiline(ToString(), spaces)
        End Function

    End Class

    ''' <summary>
    ''' An exception thrown when an error occurs while loading Json data.
    ''' </summary>
    Public Class JsonDataLoadException

        Inherits Exception

        Public Sub New(ByVal jsonData As String, ByVal targetType As Type)
            MyBase.New("An exception occured trying to read Json data into an internal format. Please check that the input data is correct.")

            Data.Add("Target type", targetType.Name)
            Data.Add("Json data", jsonData)
        End Sub

    End Class

#Region "Data type model definitions"

    ''' <summary>
    ''' The data model for a RGB color (no alpha).
    ''' </summary>
    <DataContract>
    Public Class RGBColorModel

        <DataMember(Order:=0)>
        Public Red As Byte

        <DataMember(Order:=1)>
        Public Green As Byte

        <DataMember(Order:=2)>
        Public Blue As Byte

        Public Function GetColor() As Color
            Return New Color(Red, Green, Blue)
        End Function

    End Class

    ''' <summary>
    ''' The data model for a rectangle definition.
    ''' </summary>
    <DataContract>
    Public Class RectangleModel

        <DataMember(Order:=0)>
        Public X As Integer

        <DataMember(Order:=1)>
        Public Y As Integer

        <DataMember(Order:=2)>
        Public Width As Integer

        <DataMember(Order:=3)>
        Public Height As Integer

        Public Function GetRectangle() As Rectangle
            Return New Rectangle(X, Y, Width, Height)
        End Function

    End Class

    ''' <summary>
    ''' The data model for a Vector 3 definition.
    ''' </summary>
    <DataContract>
    Public Class Vector3Model

        <DataMember(Order:=0)>
        Public X As Decimal

        <DataMember(Order:=1)>
        Public Y As Decimal

        <DataMember(Order:=2)>
        Public Z As Decimal

        Public Function ToVector3() As Vector3
            Return New Vector3(X, Y, Z)
        End Function

    End Class

    ''' <summary>
    ''' The data model for a Vector 2 definition.
    ''' </summary>
    <DataContract>
    Public Class Vector2Model

        <DataMember(Order:=0)>
        Public X As Decimal

        <DataMember(Order:=1)>
        Public Y As Decimal

        Public Function ToVector2() As Vector2
            Return New Vector2(X, Y)
        End Function

    End Class

    ''' <summary>
    ''' The data model for a range.
    ''' </summary>
    <DataContract>
    Public Class RangeModel

        <DataMember(Order:=0)>
        Public Min As Decimal

        <DataMember(Order:=1)>
        Public Max As Decimal

    End Class

    ''' <summary>
    ''' The data model for a texture source, with source file and rectangle.
    ''' </summary>
    <DataContract>
    Public Class TextureSourceModel

        <DataMember(Order:=0)>
        Public Source As String

        <DataMember(Order:=1)>
        Public Rectangle As RectangleModel

    End Class

#End Region

End Namespace