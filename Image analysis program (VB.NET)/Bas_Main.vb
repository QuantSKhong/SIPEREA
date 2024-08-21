
Imports System.IO

Module Bas_Main


    Public WithEvents MyAnalyzer As New AreaAnalyzer
    Public WithEvents MyFileSysEng As New FileSystemEngine
    Public WithEvents MyImgProc As New CompactImgProcessing
    Public WithEvents MyTextLib As New TextLibCompact
    Public OutputFilename As String = "resultArea.csv"
    Public PathDelimit As String = "\"

    Public Structure Type_ROI
        Dim Condition1 As String
        Dim Condition2 As String
        Dim TestDate As String
        Dim Plate As String
        Dim Well As String
        Dim Shape As String
        Dim Boundary As Rectangle
    End Structure

    Public ROI() As Type_ROI
    Public Filename_ROI As String = "ROI.csv"


    Public Sub Set_DefaultROI(ImageWidth, ImageHeight)
        ReDim ROI(0)

        With ROI(0)
            .Condition1 = ""
            .Condition2 = ""
            .Shape = "rectangle"
            .Well = ""
            .Plate = ""
            .TestDate = ""
            .Boundary = New Rectangle(0, 0, ImageWidth, ImageHeight)
        End With
    End Sub

    Public Function Read_ROI(SourceImageFileFullPath As String,
                             Optional delimitStr As String = ",") As String

        Dim TextTable(,) As String
        Dim TextTable_RowCount As Integer
        Dim TextTable_ColCount As Integer

        Dim RegionInfoFullPath As String =
                    MyFileSysEng.Get_OnlyPath_FullFileName(SourceImageFileFullPath) &
                                            PathDelimit & Filename_ROI

        If MyFileSysEng.FileExists(RegionInfoFullPath) Then
            TextTable = MyTextLib.Get_String2DAarry_From_TextFile(RegionInfoFullPath,
                                                          delimitStr,
                                                          False)
        Else

            Dim IndividualROIFileFullPath As String =
                   MyFileSysEng.Get_OnlyPath_FullFileName(SourceImageFileFullPath) +
                   PathDelimit +
                   MyFileSysEng.Get_FilenameWithoutExtension_From_ShortFileName(SourceImageFileFullPath) +
                   "-ROI.csv"

            If MyFileSysEng.FileExists(IndividualROIFileFullPath) Then
                TextTable = MyTextLib.Get_String2DAarry_From_TextFile(IndividualROIFileFullPath,
                                                                  delimitStr,
                                                                  False)
            Else

                ROI = Nothing
                Return "ROI file not found" + vbCrLf + RegionInfoFullPath
            End If
        End If



        TextTable_RowCount = TextTable.GetUpperBound(1)
        TextTable_ColCount = TextTable.GetUpperBound(0)
        ReDim ROI(TextTable_RowCount - 2)


        If TextTable_ColCount <> 10 Then
            Return "Invalid ROI format"
        End If

        For q As Integer = 2 To TextTable_RowCount
            Try
                With ROI(q - 2)
                    .Condition1 = TextTable(1, q)
                    .Condition2 = TextTable(2, q)
                    .TestDate = TextTable(3, q)
                    .Plate = TextTable(4, q)
                    .Well = TextTable(5, q)
                    .Shape = TextTable(6, q).ToLower
                    .Boundary = New Rectangle(CInt(TextTable(7, q)),
                                               CInt(TextTable(8, q)),
                                               CInt(TextTable(9, q)) - CInt(TextTable(7, q)) + 1,
                                               CInt(TextTable(10, q)) - CInt(TextTable(8, q)) + 1)
                End With

            Catch
                ROI = Nothing
                Return "Invalid region: Check " & Get_nth(q) + " line of the " &
                                delimitStr & RegionInfoFullPath
            End Try
        Next

        Return ""
    End Function

    Public Function Get_nth(Number As Integer) As String
        If Number = 0 Then Return "zero"
        If Number = 1 Then Return "1st"
        If Number = 2 Then Return "2nd"

        Return Number.ToString.Trim + "th"
    End Function

    Public Function Get_DateFromFilename(Filename As String) As Date
        If Mid(Filename, 5, 1) <> "-" Then Return Nothing
        If Mid(Filename, 8, 1) <> "-" Then Return Nothing
        If Mid(Filename, 11, 2) <> " (" Then Return Nothing
        If Mid(Filename, 15, 1) <> "-" Then Return Nothing
        If Mid(Filename, 18, 1) <> "-" Then Return Nothing

        Dim y As Integer = CInt(Mid(Filename, 1, 4))
        Dim m As Integer = CInt(Mid(Filename, 6, 2))
        Dim d As Integer = CInt(Mid(Filename, 9, 2))
        Dim mm As Integer = CInt(Mid(Filename, 13, 2))
        Dim h As Integer = CInt(Mid(Filename, 16, 2))
        Dim s As Integer = CInt(Mid(Filename, 19, 2))

        Return New Date(y, m, d, mm, h, s)
    End Function

    Public Sub UpdateStatus(Msg As String, ProgressValue As Integer)
        With MDI_Main
            .Status_Progressbar.Value = ProgressValue
            .Status_Progressbar.Visible = True
            .Status_Info.Text = Msg
            '.StatusStrip_Status.Update()
            .StatusStrip_Status.Refresh()
        End With
    End Sub




    Private Sub MyAnalyzer_ProgressStatus(Msg As String, ProgressValue As Integer) Handles MyAnalyzer.ProgressStatus

        UpdateStatus(Msg, ProgressValue)

    End Sub


    Public Function Get_ROIinfo(ChemicalIndex As Integer,
                                     IsIncludeDate As Boolean,
                                     Optional IDSeperator As String = "|") As String
        Dim RetStr As String

        RetStr = ROI(ChemicalIndex).Condition1 + IDSeperator +
                     ROI(ChemicalIndex).Condition2


        If ROI(ChemicalIndex).Plate <> "" Then
            RetStr &= IDSeperator & "P" & ROI(ChemicalIndex).Plate
        End If

        If ROI(ChemicalIndex).Well <> "" Then
            RetStr &= IDSeperator & "W" & ROI(ChemicalIndex).Well
        End If

        If IsIncludeDate Then
            RetStr &= "|" +
                     ROI(ChemicalIndex).TestDate
        End If


        Return RetStr
    End Function
End Module
