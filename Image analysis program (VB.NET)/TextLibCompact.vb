Public Class TextLibCompact

    Public Function Get_String2DAarry_From_LongString(ByVal SourceStr As String,
                                                      ByVal DelimitChar As String,
                                                      ByVal ExcludeEntireLineWhenNullCellDataFound As Boolean) As String(,)

        Dim RowCount As Integer, ColumnCount As Integer
        Dim LineArray(0) As String
        Dim ItemArray(0) As String
        Dim TableArray(0, 0) As String          'Actual data starts from (column index 1, row index 1)
        Dim Cur_Column, Cur_Row As Integer
        Dim EffectiveRowCount As Integer
        Dim Cur_EffectiveRow As Integer
        Dim IsNullCellDataFound As Boolean

        If SourceStr = "" Then Return Nothing



        'Split every line
        LineArray = Split(SourceStr, vbCrLf)
        RowCount = LineArray.GetUpperBound(0) + 1


        'Calculate effective line that doesn't have empty string
        EffectiveRowCount = 0
        For Cur_Row = 1 To RowCount
            If LineArray(Cur_Row - 1).Trim <> "" Then
                EffectiveRowCount += 1
            End If
        Next


        'Calculate column count
        ItemArray = Split(LineArray(0), DelimitChar)
        ColumnCount = ItemArray.GetUpperBound(0) + 1


        'Define dimension
        ReDim TableArray(ColumnCount, EffectiveRowCount)


        Cur_EffectiveRow = 1

        For Cur_Row = 1 To RowCount

            If LineArray(Cur_Row - 1).Trim <> "" Then
                ItemArray = Split(LineArray(Cur_Row - 1), DelimitChar)


                IsNullCellDataFound = False
                If ColumnCount <= ItemArray.GetUpperBound(0) + 1 Then
                    For Cur_Column = 1 To ColumnCount
                        TableArray(Cur_Column, Cur_EffectiveRow) = ItemArray(Cur_Column - 1).Trim
                        If TableArray(Cur_Column, Cur_EffectiveRow) = "" Then
                            IsNullCellDataFound = True
                        End If
                    Next
                Else
                    For Cur_Column = 1 To ItemArray.GetUpperBound(0) + 1
                        TableArray(Cur_Column, Cur_EffectiveRow) = ItemArray(Cur_Column - 1).Trim
                        If TableArray(Cur_Column, Cur_EffectiveRow) = "" Then
                            IsNullCellDataFound = True
                        End If
                    Next

                    For Cur_Column = ItemArray.GetUpperBound(0) + 2 To ColumnCount
                        TableArray(Cur_Column, Cur_EffectiveRow) = ""
                    Next
                End If



                If ExcludeEntireLineWhenNullCellDataFound And IsNullCellDataFound Then
                Else
                    Cur_EffectiveRow += 1
                End If

            End If
        Next


        If Cur_EffectiveRow = 1 Then Return Nothing



        ReDim Preserve TableArray(ColumnCount, Cur_EffectiveRow - 1)

        Return TableArray

    End Function


    Public Function Get_String2DAarry_From_TextFile(ByVal Par_Filename As String,
                                                    ByVal DelimitStr As String,
                                                    ByVal ExcludeEntireLineWhenNullCellDataFound As Boolean) As String(,)

        Dim Buf_String As String
        Dim Buf_Table(,) As String

        Try
            'Buf_String = My.Computer.FileSystem.ReadAllText(Par_Filename)
            Buf_String = MyFileSysEng.ReadAllText(Par_Filename)
            Buf_Table = Get_String2DAarry_From_LongString(Buf_String, DelimitStr, ExcludeEntireLineWhenNullCellDataFound)
        Catch
            Return Nothing
        End Try

        Return Buf_Table
    End Function

End Class
