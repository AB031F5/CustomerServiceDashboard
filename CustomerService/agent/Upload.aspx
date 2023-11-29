<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="Upload.aspx.vb" Inherits="CustomerService.Upload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script src="../Scripts/jquery-1.7.min.js"></script>
    <script src="../Scripts/DataTables/jquery.dataTables.js"></script>
    <link href="../Content/DataTables/css/jquery.dataTables.css" rel="stylesheet" />
<link href="../Content/DataTables/css/jquery.dataTables.css" rel="stylesheet" />
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

/* Style the label when a file is selected */
.custom-file-upload:active,
.custom-file-upload:focus,
.custom-file-upload:hover {
    background-color: #e0e0e0;
    border-color: #999;
}

.custom-dropdown {
    width: 200px; 
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

        .toolbar {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 5px;
            background-color: #f0f0f0;
            border-radius: 3px;
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
            <%--$(function () {
                $('#<%=processup.ClientID %>').click(function () {
                    $("#tablesorter")
                        .dataTable({
                            "bPaginate": true,
                            "bLengthChange": true,
                            "bFilter": true
                        })
                    alert("Button Clicked");
                });
            });--%>

           <%-- $(function () {
                $('#<%=processup.ClientID %>').click(function () {
                    $('#tablesorter').DataTable();
                    alert("Button Clicked");
                });
            });--%>
            $(function () {
                $(document).ready(function () {
                    $('#table_id').DataTable();
                });
            });

            <%--$(function () {
                $('#<%=Button1.ClientID %>').click(function () {                    
                    alert(" Test Button Clicked");
                });
            });--%>

            //$(document).ready(function () {
            //    $("#tablesorter")
            //        .dataTable({
            //            "bPaginate": true,
            //            "bLengthChange": true,
            //            "bFilter": true
            //        });
            //});

        </script> 
    <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
<div class=" page-holder d-flex align-items-center">
    <div class="container">
        <div style="text-align: left;">
            <div class="toolbar">
                <div class="left-container">
                    <div class="box">
                        <p style = "font-size:x-large; color: #BB2647;"><strong>Complaints</strong></p>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:FileUpload runat="server" ID="fupload" CssClass="custom-file-upload"/>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="box">
                        <asp:Button runat="server" ID="processup" Text="Upload File" CssClass="logout-button" />
                    </div>
                    <!-- Add more elements as needed -->
                </div>
    
            <div class="right-container">
                <div class="box">
                                <asp:DropDownList CssClass="custom-dropdown" ID="errortypelist" runat="server">
                                    <asp:ListItem Text="All Records" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="WIP" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="New" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Closed" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Processed" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Unprocessed" Value="5"></asp:ListItem>
                                </asp:DropDownList>
                </div>
                <div class="box">
                     <asp:Button ID="refresh" runat="server" Text="Show Records" CssClass="logout-button" OnClick="refresh_Click" />
                    <p><asp:Label ID="lblerror" runat="server" /></p>
                </div>
                <!-- Add more elements as needed -->
            </div>
        </div>
        </div>
        <br />
</p>
                        <div style="float: right;">
                             <div>
            
                            </div>
                        </div>
        <p style="text-align:right">


        </p>
 <div>
     <asp:PlaceHolder ID = "PlaceHolder1" runat="server" >
     </asp:PlaceHolder>
 </div>
    </div>

</div>
</asp:Content>
