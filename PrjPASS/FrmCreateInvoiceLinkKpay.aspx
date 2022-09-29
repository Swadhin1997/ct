<%@ Page Title="" Language="C#" MasterPageFile="~/PASS.Master" AutoEventWireup="true" CodeBehind="FrmCreateInvoiceLinkKpay.aspx.cs" Inherits="PrjPASS.FrmCreateInvoiceLinkKpay" %>

<%--<%@ Page Title="" Language="C#" MasterPageFile="~/PASS.Master" AutoEventWireup="true" CodeBehind="FrmCreateInvoiceLink.aspx.cs" Inherits="PrjPASS.FrmCreateInvoiceLink" %>--%>

<%--<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>--%>

<%@ Register TagPrefix="obout" Namespace="OboutInc.Calendar2" Assembly="obout_Calendar2_NET" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divMain").css('position', 'relative');
            $("#divMain").css('left', '0%');
            $("#divMain").css('width', '100%');
            $("#divMain").css('top', '0%');
            //$("#divSmartContainer").css('margin-top', '-30px');
            $("#divSmartContainer").css('margin-bottom', '16px');
            //$("#divLogo").css('top', '-3px');
            //$("#decorative2").css('height', '1px');
        });
        

        //var formSubmitting = false;
        //var setFormSubmitting = function () { formSubmitting = true; };

        //var isChrome = /Chrome/.test(navigator.userAgent) && /Google Inc/.test(navigator.vendor);
        

        //window.onload = function () {
        //    window.addEventListener("beforeunload", function (e) {
        //        if (formSubmitting) {
        //            formSubmitting = false;
        //            return false;
        //        }

        //        if (isChrome) {
        //            var confirmationMessage = '';
        //        }
        //        else {
        //            var confirmationMessage = 'Are you sure you want to leave this page?';
        //        }

        //        (e || window.event).returnValue = confirmationMessage; //Gecko + IE
        //        return confirmationMessage; //Gecko + Webkit, Safari, Chrome etc.

        //    });
        //};

        //function test() {
        //    var popup = $find('');
        //    popup.add_shown(SetzIndex);
        //}


        function Reset() {

            document.getElementById('<%=lnkGeneratedLinkFull.ClientID%>').innerText = '';
            document.getElementById('<%=lnkGeneratedLinkShort.ClientID%>').innerText = '';
        }

    </script>

    <style type="text/css">
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

   

    <script type="text/javascript">
        $(function () {
            
        });

        $(document).ready(function () {
            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(openModal);
        });


        function openModal() {
            $('#myModal').modal('show');
        }

        function openModalSuccess() {
            $('#myModalSuccess').modal('show');
        }  
      
        function TestTest()
        {
            alert('Invoice Creation will take few minutes, please wait till file gets downloaded');
            <%--$("#<%= btnCreateInvoiceBulk.ClientID %>").val('Loading...');--%>
        }
       
 

        function ShowProgress() {
            document.getElementById('<% Response.Write(PageUpdateProgress.ClientID); %>').style.display = "block";
        }

        function HideProgress() {
            document.getElementById('<% Response.Write(PageUpdateProgress.ClientID); %>').style.display = "none";
            $('#myModalSuccess').modal('show');
        }

    </script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MstCntFormContent" runat="server">

 
 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
          
            

             <div class="modal fade" id="myModalSuccess" role="dialog" data-backdrop="static">
                        <div class="modal-dialog">

                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header alert alert-info fade in">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title">Status</h4>
                                </div>
                                <div class="modal-body">
                                     <asp:Label ID="lblStatusSuccess" runat="server" style="color:red" /><a href="#" runat="server" id="lnkDownloadLink" target="_blank" style="font-size:14px"></a>
                                </div>
                                <!-- Modal footer-->
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
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

            <div class="smart-wrap">
                <div class="smart-forms smart-container wrap-4" id="divSmartContainer" style="margin-top: -115px; margin-bottom: 16px">
                    <div class="form-header header-blue">
                        <h4><i class="fa fa-sign-in"></i>PASS - Create Invoice</h4>
                        <div id="divLogo" class="LogoCSS">
                         <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                        </div>
                    </div>
                    <div class="form-body theme-blue" style="padding-top: 16px;">
                        <div class="frm-row">
                            <div class="section colm colm12">
                                <%-- <asp:Panel ID="Panel1" runat="server" BorderWidth="0px">
                                    <asp:ValidationSummary CssClass="validationsummary" ValidationGroup="vp" runat="server"
                                        HeaderText="<div class='validationheader'>&nbsp;Please Correct the following Errors:</div>" />
                                    <br />

                                </asp:Panel>--%>

                                <table id="table1" style="width: 100%;" cellspacing="0" cellpadding="2">
                                     <tr>
                                                       <td class="tdbkgHead" colspan="6" style="font-size:15px">Create Invoice (Single)
                                                        </td>
                                         </tr>
                                                    <tr>
                                                       <td class="tdbkg">Transaction Id
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtTransactionId" runat="server" Text=""></obout:OboutTextBox>
                                                        </td>

                                                         <td class="tdbkg">Product Info
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtProductInfo" runat="server" Text=""></obout:OboutTextBox>
                                                        </td>

                                                        
                                                          <td class="tdbkg">Amount
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtAmount" runat="server" Text="0"></obout:OboutTextBox>
                                                        </td>

                                                    </tr>
                                     <tr>
                                                       <td class="tdbkg">Customer Name
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtCustomerName" runat="server" Text=""></obout:OboutTextBox>
                                                        </td>

                                                         <td class="tdbkg">Email Address
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtCustEmailAddress" runat="server" Text=""></obout:OboutTextBox>
                                                        </td>

                                                        
                                                          <td class="tdbkg">Mobile Number
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtCustMobileNumber" runat="server" Text=""></obout:OboutTextBox>
                                                        </td>

                                                    </tr>
                                                 <tr>
                                                       <td class="tdbkg">Validation Period (In Days)
                                                        </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtValidationPeriod" runat="server" Text="1"></obout:OboutTextBox>
                                                        </td>

                                                       <td class="tdbkg"> Merchant Reference No 1  </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtMarchantReferrenceNo1" runat="server" ></obout:OboutTextBox>
                                                        </td>
                                                        <td class="tdbkg"> Merchant Reference No 2  </td>
                                                        <td>
                                                           <obout:OboutTextBox ID="txtMarchantReferrenceNo2" runat="server" ></obout:OboutTextBox>
                                                        </td>
                                                </tr>
                                                <tr>
                                                          <td class="tdbkg">Is Create Short URL
                                                        </td>
                                                        <td>
                                                           <obout:OboutCheckBox ID="chkSendShortURL" runat="server"></obout:OboutCheckBox>
                                                        </td>

                                                          <td class="tdbkg">
                                                              <span style="display:none;">Is Send KPay SMS</span>
                                                        </td>
                                                        <td>
                                                            <obout:OboutCheckBox ID="chkSendKPaySMS" runat="server" Visible="false"></obout:OboutCheckBox>
                                                        </td>

                                                    </tr>
                                       <tr style="display:none">
                                                     

                                                         <td class="tdbkg">
                                                             <span style="display:none;">Is Send KGI Email</span>
                                                        </td>
                                                        <td>
                                                            <obout:OboutCheckBox ID="chkSendKGIEmail" runat="server" Visible="false"></obout:OboutCheckBox>
                                                        </td>

                                                        
                                                          <td class="tdbkg"><span style="display:none;">Is Send KGI SMS</span>
                                                        </td>
                                                        <td>
                                                            <obout:OboutCheckBox ID="chkSendKGISMS" runat="server" Visible="false"></obout:OboutCheckBox>
                                                        </td>
                                           
                                            <td class="tdbkg">
                                                             <span style="display:none;">Is Send KPay Email</span>
                                                        </td>
                                                        <td>
                                                            <obout:OboutCheckBox ID="chkSendKPayEmail" runat="server" Visible="false"></obout:OboutCheckBox>
                                                        </td>

                                                    </tr>
                                    <tr>
                                        <td class="tdbkg">Generated Link:</td>
                                        <td class="tdbkg" colspan="4">
                                            <a id="lnkGeneratedLinkFull" runat="server" href="#" target="_blank" ></a>
                                            &nbsp;&nbsp;&nbsp;
                                            <a id="lnkGeneratedLinkShort" runat="server" href="#" target="_blank" ></a>
                                        </td>
                                       <td class="tdbkg" colspan="1" align="right">
                                           <obout:OboutButton ID="btnCreateInvoiceSingle" OnClientClick="Reset();" runat="server" Text="Create Invoice" OnClick="btnCreateInvoiceSingle_Click"></obout:OboutButton>
                                       </td>
                                    </tr>
                                    
                                                </table>
                                
                                <hr />
                                 <%--BULK SCHEDULER STARTS--%>
                                <table id="table33" style="width: 100%;" cellspacing="0" cellpadding="2">
                                     <tr>
                                                       <td class="tdbkgHead" colspan="6" style="font-size:15px">Create Invoice (Bulk By Scheduler) 
                                                        </td>
                                         </tr>
                                      <tr>
                                                     

                                                         <td class="tdbkg">Upload Excel For Bulk Invoice Links 
                                                            &nbsp;&nbsp;&nbsp;
                                                             <a id="lnkDownLoadSampleScheduler" href="https://kgipass.kotakgeneralinsurance.com/KGIPASS/ExcelTemplate/KPayBulkCreateInvoiceTemplate.xlsx" target="_blank">Download Sample Format</a>
                                                             
                                                        </td>
                                                        <td class="tdbkg" colspan="4">
                                                            <asp:FileUpload ID="FileUploadBulkInvoiceByScheduler" runat="server"  />
                                                        </td>
                                                        <td class="tdbkg" colspan="1" align="right">
                                                            <obout:OboutButton ID="btnCreateInvoiceBulkByScheduler" runat="server" Text="Upload Bulk Invoice Data File" OnClick="btnCreateInvoiceBulkByScheduler_Click" OnClientClick="return ShowProgress();"></obout:OboutButton>
                                                        </td>
                                            
                                                    </tr>
                                </table>
                                <br />
                                

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

                                <%--BULK SCHEDULER ENDS--%>
                                
                                <hr />
                              <table id="table31" style="width: 100%;" cellspacing="0" cellpadding="2">
                                     <tr>
                                                       <td class="tdbkgHead" colspan="6" style="font-size:15px">Create Invoice (Bulk upto 100 Records) 
                                                        </td>
                                         </tr>
                                      <tr>
                                                     

                                                         <td class="tdbkg">Upload Excel For Bulk Invoice Links 
                                                            &nbsp;&nbsp;&nbsp;
                                                             <a id="lnkDownLoadSample" href="https://kgipass.kotakgeneralinsurance.com/KGIPASS/ExcelTemplate/KPayBulkCreateInvoiceTemplate.xlsx" target="_blank">Download Sample Format</a>
                                                             
                                                        </td>
                                                        <td class="tdbkg" colspan="4">
                                                            <asp:FileUpload ID="FileUploadBulkInvoice" runat="server"  />
                                                        </td>
                                                        <td class="tdbkg" colspan="1" align="right">
                                                            <obout:OboutButton ID="btnCreateInvoiceBulk" runat="server" Text="Create Bulk Invoice" OnClick="btnCreateInvoiceBulk_Click" OnClientClick="return ShowProgress();"></obout:OboutButton>
                                                            <%--<asp:Button ID="btnCreateInvoiceBulk" runat="server" CssClass="btn btn-primary" Text="Create Bulk Invoice" OnClick="btnCreateInvoiceBulk_Click" OnClientClick="TestTest();" />--%>
                                                        </td>
                                            
                                                    </tr>
                                </table>
                                <br />
                                <div style="">
                                <table id="table32" style="width: 100%;" cellspacing="0" cellpadding="2">
                                     <tr>
                                                       <td class="tdbkgHead" colspan="6" style="font-size:15px">Search Transaction
                                                        </td>
                                         </tr>
                                      <tr>
                                                     

                                                         <td class="tdbkg">Search Transaction
                                                            <obout:OboutTextBox ID="txtSearchTransaction" runat="server"></obout:OboutTextBox>
                                                             <obout:OboutButton ID="btnSearchTransaction" runat="server" Text="Search Transaction" OnClick="btnSearchTransaction_Click"></obout:OboutButton>
                                                        </td>
                                                    </tr>
                                    <tr>
                                        <td class="tdbkg">
                                            <asp:GridView ID="QuoteGridView" runat="server" AutoGenerateColumns="false" ItemType="ProjectPASS.KPaySavedDetails"
                                        SelectMethod="QuoteGridView_GetData" 
                                        DataKeyNames="TransactionId" CssClass="mydatagrid" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header"
                                        RowStyle-CssClass="rows" AllowPaging="true" OnPageIndexChanging="QuoteGridView_PageIndexChanging" PageSize="10">
                                        <Columns>
                                            
                                            <asp:BoundField DataField="TransactionId" HeaderText="Transaction Id" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="ProductInfo" HeaderText="Product Info" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="Amount" HeaderText="Amount" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="CustomerEmailId" HeaderText="Customer Email" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="CustomerMobileNumber" HeaderText="Customer Mobile Number" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="ValidationPeriod" HeaderText="Validation Period" HeaderStyle-Font-Size="Smaller" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                           
                                           <asp:TemplateField HeaderText="PayInvoiceURL" HeaderStyle-Font-Size="Smaller">
                                             <ItemTemplate>
                                                  <div style="width: 140px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                      <a href="<%#Eval("PayInvoiceURL") %>" target="_blank"><%#Eval("PayInvoiceURL") %></a>
                                                  </div>
                                             </ItemTemplate>
                                            </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="ShortURL" HeaderStyle-Font-Size="Smaller">
                                             <ItemTemplate>
                                                  <div style="width: 140px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
                                                      <a href="<%#Eval("ShortURL") %>" target="_blank"><%#Eval("ShortURL") %></a>
                                                  </div>
                                             </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                
                                
                                    </div>
                               
                                <br />
                                <br />
                                 
                                <br />
                            </div>
                            <div class="section colm colm12">
                                <asp:Panel ID="Panel2" runat="server" BorderColor="#727272" BorderWidth="2" Visible="true">
                                  

                                    <div style="position: absolute; top: 40%; left: 42%; width: 10%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click"></obout:OboutButton>
                                    </div>
                                </asp:Panel>
                            </div>



                        </div>

                         

                       





                    </div>
                </div>
            </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExit" />
            <asp:PostBackTrigger ControlID="btnCreateInvoiceBulk" />
            <asp:PostBackTrigger ControlID="btnCreateInvoiceBulkByScheduler" />
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

</asp:Content>



