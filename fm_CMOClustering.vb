Imports System.Math
Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting
Public Class fm_CMOClustering
    Private RapportDistance As Double = Form1.RapportDistance
    Private NbrMinElements As Double = Form1.NbrMinElements
    Private TypePointIsoles As Double = Form1.TypePointIsoles
    Private RDis As Double = Form1.RDis
    Private TypeDistance As Integer = Form1.TypeDistance

    Private AffichageSup As Double = Form1.AffichageSup
    Private AffichageInf As Double = Form1.AffichageInf
    Private tabChargement() As Array = Nothing
    Private tab() As Array = Nothing
    Private Planet() As Array = Nothing
    Private Sat() As Array = Nothing
    Private Classes() As Array = Nothing
    Private Objets() As Array = Nothing
    Private obj_DrawLine = CreateGraphics()

    Private Sub fm_CMOClustering_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'MdiParent = Form1
        tabChargement = Nothing
        Dim BS As BindingSource = Form1.BS
        BS.MoveFirst()
        For i = 1 To BS.Count
            Dim reg As Array = {BS.Current("Id"), BS.Current("SamplingTime"), BS.Current("VibHorMoy"), BS.Current("VibVerMoy"), BS.Current("VibMoy"), BS.Current("VibHorVar"), BS.Current("VibVerVar"), BS.Current("VibVar"), BS.Current("TemperatureMoy"), BS.Current("TemperatureVar"), "0"}
            If tabChargement IsNot Nothing Then
                ReDim Preserve tabChargement(UBound(tabChargement) + 1)
                tabChargement(UBound(tabChargement)) = reg
            Else
                ReDim tabChargement(0)
                tabChargement(0) = reg
            End If
            BS.MoveNext()
        Next
        BS.MoveFirst()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If tabChargement IsNot Nothing Then

            Label1.Text = "CMO Clustering for : " & Form1.TextBox1.Text
            tab = tabChargement.Clone

            Dim NbrPlnt = 0
            Dim NbrSat = 0
            Dim BornInf As Double = 100
            Dim BornSup As Double = 0
            Dim MasseSysSolaire As Double = 2987831 ' * (10 ^ 21)
            Dim MasseUnit As Double = MasseSysSolaire
            Dim DistanceMaxSS As Double = 4442860

            Dim CptSatPlnt As Integer = 11
            For i = 0 To UBound(tab)
                If CptSatPlnt = 22 Then
                    If Planet Is Nothing Then
                        ReDim Planet(0)
                        Planet(0) = {tab(i)(0), tab(i)(1), tab(i)(4), tab(i)(7), CDbl(1), CDbl(0)}
                    Else
                        ReDim Preserve Planet(UBound(Planet) + 1)
                        Planet(UBound(Planet)) = {tab(i)(0), tab(i)(1), tab(i)(4), tab(i)(7), CDbl(1), CDbl(0)}
                    End If
                    CptSatPlnt = 1
                    NbrPlnt = NbrPlnt + 1
                Else
                    If Sat Is Nothing Then
                        ReDim Sat(0)
                        Sat(0) = {tab(i)(0), tab(i)(1), tab(i)(4), tab(i)(7), -1, 0}
                    Else
                        ReDim Preserve Sat(UBound(Sat) + 1)
                        Sat(UBound(Sat)) = {tab(i)(0), tab(i)(1), tab(i)(4), tab(i)(7), -1, 0}
                    End If
                    CptSatPlnt = CptSatPlnt + 1
                    NbrSat = NbrSat + 1
                End If

                If tab(i)(7) > BornSup Then
                    BornSup = tab(i)(7)
                End If
                If tab(i)(7) < BornInf Then
                    BornInf = tab(i)(7)
                End If
            Next

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Equilibre Terre-Lune ''''''''''''''''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim Equilibre As Boolean = False
            While Not Equilibre
                Equilibre = True
                For i = 0 To UBound(Sat)
                    Dim gain As Double = 0
                    Dim ind As Integer = 0
                    For j = 0 To UBound(Planet)
                        Dim NewGain As Double ' = Planet(j)(4) / (Abs(Planet(j)(1) - Sat(i)(1)) * Abs(Planet(j)(3) - Sat(i)(3)))
                        If TypeDistance = 0 Then
                            NewGain = Planet(j)(4) / Sqrt((Planet(j)(1) - Sat(i)(1)) ^ 2 + Abs(Planet(j)(3) - Sat(i)(3)) ^ 2)
                        Else
                            NewGain = Planet(j)(4) / (Abs(Planet(j)(1) - Sat(i)(1)) + Abs(Planet(j)(3) - Sat(i)(3)))
                        End If
                        If NewGain > gain Then
                            gain = NewGain
                            ind = j
                        End If
                    Next
                    If Sat(i)(4) <> ind Then
                        If Sat(i)(4) <> -1 Then
                            Planet(ind)(3) = (Planet(ind)(3) * Planet(ind)(4) + Sat(i)(3)) / (Planet(ind)(4) + 1)
                            'Planet(ind)(1) = (Planet(ind)(1) * Planet(ind)(4) + Sat(i)(1)) / (Planet(ind)(4) + 1)
                            Planet(ind)(4) = Planet(ind)(4) + 1

                            Planet(Sat(i)(4))(3) = (Planet(Sat(i)(4))(4) * Planet(Sat(i)(4))(3) - Sat(i)(3)) / (Planet(Sat(i)(4))(4))
                            'Planet(Sat(i)(4))(1) = (Planet(Sat(i)(4))(4) * Planet(Sat(i)(4))(1) - Sat(i)(1)) / (Planet(Sat(i)(4))(4))
                            Planet(Sat(i)(4))(4) = Planet(Sat(i)(4))(4) - 1
                        Else
                            Planet(ind)(3) = (Planet(ind)(3) * Planet(ind)(4) + Sat(i)(3)) / (Planet(ind)(4) + 1)
                            'Planet(ind)(1) = (Planet(ind)(1) * Planet(ind)(4) + Sat(i)(1)) / (Planet(ind)(4) + 1)
                            Planet(ind)(4) = Planet(ind)(4) + 1
                        End If

                        Sat(i)(4) = ind
                        Sat(i)(5) = gain
                        Equilibre = False
                    End If
                Next
            End While


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Equilibre Terre-Pomme '''''''''''''''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            MasseUnit = MasseSysSolaire / (NbrPlnt + NbrSat)
            RapportDistance = DistanceMaxSS / (BornSup - BornInf)
            For i = 1 To UBound(Planet)
                Planet(i)(4) = Planet(i)(4)
                Planet(i)(5) = 0.00534 * Planet(i)(4) * MasseUnit ' * (10 ^ -21)
            Next

            Equilibre = False
            While Not Equilibre
                Equilibre = True
                For i = 0 To UBound(Planet)
                    For j = 0 To UBound(Planet)
                        If i <> j And Planet(i)(0) <> -1 And Planet(j)(0) <> -1 Then
                            Dim DstIJ As Double
                            If typedistance = 0 Then
                                DstIJ = Sqrt((Planet(i)(1) - Planet(j)(1)) ^ 2 + (Planet(i)(3) - Planet(j)(3)) ^ 2) * RDis
                            Else
                                DstIJ = (Abs(Planet(i)(1) - Planet(j)(1)) + Abs(Planet(i)(3) - Planet(j)(3))) * RDis
                            End If
                            If Planet(i)(5) > DstIJ Then
                                Equilibre = False
                                Planet(j)(0) = -1
                                'Planet(i)(1) = ((Planet(i)(4) * Planet(i)(1)) + (Planet(j)(4) * Planet(j)(1))) / (Planet(i)(4) + Planet(j)(4))
                                Planet(i)(3) = ((Planet(i)(4) * Planet(i)(3)) + (Planet(j)(4) * Planet(j)(3))) / (Planet(i)(4) + Planet(j)(4))
                                Planet(i)(4) = Planet(i)(4) + Planet(j)(4)
                                Planet(i)(5) = Planet(i)(5) + Planet(j)(5)
                                Planet(j)(4) = 0
                                Planet(j)(5) = 0
                                For k = 0 To UBound(Sat)
                                    If Sat(k)(4) = j Then
                                        Sat(k)(4) = i
                                        Sat(k)(5) = (Abs(Planet(i)(1) - Sat(k)(1)) / (Abs(Planet(i)(3) - Sat(k)(3))) ^ 2) * Sqrt(Planet(i)(4))
                                    End If
                                Next

                            End If
                        End If
                    Next
                Next
            End While

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Traitement nombre minimal d'élements par classe '''''''''''''''''
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If TypePointIsoles = 1 Then
                TypePointIsoles = 3
            End If
            For i = 0 To UBound(Planet)
                If Planet(i)(0) <> -1 And Planet(i)(4) < NbrMinElements Then
                    Dim gain As Double = 0
                    Dim ind As Integer = -1
                    For j = 0 To UBound(Planet)

                        If Planet(j)(0) <> -1 And Planet(j)(4) >= NbrMinElements Then
                            Dim NewGain As Double
                            If TypePointIsoles = 2 Then
                                If TypeDistance = 0 Then
                                    NewGain = Sqrt((Planet(j)(0) - Planet(i)(0)) ^ 2 + (Planet(j)(3) - Planet(i)(3)) ^ 2)
                                Else
                                    NewGain = Abs(Planet(j)(0) - Planet(i)(0)) + Abs(Planet(j)(3) - Planet(i)(3))
                                End If
                            Else
                                NewGain = Abs(Planet(j)(TypePointIsoles) - Planet(i)(TypePointIsoles))
                            End If
                            If NewGain < gain Or gain = 0 Then
                                gain = NewGain
                                ind = j
                            End If
                        End If
                    Next
                    If ind <> -1 Then
                        Planet(i)(0) = -1
                        'Planet(ind)(1) = ((Planet(ind)(4) * Planet(ind)(1)) + (Planet(i)(4) * Planet(i)(1))) / (Planet(ind)(4) + Planet(i)(4))
                        Planet(ind)(3) = ((Planet(ind)(4) * Planet(ind)(3)) + (Planet(i)(4) * Planet(i)(3))) / (Planet(ind)(4) + Planet(i)(4))
                        Planet(ind)(4) = Planet(ind)(4) + Planet(i)(4)
                        Planet(ind)(5) = Planet(ind)(5) + Planet(i)(5)
                        Planet(i)(4) = 0
                        Planet(i)(5) = 0
                        For k = 0 To UBound(Sat)
                            If Sat(k)(4) = i Then
                                Sat(k)(4) = ind
                                Sat(k)(5) = (Abs(Planet(ind)(1) - Sat(k)(1)) / (Abs(Planet(ind)(3) - Sat(k)(3))) ^ 2) * Sqrt(Planet(ind)(4))
                            End If
                        Next
                    End If

                End If
            Next

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Generation fichier ''''''''''''''''''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim Fichier As String = ""
            For i = 0 To UBound(Sat)
                For j = 0 To 5
                    If j = 5 Then
                        Fichier = Fichier & Sat(i)(j) & vbNewLine
                    Else
                        Fichier = Fichier & Sat(i)(j) & ","
                    End If

                Next
            Next
            File.WriteAllText("D:\DOCTORAT\2ieme annee\Tests et resultats\CMO\FichiersCSV\" & Form1.TextBox1.Text & ".csv", Fichier)

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Affichage '''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

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
                If Planet(i)(0) <> -1 Then

                    Chart1.Series("Graphe d'évolution").Points.AddXY(Planet(i)(1), Planet(i)(3))
                    Dim clr As Color = Color.FromArgb(255, Rnd.Next(255), Rnd.Next(255), Rnd.Next(255))
                    Dim SerieName As String = "Classe " & Serie.ToString & "(" & Planet(i)(4).ToString & ")"
                    Chart1.Series.Add(SerieName)
                    Chart1.Series(SerieName).ChartType = SeriesChartType.Point
                    Chart1.Series(SerieName).Color = clr
                    Chart1.Series(SerieName).Points.AddXY(Planet(i)(1), Planet(i)(3))
                    For j = 0 To UBound(Sat)
                        If Sat(j)(4) = i And Sat(j)(3) < (BornSup / 1) Then
                            Chart1.Series(SerieName).Points.AddXY(Sat(j)(1), Sat(j)(3))

                            'Dim myGraphics As Graphics = Me.CreateGraphics
                            'Dim myPen As Pen
                            'myPen = New Pen(Brushes.DarkMagenta, 20)
                            'myGraphics.DrawLine(myPen, CInt(Planet(i)(1)), CInt(Planet(i)(3)), CInt(Sat(j)(1)), CInt(Sat(j)(3)))
                        End If
                    Next
                    Serie = Serie + 1
                End If

                '  Chart 2 (Graphe) 
                If Planet(i)(0) <> -1 Then
                    Chart3.Series("Evolution par centre de classe").Points.AddXY(Planet(i)(1), Planet(i)(3))
                    Chart2.Series("Evolution par centre de classe").Points.AddXY(Planet(i)(1), Planet(i)(3))
                End If
            Next

            For j = 0 To UBound(Sat)
                Chart2.Series("Evolution par instance").Points.AddXY(Sat(j)(1), Sat(j)(3))
            Next
        Else
            MsgBox("Les mesures ne sont pas chargées, veuillez vérifier l'affichage ou le filtre de données dans la fenetre precedente.")
        End If

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Me.Dispose()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        fm_CodeDesignation.ShowDialog()
        If fm_CodeDesignation.TextBox1.Text <> "" Then
            Dim CodeOk() As Array = mainSQL.SqlSelectSimple("SolutionFemtoCode", "*", " code='" & fm_CodeDesignation.TextBox1.Text & "'")
            If CodeOk IsNot Nothing Then
                MsgBox("La solution " & fm_CodeDesignation.TextBox1.Text & " existe déja dans les solutions enregistrées.")
            Else
                Dim reqCode As String = "insert into SolutionFemtoCode values('" & fm_CodeDesignation.TextBox1.Text & "', '" & fm_CodeDesignation.TextBox2.Text & "')"
                mainSQL.OneResRequest(reqCode)

                For i = 0 To UBound(Sat)
                    Dim req As String = "insert into SolutionFemto values('" & fm_CodeDesignation.TextBox1.Text & "', '" & fm_CodeDesignation.TextBox2.Text & "', '" & Sat(i)(1) & "', '" & Sat(i)(3) & "', '" & Sat(i)(4) & "', 'S','1')"
                    mainSQL.OneResRequest(req)
                Next
                For i = 0 To UBound(Planet)
                    If Planet(i)(0) = -1 Then
                        Dim req As String = "insert into SolutionFemto values('" & fm_CodeDesignation.TextBox1.Text & "', '" & fm_CodeDesignation.TextBox2.Text & "', '" & Planet(i)(1) & "', '" & Planet(i)(3) & "', '-1', 'P','0')"
                        mainSQL.OneResRequest(req)
                    Else
                        Dim req As String = "insert into SolutionFemto values('" & fm_CodeDesignation.TextBox1.Text & "', '" & fm_CodeDesignation.TextBox2.Text & "', '" & Planet(i)(1) & "', '" & Planet(i)(3) & "', '" & i & "', 'P','" & Planet(i)(4) & "')"
                        mainSQL.OneResRequest(req)
                    End If
                Next
                MsgBox("La solution " & fm_CodeDesignation.TextBox1.Text & " a été bien enregistrée.")
            End If
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If tabChargement IsNot Nothing Then
            'For i = 0 To UBound(tabChargement)
            '    Dim max As Double = -1
            '    Dim ind As Integer = 0
            '    For j = 0 To UBound(tabChargement)
            '        If tabChargement(j)(7) > max Then
            '            max = tabChargement(j)(7)
            '            ind = j
            '        End If
            '    Next
            '    Dim reg As Array = tabChargement(ind)
            '    If tab IsNot Nothing Then
            '        ReDim Preserve tab(UBound(tab) + 1)
            '        tab(UBound(tab)) = reg.Clone
            '    Else
            '        ReDim tab(0)
            '        tab(0) = reg.Clone
            '    End If
            '    tabChargement(ind)(7) = -1
            'Next
            Label1.Text = "CMO Clustering for : " & Form1.TextBox1.Text
            tab = tabChargement.Clone



            Dim NbrPlnt = 0
            Dim NbrSat = 0
            Dim BornInf As Double = 100
            Dim BornSup As Double = 0
            Dim MasseSysSolaire As Double = 2987831 ' * (10 ^ 21)
            Dim MasseUnit As Double = MasseSysSolaire
            Dim DistanceMaxSS As Double = 4442860
            Dim RapportDistance As Double = 1
            Dim CptSatPlnt As Integer = 11
            For i = 0 To UBound(tab)
                If CptSatPlnt = 22 Then
                    If Planet Is Nothing Then
                        ReDim Planet(0)
                        Planet(0) = {tab(i)(0), tab(i)(1), tab(i)(4), tab(i)(7), CDbl(1), CDbl(0)}
                    Else
                        ReDim Preserve Planet(UBound(Planet) + 1)
                        Planet(UBound(Planet)) = {tab(i)(0), tab(i)(1), tab(i)(4), tab(i)(7), CDbl(1), CDbl(0)}
                    End If
                    CptSatPlnt = 1
                    NbrPlnt = NbrPlnt + 1
                Else
                    If Sat Is Nothing Then
                        ReDim Sat(0)
                        Sat(0) = {tab(i)(0), tab(i)(1), tab(i)(4), tab(i)(7), -1, 0}
                    Else
                        ReDim Preserve Sat(UBound(Sat) + 1)
                        Sat(UBound(Sat)) = {tab(i)(0), tab(i)(1), tab(i)(4), tab(i)(7), -1, 0}
                    End If
                    CptSatPlnt = CptSatPlnt + 1
                    NbrSat = NbrSat + 1
                End If

                If tab(i)(7) > BornSup Then
                    BornSup = tab(i)(7)
                End If
                If tab(i)(7) < BornInf Then
                    BornInf = tab(i)(7)
                End If
            Next

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim Equilibre As Boolean = False
            While Not Equilibre
                Equilibre = True
                For i = 0 To UBound(Sat)
                    Dim gain As Double = 0
                    Dim ind As Integer = 0
                    For j = 0 To UBound(Planet)
                        Dim NewGain As Double = (Abs(Planet(j)(1) - Sat(i)(1)) / (Abs(Planet(j)(3) - Sat(i)(3))) ^ 2) * Sqrt(Planet(j)(4))
                        If NewGain > gain Then
                            gain = NewGain
                            ind = j
                        End If
                    Next
                    If Sat(i)(4) <> ind Then
                        If Sat(i)(4) <> -1 Then
                            Planet(ind)(4) = Planet(ind)(4) + 1
                            Planet(Sat(i)(4))(4) = Planet(Sat(i)(4))(4) + 1
                        Else
                            Planet(ind)(4) = Planet(ind)(4) + 1
                        End If

                        Sat(i)(4) = ind
                        Sat(i)(5) = gain

                        Equilibre = False
                    End If
                Next
            End While


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            MasseUnit = MasseSysSolaire / (NbrPlnt + NbrSat)
            RapportDistance = DistanceMaxSS / (BornSup - BornInf)
            For i = 1 To UBound(Planet)
                Planet(i)(4) = Planet(i)(4) * MasseUnit
                Planet(i)(5) = 0.00534 * Planet(i)(4) ' * (10 ^ -21)
            Next

            Equilibre = False
            While Not Equilibre
                Equilibre = True
                For i = 0 To UBound(Planet)
                    For j = 0 To UBound(Planet)
                        If i <> j And Planet(i)(0) <> -1 And Planet(j)(0) <> -1 Then
                            Dim DstIJ As Double = Sqrt(Abs(Planet(i)(4) ^ 2 - Planet(j)(4) ^ 2))
                            If Planet(i)(5) > DstIJ Then
                                Equilibre = False
                                Planet(j)(0) = -1
                                Planet(i)(4) = Planet(i)(4) + Planet(j)(4)
                                Planet(i)(5) = Planet(i)(5) + Planet(j)(5)
                                Planet(j)(4) = 0
                                Planet(j)(5) = 0
                                For k = 0 To UBound(Sat)
                                    If Sat(k)(4) = j Then
                                        Sat(k)(4) = i
                                        Sat(k)(5) = (Abs(Planet(i)(1) - Sat(k)(1)) / (Abs(Planet(i)(3) - Sat(k)(3))) ^ 2) * Sqrt(Planet(i)(4))
                                    End If
                                Next

                            End If
                        End If
                    Next
                Next
            End While
            'Planet = Planet
            'Sat = Sat
            'Dim plt(126) As Double
            'For i = 0 To UBound(plt)
            '    plt(i) = Planet(i)(4)
            'Next
            'Dim plt1(126) As Double
            'For i = 0 To UBound(plt)
            '    plt1(i) = Planet(i)(5)
            'Next
            'plt = plt
            'plt1 = plt1

            Dim Fichier As String = ""
            For i = 0 To UBound(Sat)
                For j = 0 To 5
                    If j = 5 Then
                        Fichier = Fichier & Sat(i)(j) & vbNewLine
                    Else
                        Fichier = Fichier & Sat(i)(j) & ","
                    End If

                Next
            Next
            File.WriteAllText("D:\DOCTORAT\2ieme annee\Tests et resultats\CMO\FichiersCSV\" & Form1.TextBox1.Text & ".csv", Fichier)

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
                If Planet(i)(0) <> -1 And Planet(i)(3) < (BornSup / 50) Then

                    Chart1.Series("Graphe d'évolution").Points.AddXY(Planet(i)(1), Planet(i)(3))

                    Dim clr As Color = Color.FromArgb(255, Rnd.Next(255), Rnd.Next(255), Rnd.Next(255))
                    Chart1.Series.Add("Classe " & Serie.ToString)
                    Chart1.Series("Classe " & Serie.ToString).ChartType = SeriesChartType.Point
                    Chart1.Series("Classe " & Serie.ToString).Color = clr
                    Chart1.Series("Classe " & Serie.ToString).Points.AddXY(Planet(i)(1), Planet(i)(3))
                    For j = 0 To UBound(Sat)
                        If Sat(j)(4) = i And Sat(j)(3) < (BornSup / 50) Then
                            Chart1.Series("Classe " & Serie.ToString).Points.AddXY(Sat(j)(1), Sat(j)(3))
                        End If
                    Next
                    Serie = Serie + 1
                End If

                '  Chart 2 (Graphe) 
                If Planet(i)(0) <> -1 Then
                    Chart3.Series("Evolution par centre de classe").Points.AddXY(Planet(i)(1), Planet(i)(3))
                    Chart2.Series("Evolution par centre de classe").Points.AddXY(Planet(i)(1), Planet(i)(3))
                End If
            Next

            For j = 0 To UBound(Sat)
                Chart2.Series("Evolution par instance").Points.AddXY(Sat(j)(1), Sat(j)(3))
            Next
        Else
            MsgBox("Les mesures ne sont pas chargées, veuillez vérifier l'affichage ou le filtre de données dans la fenetre precedente.")
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        '''''''' Chargement classes et objets
        For i = 0 To UBound(Planet)
            If Planet(i)(0) <> -1 Then
                If Classes Is Nothing Then
                    ReDim Classes(0)
                    Classes(0) = {Planet(i)(0), Planet(i)(1), Planet(i)(3), 0.0, 0.0, 0.0, 1}
                    ReDim Objets(0)
                    Objets(0) = {Planet(i)(0), Planet(i)(1), Planet(i)(3), Planet(i)(0), 0.0, i}
                Else
                    ReDim Preserve Classes(UBound(Classes) + 1)
                    Classes(UBound(Classes)) = {Planet(i)(0), Planet(i)(1), Planet(i)(3), 0.0, 0.0, 0.0, 1}
                    ReDim Preserve Objets(UBound(Objets))
                    Objets(UBound(Objets)) = {Planet(i)(0), Planet(i)(1), Planet(i)(3), Planet(i)(0), 0.0, i}
                End If
            End If
        Next

        For i = 0 To UBound(Sat)
            ReDim Preserve Objets(UBound(Objets) + 1)
            Objets(UBound(Objets)) = {Sat(i)(0), Sat(i)(1), Sat(i)(3), Planet(Sat(i)(4))(0), Sat(i)(5), -1}

            Dim k As Integer = -1
            For j = 0 To UBound(Classes)
                If Classes(j)(0) = Planet(Sat(i)(4))(0) Then
                    Classes(j)(1) = (Classes(j)(6) * Classes(j)(1) + Sat(i)(1)) / (Classes(j)(6) + 1)
                    Classes(j)(2) = (Classes(j)(6) * Classes(j)(2) + Sat(i)(3)) / (Classes(j)(6) + 1)
                    Classes(j)(3) = (Classes(j)(6) * Classes(j)(3) + Distance(Classes(j)(1), Sat(i)(1), Classes(j)(2), Sat(i)(3), "Euclidienne")) / (Classes(j)(6) + 1)
                    Classes(j)(4) = (Classes(j)(6) * Classes(j)(4) + Abs(Classes(j)(1) ^ 2 - Sat(i)(1) ^ 2)) / (Classes(j)(6) + 1)
                    Classes(j)(5) = (Classes(j)(6) * Classes(j)(5) + Abs(Classes(j)(2) ^ 2 - Sat(i)(3) ^ 2)) / (Classes(j)(6) + 1)
                    Classes(j)(6) = Classes(j)(6) + 1
                    k = j
                    Exit For
                End If
            Next
            If k <> -1 Then
                Objets(i)(4) = Distance(Objets(i)(1), Classes(k)(1), Objets(i)(2), Classes(k)(2), "Euclidienne")
                Objets(i)(5) = k
            Else
                Objets(i)(4) = Distance(Objets(i)(1), Classes(UBound(Classes))(1), Objets(i)(2), Classes(UBound(Classes))(2), "Euclidienne")
                Objets(i)(5) = UBound(Classes)
            End If
        Next

        For i = 0 To UBound(Objets)
            If Objets(i)(5) = -1 Then
                Objets(i)(4) = Distance(Objets(i)(1), Classes(UBound(Classes))(1), Objets(i)(2), Classes(UBound(Classes))(2), "Euclidienne")
                Objets(i)(5) = UBound(Classes)
            End If
        Next
        ''''''''''''''''''''''''' VNS

        Dim PrevStructure As Integer = 0
        Dim PrevClasses() As Array
        ReDim PrevClasses(UBound(Classes))
        For i = 0 To UBound(Classes)
            PrevClasses(i) = Classes(i).Clone
        Next
        Dim PrevObjets() As Array
        ReDim PrevObjets(UBound(Objets))
        For i = 0 To UBound(Objets)
            PrevObjets(i) = Objets(i).Clone
        Next
        Dim OptimumClasses() As Array
        Dim OptimumObjets() As Array
        Dim OptimumIteration As Integer = -1
        Dim OptimumFF As Double = FitnessFunction(Classes, Objets, "Euclidienne", "InterIntra")

        Dim EndProcess As Boolean = False
        Dim cpt As Integer = 0
        Dim parcours As String = ""
        Dim DetailsParcours As String = ""
        While (Not EndProcess)
            cpt = cpt + 1
            If cpt = 1000 Then
                EndProcess = True
            End If
            Dim CurrentFF As Double = FitnessFunction(Classes, Objets, "Euclidienne", "InterIntra")
            Dim CurrentClasses() As Array
            ReDim CurrentClasses(UBound(Classes))
            For i = 0 To UBound(Classes)
                CurrentClasses(i) = Classes(i).Clone
            Next
            Dim CurrentObjets() As Array
            ReDim CurrentObjets(UBound(Objets))
            For i = 0 To UBound(Objets)
                CurrentObjets(i) = Objets(i).Clone
            Next
            '''''''''''''' première structure de voisinage
            Expand(Classes, Objets)
            Dim NeighbordFF As Double = FitnessFunction(Classes, Objets, "Euclidienne", "InterIntra")
            Dim improvement As Boolean = False
            If NeighbordFF > CurrentFF Then
                improvement = True
                PrevStructure = 1
                PrevClasses = Nothing
                ReDim PrevClasses(UBound(Classes))
                PrevObjets = Nothing
                ReDim PrevObjets(UBound(Objets))
                For i = 0 To UBound(Classes)
                    PrevClasses(i) = Classes(i).Clone
                Next
                For i = 0 To UBound(Objets)
                    PrevObjets(i) = Objets(i).Clone
                Next
                If NeighbordFF > OptimumFF Then
                    OptimumIteration = cpt
                    OptimumFF = NeighbordFF
                    OptimumClasses = Nothing
                    ReDim OptimumClasses(UBound(Classes))
                    OptimumObjets = Nothing
                    ReDim OptimumObjets(UBound(Objets))
                    For i = 0 To UBound(Classes)
                        OptimumClasses(i) = Classes(i).Clone
                    Next
                    For i = 0 To UBound(Objets)
                        OptimumObjets(i) = Objets(i).Clone
                    Next
                End If
                DetailsParcours = DetailsParcours & cpt & "/1 "
                parcours = parcours & "1 "
            Else
                '''''''''''''' deuxième structure de voisinage
                Classes = Nothing
                ReDim Classes(UBound(CurrentClasses))
                Objets = Nothing
                ReDim Objets(UBound(CurrentObjets))
                For i = 0 To UBound(CurrentClasses)
                    Classes(i) = CurrentClasses(i).Clone
                Next
                For i = 0 To UBound(CurrentObjets)
                    Objets(i) = CurrentObjets(i).Clone
                Next
                Reduce(Classes, Objets)
                NeighbordFF = FitnessFunction(Classes, Objets, "Euclidienne", "InterIntra")
                improvement = False
                If NeighbordFF > CurrentFF Then
                    improvement = True
                    PrevStructure = 2
                    PrevClasses = Nothing
                    ReDim PrevClasses(UBound(Classes))
                    PrevObjets = Nothing
                    ReDim PrevObjets(UBound(Objets))
                    For i = 0 To UBound(Classes)
                        PrevClasses(i) = Classes(i).Clone
                    Next
                    For i = 0 To UBound(Objets)
                        PrevObjets(i) = Objets(i).Clone
                    Next
                    If NeighbordFF > OptimumFF Then
                        OptimumIteration = cpt
                        OptimumFF = NeighbordFF
                        OptimumClasses = Nothing
                        ReDim OptimumClasses(UBound(Classes))
                        OptimumObjets = Nothing
                        ReDim OptimumObjets(UBound(Objets))
                        For i = 0 To UBound(Classes)
                            OptimumClasses(i) = Classes(i).Clone
                        Next
                        For i = 0 To UBound(Objets)
                            OptimumObjets(i) = Objets(i).Clone
                        Next
                    End If
                    DetailsParcours = DetailsParcours & cpt & "/2 "
                    parcours = parcours & "2 "
                Else
                    '''''''''''''' troisième structure de voisinage

                    If PrevStructure = 1 Then
                        Classes = Nothing
                        ReDim Classes(UBound(CurrentClasses))
                        Objets = Nothing
                        ReDim Objets(UBound(CurrentObjets))
                        For i = 0 To UBound(CurrentClasses)
                            Classes(i) = CurrentClasses(i).Clone
                        Next
                        For i = 0 To UBound(CurrentObjets)
                            Objets(i) = CurrentObjets(i).Clone
                        Next
                        Reduce(Classes, Objets)
                        NeighbordFF = FitnessFunction(Classes, Objets, "Euclidienne", "InterIntra")
                        improvement = False
                        If True Then 'NeighbordFF > CurrentFF
                            improvement = True
                            PrevStructure = 3
                            PrevClasses = Nothing
                            ReDim PrevClasses(UBound(Classes))
                            PrevObjets = Nothing
                            ReDim PrevObjets(UBound(Objets))
                            For i = 0 To UBound(Classes)
                                PrevClasses(i) = Classes(i).Clone
                            Next
                            For i = 0 To UBound(Objets)
                                PrevObjets(i) = Objets(i).Clone
                            Next
                            DetailsParcours = DetailsParcours & cpt & "/-2 "
                            parcours = parcours & "-2 "
                        Else
                            Classes = Nothing
                            ReDim Classes(UBound(CurrentClasses))
                            Objets = Nothing
                            ReDim Objets(UBound(CurrentObjets))
                            For i = 0 To UBound(CurrentClasses)
                                Classes(i) = CurrentClasses(i).Clone
                            Next
                            For i = 0 To UBound(CurrentObjets)
                                Objets(i) = CurrentObjets(i).Clone
                            Next

                            Shake(Classes, Objets)
                            DetailsParcours = DetailsParcours & cpt & "/S "
                            parcours = parcours & "S "
                        End If
                    ElseIf PrevStructure = 2 Then
                        Classes = Nothing
                        ReDim Classes(UBound(CurrentClasses))
                        Objets = Nothing
                        ReDim Objets(UBound(CurrentObjets))
                        For i = 0 To UBound(CurrentClasses)
                            Classes(i) = CurrentClasses(i).Clone
                        Next
                        For i = 0 To UBound(CurrentObjets)
                            Objets(i) = CurrentObjets(i).Clone
                        Next
                        Expand(Classes, Objets)
                        NeighbordFF = FitnessFunction(Classes, Objets, "Euclidienne", "InterIntra")
                        improvement = False
                        If True Then 'NeighbordFF > CurrentFF
                            improvement = True
                            PrevStructure = 3
                            PrevClasses = Nothing
                            ReDim PrevClasses(UBound(Classes))
                            PrevObjets = Nothing
                            ReDim PrevObjets(UBound(Objets))
                            For i = 0 To UBound(Classes)
                                PrevClasses(i) = Classes(i).Clone
                            Next
                            For i = 0 To UBound(Objets)
                                PrevObjets(i) = Objets(i).Clone
                            Next
                            DetailsParcours = DetailsParcours & cpt & "/-1 "
                            parcours = parcours & "-1 "
                        Else
                            Classes = Nothing
                            ReDim Classes(UBound(CurrentClasses))
                            Objets = Nothing
                            ReDim Objets(UBound(CurrentObjets))
                            For i = 0 To UBound(CurrentClasses)
                                Classes(i) = CurrentClasses(i).Clone
                            Next
                            For i = 0 To UBound(CurrentObjets)
                                Objets(i) = CurrentObjets(i).Clone
                            Next

                            Shake(Classes, Objets)
                            DetailsParcours = DetailsParcours & cpt & "/S "
                            parcours = parcours & "S "
                        End If
                    Else
                        Classes = Nothing
                        ReDim Classes(UBound(CurrentClasses))
                        Objets = Nothing
                        ReDim Objets(UBound(CurrentObjets))
                        For i = 0 To UBound(CurrentClasses)
                            Classes(i) = CurrentClasses(i).Clone
                        Next
                        For i = 0 To UBound(CurrentObjets)
                            Objets(i) = CurrentObjets(i).Clone
                        Next

                        Shake(Classes, Objets)
                        DetailsParcours = DetailsParcours & cpt & "/S "
                        parcours = parcours & "S "
                    End If
                End If

            End If


        End While
        fm_ParcoursVNS.TxtParcours.Text = parcours
        fm_ParcoursVNS.TxtParcoursDetails.Text = DetailsParcours
        Dim NbrClasses As Integer = 0
        Dim TxtClasses As String = ""
        For i = 0 To UBound(OptimumClasses)
            If OptimumClasses(i)(6) > 1 Then
                TxtClasses = TxtClasses & " classe: " & i & " Objets: " & OptimumClasses(i)(6) & vbNewLine
            End If
        Next
        fm_ParcoursVNS.TxtClasses.Text = TxtClasses
        fm_ParcoursVNS.LblOptimum.Text = "La solution optimale est à l'itération : " & OptimumIteration & ". La fonction objectif = " & OptimumFF
        fm_ParcoursVNS.ShowDialog()

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Affichage '''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim Rnd As New Random
        Dim Serie As Integer = 1

        Chart4.Series.Add("Graphe d'évolution")
        Chart4.Series("Graphe d'évolution").ChartType = SeriesChartType.Line
        Chart4.Series("Graphe d'évolution").Color = Color.Blue
        Serie = 1
        For i = 0 To UBound(Classes)
            '   Représentation des classes 
            If Classes(i)(0) <> -1 Then

                Chart4.Series("Graphe d'évolution").Points.AddXY(Classes(i)(1), Classes(i)(2))

                Dim clr As Color = Color.FromArgb(255, Rnd.Next(255), Rnd.Next(255), Rnd.Next(255))
                Chart4.Series.Add("Classe " & Serie.ToString)
                Chart4.Series("Classe " & Serie.ToString).ChartType = SeriesChartType.Point
                Chart4.Series("Classe " & Serie.ToString).Color = clr
                Chart4.Series("Classe " & Serie.ToString).Points.AddXY(Classes(i)(1), Classes(i)(3))
                For j = 0 To UBound(Objets)
                    If Objets(j)(4) = i Then
                        Chart4.Series("Classe " & Serie.ToString).Points.AddXY(Objets(j)(1), Objets(j)(2))
                    End If
                Next
                Serie = Serie + 1
            End If

        Next
    End Sub
End Class