Sub writefile()

Dim FileNo As Integer
Dim fn As String

fn = ThisWorkbook.Path & "\" & ThisWorkbook.Name & ".txt"
FileNo = FreeFile

Open fn For Output Access Write As FileNo

Worksheets("ToDataFile").Activate

Dim RowCount As Long
Dim ColumnCount As Long
Dim row
Dim col

RowCount = ActiveSheet.UsedRange.Rows.Count
ColumnCount = ActiveSheet.UsedRange.Columns.Count

For Each row In ActiveSheet.UsedRange.Rows
    outrow = ""
    For Each col In row.Cells
        outrow = outrow & col.Value & vbTab
    Next col
    If Len(outrow) > 1 Then
        Print #FileNo, outrow
    End If
Next row

Close FileNo

End Sub
