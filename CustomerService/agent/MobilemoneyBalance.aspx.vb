Public Class MobilemoneyBalance
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con_get As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim cmd_getbycnum As New MySql.Data.MySqlClient.MySqlCommand
            Try
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



    Public Shared Function ConvertDataTableToHTML12(ByVal dt As DataTable) As String
        Dim html As String = "<table id='table_id2' class='display' cellpadding='5' cellspacing='0' style='border-collapse: collapse; width: 100%; border-radius: 10px; overflow: hidden;font-size:11pt;font-family:Calibri; width:100%'>"
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
                html += "<td style='width:100px;border: 1px solid #ddd;'>" & dt.Rows(i)(j).ToString().ToUpper() & "</td>"
            Next

            html += "</tr>"
        Next
        html += "</tbody>"
        html += "</table>"
        Return html

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