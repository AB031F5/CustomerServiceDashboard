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

    Private Function ConvertDataTableToHTML3(dataTable As DataTable) As String
        Dim htmlStringBuilder As New StringBuilder()

        htmlStringBuilder.AppendLine("<table id='table_id3' class='display cell-border hover stripe' style='font-size: 11pt;font-family:Roboto; width:100%;border-radius:5px;overflow:hidden;'>")

        htmlStringBuilder.AppendLine("<thead>")
        htmlStringBuilder.AppendLine("<tr>")
        For Each column As DataColumn In dataTable.Columns
            htmlStringBuilder.AppendFormat("<th  class='tablesorter' style='background-color:#BB2647; color: white;border: 0.5px solid #f0f0f0'>{0}</th>", column.ColumnName)
        Next
        htmlStringBuilder.AppendLine("</tr>")
        htmlStringBuilder.AppendLine("</thead>")
        For Each row As DataRow In dataTable.Rows
            htmlStringBuilder.AppendLine("<tr>")
            For Each value As Object In row.ItemArray
                htmlStringBuilder.AppendFormat("<td>{0}</td>", value)
            Next
            htmlStringBuilder.AppendLine("</tr>")
        Next

        htmlStringBuilder.AppendLine("</table>")

        Dim htmlString As String = htmlStringBuilder.ToString()
        Return htmlString
    End Function
End Class