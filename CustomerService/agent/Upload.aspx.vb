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
Imports Org.BouncyCastle.Asn1
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar

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
        Dim errors As ArrayList = New ArrayList()

        If fupload.HasFile Then
            errors.Add($"File Present as {fupload.FileName}")
            Dim uploadHistory As DataTable = New DataTable()
            errors.Add($"Checking Upload History")
            uploadHistory = RunUploadHistory(fupload.FileName)
            If uploadHistory.Rows.Count > 0 Then
                errors.Add($"File Exists")
                Dim dateInserted As String = uploadHistory.Rows(0)("DatetimeInserted").ToString()
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('A file with the same name already exists, as at " & dateInserted & " ');</script>", False)
            Else
                Using reader = New StreamReader(fupload.FileContent)
                    Using csv = New CsvReader(reader, CultureInfo.InvariantCulture)
                        Dim records = csv.GetRecords(Of Foo)()
                        Dim FileRecords As DataTable = ToDataTable(records)
                        '  Dim returntable As String = ConvertDataTableToHTML(kout)
                        '  PlaceHolder1.Controls.Add(New Literal() With {
                        '.Text = returntable.ToString()
                        '})
                        Dim sCommand As StringBuilder = New StringBuilder("insert into preprocess (case_number, customer_or_prospect_name, Account_no_number, creation_datetime, status, ThreadNumber) values ")
                        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
                        Using con_insert As New MySql.Data.MySqlClient.MySqlConnection(constr)
                            Dim cmd_add_comms, cmd_check_unprocessed As New MySql.Data.MySqlClient.MySqlCommand
                            Try
                                Dim RecordsAbleToUpload As List(Of String) = New List(Of String)()
                                errors.Add($"Getting Rows")

                                RecordsAbleToUpload = GetRows(FileRecords)
                                errors.Add($"Fetched {RecordsAbleToUpload.Count} Rows")
                                Dim returnMessage As String = ""
                                If RecordsAbleToUpload.Count > 0 Then
                                    'Dim caseNumberHistory As DataTable = New DataTable()
                                    'caseNumberHistory = Me.CaseNumberOnlyHistory("")
                                    sCommand.Append(String.Join(",", RecordsAbleToUpload))
                                    sCommand.Append(";")

                                    con_insert.Open()
                                    cmd_add_comms.Connection = con_insert
                                    cmd_add_comms.CommandText = sCommand.ToString
                                    If cmd_add_comms.ExecuteNonQuery() = 1 Then
                                        errors.Add($"Upload was successful")
                                        con_insert.Close()
                                        returnMessage = $"{RecordsAbleToUpload.Count} of {FileRecords.Rows.Count} records uploaded successfully."
                                        'Dim messagedisplay As String = String.Format("file with {0} records has been succesfully uploaded", kout.Rows.Count.ToString())

                                        fileuploadhist(fupload.FileName, Session("User_Name"), RecordsAbleToUpload.Count)
                                    Else
                                        errors.Add($"Unable to upload")
                                        returnMessage = $"Complaint Upload not successful. Please contact system admin"
                                    End If

                                Else
                                    errors.Add($"Complaint Upload for {fupload.FileName} not successful :: Nothing to upload")
                                    returnMessage = $"Nothing to upload"
                                End If
                                lblerror.Text = returnMessage
                            Catch ex As Exception
                                errors.Add($"Exception :::: {ex.Message}")
                                lblerror.Text = ex.Message
                            End Try
                        End Using
                    End Using
                End Using
            End If

        Else
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No file to upload');</script>", False)
            'lblerror.Text = "No file to upload"
        End If
        WriteToFile(errors)
    End Sub
    Private Sub WriteToFile(textContent As ArrayList)
        Dim local As String = "C:\Users\AB031F5\Documents\CxLogging\"
        Dim uat As String = "C:\Users\AB031F5\Documents\CxLogging\"
        Dim prod As String = "D:\Logs\CxLogging\System\"
        Dim url As String = prod
        Directory.CreateDirectory(url)
        Dim fileUrl As String = url + DateTime.Now.ToString("yyyMMdd") & ".txt"
        Try
            Dim stream As FileStream = New FileStream(fileUrl, FileMode.Append, FileAccess.Write)
            Dim sw As StreamWriter = New StreamWriter(stream)
            If textContent.Count < 1 Then
                sw.Close()
                createFile(fileUrl)
            Else
                For Each item As Object In textContent
                    Console.WriteLine(item)
                    sw.WriteLine($"{item} -------- {DateTime.Now.ToString()}")
                Next
                sw.Close()
            End If

        Catch ex As Exception

        End Try
    End Sub
    Friend Sub createFile(ByVal fileUrl As String)
        Try
            Dim fs As FileStream = New FileStream(fileUrl, FileMode.Append, FileAccess.Write)
            Dim sw As StreamWriter = New StreamWriter(fs)
            sw.Close()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Function RunUploadHistory(fileName As String) As DataTable
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Dim storedProcedureName As String = "sp_CheckFileUpload"
        Dim resultDataTable As New DataTable()
        Using connection As New MySqlConnection(connectionString)
            connection.Open()
            Using command As New MySqlCommand()
                command.CommandType = CommandType.StoredProcedure
                command.CommandText = storedProcedureName
                command.Connection = connection
                command.Parameters.AddWithValue("fileName", fileName)
                Using reader As MySqlDataReader = command.ExecuteReader()
                    resultDataTable.Load(reader)
                End Using
                connection.Close()
                'connection.Dispose()
            End Using
        End Using
        Return resultDataTable
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
    Function GetRows(kout As DataTable) As List(Of String)
        Dim errors As ArrayList = New ArrayList()
        Dim Correct As List(Of String) = New List(Of String)()
        Dim Failed As List(Of String) = New List(Of String)()
        Dim DuplicateCases As List(Of String) = New List(Of String)()
        Dim DuplicateUpdateCases As List(Of String) = New List(Of String)()
        For i = 0 To kout.Rows.Count - 1
            Dim rand As New Random()
            Dim threadNumber As Integer = rand.Next(1, 11)
            Dim threads As String = Convert.ToString(threadNumber)

            Dim caseNumberOnlyHistory As DataTable = New DataTable()
            caseNumberOnlyHistory = Me.CaseNumberOnlyHistory(MySqlHelper.EscapeString(kout.Rows(i)(0)))
            Dim fileStatus As String = MySqlHelper.EscapeString(kout.Rows(i)(4)).ToString()
            If caseNumberOnlyHistory.Rows.Count > 0 Then
                'Check condition for duplicate
                Dim recordStatus As String = caseNumberOnlyHistory.Rows(0)("Status").ToString()

                If recordStatus.Equals(fileStatus) Then
                    'Flag as duplicate
                    DuplicateCases.Add(String.Format("('{0}','{1}','{2}','{3}','{4}','{5}')",
                                                           MySqlHelper.EscapeString(kout.Rows(i)(0)),
                                                           MySqlHelper.EscapeString(kout.Rows(i)(1)),
                                                           MySqlHelper.EscapeString(kout.Rows(i)(2)),
                                                           MySqlHelper.EscapeString(kout.Rows(i)(3)),
                                                           MySqlHelper.EscapeString(kout.Rows(i)(4)),
                                                           threads))
                Else
                    'Update Status for record in db
                    UpdateStatus(MySqlHelper.EscapeString(kout.Rows(i)(0)).ToString(), MySqlHelper.EscapeString(kout.Rows(i)(3)), MySqlHelper.EscapeString(kout.Rows(i)(4)).ToString())
                    DuplicateUpdateCases.Add(String.Format("('{0}','{1}','{2}','{3}','{4}','{5}')",
                                                           MySqlHelper.EscapeString(kout.Rows(i)(0)),
                                                           MySqlHelper.EscapeString(kout.Rows(i)(1)),
                                                           MySqlHelper.EscapeString(kout.Rows(i)(2)),
                                                           MySqlHelper.EscapeString(kout.Rows(i)(3)),
                                                           MySqlHelper.EscapeString(kout.Rows(i)(4)),
                                                           threads))
                End If
            Else
                Dim dateValue As DateTime
                Dim fileDateCreation As String = MySqlHelper.EscapeString(kout.Rows(i)(3))

                If DateTime.TryParse(fileDateCreation, dateValue) Then
                    fileDateCreation = dateValue.ToString("dd-MM-yyyy h:mm:ss tt")
                Else
                    Console.WriteLine("Error converting string to date.")
                End If

                Dim expectedDateFormats() As String = {"MM/dd/yyyy HH:mm", "dd/M/yyyy HH:mm", "M/d/yyyy HH:mm", "M/d/yyyy H:mm", "MM-dd-yyyy h:mm:ss tt", "dd-MM-yyyy h:mm:ss tt", "dd-mm-yy h:mm", "dd-mmm-yy", "yyyy-MM-dd hh:mm t", "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy hh:mm t", "yyyy-MM-dd hh:mm:ss"}

                Dim resultDate As DateTime

                If DateTime.TryParseExact(fileDateCreation, expectedDateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, resultDate) Then
                    Correct.Add(String.Format("('{0}','{1}','{2}','{3}','{4}','{5}')",
                                                           MySqlHelper.EscapeString(kout.Rows(i)(0)),
                                                           MySqlHelper.EscapeString(kout.Rows(i)(1)),
                                                           MySqlHelper.EscapeString(kout.Rows(i)(2)),
                                                           resultDate.ToString("yyyy-MM-dd hh:mm:ss"),
                                                           MySqlHelper.EscapeString(kout.Rows(i)(4)),
                                                           threads))
                Else
                    Console.WriteLine($"{fileDateCreation} is not a valid datetime")
                    Failed.Add(String.Format("('{0}','{1}','{2}','{3}','{4}','{5}')",
                                                           MySqlHelper.EscapeString(kout.Rows(i)(0)),
                                                           MySqlHelper.EscapeString(kout.Rows(i)(1)),
                                                           MySqlHelper.EscapeString(kout.Rows(i)(2)),
                                                           fileDateCreation,
                                                           MySqlHelper.EscapeString(kout.Rows(i)(4)),
                                                           threads))
                End If
            End If

        Next

        errors.Add($"{Correct.Count} Rows are ok")
        errors.Add($"{Failed.Count} Rows have invalid Dates")
        errors.Add($"{DuplicateCases.Count} Rows are duplicates")
        If Failed.Count > 0 Then
            WriteFailedToCSV(Failed)
        End If

        If DuplicateCases.Count > 0 Then
            WriteDuplicateFilesToCSV(DuplicateCases)
        End If
        If DuplicateUpdateCases.Count > 0 Then
            Dim mess As String = $"{DuplicateUpdateCases.Count} Record(s) have been updated to a new Status"
            If Environment.UserInteractive Then
                MessageBox.Show(mess)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & mess & " ');</script>", False)
            End If
        End If
        WriteToFile(errors)
        Return Correct
    End Function
    Sub WriteDuplicateFilesToCSV(dataList As List(Of String))
        Dim delimiter As String = ","
        Dim csvContent As New StringBuilder()
        For Each dataItem In dataList
            csvContent.AppendLine(dataItem)
        Next


        Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")
        Dim csvFileName As String = $"DuplicateCases_{timestamp}.csv"

        Dim documentsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim filePath As String = $"{documentsPath}\{csvFileName}.csv"
        System.IO.File.WriteAllText(filePath, csvContent.ToString())

        Dim mess As String = $"{dataList.Count} Record(s) failed to upload due to duplicate Case Numbers. These cases are still not processed. The file has been saved in My Documents as '{csvFileName}'"

        If Environment.UserInteractive Then
            MessageBox.Show(mess)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & mess & " ');</script>", False)
        End If
    End Sub
    Sub WriteFailedToCSV(dataList As List(Of String))
        Dim delimiter As String = ","

        Dim csvContent As New StringBuilder()

        For Each dataItem In dataList
            csvContent.AppendLine(dataItem)
        Next


        Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")
        Dim csvFileName As String = $"FailedUploads_{timestamp}.csv"

        Dim documentsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim filePath As String = $"{documentsPath}\{csvFileName}.csv"
        System.IO.File.WriteAllText(filePath, csvContent.ToString())

        Dim mess As String = $"{dataList.Count} Record(s) failed to upload due to invalid creation_datetime. The file has been saved in My Documents as '{csvFileName}'"
        If Environment.UserInteractive Then
            MessageBox.Show(mess)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & mess & " ');</script>", False)
        End If

    End Sub
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

    Private Function ConvertDataTableToHTML(dataTable As DataTable) As String
        Dim htmlStringBuilder As New StringBuilder()

        htmlStringBuilder.AppendLine("<table id='table_id' class='display cell-border hover stripe' style='font-size: 11pt;font-family:Roboto; width:100%;border-radius:3px;overflow:hidden;'>")

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
    Public Class Foo
        Public Property case_number As String
        Public Property customer_name As String
        Public Property account_number As String
        Public Property creation_datetime As String
        Public Property status As String
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

    Private Function CaseNumberWithStatusHistory(caseNumber As String, status As String) As DataTable
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Dim storedProcedureName As String = "sp_CheckCaseNumberExists"
        Dim resultDataTable As New DataTable()
        Using connection As New MySqlConnection(connectionString)
            connection.Open()
            Using command As New MySqlCommand()
                command.CommandType = CommandType.StoredProcedure
                command.CommandText = storedProcedureName
                command.Connection = connection
                command.Parameters.AddWithValue("caseNumber", caseNumber)
                command.Parameters.AddWithValue("stat", status)
                Using reader As MySqlDataReader = command.ExecuteReader()
                    resultDataTable.Load(reader)
                End Using
                connection.Close()
                'connection.Dispose()
            End Using
        End Using
        Return resultDataTable
    End Function
    Private Function CaseNumberOnlyHistory(caseNumber As String) As DataTable
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Dim storedProcedureName As String = "sp_CheckCaseNumberExists"
        Dim resultDataTable As New DataTable()
        Using connection As New MySqlConnection(connectionString)
            connection.Open()
            Using command As New MySqlCommand()
                command.CommandType = CommandType.StoredProcedure
                command.CommandText = storedProcedureName
                command.Connection = connection
                command.Parameters.AddWithValue("caseNumber", caseNumber)
                Using reader As MySqlDataReader = command.ExecuteReader()
                    resultDataTable.Load(reader)
                End Using
                connection.Close()
                'connection.Dispose()
            End Using
        End Using
        Return resultDataTable
    End Function

    Public Sub UpdateStatus(caseNumber As String, creationTime As String, newStatus As String)
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con_fake As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim cmd_add_comms As New MySql.Data.MySqlClient.MySqlCommand
            Try
                con_fake.Open()
                cmd_add_comms.Connection = con_fake
                cmd_add_comms.CommandText = "updatePreprocessedFromWIPStatus"
                cmd_add_comms.CommandType = CommandType.StoredProcedure
                cmd_add_comms.Parameters.AddWithValue("caseNum", caseNumber)
                cmd_add_comms.Parameters.AddWithValue("creationTime", creationTime)
                cmd_add_comms.Parameters.AddWithValue("stat", newStatus)

                cmd_add_comms.ExecuteNonQuery()
                con_fake.Close()
            Catch ex As Exception

            Finally

                con_fake.Dispose()
            End Try
        End Using
    End Sub
End Class