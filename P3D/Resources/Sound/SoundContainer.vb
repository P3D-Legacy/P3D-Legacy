'Created to support ContentPack SFX
Public Class SoundContainer

	Public _sound As SoundEffect
	Public _origin As String

	Public Sub New(sound As SoundEffect, origin As String)
		Me._sound = sound
		Me._origin = origin
	End Sub

	Public Property Sound() As SoundEffect
		Get
			Return Me._sound
		End Get
		Set(value As SoundEffect)
			Me._sound = value
		End Set
	End Property

	Public Property Origin() As String
		Get
			Return Me._origin
		End Get
		Set(value As String)
			Me._origin = value
		End Set
	End Property

	Public ReadOnly Property IsStandardSound() As Boolean
		Get
			Return (Me.Origin = "Content")
		End Get
	End Property

End Class
