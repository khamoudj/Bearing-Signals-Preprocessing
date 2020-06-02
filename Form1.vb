Imports System.Data.OleDb

Public Class Form1
    Public RapportDistance As Double = 1
    Public RDis As Double = 1
    Public TypeDistance As Integer = 0
    Public NbrMinElements As Double = 5
    Public AffichageSup As Double = 20
    Public AffichageInf As Double = 0
    Public AffichageGauche As Double = 0
    Public AffichageDroite As Double = 50000
    Public TypePointIsoles As Integer = 0
    Public SelectionResult1 As String = ""
    Public SelectionResult2 As String = ""
    Private SQl As SQLFunctions
    Private path As String = ""
    Private FileName As String = ""
    Private Folder As String = ""
    Private sr As IO.StreamReader
    Private annee, mois, jr, hr, mn, sec As Integer

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            StartConnection()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        ComboBox1.SelectedIndex = 0

        Button1_Click(sender, e)
    End Sub

    Private Sub FolderBrowserDialog1_HelpRequest(sender As Object, e As EventArgs) Handles FolderBrowserDialog1.HelpRequest

    End Sub

    Private Sub TextBox1_TextChanged_1(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text = "" Then
            BS.Filter = ""
            DataGridView1.DataSource = BS.DataSource
            TextBox1.Text = ""
            PictureBox3.Visible = False

        Else
            PictureBox3.Visible = True
            BS.DataSource = DataGridView1.DataSource
            If ComboBox1.Text = "Tous" Or ComboBox1.Text = "Options de recherche" Then
                BS.Filter = String.Format("DataSet like '%" & TextBox1.Text & "%' OR FileName = '" & TextBox1.Text & "'")
                DataGridView1.DataSource = BS.DataSource
            Else
                If ComboBox1.Text = "DataSet" Or ComboBox1.Text = "FileName" Then
                    BS.Filter = String.Format(ComboBox1.Text & " like '%" & TextBox1.Text & "%'")
                    DataGridView1.DataSource = BS.DataSource
                Else
                    BS.Filter = String.Format(ComboBox1.Text & " = '" & TextBox1.Text & "'")
                    DataGridView1.DataSource = BS.DataSource
                End If
            End If
        End If
        Label3.Text = BS.Count
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        BS.Filter = ""
        DataGridView1.DataSource = BS.DataSource
        TextBox1.Text = ""

        PictureBox3.Visible = False
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs)
        'Dim enregistrement As String = ""
        'Dim id As Integer = "-1"
        'For Each row As DataGridViewRow In DataGridView1.SelectedRows
        '    Try
        '        enregistrement = row.Cells(3).Value.ToString
        '        id = row.Cells(0).Value.ToString
        '    Catch ex As Exception
        '        MsgBox("Vous devez sélectionner une mesure.")
        '    End Try
        'Next
        'If enregistrement <> "" Then
        '    Dim result As Integer = MessageBox.Show("Etes-vous sûr de vouloir supprimer : " + enregistrement, "Supprimer une mesure", MessageBoxButtons.YesNo)
        '    If result = DialogResult.Yes Then
        '        For Each row As DataGridViewRow In DataGridView1.SelectedRows
        '            DataGridView1.Rows.Remove(row)
        '        Next
        '        MsgBox(enregistrement + " Supprimée avec succès.")
        '        'pd.log("Suppression - indémnité: " + enregistrement, "C_UNITESDEGESTION", id)
        '    End If
        'Else
        '    MsgBox("Vous devez sélectionner une mesure")
        'End If
        mainSQL.OneResRequest("delete from MesureFEMTO where " & BS.Filter)
        Dim SQL As New SQLFunctions
        Dim DS As New DataSet
        'Dim req As String = "select DataSet, FileName ,SamplingDate, SamplingTime, Channel1, Channel2, Channel3, Channel4, Channel5, Channel6, Channel7, Channel8, Charge from mesures order by id"
        Dim req As String = "select * from MesureFEMTO"
        Dim TableAdapter As OleDbDataAdapter = New OleDbDataAdapter(req, CNN)
        Try
            TableAdapter.ContinueUpdateOnError = True
            TableAdapter.Fill(DS, "CONFIG")
            BS.DataSource = DS.Tables(0)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        DataGridView1.DataSource = BS.DataSource

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        Dim mObj As Object
        Dim res As String
        mObj = CreateObject("Matlab.Application")  'create matlab object
        res = mObj.Execute("cd C:\Users\bcrich\Desktop\dll matlab")      'change directory
        res = mObj.Execute("payment.m")            'execute your function


    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        fm_CMOClustering.ShowDialog()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim Selector = New fm_Selector
        Selector.Title = "Liste des solutions"
        Selector.titreCol1 = "Code"
        Selector.titreCol2 = "Désignation"
        Selector.Table = "SolutionFemtoCode"
        Selector.NomCol1 = "Code"
        Selector.NomCol2 = "Designation"
        Selector.Retour = 0
        Selector.Col1 = 0
        Selector.Col2 = 1
        Selector.colsAlign = {"", "", "", "center", "left"}
        Selector.Filter = ""
        Selector.ShowDialog()
        If SelectionResult1 <> "" Then
            fm_ShowSolution.Show()
        End If
        'TextBox3.Text = fm_Principale.SelectionResult2
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        fm_Parametrage.ShowDialog()
    End Sub

    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button4.Click
        Dim Selector = New fm_Selector
        Selector.Title = "Liste des solutions"
        Selector.titreCol1 = "Code"
        Selector.titreCol2 = "Désignation"
        Selector.Table = "SolutionFemtoCode"
        Selector.NomCol1 = "Code"
        Selector.NomCol2 = "Designation"
        Selector.Retour = 0
        Selector.Col1 = 0
        Selector.Col2 = 1
        Selector.colsAlign = {"", "", "", "center", "left"}
        Selector.Filter = ""
        Selector.ShowDialog()
        If SelectionResult1 <> "" Then
            mainSQL.OneResRequest("delete from SolutionFemtoCode where code='" & SelectionResult1 & "'")
            mainSQL.OneResRequest("delete from SolutionFemto where code='" & SelectionResult1 & "'")
            MsgBox("La solution(" & SelectionResult1 & ", " & SelectionResult2 & ") a été supprimée avec succés")
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim SQL As New SQLFunctions
        Dim DS As New DataSet
        'Dim req As String = "select DataSet, FileName ,SamplingDate, SamplingTime, Channel1, Channel2, Channel3, Channel4, Channel5, Channel6, Channel7, Channel8, Charge from mesures order by id"
        Dim req As String = "select * from MesureFEMTO"
        Dim TableAdapter As OleDbDataAdapter = New OleDbDataAdapter(req, CNN)
        Try
            TableAdapter.ContinueUpdateOnError = True
            TableAdapter.Fill(DS, "CONFIG")
            BS.DataSource = DS.Tables(0)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        DataGridView1.DataSource = BS.DataSource
        Label3.Text = BS.Count
        'SQL.SqlInsert("Mesures", {"4", "1", "01/01/2015", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"})

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim cc As Integer = 0
        Dim SamplingTime As Double = 0
        Dim SamplingTime1 As Double = 0
        Dim test999 As Integer = 1
        Dim req As String = ""
        For Each FN As String In OpenFileDialog1.FileNames
            Dim tab() As String = Split(FN, "\")
            FileName = tab(UBound(tab))
            If FileName.Contains("acc") Then
                sr = New IO.StreamReader(FN)
                Dim ary() As String = Nothing
                Dim p As Integer = 0
                Dim AtTime As String = ""
                Dim m1 As Double = 0
                Dim m2 As Double = 0
                Dim m3 As Double = 0
                Dim v1 As Double = 0
                Dim V2 As Double = 0
                Dim V3 As Double = 0

                While sr.Read <> -1
                    If ary IsNot Nothing Then
                        ReDim Preserve ary(UBound(ary) + 1)
                        ary(UBound(ary)) = sr.ReadLine
                        Dim attr() As String = Split(ary(p), ",")
                        If UBound(attr) = 0 Then
                            attr = Split(ary(0), ";")
                        End If
                        If attr IsNot Nothing Then
                            m1 = m1 + attr(4)
                            m2 = m2 + attr(5)
                            m3 = m3 + Math.Sqrt((attr(4) ^ 2) + (attr(5) ^ 2))
                        End If
                    Else
                        ReDim ary(0)
                        ary(0) = sr.ReadLine
                        Dim attr() As String = Split(ary(0), ",")
                        If UBound(attr) = 0 Then
                            attr = Split(ary(0), ";")
                        End If
                        If attr IsNot Nothing Then
                            m1 = m1 + attr(4)
                            m2 = m2 + attr(5)
                            m3 = m3 + Math.Sqrt((attr(4) ^ 2) + (attr(5) ^ 2))
                            AtTime = attr(0).ToString & ":" & attr(1).ToString & ":" & attr(2).ToString & ":" & attr(3).ToString
                            SamplingTime = SamplingTime + 10
                        End If

                    End If
                    p = p + 1
                End While

                m1 = arrondissement(m1 / (UBound(ary) + 1), 3)
                m2 = arrondissement(m2 / (UBound(ary) + 1), 3)
                m3 = arrondissement(m3 / (UBound(ary) + 1), 3)

                For j = 0 To UBound(ary)
                    Dim attr() As String = Split(ary(j), ",")
                    If UBound(attr) = 0 Then
                        attr = Split(ary(0), ";")
                    End If
                    v1 = v1 + ((attr(4) - m1) ^ 2)
                    V2 = V2 + ((attr(5) - m2) ^ 2)
                    V3 = V3 + (((Math.Sqrt((attr(4) ^ 2) + (attr(5) ^ 2))) - m3) ^ 2)
                Next
                v1 = arrondissement(v1 / (UBound(ary) + 1), 3)
                V2 = arrondissement(V2 / (UBound(ary) + 1), 3)
                V3 = arrondissement(V3 / (UBound(ary) + 1), 3)

                If test999 = 1 Then
                    If req <> "" Then
                        SQl = New SQLFunctions
                        SQl.OneResRequest(req)
                    End If
                    req = "Insert into MesureFEMTO values ('" & Folder & "','" & FileName & "', '" & AtTime & "','" & SamplingTime & "','" & m1 & "','" & m2 & "','" & m3 & "','" & v1 & "','" & V2 & "','" & V3 & "','-1','-1') "
                Else
                    req = req & ", ('" & Folder & "','" & FileName & "', '" & AtTime & "','" & SamplingTime & "','" & m1 & "','" & m2 & "','" & m3 & "','" & v1 & "','" & V2 & "','" & V3 & "','-1', '-1') "
                End If
                If test999 = 999 Then
                    test999 = 1
                Else
                    test999 += 1
                End If
                cc += 1
            Else
                Dim STok As Boolean = False
                Dim CptST As Integer = 1
                Dim JST As Integer = 0
                sr = New IO.StreamReader(FN)
                Dim ary() As String = Nothing
                Dim p As Integer = 0
                Dim m1 As Double = 0
                Dim v1 As Double = 0
                While sr.Read <> -1
                    If ary IsNot Nothing Then
                        ReDim Preserve ary(UBound(ary) + 1)
                        ary(UBound(ary)) = sr.ReadLine
                        Dim attr() As String = Split(ary(p), ",")
                        If UBound(attr) = 0 Then
                            attr = Split(ary(0), ";")
                        End If
                        If attr IsNot Nothing Then
                            m1 = m1 + attr(4)
                        End If
                    Else
                        ReDim ary(0)
                        ary(0) = sr.ReadLine
                        Dim attr() As String = Split(ary(0), ",")
                        If UBound(attr) = 0 Then
                            attr = Split(ary(0), ";")
                        End If
                        If attr IsNot Nothing Then
                            m1 = m1 + attr(4)
                        End If
                        JST = 0
                    End If
                    p = p + 1
                    CptST = CptST + 1
                    If CptST = 100 Then
                        CptST = 1
                        SamplingTime1 = SamplingTime1 + 10
                        m1 = arrondissement(m1 / 100, 3)
                        v1 = 0
                        For j = JST * 100 To UBound(ary) 'JST * 100 + 99
                            Dim attr() As String = Split(ary(j), ",")
                            If UBound(attr) = 0 Then
                                attr = Split(ary(0), ";")
                            End If
                            v1 = v1 + ((attr(4) - m1) ^ 2)
                        Next
                        JST = JST + 1
                        v1 = arrondissement(v1 / 100, 3)
                        mainSQL.OneResRequest("update MesureFEMTO set TemperatureMoy='" & m1 & "', TemperatureVar='" & v1 & "' where dataset='" & Folder & "' and SamplingTime='" & SamplingTime1 & "'")
                        m1 = 0
                    End If
                End While

            End If

        Next

        SQl = New SQLFunctions
        SQl.OneResRequest(req)
        mainSQL.OneResRequest("update MesureFEMTO set TemperatureMoy=(select max(TemperatureMoy) from MesureFEMTO as Msr where MesureFEMTO.DataSet=Msr.dataset) where TemperatureMoy='-1' or TemperatureMoy is null")
        MsgBox(cc & " fichiers traités")

    End Sub


    Private Sub Button3_Click(sender As Object, e As EventArgs)

    End Sub


    Private Sub TextBox1_Click(sender As Object, e As EventArgs) Handles TextBox4.Click
        OpenFileDialog1.Multiselect = True
        If OpenFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            TextBox4.Text = OpenFileDialog1.FileName.ToString
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        Dim tab() As String = Split(TextBox4.Text, "\")
        Folder = tab(UBound(tab) - 1)
        path = ""
        For i = 0 To UBound(tab) - 1
            path = path & tab(i) & "\"
        Next
    End Sub
End Class
