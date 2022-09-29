<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmIIBClaimDataUploadNew.aspx.cs" Inherits="PrjPASS.FrmIIBClaimDataUploadNew" %>
<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MstCntFormContent">
    <script>
    var updateProgress = null;
 
    function postbackButtonClick() {
        updateProgress = $find("<%= Up1.ClientID %>");
        window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
        return true;
        }

        function openModal() {
            $('#myModal').modal('show');
        }
    </script>
    <style>
        /* Absolute Center Spinner */
          #table1 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color:#4a4949;
        }
        
        #table31 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color:#4a4949;
        }

        #table32 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color:#4a4949;
        }

        #table33 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color:#4a4949;
        }

        
        #table34 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color:#4a4949;
        }

        #table21 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color:#4a4949;
        }

        #table22 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color:#4a4949;
        }

        #table24 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color:#4a4949;
        }

        #table3 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
            color:black;
        }

         #table23 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
            color:black;
        }

        #table13 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
        }

        #table4 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
            color:black;
        }

        #table5 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
            color:black;
        }

        #table6 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
        }

        #table7 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
        }

        #table8 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
        }

        #table9 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 4px;
        }

        .tdbkg {
            background-color: lightgray;
            font-size: 11px;
        }

        .tdbkgHead {
            background-color: darkgray;
            font-size: 11px;
        }

        .tdbkg2 {
            /*background-color: rgba(51, 180, 255, 0.22);*/
            font-size: 11px;
        }
        
         .mydatagrid {
            width: 100%;
            border: solid 1px black;
            font-size: 8px;
        }
         .header {
            background-color: #c1c4d0;
            font-family: Arial;
            color: black;
            border: none 0px transparent;
            height: 25px;
            text-align: center;
            font-size: 14px;
        }

        .rows {
            background-color: #fff;
            font-family: Arial;
            font-size: 12px;
            color: #000;
            min-height: 25px;
            text-align: left;
            border: none 0px transparent;
        }

            .rows:hover {
                background-color: #c1c4d0;
                font-family: Arial;
                color: black;
                text-align: left;
            }

        .selectedrow {
            background-color: #ff8000;
            font-family: Arial;
            color: #fff;
            font-weight: bold;
            text-align: left;
        }

        .mydatagrid a /** FOR THE PAGING ICONS  **/ {
            background-color: Transparent;
            padding: 5px 5px 5px 5px;
            color: navy;
            text-decoration: none;
            font-weight: bold;
        }

            .mydatagrid a:hover /** FOR THE PAGING ICONS  HOVER STYLES**/ {
                color: navy;
            }

        .mydatagrid span /** FOR THE PAGING ICONS CURRENT PAGE INDICATOR **/ {
            color: #000;
            padding: 5px 5px 5px 5px;
        }

        .pager {
            background-color: #c1c4d0;
            font-family: Arial;
            color: White;
            height: 30px;
            text-align: left;
        }

        .mydatagrid td {
            padding: 5px;
            border: 1px solid black;
        }

        .mydatagrid th {
            padding: 5px;
            border: 1px solid black;
        }
        #resultLoading {
            position: fixed;
            z-index: 999;
            height: 2em;
            width: 2em;
            overflow: show;
            margin: auto;
            top: 0;
            left: 0;
            bottom: 0;
            right: 0;
        }

            /* Transparent Overlay */
            #resultLoading:before {
                content: '';
                display: block;
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background-color: rgba(0,0,0,0.3);
            }

            /* :not(:required) hides these rules from IE9 and below */
            #resultLoading:not(:required) {
                /* hide "loading..." text */
                font: 0/0 a;
                color: transparent;
                text-shadow: none;
                background-color: transparent;
                border: 0;
            }

                #resultLoading:not(:required):after {
                    content: '';
                    display: block;
                    font-size: 10px;
                    width: 1em;
                    height: 1em;
                    margin-top: -0.5em;
                    -webkit-animation: spinner 1500ms infinite linear;
                    -moz-animation: spinner 1500ms infinite linear;
                    -ms-animation: spinner 1500ms infinite linear;
                    -o-animation: spinner 1500ms infinite linear;
                    animation: spinner 1500ms infinite linear;
                    border-radius: 0.5em;
                    -webkit-box-shadow: rgba(0, 0, 0, 0.75) 1.5em 0 0 0, rgba(0, 0, 0, 0.75) 1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) 0 1.5em 0 0, rgba(0, 0, 0, 0.75) -1.1em 1.1em 0 0, rgba(0, 0, 0, 0.5) -1.5em 0 0 0, rgba(0, 0, 0, 0.5) -1.1em -1.1em 0 0, rgba(0, 0, 0, 0.75) 0 -1.5em 0 0, rgba(0, 0, 0, 0.75) 1.1em -1.1em 0 0;
                    box-shadow: rgba(0, 0, 0, 0.75) 1.5em 0 0 0, rgba(0, 0, 0, 0.75) 1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) 0 1.5em 0 0, rgba(0, 0, 0, 0.75) -1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) -1.5em 0 0 0, rgba(0, 0, 0, 0.75) -1.1em -1.1em 0 0, rgba(0, 0, 0, 0.75) 0 -1.5em 0 0, rgba(0, 0, 0, 0.75) 1.1em -1.1em 0 0;
                }

        /* Animation */

        @-webkit-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-moz-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-o-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="Server" ChildrenAsTriggers="False" UpdateMode="Conditional">
        <ContentTemplate>
    <%--<form id="form1" runat="server">--%>
        <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>PASS - IIB Claim Data Upload</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 21%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label1" runat="server" Text="Select File" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 21%; left: 10%; width: 40%; margin-top: 10px; margin-left: 10px">
                                <asp:FileUpload ID="FileUpload1" runat="server" Width="100%"/>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left:15%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnImport" runat="server" OnClientClick="return postbackButtonClick();" Text="Import Data" Width="100%" OnClick="Upload" />
                            </div>
                             <div style="position: absolute; top: 40%; left:35%;width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExport" runat="server" Text="Get Template" Width="100%" OnClick="btnExport_Click"/>
                            </div>
                            <div style="position: absolute; top: 40%; left:55%; width: 15%; margin-top: 10px; margin-left: 10px;display:none;">
                                <obout:OboutButton ID="btnDownloadLink" Width="100%" runat="server" Text="Download IIB Claim Data" OnClick="btnDownloadLink_Click" />
                            </div>
                            <div style="position: absolute; top: 40%; left:75%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click"/>
                            </div>
                            <div style="position: absolute; top: 58%; left: 35%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblstatus1" runat="server" />
                            </div>
                        </asp:Panel>
                    </div>
                      <!--BulkStatus-->

                      <div style="">
                                <table id="table34" style="width: 100%;" cellspacing="0" cellpadding="2">
                                     <tr>
                                                       <td class="tdbkgHead" colspan="6" style="font-size:15px">View Uploaded File Status
                                                        </td>
                                         </tr>
                                      
                                    <tr>
                                        <td class="tdbkg">
                                            <asp:GridView ID="FileProcessGridView" runat="server" AutoGenerateColumns="false" ItemType="ProjectPASS.FileUploadedInformation"
                                        SelectMethod="FileProcessGridView_GetData" 
                                        DataKeyNames="FileUploadTransactionId" CssClass="mydatagrid" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header"
                                        RowStyle-CssClass="rows" AllowPaging="true" OnPageIndexChanging="FileProcessGridView_PageIndexChanging" PageSize="10">
                                        <Columns>
                                            
                                            <asp:BoundField DataField="FileUploadTransactionId" HeaderText="File Upload Transaction Id" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="FileName" HeaderText="File Name" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="FileUploadedBy" HeaderText="File Uploaded By" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="FileUploadedOn" HeaderText="File Uploaded On" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="IsFileProcessed" HeaderText="Is File Processed" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="FileProcessedOn" HeaderText="File Processed On" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
      
                                        </Columns>
                                    </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                
                                
                                    </div>

                    <!--BulkStatus-->

                </div>
            </div>
        </div>
    </div>

 <div class="modal fade" id="myModal" role="dialog" data-backdrop="static">
                        <div class="modal-dialog">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header alert alert-info fade in">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Status</h4>
                                </div>
                                <div class="modal-body">
                                     <asp:Label ID="lblstatus" runat="server" style="color:red" />
                                </div>
                                <!-- Modal footer-->
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                </div>
                            </div>

                        </div>
                    </div>
    <%--</form>--%>
            </ContentTemplate>
   <Triggers>
    <asp:PostBackTrigger ControlID="btnExit" />
      <asp:PostBackTrigger ControlID="btnImport" />
       <asp:PostBackTrigger ControlID="btnExport" />
       <asp:PostBackTrigger ControlID="btnDownloadLink" />
       <asp:PostBackTrigger ControlID="btnExit" />            
   </Triggers>
    </asp:UpdatePanel>
<%--<asp:UpdateProgress ID="Up1" runat="Server" AssociatedUpdatePanelID="UpdatePanel1">
    <ProgressTemplate>
        <span style="background-color:#66997A;"> <img src="Images/ajax-loader.gif" alt="Uploading.....Please wait"  width="100px"/> Please wait ...</span>
    </ProgressTemplate>
</asp:UpdateProgress> --%>  
   <asp:UpdateProgress runat="server" ID="Up1" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div id="resultLoading">
                <div>
                    <img alt="" src="Images/ajax-loader.gif"><div>Loading...Please Wait</div>
                </div>
                <div class="bg"></div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>


