Public Class Site1
    Inherits System.Web.UI.MasterPage

    Private Const AntiXsrfTokenKey As String = "__AntiXsrfToken"
    Private Const AntiXsrfUserNameKey As String = "__AntiXsrfUserName"
    Private _antiXsrfTokenValue As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub




    Protected Sub OnMenuItemDataBound(sender As Object, e As MenuEventArgs)
        If SiteMap.CurrentNode IsNot Nothing Then
            If e.Item.Text = SiteMap.CurrentNode.Title Then
                'If e.Item.Parent IsNot Nothing Then
                'e.Item.Parent.Selected = True
                'Else
                'e.Item.Selected = True
                'End If
            End If
        End If
    End Sub

    Protected Sub btnLogout_Click(sender As Object, e As System.EventArgs)
        Dim User_Name As String = Session("User_Name").ToString()
        LogUserOut(User_Name)
        FormsAuthentication.SignOut()
        Response.Redirect("~/Default.aspx")
    End Sub

    Protected Sub LoginStatus1_LoggedOut(ByVal sender As Object, ByVal e As System.EventArgs)
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
    End Sub

    Private Sub Site1_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim requestCookie = Request.Cookies(AntiXsrfTokenKey)
        Dim requestCookieGuidValue As Guid

        If requestCookie IsNot Nothing AndAlso Guid.TryParse(requestCookie.Value, requestCookieGuidValue) Then
            _antiXsrfTokenValue = requestCookie.Value
            Page.ViewStateUserKey = _antiXsrfTokenValue
        Else
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N")
            Page.ViewStateUserKey = _antiXsrfTokenValue
            Dim responseCookie = New HttpCookie(AntiXsrfTokenKey) With {
                .HttpOnly = True,
                .Value = _antiXsrfTokenValue
            }
            If FormsAuthentication.RequireSSL AndAlso Request.IsSecureConnection Then responseCookie.Secure = True
            Response.Cookies.[Set](responseCookie)
        End If

        AddHandler Page.PreLoad, AddressOf Site1_PreLoad

    End Sub


    Protected Sub Site1_PreLoad(ByVal sender As Object, ByVal e As EventArgs)
        If Not IsPostBack Then
            ViewState(AntiXsrfTokenKey) = Page.ViewStateUserKey
            ViewState(AntiXsrfUserNameKey) = If(Context.User.Identity.Name, String.Empty)
        Else

            If CStr(ViewState(AntiXsrfTokenKey)) <> _antiXsrfTokenValue OrElse CStr(ViewState(AntiXsrfUserNameKey)) <> (If(Context.User.Identity.Name, String.Empty)) Then
                Throw New InvalidOperationException("Validation of Anti - XSRF token failed")
            End If
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
                    'Session.Clear()
                    'Session.Abandon()
                    con_insert.Close()
                    'MessageBox.Show("User Reset Succesfully and sent by mail")
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

End Class