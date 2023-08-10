Imports System.Runtime.InteropServices

<StructLayout(LayoutKind.Sequential)>
Public Structure VertexPerInstancePosition
    Implements IVertexType
    
    Public Shared Property VertexDeclaration As New VertexDeclaration(new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.Position, 1))

    Public ReadOnly Property IVertexType_VertexDeclaration As VertexDeclaration Implements IVertexType.VertexDeclaration
        Get
            Return VertexDeclaration
        End Get
    End Property
    
    Public Property Position As Vector4
    
    Public Sub New(position As Vector3)
        Me.Position = New Vector4(position.X, position.Y, position.Z, 0)
    End Sub
End Structure