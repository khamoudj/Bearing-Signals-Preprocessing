Public Class fm_CodeDesignation
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TextBox1.Text = ""
        TextBox2.Text = ""
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub fm_CodeDesignation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'MdiParent = Form1
        TextBox1.Text = ""
        TextBox2.Text = ""
    End Sub
End Class