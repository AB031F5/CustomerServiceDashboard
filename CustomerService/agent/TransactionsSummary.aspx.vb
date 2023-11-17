Imports System.Data.SqlClient

Public Class TransactionsSummary
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub getMNO_Click(sender As Object, e As EventArgs) Handles getMNO.Click
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con_get As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim cmd_getbycnum As New MySql.Data.MySqlClient.MySqlCommand
            Try
                con_get.Open()
                Dim selectedMno As String = searchmnos.SelectedItem.Text
                Dim selectedType As String = searchType.SelectedItem.Text
                If selectedMno = "Summary" Then
                    cmd_getbycnum.Connection = con_get
                    cmd_getbycnum.CommandText = "customers.Momo_getSummary"
                    cmd_getbycnum.CommandType = CommandType.StoredProcedure
                    Using kda As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnum)
                        Dim getmsgtable As DataTable = New DataTable()
                        kda.Fill(getmsgtable)
                        If getmsgtable.Rows.Count = 0 Then
                            lblerroratmsg.Text = "No Records to display"
                        Else
                            Dim returnrecods As String = ConvertDataTableToHTML(getmsgtable)
                            PlaceHolder1.Controls.Add(New Literal() With {
                              .Text = returnrecods.ToString()
                              })
                            lblerroratmsg.Text = getmsgtable.Rows.Count.ToString() + " Records Returned"
                        End If
                    End Using
                ElseIf selectedMno = "MTN" Or selectedMno = "Airtel" Then

                    cmd_getbycnum.Connection = con_get
                    cmd_getbycnum.CommandType = CommandType.StoredProcedure
                    cmd_getbycnum.CommandText = "customers._AggregateTxnSummary"
                    cmd_getbycnum.CommandType = CommandType.StoredProcedure
                    cmd_getbycnum.Parameters.AddWithValue("selectedMno", selectedMno)
                    cmd_getbycnum.Parameters.AddWithValue("selectedType", selectedType)

                    Using kda2 As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd_getbycnum)
                        Dim getmsgtablefs As DataTable = New DataTable()
                        kda2.Fill(getmsgtablefs)
                        If getmsgtablefs.Rows.Count = 0 Then
                            lblerroratmsg.Text = "No Records to display"
                        Else
                            Dim returnrecodsfs As String = ConvertDataTableToHTML(getmsgtablefs)
                            PlaceHolder1.Controls.Add(New Literal() With {.Text = returnrecodsfs.ToString()})
                            lblerroratmsg.Text = getmsgtablefs.Rows.Count.ToString() + " Records Returned"
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
    Public Shared Function ConvertDataTableToHTML(ByVal dt As DataTable) As String
        Dim html As String = "<table id='table_id2' class='display' cellpadding='5' cellspacing='0' style='border: 1px solid #ccc;font-size: 9pt;font-family:Calibri; width:100%'>"
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