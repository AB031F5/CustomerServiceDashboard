Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports System.Xml
Imports System.Web.UI.HtmlControls
Imports MySql.Data.MySqlClient
Imports CsvHelper
Imports System.Globalization

Public Class Upload
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Shared Function ConvertDataTableToHTML2(ByVal dt As DataTable) As String
        Dim html As String = "<table id='table_id' class='display' cellpadding='5' cellspacing='0' style='border: 1px solid #ccc;font-size: 9pt;font-family:Calibri; width:100%'>"
        html += "<thead>"
        html += "<tr>"

        For i As Integer = 0 To dt.Columns.Count - 1
            html += "<th class='tablesorter' style='background-color: #B8DBFD;border: 1px solid #ccc'>" & dt.Columns(i).ColumnName.ToUpper() & "</th>"
        Next

        html += "</tr>"
        html += "</thead>"
        html += "<tbody>"
        For i As Integer = 0 To dt.Rows.Count - 1
            html += "<tr>"

            For j As Integer = 0 To dt.Columns.Count - 1
                html += "<td style='width:100px;border: 1px solid #ccc'>" & dt.Rows(i)(j).ToString().ToUpper() & "</td>"
            Next

            html += "</tr>"
        Next
        html += "</tbody>"
        html += "</table>"
        Return html

    End Function

    Private Sub processup_Click(sender As Object, e As EventArgs) Handles processup.Click
        If fupload.HasFile Then
            Using reader = New StreamReader(fupload.FileContent)
                Using csv = New CsvReader(reader, CultureInfo.InvariantCulture)
                    Dim records = csv.GetRecords(Of Foo)()
                    Dim kout As DataTable = ToDataTable(records)
                    '  Dim returntable As String = ConvertDataTableToHTML(kout)
                    '  PlaceHolder1.Controls.Add(New Literal() With {
                    '.Text = returntable.ToString()
                    '})
                    Dim sCommand As StringBuilder = New StringBuilder("insert into preprocess (case_number, customer_or_prospect_name, Account_no_number, creation_datetime, status) values ")
                    Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
                    Using con_insert As New MySql.Data.MySqlClient.MySqlConnection(constr)
                        Dim cmd_add_comms, cmd_check_unprocessed As New MySql.Data.MySqlClient.MySqlCommand
                        Try
                            Dim Rowz As List(Of String) = New List(Of String)()
                            Dim i As Integer
                            For i = 0 To kout.Rows.Count - 1
                                Rowz.Add(String.Format("('{0}','{1}','{2}','{3}','{4}')", MySqlHelper.EscapeString(kout.Rows(i)(0)), MySqlHelper.EscapeString(kout.Rows(i)(1)), MySqlHelper.EscapeString(kout.Rows(i)(2)), MySqlHelper.EscapeString(kout.Rows(i)(3)), MySqlHelper.EscapeString(kout.Rows(i)(4))))
                            Next
                            sCommand.Append(String.Join(",", Rowz))
                            sCommand.Append(";")


                            cmd_add_comms.Connection = con_insert
                            cmd_add_comms.CommandText = sCommand.ToString
                            cmd_add_comms.ExecuteNonQuery()
                            con_insert.Close()
                            Dim messagedisplay As String = String.Format("file with {0} records has been succesfully uploaded", kout.Rows.Count.ToString())
                            lblerror.Text = messagedisplay
                            fileuploadhist(fupload.FileName, Session("User_Name"), kout.Rows.Count)
                        Catch ex As Exception
                            lblerror.Text = ex.Message
                        End Try
                    End Using
                End Using
            End Using
        Else
            lblerror.Text = "No file to upload"
        End If
    End Sub
    'Private Sub processup_Click(sender As Object, e As EventArgs) Handles processup.Click
    '    If fupload.HasFile Then
    '        Using reader = New StreamReader(fupload.FileContent)
    '            Using csv = New CsvReader(reader, CultureInfo.InvariantCulture)
    '                Dim records = csv.GetRecords(Of Foo)()
    '                Dim kout As DataTable = ToDataTable(records)
    '                '  Dim returntable As String = ConvertDataTableToHTML(kout)
    '                '  PlaceHolder1.Controls.Add(New Literal() With {
    '                '.Text = returntable.ToString()
    '                '})
    '                Dim sCommand As StringBuilder = New StringBuilder("insert into preprocess (case_number, customer_or_prospect_name, Account_no_number, creation_datetime, status) values ")
    '                Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    '                Using con_insert As New MySql.Data.MySqlClient.MySqlConnection(constr)
    '                    Dim cmd_add_comms, cmd_check_unprocessed As New MySql.Data.MySqlClient.MySqlCommand
    '                    Try
    '                        con_insert.Open()
    '                        cmd_check_unprocessed.CommandText = "select count(*) from preprocess"
    '                        cmd_check_unprocessed.Connection = con_insert
    '                        Dim numberofrecords = CInt(cmd_check_unprocessed.ExecuteScalar)
    '                        If numberofrecords > 0 Then
    '                            lblerror.Text = "Last file uploaded is not done processing. This file can not be uploaded"
    '                            con_insert.Close()
    '                            Exit Sub
    '                        Else
    '                            Dim Rowz As List(Of String) = New List(Of String)()
    '                            Dim i As Integer
    '                            For i = 0 To kout.Rows.Count - 1
    '                                Rowz.Add(String.Format("('{0}','{1}','{2}','{3}','{4}')", MySqlHelper.EscapeString(kout.Rows(i)(0)), MySqlHelper.EscapeString(kout.Rows(i)(1)), MySqlHelper.EscapeString(kout.Rows(i)(2)), MySqlHelper.EscapeString(kout.Rows(i)(3)), MySqlHelper.EscapeString(kout.Rows(i)(4))))
    '                            Next
    '                            sCommand.Append(String.Join(",", Rowz))
    '                            sCommand.Append(";")


    '                            cmd_add_comms.Connection = con_insert
    '                            cmd_add_comms.CommandText = sCommand.ToString
    '                            cmd_add_comms.ExecuteNonQuery()
    '                            con_insert.Close()
    '                            Dim messagedisplay As String = String.Format("file with {0} records has been succesfully uploaded", kout.Rows.Count.ToString())
    '                            lblerror.Text = messagedisplay
    '                            fileuploadhist(fupload.FileName, Session("User_Name"), kout.Rows.Count)
    '                        End If
    '                    Catch ex As Exception
    '                        lblerror.Text = ex.Message
    '                    End Try
    '                End Using
    '            End Using
    '        End Using
    '    Else
    '        lblerror.Text = "No file to upload"
    '    End If
    'End Sub

    Protected Sub refresh_Click(sender As Object, e As EventArgs)
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con_getfs As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim cmd_getbycnumfs As New MySql.Data.MySqlClient.MySqlCommand
            Try
                con_getfs.Open()
                cmd_getbycnumfs.Connection = con_getfs
                If errortypelist.SelectedItem.Text = "Closed" Then
                    cmd_getbycnumfs.CommandText = "_GetClosed_Preprocess"
                ElseIf errortypelist.SelectedItem.Text = "WIP" Then
                    cmd_getbycnumfs.CommandText = "_GetWIP_Preprocess"
                ElseIf errortypelist.SelectedItem.Text = "New" Then
                    cmd_getbycnumfs.CommandText = "_GetNew_Preprocess"
                ElseIf errortypelist.SelectedItem.Text = "All Records" Then
                    cmd_getbycnumfs.CommandText = "_GetPreProcess"
                ElseIf errortypelist.SelectedItem.Text = "Processed" Then
                    cmd_getbycnumfs.CommandText = "_GetProcessed_Preprocess"
                ElseIf errortypelist.SelectedItem.Text = "Unprocessed" Then
                    cmd_getbycnumfs.CommandText = "_GetUnProcessed_Preprocess"
                End If
                cmd_getbycnumfs.CommandType = CommandType.StoredProcedure
                Using kdafs As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnumfs)
                    Dim getmsgtablefs As DataTable = New DataTable()
                    kdafs.Fill(getmsgtablefs)
                    If getmsgtablefs.Rows.Count = 0 Then
                        lblerror.Text = "No Records to display"
                    Else
                        Dim returnrecodsfs As String = ConvertDataTableToHTML(getmsgtablefs)
                        PlaceHolder1.Controls.Add(New Literal() With {
                          .Text = returnrecodsfs.ToString()
                          })
                        lblerror.Text = getmsgtablefs.Rows.Count.ToString() + " Records Returned"
                    End If
                End Using
                con_getfs.Close()
            Catch ex As Exception
                lblerror.Text = ex.Message
            Finally
                con_getfs.Dispose()
            End Try
        End Using
    End Sub

    Public Shared Function ConvertDataTableToHTML(ByVal dt As DataTable) As String
        Dim html As String = "<table id='table_id' class='display' cellpadding='5' cellspacing='0' style='border: 1px solid #ccc;font-size: 9pt;font-family:Calibri; width:100%'>"
        html += "<thead>"
        html += "<tr>"

        For i As Integer = 0 To dt.Columns.Count - 1
            html += "<th class='tablesorter' style='background-color:#BB2647; color: white;border: 1px solid #ccc'>" & dt.Columns(i).ColumnName & "</th>"
        Next

        html += "</tr>"
        html += "</thead>"
        html += "<tbody>"
        For i As Integer = 0 To dt.Rows.Count - 1
            html += "<tr>"

            For j As Integer = 0 To dt.Columns.Count - 1
                html += "<td style='width:100px;border: 1px solid #ccc'>" & dt.Rows(i)(j).ToString().ToUpper() & "</td>"
            Next

            html += "</tr>"
        Next
        html += "</tbody>"
        html += "</table>"
        Return html

    End Function

    Public Class Foo
        Public Property f1 As String
        Public Property f2 As String
        Public Property f3 As String
        Public Property f4 As String
        Public Property f5 As String
    End Class

    Public Shared Function ToDataTable(Of T)(ByVal self As IEnumerable(Of T)) As DataTable
        Dim properties = GetType(T).GetProperties()
        Dim dataTable = New DataTable()

        For Each info In properties
            dataTable.Columns.Add(info.Name, If(Nullable.GetUnderlyingType(info.PropertyType), info.PropertyType))
        Next

        For Each entity In self
            dataTable.Rows.Add(properties.[Select](Function(p) p.GetValue(entity)).ToArray())
        Next

        Return dataTable
    End Function

    Public Sub fileuploadhist(fnomer As String, uploader As String, filecount As Integer)
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con_ffiiles As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim cmd_updatefiles As New MySql.Data.MySqlClient.MySqlCommand
            Try
                con_ffiiles.Open()
                cmd_updatefiles.CommandText = "fileuploadhistory"
                cmd_updatefiles.CommandType = CommandType.StoredProcedure
                cmd_updatefiles.Connection = con_ffiiles
                cmd_updatefiles.Parameters.AddWithValue("fname", fnomer)
                cmd_updatefiles.Parameters.AddWithValue("fuploaddate", DateTime.Now)
                cmd_updatefiles.Parameters.AddWithValue("uplaoduser", uploader)
                cmd_updatefiles.Parameters.AddWithValue("uploadnumber", filecount)
                cmd_updatefiles.ExecuteNonQuery()
                con_ffiiles.Close()
            Catch ex As Exception
                lblerror.Text = ex.Message
            Finally
                con_ffiiles.Dispose()
            End Try
        End Using
    End Sub

End Class