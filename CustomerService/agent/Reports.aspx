﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="Reports.aspx.vb" Inherits="CustomerService.WebForm1" %>
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
    .table-container {
        height: 100%;
        overflow-y: auto; /* Add vertical scrollbar if content overflows */
    }

    /* Optional: Style the table cells, headers, or other elements */
    .table-container table td,
    .table-container table th {
        border: 1px solid #ddd;
        padding: 8px;
    }
    </style>
    <script type="text/javascript">
        $(function () {
            $(document).ready(function () {
                $('#table_id2').DataTable();
            });
        });
    </script>  
    <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
    <p style = "font-size:x-large; color: #800000;"><strong>Reports</strong></p>
    <br />
    <p><asp:Label ID="lblerroratmsg" runat="server" /></p>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Please correct the following" ShowMessageBox="false" DisplayMode="BulletList" ShowSummary="true" BackColor="Snow" Width="450" ForeColor="Violet" Font-Size="Medium" Font-Italic="true"/>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="text-align: right;">
                <p style="font-size:smaller"><asp:Label ID="Searchstuff" runat="server" AssociatedControlID="searchmsgs">Search criteria</asp:Label>    
                   <asp:DropDownList CssClass="custom-dropdown" ID="searchmsgs" runat="server" Width="175px" AutoPostBack="true">
                       <asp:ListItem Text="Request Reference" Value="1"></asp:ListItem>
                       <asp:ListItem Text="Phone Number" Value="2"></asp:ListItem>
                       <asp:ListItem Text="Date Range" Value="3"></asp:ListItem>
                   </asp:DropDownList>
                    &nbsp &nbsp
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="searchmsgs" errormessage="Please select search criteria" ForeColor="Red" Text="*"></asp:RequiredFieldValidator>
                    <asp:Label ID="search2" runat="server" AssociatedControlID="searchstringone">Search Value</asp:Label>
                    <asp:TextBox CssClass="custom-dropdown" ID="searchstringone" runat="server" Width="175px"></asp:TextBox>
                    &nbsp &nbsp
                    <asp:RequiredFieldValidator ID="reqvalid1" runat="server" ControlToValidate="searchstringone" errormessage="Please input search field" ForeColor="Red" Text="*"></asp:RequiredFieldValidator>          
                    <asp:Label ID="Label1" runat="server" AssociatedControlID="startddate">Start Date</asp:Label>
                    <asp:TextBox CssClass="custom-dropdown" ID="startddate" runat="server" Width="175px" TextMode="Date"></asp:TextBox>
                    &nbsp &nbsp
                    <asp:Label ID="Label2" runat="server" AssociatedControlID="searchstringone">End Date</asp:Label>
                    <asp:TextBox CssClass="custom-dropdown" ID="enddate" runat="server" Width="175px" TextMode="Date"></asp:TextBox>
                    &nbsp &nbsp
                    <asp:CheckBox ID="download" CssClass="custom-checkbox" runat="server" Text="Save Data" Checked="false"/>
                    
                </p> 
            </div>
            
            <p>
                
            </p>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
     <p style="text-align:right">
         <asp:Button CssClass="logout-button" ID="getlist" runat="server" Text="Search" CausesValidation="true" OnClick="getlist_Click"/>
     </p>
    <br />
    <div>
     <asp:PlaceHolder ID = "PlaceHolder1" runat="server" />
 </div>

</asp:Content>
