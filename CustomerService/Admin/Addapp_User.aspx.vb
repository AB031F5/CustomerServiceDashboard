Option Strict On
Option Explicit On
Option Infer Off
Imports System.Windows.Forms
Imports System.Net.Mail
Imports System.Net
Imports System.Security.Cryptography
Imports MySql.Data.MySqlClient
Imports System.IO
Public Class Addapp_User
    Inherits System.Web.UI.Page

    Public Shared Function Send_My_Mail(ByVal mreciepient As String, ByVal sujet As String, ByVal mbody As String) As String
        'Dim path As String = "\\22.68.25.56\c:\Users\ABBK366-ADMIN\Desktop\MyTest.txt"
        'Dim path As String = Server.MapPath("Folder") + "\\anifile.txt"

        'If Not File.Exists(path) Then
        '    Dim createText As String = "Hello and Welcome" & Environment.NewLine
        '    File.WriteAllText(path, createText)
        'End If

        'Dim appendText As String = "This is extra text" & Environment.NewLine


        Try
            Dim SmtpServer As New SmtpClient()
            Dim mail As New MailMessage()
            SmtpServer.UseDefaultCredentials = False
            mail = New MailMessage()
            mail.To.Add(mreciepient)
            mail.Subject = sujet
            mail.Body = mbody
            SmtpServer.Send(mail)
            'MessageBox.Show("mail sent")
            'File.AppendAllText(path, "mail sent")
        Catch ex As Exception
            'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.ToString & " ');</script>", False)
            'MsgBox(ex.ToString)
            'File.AppendAllText(path, ex.ToString())
        End Try
    End Function

    Public Shared Function Send_My_Mail_Attachment(ByVal mreciepient As String, ByVal sujet As String, ByVal mbody As String, ByVal att As Attachment) As String
        Try
            Dim SmtpServer As New SmtpClient()
            Dim mail As New MailMessage()
            SmtpServer.UseDefaultCredentials = True
            mail = New MailMessage()
            mail.To.Add(mreciepient)
            mail.Subject = sujet
            mail.Body = mbody
            mail.Attachments.Add(att)
            SmtpServer.Send(mail)
            'MessageBox.Show("mail sent")
        Catch ex As Exception
            'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.ToString & " ');</script>", False)
            'MsgBox(ex.ToString)
        End Try
    End Function

    Public Shared Function Send_My_SMS() As String
        Try
            Dim fr As HttpWebRequest
            Dim targetURI As New Uri("http://nickel.trueafrican.com/esme.php?USERNAME=barclaysCC&PASSWORD=BarC735Hdk&MSISDN=256776754012&MESSAGE=Jesus the Christ")

            fr = DirectCast(HttpWebRequest.Create(targetURI), System.Net.HttpWebRequest)
            'fr.Credentials = CredentialCache.DefaultCredentials
            fr.Proxy.Credentials = CredentialCache.DefaultCredentials
            'fr.Method = "POST"
            If (fr.GetResponse().ContentLength > 0) Then
                MessageBox.Show("Am in here")
                Dim str As New System.IO.StreamReader(fr.GetResponse().GetResponseStream())
                'Response.Write(str.ReadToEnd())
                str.Close()
            Else
                MessageBox.Show("Am in now here")
            End If
            MessageBox.Show(CType(fr.GetResponse().ContentLength, String))

            'Dim str As New System.IO.StreamReader(fr.GetResponse().GetResponseStream())
            'Response.Write(str.ReadToEnd())
            'str.Close()

        Catch ex As System.Net.WebException
            'Error in accessing the resource, handle it
        End Try
    End Function

    Public Shared Function ComputeHash(ByVal plainText As String, ByVal hashAlgorithm As String, ByVal saltBytes As Byte()) As String
        If saltBytes Is Nothing Then
            Dim minSaltSize As Integer = 4
            Dim maxSaltSize As Integer = 8
            Dim random As Random = New Random()
            Dim saltSize As Integer = random.[Next](minSaltSize, maxSaltSize)
            saltBytes = New Byte(saltSize - 1) {}
            Dim rng As RNGCryptoServiceProvider = New RNGCryptoServiceProvider()
            rng.GetNonZeroBytes(saltBytes)
        End If

        Dim plainTextBytes As Byte() = Encoding.UTF8.GetBytes(plainText)
        Dim plainTextWithSaltBytes As Byte() = New Byte(plainTextBytes.Length + saltBytes.Length - 1) {}

        For i As Integer = 0 To plainTextBytes.Length - 1
            plainTextWithSaltBytes(i) = plainTextBytes(i)
        Next

        For i As Integer = 0 To saltBytes.Length - 1
            plainTextWithSaltBytes(plainTextBytes.Length + i) = saltBytes(i)
        Next

        Dim hash As HashAlgorithm
        If hashAlgorithm Is Nothing Then hashAlgorithm = ""

        Select Case hashAlgorithm.ToUpper()
            Case "SHA384"
                hash = New SHA384Managed()
            Case "SHA512"
                hash = New SHA512Managed()
            Case Else
                hash = New MD5CryptoServiceProvider()
        End Select

        Dim hashBytes As Byte() = hash.ComputeHash(plainTextWithSaltBytes)
        Dim hashWithSaltBytes As Byte() = New Byte(hashBytes.Length + saltBytes.Length - 1) {}

        For i As Integer = 0 To hashBytes.Length - 1
            hashWithSaltBytes(i) = hashBytes(i)
        Next

        For i As Integer = 0 To saltBytes.Length - 1
            hashWithSaltBytes(hashBytes.Length + i) = saltBytes(i)
        Next

        Dim hashValue As String = Convert.ToBase64String(hashWithSaltBytes)
        Return hashValue
    End Function

    Public Shared Function VerifyHash(ByVal plainText As String, ByVal hashAlgorithm As String, ByVal hashValue As String) As Boolean
        Dim hashWithSaltBytes As Byte() = Convert.FromBase64String(hashValue)
        Dim hashSizeInBits, hashSizeInBytes As Integer
        If hashAlgorithm Is Nothing Then hashAlgorithm = ""

        Select Case hashAlgorithm.ToUpper()
            Case "SHA384"
                hashSizeInBits = 384
            Case "SHA512"
                hashSizeInBits = 512
            Case Else
                hashSizeInBits = 128
        End Select

        hashSizeInBytes = CInt(hashSizeInBits / 8)
        If hashWithSaltBytes.Length < hashSizeInBytes Then Return False
        Dim saltBytes As Byte() = New Byte(hashWithSaltBytes.Length - hashSizeInBytes - 1) {}

        For i As Integer = 0 To saltBytes.Length - 1
            saltBytes(i) = hashWithSaltBytes(hashSizeInBytes + i)
        Next

        Dim expectedHashString As String = ComputeHash(plainText, hashAlgorithm, saltBytes)
        Return (hashValue = expectedHashString)
    End Function

    Public Shared Function checkbrid(ByVal supbrid As String) As Boolean
        Dim found As Boolean
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Dim con_check As New MySql.Data.MySqlClient.MySqlConnection(constr)
        Using cmd As MySql.Data.MySqlClient.MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand("Select Brid from Allowed_users where Brid=@Usernm", con_check)
            cmd.Parameters.AddWithValue("@Usernm", supbrid)
            Dim da As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
            Dim dt As DataTable = New DataTable()
            da.Fill(dt)

            If dt.Rows.Count = 0 Then
                found = True
            Else
                found = False
            End If
        End Using
        Return found
    End Function

    Public Shared Function checkbregd(ByVal supbridrgd As String) As Boolean
        Dim foundreg As Boolean
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Dim con_check As New MySql.Data.MySqlClient.MySqlConnection(constr)
        Using cmd As MySql.Data.MySqlClient.MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand("Select User_Name from System_Users where User_Name=@Usernm", con_check)
            cmd.Parameters.AddWithValue("@Usernm", supbridrgd)
            Dim da As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
            Dim dt As DataTable = New DataTable()
            da.Fill(dt)

            If dt.Rows.Count = 0 Then
                foundreg = True
            Else
                foundreg = False
            End If
        End Using
        Return foundreg
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
            Using con As New MySql.Data.MySqlClient.MySqlConnection(constr)
                Dim searchstage As MySql.Data.MySqlClient.MySqlDataReader
                Dim cmd_stage As New MySql.Data.MySqlClient.MySqlCommand

                Try
                    con.Open()
                    cmd_stage.Connection = con
                    cmd_stage.CommandText = "Select_Profiles"
                    cmd_stage.CommandType = CommandType.StoredProcedure
                    cmd_stage.Parameters.AddWithValue("condind", "Y")
                    searchstage = cmd_stage.ExecuteReader
                    userp.DataTextField = "Profile_Name"
                    userp.DataValueField = "Profile_ID"
                    userp.DataSource = searchstage
                    userp.DataBind()
                    userp.Items.Insert(0, New ListItem("--Select--", String.Empty))
                    con.Close()
                Catch ex As Exception
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.Message & " ');</script>", False)
                Finally
                    con.Dispose()
                End Try
            End Using
        End If
    End Sub

    Private Sub usrcaddcnc_Click(sender As Object, e As EventArgs) Handles usrcaddcnc.Click

    End Sub

    Private Sub usrcaddbtn_Click(sender As Object, e As EventArgs) Handles usrcaddbtn.Click
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        If userp.SelectedItem.Text = "--Select--" Then
            'MessageBox.Show("Please select a user profile Option")
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please select a user Profile');</script>", False)
            Exit Sub
        End If

        Dim addedusr As Integer
        Dim xb As Byte()
        Dim ePass As String = ComputeHash(passwd.Text, "SHA512", xb)
        Using con_insert As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim cmd_con_insert As New MySql.Data.MySqlClient.MySqlCommand
            Dim cmd_con_insert_2 As New MySql.Data.MySqlClient.MySqlCommand
            Try
                con_insert.Open()
                'Dim insert_query, insert_query_2 As String
                'insert_query = "Insert into System_Users (First_Name, Last_Name, User_Name,Email_Address,Mobile_Phone, Password, Lastpwdchangedate) values (@First_Name_var, @Last_Name_var,@User_Name_var,@Email_Address_var,@Mobile_Phone_var,@Password_Var, @lstchdt_Var);SELECT LAST_INSERT_ID()"
                cmd_con_insert_2.Connection = con_insert
                cmd_con_insert_2.CommandText = "Create_system_user"
                cmd_con_insert_2.CommandType = CommandType.StoredProcedure
                cmd_con_insert_2.Parameters.AddWithValue("First_Name_var", Fname.Text)
                cmd_con_insert_2.Parameters.AddWithValue("Last_Name_var", Lname.Text)
                cmd_con_insert_2.Parameters.AddWithValue("User_Name_var", uname.Text)
                cmd_con_insert_2.Parameters.AddWithValue("Email_Address_var", emadd.Text)
                cmd_con_insert_2.Parameters.AddWithValue("Mobile_Phone_var", msisdn.Text)
                cmd_con_insert_2.Parameters.AddWithValue("Password_Var", ePass)
                cmd_con_insert_2.Parameters.AddWithValue("lstchdt_Var", DateTime.Now)
                cmd_con_insert_2.Parameters.AddWithValue("flagger_var", "Y")
                addedusr = CInt(cmd_con_insert_2.ExecuteScalar())
                'insert_query_2 = "Insert into User_Profile_Information (User_ID, User_Profile) Values (@User_ID_Var, @User_Profile_Var)"
                cmd_con_insert.Connection = con_insert
                cmd_con_insert.CommandText = "Create_user_profile"
                cmd_con_insert.CommandType = CommandType.StoredProcedure
                cmd_con_insert.Parameters.AddWithValue("@User_ID_Var", addedusr)
                cmd_con_insert.Parameters.AddWithValue("@User_Profile_Var", userp.SelectedItem.Text)
                cmd_con_insert.ExecuteScalar()
                Addapp_User.Send_My_Mail(emadd.Text, "Profile Created", "your Profile on the Wholesale Lending system(http://BDSPUGD70849013/WholeSaleLending) has been created with password: " + passwd.Text + " and User Name: " + uname.Text)
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('User Created succesfully');location.href = ' " + ResolveUrl("~/Default.aspx") + "';</script>", False)
                'Response.Redirect("~/Default.aspx", False)
                'Context.ApplicationInstance.CompleteRequest()
                con_insert.Close()
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.Message & " ');</script>", False)
            Finally
                con_insert.Dispose()
            End Try
        End Using
    End Sub

    Private Sub uname_TextChanged(sender As Object, e As EventArgs) Handles uname.TextChanged
        'Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        'Dim con_check As New MySql.Data.MySqlClient.MySqlConnection(constr)
        'Using cmd As MySql.Data.MySqlClient.MySqlCommand = New MySql.Data.MySqlClient.MySqlCommand("Select User_Name from System_Users where User_Name=@Usernm", con_check)
        '    cmd.Parameters.AddWithValue("@Usernm", uname.Text)
        '    Dim da As MySql.Data.MySqlClient.MySqlDataAdapter = New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
        '    Dim dt As DataTable = New DataTable()
        '    da.Fill(dt)

        '    If dt.Rows.Count > 0 Then
        '        MessageBox.Show("User Name already in Use. Please select another Username")
        '        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('User Name already in Use. Please select another Username');</script>", False)
        '        uname.Text = vbNullString
        '        Response.Write("<script>alert('User Name already in Use. Please select another Username');</script>")
        '    End If
        'End Using
        Dim waschecked As Boolean = checkbrid(uname.Text)
        If waschecked = True Then
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Brid is not Allowed. Please enter brid allowed to access this system');</script>", False)
            uname.Text = vbNullString
        Else
            Dim wascheckedregd As Boolean = checkbregd(uname.Text)
            If wascheckedregd = False Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('User Name already in Use. Please select another Username');</script>", False)
                uname.Text = vbNullString
            Else
                Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
                Using con As New MySql.Data.MySqlClient.MySqlConnection(constr)
                    Dim cmd_found As New MySql.Data.MySqlClient.MySqlCommand
                    Dim foundemp As MySql.Data.MySqlClient.MySqlDataReader


                    Try
                        con.Open()
                        cmd_found.CommandText = "get_users"
                        cmd_found.Connection = con
                        cmd_found.CommandType = CommandType.StoredProcedure
                        cmd_found.Parameters.AddWithValue("empbrid", uname.Text)
                        foundemp = cmd_found.ExecuteReader()
                        While foundemp.Read
                            Fname.Text = foundemp.GetString(0)
                            Lname.Text = foundemp.GetString(1)
                            emadd.Text = foundemp.GetString(2)
                            msisdn.Text = foundemp.GetInt64(3).ToString()
                        End While
                        foundemp.Close()
                        foundemp.Dispose()
                        con.Close()
                    Catch ex As Exception
                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" & ex.Message & " ');</script>", False)
                    Finally
                        con.Dispose()
                    End Try

                End Using
            End If
        End If
    End Sub

    Private Sub userp_TextChanged(sender As Object, e As EventArgs) Handles userp.TextChanged

    End Sub

End Class