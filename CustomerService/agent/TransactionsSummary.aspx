<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="TransactionsSummary.aspx.vb" Inherits="CustomerService.TransactionsSummary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script src="../Scripts/jquery-1.7.min.js"></script>
    <script src="../Scripts/DataTables/jquery.dataTables.js"></script>
    <link href="../Content/DataTables/css/jquery.dataTables.css" rel="stylesheet" />

    <script type="text/javascript">
        $(function () {
            $(document).ready(function () {
                $('#table_id2').DataTable();
            });
        });
    </script>  
        <style>
.custom-dropdown {
    width: 200px; 
    padding: 8px;
    border: 1px solid #ccc;
    border-radius: 5px;
    font-size: 14px;
    color: #333;
    background-color: #fff;
    
}

.custom-dropdown:hover {
    border-color: #BB2647; 
}
.custom-dropdown:focus {
    outline: none; 
    border-color: #BB2647;
}
.table-container {
        height: 100%;
        overflow-y: auto;
    }

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
            border-radius: 7px;
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
                <p style = "font-size:x-large; color: #BB2647;"><strong>Transaction Summary</strong></p>
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
                            <p style="font-size:smaller"><asp:Label ID="Searchstuff" runat="server" AssociatedControlID="searchmnos">Select MNO</asp:Label>
                                &nbsp &nbsp
                            <asp:DropDownList CssClass="custom-dropdown" ID="searchmnos" runat="server" Width="175px" AutoPostBack="true">
                               <asp:ListItem Text="Summary" Value="1"></asp:ListItem>
                               <asp:ListItem Text="MTN" Value="2"></asp:ListItem>
                               <asp:ListItem Text="Airtel" Value="3"></asp:ListItem>
                           </asp:DropDownList>
                            &nbsp &nbsp
                            <asp:DropDownList CssClass="custom-dropdown" ID="searchType" runat="server" Width="175px" AutoPostBack="true">
                                <asp:ListItem Text="Completed" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Invalid Transaction ID" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Failed" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Successful" Value="4"></asp:ListItem>
                            </asp:DropDownList>
                                &nbsp &nbsp
                            <asp:Label runat="server">Start Date</asp:Label>
                                <asp:TextBox CssClass="custom-dropdown" ID="startddate" runat="server" Width="175px" TextMode="Date"></asp:TextBox>
                                &nbsp &nbsp
                            <asp:Label  runat="server" >End Date</asp:Label>
                            <asp:TextBox CssClass="custom-dropdown" ID="enddate" runat="server" Width="175px" TextMode="Date"></asp:TextBox>
                            </p>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="box">
                <p style="text-align:right">
                    <asp:Button CssClass="download-button" ID="Report" runat="server" Text="Download" OnClick="DownloadReport_Click"/>
                    <asp:Button CssClass="logout-button" ID="getMNO" runat="server" Text="Search" CausesValidation="true" OnClick="getMNO_Click"/>
                </p>
            </div>
            <!-- Add more elements as needed -->
        </div>
    </div>
    <p></p>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Please correct the following" ShowMessageBox="false" DisplayMode="BulletList" ShowSummary="true" BackColor="Snow" Width="450" ForeColor="Violet" Font-Size="Medium" Font-Italic="true"/>
    
    <br />
     
    <br />
    <div>
     <asp:PlaceHolder ID = "PlaceHolder1" runat="server" />
 </div>

</asp:Content>

