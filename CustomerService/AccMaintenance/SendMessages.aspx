<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="SendMessages.aspx.vb" Inherits="CustomerService.SendMessages" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
        <script src="../Scripts/jquery-1.7.min.js"></script>
    <script src="../Scripts/DataTables/jquery.dataTables.js"></script>
    <link href="../Content/DataTables/css/jquery.dataTables.css" rel="stylesheet" />

    <script type="text/javascript">
        $(function () {
            $(document).ready(function () {
                $('#table_id2SM').DataTable();
            });
        });
    </script>  
            <style>
                .custom-file-upload {
    /* Add styles as needed */
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 3px;
    background-color: #f8f8f8;
    color: #333;
    cursor: pointer;
}

.custom-file-upload:active,
.custom-file-upload:focus,
.custom-file-upload:hover {
    background-color: #e0e0e0;
    border-color: #BB2647;
}

.custom-dropdown {
    width: 200px; /* Set the width as needed */
    padding: 8px;
    border: 1px solid #ccc;
    border-radius: 3px;
    font-size: 14px;
    color: #333;
    background-color: #fff;
    /* Add more styles as needed */
}
.custom-dropdown:hover {
    border-color: #BB2647; /* Change border color on hover */
}

.custom-dropdown:focus {
    outline: none; /* Remove the focus outline if needed */
    border-color: #BB2647; /* Change border color on focus */
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
            .toolbar {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 5px;
            background-color: #f0f0f0;
            border-radius: 5px;
            margin: 5px 5px;
            position: sticky;
            top: 0;
        }
        .left-container {
            text-align: left;
        }

        .right-container {
            text-align: right;
        }


        .box {
            padding: 10px;
            margin: 5px;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
    <div class="toolbar">
        <div class="left-container">
            <div class="box">
                <p style = "font-size:x-large; color: #BB2647;"><strong>Maintenance :: Messages</strong></p>
            </div>
            <div class="box">
                <asp:Label ID="lblerroratmsg" runat="server" />
            </div>
            <!-- Add more elements as needed -->
        </div>
    
        <div class="right-container">
            <div class="box">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div style="text-align: right;">
                                <p style="font-size:smaller"><asp:Label ID="Searchstuff" runat="server" AssociatedControlID="searchmsgs">Search criteria</asp:Label>    
                   <asp:DropDownList CssClass="custom-dropdown" ID="searchmsgs" runat="server" Width="175px" AutoPostBack="true">
                       <asp:ListItem Text="Case Number" Value="1"></asp:ListItem>
                       <asp:ListItem Text="Account Number" Value="2"></asp:ListItem>
                       <asp:ListItem Text="Date Range" Value="3"></asp:ListItem>
                   </asp:DropDownList>
                    &nbsp &nbsp
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="searchmsgs" errormessage="Please select search criteria" ForeColor="Red" Text="*"></asp:RequiredFieldValidator>
                    <asp:Label ID="search2" runat="server" AssociatedControlID="searchstringone">Search Value</asp:Label>
                    <asp:TextBox CssClass="custom-dropdown" ID="searchstringone" runat="server" Width="175px"></asp:TextBox>
                    &nbsp &nbsp
                    <asp:RequiredFieldValidator ID="reqvalid1" runat="server" ControlToValidate="searchstringone" errormessage="Please input search field" ForeColor="Red" Text="*"></asp:RequiredFieldValidator>          
                    <asp:Label ID="Label1" runat="server" AssociatedControlID="startddate">From</asp:Label>
                    <asp:TextBox CssClass="custom-dropdown" ID="startddate" runat="server" Width="175px" TextMode="Date"></asp:TextBox>
                    &nbsp &nbsp
                    <asp:Label ID="Label2" runat="server" AssociatedControlID="searchstringone">To:</asp:Label>
                    <asp:TextBox CssClass="custom-dropdown" ID="enddate" runat="server" Width="175px" TextMode="Date"></asp:TextBox>
                </p>
                            </div>
                            <br />
                        </ContentTemplate>
                    </asp:UpdatePanel>
            </div>
            <div class="box">
                <asp:Button CssClass="logout-button" ID="getlist" runat="server" Text="Search" CausesValidation="true" onclick="getlist_Click1"/>
            </div>
            <!-- Add more elements as needed -->
        </div>
    </div>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Please correct the following" ShowMessageBox="false" DisplayMode="BulletList" ShowSummary="true" BackColor="Snow" Width="450" ForeColor="Violet" Font-Size="Medium" Font-Italic="true"/>
    <div>
     <asp:PlaceHolder ID = "PlaceHolder1" runat="server" />
 </div>
</asp:Content>
