Imports System.Data.OleDb
Public Class PDGrid
    Public gdc1 As DataGridView
    Public gdc2 As DataGridView
    Public colsAlign() As String
    Public Sub Dump(table As String, id As Integer, nbcols As Integer)
        If nbcols > 80 Then
            MsgBox("Erreur lors de l'opération dump: cette table est très grande.")
        Else
            Dim req As String = "INSERT INTO C_DUMP VALUES('" + table + "', '" + id.ToString + "',"
            Dim nbv As Integer = 2
            Dim req2 As String = "SELECT * FROM " + table + " WHERE id='" + id.ToString + "'"
            Dim reqc2 As OleDbCommand = New OleDbCommand(req2, CNN)
            Dim res2 As OleDbDataReader = Nothing
            Try
                res2 = reqc2.ExecuteReader()
                While res2.Read
                    Dim val As String = ""
                    For i As Integer = 0 To nbcols - 1
                        Dim vv As String = res2(i).ToString
                        vv = vv.Replace("'", "''")
                        val = "'" + vv + "',"
                        req = req + val
                    Next
                End While
                For i As Integer = nbcols To 31
                    req = req + "'',"
                Next
                req = req.Substring(0, req.Length - 1)
                req = req + ")"
                res2.Close()
            Catch ex As Exception
                MsgBox("Erreur de l'opération dump (CODE:001): " + ex.Message)
                Try
                    res2.Close()
                Catch ex2 As Exception

                End Try
            End Try
            Dim reqc As OleDbCommand = New OleDbCommand(req, CNN)
            Dim res As OleDbDataReader = Nothing
            Try
                res = reqc.ExecuteReader()
                res.Read()
                res.Close()
            Catch ex As Exception
                MsgBox("Erreur de l'opération dump (CODE:002): " + ex.Message)
                Try
                    res.Close()
                Catch ex2 As Exception

                End Try
            End Try
        End If
    End Sub
    Public Overridable Sub StartConnection(str As String)
        CNN = New OleDbConnection(str)
        Dim ok As Boolean
        Try
            CNN.Open()
            'Form1.msgconn = "Connexion au serveur éffectuée avec succès."
            ok = True
        Catch ex As Exception
            'Form1.msgconn = "Une erreur est survenue lors de la tentative de connexion au serveur principale:" + vbNewLine + ex.Message
        End Try
        If ok Then
            Dim req As String = "Select * From DOSSIER"
            Dim reqc As OleDbCommand = New OleDbCommand(req, CNN)
            Dim res As OleDbDataReader = reqc.ExecuteReader()
            Dim nb As Integer = 0
            While res.Read()
                nb = nb + 1
            End While
            If nb = 0 Then
                MsgBox("Celle-ci est votre proemière utilisation du système, Vous allez passer en mode de configuration.")
                Try
                    'fm_DétailsSocieteInit.Show()
                Catch ex As Exception

                End Try
            Else
                'fm_login.show()
            End If
            res.Close()
        Else
            'fm_ConnectionError.Show()
        End If
    End Sub
    Public dataadapter As OleDbDataAdapter
    Public ds As New DataSet()
    Public Overridable Sub LoadTableIntoGrid(table As String, grd As DataGridView, Optional filter As String = "1=1")
        If filter = "" Then
            filter = "1=1"
        Else
            'filter = filter.Replace("'", "''")
        End If
        Dim sql As String = "SELECT * FROM " + table + " WHERE " + filter
        dataadapter = New OleDbDataAdapter(sql, CNN)
        Try
            dataadapter.Fill(ds, "CONFIG")
            grd.DataSource = ds.Tables(0)
        Catch ex As Exception
            MsgBox("Erreur lors du chargement de données: " + ex.Message)
        End Try
    End Sub
    Public Overridable Sub UpdateTableFromGrid(table As String)
        Dim cm As New OleDbCommandBuilder(dataadapter)
        Try
            dataadapter.Update(ds, table)
        Catch ex As Exception
            MsgBox("Erreur lors de la mise à jour de données: " + ex.Message)
        End Try
    End Sub
    Public Overridable Sub saveGridToTable(grid As DataGridView, table As String, cols() As Integer)
        For Each row As DataGridViewRow In grid.Rows
            Dim values() As String = Nothing
            For i = 0 To row.Cells.Count - 1
                If cols.Contains(i) Then
                    If values Is Nothing Then
                        ReDim values(0)
                        values(0) = row.Cells(i).Value.ToString
                    Else
                        ReDim values(UBound(values) + 1)
                        values(UBound(values)) = row.Cells(i).Value.ToString
                    End If
                Else
                    If values Is Nothing Then
                        ReDim values(0)
                        values(0) = ""
                    Else
                        ReDim values(UBound(values) + 1)
                        values(UBound(values)) = ""
                    End If
                End If
            Next
            If values IsNot Nothing Then
                Dim sql As New SQLFunctions
                sql.SqlInsert(table, values)
            End If
        Next
    End Sub
    Public Overridable Sub ApplyGridTheme(grid As DataGridView, theme As String, titles() As String, cols() As Integer)
        'delete the last empty row:
        grid.AllowUserToAddRows = False
        'Hide unwanted columns               
        For index = 0 To grid.Columns.Count - 1
            grid.Columns(index).Visible = False
        Next
        For index = 0 To cols.GetUpperBound(0) - 1
            Try
                grid.Columns(cols(index)).Visible = True
            Catch ex As Exception

            End Try
        Next
        'Apply the selected theme
        Dim req As String = "Select * From THEMES WHERE Theme = '1'"
        Dim reqc As OleDbCommand = New OleDbCommand(req, CNN)
        Dim res As OleDbDataReader = Nothing
        Try
            res = reqc.ExecuteReader()
            'Dim lignes1color As String = res(1)
            'Dim lignes2color As String = res(2)

            Dim i As Integer = 1
            For Each rw As DataGridViewRow In grid.Rows
                If i = 1 Then
                    rw.DefaultCellStyle.BackColor = Color.LightBlue
                    i = 2
                Else
                    rw.DefaultCellStyle.BackColor = Color.White
                    i = 1
                End If
            Next
            res.Close()
        Catch ex As Exception
            MsgBox("Erreur d'ouverture du thème: " + ex.Message)
            Try
                res.Close()
            Catch ex2 As Exception

            End Try
        End Try
        'Activate full row selection
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grid.MultiSelect = False
        'Hide the first column and apply column titles
        grid.RowHeadersVisible = False
        For index = 0 To titles.GetUpperBound(0)
            If titles(index) <> "NoChange" Then
                Try
                    grid.Columns(index).HeaderCell.Value = titles(index)
                Catch ex As Exception

                End Try
            End If
        Next
        'Other Standard options:
        'Stretch the columns to fill the page
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        'Center the text inside the columns
        For index = 0 To grid.Columns.Count
            Try
                Dim ok As Boolean = False
                If colsAlign Is Nothing Then
                    grid.Columns(index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                Else
                    For i = 0 To UBound(colsAlign)
                        If i = index Then
                            If colsAlign(i) = "center" Then
                                grid.Columns(index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                            ElseIf colsAlign(i) = "left" Then
                                grid.Columns(index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                            ElseIf colsAlign(i) = "right" Then
                                grid.Columns(index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                            Else
                                grid.Columns(index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                            End If
                            ok = True
                        End If
                    Next
                    If Not ok Then
                        grid.Columns(index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    End If
                End If
                'Disable edit and sort
                grid.Columns(index).SortMode = DataGridViewColumnSortMode.NotSortable
                grid.Columns(index).ReadOnly = True
            Catch ex As Exception

            End Try
        Next
        'Column headers customization        
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        Dim gfont As Font
        gfont = New Font(grid.Font.Name, 9, FontStyle.Bold)
        grid.ColumnHeadersDefaultCellStyle.Font = New Font(gfont, FontStyle.Bold)
        grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        grid.AllowUserToResizeRows = False
        grid.ColumnHeadersHeight = 35
        grid.EnableHeadersVisualStyles = False

    End Sub
    Public Overridable Sub SetClickableGrid(grid As DataGridView, grid2 As DataGridView, Col1 As Integer, Col2 As Integer)
        'delete the last empty row:
        grid.AllowUserToAddRows = False
        'Hide unwanted columns               
        For index = 0 To grid.Columns.Count - 1
            If index <> Col1 And index <> Col2 Then
                grid.Columns(index).Visible = False
            End If
        Next
        'Apply the selected theme
        Dim req As String = "Select * From THEMES WHERE Theme = '1'"
        Dim reqc As OleDbCommand = New OleDbCommand(req, CNN)
        Dim res As OleDbDataReader = Nothing
        Try
            res = reqc.ExecuteReader()
            'Dim lignes1color As String = res(1)
            'Dim lignes2color As String = res(2)

            Dim i As Integer = 1
            For Each rw As DataGridViewRow In grid.Rows
                If i = 1 Then
                    rw.DefaultCellStyle.BackColor = Color.LightCyan
                    i = 2
                Else
                    rw.DefaultCellStyle.BackColor = Color.LightCyan
                    i = 1
                End If
            Next
            res.Close()
        Catch ex As Exception
            MsgBox("Erreur d'ouverture du thème: " + ex.Message)
            Try
                res.Close()
            Catch ex2 As Exception

            End Try

        End Try
        'Activate full row selection
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grid.MultiSelect = False
        'Hide the first column and apply column titles
        grid.RowHeadersVisible = False
        grid.ColumnHeadersVisible = False
        'Other Standard options:
        'Stretch the columns to fill the page
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        grid.Columns(0).Width = 50
        'Center the text inside the columns
        For index = 0 To grid.Rows.Count
            Try
                grid.Columns(index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                'Disable edit and sort
                grid.Columns(index).SortMode = DataGridViewColumnSortMode.NotSortable
                grid.Columns(index).ReadOnly = True
            Catch ex As Exception

            End Try
        Next
        'Column headers customization        
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        grid.ColumnHeadersDefaultCellStyle.Font = New Font(grid.Font, FontStyle.Bold)
        grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        grid.ColumnHeadersHeight = 35
        grid.EnableHeadersVisualStyles = False
        'Set the columns in the second grid
        'gdc2.ColumnCount = 2        
        'Add the DoubleClick Handler:
        gdc1 = New DataGridView
        gdc2 = New DataGridView
        gdc1 = grid
        gdc2 = grid2
        AddHandler grid.DoubleClick, AddressOf PDDC

    End Sub
    Private Sub PDDC()
        Try
            For rowIndex As Integer = 0 To (gdc1.Rows.Count - 1)
                If gdc1.Rows(rowIndex).Selected Then
                    If gdc2.ColumnCount = 0 Then
                        For Each Col As DataGridViewColumn In gdc1.Columns
                            gdc2.Columns.Add(DirectCast(Col.Clone, DataGridViewColumn))
                        Next
                    End If
                    gdc2.Rows.Add(gdc1.Rows(rowIndex).Cells.Cast(Of DataGridViewCell).Select(Function(c) c.Value).ToArray)
                End If
            Next
        Catch ex As Exception
            MsgBox("Erreur lors de l'opération: " + ex.Message)
        End Try
        Try
            For Each row As DataGridViewRow In gdc1.SelectedRows
                gdc1.Rows.Remove(row)
            Next
        Catch ex As Exception

        End Try
        'delete the last empty row:
        gdc2.AllowUserToAddRows = False
        'Hide unwanted columns               
        For index = 0 To gdc2.Columns.Count - 1
            If index <> 0 And index <> 1 Then
                gdc2.Columns(index).Visible = False
            End If
        Next
        'Apply the selected theme
        Dim req As String = "Select * From THEMES WHERE Theme = '1'"
        Dim reqc As OleDbCommand = New OleDbCommand(req, CNN)
        Dim res As OleDbDataReader = Nothing
        Try
            res = reqc.ExecuteReader()
            'Dim lignes1color As String = res(1)
            'Dim lignes2color As String = res(2)

            Dim i As Integer = 1
            For Each rw As DataGridViewRow In gdc2.Rows
                If i = 1 Then
                    rw.DefaultCellStyle.BackColor = Color.LightCyan
                    i = 2
                Else
                    rw.DefaultCellStyle.BackColor = Color.LightCyan
                    i = 1
                End If
            Next
            res.Close()
        Catch ex As Exception
            MsgBox("Erreur d'ouverture du thème: " + ex.Message)
            Try
                res.Close()
            Catch ex2 As Exception

            End Try
        End Try
        'Activate full row selection
        gdc2.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        gdc2.MultiSelect = False
        'Hide the first column and apply column titles
        gdc2.RowHeadersVisible = False
        gdc2.ColumnHeadersVisible = False
        'Other Standard options:
        'Stretch the columns to fill the page
        gdc2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        Try
            gdc2.Columns(0).Width = 50
        Catch ex As Exception

        End Try
        'Center the text inside the columns
        For index = 0 To gdc2.Rows.Count
            Try
                gdc2.Columns(index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                'Disable edit and sort
                gdc2.Columns(index).SortMode = DataGridViewColumnSortMode.NotSortable
                gdc2.Columns(index).ReadOnly = True
            Catch ex As Exception

            End Try
        Next
        'Column headers customization        
        gdc2.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy
        gdc2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        gdc2.ColumnHeadersDefaultCellStyle.Font = New Font(gdc2.Font, FontStyle.Bold)
        gdc2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gdc2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        gdc2.ColumnHeadersHeight = 35
        gdc2.EnableHeadersVisualStyles = False
        'Set the columns in the second grid
        'gdc2.ColumnCount = 2        
        'Add the DoubleClick Handler:
    End Sub
    Public Function SqlDelete(table As String, colname As String, colvalue As String)
        Dim result As Boolean = False
        Dim req As String = "DELETE FROM " + table + " WHERE " + colname + "='" + colvalue + "'"
        Dim reqc As OleDbCommand = New OleDbCommand(req, CNN)
        Dim res As OleDbDataReader = Nothing
        Try
            res = reqc.ExecuteReader()
            res.Read()
            res.Close()
            result = True
        Catch ex As Exception
            MsgBox("Erreur de suppression de la table " + table + " : " + ex.Message)
            Try
                res.Close()
            Catch ex1 As Exception

            End Try
            result = False
        End Try
        Return result
    End Function
    Public Function SqlDeleteAll(table As String)
        Dim result As Boolean = False
        Dim req As String = "DELETE FROM " + table
        Dim reqc As OleDbCommand = New OleDbCommand(req, CNN)
        Dim res As OleDbDataReader = Nothing
        Try
            res = reqc.ExecuteReader()
            res.Read()
            res.Close()
            result = True
        Catch ex As Exception
            MsgBox("Erreur de vidage de la table " + table + " : " + ex.Message)
            Try
                res.Close()
            Catch ex1 As Exception

            End Try
            result = False
        End Try
        Return result
    End Function
    Public Function SqlSelect(table As String, Optional col As String = "*", Optional filtername As String = Nothing, Optional filtervalue As String = "")
        Dim results() As Array = Nothing
        Dim req As String = ""
        req = "SELECT isnull(" + col + ",'') FROM " + table
        If filtername IsNot Nothing Then
            req += " WHERE " + filtername + "='" + filtervalue + "'"
        End If
        Dim reqc As OleDbCommand = New OleDbCommand(req, CNN)
        Dim res As OleDbDataReader = Nothing
        Try
            res = reqc.ExecuteReader()
            While res.Read()
                Dim result(res.FieldCount)
                For i = 0 To res.FieldCount - 1
                    result(i) = res(i)
                Next
                If results IsNot Nothing Then
                    ReDim Preserve results(UBound(results) + 1)
                    results(UBound(results)) = result
                Else
                    ReDim results(0)
                    results(0) = result
                End If
            End While
            res.Close()
        Catch ex As Exception
            MsgBox("Erreur de sélection dans la table " + table + " : " + ex.Message)
            Try
                res.Close()
            Catch ex1 As Exception

            End Try
        End Try
        Return results
    End Function
    Public Function SqlInsert(table As String, values() As String)
        Dim resul As Boolean
        Dim req As String = "INSERT INTO " + table + " VALUES("
        For i = 0 To UBound(values) - 1
            req += "'" + values(i) + "'"
            If i = UBound(values) - 1 Then
                req += ")"
            Else
                req += ","
            End If
        Next
        Dim reqc As OleDbCommand = New OleDbCommand(req, CNN)
        Dim res As OleDbDataReader = Nothing
        Try
            res = reqc.ExecuteReader()
            res.Read()
            res.Close()
            resul = True
        Catch ex As Exception
            MsgBox("Erreur de SqlInsert de la table " + table + " : " + ex.Message)
            Try
                res.Close()
            Catch ex1 As Exception

            End Try
            resul = False
        End Try
        Return resul
    End Function
End Class


