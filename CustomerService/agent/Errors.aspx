﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="Errors.aspx.vb" Inherits="CustomerService.Errors" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
        <script src="../Scripts/jquery-1.7.min.js"></script>
    <script src="../Scripts/DataTables/jquery.dataTables.js"></script>
    <link href="../Content/DataTables/css/jquery.dataTables.css" rel="stylesheet" />
        <style>
                .custom-file-upload {
    /* Add styles as needed */
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 5px;
    background-color: #f8f8f8;
    color: #333;
    cursor: pointer;
}

/* Style the label when a file is selected */
.custom-file-upload:active,
.custom-file-upload:focus,
.custom-file-upload:hover {
    background-color: #e0e0e0;
    border-color: #999;
}
.custom-dropdown {
    width: 200px; /* Set the width as needed */
    padding: 8px;
    border: 1px solid #ccc;
    border-radius: 5px;
    font-size: 14px;
    color: #333;
    background-color: #fff;
    /* Add more styles as needed */
}

.custom-dropdown:hover {
    border-color: #007bff; /* Change border color on hover */
}

.custom-dropdown:focus {
    outline: none; /* Remove the focus outline if needed */
    border-color: #007bff; /* Change border color on focus */
}
    </style>
    <script type="text/javascript">
        $(function () {
            $(document).ready(function () {
                $('#table_id3').DataTable();
            });
        });
    </script>  
    <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>



        <p style = "font-size:x-large; color: #800000;"><strong>Errors</strong></p>
    <br />
    <p><asp:Label ID="lblerrorfails" runat="server" /></p>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Please correct the following" ShowMessageBox="false" DisplayMode="BulletList" ShowSummary="true" BackColor="Snow" Width="450" ForeColor="Violet" Font-Size="Medium" Font-Italic="true"/>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="text-align: right;">
                <p style="font-size:smaller"><asp:Label ID="errortype" runat="server" AssociatedControlID="errortypelist">Select Report</asp:Label>    
                        <asp:DropDownList  CssClass="custom-dropdown" ID="errortypelist" runat="server">
                            <asp:ListItem Text="Upload History" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Invalid Contacts" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Invalid Accounts" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Failed Records" Value="3"></asp:ListItem>
                   </asp:DropDownList>
                </p> 
            </div>
            
            <p>
                
            </p>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
     <p style="text-align:right">
         <asp:Button CssClass="logout-button" ID="getfails" runat="server" Text="Search" CausesValidation="true" OnClick="getfails_Click"/>
     </p>
    <br />
    <div>
     <asp:PlaceHolder ID = "PlaceHolder1" runat="server" />
 </div>
</asp:Content>
