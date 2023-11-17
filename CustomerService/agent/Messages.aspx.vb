Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports System.Xml
Imports System.Web.UI.HtmlControls
Public Class Messages
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub



    Protected Sub getlist_Click(sender As Object, e As EventArgs) Handles getlist.Click
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con_get As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim cmd_getbycnum As New MySql.Data.MySqlClient.MySqlCommand
            Try
                con_get.Open()
                If searchmsgs.SelectedItem.Text = "Case Number" Then
                    cmd_getbycnum.Connection = con_get
                    cmd_getbycnum.CommandText = "getcomsbycnumber"
                    cmd_getbycnum.CommandType = CommandType.StoredProcedure
                    cmd_getbycnum.Parameters.AddWithValue("cnumber", searchstringone.Text)
                    Using kda As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnum)
                        Dim getmsgtable As DataTable = New DataTable()
                        kda.Fill(getmsgtable)
                        If getmsgtable.Rows.Count = 0 Then
                            lblerroratmsg.Text = "No Records to display"
                        Else
                            Dim returnrecods As String = ConvertDataTableToHTML2(getmsgtable)
                            PlaceHolder1.Controls.Add(New Literal() With {
                              .Text = returnrecods.ToString()
                              })
                            'lblerroratmsg.Text = getmsgtable.Rows.Count.ToString() + " Records Returned"
                        End If
                    End Using
                ElseIf searchmsgs.SelectedItem.Text = "Account Number" Then
                    cmd_getbycnum.Connection = con_get
                    cmd_getbycnum.CommandText = "getcomsbyacct"
                    cmd_getbycnum.CommandType = CommandType.StoredProcedure
                    cmd_getbycnum.Parameters.AddWithValue("ccustact", searchstringone.Text)
                    Using kda2 As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnum)
                        Dim getmsgtable2 As DataTable = New DataTable()
                        kda2.Fill(getmsgtable2)
                        If getmsgtable2.Rows.Count = 0 Then
                            lblerroratmsg.Text = "No Records to display"
                        Else
                            Dim returnrecods2 As String
                            returnrecods2 = ConvertDataTableToHTML2(getmsgtable2)
                            PlaceHolder1.Controls.Add(New Literal() With {
                              .Text = returnrecods2.ToString()
                              })
                            'lblerroratmsg.Text = getmsgtable2.Rows.Count.ToString() + " Records Returned"
                            If download.Checked = True Then
                                Dim fings As String
                                fings = savedatatable(getmsgtable2)
                            End If
                        End If
                    End Using
                ElseIf searchmsgs.SelectedItem.Text = "Date Range" Then
                    cmd_getbycnum.Connection = con_get
                    cmd_getbycnum.CommandText = "getcomsbydaterange"
                    cmd_getbycnum.CommandType = CommandType.StoredProcedure
                    cmd_getbycnum.Parameters.AddWithValue("sdate", startddate.Text)
                    cmd_getbycnum.Parameters.AddWithValue("edate", enddate.Text)
                    Using kda2 As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnum)
                        Dim getmsgtable2 As DataTable = New DataTable()
                        kda2.Fill(getmsgtable2)
                        If getmsgtable2.Rows.Count = 0 Then
                            lblerroratmsg.Text = "No Records to display"
                        Else
                            Dim returnrecods2 As String = ConvertDataTableToHTML2(getmsgtable2)
                            PlaceHolder1.Controls.Add(New Literal() With {
                              .Text = returnrecods2.ToString()
                              })
                            'lblerroratmsg.Text = getmsgtable2.Rows.Count.ToString() + " Records Returned"
                            If download.Checked = True Then
                                Dim fings As String
                                fings = savedatatable(getmsgtable2)
                            End If
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

    Private Sub searchmsgs_TextChanged(sender As Object, e As EventArgs) Handles searchmsgs.TextChanged
        If searchmsgs.SelectedItem.Text = "Date Range" Then
            searchstringone.Text = "Date Range"
        Else
            searchstringone.Text = ""
        End If
    End Sub

    Public Shared Function ConvertDataTableToHTML2(ByVal dt As DataTable) As String
        Dim html As String = "<table id='table_id2' class='display' cellpadding='5' cellspacing='0' style='border: 1px solid #ccc;font-size: 11pt;font-family:Calibri; width:100%'>"
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