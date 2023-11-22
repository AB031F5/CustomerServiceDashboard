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
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class CompletedInstructions
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub processup_Click(sender As Object, e As EventArgs) Handles processup.Click
        Dim msgout As String = "Dear customer, we have completed processing your instruction today. For any clarifications, visit your nearest branch  or call 080022333/0312218348"
        If fupload.HasFile Then
            Using reader = New StreamReader(fupload.FileContent)
                Using csv = New CsvReader(reader, CultureInfo.InvariantCulture)
                    'csv.Configuration.Delimiter = ","
                    Dim records = csv.GetRecords(Of Foo2xx)()
                    Dim kout As DataTable = ToDataTable2xx(records)
                    Dim returntable As String = ConvertDataTableToHTML2xx(kout)
                    PlaceHolder1.Controls.Add(New Literal() With {
                  .Text = returntable.ToString()
                  })
                    Dim sCommand As StringBuilder = New StringBuilder("insert into acctmaintenance (Branch ,  Case_No , Sequence , Account_Number , Customer_ID , Sub_Product , Current_Queue , Case_Status , initiation_Date) values ")
                    Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
                    Using con_insert As New MySql.Data.MySqlClient.MySqlConnection(constr)
                        Dim cmd_add_comms, cmd_check_unprocessed As New MySql.Data.MySqlClient.MySqlCommand
                        Try
                            con_insert.Open()

                            Dim Rowz As List(Of String) = New List(Of String)()
                            Dim i, j As Integer
                            For i = 0 To kout.Rows.Count - 1
                                Rowz.Add(String.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                                                       MySqlHelper.EscapeString(kout.Rows(i)(0)), MySqlHelper.EscapeString(kout.Rows(i)(1)),
                                                       MySqlHelper.EscapeString(kout.Rows(i)(2)), MySqlHelper.EscapeString(kout.Rows(i)(3)),
                                                       MySqlHelper.EscapeString(kout.Rows(i)(4)), MySqlHelper.EscapeString(kout.Rows(i)(5)),
                                                       MySqlHelper.EscapeString(kout.Rows(i)(6)), MySqlHelper.EscapeString(kout.Rows(i)(7)),
                                                       MySqlHelper.EscapeString(kout.Rows(i)(8))))
                            Next
                            sCommand.Append(String.Join(",", Rowz))
                            sCommand.Append(";")


                            cmd_add_comms.Connection = con_insert
                            cmd_add_comms.CommandText = sCommand.ToString
                            cmd_add_comms.ExecuteNonQuery()
                            con_insert.Close()
                            Dim messagedisplay As String = String.Format("file with {0} records has been succesfully uploaded", kout.Rows.Count.ToString())
                            lblerror.Text = messagedisplay
                            fileuploadhist2xx(fupload.FileName, Session("User_Name"), kout.Rows.Count)
                            For j = 0 To kout.Rows.Count - 1
                                Dim recepacct, recepphone, recepcase As String
                                recepacct = vbNullString
                                recepphone = vbNullString
                                recepcase = vbNullString
                                recepacct = kout.Rows(j)(3).ToString()
                                recepcase = kout.Rows(j)(1).ToString()
                                recepphone = InvokeServiceMTN(recepacct)
                                If recepphone <> vbNullString Then
                                    If recepphone.Substring(0, 1) = "0" Then
                                        recepphone = "256" & recepphone.Substring(1, recepphone.Length - 1)
                                    Else
                                        recepphone = "256" & recepphone
                                    End If
                                    If recepphone.Length > 5 Then
                                        'Dim sendstatus As String = sendsms(recepphone, msgout)
                                        addcomstodb("SMS", recepphone, msgout, DateTime.Now, "Case Completion", recepcase, recepacct)
                                    Else
                                        addinvalid(recepacct, "Y", "N")
                                    End If
                                Else
                                    addinvalid(recepacct, "Y", "N")
                                End If
                            Next
                        Catch ex As Exception
                            lblerror.Text = ex.Message
                        End Try
                    End Using
                End Using
            End Using
        Else
            lblerror.Text = "No file to upload"
        End If

    End Sub

    Private Function ConvertDataTableToHTML2xx(dataTable As DataTable) As String
        Dim htmlStringBuilder As New StringBuilder()

        htmlStringBuilder.AppendLine("<table id='table_id' class='display cell-border hover stripe' style='font-size: 11pt;font-family:Roboto; width:100%;border-radius:5px;overflow:hidden;'>")

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
    Public Class Foo2xx
        Public Property f1 As String
        Public Property f2 As String
        Public Property f3 As String
        Public Property f4 As String
        Public Property f5 As String
        Public Property f6 As String
        Public Property f7 As String
        Public Property f8 As String
        Public Property f9 As String
    End Class

    Public Shared Function ToDataTable2xx(Of T)(ByVal self As IEnumerable(Of T)) As DataTable
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

    Public Sub fileuploadhist2xx(fnomer As String, uploader As String, filecount As Integer)
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con_ffiiles As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim cmd_updatefiles As New MySql.Data.MySqlClient.MySqlCommand
            Try
                con_ffiiles.Open()
                cmd_updatefiles.CommandText = "fileuploadhistory2xx"
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

    Public Function sendsms(ByVal msisdn As String, ByVal smscontent As String) As String
        Dim url As String = "http://nickel.trueafrican.com/esme.php"
        Dim feedBack As String = ""
        Dim UserName As String = "barclaysCC"
        Dim Password As String = "BarC735Hdk"
        Dim receiver As String = msisdn
        Dim message As String = smscontent
        Dim parameters As String = "USERNAME=" & UserName & "&PASSWORD=" & Password
        parameters += "&MSISDN=" & receiver & "&MESSAGE=" & message

        Try
            Dim r As HttpWebRequest = CType(System.Net.WebRequest.Create(url & "?" & parameters), HttpWebRequest)
            r.Headers.Clear()
            r.KeepAlive = False
            r.ContentType = "application / x - www - form - urlencoded"
            r.Credentials = CredentialCache.DefaultCredentials
            r.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)"
            r.Timeout = 150000
            r.Timeout = 100000
            Dim byteArray As Encoding = Encoding.GetEncoding("utf-8")
            Dim dataStream As Stream
            Dim response As WebResponse = CType(r.GetResponse(), HttpWebResponse)
            dataStream = response.GetResponseStream()
            Dim rdr As StreamReader = New StreamReader(dataStream)
            feedBack = rdr.ReadToEnd()

            If feedBack.ToUpper().Contains("SUCCESS") Then
            Else
            End If

        Catch ee As Exception
        End Try

        Return feedBack
    End Function

    Public Function InvokeServiceMTN(actor As String) As String
        Dim phoneemail As String
        phoneemail = vbNullString
        Try
            Dim request As HttpWebRequest = CreateSOAPWebRequest()
            Dim SOAPReqBody As XmlDocument = New XmlDocument()
            SOAPReqBody.LoadXml("<?xml version=""1.0"" encoding=""utf-8""?>  
            <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-   instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">  
             <soap:Body>  
                <GetCustomer xmlns=""http://tempuri.org/"">  
                  <ApiUsername>Api</ApiUsername>  
                  <ApiPassword>C0ld@f33xnes</ApiPassword> 
                  <AccountNumber>" & actor & "</AccountNumber>
                </GetCustomer>  
              </soap:Body>  
            </soap:Envelope>")

            Using stream As Stream = request.GetRequestStream()
                SOAPReqBody.Save(stream)
            End Using

            Using Serviceres As WebResponse = request.GetResponse()

                Using rd As StreamReader = New StreamReader(Serviceres.GetResponseStream())
                    Dim ServiceResult = rd.ReadToEnd()
                    Dim Xml = System.Xml.Linq.XElement.Parse(ServiceResult)
                    'lblerror.Text = Xml.Elements.ToString()
                    Dim doc As New XmlDocument
                    doc.LoadXml(ServiceResult)
                    Dim jsonText As String = JsonConvert.SerializeXmlNode(doc)
                    Dim jObj = JObject.Parse(jsonText)
                    Dim callresut As String = jObj.SelectToken("s:Envelope.s:Body.GetCustomerResponse.GetCustomerResult.a:ErrorCode").ToString()
                    Dim calldescription As String = jObj.SelectToken("s:Envelope.s:Body.GetCustomerResponse.GetCustomerResult.a:ErrorDescription").ToString()
                    Dim jemail As String = jObj.SelectToken("s:Envelope.s:Body.GetCustomerResponse.GetCustomerResult.a:EmailAddress").ToString()
                    Dim jphone As String = jObj.SelectToken("s:Envelope.s:Body.GetCustomerResponse.GetCustomerResult.a:PhoneNumber").ToString()
                    phoneemail = jphone

                End Using

            End Using
        Catch ex As Exception
            Dim errortext As String = ex.Message
        End Try
        Return phoneemail

    End Function

    Public Function CreateSOAPWebRequest() As HttpWebRequest
        Dim myUri As Uri = New Uri("http://ugpbhkmapp000f:8082/ConnnectPKApi/mex", UriKind.Absolute)
        Dim Req As HttpWebRequest = CType(WebRequest.Create(myUri), HttpWebRequest)
        Req.Headers.Add("SOAPAction:http://tempuri.org/IConnectPKServiceApi/GetCustomer")
        Req.ContentType = "text/xml;charset=""utf-8"""
        Req.Accept = "text/xml"
        Req.Method = "POST"
        Return Req

    End Function

    Public Sub addcomstodb(cctype As String, recep As String, cmsg As String, csd As DateTime, craison As String, cccomplaint As String, cccact As String)
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con_insert As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim cmd_add_comms As New MySql.Data.MySqlClient.MySqlCommand
            Try
                con_insert.Open()
                cmd_add_comms.Connection = con_insert
                cmd_add_comms.CommandText = "addcomsacctmaint"
                cmd_add_comms.CommandType = CommandType.StoredProcedure
                cmd_add_comms.Parameters.AddWithValue("ccomtype", cctype)
                cmd_add_comms.Parameters.AddWithValue("cbeneficiary", recep)
                cmd_add_comms.Parameters.AddWithValue("ccomms", cmsg)
                cmd_add_comms.Parameters.AddWithValue("csent_date", csd)
                cmd_add_comms.Parameters.AddWithValue("csend_reason", craison)
                cmd_add_comms.Parameters.AddWithValue("zeecmplt", cccomplaint)
                cmd_add_comms.Parameters.AddWithValue("custacc", cccact)
                cmd_add_comms.ExecuteNonQuery()
                con_insert.Close()
            Catch ex As Exception

            Finally

                con_insert.Dispose()

            End Try
        End Using
    End Sub

    Public Sub addinvalid(acctnumber As String, invalph As String, invalem As String)
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con_fake As New MySql.Data.MySqlClient.MySqlConnection(constr)
            Dim cmd_add_comms As New MySql.Data.MySqlClient.MySqlCommand
            Try
                con_fake.Open()
                cmd_add_comms.Connection = con_fake
                cmd_add_comms.CommandText = "addinvalidcontactmaint"
                cmd_add_comms.CommandType = CommandType.StoredProcedure
                cmd_add_comms.Parameters.AddWithValue("acctnum", acctnumber)
                cmd_add_comms.Parameters.AddWithValue("phinvalid", invalph)
                cmd_add_comms.Parameters.AddWithValue("email_invalid", invalem)

                cmd_add_comms.ExecuteNonQuery()
                con_fake.Close()
            Catch ex As Exception

            Finally

                con_fake.Dispose()
            End Try
        End Using
    End Sub


End Class