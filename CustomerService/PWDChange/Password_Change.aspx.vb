Option Strict On
Option Explicit On
Option Infer Off
Imports System.Windows.Forms
Imports System.Security.Cryptography
Public Class Password_Change
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        pwd_change.Show()
        'Try
        '    If Session("Reset_Flag").ToString = "Y" Then
        '        pwd_change.Show()
        '    End If
        'Catch ex As Exception

        'End Try

    End Sub

    Private Sub usrcpwdccnc_Click(sender As Object, e As EventArgs) Handles usrcpwdccnc.Click
        Session.Clear()
        Session.Abandon()
        Response.Redirect("~/Login.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    Private Sub usrcpwdcbtn_Click(sender As Object, e As EventArgs) Handles usrcpwdcbtn.Click
        If nwpwd.Text <> renwpwd.Text Then
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Entered Passwords do not Match');</script>", False)
            'MessageBox.Show("Entered Passwords do not Match")
            Exit Sub
        Else
            If Session("User_Name").ToString() <> vbNullString Then
                Dim amendusr As String
                amendusr = CType(Session("User_Name"), String)
                Dim xb As Byte()
                Dim ePass As String = Addapp_User.ComputeHash(nwpwd.Text, "SHA512", xb)
                Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
                Using con_insert As New MySql.Data.MySqlClient.MySqlConnection(constr)
                    Dim cmd_con_amend As New MySql.Data.MySqlClient.MySqlCommand
                    Dim login_cmd As New MySql.Data.MySqlClient.MySqlCommand
                    Try
                        con_insert.Open()
                        login_cmd.Connection = con_insert
                        login_cmd.CommandText = "Validate_system_user"
                        login_cmd.CommandType = CommandType.StoredProcedure
                        login_cmd.Parameters.AddWithValue("usern_var", amendusr)
                        Using sdalogin As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(login_cmd)
                            Dim dtlogin As DataTable = New DataTable()
                            sdalogin.Fill(dtlogin)
                            Dim pwdstring As String = dtlogin.Rows(0)("Password").ToString()
                            Dim flag As Boolean = Addapp_User.VerifyHash(cpwd.Text, "SHA512", pwdstring)
                            If flag = True Then
                                cmd_con_amend.Connection = con_insert
                                cmd_con_amend.CommandText = "amend_system_user"
                                cmd_con_amend.CommandType = CommandType.StoredProcedure
                                cmd_con_amend.Parameters.AddWithValue("nupass", ePass)
                                cmd_con_amend.Parameters.AddWithValue("flag", "N")
                                cmd_con_amend.Parameters.AddWithValue("myuser", amendusr)
                                cmd_con_amend.Parameters.AddWithValue("mylastdt", DateTime.Now)

                                cmd_con_amend.ExecuteScalar()

                                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('User Password Amended Succesfully');location.href = ' " + ResolveUrl("~/Default.aspx") + "';</script>", False)
                                'MessageBox.Show("User Password Amended Succesfully")
                                'Response.Redirect("~/Default.aspx", False)
                                'Context.ApplicationInstance.CompleteRequest()
                            Else
                                con_insert.Close()
                                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Current Password is Wrong. Please try Again');</script>", False)
                                'MessageBox.Show("Current Password is Wrong. Please try Again")
                                Exit Sub
                            End If
                        End Using
                        con_insert.Close()
                    Catch ex As Exception
                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.Message & " ');</script>", False)
                    Finally
                        con_insert.Dispose()
                    End Try
                End Using
            End If

        End If
    End Sub

    Private Sub model_kok2_Click(sender As Object, e As EventArgs) Handles model_kok2.Click
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim login_status As New MySql.Data.MySqlClient.MySqlCommand
            Try
                con.Open()
                'update status to logged off
                login_status.Connection = con
                login_status.CommandText = "Set_Loggin_Status"
                login_status.CommandType = CommandType.StoredProcedure
                login_status.Parameters.AddWithValue("usname", Session("User_Name"))
                login_status.Parameters.AddWithValue("logsts", 0)
                login_status.ExecuteScalar()
                con.Close()
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.Message & " ');</script>", False)
            Finally
                con.Dispose()
            End Try
        End Using
        Session.Clear()
        Session.Abandon()
        Response.Redirect("~/Login.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    Private Sub Submit_det2_Click(sender As Object, e As EventArgs) Handles Submit_det2.Click
        If nwpwdpop.Text <> renwpwdpop.Text Then
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Entered Passwords do not Match');</script>", False)
            Exit Sub
        Else
            If Session("User_Name").ToString() <> vbNullString Then
                Dim amendusr As String
                amendusr = Session("User_Name").ToString()
                Dim xb As Byte()
                Dim ePass As String = Addapp_User.ComputeHash(nwpwdpop.Text, "SHA512", xb)
                Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
                Using con_insert As New MySql.Data.MySqlClient.MySqlConnection(constr)
                    Dim cmd_con_amend As New MySql.Data.MySqlClient.MySqlCommand
                    Dim login_cmd As New MySql.Data.MySqlClient.MySqlCommand
                    Try
                        con_insert.Open()
                        login_cmd.Connection = con_insert
                        login_cmd.CommandText = "Validate_system_user"
                        login_cmd.CommandType = CommandType.StoredProcedure
                        login_cmd.Parameters.AddWithValue("usern_var", amendusr)
                        Using sdalogin As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(login_cmd)
                            Dim dtlogin As DataTable = New DataTable()
                            sdalogin.Fill(dtlogin)
                            Dim pwdstring As String = dtlogin.Rows(0)("Password").ToString()
                            Dim flag As Boolean = Addapp_User.VerifyHash(cpwdpop.Text, "SHA512", pwdstring)
                            If flag = True Then
                                cmd_con_amend.Connection = con_insert
                                cmd_con_amend.CommandText = "amend_system_user"
                                cmd_con_amend.CommandType = CommandType.StoredProcedure
                                cmd_con_amend.Parameters.AddWithValue("nupass", ePass)
                                cmd_con_amend.Parameters.AddWithValue("flag", "N")
                                cmd_con_amend.Parameters.AddWithValue("myuser", amendusr)
                                cmd_con_amend.Parameters.AddWithValue("mylastdt", DateTime.Now)
                                cmd_con_amend.ExecuteScalar()

                                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('User Password Amended Succesfully');location.href = ' " + ResolveUrl("~/Default.aspx") + "';</script>", False)
                                '    MessageBox.Show("User Password Amended Succesfully")
                                '    Response.Redirect("~/Default.aspx", False)
                                '    Context.ApplicationInstance.CompleteRequest()
                            Else
                                con_insert.Close()
                                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Current Password is Wrong. Please try Again');</script>", False)
                                'MessageBox.Show("Current Password is Wrong. Please try Again")
                                pwd_change.Show()
                                Exit Sub
                            End If
                        End Using
                        con_insert.Close()
                    Catch ex As Exception
                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.Message & " ');</script>", False)
                    Finally
                        con_insert.Dispose()
                    End Try
                End Using
            End If

        End If
    End Sub

End Class