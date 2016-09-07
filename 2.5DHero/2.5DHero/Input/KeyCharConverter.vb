''' <summary>
''' This class serves the only purpose to convert a key form the Keys enumeration to an actual Char that the key represents on the currently loaded keyboard layout.
''' </summary>
Public Class KeyCharConverter

#Region "KeyConversion"
    ' This shit right here...

#Region "VirtualKeys"

    Private Const QS_EVENT As Integer = &H2000

    Private Const VK_CANCEL As Integer = &H3


    Private Const VK_BACK As Integer = &H8

    Private Const VK_CLEAR As Integer = &HC

    Private Const VK_RETURN As Integer = &HD

    Private Const VK_PAUSE As Integer = &H13

    Private Const VK_CAPITAL As Integer = &H14

    Private Const VK_KANA As Integer = &H15

    Private Const VK_HANGEUL As Integer = &H15

    Private Const VK_HANGUL As Integer = &H15

    Private Const VK_JUNJA As Integer = &H17

    Private Const VK_FINAL As Integer = &H18

    Private Const VK_HANJA As Integer = &H19

    Private Const VK_KANJI As Integer = &H19

    Private Const VK_ESCAPE As Integer = &H1B

    Private Const VK_CONVERT As Integer = &H1C

    Private Const VK_NONCONVERT As Integer = &H1D

    Private Const VK_ACCEPT As Integer = &H1E

    Private Const VK_MODECHANGE As Integer = &H1F

    Private Const VK_SPACE As Integer = &H20

    Private Const VK_PRIOR As Integer = &H21

    Private Const VK_NEXT As Integer = &H22

    Private Const VK_END As Integer = &H23

    Private Const VK_HOME As Integer = &H24

    Private Const VK_LEFT As Integer = &H25

    Private Const VK_UP As Integer = &H26

    Private Const VK_RIGHT As Integer = &H27

    Private Const VK_DOWN As Integer = &H28

    Private Const VK_SELECT As Integer = &H29

    Private Const VK_PRINT As Integer = &H2A

    Private Const VK_EXECUTE As Integer = &H2B

    Private Const VK_SNAPSHOT As Integer = &H2C

    Private Const VK_INSERT As Integer = &H2D

    Private Const VK_DELETE As Integer = &H2E

    Private Const VK_HELP As Integer = &H2F

    Private Const VK_0 As Integer = &H30

    Private Const VK_1 As Integer = &H31

    Private Const VK_2 As Integer = &H32

    Private Const VK_3 As Integer = &H33

    Private Const VK_4 As Integer = &H34

    Private Const VK_5 As Integer = &H35

    Private Const VK_6 As Integer = &H36

    Private Const VK_7 As Integer = &H37

    Private Const VK_8 As Integer = &H38

    Private Const VK_9 As Integer = &H39

    Private Const VK_A As Integer = &H41

    Private Const VK_B As Integer = &H42

    Private Const VK_C As Integer = &H43

    Private Const VK_D As Integer = &H44

    Private Const VK_E As Integer = &H45

    Private Const VK_F As Integer = &H46

    Private Const VK_G As Integer = &H47

    Private Const VK_H As Integer = &H48

    Private Const VK_I As Integer = &H49

    Private Const VK_J As Integer = &H4A

    Private Const VK_K As Integer = &H4B

    Private Const VK_L As Integer = &H4C

    Private Const VK_M As Integer = &H4D

    Private Const VK_N As Integer = &H4E

    Private Const VK_O As Integer = &H4F

    Private Const VK_P As Integer = &H50

    Private Const VK_Q As Integer = &H51

    Private Const VK_R As Integer = &H52

    Private Const VK_S As Integer = &H53

    Private Const VK_T As Integer = &H54

    Private Const VK_U As Integer = &H55

    Private Const VK_V As Integer = &H56

    Private Const VK_W As Integer = &H57

    Private Const VK_X As Integer = &H58

    Private Const VK_Y As Integer = &H59

    Private Const VK_Z As Integer = &H5A

    Private Const VK_LWIN As Integer = &H5B

    Private Const VK_RWIN As Integer = &H5C

    Private Const VK_APPS As Integer = &H5D

    Private Const VK_POWER As Integer = &H5E

    Private Const VK_SLEEP As Integer = &H5F

    Private Const VK_NUMPAD0 As Integer = &H60

    Private Const VK_NUMPAD1 As Integer = &H61

    Private Const VK_NUMPAD2 As Integer = &H62

    Private Const VK_NUMPAD3 As Integer = &H63

    Private Const VK_NUMPAD4 As Integer = &H64

    Private Const VK_NUMPAD5 As Integer = &H65

    Private Const VK_NUMPAD6 As Integer = &H66

    Private Const VK_NUMPAD7 As Integer = &H67

    Private Const VK_NUMPAD8 As Integer = &H68

    Private Const VK_NUMPAD9 As Integer = &H69

    Private Const VK_MULTIPLY As Integer = &H6A

    Private Const VK_ADD As Integer = &H6B

    Private Const VK_SEPARATOR As Integer = &H6C

    Private Const VK_SUBTRACT As Integer = &H6D

    Private Const VK_DECIMAL As Integer = &H6E

    Private Const VK_DIVIDE As Integer = &H6F

    Private Const VK_F1 As Integer = &H70

    Private Const VK_F2 As Integer = &H71

    Private Const VK_F3 As Integer = &H72

    Private Const VK_F4 As Integer = &H73

    Private Const VK_F5 As Integer = &H74

    Private Const VK_F6 As Integer = &H75

    Private Const VK_F7 As Integer = &H76

    Private Const VK_F8 As Integer = &H77

    Private Const VK_F9 As Integer = &H78

    Private Const VK_F10 As Integer = &H79

    Private Const VK_F11 As Integer = &H7A

    Private Const VK_F12 As Integer = &H7B

    Private Const VK_F13 As Integer = &H7C

    Private Const VK_F14 As Integer = &H7D

    Private Const VK_F15 As Integer = &H7E

    Private Const VK_F16 As Integer = &H7F

    Private Const VK_F17 As Integer = &H80

    Private Const VK_F18 As Integer = &H81

    Private Const VK_F19 As Integer = &H82

    Private Const VK_F20 As Integer = &H83

    Private Const VK_F21 As Integer = &H84

    Private Const VK_F22 As Integer = &H85

    Private Const VK_F23 As Integer = &H86

    Private Const VK_F24 As Integer = &H87

    Private Const VK_NUMLOCK As Integer = &H90

    Private Const VK_SCROLL As Integer = &H91


    Private Const VK_RSHIFT As Integer = &HA1

    Private Const VK_BROWSER_BACK As Integer = &HA6

    Private Const VK_BROWSER_FORWARD As Integer = &HA7

    Private Const VK_BROWSER_REFRESH As Integer = &HA8

    Private Const VK_BROWSER_STOP As Integer = &HA9

    Private Const VK_BROWSER_SEARCH As Integer = &HAA

    Private Const VK_BROWSER_FAVORITES As Integer = &HAB

    Private Const VK_BROWSER_HOME As Integer = &HAC

    Private Const VK_VOLUME_MUTE As Integer = &HAD

    Private Const VK_VOLUME_DOWN As Integer = &HAE

    Private Const VK_VOLUME_UP As Integer = &HAF

    Private Const VK_MEDIA_NEXT_TRACK As Integer = &HB0

    Private Const VK_MEDIA_PREV_TRACK As Integer = &HB1

    Private Const VK_MEDIA_STOP As Integer = &HB2

    Private Const VK_MEDIA_PLAY_PAUSE As Integer = &HB3

    Private Const VK_LAUNCH_MAIL As Integer = &HB4

    Private Const VK_LAUNCH_MEDIA_SELECT As Integer = &HB5

    Private Const VK_LAUNCH_APP1 As Integer = &HB6

    Private Const VK_LAUNCH_APP2 As Integer = &HB7

    Private Const VK_PROCESSKEY As Integer = &HE5

    Private Const VK_PACKET As Integer = &HE7

    Private Const VK_ATTN As Integer = &HF6

    Private Const VK_CRSEL As Integer = &HF7

    Private Const VK_EXSEL As Integer = &HF8

    Private Const VK_EREOF As Integer = &HF9

    Private Const VK_PLAY As Integer = &HFA

    Private Const VK_ZOOM As Integer = &HFB

    Private Const VK_NONAME As Integer = &HFC

    Private Const VK_PA1 As Integer = &HFD

    Private Const VK_OEM_CLEAR As Integer = &HFE

    Private Const VK_TAB As Integer = &H9
    Private Const VK_SHIFT As Integer = &H10
    Private Const VK_CONTROL As Integer = &H11
    Private Const VK_MENU As Integer = &H12

    Private Const VK_LSHIFT As Integer = &HA0
    Private Const VK_RMENU As Integer = &HA5
    Private Const VK_LMENU As Integer = &HA4
    Private Const VK_LCONTROL As Integer = &HA2
    Private Const VK_RCONTROL As Integer = &HA3
    Private Const VK_LBUTTON As Integer = &H1
    Private Const VK_RBUTTON As Integer = &H2
    Private Const VK_MBUTTON As Integer = &H4
    Private Const VK_XBUTTON1 As Integer = &H5
    Private Const VK_XBUTTON2 As Integer = &H6

    Private Const VK_OEM_1 As Integer = &HBA
    Private Const VK_OEM_PLUS As Integer = &HBB
    Private Const VK_OEM_COMMA As Integer = &HBC
    Private Const VK_OEM_MINUS As Integer = &HBD
    Private Const VK_OEM_PERIOD As Integer = &HBE
    Private Const VK_OEM_2 As Integer = &HBF
    Private Const VK_OEM_3 As Integer = &HC0
    Private Const VK_C1 As Integer = &HC1
    ' Brazilian ABNT_C1 key (not defined in winuser.h).
    Private Const VK_C2 As Integer = &HC2
    ' Brazilian ABNT_C2 key (not defined in winuser.h).
    Private Const VK_OEM_4 As Integer = &HDB
    Private Const VK_OEM_5 As Integer = &HDC
    Private Const VK_OEM_6 As Integer = &HDD
    Private Const VK_OEM_7 As Integer = &HDE
    Private Const VK_OEM_8 As Integer = &HDF
    Private Const VK_OEM_AX As Integer = &HE1
    Private Const VK_OEM_102 As Integer = &HE2
    Private Const VK_OEM_RESET As Integer = &HE9
    Private Const VK_OEM_JUMP As Integer = &HEA
    Private Const VK_OEM_PA1 As Integer = &HEB
    Private Const VK_OEM_PA2 As Integer = &HEC
    Private Const VK_OEM_PA3 As Integer = &HED
    Private Const VK_OEM_WSCTRL As Integer = &HEE
    Private Const VK_OEM_CUSEL As Integer = &HEF
    Private Const VK_OEM_ATTN As Integer = &HF0
    Private Const VK_OEM_FINISH As Integer = &HF1
    Private Const VK_OEM_COPY As Integer = &HF2
    Private Const VK_OEM_AUTO As Integer = &HF3
    Private Const VK_OEM_ENLW As Integer = &HF4
    Private Const VK_OEM_BACKTAB As Integer = &HF5

#End Region

    Private Shared Function GetVirtualKeyFromKey(ByVal k As Windows.Forms.Keys) As Integer

        Dim virtualKey As Integer = 0

        Select Case CInt(k)
            Case Windows.Forms.Keys.Cancel
                virtualKey = VK_CANCEL
                Exit Select

            Case Windows.Forms.Keys.Back
                virtualKey = VK_BACK
                Exit Select

            Case Windows.Forms.Keys.Tab
                virtualKey = VK_TAB
                Exit Select

            Case Windows.Forms.Keys.Clear
                virtualKey = VK_CLEAR
                Exit Select

            Case Windows.Forms.Keys.[Return]
                virtualKey = VK_RETURN
                Exit Select

            Case Windows.Forms.Keys.Pause
                virtualKey = VK_PAUSE
                Exit Select

            Case Windows.Forms.Keys.Capital
                virtualKey = VK_CAPITAL
                Exit Select

            Case Windows.Forms.Keys.KanaMode
                virtualKey = VK_KANA
                Exit Select

            Case Windows.Forms.Keys.JunjaMode
                virtualKey = VK_JUNJA
                Exit Select

            Case Windows.Forms.Keys.FinalMode
                virtualKey = VK_FINAL
                Exit Select

            Case Windows.Forms.Keys.KanjiMode
                virtualKey = VK_KANJI
                Exit Select

            Case Windows.Forms.Keys.Escape
                virtualKey = VK_ESCAPE
                Exit Select

            Case Windows.Forms.Keys.IMEConvert
                virtualKey = VK_CONVERT
                Exit Select

            Case Windows.Forms.Keys.IMENonconvert
                virtualKey = VK_NONCONVERT
                Exit Select

            Case Windows.Forms.Keys.IMEAccept
                virtualKey = VK_ACCEPT
                Exit Select

            Case Windows.Forms.Keys.IMEModeChange
                virtualKey = VK_MODECHANGE
                Exit Select

            Case Windows.Forms.Keys.Space
                virtualKey = VK_SPACE
                Exit Select

            Case Windows.Forms.Keys.Prior
                virtualKey = VK_PRIOR
                Exit Select

            Case Windows.Forms.Keys.[Next]
                virtualKey = VK_NEXT
                Exit Select

            Case Windows.Forms.Keys.[End]
                virtualKey = VK_END
                Exit Select

            Case Windows.Forms.Keys.Home
                virtualKey = VK_HOME
                Exit Select

            Case Windows.Forms.Keys.Left
                virtualKey = VK_LEFT
                Exit Select

            Case Windows.Forms.Keys.Up
                virtualKey = VK_UP
                Exit Select

            Case Windows.Forms.Keys.Right
                virtualKey = VK_RIGHT
                Exit Select

            Case Windows.Forms.Keys.Down
                virtualKey = VK_DOWN
                Exit Select

            Case Windows.Forms.Keys.[Select]
                virtualKey = VK_SELECT
                Exit Select

            Case Windows.Forms.Keys.Print
                virtualKey = VK_PRINT
                Exit Select

            Case Windows.Forms.Keys.Execute
                virtualKey = VK_EXECUTE
                Exit Select

            Case Windows.Forms.Keys.Snapshot
                virtualKey = VK_SNAPSHOT
                Exit Select

            Case Windows.Forms.Keys.Insert
                virtualKey = VK_INSERT
                Exit Select

            Case Windows.Forms.Keys.Delete
                virtualKey = VK_DELETE
                Exit Select

            Case Windows.Forms.Keys.Help
                virtualKey = VK_HELP
                Exit Select

            Case Windows.Forms.Keys.D0
                virtualKey = VK_0
                Exit Select

            Case Windows.Forms.Keys.D1
                virtualKey = VK_1
                Exit Select

            Case Windows.Forms.Keys.D2
                virtualKey = VK_2
                Exit Select

            Case Windows.Forms.Keys.D3
                virtualKey = VK_3
                Exit Select

            Case Windows.Forms.Keys.D4
                virtualKey = VK_4
                Exit Select

            Case Windows.Forms.Keys.D5
                virtualKey = VK_5
                Exit Select

            Case Windows.Forms.Keys.D6
                virtualKey = VK_6
                Exit Select

            Case Windows.Forms.Keys.D7
                virtualKey = VK_7
                Exit Select

            Case Windows.Forms.Keys.D8
                virtualKey = VK_8
                Exit Select

            Case Windows.Forms.Keys.D9
                virtualKey = VK_9
                Exit Select

            Case Windows.Forms.Keys.A
                virtualKey = VK_A
                Exit Select

            Case Windows.Forms.Keys.B
                virtualKey = VK_B
                Exit Select

            Case Windows.Forms.Keys.C
                virtualKey = VK_C
                Exit Select

            Case Windows.Forms.Keys.D
                virtualKey = VK_D
                Exit Select

            Case Windows.Forms.Keys.E
                virtualKey = VK_E
                Exit Select

            Case Windows.Forms.Keys.F
                virtualKey = VK_F
                Exit Select

            Case Windows.Forms.Keys.G
                virtualKey = VK_G
                Exit Select

            Case Windows.Forms.Keys.H
                virtualKey = VK_H
                Exit Select

            Case Windows.Forms.Keys.I
                virtualKey = VK_I
                Exit Select

            Case Windows.Forms.Keys.J
                virtualKey = VK_J
                Exit Select

            Case Windows.Forms.Keys.K
                virtualKey = VK_K
                Exit Select

            Case Windows.Forms.Keys.L
                virtualKey = VK_L
                Exit Select

            Case Windows.Forms.Keys.M
                virtualKey = VK_M
                Exit Select

            Case Windows.Forms.Keys.N
                virtualKey = VK_N
                Exit Select

            Case Windows.Forms.Keys.O
                virtualKey = VK_O
                Exit Select

            Case Windows.Forms.Keys.P
                virtualKey = VK_P
                Exit Select

            Case Windows.Forms.Keys.Q
                virtualKey = VK_Q
                Exit Select

            Case Windows.Forms.Keys.R
                virtualKey = VK_R
                Exit Select

            Case Windows.Forms.Keys.S
                virtualKey = VK_S
                Exit Select

            Case Windows.Forms.Keys.T
                virtualKey = VK_T
                Exit Select

            Case Windows.Forms.Keys.U
                virtualKey = VK_U
                Exit Select

            Case Windows.Forms.Keys.V
                virtualKey = VK_V
                Exit Select

            Case Windows.Forms.Keys.W
                virtualKey = VK_W
                Exit Select

            Case Windows.Forms.Keys.X
                virtualKey = VK_X
                Exit Select

            Case Windows.Forms.Keys.Y
                virtualKey = VK_Y
                Exit Select

            Case Windows.Forms.Keys.Z
                virtualKey = VK_Z
                Exit Select

            Case Windows.Forms.Keys.LWin
                virtualKey = VK_LWIN
                Exit Select

            Case Windows.Forms.Keys.RWin
                virtualKey = VK_RWIN
                Exit Select

            Case Windows.Forms.Keys.Apps
                virtualKey = VK_APPS
                Exit Select

            Case Windows.Forms.Keys.Sleep
                virtualKey = VK_SLEEP
                Exit Select

            Case Windows.Forms.Keys.NumPad0
                virtualKey = VK_NUMPAD0
                Exit Select

            Case Windows.Forms.Keys.NumPad1
                virtualKey = VK_NUMPAD1
                Exit Select

            Case Windows.Forms.Keys.NumPad2
                virtualKey = VK_NUMPAD2
                Exit Select

            Case Windows.Forms.Keys.NumPad3
                virtualKey = VK_NUMPAD3
                Exit Select

            Case Windows.Forms.Keys.NumPad4
                virtualKey = VK_NUMPAD4
                Exit Select

            Case Windows.Forms.Keys.NumPad5
                virtualKey = VK_NUMPAD5
                Exit Select

            Case Windows.Forms.Keys.NumPad6
                virtualKey = VK_NUMPAD6
                Exit Select

            Case Windows.Forms.Keys.NumPad7
                virtualKey = VK_NUMPAD7
                Exit Select

            Case Windows.Forms.Keys.NumPad8
                virtualKey = VK_NUMPAD8
                Exit Select

            Case Windows.Forms.Keys.NumPad9
                virtualKey = VK_NUMPAD9
                Exit Select

            Case Windows.Forms.Keys.Multiply
                virtualKey = VK_MULTIPLY
                Exit Select

            Case Windows.Forms.Keys.Add
                virtualKey = VK_ADD
                Exit Select

            Case Windows.Forms.Keys.Separator
                virtualKey = VK_SEPARATOR
                Exit Select

            Case Windows.Forms.Keys.Subtract
                virtualKey = VK_SUBTRACT
                Exit Select

            Case Windows.Forms.Keys.[Decimal]
                virtualKey = VK_DECIMAL
                Exit Select

            Case Windows.Forms.Keys.Divide
                virtualKey = VK_DIVIDE
                Exit Select

            Case Windows.Forms.Keys.F1
                virtualKey = VK_F1
                Exit Select

            Case Windows.Forms.Keys.F2
                virtualKey = VK_F2
                Exit Select

            Case Windows.Forms.Keys.F3
                virtualKey = VK_F3
                Exit Select

            Case Windows.Forms.Keys.F4
                virtualKey = VK_F4
                Exit Select

            Case Windows.Forms.Keys.F5
                virtualKey = VK_F5
                Exit Select

            Case Windows.Forms.Keys.F6
                virtualKey = VK_F6
                Exit Select

            Case Windows.Forms.Keys.F7
                virtualKey = VK_F7
                Exit Select

            Case Windows.Forms.Keys.F8
                virtualKey = VK_F8
                Exit Select

            Case Windows.Forms.Keys.F9
                virtualKey = VK_F9
                Exit Select

            Case Windows.Forms.Keys.F10
                virtualKey = VK_F10
                Exit Select

            Case Windows.Forms.Keys.F11
                virtualKey = VK_F11
                Exit Select

            Case Windows.Forms.Keys.F12
                virtualKey = VK_F12
                Exit Select

            Case Windows.Forms.Keys.F13
                virtualKey = VK_F13
                Exit Select

            Case Windows.Forms.Keys.F14
                virtualKey = VK_F14
                Exit Select

            Case Windows.Forms.Keys.F15
                virtualKey = VK_F15
                Exit Select

            Case Windows.Forms.Keys.F16
                virtualKey = VK_F16
                Exit Select

            Case Windows.Forms.Keys.F17
                virtualKey = VK_F17
                Exit Select

            Case Windows.Forms.Keys.F18
                virtualKey = VK_F18
                Exit Select

            Case Windows.Forms.Keys.F19
                virtualKey = VK_F19
                Exit Select

            Case Windows.Forms.Keys.F20
                virtualKey = VK_F20
                Exit Select

            Case Windows.Forms.Keys.F21
                virtualKey = VK_F21
                Exit Select

            Case Windows.Forms.Keys.F22
                virtualKey = VK_F22
                Exit Select

            Case Windows.Forms.Keys.F23
                virtualKey = VK_F23
                Exit Select

            Case Windows.Forms.Keys.F24
                virtualKey = VK_F24
                Exit Select

            Case Windows.Forms.Keys.NumLock
                virtualKey = VK_NUMLOCK
                Exit Select

            Case Windows.Forms.Keys.Scroll
                virtualKey = VK_SCROLL
                Exit Select

            Case Windows.Forms.Keys.LShiftKey
                virtualKey = VK_LSHIFT
                Exit Select

            Case Windows.Forms.Keys.RShiftKey
                virtualKey = VK_RSHIFT
                Exit Select

            Case Windows.Forms.Keys.LControlKey
                virtualKey = VK_LCONTROL
                Exit Select

            Case Windows.Forms.Keys.RControlKey
                virtualKey = VK_RCONTROL
                Exit Select

            Case 164
                virtualKey = VK_LMENU
                Exit Select

            Case 165
                virtualKey = VK_RMENU
                Exit Select

            Case Windows.Forms.Keys.BrowserBack
                virtualKey = VK_BROWSER_BACK
                Exit Select

            Case Windows.Forms.Keys.BrowserForward
                virtualKey = VK_BROWSER_FORWARD
                Exit Select

            Case Windows.Forms.Keys.BrowserRefresh
                virtualKey = VK_BROWSER_REFRESH
                Exit Select

            Case Windows.Forms.Keys.BrowserStop
                virtualKey = VK_BROWSER_STOP
                Exit Select

            Case Windows.Forms.Keys.BrowserSearch
                virtualKey = VK_BROWSER_SEARCH
                Exit Select

            Case Windows.Forms.Keys.BrowserFavorites
                virtualKey = VK_BROWSER_FAVORITES
                Exit Select

            Case Windows.Forms.Keys.BrowserHome
                virtualKey = VK_BROWSER_HOME
                Exit Select

            Case Windows.Forms.Keys.VolumeMute
                virtualKey = VK_VOLUME_MUTE
                Exit Select

            Case Windows.Forms.Keys.VolumeDown
                virtualKey = VK_VOLUME_DOWN
                Exit Select

            Case Windows.Forms.Keys.VolumeUp
                virtualKey = VK_VOLUME_UP
                Exit Select

            Case Windows.Forms.Keys.MediaNextTrack
                virtualKey = VK_MEDIA_NEXT_TRACK
                Exit Select

            Case Windows.Forms.Keys.MediaPreviousTrack
                virtualKey = VK_MEDIA_PREV_TRACK
                Exit Select

            Case Windows.Forms.Keys.MediaStop
                virtualKey = VK_MEDIA_STOP
                Exit Select

            Case Windows.Forms.Keys.MediaPlayPause
                virtualKey = VK_MEDIA_PLAY_PAUSE
                Exit Select

            Case Windows.Forms.Keys.LaunchMail
                virtualKey = VK_LAUNCH_MAIL
                Exit Select

            Case Windows.Forms.Keys.SelectMedia
                virtualKey = VK_LAUNCH_MEDIA_SELECT
                Exit Select

            Case Windows.Forms.Keys.LaunchApplication1
                virtualKey = VK_LAUNCH_APP1
                Exit Select

            Case Windows.Forms.Keys.LaunchApplication2
                virtualKey = VK_LAUNCH_APP2
                Exit Select

            Case Windows.Forms.Keys.OemSemicolon
                virtualKey = VK_OEM_1
                Exit Select

            Case Windows.Forms.Keys.Oemplus
                virtualKey = VK_OEM_PLUS
                Exit Select

            Case Windows.Forms.Keys.Oemcomma
                virtualKey = VK_OEM_COMMA
                Exit Select

            Case Windows.Forms.Keys.OemMinus
                virtualKey = VK_OEM_MINUS
                Exit Select

            Case Windows.Forms.Keys.OemPeriod
                virtualKey = VK_OEM_PERIOD
                Exit Select

            Case Windows.Forms.Keys.OemQuestion
                virtualKey = VK_OEM_2
                Exit Select

            Case Windows.Forms.Keys.Oemtilde
                virtualKey = VK_OEM_3
                Exit Select

            Case 193
                virtualKey = VK_C1
                Exit Select

            Case 194
                virtualKey = VK_C2
                Exit Select

            Case Windows.Forms.Keys.OemOpenBrackets
                virtualKey = VK_OEM_4
                Exit Select

            Case Windows.Forms.Keys.OemPipe
                virtualKey = VK_OEM_5
                Exit Select

            Case Windows.Forms.Keys.OemCloseBrackets
                virtualKey = VK_OEM_6
                Exit Select

            Case Windows.Forms.Keys.OemQuotes
                virtualKey = VK_OEM_7
                Exit Select

            Case Windows.Forms.Keys.Oem8
                virtualKey = VK_OEM_8
                Exit Select

            Case Windows.Forms.Keys.OemBackslash
                virtualKey = VK_OEM_102
                Exit Select

            Case 229
                virtualKey = VK_PROCESSKEY
                Exit Select

            Case 240
                ' DbeAlphanumeric
                virtualKey = VK_OEM_ATTN
                ' VK_DBE_ALPHANUMERIC
                Exit Select

            Case 241
                ' DbeKatakana
                virtualKey = VK_OEM_FINISH
                ' VK_DBE_KATAKANA
                Exit Select

            Case 242
                ' DbeHiragana
                virtualKey = VK_OEM_COPY
                ' VK_DBE_HIRAGANA
                Exit Select

            Case 243
                ' DbeSbcsChar
                virtualKey = VK_OEM_AUTO
                ' VK_DBE_SBCSCHAR
                Exit Select

            Case 244
                ' DbeDbcsChar
                virtualKey = VK_OEM_ENLW
                ' VK_DBE_DBCSCHAR
                Exit Select

            Case 245 'No Key here.
                ' DbeRoman
                virtualKey = VK_OEM_BACKTAB
                ' VK_DBE_ROMAN
                Exit Select

            Case Windows.Forms.Keys.Attn
                ' DbeNoRoman
                virtualKey = VK_ATTN
                ' VK_DBE_NOROMAN
                Exit Select

            Case Windows.Forms.Keys.Crsel
                ' DbeEnterWordRegisterMode
                virtualKey = VK_CRSEL
                ' VK_DBE_ENTERWORDREGISTERMODE
                Exit Select

            Case Windows.Forms.Keys.Exsel
                ' EnterImeConfigureMode
                virtualKey = VK_EXSEL
                ' VK_DBE_ENTERIMECONFIGMODE
                Exit Select

            Case Windows.Forms.Keys.EraseEof
                ' DbeFlushString
                virtualKey = VK_EREOF
                ' VK_DBE_FLUSHSTRING
                Exit Select

            Case Windows.Forms.Keys.Play
                ' DbeCodeInput
                virtualKey = VK_PLAY
                ' VK_DBE_CODEINPUT
                Exit Select

            Case Windows.Forms.Keys.Zoom
                ' DbeNoCodeInput
                virtualKey = VK_ZOOM
                ' VK_DBE_NOCODEINPUT
                Exit Select

            Case Windows.Forms.Keys.NoName
                ' DbeDetermineString
                virtualKey = VK_NONAME
                ' VK_DBE_DETERMINESTRING
                Exit Select

            Case Windows.Forms.Keys.Pa1
                ' DbeEnterDlgConversionMode
                virtualKey = VK_PA1
                ' VK_ENTERDLGCONVERSIONMODE
                Exit Select

            Case Windows.Forms.Keys.OemClear
                virtualKey = VK_OEM_CLEAR
                Exit Select

            Case Else

                virtualKey = 0
                Exit Select
        End Select

        Return virtualKey

    End Function

#End Region

    Private Enum MapType As UInteger
        MAPVK_VK_TO_VSC = &H0
        MAPVK_VSC_TO_VK = &H1
        MAPVK_VK_TO_CHAR = &H2
        MAPVK_VSC_TO_VK_EX = &H3
    End Enum

    <Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function ToUnicode(wVirtKey As UInteger, wScanCode As UInteger, lpKeyState As Byte(), pwszBuff As System.Text.StringBuilder, cchBuff As Integer, wFlags As UInteger) As Integer
    End Function

    <Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function GetKeyboardState(lpKeyState As Byte()) As Boolean
    End Function

    <Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function MapVirtualKey(uCode As UInteger, uMapType As MapType) As UInteger
    End Function

    ''' <summary>
    ''' Returns chars representing the keys pressed on a keyboard.
    ''' </summary>
    ''' <param name="key">The key. Returns nothing when no keypress was detected.</param>
    ''' <returns></returns>
    Public Shared Function GetCharFromKey(key As Keys) As Char?
        Return GetCharFromKey(CType(key, Windows.Forms.Keys))
    End Function

    ''' <summary>
    ''' Returns chars representing the keys pressed on a keyboard.
    ''' </summary>
    ''' <param name="key">The key. Returns nothing when no keypress was detected.</param>
    ''' <returns></returns>
    Public Shared Function GetCharFromKey(key As Windows.Forms.Keys) As Char?
        Dim ch As Char? = Nothing

        Dim virtualKey As Integer = GetVirtualKeyFromKey(key)
        Dim keyboardState As Byte() = New Byte(255) {}
        GetKeyboardState(keyboardState)

        Dim scanCode As UInteger = MapVirtualKey(CUInt(virtualKey), MapType.MAPVK_VK_TO_VSC)
        Dim stringBuilder As New System.Text.StringBuilder(2)

        Dim result As Integer = ToUnicode(CUInt(virtualKey), scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0)
        Select Case result
            Case -1
                Exit Select
            Case 0
                Exit Select
            Case 1
                ch = stringBuilder(0)
            Case Else
                ch = stringBuilder(0)
        End Select
        Return ch
    End Function

End Class