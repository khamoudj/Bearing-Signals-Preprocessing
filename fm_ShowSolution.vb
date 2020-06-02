Imports System.Windows.Forms.DataVisualization.Charting
Public Class fm_ShowSolution
    Private Sub fm_ShowSolution_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim Planet() As Array = mainSQL.SqlSelectSimple("SolutionFemto", "*", " code='" & Form1.SelectionResult1 & "' and PlanetSatelitte='P' ")
        Dim Sat() As Array = mainSQL.SqlSelectSimple("SolutionFemto", "*", " code='" & Form1.SelectionResult1 & "' and PlanetSatelitte='S' ")

        Dim Rnd As New Random
        Dim Serie As Integer = 1

        Chart1.Series.Add("Graphe d'évolution")
        Chart1.Series("Graphe d'évolution").ChartType = SeriesChartType.Line
        Chart1.Series("Graphe d'évolution").Color = Color.Blue

        Chart2.Series.Add("Evolution par instance")
        Chart2.Series("Evolution par instance").ChartType = SeriesChartType.Line
        Chart2.Series("Evolution par instance").Color = Color.Red
        Chart2.Series.Add("Evolution par centre de classe")
        Chart2.Series("Evolution par centre de classe").ChartType = SeriesChartType.Line
        Chart2.Series("Evolution par centre de classe").Color = Color.Blue

        Chart3.Series.Add("Evolution par centre de classe")
        Chart3.Series("Evolution par centre de classe").ChartType = SeriesChartType.Line
        Chart3.Series("Evolution par centre de classe").Color = Color.Blue


        For i = 0 To UBound(Planet)
            '   Représentation des classes 
            If Planet(i)(4) <> -1 And Planet(i)(3) > Form1.AffichageInf And Planet(i)(3) < Form1.AffichageSup And Planet(i)(2) > Form1.AffichageGauche And Planet(i)(2) < Form1.AffichageDroite Then

                Chart1.Series("Graphe d'évolution").Points.AddXY(Planet(i)(2), Planet(i)(3))

                Dim clr As Color = Color.FromArgb(255, Rnd.Next(255), Rnd.Next(255), Rnd.Next(255))
                Dim SerieName As String = "Classe " & Serie.ToString & "(" & Planet(i)(6).ToString & ")"
                Chart1.Series.Add(SerieName)
                Chart1.Series(SerieName).ChartType = SeriesChartType.Point
                Chart1.Series(SerieName).Color = clr
                Chart1.Series(SerieName).Points.AddXY(Planet(i)(2), Planet(i)(3))
                For j = 0 To UBound(Sat)
                    If Sat(j)(4) = i And Sat(j)(3) > Form1.AffichageInf And Sat(j)(3) < Form1.AffichageSup And Sat(j)(2) > Form1.AffichageGauche And Sat(j)(2) < Form1.AffichageDroite Then
                        Chart1.Series(SerieName).Points.AddXY(Sat(j)(2), Sat(j)(3))
                    End If
                Next
                Serie = Serie + 1
            End If

            '  Chart 2 (Graphe) 
            If Planet(i)(4) <> -1 Then
                Chart3.Series("Evolution par centre de classe").Points.AddXY(Planet(i)(2), Planet(i)(3))
                Chart2.Series("Evolution par centre de classe").Points.AddXY(Planet(i)(2), Planet(i)(3))
            End If
        Next

        For j = 0 To UBound(Sat)
            Chart2.Series("Evolution par instance").Points.AddXY(Sat(j)(2), Sat(j)(3))
        Next
    End Sub
End Class