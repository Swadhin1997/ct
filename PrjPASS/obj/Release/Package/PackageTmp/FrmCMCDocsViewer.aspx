<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="FrmCMCDocsViewer.aspx.cs" Inherits="PrjPASS.FrmCMCDocsViewer" %>

<%--<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>--%>

<%@ Register TagPrefix="obout" Namespace="OboutInc.Calendar2" Assembly="obout_Calendar2_NET" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at1" %>

<%@ Register Assembly="System.Web" Namespace="System.Web.UI.HtmlControls" TagPrefix="ifrasp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=9; IE=8; IE=7; IE=10" />
    <title>PASS - CMC Docs Viewer</title>

    <link href="SmartForms/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="SmartForms/css/smart-themes/blue.css" />
    <link href="SmartForms/css/smart-forms.css" rel="stylesheet" type="text/css" />
    <link href="site/styles/custom.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="Grid/resources/custom_scripts/excel-style/excel-style.css" rel="stylesheet" />

    <style type="text/css">
        #table1 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color: #4a4949;
        }

        #table21 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
            color: #4a4949;
        }



        .tdbkg {
            background-color: lightgray;
            font-size: 11px;
        }

        .tdbkg2 {
            /*background-color: rgba(51, 180, 255, 0.22);*/
            font-size: 11px;
        }



        .validationsummary {
            border: 1px solid rgba(51, 153, 255, 0.2);
            font-size: 11px;
            padding: 0px 0px 13px 0px;
        }

        .validationheader {
            left: 0px;
            position: relative;
            font-size: 12px;
            background-color: rgba(51, 153, 255, 0.27);
            color: black;
            height: 12px;
            border-bottom: 1px solid rgba(51, 153, 255, 0.2);
            padding-top: 3px;
            border: 1px solid rgba(51, 153, 255, 0.2);
            padding: 0px 0px 13px 0px;
        }


        .validationsummary ul li {
            padding: 2px 0px 0px 15px;
            background-image: url(Images/error4.png);
            background-size: 10px 10px;
            background-position: 0px 3px;
            background-repeat: no-repeat;
        }

        .validationsummary ul {
            padding-top: 5px;
            padding-left: 10px;
            list-style: none;
            font-size: 11px;
            color: #ff0000;
        }

        .drp {
            width: 150px;
            height: 19px;
            border: 1px solid #5B768A;
            border-radius: 3px;
            background-color: #EAEAEA;
            background: linear-gradient(EAEAEA, white);
        }

            .drp:hover {
                cursor: pointer;
            }

        /* Absolute Center Spinner */
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

        .mydatagrid {
            width: 100%;
            border: solid 1px black;
            font-size: 6px;
        }

        .header {
            background-color: #c1c4d0;
            font-family: Arial;
            color: black;
            border: none 0px transparent;
            height: 20px;
            text-align: center;
            font-size: 11px;
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
            text-align: center;
        }

        .mydatagrid th {
            padding: 3px;
            border: 1px solid black;
            text-align: center;
        }
    </style>

    <style type="text/css">
        .modalBackgroundStatus {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }

        .modalPopupStatus {
            background-color: #FFFFFF;
            width: 500px;
            border: 2px solid #18B5F0;
            border-radius: 8px;
            padding: 0;
        }

            .modalPopupStatus .headerStatus {
                background-color: #0087bb;
                height: 30px;
                color: White;
                line-height: 30px;
                text-align: left;
                font-weight: bold;
                border-top-left-radius: 6px;
                border-top-right-radius: 6px;
                padding-left: 5px;
                font-size: small;
            }

            .modalPopupStatus .bodyStatus {
                min-height: 50px;
                line-height: 20px;
                font-weight: bold;
                font-size: small;
                padding: 6px;
                text-align: left;
                color: red;
            }

            .modalPopupStatus .footerStatus {
                padding: 6px;
            }

            .modalPopupStatus .yesStatus, .modalPopupStatus .noStatus {
                color: White;
                text-align: center;
                font-weight: bold;
                cursor: pointer;
                border-radius: 4px;
            }

            .modalPopupStatus .yesStatus {
                background-color: #2FBDF1;
                border: 1px solid #0DA9D0;
                font-size: small;
                padding: 5px;
            }

            .modalPopupStatus .noStatus {
                background-color: #9F9F9F;
                border: 1px solid #5C5C5C;
            }


        .modalPopupPB {
            background-color: #FFFFFF;
            width: 70%;
            height: 590px;
            border: 2px solid #18B5F0;
            border-radius: 8px;
            padding: 0;
        }

            .modalPopupPB .headerPB {
                background-color: #0087bb;
                height: 30px;
                color: White;
                line-height: 30px;
                text-align: left;
                border-top-left-radius: 6px;
                border-top-right-radius: 6px;
                padding-left: 5px;
                font-size: small;
            }

            .modalPopupPB .bodyPB {
                min-height: 50px;
                font-size: small;
                padding: 4px;
                text-align: left;
            }

            .modalPopupPB .footerPB {
                padding: 6px;
            }

            .modalPopupPB .yesPB, .modalPopupPB .noPB {
                color: White;
                text-align: center;
                cursor: pointer;
                border-radius: 4px;
            }

            .modalPopupPB .yesPB {
                background-color: #2FBDF1;
                border: 1px solid #0DA9D0;
                font-size: small;
                padding: 5px;
            }

                .modalPopupPB .yesPB:hover {
                    background-color: dodgerblue;
                }

            .modalPopupPB .DownloadPB {
                background-color: #2FBDF1;
                border: 1px solid #0DA9D0;
                font-size: small;
                padding: 5px;
                color: White;
                text-align: center;
                cursor: pointer;
                border-radius: 4px;
                margin-right: 10px;
            }

                .modalPopupPB .DownloadPB:hover {
                    background-color: dodgerblue;
                }


            .modalPopupPB .noPB {
                background-color: #9F9F9F;
                border: 1px solid #5C5C5C;
            }

        .accordionHeaderSelected {
            border: 1px solid #2E4d7B;
            color: white;
            background-color: rgba(51, 51, 51, 0.69);
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            padding-left: 30px;
            margin-top: 5px;
            cursor: pointer;
            background-image: url(Images/bullet_toggle_minus.png);
            background-repeat: no-repeat;
            background-position: left;
        }

        .accordionHeader {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: rgba(51, 51, 51, 0.69);
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            padding-left: 30px;
            margin-top: 4px;
            cursor: pointer;
            background-image: url(Images/bullet_toggle_plus.png);
            background-repeat: no-repeat;
            background-position: left;
        }

        .accordionContent {
            border: 1px solid #2F4F4F;
            border-top: none;
            padding: 5px;
            padding-top: 6px;
        }
    </style>

    <style type="text/css">
        .style1 {
            font-size: xx-small;
            color: #990000;
            text-decoration: underline;
            font-style: italic;
        }

        #tiffobj0 {
            height: 496px;
        }

        .auto-style6 {
            width: 20px;
        }
    </style>

   <%--SR79993 : VAPT SEPT 2020 RETESTING  - HASMUKH FIX FOR VULNERABLE COMPONENT IN USE **START** --%>

    <link href="css/bootstrap/bootstrap.min.css" rel="stylesheet" /> 
    <link href="js/jquery-ui-1.12.1/jquery-ui.css" rel="stylesheet" />

    <script src="js/jquery-3.5.1.min.js" type="text/javascript"></script>
    <script src="js/jquery-ui-1.12.1/jquery-ui.js" type="text/javascript"></script>
    <script src="js/jquery-ui-1.12.1/bootstrap3.3.7min.js" type="text/javascript"></script> 
    
     <%--SR79993 : VAPT SEPT 2020 RETESTING  - HASMUKH FIX FOR VULNERABLE COMPONENT IN USE **END** --%>

    <script type="text/javascript">
        $(function () {
            SetAutoComplete();
        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetAutoComplete);
        });


        function openModalADLogin() {
            $('#ModalADLogin').modal('show');
        }

        function CloseModalADLogin() {
            $('#ModalADLogin').modal('hide');
        }
        function SetAutoComplete() {


        }



    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div style="position: relative; top: 20%; background-color: #ff0000; background-image: url(pg_header_kotak.png); background-size: auto; -webkit-background-size: cover; background-repeat: no-repeat">
            <div id="headerSeparator"></div>
            <div class="row-fluid">
                <div class="span6">
                </div>
                <div class="span6">
                </div>
            </div>
            <div id="headerSeparator2">
            </div>
        </div>

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <div class="modal fade" id="ModalADLogin" role="dialog" data-backdrop="static">
                    <div class="modal-dialog">

                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header alert alert-info fade in">
                                <h4 class="modal-title">Login</h4>
                            </div>
                            <div class="modal-body">
                                <table border="1" style="width=100%" cellpadding="5" cellspacing="5">
                                    <tr>
                                        <td>Login Id: 
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLoginId" runat="server" MaxLength="10" ValidationGroup="login" AutoCompleteType="None" Style="border: 1px solid;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rqdfValidLoginId" runat="server" ControlToValidate="txtLoginId" ErrorMessage="*" ValidationGroup="login">
                                            </asp:RequiredFieldValidator>
                                        </td>

                                        <td>Password: 
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="20" ValidationGroup="login" AutoCompleteType="None" Style="border: 1px solid;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rqdfValidPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="*" ValidationGroup="login">
                                            </asp:RequiredFieldValidator>
                                        </td>

                                    </tr>

                                </table>



                            </div>
                            <!-- Modal footer-->
                            <div class="modal-footer">
                                <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Login" CssClass="btn btn-default" ValidationGroup="login" />
                            </div>
                        </div>

                    </div>
                </div>

                <div class="smart-wrap">
                    <div class="smart-forms smart-container wrap-4" id="divSmartContainer" style="margin-top: -115px; margin-bottom: 16px">
                        <div class="form-header header-blue">
                            <h4><i class="fa fa-sign-in"></i>PASS - CMC Docs Viewer</h4>
                            <div id="divLogo" class="LogoCSS">
                                <img src="./Images/logo.jpg" style="height: 70px; width: 230px">
                            </div>
                        </div>
                        <div class="form-body theme-blue" style="padding-top: 16px;">
                            <div class="frm-row">
                                <div class="section colm colm12">



                                    <table id="table1" style="width: 100%;" cellspacing="0" cellpadding="2">
                                        <tr>
                                            <td colspan="6" style="background-color: darkgray; font-size: 14px; color: white; font-weight: bold;">Search By
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdbkg">
                                                <obout:OboutRadioButton ID="rdbtnInwardId" runat="server" Text="Inward Id" GroupName="search" Checked="true"></obout:OboutRadioButton>
                                            </td>
                                            <td class="tdbkg">
                                                <obout:OboutRadioButton ID="rdbtnApplicationNumber" runat="server" Text="Application No." GroupName="search"></obout:OboutRadioButton>
                                            </td>
                                            <td class="tdbkg">
                                                <obout:OboutRadioButton ID="rdbtnCustomerId" runat="server" Text="Customer Id" GroupName="search"></obout:OboutRadioButton>
                                            </td>
                                            <td class="tdbkg">
                                                <obout:OboutRadioButton ID="rdbtnDocumentUniqueNumber" runat="server" Text="Document Unique Number" GroupName="search"></obout:OboutRadioButton>
                                            </td>
                                            <td class="tdbkg">
                                                <obout:OboutRadioButton ID="rdbtnPolicyNumber" runat="server" Text="Policy Number" GroupName="search"></obout:OboutRadioButton>
                                            </td>
                                            <td class="tdbkg">
                                                <obout:OboutRadioButton ID="rdbtnClaimNumber" runat="server" Text="Claim Number" GroupName="search"></obout:OboutRadioButton>
                                            </td>

                                        </tr>
                                        <tr align="center">
                                            <td colspan="6">
                                                <obout:OboutTextBox ID="txtSearchNumber" runat="server" MaxLength="20" Enabled="false" ValidationGroup="search"></obout:OboutTextBox>
                                                <obout:OboutButton ID="btnSearchDocument" runat="server" Text="Search" Enabled="false" OnClick="btnSearchDocument_Click" Width="150px" ValidationGroup="search"></obout:OboutButton>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <asp:RequiredFieldValidator ID="rqdfvalidNumber" runat="server" ControlToValidate="txtSearchNumber" ErrorMessage="Please enter document number" ValidationGroup="search"></asp:RequiredFieldValidator>
                                    <br />






                                </div>




                                <br />

                                <br />






                                <table style="width: 99%">
                                    <tr>
                                        <td style="vertical-align: top; min-width: 450px;" class="auto-style6">


                                            <fieldset id="fldsetDoclist" visible="true" runat="server" style="border-style: solid; border-color: inherit; border-width: 1px; height: 400px; overflow: auto;">
                                               <div class="header"> <center><span style="color:navy;font-weight:bold;margin-top:4px;" class="header">Document List</span></center></div>
                                                <table>
                                                    <tr>
                                                        <td style="width: 450px">



                                                            <asp:Repeater ID="rptViewDocuments" runat="server" OnItemDataBound="rptViewDocuments_ItemDataBound" OnItemCommand="rptViewDocuments_ItemCommand">
                                                                <HeaderTemplate>
                                                                    <table cellspacing="4" rules="all" border="0" id="table21" style="width: 100%">
                                                                        <%--<tr class="header">--%>
                                                                            <%--<th>Sr.No.</th>--%>
                                                                            <%--<th scope="col" style="width: 80px">Inward Id
                                                </th>--%>
                                                                            <%--<th scope="col" style=""></th>--%>
                                                                            <%--<th scope="col" style="width: 120px; text-align: center;">View Document
                                                                            </th>--%>
                                                                        <%--</tr>--%>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="rows">
                                                                        <%--<td style="width:50px;">
                                                <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblInwardId" runat="server" Text='<%# Eval("InwardId") %>' />
                                            </td>--%>
                                                                        <td style="width: 10px">
                                                                            <asp:ImageButton ClientIDMode="AutoID" ID="imgBtnDocumentPath" Width="15px" Height="15px" ImageUrl="Images/download.jpg" runat="server" CommandName="Download" CommandArgument='<%# Eval("DocumentPath") %>'></asp:ImageButton>
                                                                        </td>
                                                                        <td>

                                                                            <asp:LinkButton ClientIDMode="AutoID" ID="lnkViewDocument" runat="server" Text='<%# Eval("DocumentPath") %>' CommandName="View" CommandArgument='<%# Eval("DocumentPath") %>'></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <tr style="background-color: white;">
                                                                        <td colspan="3">
                                                                            <asp:Label ID="lblEmptyData" Text="No Records To Display" runat="server" Visible="false"></asp:Label>
                                                                        </td>
                                                                    </t>
                                     </table>
                                                                </FooterTemplate>
                                                            </asp:Repeater>


                                                        </td>

                                                    </tr>
                                                </table>
                                            </fieldset>

                                        </td>
                                        <td style="height: auto">


                                            <asp:Panel ID="pnlSearchDoc" runat="server" BorderStyle="Solid" BorderColor="Wheat" CssClass="PaneSelected" Height="1000px" style="margin-left:3px;">



                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="paneliframe" runat="server" BackColor="White" Style="position: absolute; height: 1000px; width: 830px; font-family: Verdana; font-size: small;">
                                                                <iframe id="I1" src="<%=  ViewState["FilePath"].ToString() %>" style="height: 99%; width: 99%; top: 6px; left: 3px; position: absolute; margin-top: 0px;"
                                                                    name="I1"></iframe>

                                                            </asp:Panel>

                                                        </td>

                                                    </tr>
                                                </table>

                                            </asp:Panel>

                                        </td>
                                    </tr>
                                </table>

                                <br />


                                <div class="section colm colm12">
                                    <asp:Panel ID="Panel2" runat="server" BorderColor="#727272" BorderWidth="2" Visible="true">


                                        <div style="position: absolute; top: 40%; left: 42%; width: 10%; margin-top: 10px; margin-left: 10px">
                                            <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click"></obout:OboutButton>
                                        </div>
                                    </asp:Panel>
                                </div>

                            </div>
                            <br />
                            <br />




                        </div>


                    </div>



                </div>
                </div>
         
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExit" />
                <asp:PostBackTrigger ControlID="btnLogin" />
            </Triggers>
        </asp:UpdatePanel>

        <asp:UpdateProgress runat="server" ID="PageUpdateProgress">
            <ProgressTemplate>
                <div id="resultLoading">
                    <div>
                        <img alt="" src="Images/ajax-loader.gif"><div>Loading...Please Wait</div>
                    </div>
                    <div class="bg"></div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>


    </form>
</body>
</html>


