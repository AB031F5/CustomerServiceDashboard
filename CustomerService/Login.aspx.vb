Option Strict On
Option Explicit On
Option Infer Off
Imports System.Windows.Forms
Imports System.Security.Cryptography
Imports System.Web.Script.Services
Imports System.Web.Services
Imports System.Collections
Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Me.Page.User.Identity.IsAuthenticated Then
                FormsAuthentication.SignOut()
                Response.Redirect("~/Login.aspx")
            End If
        End If
    End Sub

    Protected Sub testLogin_Click(sender As Object, e As EventArgs) Handles testLogin.Click
        Dim userId As Integer = 0
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con As New MySql.Data.MySqlClient.MySqlConnection(constr)
            'Dim fing As MySql.Data.MySqlClient.MySqlDataReader
            Dim login_cmd, login_status As New MySql.Data.MySqlClient.MySqlCommand
            Dim logged_user As String = email.Text

            Dim logged_password As String = pass.Text
            Try
                LogUserOut(logged_user)
                con.Open()
                login_cmd.Connection = con
                login_cmd.CommandText = "getlogin_user"
                login_cmd.CommandType = CommandType.StoredProcedure
                login_cmd.Parameters.AddWithValue("usern_var", logged_user)
                login_cmd.Parameters.AddWithValue("flag_var", "N")
                Using sdalogin As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(login_cmd)
                    Dim dtlogin As DataTable = New DataTable()
                    sdalogin.Fill(dtlogin)
                    If dtlogin.Rows.Count = 0 Then
                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Invalid Username or password');</script>", False)
                        'Login1.FailureText = "Invalid Username or password"
                        Exit Sub
                    End If
                    Dim usid As Integer = CInt(dtlogin.Rows(0)("User_ID"))
                    Dim resetflg As String = dtlogin.Rows(0)("Reset_Flg").ToString()
                    Dim usname As String = dtlogin.Rows(0)("User_Name").ToString()
                    Dim name As String = dtlogin.Rows(0)("First_Name").ToString()
                    Dim pwdstring As String = dtlogin.Rows(0)("Password").ToString()
                    Dim uprof As String = dtlogin.Rows(0)("User_Profile").ToString()
                    Dim llogdt As Date = CDate(dtlogin.Rows(0)("Lastpwdchangedate"))
                    Dim loginstatus As Integer = CInt(dtlogin.Rows(0)("Isloggedon"))
                    Dim flag As Boolean = Addapp_User.VerifyHash(logged_password, "SHA512", pwdstring)
                    If usname = logged_user And flag = True Then
                        If loginstatus = 0 Then
                            'update status to logged in
                            login_status.Connection = con
                            login_status.CommandText = "Set_Loggin_Status"
                            login_status.CommandType = CommandType.StoredProcedure
                            login_status.Parameters.AddWithValue("usname", logged_user)
                            login_status.Parameters.AddWithValue("logsts", 1)
                            login_status.ExecuteScalar()

                            FormsAuthentication.SetAuthCookie(Login1.UserName, True)
                            Session("User_Name") = usname
                            Session("User_Profile") = uprof
                            Session("User_ID") = usid
                            Session("Reset_Flag") = resetflg
                            Session("Lastpwdchangedate") = llogdt
                            Session("Name") = name
                            Dim ticket As New FormsAuthenticationTicket(1, name, DateTime.Now, DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes), Login1.RememberMeSet, uprof, FormsAuthentication.FormsCookiePath)
                            Dim hash As String = FormsAuthentication.Encrypt(ticket)
                            Dim cookie As New HttpCookie(FormsAuthentication.FormsCookieName, hash)
                            cookie.HttpOnly = True
                            cookie.Secure = FormsAuthentication.RequireSSL
                            cookie.Path = FormsAuthentication.FormsCookiePath
                            cookie.Domain = FormsAuthentication.CookieDomain
                            If ticket.IsPersistent Then
                                cookie.Expires = ticket.Expiration
                            End If
                            Response.Cookies.Add(cookie)
                            If resetflg = "Y" Then
                                Response.Redirect("~/PWDChange/Password_Change.aspx", False)
                                Context.ApplicationInstance.CompleteRequest()
                            Else
                                If llogdt.Date.AddDays(30) < DateTime.Now.Date Then
                                    Response.Redirect("~/PWDChange/Password_Change.aspx", False)
                                    Context.ApplicationInstance.CompleteRequest()
                                Else
                                    Response.Redirect(FormsAuthentication.GetRedirectUrl(Login1.UserName, Login1.RememberMeSet), False)
                                    Context.ApplicationInstance.CompleteRequest()
                                End If
                            End If
                        Else
                            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('User Already Logged in');</script>", False)
                            'Login1.FailureText = "User Already Logged in"
                        End If

                    Else
                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Username and/or password is incorrect');</script>", False)
                        Login1.FailureText = "Username and/or password is incorrect."
                    End If

                    con.Close()
                End Using
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.Message & " ');</script>", False)
            Finally
                con.Dispose()
            End Try
        End Using
    End Sub

    Protected Sub Login1_Authenticate(sender As Object, e As AuthenticateEventArgs) Handles Login1.Authenticate
        Dim userId As Integer = 0
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con As New MySql.Data.MySqlClient.MySqlConnection(constr)
            'Dim fing As MySql.Data.MySqlClient.MySqlDataReader
            Dim login_cmd, login_status As New MySql.Data.MySqlClient.MySqlCommand
            Dim logged_user As String = Login1.UserName
            Dim logged_password As String = Login1.Password
            Try
                con.Open()
                login_cmd.Connection = con
                login_cmd.CommandText = "getlogin_user"
                login_cmd.CommandType = CommandType.StoredProcedure
                login_cmd.Parameters.AddWithValue("usern_var", logged_user)
                login_cmd.Parameters.AddWithValue("flag_var", "N")
                Using sdalogin As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(login_cmd)
                    Dim dtlogin As DataTable = New DataTable()
                    sdalogin.Fill(dtlogin)
                    If dtlogin.Rows.Count = 0 Then
                        Login1.FailureText = "Invalid Username or password"
                        Exit Sub
                    End If
                    Dim usid As Integer = CInt(dtlogin.Rows(0)("User_ID"))
                    Dim resetflg As String = dtlogin.Rows(0)("Reset_Flg").ToString()
                    Dim usname As String = dtlogin.Rows(0)("User_Name").ToString()
                    Dim pwdstring As String = dtlogin.Rows(0)("Password").ToString()
                    Dim uprof As String = dtlogin.Rows(0)("User_Profile").ToString()
                    Dim llogdt As Date = CDate(dtlogin.Rows(0)("Lastpwdchangedate"))
                    Dim loginstatus As Integer = CInt(dtlogin.Rows(0)("Isloggedon"))
                    Dim flag As Boolean = Addapp_User.VerifyHash(logged_password, "SHA512", pwdstring)
                    If usname = logged_user And flag = True Then
                        If loginstatus = 0 Then
                            'update status to logged in
                            login_status.Connection = con
                            login_status.CommandText = "Set_Loggin_Status"
                            login_status.CommandType = CommandType.StoredProcedure
                            login_status.Parameters.AddWithValue("usname", logged_user)
                            login_status.Parameters.AddWithValue("logsts", 1)
                            login_status.ExecuteScalar()

                            FormsAuthentication.SetAuthCookie(Login1.UserName, True)
                            Session("User_Name") = usname
                            Session("User_Profile") = uprof
                            Session("User_ID") = usid
                            Session("Reset_Flag") = resetflg
                            Session("Lastpwdchangedate") = llogdt
                            Dim ticket As New FormsAuthenticationTicket(1, Login1.UserName, DateTime.Now, DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes), Login1.RememberMeSet, uprof, FormsAuthentication.FormsCookiePath)
                            Dim hash As String = FormsAuthentication.Encrypt(ticket)
                            Dim cookie As New HttpCookie(FormsAuthentication.FormsCookieName, hash)
                            cookie.HttpOnly = True
                            cookie.Secure = FormsAuthentication.RequireSSL
                            cookie.Path = FormsAuthentication.FormsCookiePath
                            cookie.Domain = FormsAuthentication.CookieDomain
                            If ticket.IsPersistent Then
                                cookie.Expires = ticket.Expiration
                            End If
                            Response.Cookies.Add(cookie)
                            If resetflg = "Y" Then
                                Response.Redirect("~/PWDChange/Password_Change.aspx", False)
                                Context.ApplicationInstance.CompleteRequest()
                            Else
                                If llogdt.Date.AddDays(30) < DateTime.Now.Date Then
                                    Response.Redirect("~/PWDChange/Password_Change.aspx", False)
                                    Context.ApplicationInstance.CompleteRequest()
                                Else
                                    Response.Redirect(FormsAuthentication.GetRedirectUrl(Login1.UserName, Login1.RememberMeSet), False)
                                    Context.ApplicationInstance.CompleteRequest()
                                End If
                            End If
                        Else
                            Login1.FailureText = "User Already Logged in"
                        End If

                    Else
                        Login1.FailureText = "Username and/or password is incorrect."
                    End If

                    con.Close()
                End Using
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.Message & " ');</script>", False)
            Finally
                con.Dispose()
            End Try
        End Using
    End Sub

    Private Sub Login1_LoggedIn(sender As Object, e As EventArgs) Handles Login1.LoggedIn

    End Sub

    Protected Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            UserName.Enabled = True
            ResetUser.Enabled = True
        Else
            UserName.Enabled = False
            ResetUser.Enabled = False
        End If
    End Sub

    Protected Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            Register_DIV.Visible = True
        Else
            Register_DIV.Visible = False
        End If
    End Sub

    Protected Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked = True Then
            Logout_DIV.Visible = True
            UserName_logout.Enabled = True
            Logout_User.Enabled = True
        Else
            Logout_DIV.Visible = False
            UserName_logout.Enabled = False
            Logout_User.Enabled = False
        End If
    End Sub

    Private Sub ResetUser_Click(sender As Object, e As EventArgs) Handles ResetUser.Click
        Dim amendusr, emaal_add As String
        emaal_add = vbNullString
        If UserName.Text = vbNullString Then
            'MessageBox.Show("Please input User Name to Reset")
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please input User Name To Reset ');</script>", False)
            Exit Sub
        Else
            amendusr = UserName.Text
        End If
        Dim size As Integer = 8
        Dim chars As Char() = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray()
        Dim data As Byte() = New Byte(size - 1) {}

        Using crypto As RNGCryptoServiceProvider = New RNGCryptoServiceProvider()
            crypto.GetBytes(data)
        End Using

        Dim result As StringBuilder = New StringBuilder(size)

        For Each b As Byte In data
            result.Append(chars(b Mod (chars.Length)))
        Next

        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con_insert As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim cmd_con_amend, con_lookup, cmd_sndmsg As New MySql.Data.MySqlClient.MySqlCommand
            Try
                con_insert.Open()
                con_lookup.Connection = con_insert
                con_lookup.CommandText = "get_user_Email"
                con_lookup.CommandType = CommandType.StoredProcedure
                con_lookup.Parameters.AddWithValue("deeuser", amendusr)
                Using user_lookup As MySql.Data.MySqlClient.MySqlDataReader = con_lookup.ExecuteReader()
                    Dim flexiread As New DataTable
                    flexiread.Load(user_lookup)


                    If flexiread.Rows.Count = 0 Then
                        ' MessageBox.Show("Invalid User Name or User not registered")
                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Invalid User Name or User not registered');</script>", False)
                        user_lookup.Close()
                        user_lookup.Dispose()
                        con_insert.Close()
                        con_insert.Dispose()
                        Exit Sub
                    Else
                        emaal_add = CType(flexiread.Rows(0)("Email_Address"), String)
                    End If
                    user_lookup.Close()
                    user_lookup.Dispose()
                End Using

                'Dim instmsg As String = "your password for the nomination system has been Reset to: "
                'instmsg = instmsg + result.ToString()
                'Dim numsg As New MySql.Data.MySqlClient.MySqlCommand
                'numsg.Connection = con_insert
                'numsg.CommandText = "myoutbox"
                'numsg.CommandType = CommandType.StoredProcedure
                'numsg.Parameters.AddWithValue("eaddress", emaal_add)
                'numsg.Parameters.AddWithValue("emessage", instmsg)
                'numsg.Parameters.AddWithValue("wabt", "N")
                'numsg.Parameters.AddWithValue("ist", DateTime.Now)
                'numsg.ExecuteNonQuery()

                Dim xb As Byte()
                Dim ePass As String = Addapp_User.ComputeHash(result.ToString(), "SHA512", xb)
                cmd_con_amend.Connection = con_insert
                cmd_con_amend.CommandText = "amend_system_user"
                cmd_con_amend.CommandType = CommandType.StoredProcedure
                cmd_con_amend.Parameters.AddWithValue("nupass", ePass)
                cmd_con_amend.Parameters.AddWithValue("flag", "Y")
                cmd_con_amend.Parameters.AddWithValue("myuser", amendusr)
                cmd_con_amend.Parameters.AddWithValue("mylastdt", DateTime.Now)
                cmd_con_amend.ExecuteScalar()

                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Outbox posted');</script>", False)


                con_insert.Close()
                con_insert.Dispose()

                'MessageBox.Show("User Reset Succesfully and sent by mail")
                Addapp_User.Send_My_Mail(emaal_add, "User Password Reset", "your password for the customer service portal has been Reset to: " + result.ToString())

                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('User Reset Succesfully and credentials sent by mail');</script>", False)
                Response.Redirect("~/Default.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.Message & " ');</script>", False)
            Finally
                con_insert.Dispose()
            End Try
        End Using

    End Sub

    <WebMethod()> Public Shared Function GetSearch(ByVal prefixText As String) As List(Of String)
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim customer_reader As MySql.Data.MySqlClient.MySqlDataReader
            Dim customer_get As New MySql.Data.MySqlClient.MySqlCommand
            Try
                con.Open()
                Dim customer_query As String
                customer_query = "Select Brid from Allowed_users where Brid like CONCAT(@usrbrid,'%')"
                customer_get.Connection = con
                customer_get.CommandText = customer_query
                customer_get.Parameters.AddWithValue("@usrbrid", prefixText)
                customer_reader = customer_get.ExecuteReader
                Dim dt As New DataTable()
                dt.Load(customer_reader)
                Dim Output As List(Of String) = New List(Of String)()

                For i As Integer = 0 To dt.Rows.Count - 1
                    Output.Add(dt.Rows(i)(0).ToString())
                Next

                Return Output
                customer_reader.Close()
                customer_reader.Dispose()
                con.Close()
            Catch ex As Exception
                HttpContext.Current.Response.Write("<script>alert('" & ex.Message & " ');</script>")
                'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.Message & " ');</script>", False)
            Finally
                con.Dispose()
            End Try
        End Using
    End Function


    Protected Sub BRID_TextChanged(sender As Object, e As EventArgs) Handles BRID.TextChanged
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim customer_reader As MySql.Data.MySqlClient.MySqlDataReader
            Dim customer_get As New MySql.Data.MySqlClient.MySqlCommand
            User_department.Text = vbNullString
            User_Email.Text = vbNullString
            User_Profile.Text = vbNullString
            User_Fname.Text = vbNullString
            User_Lname.Text = vbNullString

            Try
                con.Open()
                customer_get.Connection = con
                customer_get.CommandText = "Get_staff_Details"
                customer_get.CommandType = CommandType.StoredProcedure
                customer_get.Parameters.AddWithValue("Staff_Brid", BRID.Text)
                customer_reader = customer_get.ExecuteReader
                Dim dt As New DataTable()
                dt.Load(customer_reader)
                customer_reader.Close()
                customer_reader.Dispose()

                If dt.Rows.Count > 0 Then

                    User_department.Text = dt.Rows(0)(0).ToString()
                    User_Email.Text = dt.Rows(0)(1).ToString()
                    User_Profile.Text = dt.Rows(0)(2).ToString()
                    User_Fname.Text = dt.Rows(0)(3).ToString()
                    User_Lname.Text = dt.Rows(0)(4).ToString()
                    Signup_User.Enabled = True

                    con.Close()

                Else
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('User Brid does not exist');</script>", False)
                    con.Close()
                    Signup_User.Enabled = False
                    Exit Sub
                End If


            Catch ex As Exception
                'HttpContext.Current.Response.Write("<script>alert('" & ex.Message & " ');</script>")
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.Message & " ');</script>", False)
            Finally
                con.Dispose()
            End Try
        End Using
    End Sub

    Private Sub Signup_User_Click(sender As Object, e As EventArgs) Handles Signup_User.Click
        Dim thebrid As String = BRID.Text
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim Get_Registered_Cmd As New MySql.Data.MySqlClient.MySqlCommand

            Try
                con.Open()
                Get_Registered_Cmd.Connection = con
                Get_Registered_Cmd.CommandText = "Check_Staff"
                Get_Registered_Cmd.CommandType = CommandType.StoredProcedure
                Get_Registered_Cmd.Parameters.AddWithValue("usrnom", BRID.Text)
                Dim xint As Integer = CInt(Get_Registered_Cmd.ExecuteScalar())
                If xint > 0 Then
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('User already signed up');</script>", False)
                    Signup_User.Enabled = False
                    User_department.Text = vbNullString
                    User_Email.Text = vbNullString
                    User_Profile.Text = vbNullString
                    User_Fname.Text = vbNullString
                    User_Lname.Text = vbNullString
                    con.Close()
                    Exit Sub
                Else
                    Dim size As Integer = 8
                    Dim chars As Char() = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray()
                    Dim data As Byte() = New Byte(size - 1) {}

                    Using crypto As RNGCryptoServiceProvider = New RNGCryptoServiceProvider()
                        crypto.GetBytes(data)
                    End Using

                    Dim result As StringBuilder = New StringBuilder(size)

                    For Each b As Byte In data
                        result.Append(chars(b Mod (chars.Length)))
                    Next
                    Dim xb As Byte()
                    Dim ePass As String = Addapp_User.ComputeHash(result.ToString(), "SHA512", xb)

                    Dim addedusr As Integer
                    Dim cmd_con_insert_2 As New MySql.Data.MySqlClient.MySqlCommand
                    Dim cmd_con_insert As New MySql.Data.MySqlClient.MySqlCommand
                    cmd_con_insert_2.Connection = con
                    cmd_con_insert_2.CommandText = "Create_system_user"
                    cmd_con_insert_2.CommandType = CommandType.StoredProcedure
                    cmd_con_insert_2.Parameters.AddWithValue("First_Name_var", User_Fname.Text)
                    cmd_con_insert_2.Parameters.AddWithValue("Last_Name_var", User_Lname.Text)
                    cmd_con_insert_2.Parameters.AddWithValue("User_Name_var", BRID.Text)
                    cmd_con_insert_2.Parameters.AddWithValue("Email_Address_var", User_Email.Text)
                    cmd_con_insert_2.Parameters.AddWithValue("Mobile_Phone_var", "256417122402")
                    cmd_con_insert_2.Parameters.AddWithValue("Password_Var", ePass)
                    cmd_con_insert_2.Parameters.AddWithValue("lstchdt_Var", DateTime.Now)
                    cmd_con_insert_2.Parameters.AddWithValue("flagger_var", "Y")
                    addedusr = CInt(cmd_con_insert_2.ExecuteScalar())
                    'insert_query_2 = "Insert into User_Profile_Information (User_ID, User_Profile) Values (@User_ID_Var, @User_Profile_Var)"
                    cmd_con_insert.Connection = con
                    cmd_con_insert.CommandText = "Create_user_profile"
                    cmd_con_insert.CommandType = CommandType.StoredProcedure
                    cmd_con_insert.Parameters.AddWithValue("@User_ID_Var", addedusr)
                    cmd_con_insert.Parameters.AddWithValue("@User_Profile_Var", User_Profile.Text)
                    cmd_con_insert.ExecuteScalar()
                    Addapp_User.Send_My_Mail(User_Email.Text, "Profile Created", "your Profile on the customer service portal(https://UGUKKAMAPP0001/CustomerService) has been created with password: " + result.ToString() + " and User Name: " + BRID.Text)
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('User Created succesfully. Details sent by mail');location.href = ' " + ResolveUrl("~/Default.aspx") + "';</script>", False)
                    'Response.Redirect("~/Default.aspx", False)
                    'Context.ApplicationInstance.CompleteRequest()
                    con.Close()
                End If


            Catch ex As Exception
                'HttpContext.Current.Response.Write("<script>alert('" & ex.Message & " ');</script>")
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.Message & " ');</script>", False)
            Finally
                con.Dispose()
            End Try
        End Using

    End Sub

    Private Sub Logout_User_Click(sender As Object, e As EventArgs) Handles Logout_User.Click
        Dim amendusr As String
        If UserName_logout.Text = vbNullString Then
            'MessageBox.Show("Please input User Name to Reset")
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please input User Name To Reset ');</script>", False)
            Exit Sub
        Else
            amendusr = UserName_logout.Text
            Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
            Using con_insert As New MySql.Data.MySqlClient.MySqlConnection(constr)
                Dim cmd_con_amend, con_lookup As New MySql.Data.MySqlClient.MySqlCommand
                Try
                    con_insert.Open()
                    con_lookup.Connection = con_insert
                    con_lookup.CommandText = "Get_Loggin_Status"
                    con_lookup.CommandType = CommandType.StoredProcedure
                    con_lookup.Parameters.AddWithValue("usname", amendusr)
                    Using user_lookup As MySql.Data.MySqlClient.MySqlDataReader = con_lookup.ExecuteReader()
                        Dim flexiread As New DataTable
                        flexiread.Load(user_lookup)

                        If flexiread.Rows.Count = 0 Then
                            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Invalid User Name or User not registered');</script>", False)
                            user_lookup.Close()
                            user_lookup.Dispose()
                            con_insert.Close()
                            con_insert.Dispose()
                            Exit Sub
                        Else
                            If CInt(flexiread.Rows(0)("Isloggedon")) = 0 Then
                                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('User is not logged in');</script>", False)
                                user_lookup.Close()
                                user_lookup.Dispose()
                                con_insert.Close()
                                con_insert.Dispose()
                                Exit Sub
                            End If
                        End If
                        user_lookup.Close()
                        user_lookup.Dispose()
                    End Using

                    Dim login_status As New MySql.Data.MySqlClient.MySqlCommand
                    'update status to logged off
                    login_status.Connection = con_insert
                    login_status.CommandText = "Set_Loggin_Status"
                    login_status.CommandType = CommandType.StoredProcedure
                    login_status.Parameters.AddWithValue("usname", amendusr)
                    login_status.Parameters.AddWithValue("logsts", 0)
                    login_status.ExecuteScalar()
                    con_insert.Close()
                    Session.Clear()
                    Session.Abandon()
                    con_insert.Close()
                    'MessageBox.Show("User Reset Succesfully and sent by mail")
                    UserName_logout.Text = vbNullString
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('User Logged out succesfully');</script>", False)
                Catch ex As Exception
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.Message & " ');</script>", False)
                Finally
                    con_insert.Dispose()
                End Try
            End Using

        End If
    End Sub



    Function LogUserOut(user As String) As Boolean
        Dim amendusr As String = user
        Dim result As Boolean = False
        If amendusr = vbNullString Then
            'MessageBox.Show("Please input User Name to Reset")
            'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please input User Name To Reset ');</script>", False)

        Else
            Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
            Using con_insert As New MySql.Data.MySqlClient.MySqlConnection(constr)
                Dim cmd_con_amend, con_lookup As New MySql.Data.MySqlClient.MySqlCommand
                Try
                    con_insert.Open()
                    con_lookup.Connection = con_insert
                    con_lookup.CommandText = "Get_Loggin_Status"
                    con_lookup.CommandType = CommandType.StoredProcedure
                    con_lookup.Parameters.AddWithValue("usname", user)
                    Using user_lookup As MySql.Data.MySqlClient.MySqlDataReader = con_lookup.ExecuteReader()
                        Dim flexiread As New DataTable
                        flexiread.Load(user_lookup)

                        If flexiread.Rows.Count = 0 Then
                            'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Invalid User Name or User not registered');</script>", False)
                            user_lookup.Close()
                            user_lookup.Dispose()
                            con_insert.Close()
                            con_insert.Dispose()
                            result = False
                        Else
                            If CInt(flexiread.Rows(0)("Isloggedon")) = 0 Then
                                'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('User is not logged in');</script>", False)
                                user_lookup.Close()
                                user_lookup.Dispose()
                                con_insert.Close()
                                con_insert.Dispose()
                                result = False
                            End If
                        End If
                        user_lookup.Close()
                        user_lookup.Dispose()
                    End Using

                    Dim login_status As New MySql.Data.MySqlClient.MySqlCommand
                    'update status to logged off
                    con_insert.Close()
                    con_insert.Dispose()
                    con_insert.Open()
                    login_status.Connection = con_insert
                    login_status.CommandText = "Set_Loggin_Status"
                    login_status.CommandType = CommandType.StoredProcedure
                    login_status.Parameters.AddWithValue("usname", amendusr)
                    login_status.Parameters.AddWithValue("logsts", 0)
                    login_status.ExecuteScalar()
                    con_insert.Close()
                    Session.Clear()
                    Session.Abandon()
                    con_insert.Close()
                    'MessageBox.Show("User Reset Succesfully and sent by mail")
                    UserName_logout.Text = vbNullString
                    'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('User Logged out succesfully');</script>", False)
                    result = True
                Catch ex As Exception
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.Message & " ');</script>", False)
                Finally
                    con_insert.Dispose()
                End Try
            End Using

        End If
        Return result
    End Function

    Protected Sub LoginButton_Click(sender As Object, e As EventArgs)

    End Sub
End Class