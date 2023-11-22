<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="MobilemoneyBalance.aspx.vb" Inherits="CustomerService.MobilemoneyBalance" %>
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
    border-color: #BB2647;
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
    <script type="text/javascript">
        $(function () {
            $(document).ready(function () {
                $('#table_id2').DataTable();
            });
        });
    </script>  
    <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
     <div class="toolbar">
        <div class="left-container">
            <div class="box"><p style = "font-size:x-large; color: #BB2647;"><strong>MM Balance</strong></p></div>
            <!-- Add more elements as needed -->
        </div>
    
        <div class="right-container">
            <div class="box"><asp:Label ID="lblerroratmsg1" runat="server" /></div>
            <!-- Add more elements as needed -->
        </div>
    </div>
    <p></p>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Please correct the following" ShowMessageBox="false" DisplayMode="BulletList" ShowSummary="true" BackColor="Snow" Width="450" ForeColor="Violet" Font-Size="Medium" Font-Italic="true"/>
    <br />
    <div>
     <asp:PlaceHolder ID = "PlaceHolder12" runat="server" />
    </div>

</asp:Content>
