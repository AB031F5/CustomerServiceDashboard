Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports System.Xml
Imports System.Web.UI.HtmlControls
Imports MySql.Data.MySqlClient

Public Class SendMessages
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub searchmsgs_TextChanged(sender As Object, e As EventArgs) Handles searchmsgs.TextChanged
        If searchmsgs.SelectedItem.Text = "Date Range" Then
            searchstringone.Text = "Date Range"
        Else
            searchstringone.Text = ""
        End If
    End Sub


    Private Function ConvertDataTableToHTML2xxxSM(dataTable As DataTable) As String
        Dim htmlStringBuilder As New StringBuilder()

        htmlStringBuilder.AppendLine("<table id='table_id2SM' class='display cell-border hover stripe' style='font-size: 11pt;font-family:Roboto; width:100%;border-radius:3px;overflow:hidden;'>")

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



    Protected Sub getlist_Click1(sender As Object, e As EventArgs) Handles getlist.Click
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con_get As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim cmd_getbycnum As New MySql.Data.MySqlClient.MySqlCommand
            Try
                con_get.Open()
                If searchmsgs.SelectedItem.Text = "Case Number" Then
                    cmd_getbycnum.Connection = con_get
                    cmd_getbycnum.CommandText = "getcomsbycnumbermaint"
                    cmd_getbycnum.CommandType = CommandType.StoredProcedure
                    cmd_getbycnum.Parameters.AddWithValue("cnumber", searchstringone.Text)
                    Using kda As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnum)
                        Dim getmsgtable As DataTable = New DataTable()
                        kda.Fill(getmsgtable)
                        If getmsgtable.Rows.Count = 0 Then
                            lblerroratmsg.Text = "No Records to display"
                        Else
                            Dim returnrecods As String = ConvertDataTableToHTML2xxxSM(getmsgtable)
                            PlaceHolder1.Controls.Add(New Literal() With {
                              .Text = returnrecods.ToString()
                              })
                            lblerroratmsg.Text = getmsgtable.Rows.Count.ToString() + " Records Returned"
                        End If
                    End Using
                ElseIf searchmsgs.SelectedItem.Text = "Account Number" Then
                    cmd_getbycnum.Connection = con_get
                    cmd_getbycnum.CommandText = "getcomsbyacctmaint"
                    cmd_getbycnum.CommandType = CommandType.StoredProcedure
                    cmd_getbycnum.Parameters.AddWithValue("ccustact", searchstringone.Text)
                    Using kda2 As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnum)
                        Dim getmsgtable2 As DataTable = New DataTable()
                        kda2.Fill(getmsgtable2)
                        If getmsgtable2.Rows.Count = 0 Then
                            lblerroratmsg.Text = "No Records to display"
                        Else
                            Dim returnrecods2 As String
                            returnrecods2 = ConvertDataTableToHTML2xxxSM(getmsgtable2)
                            PlaceHolder1.Controls.Add(New Literal() With {
                              .Text = returnrecods2.ToString()
                              })
                            lblerroratmsg.Text = getmsgtable2.Rows.Count.ToString() + " Records Returned"
                        End If
                    End Using
                ElseIf searchmsgs.SelectedItem.Text = "Date Range" Then
                    cmd_getbycnum.Connection = con_get
                    cmd_getbycnum.CommandText = "getcomsbydaterangemaint"
                    cmd_getbycnum.CommandType = CommandType.StoredProcedure
                    cmd_getbycnum.Parameters.AddWithValue("sdate", startddate.Text)
                    cmd_getbycnum.Parameters.AddWithValue("edate", enddate.Text)
                    Using kda2 As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnum)
                        Dim getmsgtable2 As DataTable = New DataTable()
                        kda2.Fill(getmsgtable2)
                        If getmsgtable2.Rows.Count = 0 Then
                            lblerroratmsg.Text = "No Records to display"
                        Else
                            Dim returnrecods2 As String = ConvertDataTableToHTML2xxxSM(getmsgtable2)
                            PlaceHolder1.Controls.Add(New Literal() With {
                              .Text = returnrecods2.ToString()
                              })
                            lblerroratmsg.Text = getmsgtable2.Rows.Count.ToString() + " Records Returned"
                        End If
                    End Using
                End If
                con_get.Close()
            Catch ex As Exception
                lblerroratmsg.Text = ex.Message
            Finally
                con_get.Dispose()
            End Try
        End Using
    End Sub

    Private Function RunReport(stProcedure As String, stDate As String, edDate As String, mno As String, status As String) As DataTable
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
                command.Parameters.AddWithValue("caseNumber", mno)
                command.Parameters.AddWithValue("accNumber", status)
                Using reader As MySqlDataReader = command.ExecuteReader()
                    resultDataTable.Load(reader)
                End Using
                connection.Close()
                'connection.Dispose()
            End Using
        End Using
        Return resultDataTable
    End Function
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