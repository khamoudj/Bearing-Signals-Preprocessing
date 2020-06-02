Option Explicit On
'Option Strict On
Imports System.IO
Imports System.Environment
Imports System.Data.OleDb
Imports System.Runtime.InteropServices
Imports System.Globalization
Imports System.Data.SqlClient
Imports System.Math

Module ModGlobal
    Public connexions() As String = Nothing
    Public tvnumero As String
    Public tvdate As String
    Public tvremarques As String
    Public ConnectionString As String = "Data Source=BCRICH-PC;Initial Catalog=Pronostia;User ID=sa;Password=GMB2015"
    Public conn As New SqlConnection(connectionstring)
    Public Class TargetSociete
        Public codeSociete As String
        Public LibSociete As String
    End Class
    Public Class TargetUG
        Public codeUG As String
        Public LibUG As String
    End Class
    Public Class Targetuser
        Public ID As String
        Public Group As String
    End Class
    Public idCol As String = "0"
    Public UG As New TargetUG
    Public Societe As New TargetSociete
    Public TargetConnetion As String
    Public User As New Targetuser
    Public Exercice As String
    Public CNN As OleDbConnection
    Public sqlReq As String = ""
    Public sqlReq1 As String = ""
    Public sqlReq2 As String = ""
    Public Annee As Integer
    Public mainSQL As New SQLFunctions

    Public Function EnLettres(ByVal chiffre As Double) As String

        Dim centaine As Integer
        Dim dizaine As Integer
        Dim unite As Integer
        Dim reste As Integer
        Dim y As Integer
        Dim dix As Boolean = False
        Dim virgule As Integer
        Dim lettre As String = ""
        Dim fin As Boolean = False


        reste = CInt(chiffre)
        If reste > chiffre Then
            reste = reste - 1
        End If

        virgule = CInt((chiffre - reste) * 100)
        Dim i As Integer = 1000000000
        'Dim k As Integer = i \ 1000
        'For i = 1000000000 To 1 Step -k
        While i >= 0 And Not fin

            If i = 0 Then
                y = virgule
                If (Len(lettre) = 0) Then
                    lettre = lettre + "zero DA et "
                Else
                    lettre = lettre & " DA et "
                End If
                fin = True
            Else
                y = reste \ i
            End If
            '******************************
            If (y <> 0) Then

                centaine = y \ 100
                dizaine = (y - centaine * 100) \ 10
                unite = y - (centaine * 100) - (dizaine * 10)
                '*******************
                Select Case centaine
                    Case 0

                    Case 1
                        lettre = lettre + "cent "

                    Case 2
                        If ((dizaine = 0) And (unite = 0)) Then
                            lettre = lettre + "deux cents "
                        Else
                            lettre = lettre + "deux cent "
                        End If

                    Case 3
                        If ((dizaine = 0) And (unite = 0)) Then
                            lettre = lettre + "trois cents "
                        Else
                            lettre = lettre + "trois cent "
                        End If

                    Case 4
                        If ((dizaine = 0) And (unite = 0)) Then
                            lettre = lettre + "quatre cents "
                        Else
                            lettre = lettre + "quatre cent "
                        End If

                    Case 5
                        If ((dizaine = 0) And (unite = 0)) Then
                            lettre = lettre + "cinq cents "
                        Else
                            lettre = lettre + "cinq cent "
                        End If

                    Case 6
                        If ((dizaine = 0) And (unite = 0)) Then
                            lettre = lettre + "six cents "
                        Else
                            lettre = lettre + "six cent "
                        End If

                    Case 7
                        If ((dizaine = 0) And (unite = 0)) Then
                            lettre = lettre + "sept cents "
                        Else
                            lettre = lettre + "sept cent "
                        End If

                    Case 8
                        If ((dizaine = 0) And (unite = 0)) Then
                            lettre = lettre + "huit cents "
                        Else
                            lettre = lettre + "huit cent "
                        End If

                    Case 9
                        If ((dizaine = 0) And (unite = 0)) Then
                            lettre = lettre + "neuf cents "
                        Else
                            lettre = lettre + "neuf cent "
                        End If
                End Select
                '*****************************
                '********************************
                Select Case dizaine
                    Case 0

                    Case 1
                        dix = True

                    Case 2
                        lettre = lettre + "vingt "

                    Case 3
                        lettre = lettre + "trente "

                    Case 4
                        lettre = lettre + "quarante "

                    Case 5
                        lettre = lettre + "cinquante "

                    Case 6
                        lettre = lettre + "soixante "

                    Case 7
                        dix = True
                        lettre = lettre + "soixante "

                    Case 8
                        lettre = lettre + "quatre-vingt "

                    Case 9
                        dix = True
                        lettre = lettre + "quatre-vingt "
                End Select
                '*******************************
                '********************************
                Select Case unite
                    Case 0
                        If (dix) Then
                            lettre = lettre + "dix "
                        End If

                    Case 1
                        If (dix) Then
                            lettre = lettre + "onze "
                        Else
                            lettre = lettre + "un "
                        End If

                    Case 2
                        If (dix) Then
                            lettre = lettre + "douze "
                        Else
                            lettre = lettre + "deux "
                        End If

                    Case 3
                        If (dix) Then
                            lettre = lettre + "treize "
                        Else
                            lettre = lettre + "trois "
                        End If

                    Case 4
                        If (dix) Then
                            lettre = lettre + "quatorze "
                        Else
                            lettre = lettre + "quatre "
                        End If

                    Case 5
                        If (dix) Then
                            lettre = lettre + "quinze "
                        Else
                            lettre = lettre + "cinq "
                        End If

                    Case 6
                        If (dix) Then
                            lettre = lettre + "seize "
                        Else
                            lettre = lettre + "six "
                        End If

                    Case 7
                        If (dix) Then
                            lettre = lettre + "dix-sept "
                        Else
                            lettre = lettre + "sept "
                        End If

                    Case 8
                        If (dix) Then
                            lettre = lettre + "dix-huit "
                        Else
                            lettre = lettre + "huit "
                        End If

                    Case 9
                        If (dix) Then
                            lettre = lettre + "dix-neuf "
                        Else
                            lettre = lettre + "neuf "
                        End If
                End Select
                '*********************************
                '**********************************
                Select Case i
                    Case 1000000000
                        If (y > 1) Then
                            lettre = lettre + "milliards "
                        Else
                            lettre = lettre + "milliard "
                        End If

                    Case 1000000
                        If (y > 1) Then
                            lettre = lettre + "millions "
                        Else
                            lettre = lettre + "million "
                        End If

                    Case 1000
                        lettre = lettre + "mille "
                End Select
                '**************************************
            End If
            '************************************
            reste = reste - y * i
            dix = False
            i = i \ 1000
            If fin Then
                Dim sp As String
                sp = lettre.Split(CChar("D"))(1)
                If sp = "A et " Then
                    lettre = lettre & " zéro Ct"
                Else
                    lettre = lettre & "Cts"
                End If

            End If
        End While
        'Next i
        ' end for

        Return lettre

    End Function




    Public Function getIsNumeric(table As String, col As String) As Boolean
        'reslut(description text, size[small, medium, big], type[int, real, varchar, date])
        Dim result(3) As String
        'Get desc:
        Dim s As String = "SELECT DescriptionCol FROM C_SYS WHERE NomTable = '" + table + "' AND NomCol='" + col + "' AND DescriptionCol<>''"
        Dim sc As OleDbCommand = New OleDbCommand(s, CNN)
        Dim sr As OleDbDataReader = Nothing
        Try
            sr = sc.ExecuteReader()
            While sr.Read()
                result(0) = sr(0)
            End While
            sr.Close()
        Catch ex As Exception
            MsgBox("Erreur de getDataType (CODE:001): " + ex.Message)
            Try
                sr.Close()
            Catch ex1 As Exception

            End Try
        End Try
        'Get type:
        Dim t As String = "select data_type from INFORMATION_SCHEMA.COLUMNS where table_name = '" + table + "' AND column_name = '" + col + "'"
        Dim tc As OleDbCommand = New OleDbCommand(t, CNN)
        Dim tr As OleDbDataReader = Nothing
        Try
            tr = tc.ExecuteReader()
            While tr.Read()
                result(2) = tr(0)
            End While
            tr.Close()
        Catch ex As Exception
            MsgBox("Erreur de getDataType (CODE:003): " + ex.Message)
            Try
                tr.Close()
            Catch ex1 As Exception

            End Try
        End Try
        'Get size:
        result(1) = "medium"
        If result(2) = "varchar" Then
            Dim si As String = "SELECT COL_LENGTH('" + table + "','" + col + "') AS 'VarChar' "
            Dim sic As OleDbCommand = New OleDbCommand(si, CNN)
            Dim sir As OleDbDataReader = Nothing
            Try
                sir = sic.ExecuteReader()
                While sir.Read()
                    If sir(0) < 50 Then
                        result(1) = "small"
                    ElseIf sir(0) > 50 Then
                        result(1) = "big"
                    End If
                End While
                sir.Close()
            Catch ex As Exception
                MsgBox("Erreur de getDataType (CODE:002): " + ex.Message)
                Try
                    sir.Close()
                Catch ex1 As Exception

                End Try
            End Try
        End If
        If result(2) = "real" Or result(2).Contains("int") Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function getConnectionString() As String
        Dim result As String = ""
        Dim mydlg As New MSDASC.DataLinks()
        Dim ADOcon As New ADODB.Connection
        Try
            ADOcon = CType(mydlg.PromptNew, ADODB.Connection)
            ADOcon.Open()
            result = ADOcon.ConnectionString
        Catch ex1 As Exception
            'MsgBox(ex1.ToString)
        End Try
        Return result
    End Function
    Public Sub Connect()
        Try
            Dim mydlg As New MSDASC.DataLinks()
            Dim ADOcon As New ADODB.Connection
            Dim appData As String = GetFolderPath(SpecialFolder.ApplicationData)
            Dim connectFile As String = appData + "\CNNClst.trg"
            Dim fichierOK As Integer = 0
            If Not File.Exists(connectFile) Then
                File.Create(connectFile)
            End If
            Try
                ADOcon.ConnectionString = File.ReadAllText(connectFile)
                ADOcon.Open()
                fichierOK = 1
            Catch ex As Exception

                MsgBox(ex.Message)

            End Try
            If fichierOK = 0 Then
                Try
                    ADOcon = CType(mydlg.PromptNew, ADODB.Connection)
                    ADOcon.Open()
                    File.WriteAllText(connectFile, ADOcon.ConnectionString)
                Catch ex1 As Exception
                    MsgBox(ex1.ToString)
                End Try
            End If
            TargetConnetion = ADOcon.ConnectionString
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Function convertToMoney(str As String) As String
        If Not str.Contains(" ") And Not str.Contains(".") Then
            Try
                str = str.Replace(".", ",")
                Dim price As Double = Convert.ToDouble(str)
                str = String.Format("{0:C}", price)
                str = str.Substring(0, str.Length - 2)
                Return str
            Catch ex As Exception
                Return str
            End Try
        Else
            Return str
        End If
    End Function
    Public Function sqlTxt(txt As String) As String
        Dim txtS As String
        txtS = Replace(txt, "'", "''")
        sqlTxt = "'" & Trim(txtS) & "'"
    End Function
    Public Function StartConnection() As Boolean
        Dim ok As Boolean
        Connect()
        CNN = New OleDbConnection(TargetConnetion)
        Try
            CNN.Open()
            ok = True
            Return ok
        Catch ex As Exception
            Return ok
            'MsgBox("Erreur lors de la connection au serveur: " + ex.Message)
            'Connect()
        End Try
        'If ok Then
        'Else
        'StartConnection()
        'End If
        ' MsgBox("Connexion à la base de données établie avec succès")        
    End Function

    Public Sub LinkTxtToChk(ByVal txt As TextBox, ByVal chk As CheckBox)
        AddHandler txt.TextChanged, Sub(sender, e) UpdateChkFromTxt(txt, chk)
        AddHandler chk.CheckedChanged, Sub(sender, e) UpdateTxtFromChk(txt, chk)
        UpdateChkFromTxt(txt, chk)
    End Sub
    Public Sub UpdateChkFromTxt(ByVal txt As TextBox, ByVal chk As CheckBox)
        Try
            Select Case CInt(txt.Text)
                Case 0
                    chk.Checked = False
                Case Else
                    chk.Checked = True
            End Select
        Catch ex As Exception
            chk.Checked = False
        End Try
    End Sub
    Public Sub UpdateTxtFromChk(ByVal txt As TextBox, ByRef chk As CheckBox)
        Select Case chk.Checked
            Case True
                txt.Text = "1"
            Case Else
                txt.Text = "0"
        End Select
    End Sub
    Public Function tDate(DateI As Date) As String
        Dim dateRet As String = ""
        tDate = dateRet
    End Function
    Public Sub LinkTxtToOpts(txt As TextBox, opts As List(Of RadioButton))
        Dim ii As Integer
        For ii = 0 To opts.Count - 1
            AddHandler opts(ii).CheckedChanged, Sub(sender, e) RadioToText(txt, opts)
        Next
        AddHandler txt.TextChanged, Sub(sender, e) TextToRadio(txt, opts)
        TextToRadio(txt, opts)
    End Sub
    Private Sub TextToRadio(txt As TextBox, opts As List(Of RadioButton))
        Dim ii As Integer
        For ii = 0 To opts.Count - 1
            If opts(ii).Text = Trim(txt.Text) Then opts(ii).Checked = True
        Next
    End Sub
    Private Sub RadioToText(txt As TextBox, opts As List(Of RadioButton))
        Dim ii As Integer
        For ii = 0 To opts.Count - 1
            If opts(ii).Checked Then
                txt.Text = opts(ii).Text
            End If
        Next
    End Sub
    Public Function getColID(tbl As String, Col As String) As Integer
        Dim id As Integer
        id = -1
        Try
            sqlReq = "select column_id from " & CNN.Database.ToString & ".sys.columns"
            sqlReq = sqlReq & " where object_id=object_id('" & CNN.Database.ToString & ".dbo." & tbl & "') and name = " & sqlTxt(Col) & ""
            Dim sqlCom As New OleDbCommand(sqlReq, CNN)
            Dim dr As OleDbDataReader = Nothing
            dr = sqlCom.ExecuteReader()
            Do While dr.Read
                id = CInt(dr(0))
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        If id = -1 Then
            MsgBox("Colonne introuvable dans la table, contacter votre administrateur de base de données!")
        End If
        getColID = id - 1
    End Function
    Public Function RS(txt As String, connectStr As OleDbConnection) As DataTable
        RS = New DataTable
        Try
            Dim sqlCom As New OleDbCommand(txt, connectStr)
            Dim dr As New OleDbDataAdapter
            dr.SelectCommand = sqlCom
            dr.Fill(RS)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function
    Public Sub LinkTableToGrid(Table As String, ds As DataSet, grd As DataGridView, Filter As String)
        Dim Reqsql As String = "SELECT * FROM " + Table + " where " + Filter
        Dim da As New OleDbDataAdapter
        '     Dim sqlC As oledbcommand = New oledbcommand(sql, cnn)
        da = New OleDbDataAdapter(Reqsql, CNN)
        da.SelectCommand.Connection = CNN
        da.SelectCommand.CommandText = Reqsql
        Try
            da.Fill(ds, Table)
            grd.DataSource = ds.Tables(0)
        Catch ex As Exception
            MsgBox("Erreur lors du chargement de données: " + ex.Message)
        End Try
    End Sub

    Private Function sqlDate(DateEnt As Date) As String
        Dim DateSort As Date
        sqlDate = CStr(DateEnt)
        If DateTime.TryParseExact(CStr(DateEnt), "dd/mm/yyyy", Nothing, DateTimeStyles.None, DateSort) Then
            sqlDate = CStr(DateSort)
        Else
            MsgBox("Impossible de convertir les dates, contacter votre administrateur système!")
        End If
    End Function
    Public Function getCFCompteur(CodeFlx As String, NatFlx As String, AnneeCpt As Integer) As Integer
        Dim cpt As Integer
        cpt = -1
        Try
            sqlReq = "select valeur from CF_Compteurs where CodeOp=" & sqlTxt(CodeFlx) & " and CodeNatFlx=" & sqlTxt(NatFlx)
            sqlReq = sqlReq & " and annee=" & AnneeCpt
            Dim sqlCom As New OleDbCommand(sqlReq, CNN)
            Dim dr As OleDbDataReader = Nothing
            dr = sqlCom.ExecuteReader()
            Do While dr.Read
                cpt = CInt(dr(0))
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        If cpt = -1 Then
            MsgBox("Compteur d'opération trésorerie: " & CodeFlx & "/" & NatFlx & " introuvable, contacter votre administrateur de système!")
        End If
        getCFCompteur = cpt
    End Function
    Public Function getCPCompteur(CodeJournal As String, Anneecpt As Integer) As Integer
        Dim cpt As Integer
        cpt = -1
        Try
            sqlReq = "select valeur from CP_Compteurs where CodeJournal=" & sqlTxt(CodeJournal)
            sqlReq = sqlReq & " and annee=" & Anneecpt
            Dim sqlCom As New OleDbCommand(sqlReq, CNN)
            Dim dr As OleDbDataReader = Nothing
            dr = sqlCom.ExecuteReader()
            Do While dr.Read
                cpt = CInt(dr(0))
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        If cpt = -1 Then
            MsgBox("Compteur du journal comptable: " & CodeJournal & "  introuvable, contacter votre administrateur de système!")
        End If
        getCPCompteur = cpt
    End Function
    Public Function MajCPCompteur(NewVal As Integer, CodeJournal As String, Anneecpt As Integer) As Boolean
        MajCPCompteur = False
        Try
            sqlReq = "update CP_Compteurs set valeur=" & NewVal & " where CodeJournal=" & sqlTxt(CodeJournal)
            sqlReq = sqlReq & " and annee=" & Anneecpt
            Dim sqlCom As New OleDbCommand(sqlReq, CNN)
            sqlCom.ExecuteReader()
            MajCPCompteur = True
        Catch ex As Exception
            MsgBox("Mise à jour du compteur pour le journal comptable: " & CodeJournal & "  echouée, contacter votre administrateur de système!")
        End Try
    End Function
    Public Function MajCFCompteur(NewVal As Integer, CodeOp As String, NatFlx As String, Anneecpt As Integer) As Boolean
        MajCFCompteur = False
        Try
            sqlReq = "update CF_Compteurs set valeur=" & NewVal & " where CodeOp=" & sqlTxt(CodeOp) & " and CodeNatFlx=" & sqlTxt(NatFlx)
            sqlReq = sqlReq & " and annee=" & Anneecpt
            Dim sqlCom As New OleDbCommand(sqlReq, CNN)
            sqlCom.ExecuteReader()
            MajCFCompteur = True
        Catch ex As Exception
            MsgBox("Mise à jour du compteur : " & CodeOp & "/" & NatFlx & "  echouée, contacter votre administrateur de système!")
        End Try
    End Function
    Public Function InitCFCompteur(NewVal As Integer, CodeOp As String, NatFlx As String, Anneecpt As Integer) As Boolean
        InitCFCompteur = False
        Try
            sqlReq = "insert into CF_Compteurs(EP,UG,CodeOp,CodeNatFlx,Annee,Valeur) values (" & sqlTxt(Societe.codeSociete) & " , "
            sqlReq = sqlReq & sqlTxt(UG.codeUG) & " , "
            sqlReq = sqlReq & sqlTxt(CodeOp) & " , "
            sqlReq = sqlReq & sqlTxt(NatFlx) & " , "
            sqlReq = sqlReq & Anneecpt & " , "
            sqlReq = sqlReq & NewVal & " ) "

            Dim sqlCom As New OleDbCommand(sqlReq, CNN)
            sqlCom.ExecuteReader()
            InitCFCompteur = True
        Catch ex As Exception
            MsgBox("Initialisation du compteur : " & CodeOp & "/" & NatFlx & "  echouée, contacter votre administrateur de système!")
        End Try
    End Function
    Public Function InitCPCompteur(NewVal As Integer, CodeJournal As String, AnneeCpt As Integer) As Boolean
        InitCPCompteur = False
        Try
            sqlReq = "insert into CP_Compteurs(EP,UG,CodeJournal,Annee,Valeur) values (" & sqlTxt(Societe.codeSociete) & " , "
            sqlReq = sqlReq & sqlTxt(UG.codeUG) & " , "
            sqlReq = sqlReq & sqlTxt(CodeJournal) & " , "
            sqlReq = sqlReq & AnneeCpt & " , "
            sqlReq = sqlReq & NewVal & " ) "
            Dim sqlCom As New OleDbCommand(sqlReq, CNN)
            sqlCom.ExecuteReader()
            InitCPCompteur = True
        Catch ex As Exception
            MsgBox("Initialisation du compteur pour le journal comptable: " & CodeJournal & "  echouée, contacter votre administrateur de système!")
        End Try
    End Function
    Public Function CompteurExistantCP(CodeJournal As String, Anneecpt As Integer) As Boolean
        CompteurExistantCP = False
        Try
            sqlReq = "select valeur from CP_Compteurs where CodeJournal=" & sqlTxt(CodeJournal)
            sqlReq = sqlReq & " and annee=" & Anneecpt
            Dim sqlCom As New OleDbCommand(sqlReq, CNN)
            Dim dr As OleDbDataReader = Nothing
            dr = sqlCom.ExecuteReader()
            Do While dr.Read
                CompteurExistantCP = True
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Function
    Public Function CompteurExistantCF(CodeFlx As String, NatFlx As String, Anneecpt As Integer) As Boolean
        CompteurExistantCF = False
        Try
            sqlReq = "select valeur from CF_Compteurs where CodeOp=" & sqlTxt(CodeFlx) & " and CodeNatFlx=" & sqlTxt(NatFlx)
            sqlReq = sqlReq & " and annee=" & Anneecpt
            Dim sqlCom As New OleDbCommand(sqlReq, CNN)
            Dim dr As OleDbDataReader = Nothing
            dr = sqlCom.ExecuteReader()
            Do While dr.Read
                CompteurExistantCF = True
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Function arrondissement(val As Double, Optional ordre As Integer = 2) As Double
        Dim x As Double
        Dim ord As Integer = 10 ^ ordre
        x = ((val * ord) \ 1) / ord
        Return x
    End Function

    Public Function Distance(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, Optional TypeDist As String = "Euclidienne") As Double
        Dim dist As Double
        If TypeDist = "Euclidienne" Then
            dist = Sqrt((x1 - x2) ^ 2 + Abs(y1 - y2) ^ 2)
        Else
            dist = Abs(x1 - x2) + Abs(y1 - y2)
        End If
        Return dist
    End Function
    Public Function DInterClass(Classes() As Array, Optional TypeDist As String = "Euclidienne") As Double
        Dim D As Double = 0
        If TypeDist = "Euclidienne" Then
            For i = 0 To UBound(Classes)
                For j = 0 To UBound(Classes)
                    D = D + Sqrt((Classes(i)(1) - Classes(j)(1)) ^ 2 + (Classes(i)(2) - Classes(j)(2)) ^ 2)
                Next
            Next
        Else
            For i = 0 To UBound(Classes)
                For j = 0 To UBound(Classes)
                    D = D + Abs(Classes(i)(1) - Classes(j)(1)) + Abs(Classes(i)(2) - Classes(j)(2))
                Next
            Next
        End If
        Return D
    End Function
    Public Function DIntraClass(Classes() As Array, Objets() As Array, Optional TypeDist As String = "Euclidienne") As Double
        Dim D As Double
        If TypeDist = "Euclidienne" Then
            For i = 0 To UBound(Objets)
                D = D + Sqrt((Objets(i)(1) - Classes(Objets(i)(5))(1)) ^ 2 + (Objets(i)(2) - Classes(Objets(i)(5))(2)) ^ 2)
            Next
        Else
            For i = 0 To UBound(Objets)
                D = D + Abs(Objets(i)(1) - Classes(Objets(i)(5))(1)) + Abs(Objets(i)(2) - Classes(Objets(i)(5))(2))
            Next
        End If
        Return D
    End Function
    Public Function FitnessFunction(Classes() As Array, Objets() As Array, Optional TypeDist As String = "Euclidienne", Optional Distance As String = "InterIntra") As Double
        Dim FF As Double = 0
        If Distance = "InterIntra" Then
            FF = 2 * DInterClass(Classes, TypeDist) / ((UBound(Classes) - 1) * DIntraClass(Classes, Objets, TypeDist))
        Else
            FF = DIntraClass(Classes, Objets, TypeDist)
        End If
        Return FF
    End Function
    Public Function Min(Classes() As Array, colonne As Integer) As Integer
        Dim indice As Integer = 0
        For i = 1 To UBound(Classes)
            If Classes(i)(colonne) < Classes(indice)(colonne) Then
                indice = i
            End If
        Next
        Return indice
    End Function
    Public Function Max(Classes() As Array, colonne As Integer) As Integer
        Dim indice As Integer = 0
        For i = 1 To UBound(Classes)
            If Classes(i)(colonne) > Classes(indice)(colonne) Then
                indice = i
            End If
        Next
        Return indice
    End Function
    Public Function MeanRay(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, Optional TypeDist As String = "Euclidienne") As Double
        Dim V As Double

        Return V
    End Function
    Public Function Variance(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, Optional TypeDist As String = "Euclidienne") As Double
        Dim V As Double

        Return V
    End Function

    Public Sub Expand(Classes() As Array, Objets() As Array)
        Dim minIndex As Integer = Min(Classes, 5)
        Dim MR As Double = Classes(minIndex)(3) * 2
        For i = 0 To UBound(Objets)
            Dim dist As Double = Distance(Objets(i)(1), Classes(minIndex)(1), Objets(i)(2), Classes(minIndex)(2), "Euclidienne")
            If dist < MR Then
                Dim ExDistance As Integer = Objets(i)(4)
                Dim ExIndiceClasse As Integer = Objets(i)(5)

                Objets(i)(3) = Classes(minIndex)(0)
                Objets(i)(4) = dist
                Objets(i)(5) = minIndex

                Classes(minIndex)(1) = (Classes(minIndex)(6) * Classes(minIndex)(1) + Objets(i)(1)) / (Classes(minIndex)(6) + 1)
                Classes(minIndex)(2) = (Classes(minIndex)(6) * Classes(minIndex)(2) + Objets(i)(2)) / (Classes(minIndex)(6) + 1)
                Classes(minIndex)(3) = (Classes(minIndex)(6) * Classes(minIndex)(3) + Distance(Classes(minIndex)(1), Objets(i)(1), Classes(minIndex)(2), Objets(i)(2), "Euclidienne")) / (Classes(minIndex)(6) + 1)
                Classes(minIndex)(4) = (Classes(minIndex)(6) * Classes(minIndex)(4) + Abs(Classes(minIndex)(1) ^ 2 - Objets(i)(1) ^ 2)) / (Classes(minIndex)(6) + 1)
                Classes(minIndex)(5) = (Classes(minIndex)(6) * Classes(minIndex)(5) + Abs(Classes(minIndex)(2) ^ 2 - Objets(i)(2) ^ 2)) / (Classes(minIndex)(6) + 1)
                Classes(minIndex)(6) = Classes(minIndex)(6) + 1

                Classes(ExIndiceClasse)(1) = (Classes(ExIndiceClasse)(6) * Classes(ExIndiceClasse)(1) - Objets(i)(1)) / (Classes(ExIndiceClasse)(6))
                Classes(ExIndiceClasse)(2) = (Classes(ExIndiceClasse)(6) * Classes(ExIndiceClasse)(2) - Objets(i)(2)) / (Classes(ExIndiceClasse)(6))
                Classes(ExIndiceClasse)(3) = (Classes(ExIndiceClasse)(6) * Classes(ExIndiceClasse)(3) - Distance(Classes(ExIndiceClasse)(1), Objets(i)(1), Classes(ExIndiceClasse)(2), Objets(i)(2), "Euclidienne")) / (Classes(ExIndiceClasse)(6))
                Classes(ExIndiceClasse)(4) = (Classes(ExIndiceClasse)(6) * Classes(ExIndiceClasse)(4) - Abs(Classes(ExIndiceClasse)(1) ^ 2 - Objets(i)(1) ^ 2)) / (Classes(ExIndiceClasse)(6))
                Classes(ExIndiceClasse)(5) = (Classes(ExIndiceClasse)(6) * Classes(ExIndiceClasse)(5) - Abs(Classes(ExIndiceClasse)(2) ^ 2 - Objets(i)(2) ^ 2)) / (Classes(ExIndiceClasse)(6))
                Classes(ExIndiceClasse)(6) = Classes(ExIndiceClasse)(6) - 1
            End If
        Next
    End Sub
    Public Sub Reduce(Classes() As Array, Objets() As Array)
        Dim maxIndex As Integer = Max(Classes, 5)
        Dim MR As Double = Classes(maxIndex)(3)
        For i = 0 To UBound(Objets)
            If Objets(i)(5) = maxIndex Then
                Dim dist As Double = Distance(Objets(i)(1), Classes(maxIndex)(1), Objets(i)(2), Classes(maxIndex)(2), "Euclidienne")
                If dist > MR Then
                    Dim ExDistance As Integer = Objets(i)(4)
                    Dim ExIndiceClasse As Integer = Objets(i)(5)
                    Dim NearClassDistance As Double = dist * 2
                    Dim NearClassIndex As Integer = maxIndex
                    For j = 0 To UBound(Classes)
                        If j <> maxIndex Then
                            Dim D As Double = Distance(Objets(i)(1), Classes(j)(1), Objets(i)(2), Classes(j)(2), "Euclidienne")
                            If D < NearClassDistance Then
                                NearClassDistance = D
                                NearClassIndex = j
                            End If
                        End If
                    Next
                    If NearClassIndex <> ExIndiceClasse Then
                        Classes(NearClassIndex)(1) = (Classes(NearClassIndex)(6) * Classes(NearClassIndex)(1) + Objets(i)(1)) / (Classes(NearClassIndex)(6) + 1)
                        Classes(NearClassIndex)(2) = (Classes(NearClassIndex)(6) * Classes(NearClassIndex)(2) + Objets(i)(2)) / (Classes(NearClassIndex)(6) + 1)
                        Classes(NearClassIndex)(3) = (Classes(NearClassIndex)(6) * Classes(NearClassIndex)(3) + Distance(Classes(NearClassIndex)(1), Objets(i)(1), Classes(NearClassIndex)(2), Objets(i)(2), "Euclidienne")) / (Classes(NearClassIndex)(6) + 1)
                        Classes(NearClassIndex)(4) = (Classes(NearClassIndex)(6) * Classes(NearClassIndex)(4) + Abs(Classes(NearClassIndex)(1) ^ 2 - Objets(i)(1) ^ 2)) / (Classes(NearClassIndex)(6) + 1)
                        Classes(NearClassIndex)(5) = (Classes(NearClassIndex)(6) * Classes(NearClassIndex)(5) + Abs(Classes(NearClassIndex)(2) ^ 2 - Objets(i)(2) ^ 2)) / (Classes(NearClassIndex)(6) + 1)
                        Classes(NearClassIndex)(6) = Classes(NearClassIndex)(6) + 1

                        Classes(ExIndiceClasse)(1) = (Classes(ExIndiceClasse)(6) * Classes(ExIndiceClasse)(1) - Objets(i)(1)) / (Classes(ExIndiceClasse)(6))
                        Classes(ExIndiceClasse)(2) = (Classes(ExIndiceClasse)(6) * Classes(ExIndiceClasse)(2) - Objets(i)(2)) / (Classes(ExIndiceClasse)(6))
                        Classes(ExIndiceClasse)(3) = (Classes(ExIndiceClasse)(6) * Classes(ExIndiceClasse)(3) - Distance(Classes(ExIndiceClasse)(1), Objets(i)(1), Classes(ExIndiceClasse)(2), Objets(i)(2), "Euclidienne")) / (Classes(ExIndiceClasse)(6))
                        Classes(ExIndiceClasse)(4) = (Classes(ExIndiceClasse)(6) * Classes(ExIndiceClasse)(4) - Abs(Classes(ExIndiceClasse)(1) ^ 2 - Objets(i)(1) ^ 2)) / (Classes(ExIndiceClasse)(6))
                        Classes(ExIndiceClasse)(5) = (Classes(ExIndiceClasse)(6) * Classes(ExIndiceClasse)(5) - Abs(Classes(ExIndiceClasse)(2) ^ 2 - Objets(i)(2) ^ 2)) / (Classes(ExIndiceClasse)(6))
                        Classes(ExIndiceClasse)(6) = Classes(ExIndiceClasse)(6) - 1

                        Objets(i)(3) = Classes(NearClassIndex)(0)
                        Objets(i)(4) = NearClassDistance
                        Objets(i)(5) = NearClassIndex
                    End If


                End If
            End If

        Next
    End Sub
    Public Function ReturnBack(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, Optional TypeDist As String = "Euclidienne") As Double
        Dim D As Double

        Return D
    End Function
    Public Sub Shake(Classes() As Array, Objets() As Array)
        For i = 0 To UBound(Classes)
            Dim C As Integer = CInt(Ceiling(Rnd() * UBound(Classes)))
            Dim O As Integer = CInt(Ceiling(Rnd() * UBound(Objets)))
            Dim ClassIndex As Integer = C
            Dim ExIndiceClasse As Integer = Objets(O)(5)
            Classes(ClassIndex)(1) = (Classes(ClassIndex)(6) * Classes(ClassIndex)(1) + Objets(O)(1)) / (Classes(ClassIndex)(6) + 1)
            Classes(ClassIndex)(2) = (Classes(ClassIndex)(6) * Classes(ClassIndex)(2) + Objets(O)(2)) / (Classes(ClassIndex)(6) + 1)
            Classes(ClassIndex)(3) = (Classes(ClassIndex)(6) * Classes(ClassIndex)(3) + Distance(Classes(ClassIndex)(1), Objets(O)(1), Classes(ClassIndex)(2), Objets(O)(2), "Euclidienne")) / (Classes(ClassIndex)(6) + 1)
            Classes(ClassIndex)(4) = (Classes(ClassIndex)(6) * Classes(ClassIndex)(4) + Abs(Classes(ClassIndex)(1) ^ 2 - Objets(O)(1) ^ 2)) / (Classes(ClassIndex)(6) + 1)
            Classes(ClassIndex)(5) = (Classes(ClassIndex)(6) * Classes(ClassIndex)(5) + Abs(Classes(ClassIndex)(2) ^ 2 - Objets(O)(2) ^ 2)) / (Classes(ClassIndex)(6) + 1)
            Classes(ClassIndex)(6) = Classes(ClassIndex)(6) + 1

            Classes(ExIndiceClasse)(1) = (Classes(ExIndiceClasse)(6) * Classes(ExIndiceClasse)(1) - Objets(O)(1)) / (Classes(ExIndiceClasse)(6))
            Classes(ExIndiceClasse)(2) = (Classes(ExIndiceClasse)(6) * Classes(ExIndiceClasse)(2) - Objets(O)(2)) / (Classes(ExIndiceClasse)(6))
            Classes(ExIndiceClasse)(3) = (Classes(ExIndiceClasse)(6) * Classes(ExIndiceClasse)(3) - Distance(Classes(ExIndiceClasse)(1), Objets(O)(1), Classes(ExIndiceClasse)(2), Objets(O)(2), "Euclidienne")) / (Classes(ExIndiceClasse)(6))
            Classes(ExIndiceClasse)(4) = (Classes(ExIndiceClasse)(6) * Classes(ExIndiceClasse)(4) - Abs(Classes(ExIndiceClasse)(1) ^ 2 - Objets(O)(1) ^ 2)) / (Classes(ExIndiceClasse)(6))
            Classes(ExIndiceClasse)(5) = (Classes(ExIndiceClasse)(6) * Classes(ExIndiceClasse)(5) - Abs(Classes(ExIndiceClasse)(2) ^ 2 - Objets(O)(2) ^ 2)) / (Classes(ExIndiceClasse)(6))
            Classes(ExIndiceClasse)(6) = Classes(ExIndiceClasse)(6) - 1

            Objets(O)(3) = Classes(ClassIndex)(0)
            Objets(O)(4) = Distance(Objets(O)(1), Classes(ClassIndex)(1), Objets(O)(2), Classes(ClassIndex)(2), "Euclidienne")
            Objets(O)(5) = ClassIndex
        Next
    End Sub

End Module