Imports System.Windows.Forms
Imports MySql.Data.MySqlClient

Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub DownloadReport_Click(sender As Object, e As EventArgs) Handles Report.Click

        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con_get As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim cmd_getbycnum As New MySql.Data.MySqlClient.MySqlCommand
            Dim getStartDate As String = Nothing
            Dim getEndDate As String = Nothing
            Dim getProcToRun As String = Nothing

            Try
                con_get.Open()
                Dim searchType As String = searchmsgs.SelectedItem.Text
                Dim search As String = searchstringone.Text
                Dim startSearch As String = startddate.Text
                Dim endSearch As String = enddate.Text

                If startSearch = "" Then
                    getStartDate = Nothing
                Else
                    getStartDate = startSearch
                End If

                If endSearch = "" Then
                    getEndDate = Nothing
                Else
                    getEndDate = endSearch
                End If
                cmd_getbycnum.Connection = con_get

                If searchType = "Request Reference" Then
                    getProcToRun = "customers.NewGetReportMessages"
                    Dim resultDataTable As DataTable = RunMessagesReport(getProcToRun, getStartDate, getEndDate, search, Nothing, Nothing)
                    Using kda As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnum)
                        If resultDataTable.Rows.Count = 0 Then
                            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No Records to download');</script>", False)
                        Else
                            savedatatable(resultDataTable)
                        End If
                    End Using
                ElseIf searchType = "Phone Number" Then
                    getProcToRun = "customers.NewGetReportMessages"
                    Dim resultDataTable As DataTable = RunMessagesReport(getProcToRun, getStartDate, getEndDate, Nothing, search, Nothing)
                    Using kda As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnum)
                        'kda.Fill(resultDataTable)
                        If resultDataTable.Rows.Count = 0 Then
                            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No Records to download');</script>", False)
                        Else
                            savedatatable(resultDataTable)
                        End If
                    End Using
                ElseIf searchType = "Date Range" Then
                    getProcToRun = "customers.NewGetReportMessages"
                    Dim resultDataTable As DataTable = RunMessagesReport(getProcToRun, getStartDate, getEndDate, Nothing, Nothing, search)
                    Using kda As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnum)
                        'kda.Fill(resultDataTable)
                        If resultDataTable.Rows.Count = 0 Then
                            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No Records to download');</script>", False)
                        Else
                            savedatatable(resultDataTable)
                        End If
                    End Using
                Else

                End If
                con_get.Close()
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.Message & " ');</script>", False)
                'lblerroratmsg.Text = ex.Message
            Finally
                con_get.Dispose()
            End Try
        End Using
    End Sub
    Private Function RunMessagesReport(stProcedure As String, stDate As String, edDate As String, refId As String, accNumber As String, byDate As String) As DataTable
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Dim storedProcedureName As String = stProcedure
        Dim resultDataTable As New DataTable()
        Using connection As New MySqlConnection(connectionString)
            connection.Open()
            Using command As New MySqlCommand()
                command.CommandType = CommandType.StoredProcedure
                command.CommandText = storedProcedureName
                command.Connection = connection
                command.Parameters.AddWithValue("StartDate", stDate)
                command.Parameters.AddWithValue("EndDate", edDate)
                command.Parameters.AddWithValue("recid", refId)
                command.Parameters.AddWithValue("ccustact", accNumber)
                command.Parameters.AddWithValue("byDate", byDate)
                Using reader As MySqlDataReader = command.ExecuteReader()
                    resultDataTable.Load(reader)
                End Using
                connection.Close()
                'connection.Dispose()
            End Using
        End Using
        Return resultDataTable
    End Function
    Protected Sub getlist_Click(sender As Object, e As EventArgs) Handles getlist.Click
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con_get As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim cmd_getbycnum As New MySql.Data.MySqlClient.MySqlCommand
            Dim getStartDate As String = Nothing
            Dim getEndDate As String = Nothing
            Dim getProcToRun As String = Nothing
            Try
                con_get.Open()
                Dim searchType As String = searchmsgs.SelectedItem.Text
                Dim search As String = searchstringone.Text
                Dim startSearch As String = startddate.Text
                Dim endSearch As String = enddate.Text

                If startSearch = "" Then
                    getStartDate = Nothing
                Else
                    getStartDate = startSearch
                End If

                If endSearch = "" Then
                    getEndDate = Nothing
                Else
                    getEndDate = endSearch
                End If
                cmd_getbycnum.Connection = con_get
                If searchType = "Request Reference" Then
                    getProcToRun = "customers.NewGetReportMessages"
                    Dim resultDataTable As DataTable = RunMessagesReport(getProcToRun, getStartDate, getEndDate, search, Nothing, Nothing)
                    Using kda As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnum)
                        If resultDataTable.Rows.Count = 0 Then
                            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No Records to download');</script>", False)
                        Else
                            Dim returnrecods As String = ConvertDataTableToHTML2(resultDataTable)
                            PlaceHolder1.Controls.Add(New Literal() With {
                                  .Text = returnrecods.ToString()
                                  })
                        End If
                    End Using
                ElseIf searchType = "Phone Number" Then
                    getProcToRun = "customers.NewGetReportMessages"
                    Dim resultDataTable As DataTable = RunMessagesReport(getProcToRun, getStartDate, getEndDate, Nothing, search, Nothing)
                    Using kda As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnum)
                        'kda.Fill(resultDataTable)
                        If resultDataTable.Rows.Count = 0 Then
                            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No Records to download');</script>", False)
                        Else
                            Dim returnrecods As String = ConvertDataTableToHTML2(resultDataTable)
                            PlaceHolder1.Controls.Add(New Literal() With {
                                  .Text = returnrecods.ToString()
                                  })
                        End If
                    End Using
                ElseIf searchType = "Date Range" Then
                    getProcToRun = "customers.NewGetReportMessages"
                    Dim resultDataTable As DataTable = RunMessagesReport(getProcToRun, getStartDate, getEndDate, Nothing, Nothing, search)
                    Using kda As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnum)
                        'kda.Fill(resultDataTable)
                        If resultDataTable.Rows.Count = 0 Then
                            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No Records to download');</script>", False)
                        Else
                            Dim returnrecods As String = ConvertDataTableToHTML2(resultDataTable)
                            PlaceHolder1.Controls.Add(New Literal() With {
                                  .Text = returnrecods.ToString()
                                  })
                        End If
                    End Using
                Else

                End If
                con_get.Close()
            Catch ex As Exception
                lblerroratmsg.Text = ex.Message
            Finally
                con_get.Dispose()
            End Try
        End Using
    End Sub

    Private Sub searchmsgs_TextChanged(sender As Object, e As EventArgs) Handles searchmsgs.TextChanged
        If searchmsgs.SelectedItem.Text = "Date Range" Then
            searchstringone.Text = "Date Range"
        Else
            searchstringone.Text = ""
        End If
    End Sub

    Private Function ConvertDataTableToHtml2(dataTable As DataTable) As String
        Dim htmlStringBuilder As New StringBuilder()

        htmlStringBuilder.AppendLine("<table id='table_id2' class='display cell-border hover stripe' style='font-size: 11pt;font-family:Roboto; width:100%;border-radius:5px;overflow:hidden;'>")

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
        Dim fileName As String = DateTime.Now.ToString("yyyMMddhhmm")
        Response.AddHeader("content-disposition", $"attachment;filename=Messages_{fileName}.csv")
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