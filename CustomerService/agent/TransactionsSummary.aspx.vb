Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports System.Runtime.CompilerServices

Public Class TransactionsSummary
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Protected Sub DownloadReport_Click(sender As Object, e As EventArgs) Handles Report.Click

        Dim getStartDate As String = Nothing
        Dim getEndDate As String = Nothing
        Dim getselectedMno As String = Nothing
        Dim getselectedType As String = Nothing
        Dim getProcToRun As String = Nothing

        Try
            Dim resultDataTable As DataTable = New DataTable()
            Dim selectedMno As String = searchmnos.SelectedItem.Text
            Dim selectedType As String = searchType.SelectedItem.Text
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

            If selectedMno = "" Then
                getselectedMno = Nothing
            Else
                getselectedMno = selectedMno
            End If

            If selectedType = "" Then
                getselectedType = Nothing
            Else
                getselectedType = selectedType
            End If

            If selectedMno = "Summary" Then

                getProcToRun = "customers.Momo_getSummary"
                resultDataTable = RunSummaryReport(getProcToRun, getStartDate, getEndDate)
            Else
                getProcToRun = "customers.GetByMNO"
                resultDataTable = RunMNOReport(getProcToRun, getStartDate, getEndDate, selectedMno, selectedType)
            End If
            If resultDataTable.Rows.Count > 0 Then
                Dim dt As String = DateTime.Now.ToString("yyyMMddhhmm")
                savedatatable(resultDataTable, $"TransactionSummary_{dt}")
            Else
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Search Filter not correct');</script>", False)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.Message & " ');</script>", False)
            'lblerroratmsg.Text = ex.Message
        Finally
            'con_get.Dispose()
        End Try
    End Sub
    Protected Sub getMNO_Click(sender As Object, e As EventArgs) Handles getMNO.Click
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con_get As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim cmd_getbycnum As New MySql.Data.MySqlClient.MySqlCommand
            Dim getStartDate As String = Nothing
            Dim getEndDate As String = Nothing
            Dim getselectedMno As String = Nothing
            Dim getselectedType As String = Nothing
            Dim getProcToRun As String = Nothing

            Try
                con_get.Open()
                Dim selectedMno As String = searchmnos.SelectedItem.Text
                Dim selectedType As String = searchType.SelectedItem.Text
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

                If selectedMno = "" Then
                    getselectedMno = Nothing
                Else
                    getselectedMno = selectedMno
                End If

                If selectedType = "" Then
                    getselectedType = Nothing
                Else
                    getselectedType = selectedType
                End If

                cmd_getbycnum.Connection = con_get
                If selectedMno = "Summary" Then

                    getProcToRun = "customers.Momo_getSummary"
                    Dim resultDataTable As DataTable = RunSummaryReport(getProcToRun, getStartDate, getEndDate)
                    Using kda As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnum)
                        'kda.Fill(resultDataTable)
                        If resultDataTable.Rows.Count = 0 Then
                            lblerroratmsg.Text = "No Records to display"
                        Else
                            Dim returnrecods As String = ConvertDataTableToHtml2(resultDataTable)
                            PlaceHolder1.Controls.Add(New Literal() With {
                                  .Text = returnrecods.ToString()
                                  })
                            lblerroratmsg.Text = resultDataTable.Rows.Count.ToString() + " Records Returned"

                        End If
                    End Using
                Else
                    getProcToRun = "customers.GetByMNO"
                    Dim resultDataTable As DataTable = RunMNOReport(getProcToRun, getStartDate, getEndDate, selectedMno, selectedType)
                    Using kda2 As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnum)
                        If resultDataTable.Rows.Count = 0 Then
                            lblerroratmsg.Text = "No Records to display"
                        Else
                            Dim returnrecodsfs As String = ConvertDataTableToHtml2(resultDataTable)
                            PlaceHolder1.Controls.Add(New Literal() With {.Text = returnrecodsfs.ToString()})
                            lblerroratmsg.Text = resultDataTable.Rows.Count.ToString() + " Records Returned"
                        End If
                    End Using
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

    Private Function RunMNOReport(stProcedure As String, stDate As String, edDate As String, mno As String, status As String) As DataTable
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
                command.Parameters.AddWithValue("selectedMno", mno)
                command.Parameters.AddWithValue("selectedType", status)
                Using reader As MySqlDataReader = command.ExecuteReader()
                    resultDataTable.Load(reader)
                End Using
                connection.Close()
                'connection.Dispose()
            End Using
        End Using
        Return resultDataTable
    End Function

    Private Function RunSummaryReport(stProcedure As String, stDate As String, edDate As String) As DataTable
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
    Private Sub DownloadDataTableAsCSV(dataTable As DataTable, fileName As String)
        ' Create a StringBuilder to build the CSV content
        Dim csvContent As New StringBuilder()

        ' Create the header row
        For Each column As DataColumn In dataTable.Columns
            csvContent.Append($"{column.ColumnName},")
        Next
        csvContent.AppendLine()

        ' Create the data rows
        For Each row As DataRow In dataTable.Rows
            For Each value As Object In row.ItemArray
                csvContent.Append($"{value},")
            Next
            csvContent.AppendLine()
        Next

        ' Save the CSV content to a file in the My Documents folder
        Dim documentsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim filePath As String = $"{documentsPath}\{fileName}.csv"
        System.IO.File.WriteAllText(filePath, csvContent.ToString())

        ' Display a message or perform further actions as needed
        MessageBox.Show($"CSV file '{fileName}.csv' has been created in My Documents.")
    End Sub

    Protected Function savedatatable(ByVal dt As DataTable, fileName As String) As String
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
        Response.AddHeader("content-disposition", $"attachment;filename={fileName}.csv")
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