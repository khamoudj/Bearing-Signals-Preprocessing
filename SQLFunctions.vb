Imports System.Data.OleDb
Imports System.Data.SqlClient
Public Class SQLFunctions
    
    Public Function sqlFix(txt As String)
        txt.Replace("'", "''")
        Return txt
    End Function
    Public Function ReturnId(table As String, value1 As String, col1 As String, Optional value2 As String = "", Optional col2 As String = "", Optional value3 As String = "", Optional col3 As String = "")
        Dim id As String = -1
        Dim req As String = "SELECT isnull(id,-1) FROM " & table & " WHERE "
        If col1 <> "" Then
            req = req & col1 & "='" & sqlFix(value1) & "'"
        End If
        If col2 <> "" Then
            req = req & " AND " & col2 & "='" & sqlFix(value2) & "'"
        End If
        If col3 <> "" Then
            req = req & " AND " & col3 & "='" & sqlFix(value3) & "'"
        End If
        Dim ids = OneResRequest(req)
        If ids IsNot Nothing Then
            id = ids(0)
        End If
        Return id
    End Function
    Public Function SqlDelete(table As String, colname As String, colvalue As String)
        colvalue = colvalue.Replace("'", "")
        Dim result As Boolean = False
        Dim req As String = "DELETE FROM " + table + " WHERE " + colname + "='" + sqlFix(colvalue) + "'"
        Dim reqc As OleDbCommand = New OleDbCommand(req, cnn)
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
            Catch ex2 As Exception
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
            Catch ex2 As Exception
            End Try

            result = False
        End Try
        Return result
    End Function
    Public Function SqlSelect(table As String, Optional col As String = "*", Optional filtername As String = Nothing, Optional filtervalue As String = "", Optional filtername2 As String = Nothing, Optional filtervalue2 As String = "", Optional filtername3 As String = Nothing, Optional filtervalue3 As String = "", Optional filtername4 As String = Nothing, Optional filtervalue4 As String = "", Optional filtername5 As String = Nothing, Optional filtervalue5 As String = "")
        Dim results() As Array = Nothing
        Dim req As String = ""
        req = "SELECT " + col + " FROM " + table
        If filtername IsNot Nothing Then
            req += " WHERE " + filtername + "='" + sqlFix(filtervalue) + "'"
            If filtername2 IsNot Nothing Then
                req += " AND " + filtername2 + "='" + sqlFix(filtervalue2) + "'"
                If filtername3 IsNot Nothing Then
                    req += " AND " + filtername3 + "='" + sqlFix(filtervalue3) + "'"
                    If filtername4 IsNot Nothing Then
                        req += " AND " + filtername4 + "='" + sqlFix(filtervalue4) + "'"
                        If filtername5 IsNot Nothing Then
                            req += " AND " + filtername5 + "='" + sqlFix(filtervalue5) + "'"
                        End If
                    End If
                End If
            End If
        End If
        'MsgBox(req)        
        Dim reqc As OleDbCommand = New OleDbCommand(req, CNN)
        Dim res As OleDbDataReader = Nothing
        Try
            res = reqc.ExecuteReader()
            While res.Read()
                Dim result(res.FieldCount)
                For i = 0 To res.FieldCount - 1
                    result(i) = res(i)
                    If IsDBNull(result(i)) Then
                        result(i) = ""
                    End If
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
            Catch ex2 As Exception
            End Try

        End Try
        Return results
    End Function
    Public Function SqlSelectSimple(table As String, Optional col As String = "*", Optional filte As String = " 1=1 ")
        Dim results() As Array = Nothing
        Dim req As String = ""
        req = "SELECT " + col + " FROM " + table & " WHERE " & filte
        'MsgBox(req)        
        Dim reqc As OleDbCommand = New OleDbCommand(req, CNN)
        Dim res As OleDbDataReader = Nothing
        Try
            res = reqc.ExecuteReader()
            While res.Read()
                Dim result(res.FieldCount)
                For i = 0 To res.FieldCount - 1
                    result(i) = res(i)
                    If IsDBNull(result(i)) Then
                        result(i) = ""
                    End If
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
            MsgBox("Erreur de SqlSelectSimple dans la table " + table + " : " + ex.Message)
            Try
                res.Close()
            Catch ex2 As Exception
            End Try

        End Try
        Return results
    End Function
    Public Function sqlERSelect(table As String, col As String, filter As String)
        Dim resul() As String = Nothing
        Dim req As String = "SELECT " & col & " FROM " & table & " WHERE " & filter
        Dim reqc As OleDbCommand = New OleDbCommand(req, CNN)
        Dim reqr As OleDbDataReader = Nothing
        Try
            reqr = reqc.ExecuteReader()
            While reqr.Read()
                If reqr(0) IsNot Nothing Then
                    If resul Is Nothing Then
                        ReDim resul(0)
                        resul(0) = reqr(0)
                        If resul(0) Is Nothing Then
                            resul(0) = ""
                        End If
                    Else
                        ReDim Preserve resul(UBound(resul) + 1)
                        resul(UBound(resul)) = reqr(0)
                        If resul(UBound(resul)) Is Nothing Then
                            resul(UBound(resul)) = ""
                        End If
                    End If
                End If
            End While
            reqr.Close()
        Catch ex As Exception
            MsgBox("Erreur de sqlERSelect pour " & table & "." & col & " : " & ex.Message)
            Try
                reqr.Close()
            Catch ex2 As Exception

            End Try
        End Try
        Return resul
    End Function
    Public Function SqlSelectFilter(table As String, Optional col As String = "*", Optional filter As String = "1=1")
        Dim results() As Array = Nothing
        Dim req As String = ""
        req = "SELECT " + col + " FROM " + table + " WHERE " + filter
        'MsgBox(req)        
        Dim reqc As OleDbCommand = New OleDbCommand(req, CNN)
        Dim res As OleDbDataReader = Nothing
        Try
            res = reqc.ExecuteReader()
            While res.Read()
                Dim result(res.FieldCount)
                For i = 0 To res.FieldCount - 1
                    result(i) = res(i)
                    If result(i) Is Nothing Then
                        result(i) = ""
                    End If
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
            Catch ex2 As Exception

            End Try

        End Try
        Return results
    End Function
    Public Function SqlInsert(table As String, values() As String)
        Dim resul As Boolean
        Dim req As String = "INSERT INTO " + table + " VALUES("
        For i = 0 To UBound(values)
            If values(i) IsNot Nothing Then
                values(i) = values(i).Replace("'", "''")
            End If
            req += "'" + values(i) + "'"
            If i = UBound(values) Then
                req += ")"
            Else
                req += ","
            End If
        Next
        Dim reqc As OleDbCommand = New OleDbCommand(req, CNN)
        Dim res As OleDbDataReader = reqc.ExecuteReader()
        Try
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
    Public Function getDataDetails(table As String, col As String)
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
        Return result
    End Function

    Public Function OneResRequest(req As String)
        Dim res2 As Boolean = False
        Dim result() As String = Nothing
        Dim reqc As OleDbCommand = New OleDbCommand(req, CNN)
        Dim res As OleDbDataReader = Nothing
        Try
            res = reqc.ExecuteReader()
            While res.Read()
                If result IsNot Nothing Then
                    ReDim Preserve result(UBound(result) + 1)
                    result(UBound(result)) = res(0)
                Else
                    ReDim result(0)
                    result(0) = res(0)
                End If
            End While
            res.Close()
            res2 = True
        Catch ex As Exception
            MsgBox("Erreur de OneResRequest " + req + " : " + ex.Message)
            Try
                res.Close()
            Catch ex1 As Exception
            End Try
        End Try
        If (Not LCase(req).Contains("select")) And (LCase(req).Contains("insert") Or LCase(req).Contains("delete") Or LCase(req).Contains("update")) Then
            Return res2
        End If
        Return result
    End Function
End Class
