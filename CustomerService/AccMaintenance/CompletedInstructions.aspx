﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="CompletedInstructions.aspx.vb" Inherits="CustomerService.CompletedInstructions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script src="../Scripts/jquery-1.7.min.js"></script>
<script src="../Scripts/DataTables/jquery.dataTables.js"></script>
<link href="../Content/DataTables/css/jquery.dataTables.css" rel="stylesheet" />

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

    <div class=" page-holder d-flex align-items-center">
    <div class="container">
        <div style="text-align: left;">
            
            <br />
            <br />
            <div class="toolbar">
                <div class="left-container">
                    <div class="box">
                        <p style = "font-size:x-large; color: #BB2647;"><strong>Maintenance :: Instructions</strong></p>
                    </div>
                    <div class="box"><asp:Label ID="lblerror" runat="server" /></div>
                    <!-- Add more elements as needed -->
                </div>
    
                <div class="right-container">
                    <div class="box">
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
            </div>
            
        </div>
        <br />
        
    <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Please correct the following" ShowMessageBox="false" DisplayMode="BulletList" ShowSummary="true" BackColor="Snow" Width="450" ForeColor="Violet" Font-Size="Medium" Font-Italic="true"/>--%>
        
</p>
 <div>
     <asp:PlaceHolder ID = "PlaceHolder1" runat="server" />
 </div>
    </div>

</div>
</asp:Content>
