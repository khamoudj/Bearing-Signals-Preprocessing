Public Class fm_Parametrage
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form1.RDis = TextBox1.Text
        Form1.NbrMinElements = NumericUpDown1.Value
        Form1.AffichageInf = TextBox2.Text
        Form1.AffichageSup = TextBox3.Text
        Form1.AffichageGauche = TextBox4.Text
        Form1.AffichageDroite = TextBox5.Text
        Form1.TypePointIsoles = ComboBox1.SelectedIndex
        If RadioButton1.Checked Then
            Form1.TypeDistance = 0
        Else
            Form1.TypeDistance = 1
        End If
        Close()
    End Sub

    Private Sub fm_Parametrage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = Form1.RDis
        NumericUpDown1.Value = Form1.NbrMinElements
        TextBox2.Text = Form1.AffichageInf
        TextBox3.Text = Form1.AffichageSup
        TextBox4.Text = Form1.AffichageGauche
        TextBox5.Text = Form1.AffichageDroite
        ComboBox1.SelectedIndex = Form1.TypePointIsoles
        If Form1.TypeDistance = 0 Then
            RadioButton1.Checked = True
        Else
            RadioButton2.Checked = True
        End If

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        RadioButton1.Checked = True
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        RadioButton2.Checked = True
    End Sub
End Class