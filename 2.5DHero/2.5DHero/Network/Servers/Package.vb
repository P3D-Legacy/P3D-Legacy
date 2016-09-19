Namespace Servers

    ''' <summary>
    ''' A class to handle the network package protocol.
    ''' </summary>
    ''' <remarks>Call PackageHandler to handle incoming packages.</remarks>
    Public Class Package

        'Package data:
        'ProtcolVersion|PackageType|Origin|DataItemsCount|Offset1|Offset2|Offset3...|Data1Data2Data3
        'The package contains:
        '    - Its protocol version.
        '    - The PackageType, defining the type of the package
        '    - The Origin, indicating which computer sent this package.
        '    - The DataItemsCount tells the package how many data items it contains.
        '    - A list of offsets that separate the data.
        '    - A list of data items, that aren't separated.

#Region "Fields and Enums"

        Public Enum PackageTypes As Integer
            GameData = 0

            ''' <summary>
            ''' Not used anymore, use GameData instead.
            ''' </summary>
            PlayData = 1

            PrivateMessage = 2
            ChatMessage = 3
            Kicked = 4
            ID = 7
            CreatePlayer = 8
            DestroyPlayer = 9
            ServerClose = 10
            ServerMessage = 11
            WorldData = 12
            Ping = 13
            GamestateMessage = 14

            TradeRequest = 30
            TradeJoin = 31
            TradeQuit = 32

            TradeOffer = 33
            TradeStart = 34

            BattleRequest = 50
            BattleJoin = 51
            BattleQuit = 52

            BattleOffer = 53
            BattleStart = 54

            BattleClientData = 55
            BattleHostData = 56
            BattlePokemonData = 57

            ServerInfoData = 98
            ServerDataRequest = 99
        End Enum

        Public Enum ProtocolTypes As Integer
            TCP = 0
            UDP = 1
        End Enum

        Private _packageType As Integer = 0
        Private _origin As Integer = 0
        Private _dataItems As New List(Of String)
        Private _protocolVersion As String = ""
        Private _protocolType As ProtocolTypes = ProtocolTypes.TCP 'Only to remember which protocol to use when sending the data.

        Private _isValid As Boolean = True

#End Region

#Region "Properties"

        ''' <summary>
        ''' The PackageType of this Package.
        ''' </summary>
        Public ReadOnly Property PackageType() As PackageTypes
            Get
                Return CType(Me._packageType, PackageTypes)
            End Get
        End Property

        ''' <summary>
        ''' The Origin ID of this Package.
        ''' </summary>
        Public ReadOnly Property Origin() As Integer
            Get
                Return Me._origin
            End Get
        End Property

        ''' <summary>
        ''' The DataItems of this Package.
        ''' </summary>
        Public ReadOnly Property DataItems() As List(Of String)
            Get
                Return Me._dataItems
            End Get
        End Property

        ''' <summary>
        ''' Returns if the data used to create this Package was valid.
        ''' </summary>
        Public ReadOnly Property IsValid() As Boolean
            Get
                Return Me._isValid
            End Get
        End Property

        ''' <summary>
        ''' The protocol version of this package.
        ''' </summary>
        Public ReadOnly Property ProtocolVersion() As String
            Get
                Return Me._protocolVersion
            End Get
        End Property

        ''' <summary>
        ''' The protocol type (TCP or UDP) this package is using when sending data.
        ''' </summary>
        Public Property ProtocolType() As ProtocolTypes
            Get
                Return Me._protocolType
            End Get
            Set(value As ProtocolTypes)
                Me._protocolType = value
            End Set
        End Property

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Creates a new instance of the Package class.
        ''' </summary>
        ''' <param name="ByteArray">The raw Package data as bytes.</param>
        Public Sub New(ByVal ByteArray As Byte())
            Me.New(System.Text.Encoding.ASCII.GetString(ByteArray))
        End Sub

        ''' <summary>
        ''' Creates a new instance of the Package class.
        ''' </summary>
        ''' <param name="FullData">The raw Package data.</param>
        Public Sub New(ByVal FullData As String)
            Try
                If FullData.Contains("|") = False Then
                    Exit Sub
                End If

                Dim bits As List(Of String) = FullData.Split(CChar("|")).ToList()

                If bits.Count >= 5 Then
                    'Get first part, set the protocol version:
                    Me._protocolVersion = bits(0)

                    'Get second part, set PackageType:
                    If IsNumeric(bits(1)) = True Then
                        Me._packageType = CType(CInt(bits(1)), PackageTypes)
                    Else
                        Me._isValid = False
                        Exit Sub
                    End If

                    'Get third part, set Origin:
                    If IsNumeric(bits(2)) = True Then
                        Me._origin = CInt(bits(2))
                    Else
                        Me._isValid = False
                        Exit Sub
                    End If

                    'Get data items count:
                    Dim dataItemsCount As Integer = 0
                    If IsNumeric(bits(3)) = True Then
                        dataItemsCount = CInt(bits(3))
                    Else
                        Me._isValid = False
                        Exit Sub
                    End If

                    Dim OffsetList As New List(Of Integer)

                    'Count from 4th item to second last item. Those are the offsets.
                    For i = 4 To dataItemsCount - 1 + 4
                        If IsNumeric(bits(i)) = True Then
                            OffsetList.Add(CInt(bits(i)))
                        Else
                            Me._isValid = False
                            Exit Sub
                        End If
                    Next

                    'Set the datastring, its the last item in the list. If it contained any separators, they will get readded here:
                    Dim dataString As String = ""
                    For i = dataItemsCount + 4 To bits.Count - 1
                        If i > dataItemsCount + 4 Then
                            dataString &= "|"
                        End If
                        dataString &= bits(i)
                    Next

                    'Cutting the data:
                    For i = 0 To OffsetList.Count - 1
                        Dim cOffset As Integer = OffsetList(i)
                        Dim length As Integer = dataString.Length - cOffset
                        If i < OffsetList.Count - 1 Then
                            length = OffsetList(i + 1) - cOffset
                        End If

                        Me._dataItems.Add(dataString.Substring(cOffset, length))
                    Next
                Else
                    Me._isValid = False
                End If
            Catch ex As Exception
                Me._isValid = False
            End Try
        End Sub

        ''' <summary>
        ''' Creates a new instance of the Package class.
        ''' </summary>
        ''' <param name="PackageType">The PackageType of the new Package.</param>
        ''' <param name="Origin">The Origin computer ID of the new Package.</param>
        ''' <param name="ProtocolType">The ProtocolType this package is going to use.</param>
        ''' <param name="DataItems">An array of DataItems the Package contains.</param>
        Public Sub New(ByVal PackageType As PackageTypes, ByVal Origin As Integer, ByVal ProtocolType As ProtocolTypes, ByVal DataItems As List(Of String))
            Me._protocolVersion = ServersManager.PROTOCOLVERSION
            Me._packageType = PackageType
            Me._origin = Origin
            Me._dataItems = DataItems
            Me._protocolType = ProtocolType
        End Sub

        ''' <summary>
        ''' Creates a new instance of the Package class.
        ''' </summary>
        ''' <param name="PackageType">The PackageType of the new Package.</param>
        ''' <param name="Origin">The Origin computer ID of the new Package.</param>
        ''' <param name="ProtocolType">The ProtocolType this package is going to use.</param>
        Public Sub New(ByVal PackageType As PackageTypes, ByVal Origin As Integer, ByVal ProtocolType As ProtocolTypes)
            Me._protocolVersion = ServersManager.PROTOCOLVERSION
            Me._packageType = PackageType
            Me._origin = Origin
            Me._protocolType = ProtocolType
        End Sub

        ''' <summary>
        ''' Creates a new instance of the Package class.
        ''' </summary>
        ''' <param name="PackageType">The PackageType of the new Package.</param>
        ''' <param name="ProtocolType">The ProtocolType this package is going to use.</param>
        Public Sub New(ByVal PackageType As PackageTypes, ByVal ProtocolType As ProtocolTypes)
            Me._protocolVersion = ServersManager.PROTOCOLVERSION
            Me._packageType = PackageType
            Me._origin = Core.ServersManager.ID
            Me._protocolType = ProtocolType
        End Sub

        ''' <summary>
        ''' Creates a new instance of the Package class with a single data item.
        ''' </summary>
        ''' <param name="Packagetype">The PackageType of the new Package.</param>
        ''' <param name="Origin">The Origin computer ID of the new Package.</param>
        ''' <param name="ProtocolType">The ProtocolType this package is going to use.</param>
        ''' <param name="DataItem">The single Data Item to create the package with.</param>
        Public Sub New(ByVal Packagetype As PackageTypes, ByVal Origin As Integer, ByVal ProtocolType As ProtocolTypes, ByVal DataItem As String)
            Me.New(Packagetype, Origin, ProtocolType, {DataItem}.ToList())
        End Sub

#End Region

#Region "Methods"

        ''' <summary>
        ''' Returns the raw Package data from the members of this instance.
        ''' </summary>
        Public Overrides Function ToString() As String
            Dim outputStr As String = Me._protocolVersion & "|" & CInt(Me._packageType).ToString() & "|" & Me._origin.ToString() & "|" & Me._dataItems.Count

            Dim currentIndex As Integer = 0
            Dim data As String = ""
            For Each dataItem As String In Me._dataItems
                outputStr &= "|" & currentIndex.ToString()
                data &= dataItem
                currentIndex += dataItem.Length
            Next

            outputStr &= "|" & data

            Return outputStr
        End Function

        ''' <summary>
        ''' Gives this package to the PackageHandler.
        ''' </summary>
        Public Sub Handle()
            PackageHandler.HandlePackage(Me)
        End Sub

        ''' <summary>
        ''' Returns a byte array of the data of this package.
        ''' </summary>
        Public Function GetByteArray() As Byte()
            Return System.Text.Encoding.ASCII.GetBytes(Me.ToString())
        End Function

#End Region

    End Class

End Namespace