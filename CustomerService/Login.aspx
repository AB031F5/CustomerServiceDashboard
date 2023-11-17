<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="Login.aspx.vb" Inherits="CustomerService.Login" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../Content/Helper/css/style.default.css" rel="stylesheet" media="screen" runat="server"/>
    <link href="../Content/Helper/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet" media="screen" runat="server" />
    <script type="text/javascript">
        function doPostBack(t) {
            if (t.value != "") {
                __doPostBack(t.name, "");
            }
        }

    </script> 
 <asp:ScriptManager ID="ScriptManager2" runat="server" EnablePageMethods="true"></asp:ScriptManager>
<div class=" page-holder d-flex align-items-center">
    <div class="container">
        <div class="row d-flex justify-content-center align-items-center h-100">
            <div class="col-5 col-lg-7 mx-auto mb-5 mb-lg-0">

                <div class="pr-lg-5">
                    <img src="illustration.svg" alt="" class="img-fluid" />     
                </div>
            </div>
            <div id="infor" class="card-body p-md-5 mx-md-4">
                <h1 class="text-base text-danger text-uppercase mb-4">CEX Dashboard</h1>
                <h2 class="mb-4">Welcome Back!</h2>

                <div class ="form-outline mb-4">
                    <asp:TextBox ID="email" required= "true" CssClass="form-control border-0 shadow form-control-lg text-base" placeholder="User Name" runat="server" ></asp:TextBox>

                </div>

                     <div class ="form-outline mb-4">
                    <asp:TextBox ID="pass" required= "true" TextMode="Password" CssClass="form-control border-0 shadow form-control-lg text-base" placeholder="Password" runat="server" ></asp:TextBox>

                </div>
                <div class="text-center pt-1 mb-5 pb-1">
                    <asp:Button Text="LOGIN" CssClass="btn btn-danger btn-block fa-lg gradient-custom-2 mb-3" Height="45px" runat="server" OnClick="testLogin_Click" ID="testLogin"/>
                    </div>
            </div>
        </div>
    </div>

