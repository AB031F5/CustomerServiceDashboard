<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="SendErrors.aspx.vb" Inherits="CustomerService.SendErrors" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
     <script src="../Scripts/jquery-1.7.min.js"></script>
    <script src="../Scripts/DataTables/jquery.dataTables.js"></script>
    <link href="../Content/DataTables/css/jquery.dataTables.css" rel="stylesheet" />
    <script type="text/javascript">
        $(function () {
            $(document).ready(function () {
                $('#table_id3SE').DataTable();
            });
        });
    </script>  
        </script>  
    <style>
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
    <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>




           <p style = "font-size:x-large; color: #800000;"><strong>Maintenance :: Errors</strong></p>
   <br />
   <p><asp:Label ID="lblerrorfails" runat="server" /></p>
   <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Please correct the following" ShowMessageBox="false" DisplayMode="BulletList" ShowSummary="true" BackColor="Snow" Width="450" ForeColor="Violet" Font-Size="Medium" Font-Italic="true"/>
   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
       <ContentTemplate>
           <div style="text-align: right;">
               <p style="font-size:smaller"><asp:Label ID="errortype" runat="server" AssociatedControlID="errortypelist">Select Report</asp:Label>
                   &nbsp &nbsp
               <asp:DropDownList CssClass="custom-dropdown" ID="errortypelist" runat="server" Width="175px" AutoPostBack="true">
                    <asp:ListItem Text="Upload History" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Invalid Contacts" Value="1"></asp:ListItem>
              </asp:DropDownList>
                   &nbsp &nbsp
<%--                   <asp:CheckBox ID="download" CssClass="custom-checkbox" runat="server" Text="Save Data" Checked="false"/>--%>
               </p>
           </div>
       </ContentTemplate>
   </asp:UpdatePanel>
   <br />
    <p style="text-align:right">
        <asp:Button CssClass="logout-button" ID="getfails" runat="server" Text="Get Report" CausesValidation="true" OnClick="getfails_Click1"/>
    </p>
   <br />
   <div>
    <asp:PlaceHolder ID = "PlaceHolder1" runat="server" />
</div>
</asp:Content>
