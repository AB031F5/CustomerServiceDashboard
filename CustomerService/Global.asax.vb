
Option Strict On
Option Explicit On
Option Infer Off
Imports System.Web.SessionState
Imports System.Windows.Forms
Imports System.Web.Security
Imports System.Security.Principal
Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
        If HttpContext.Current.User IsNot Nothing Then
            If HttpContext.Current.User.Identity.IsAuthenticated Then
                If TypeOf HttpContext.Current.User.Identity Is FormsIdentity Then
                    Dim id As FormsIdentity = DirectCast(HttpContext.Current.User.Identity, FormsIdentity)
                    Dim ticket As FormsAuthenticationTicket = id.Ticket
                    Dim userData As String = ticket.UserData
                    Dim roles As String() = userData.Split(","c)
                    HttpContext.Current.User = New GenericPrincipal(id, roles)
                End If
            End If
        End If
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub
    Sub Session_onEnd(ByVal sender As Object, ByVal e As EventArgs)
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
                'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.Message & " ');</script>", False)
            Finally
                con.Dispose()
            End Try
        End Using
        Try
            Session.Clear()
            Session.Abandon()
            Response.Redirect("~/Login.aspx", True)
            Context.ApplicationInstance.CompleteRequest()
        Catch ex As Exception

        End Try

    End Sub

    Sub Application_logout(ByVal sender As Object, ByVal e As EventArgs)
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
                'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.Message & " ');</script>", False)
            Finally
                con.Dispose()
            End Try
        End Using
        Session.Clear()
        Session.Abandon()
    End Sub

End Class