</div>

            <div hidden="hidden">
                <p>
                    <asp:Login ID="Login1" runat="server" OnAuthenticate="Login1_Authenticate" BackColor="#F7F7DE" BorderColor="#CCCC99" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="10pt" Width="383px" BorderPadding="0" DestinationPageUrl="~/Home_RM.aspx">
                        <LayoutTemplate>
                            <table cellpadding="10" cellspacing="0" style="border: thin solid #CC0000; border-collapse:collapse; background-color: #9999FF;">
                                <tr>
                                    <td>
                                        <table cellpadding="6" style="width:383px;">
                                            <tr>
                                                <td align="center" colspan="2" style="color:#FF0000; background-color:#FFFFFF; font-weight:bold; height: 35px">Log on</td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="height: 35px;width:80px">
                                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                                </td>
                                                <td style="height: 35px;width:120px">
                                                    <asp:TextBox ID="UserName" runat="server" style="width:120px" AutoCompleteType="Disabled"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="height: 35px; Width:80px;">
                                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                                </td>
                                                <td style="height: 35px;width:120px">
                                                    <asp:TextBox ID="Password" runat="server" style="width:120px" TextMode="Password"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2" style="color:Red;">
                                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" colspan="2">
                                                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" ValidationGroup="Login1" ShowRoundedCorner="true" ForeColor="White" BackColor="#FF6600" OnClick="LoginButton_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <TitleTextStyle BackColor="#6B696B" Font-Bold="True" ForeColor="#FFFFFF" />
                    </asp:Login>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </p>
            </div>
            <div style="text-align: center">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" Height="43px" style="text-align: left" ValidationGroup="Login1" Width="255px" />
            </div>

            <div hidden="hidden" style="width:380px; border-radius: 10px; padding-left: 10px; padding-right: 10px; padding-top: 5px; padding-bottom: 5px; border: thin solid blue; font-size:smaller">
                <p> <asp:CheckBox ID="CheckBox1" runat="server" Checked="false" Oncheckchanged="CheckBox1_CheckedChanged" AutoPostBack="true"/>
                    Reset user credentials
                </p>
                <p>
                    <asp:Label ID="usernamelabel" style="                            width: 80px;
                            display: inline-block" runat="server" AssociatedControlID="UserName">User Name</asp:Label>
                    <asp:TextBox ID="UserName" style="width:130px" runat="server" Enabled="false" AutoCompleteType="Disabled"></asp:TextBox>  
                    <asp:Button ID="ResetUser" style="width:100px" runat="server" Text="Reset"  Enabled="false"/>
                </p>
            </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" hidden="hidden">
        <ContentTemplate>

                <p style="font-size:smaller"> <asp:CheckBox ID="CheckBox3" runat="server" Checked="false" Oncheckchanged="CheckBox2_CheckedChanged" AutoPostBack="true"/>
                        Log me out
                </p>

                <div ID="Logout_DIV" runat="server" visible="false" style="width:380px; border-radius: 10px; padding-left: 10px; padding-right: 10px; padding-top: 5px; padding-bottom: 5px; border: thin solid blue; font-size:smaller">                
                    <p>
                        <asp:Label ID="Label7" style="width:80px;display: inline-block" runat="server" AssociatedControlID="UserName_logout">User Name</asp:Label>
                        <asp:TextBox ID="UserName_logout" style="width:130px" runat="server" Enabled="false" AutoCompleteType="Disabled"></asp:TextBox>  
                        <asp:Button ID="Logout_User" style="width:100px" runat="server" Text="Logout"  Enabled="false"/>
                    </p>              
                </div>
         </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel4" runat="server" hidden="hidden">
    <ContentTemplate>

            <p style="font-size:smaller"> <asp:CheckBox ID="CheckBox2" runat="server" Checked="false" Oncheckchanged="CheckBox2_CheckedChanged" AutoPostBack="true"/>
                    Sign Up
            </p>

            <div ID="Register_DIV" runat="server" visible="false" style="width:380px; border-radius: 10px; padding-left: 10px; padding-right: 10px; padding-top: 5px; padding-bottom: 5px; border: thin solid blue; font-size:smaller">                
                <p>
                    <asp:Label ID="Label1" style="width:130px;display: inline-block" runat="server" AssociatedControlID="BRID">AB Number</asp:Label>
                    <asp:TextBox ID="BRID" style="width:230px" runat="server" Enabled="true" OnTextChanged="BRID_TextChanged" AutoCompleteType="Disabled" onblur = "doPostBack(this)" AutoPostBack="true"></asp:TextBox>  
                    <cc1:AutoCompleteExtender ServiceMethod="GetSearch" MinimumPrefixLength="1" CompletionInterval="100" EnableCaching="false" CompletionSetCount="10" TargetControlID="BRID" ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false"></cc1:AutoCompleteExtender>  
                </p>
                <p>
                    <asp:Label ID="Label2" style="width:130px;display: inline-block" runat="server" AssociatedControlID="User_department">Department</asp:Label>
                    <asp:TextBox ID="User_department" style="width:230px" runat="server" Enabled="false"></asp:TextBox>  
                </p>
                <p>
                    <asp:Label ID="Label3" style="width:130px;display: inline-block" runat="server" AssociatedControlID="User_Email">Email Address</asp:Label>
                    <asp:TextBox ID="User_Email" style="width:230px" runat="server" Enabled="false"></asp:TextBox>  
                </p>
                <p>
                    <asp:Label ID="Label4" style="width:130px;display: inline-block" runat="server" AssociatedControlID="User_Profile">Profile</asp:Label>
                    <asp:TextBox ID="User_Profile" style="width:230px" runat="server" Enabled="false"></asp:TextBox>  
                </p>
                <p>
                    <asp:Label ID="Label5" style="width:130px;display: inline-block" runat="server" AssociatedControlID="User_Fname">First Name</asp:Label>
                    <asp:TextBox ID="User_Fname" style="width:230px" runat="server" Enabled="false"></asp:TextBox>  
                </p>
                <p>
                    <asp:Label ID="Label6" style="width:130px;display: inline-block" runat="server" AssociatedControlID="User_Lname">Last Name</asp:Label>
                    <asp:TextBox ID="User_Lname" style="width:230px" runat="server" Enabled="false"></asp:TextBox>  
                </p>
                <p style="width:360px; align-content:end">
                    <asp:Button ID="Signup_User" style="width:100px" runat="server" Text="Sign me Up"  Enabled="false"/>
                </p>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>
