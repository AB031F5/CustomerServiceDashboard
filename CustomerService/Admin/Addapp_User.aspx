<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="Addapp_User.aspx.vb" Inherits="CustomerService.Addapp_User" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ValidationSummary id="valladduser" ValidationGroup="valladdusergp" runat="server" style="color:red; font-size:small" HeaderText="There were errors on the page:" />    
    <div style="width:788px; border-radius: 10px; padding-left: 10px; padding-right: 10px; padding-top: 5px; padding-bottom: 5px; border: thin solid blue; font-size:smaller">
        <p>
            <asp:Label ID="Label1_uname" style="width:120px;display: inline-block" runat="server" AssociatedControlID="uname">BRID</asp:Label>
            <asp:TextBox ID="uname" style="width:170px;border-radius:5px" runat="server" EnableTheming="True" AutoPostBack="true" AutoCompleteType="Disabled"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidatoruname" runat="server" ControlToValidate="uname" ErrorMessage="BRID is required." style="color:red; font-size:small" ValidationGroup="valladdusergp">*</asp:RequiredFieldValidator>  
            &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp
            <asp:Label ID="Label1_emadd" style="width:120px;display: inline-block" runat="server" AssociatedControlID="emadd">Email Address</asp:Label>
            <asp:TextBox ID="emadd" style="width:170px;border-radius:5px" runat="server" EnableTheming="True" TextMode="Email" Enabled="false"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidatoremadd" runat="server" ControlToValidate="emadd" ErrorMessage="Email Address is required." style="color:red; font-size:small" ValidationGroup="valladdusergp">*</asp:RequiredFieldValidator>  
        </p>
        <p>
            <asp:Label ID="Label1_fname" style="width:120px;display: inline-block" runat="server" AssociatedControlID="Fname">First Name</asp:Label>
            <asp:TextBox ID="Fname" style="width:170px;border-radius:5px" runat="server" Enabled="false"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidatorfname" runat="server" ControlToValidate="Fname" ErrorMessage="First Name is required." style="color:red; font-size:small" ValidationGroup="valladdusergp">*</asp:RequiredFieldValidator>  
            &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp
            <asp:Label ID="Label1_lname" style="width:120px;display: inline-block" runat="server" AssociatedControlID="Lname">Last Name</asp:Label>
            <asp:TextBox ID="Lname" style="width:170px;border-radius:5px" runat="server" EnableTheming="True" Enabled="false"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidatorlname" runat="server" ControlToValidate="Lname" ErrorMessage="Last Name is required." style="color:red; font-size:small" ValidationGroup="valladdusergp">*</asp:RequiredFieldValidator>  
        </p>
        <p>
            <asp:Label ID="Label_msisdn" style="width:120px;display: inline-block" runat="server" AssociatedControlID="msisdn">Phone Number</asp:Label>
            <asp:TextBox ID="msisdn" style="width:170px;border-radius:5px" runat="server" EnableTheming="True" TextMode="Phone" Enabled="false"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="msisdn" ErrorMessage="Mbile Number is required." style="color:red; font-size:small" ValidationGroup="valladdusergp">*</asp:RequiredFieldValidator>  
            &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp
            <asp:Label ID="Label1_passwd" style="width:120px;display: inline-block" runat="server" AssociatedControlID="passwd">Password</asp:Label>
            <asp:TextBox ID="passwd" style="width:170px;border-radius:5px" runat="server" EnableTheming="True" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidatorpasswd" runat="server" ControlToValidate="passwd" ErrorMessage="Password Number is required." style="color:red; font-size:small" ValidationGroup="valladdusergp">*</asp:RequiredFieldValidator>  
            <asp:RegularExpressionValidator ID="Regex4" runat="server" ControlToValidate="passwd" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}" ErrorMessage="Password must contain: Minimum 8 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character" ForeColor="Red" ValidationGroup="valladdusergp"/>
        </p>
        <p>
            <asp:Label ID="Label1_up" style="width:120px;display: inline-block" runat="server" AssociatedControlID="userp">User Profile</asp:Label>
            <asp:DropDOwnList ID="userp" style="width:170px;border-radius:5px" runat="server" EnableTheming="True" AutoPostBack="false"></asp:DropDOwnList>
        </p>
        <p>
            <asp:Button ID="usrcaddbtn" runat="server" Text="Submit" CausesValidation="true" ValidationGroup="valladdusergp"></asp:Button>
            <asp:Button ID="usrcaddcnc" runat="server" Text="Cancel"></asp:Button>            
     </p>
    </div>
</asp:Content>
