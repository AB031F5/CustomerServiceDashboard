Public Class Errors
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub getfails_Click(sender As Object, e As EventArgs)
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con_getfs As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim cmd_getbycnumfs As New MySql.Data.MySqlClient.MySqlCommand
            Try
                con_getfs.Open()
                cmd_getbycnumfs.Connection = con_getfs
                If errortypelist.SelectedItem.Text = "Failed Records" Then
                    cmd_getbycnumfs.CommandText = "getallfailures"
                ElseIf errortypelist.SelectedItem.Text = "Upload History" Then
                    cmd_getbycnumfs.CommandText = "getalluploadedfiles"
                ElseIf errortypelist.SelectedItem.Text = "Invalid Contacts" Then
                    cmd_getbycnumfs.CommandText = "getallinvalidcontacts"
                ElseIf errortypelist.SelectedItem.Text = "Invalid Accounts" Then
                    cmd_getbycnumfs.CommandText = "getallinvalidcalls"
                End If
                cmd_getbycnumfs.CommandType = CommandType.StoredProcedure
                Using kdafs As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnumfs)
                    Dim getmsgtablefs As DataTable = New DataTable()
                    kdafs.Fill(getmsgtablefs)
                    If getmsgtablefs.Rows.Count = 0 Then
                        lblerrorfails.Text = "No Records to display"
                    Else
                        Dim returnrecodsfs As String = ConvertDataTableToHTML3(getmsgtablefs)
                        PlaceHolder1.Controls.Add(New Literal() With {
                          .Text = returnrecodsfs.ToString()
                          })
                        lblerrorfails.Text = getmsgtablefs.Rows.Count.ToString() + " Records Returned"
                    End If
                End Using
                con_getfs.Close()
            Catch ex As Exception
                lblerrorfails.Text = ex.Message
            Finally
                con_getfs.Dispose()
            End Try
        End Using
    End Sub

    Public Shared Function ConvertDataTableToHTML3(ByVal dt As DataTable) As String
        Dim html As String = "<table id='table_id3' class='display' cellpadding='5' cellspacing='0' style='border: 1px solid #ccc;font-size: 9pt;font-family:Calibri; width:100%'>"
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
                html += "<td style='width:100px;border: 1px solid #ccc'>" & dt.Rows(i)(j).ToString() & "</td>"
            Next

            html += "</tr>"
        Next
        html += "</tbody>"
        html += "</table>"
        Return html

    End Function
End Class