Option Strict On
Imports Kolben.Adapters

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="Daycare")>
    Friend NotInheritable Class DaycarePrototype

        <ScriptVariable(VariableName:="daycareId")>
        Public daycareId As Integer

        Public Sub New() : End Sub

        Public Sub New(daycareId As Integer)
            Me.daycareId = daycareId
        End Sub

        <ScriptFunction(ScriptFunctionType.Constructor, VariableName:="constructor")>
        Public Shared Function Constructor(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(Integer)) Then

                objLink.SetMember("daycareId", CType(parameters(0), Integer))

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.IndexerGet, VariableName:="indexerGet")>
        Public Shared Function IndexerGet(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim pokemonIndex = CType(parameters(0), Integer)
            Dim daycareId = CType(This, DaycarePrototype).daycareId

            For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                If line.StartsWith(daycareId.ToString() & "|" & pokemonIndex.ToString() & "|") Then

                    Dim data = line.Remove(0, line.IndexOf("{"))
                    Dim p = Pokemon.GetPokemonByData(data)

                    Return New PokemonPrototype(p)

                End If
            Next

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="getGrownLevels")>
        Public Shared Function GetGrownLevels(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {GetType(Integer)}) Then

                Dim pokemonIndex = CType(parameters(0), Integer)
                Dim daycareId = CType(This, DaycarePrototype).daycareId

                For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                    If line.StartsWith(daycareId.ToString() & "|" & pokemonIndex.ToString() & "|") Then

                        Dim data = line.Remove(0, line.IndexOf("{"))
                        Dim startStep = CInt(line.Split(CChar("|"))(2))
                        Dim p = Pokemon.GetPokemonByData(data)
                        Dim startLevel = p.Level

                        p.GetExperience(Core.Player.DaycareSteps - startStep, True)

                        Return p.Level - startLevel

                    End If
                Next

            End If

            Return 0

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="canBreed")>
        Public Shared Function GetCanBreed(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim daycareId = CType(This, DaycarePrototype).daycareId
            Return Daycare.CanBreed(daycareId)

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="hasEgg")>
        Public Shared Function GetHasEgg(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim daycareId = CType(This, DaycarePrototype).daycareId

            Return Core.Player.DaycareData.SplitAtNewline().Any(Function(line As String)
                                                                    Return line.StartsWith(daycareId.ToString() & "|Egg|")
                                                                End Function)

        End Function

        Private Shared Function GetDaycarePokemonCount(daycareId As Integer) As Integer

            If Core.Player.DaycareData <> "" Then

                Return Core.Player.DaycareData.SplitAtNewline().Count(Function(line As String)
                                                                          Return line.StartsWith(daycareId.ToString() & "|")
                                                                      End Function)

            End If

            Return 0

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="pokemonCount")>
        Public Shared Function GetPokemonCount(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim daycareId = CType(This, DaycarePrototype).daycareId

            Return GetDaycarePokemonCount(daycareId)

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="takeEgg")>
        Public Shared Function TakeEgg(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim daycareId = CType(This, DaycarePrototype).daycareId

            Dim newData As String = ""
            Dim eggPokemon As Pokemon = Nothing

            For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                If line.StartsWith(daycareId.ToString() & "|Egg|") = False Then
                    If newData <> "" Then
                        newData &= Environment.NewLine
                    End If
                    newData &= line
                Else
                    eggPokemon = Daycare.ProduceEgg(daycareId)
                End If
            Next


            If eggPokemon Is Nothing Then
                Return Nothing
            Else

                Core.Player.DaycareData = newData
                Return New PokemonPrototype(eggPokemon)

            End If

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="takePokemon")>
        Public Shared Function TakePokemon(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, GetType(Integer)) Then

                Dim daycareId = CType(This, DaycarePrototype).daycareId
                Dim pokemonIndex = CType(parameters(0), Integer)
                Dim newData As String = ""
                Dim takenPokemon As Pokemon = Nothing

                For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                    If line.StartsWith(daycareId.ToString() & "|" & pokemonIndex.ToString() & "|") = True Then

                        Dim data As String = line.Remove(0, line.IndexOf("{"))
                        Dim startStep As Integer = CInt(line.Split(CChar("|"))(2))
                        takenPokemon = Pokemon.GetPokemonByData(data)

                        takenPokemon.GetExperience(Core.Player.DaycareSteps - startStep, True)

                    Else

                        If newData <> "" Then
                            newData &= Environment.NewLine
                        End If
                        newData &= line

                    End If
                Next

                If takenPokemon Is Nothing Then
                    Return Nothing
                Else

                    Core.Player.DaycareData = newData
                    Return New PokemonPrototype(takenPokemon)

                End If

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="putPokemon")>
        Public Shared Function PutPokemon(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {GetType(Integer), GetType(PokemonPrototype)}) Then

                Dim daycareId = CType(This, DaycarePrototype).daycareId
                Dim daycareIndex As Integer = CType(parameters(0), Integer)
                Dim wrapper = CType(parameters(1), PokemonPrototype)
                Dim pokemonData = PokemonPrototype.GetPokemon(wrapper).GetSaveData()

                If Core.Player.DaycareData <> "" Then
                    Core.Player.DaycareData &= Environment.NewLine
                End If

                Core.Player.DaycareData &= daycareId.ToString() & "|" & daycareIndex.ToString() & "|" & Core.Player.DaycareSteps & "|0|" & pokemonData

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="cleanData")>
        Public Shared Function CleanData(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim daycareId = CType(This, DaycarePrototype).daycareId
            Dim newData As String = ""

            Dim lines As New List(Of String)

            For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                If line.StartsWith(daycareId & "|") = True Then
                    lines.Add(line)
                Else
                    If newData <> "" Then
                        newData &= Environment.NewLine
                    End If
                    newData &= line
                End If
            Next

            For i = 0 To lines.Count - 1
                Dim line As String = lines(i)
                Dim data() As String = line.Split(CChar("|"))

                If newData <> "" Then
                    newData &= Environment.NewLine
                End If

                If data(1) = "Egg" Then
                    newData &= daycareId.ToString() & "|Egg|" & data(2)
                Else
                    newData &= daycareId.ToString() & "|" & i.ToString() & "|" & data(2) & "|" & data(3) & "|" & line.Remove(0, line.IndexOf("{"))
                End If
            Next

            Core.Player.DaycareData = newData

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="clear")>
        Public Shared Function Clear(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim daycareId = CType(This, DaycarePrototype).daycareId
            Dim newData As String = ""

            For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                If line.StartsWith(daycareId.ToString() & "|") = False Then
                    If newData <> "" Then
                        newData &= Environment.NewLine
                    End If
                    newData &= line
                End If
            Next

            Core.Player.DaycareData = newData

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="call")>
        Public Shared Function CallPhone(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim daycareId = CType(This, DaycarePrototype).daycareId
            Daycare.TriggerCall(daycareId)
            Return NetUndefined.Instance

        End Function

    End Class

End Namespace
