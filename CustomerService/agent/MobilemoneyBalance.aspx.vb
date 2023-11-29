Public Class MobilemoneyBalance
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con_get As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim cmd_getbycnum As New MySql.Data.MySqlClient.MySqlCommand
            Try

                'Dim User_Name As String = Session("User_Name").ToString()

                'Dim ses As String = System.Web.HttpContext.Current.Session(“MyVariable”).ToString()
                con_get.Open()

                cmd_getbycnum.Connection = con_get
                cmd_getbycnum.CommandText = "mobilemoney.Balance_check"
                cmd_getbycnum.CommandType = CommandType.StoredProcedure
                'cmd_getbycnum.Parameters.AddWithValue("mno", DDLMNO.SelectedItem.ToString())
                Using kda As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnum)
                    Dim getmsgtable As DataTable = New DataTable()
                    kda.Fill(getmsgtable)
                    If getmsgtable.Rows.Count = 0 Then
                        lblerroratmsg1.Text = "No Records to display"
                    Else
                        Dim returnrecods As String = ConvertDataTableToHTML12(getmsgtable)
                        PlaceHolder12.Controls.Add(New Literal() With {
                          .Text = returnrecods.ToString()
                          })
                        'lblerroratmsg1.Text = getmsgtable.Rows.Count.ToString() + " Records Returned"
                    End If
                End Using

                con_get.Close()
            Catch ex As Exception
                lblerroratmsg1.Text = ex.Message
            Finally
                con_get.Dispose()
            End Try
        End Using

    End Sub



    'Protected Sub getbal_Click(sender As Object, e As EventArgs) Handles getbal.Click
    '    Dim constr As String = ConfigurationManager.ConnectionStrings("connstring").ConnectionString
    '    Using con_get As New MySql.Data.MySqlClient.MySqlConnection(constr)
    '        Dim cmd_getbycnum As New MySql.Data.MySqlClient.MySqlCommand
    '        Try
    '            con_get.Open()

    '            cmd_getbycnum.Connection = con_get
    '            cmd_getbycnum.CommandText = "mobilemoney.Balance_check"
    '            cmd_getbycnum.CommandType = CommandType.StoredProcedure
    '            'cmd_getbycnum.Parameters.AddWithValue("mno", DDLMNO.SelectedItem.ToString())
    '            Using kda As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnum)
    '                Dim getmsgtable As DataTable = New DataTable()
    '                kda.Fill(getmsgtable)
    '                If getmsgtable.Rows.Count = 0 Then
    '                    lblerroratmsg1.Text = "No Records to display"
    '                Else
    '                    Dim returnrecods As String = ConvertDataTableToHTML12(getmsgtable)
    '                    PlaceHolder12.Controls.Add(New Literal() With {
    '                      .Text = returnrecods.ToString()
    '                      })
    '                    lblerroratmsg1.Text = getmsgtable.Rows.Count.ToString() + " Records Returned"
    '                End If
    '            End Using

    '            con_get.Close()
    '        Catch ex As Exception
    '            lblerroratmsg1.Text = ex.Message
    '        Finally
    '            con_get.Dispose()
    '        End Try
    '    End Using
    'End Sub

    Private Function ConvertDataTableToHTML12(dataTable As DataTable) As String
        Dim htmlStringBuilder As New StringBuilder()

        htmlStringBuilder.AppendLine("<table id='table_id2' class='display cell-border hover stripe' style='font-size: 11pt;font-family:Roboto; width:100%;border-radius:3px;overflow:hidden;'>")

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
    Protected Function savedatatable(ByVal dt As DataTable) As String
        'Add the Header row for CSV file.
        Dim csv As String = String.Empty

        For i As Integer = 0 To dt.Columns.Count - 1
            csv += dt.Columns(i).ColumnName & ","c
        Next

        'Add new line.
        csv += vbCr & vbLf
        'Adding the Rows
        For i As Integer = 0 To dt.Rows.Count - 1


            For j As Integer = 0 To dt.Columns.Count - 1
                csv += dt.Rows(i)(j).ToString().Replace(",", ";") & ","c
            Next
            'Add new line.
            csv += vbCr & vbLf
        Next


        'Exporting to Excel
        'Download the CSV file.
        Response.Clear()
        'Response.ContentType = "text/csv"
        'Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=SqlExport.csv")
        'Response.Charset = ""
        'Response.OutputStream(csv)
        Response.Charset = ""
        Response.ContentType = "application/text"
        Response.Write(csv.ToString())
        'Response.TransmitFile(Server.MapPath("~/docs/SqlExport.csv"))
        Response.Flush()
        Response.End()

        Return True

    End Function


End Class