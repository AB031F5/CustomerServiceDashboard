<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="Password_Change.aspx.vb" Inherits="CustomerService.Password_Change" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
    <asp:ValidationSummary runat="server" ValidationGroup="valladdusergp" style="color:red; font-size:small" HeaderText="There were errors on the page:" />
    <p style = "font-size:small"><strong>Password Change Details. You need to set a new password upon reset and after every 30 days</strong></p>
    <div style="width:1050px; border-radius: 10px; padding-left: 10px; padding-right: 10px; padding-top: 5px; padding-bottom: 5px; border: thin solid blue; font-size:smaller">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <p>
                    <asp:Label ID="Label1_cpwd" style="width:120px;display: inline-block" runat="server" AssociatedControlID="cpwd">Current Password</asp:Label>
                    <asp:TextBox ID="cpwd" style="width:170px;border-radius:5px" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorcpwd" runat="server" ControlToValidate="cpwd" ErrorMessage="Current Password is required." style="color:red; font-size:small" ValidationGroup="valladdusergp">*</asp:RequiredFieldValidator>  
                </p>
                <p>
                    <asp:Label ID="Label1_nwpwd" style="width:120px;display: inline-block" runat="server" AssociatedControlID="nwpwd">New Password</asp:Label>
                    <asp:TextBox ID="nwpwd" style="width:170px;border-radius:5px" runat="server" EnableTheming="True" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatornwpwd" runat="server" ControlToValidate="nwpwd" ErrorMessage="New Password is required." style="color:red; font-size:small" ValidationGroup="valladdusergp">*</asp:RequiredFieldValidator>  
                    <asp:RegularExpressionValidator ID="Regex4" runat="server" ControlToValidate="nwpwd" ValidationGroup="valladdusergp" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}" ErrorMessage="Password must contain: Minimum 8 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character" ForeColor="Red" >*</asp:RegularExpressionValidator>
                </p>
                <p>
                    <asp:Label ID="Label1" style="width:120px;display: inline-block" runat="server" AssociatedControlID="renwpwd">Re-Enter Password</asp:Label>
                    <asp:TextBox ID="renwpwd" style="width:170px;border-radius:5px" runat="server" EnableTheming="True" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorrenwpwd" runat="server" ControlToValidate="renwpwd" ErrorMessage="Re-enter New password." style="color:red; font-size:small" ValidationGroup="valladdusergp">*</asp:RequiredFieldValidator>  
                    <asp:CompareValidator ID="comp12" runat="server" Operator="GreaterThanEqual" ControlToValidate="nwpwd" ControlToCompare="renwpwd" ErrorMessage="Entered and confirmed passwords must be the same" SetFocusOnError="true" ForeColor="Red" ValidationGroup="valladdusergp">*</asp:CompareValidator>
                </p>
                <p>
                    <asp:Button ID="usrcpwdcbtn" style="width:120px" runat="server" Text="Submit" CausesValidation="true" ValidationGroup="valladdusergp"></asp:Button>
                    <asp:Button ID="usrcpwdccnc" style="width:120px" runat="server" Text="Cancel"></asp:Button>            
                </p>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div>       
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <cc1:ModalPopupExtender id="pwd_change" runat="server" PopupControlID="panel2k2" CancelControlID="model_kok3" targetcontrolid="HiddenField5" BackgroundCssClass="modalBackground" BehaviorID="Edit_sec_att_Behaviour"></cc1:ModalPopupExtender>
                <asp:Panel id="panel2k2" runat="server" CssClass="modalPopupTinyTwo">  
                    <asp:ValidationSummary id="valatt" ValidationGroup="valGroupsatt" runat="server" style="color:red; font-size:small" HeaderText="There were errors on the page:" />
                        <asp:HiddenField id="HiddenField5" runat="server" />
                        <div class="header">
                            Change Password 
                        </div>
            
                         <asp:ValidationSummary runat="server" ValidationGroup="valladdusergp" style="color:red; font-size:small" HeaderText="There were errors on the page:" />
                        <p style = "font-size:small"><strong>Password Change Details</strong></p>
    
                            <p>
                                <asp:Label ID="Label2" style="width:120px;display: inline-block" runat="server" AssociatedControlID="cpwd">Current Password</asp:Label>
                                <asp:TextBox ID="cpwdpop" style="width:170px;border-radius:5px" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="cpwdpop" ErrorMessage="Current Password is required." style="color:red; font-size:small" ValidationGroup="valGroupsatt">*</asp:RequiredFieldValidator>  
                            </p>
                            <p>
                                <asp:Label ID="Label3" style="width:120px;display: inline-block" runat="server" AssociatedControlID="nwpwd">New Password</asp:Label>
                                <asp:TextBox ID="nwpwdpop" style="width:170px;border-radius:5px" runat="server" EnableTheming="True" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="nwpwdpop" ErrorMessage="New Password is required." style="color:red; font-size:small" ValidationGroup="valGroupsatt">*</asp:RequiredFieldValidator>  
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="nwpwdpop" ValidationGroup="valGroupsatt" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}" ErrorMessage="Password must contain: Minimum 8 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character" ForeColor="Red" >*</asp:RegularExpressionValidator>
                            </p>
                            <p>
                                <asp:Label ID="Label4" style="width:120px;display: inline-block" runat="server" AssociatedControlID="renwpwd">Re-Enter Password</asp:Label>
                                <asp:TextBox ID="renwpwdpop" style="width:170px;border-radius:5px" runat="server" EnableTheming="True" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="renwpwdpop" ErrorMessage="Re-enter New password." style="color:red; font-size:small" ValidationGroup="valGroupsatt">*</asp:RequiredFieldValidator>  
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="renwpwdpop" ControlToCompare="nwpwdpop" ErrorMessage="Entered and confirmed passwords must be the same" SetFocusOnError="true" ForeColor="Red" ValidationGroup="valGroupsatt">*</asp:CompareValidator>
                            </p>

                        <div class="footer">
                            <asp:Button runat="server" ID="Submit_det2" causesvalidation="true" ValidationGroup="valGroupsatt" Text="Submit"/>
                            <asp:Button runat="server" ID="model_kok3" Text="Cancel" Enabled="false"/>
                            <asp:Button runat="server" ID="model_kok2" Text="Cancel"/>
                        </div>

                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="Submit_det2" />
            </Triggers>
    </asp:UpdatePanel>
    </div>
</asp:Content>
