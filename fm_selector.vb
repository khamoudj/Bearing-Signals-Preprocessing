Public Class fm_Selector
    Public Title As String
    Public Table, Filter As String
    Public titreCol1, titreCol2, NomCol1, NomCol2 As String
    Public Col1, Col2, Retour As Integer
    Public titreCol3 As String = ""
    Public NomCol3 As String = ""
    Public Col3 As Integer = -1
    Public colsAlign() As String
    Dim titles() As String = Nothing
    Dim cols() As Integer = Nothing
    Dim pdg As New PDGrid

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        BindingSource1.Filter = ""
        DataGridView1.DataSource = BindingSource1.DataSource
        TextBox1.Text = ""
        pdg.ApplyGridTheme(DataGridView1, "1", titles, cols)
        DataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically
        PictureBox3.Visible = False
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub fm_Selector_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Me.MdiParent = Form1
        'Me.MdiParent = Form1
        Dim x, y As Integer
        x = Me.Size.Width
        y = Me.Size.Height
        Me.MaximumSize = New Size(x, y)
        Me.MinimumSize = New Size(x, y)
        Me.MaximizeBox = False
        Dim Screen As System.Drawing.Rectangle
        Screen = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea()
        Me.Top = (Screen.Height \ 2) - (Me.Height \ 2)
        Me.Left = (Screen.Width \ 2) - (Me.Width \ 2)
        If Title = "" Or Table = "" Then
            Form1.SelectionResult1 = ""
            Form1.SelectionResult2 = ""
            Me.Close()
        Else
            pdg.colsAlign = colsAlign
            Form1.SelectionResult1 = ""
            Form1.SelectionResult2 = ""
            Titre.Text = Title
            pdg.LoadTableIntoGrid(Table, DataGridView1, Filter)
            cols = {Col1, Col2, Col2}
            If Col3 <> -1 Then
                cols(2) = Col3
                ReDim Preserve cols(3)
                cols(3) = Col3
            End If
            If titles Is Nothing Then
                ReDim titles(0)
                titles(0) = "NoChange"
            End If
            While UBound(titles) < Col1
                If titles Is Nothing Then
                    ReDim titles(0)
                    titles(0) = "NoChange"
                Else
                    ReDim Preserve titles(UBound(titles) + 1)
                    titles(UBound(titles)) = "NoChange"
                End If
            End While
            titles(Col1) = titreCol1
            While UBound(titles) < Col2
                If titles Is Nothing Then
                    ReDim titles(0)
                    titles(0) = "NoChange"
                Else
                    ReDim Preserve titles(UBound(titles) + 1)
                    titles(UBound(titles)) = "NoChange"
                End If
            End While
            titles(Col2) = titreCol2
            If Col3 <> -1 Then
                While UBound(titles) < Col3
                    If titles Is Nothing Then
                        ReDim titles(0)
                        titles(0) = "NoChange"
                    Else
                        ReDim Preserve titles(UBound(titles) + 1)
                        titles(UBound(titles)) = "NoChange"
                    End If
                End While
                titles(Col3) = titreCol3
            End If
            pdg.ApplyGridTheme(DataGridView1, "1", titles, cols)
            DataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically
        End If
        TextBox1.Select()
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim idx As Integer = 0
            For Each cell In DataGridView1.SelectedCells
                If idx = Col1 Then
                    Form1.SelectionResult1 = cell.Value.ToString().Trim
                    idx += 1
                ElseIf idx = Col2 Then
                    Form1.SelectionResult2 = cell.Value.ToString().Trim
                    idx += 1
                Else
                    idx += 1
                End If
            Next
            Me.Close()
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text = "" Then
            BindingSource1.Filter = ""
            DataGridView1.DataSource = BindingSource1.DataSource
            TextBox1.Text = ""

            pdg.ApplyGridTheme(DataGridView1, "1", titles, cols)
            DataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically
            PictureBox3.Visible = False
            'Timer1.Enabled = True
        Else
            'Timer1.Enabled = False
            PictureBox3.Visible = True
            BindingSource1.DataSource = DataGridView1.DataSource
            'MsgBox(BindingSource1.Current(NomCol2).GetType.ToString)
            Try
                BindingSource1.Filter = "Convert(" & NomCol1 & ", 'System.String') like '%" & TextBox1.Text & "%' OR Convert(" & NomCol2 & ", 'System.String') like '%" & TextBox1.Text & "%'"
            Catch ex As Exception
                Try
                    BindingSource1.Filter = String.Format(NomCol1 & " = '" & TextBox1.Text & "' OR " & NomCol2 & " = '" & TextBox1.Text & "'")
                Catch ex2 As Exception

                End Try
            End Try
            DataGridView1.DataSource = BindingSource1.DataSource
            pdg.ApplyGridTheme(DataGridView1, "1", titles, cols)
            DataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically
        End If
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Form1.SelectionResult1 = ""
        Form1.SelectionResult2 = ""
        Me.Close()
    End Sub

    Private Sub DataGridView1_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentDoubleClick

    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        'Dim idx As Integer = 0
        'For Each cell In DataGridView1.SelectedCells
        '    If idx = Retour Then
        '        Form1.SelectionResult = cell.Value.ToString().Trim
        '        Exit For
        '    Else
        '        idx += 1
        '    End If
        'Next
        Dim idx As Integer = 0
        For Each cell In DataGridView1.SelectedCells
            If idx = Col1 Then
                Form1.SelectionResult1 = cell.Value.ToString().Trim
                idx += 1
            ElseIf idx = Col2 Then
                Form1.SelectionResult2 = cell.Value.ToString().Trim
                idx += 1
            Else
                idx += 1
            End If
        Next
        Me.Close()
    End Sub


End Class