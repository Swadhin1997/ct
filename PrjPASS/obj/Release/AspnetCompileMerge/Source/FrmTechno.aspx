<%--<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmTechno.aspx.cs" Inherits="ProjectPASS.FrmTechno" %>--%>
<%@ Page Language="C#" AutoEventWireup="true"CodeBehind="FrmTechno.aspx.cs" Inherits="ProjectPASS.FrmTechno" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>

<form runat="server" style="background-color:white!important;background:url('')!important;">

<%--<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">--%>
    <link href="SmartForms/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="SmartForms/css/smart-themes/blue.css" />
    <link href="SmartForms/css/smart-forms.css" rel="stylesheet" type="text/css" />
    <link href="site/styles/custom.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="Grid/resources/custom_scripts/excel-style/excel-style.css" rel="stylesheet" />

    <style type="text/css">
        
/*custom.css (386)*/
body
{
padding-top:0px!important;
background-color:white;
} 
.smart-forms .form-body 
{
  /*background: url(../../images/mybackend1.png)!important;*/
}
.btn
{
    margin: 0 auto;
}
        .ob_iBC
        {
          font-weight:bold!important;
          font-size:15px!important;
        }
        .ob_iBOv 
         {
          font-weight:bold!important;
          font-size:20px!important;
        }
        .ob_iBCN
        {
          font-weight:bold!important;
          font-size:20px!important;
        }
        
        /* ASPX Button Style */
  .btn2 {
   display: inline-block;
   cursor: pointer;
   text-align: center;
   outline: 1px!important;
   color:black!important;
   font-weight:bold!important;
   background-color: #99CCFF;
   border: none;
   border-radius: 10px!important;
   box-shadow: 0 7px #CCCCCC;
   height:35px!important;
   width:150px!important;
 }

 .btn2:hover {
    background-color: #9999FF
 }

 .btn2:active {
    background-color: #9999FF;
    box-shadow: 0 4px #666;
    transform: translateY(5px);
 }

  .cssActive {

   display: inline-block;
   cursor: pointer;
   text-align: center;
   outline: 1px!important;
   color:black!important;
   font-weight:bold!important;
   
   border: none;
   border-radius: 10px!important;
   height:35px!important;
   width:150px!important;

   background-color: #9999FF;
   box-shadow: 0 4px #666;
   transform: translateY(5px);
 }
        /* End */

        #table1 td 
        {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
           
            font-size: 15px;
            color: #0B0201;
        }
        #tblPnlReg td
        {
            border: 1px solid rgb(199, 198, 198);
            padding: 5px;
            margin-bottom:20px;
            font-size: 15px;
            color: #0B0201;

        }
        .tdbkg {
            background-color: lightgray;
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
            font-size: 16px;
        }
        .rows {
            background-color: #fff;
            font-family: Arial;
            font-size: 15px;
            color: #0B0201;
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


      /*timer*/ 
        .secspan {
            position: absolute;
            left: 50%;
            z-index: 999;
            top: 49%;
            transform: translateX(-50%);
            -webkit-transform: translateX(-50%);
            -o-transform: translateX(-50%);
            -moz-transform: translateX(-50%);
            -ms-transform: translateX(-50%);
            color: #696969;
        }

        /* line 12, ../scss/imports/_timer.scss */
        .timerimg {
            position: absolute;
            z-index: 99;
            left: 0;
        }

        .timercountTxt {
            background: rgba(0, 0, 0, 0) none repeat scroll 0 0;
            color: red !important;
            font-size: 1.6rem;
            border: none !important;
            width: 100%;
            text-align: center;
            padding-bottom: 5px;
        }

        /*.otpPanel {
  width: 400px;
  padding: 20px;
  border: 1px solid #ccc;
  margin: 20px 0;
  float: left;
  clear: both; }*/

        .otpPanel .otpButton {
            margin: 20px 0 0 0;
        }

        .otpPanel p {
            margin: 0;
        }
        /* line 437, ../scss/imports/_motorinsurance.scss */
        .otpPanel .inputBox input {
            padding: 10px 15px 5px 0;
        }


        .timer {
            width: 84px;
            height: 84px;
            margin: auto;
            font-family: 'source_sans_probold';
        }

        .auto-style1 {
            background-color: lightgray;
            font-size: 11px;
            height: 27px;
        }
        .auto-style2 {
            height: 27px;
        }

    </style>

    <script src="js/jquery-3.3.1.min.js" type="text/javascript"></script>
    <script src="js/jquery-ui-1.12.1/jquery-ui.js" type="text/javascript"></script>
    <link href="js/jquery-ui-1.12.1/jquery-ui.css" rel="stylesheet" />
    <link href="css/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <script src="js/bootstrap/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/circular-countdown.js" type="text/javascript"></script>
    
    <script type="text/javascript">

      

        function GetData() {  
                  $.ajax({  
                      type: "POST",  
                      url: "FrmTechnoMobileRegisteration.aspx/Fn_Get_City_And_State_By_Pincode",  
                      data: '',  
                      contentType: "application/json; charset=utf-8",  
                      dataType: "json",  
                      success: function (response) {  
                          alert(response.d);  
                      },  
                  });  
        }  

        $(document).ready(function ()
        {
            <%--var divvali = document.getElementById("<%=btnValidate%>"); 
            
             divvali.style["font-weight"] = "bd!olimportant";
             divvali.style["font-size"] = "20px!important";--%>

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                $("[id$=txtPincode]").change(function () {
                    var obj = {};
                    obj.nPincode = $.trim($("[id*=txtPincode]").val());
                    //obj.age = $.trim($("[id*=txtAge]").val());
                    //contentType: "application/json; charset=utf-8",
                    debugger;
                    $.ajax({
                        type: "POST",
                        url: "FrmTechno.aspx/Fn_Get_City_And_State_By_Pincode",
                        data: JSON.stringify(obj),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            //alert(response.d);
                            var arr = response.d.split("|");
                            $("[id*=txtCity]").val(arr[0]);
                            $("[id*=txtState]").val(arr[1]);

                        },
                    });
                });

            }
        });

         function Confirm(msg)
        {
            debugger;
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm(msg))
            {
                confirm_value.value = "Yes";
                window.location.href="FrmTechno.aspx";
            }
            else
            {
                confirm_value.value = "No";
            }
           //document.forms[0].appendChild(confirm_value);
        }
         
        var count;
        var fnCall;

    function StartTimer() 
    {
        debugger;
       //document.getElementById("<%=hdnCounter.ClientID%>").value = "0";
        var i = parseInt(document.getElementById("<%=hdnCounter.ClientID%>").value);
        console.log(i);
        if (i == 0)
        {
            count = 180;
        }
        else {
           count = i;
        }
        $('#btnMobileVerify').removeAttr('disabled');
        $('#btnMobileVerify').removeClass('disabled');
        $(".timercountTxt").show();
        $('#btnSave').attr('disabled', true);
        $('#btnMobileReSend').attr('disabled', true);
         fnCall = setInterval(timer, 1000); // 1000 will  run it every 1 second
    }
    function timer()
    {  
        debugger;
        
        if (count > 0)
        { 
            count = count - 1;
            sessionStorage.setItem("Counter", count);
            console.log(count);
            document.getElementById("<%=hdnCounter.ClientID%>").value = count;
            document.getElementById("<%=txtTimer.ClientID%>").value = getTime(count);
        }
        else
        {
            $(".timercountTxt").hide();
            $('#btnMobileVerify').addClass('disabled');
            $('#btnMobileVerify').attr('disabled', 'disabled');
            $('#btnSave').removeAttr('disabled');
            $('#btnSave').removeClass('disabled');
            $('#btnMobileReSend').removeAttr('disabled');
            $('#btnMobileReSend').removeClass('disabled');
            document.getElementById("<%=txtTimer.ClientID%>").value = 'Your One Time Password has expired';
            clearInterval(fnCall);
            
        }       
    }

    function getTime(seconds) {

    //a day contains 60 * 60 * 24 = 86400 seconds
    //an hour contains 60 * 60 = 3600 seconds
    //a minut contains 60 seconds
    //the amount of seconds we have left
    var leftover = seconds;

    //how many full days fits in the amount of leftover seconds
    //var days = Math.floor(leftover / 86400);

    //how many seconds are left
   // leftover = leftover - (days * 86400);

    //how many full hours fits in the amount of leftover seconds
    //var hours = Math.floor(leftover / 3600);

    //how many seconds are left
    //leftover = leftover - (hours * 3600);

    //how many minutes fits in the amount of leftover seconds
    var minutes = Math.floor(leftover / 60);

    //how many seconds are left
      leftover = leftover - (minutes * 60);
      var timestamp = '';
      timestamp = minutes + ':' + leftover;
      return timestamp;
}

    function fnValidateOTPNumber(source, args)
        {
            var txtOtpNumber = $(".txtOtpNumber").val();
            args.IsValid = (txtOtpNumber.length > 5);
        }

    function runme()
    {

                $('.timer').circularCountDown({
                    delayToFadeIn: 50,
                    size: 70,
                    fontColor: '#696969',
                    colorCircle: 'transparent',
                    background: '#ffa58c',
                    reverseLoading: false,
                    duration:
                    {
                        seconds: parseInt(179)
                    },
                    beforeStart: function () {
                        $(".timercountTxt").hide();
                        $('#btnMakePayment').attr('disabled', true);
                        $('#btnMobileReSend').attr('disabled', true);
                    },
                    end: function (countdown) {
                        debugger;
                        countdown.destroy();
                        $(".timercountTxt").show();
                        $('#btnMobileVerify').addClass('disabled');
                        $('#btnMobileVerify').attr('disabled', 'disabled');
                        $('#btnMakePayment').removeAttr('disabled');
                        $('#btnMakePayment').removeClass('disabled');
                        $('#btnMobileReSend').removeAttr('disabled');
                        $('#btnMobileReSend').removeClass('disabled');
                        document.getElementById("<%=txtTimer.ClientID%>").value = 'Your One Time Password has expired';
                    }
                });
        }

      </script>

    <asp:UpdatePanel ID="UpdatePanel_OPS" runat="server" UpdateMode="Conditional" ClientIDMode="Static" >    
        <ContentTemplate>   
            <asp:ScriptManager ID="ScriptManager1" runat="server" LoadScriptsBeforeUI="true"
            EnablePartialRendering="true" />
            <script type="text/javascript">
    //try 
    //{
    //    var prm = Sys.WebForms.PageRequestManager.getInstance();
    //    function beginRequest() {
    //        prm._scrollPosition = null;
    //    }
    //    prm.add_beginRequest(beginRequest);


    //}
    //catch (err) {
    //    alert(err);
    //}
</script>
            <div class="smart-wrap" style="background-color:white!important;margin-left:0px!important">
             <div class="smart-forms smart-container wrap-4" style="padding:0px!important;margin-top:0px!important;background-color:white!important;margin-left:0px!important">
                    <div class="form-header header-blue" style="margin-left:0px!important">
                        <h4><i class="fa fa-sign-in"></i>TECNO Mobile Corona Kavach Registration</h4>
                        <div id="divLogo" class="LogoCSS">
                            <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                        </div>
                    </div>
                    <div class="form-body theme-blue" style="background-color:white!important">
                        <div class="frm-row">
                            <div class="section colm colm12">
                                <asp:Panel ID="Panel1" runat="server" BorderColor="" BorderWidth="0">

                                    <table id="table1" style="width: 100%; border-collapse:collapse!important;border:0px none!important;border-color:none!important" cellspacing="0" cellpadding="2" >
                                        <tr>
                                            <td >IMEI No.
                                            </td>
                                            <td class="auto-style2">
                                                <obout:OboutTextBox ID="txtUniqueIdentificationNos" Style="text-transform: uppercase" runat="server" MaxLength="15" TextMode="Number"></obout:OboutTextBox>
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator2" runat="server" Display="Dynamic" ControlToValidate="txtUniqueIdentificationNos" ErrorMessage="Unique Identification No Required..!" ValidationGroup="IMEI_chk" />
                                                <%--<asp:RegularExpressionValidator runat="server" ControlToValidate="txtUniqueIdentificationNos" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Special characters are not allowed..!" ID="rfvname" ValidationExpression="^[\sa-zA-Z0-9]*$" />--%>
                                            </td>
                                            <td >Unique Card No.
                                            </td>
                                            <td class="auto-style2">
                                                <obout:OboutTextBox ID="txtScratchCardUniqueNos" Style="text-transform: uppercase" runat="server" MaxLength="50"></obout:OboutTextBox>
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator4" runat="server" Display="Dynamic" ControlToValidate="txtScratchCardUniqueNos" ErrorMessage="Scratch Card Unique No Required..!" ValidationGroup="IMEI_chk" />
                                            </td>
                                        </tr>
<%--                                        <tr style="height:40px">
                                            <td colspan="4">
                                            </td>
                                        </tr>--%>
                                        <tr>
                                             <td >
                                                Master Policy Holder Name  
                                            </td>
                                            <td colspan="3">
                                                <obout:OboutTextBox ID="txtMasterPolicyHolderName" Style="text-transform: uppercase" runat="server" MaxLength="100" Enabled="false" Text=""></obout:OboutTextBox>
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator6" runat="server" Display="Dynamic" ControlToValidate="txtMasterPolicyHolderName" ErrorMessage="Maste Policy Holder Name Required..!" ValidationGroup="Blnk_chk" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                               <%-- <asp:Button ID="btnValidate" runat="server" Text="Validate/Search" />--%>
                                               <%--<obout:OboutButton ID="btnProceed" runat="server" Text="Proceed" OnClick="btnProceed_Click" />--%>
                                                <asp:Label runat="server" ID="lblMsg" />
                                            </td>
                                        </tr>
                                        </table>
                                   
                                </asp:Panel>
                                <asp:Panel runat="server" ID="pnlReg" Visible="false">
                                      <table runat="server" id="tblPnlReg"> 
                                        <tr><td>Salutation</td>
                                            <td><asp:DropDownList ID="ddlSalutation" runat="server"/>
                                                 <asp:RequiredFieldValidator ID="Requiredfieldvalidator11" runat="server" Display="Dynamic" ControlToValidate="ddlSalutation"
                                                  InitialValue="-Select-" ErrorMessage="Salutation Is Required..!" ValidationGroup="Blnk_chk" />
                                            </td>
                                            <td>Proposer First Name</td>
                                            <td><obout:OboutTextBox ID="txtProposerFirstName" runat="server" MaxLength="20" TextMode="SingleLine"></obout:OboutTextBox>
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator7" runat="server" Display="Dynamic" ControlToValidate="txtProposerFirstName" ErrorMessage="Proposer First Name Is Required..!" ValidationGroup="Blnk_chk" />
                                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtProposerFirstName" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" 
                                                   ErrorMessage="Invalid First Name..!" ID="RegularExpressionValidator1" ValidationExpression="^[a-zA-Z]{3,}$" ValidationGroup="Blnk_chk" />
                                            </td>
                                        </tr>
                                        <tr><td>Proposer Middle Name</td><td><obout:OboutTextBox ID="txtProposerMiddleName" runat="server" MaxLength="20" TextMode="SingleLine"></obout:OboutTextBox>
                                               <%-- <asp:RequiredFieldValidator ID="Requiredfieldvalidator8" runat="server" Display="Dynamic" ControlToValidate="txtProposerMiddleName" ErrorMessage="Proposer Middle Name Is Required..!" ValidationGroup="Blnk_chk" />--%>
                                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtProposerMiddleName" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" 
                                              ErrorMessage="Invalid Middle Name..!" ID="RegularExpressionValidator3" ValidationExpression="^[\sa-zA-Z0-9]*$" ValidationGroup="Blnk_chk" />    
                                        </td>
                                            <td>Proposer Last Name</td>
                                            <td><obout:OboutTextBox ID="txtProposerLastName" runat="server" MaxLength="20" TextMode="SingleLine"></obout:OboutTextBox>
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator9" runat="server" Display="Dynamic" ControlToValidate="txtProposerLastName" ErrorMessage="Proposer Last Name Is Required..!" ValidationGroup="Blnk_chk" />
                                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtProposerLastName" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" 
                                                 ErrorMessage="Invalid Last Name..!" ID="RegularExpressionValidator2" ValidationExpression="^[A-Za-z\/\s\']{3,}$" ValidationGroup="Blnk_chk" />
                                            </td>
                                        </tr>
                                        <tr><td>Date Of Birth</td>
                                            <td>
                                            DD<asp:DropDownList ID="ddlDOBDD" runat="server" Width="50px"></asp:DropDownList>
                                            MM<asp:DropDownList ID="ddlDOBMM" runat="server"  TextMode="Number" Width="50px"></asp:DropDownList>
                                            YYYY<asp:DropDownList ID="ddlDOBYYYY" runat="server" TextMode="Number" Width="80px"></asp:DropDownList>
                                               <asp:RequiredFieldValidator ID="Requiredfieldvalidator5" runat="server" Display="Dynamic" InitialValue="00" ControlToValidate="ddlDOBDD" ErrorMessage="Day Is Required..!" ValidationGroup="Blnk_chk" />
                                               <asp:RequiredFieldValidator ID="Requiredfieldvalidator8" runat="server" Display="Dynamic" InitialValue="00" ControlToValidate="ddlDOBMM" ErrorMessage="Month Is Required..!" ValidationGroup="Blnk_chk" />
                                               <asp:RequiredFieldValidator ID="Requiredfieldvalidator10" runat="server" Display="Dynamic"  InitialValue="0000" ControlToValidate="ddlDOBYYYY" ErrorMessage="Year Is Required..!" ValidationGroup="Blnk_chk" />
                                           <%-- <asp:RegularExpressionValidator runat="server" ControlToValidate="txtDOBDD"
                                                ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Enter Valid Day."
                                                ID="RegularExpressionValidator5" ValidationExpression="^[0-9][31]$" />
                                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtDOBMM"
                                                ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Enter Valid Month. "
                                                ID="RegularExpressionValidator6" ValidationExpression="^[0-9][12]$" />
                                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtDOBYYYY"
                                                ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Enter valid Year."
                                                ID="RegularExpressionValidator7" ValidationExpression="^[0-9][]$" />--%>
                                            </td>
                                            <td>Gender</td><td><asp:DropDownList ID="ddlGender" runat="server" /> 
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator13" runat="server" Display="Dynamic" ControlToValidate="ddlGender" 
                                                  InitialValue="-Select-" ErrorMessage="Gender Is Required..!" ValidationGroup="Blnk_chk" />
                                            </td>
                                        </tr>
                                         <tr><td>Marital Status</td>
                                            <td>
                                            <asp:DropDownList ID="ddlMaritalStatus" runat="server"/>
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator14" runat="server" Display="Dynamic" ControlToValidate="ddlMaritalStatus"
                                                  InitialValue="-Select-" ErrorMessage="Marital Status Is Required..!" ValidationGroup="Blnk_chk" />
                                            </td>
                                            <td>Occupation</td>
                                            <td><asp:DropDownList ID="ddlOccupation" runat="server"/>
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator15" runat="server" Display="Dynamic" ControlToValidate="ddlOccupation"
                                                  InitialValue="-Select-" ErrorMessage="Occupation Is Required..!" ValidationGroup="Blnk_chk" />
                                            </td>
                                        </tr>

                                         <tr><td>Nominee Name</td>
                                            <td>
                                            <obout:OboutTextBox ID="txtNomineeName" runat="server" MaxLength="50" TextMode="SingleLine"></obout:OboutTextBox>
                                            <asp:RequiredFieldValidator ID="Requiredfieldvalidator17" runat="server" Display="Dynamic" ControlToValidate="txtNomineeName"
                                               ErrorMessage="Nominee Name Is Required..!" ValidationGroup="Blnk_chk" />

                                            </td>
                                            <td>Nominee Relationship</td>
                                            <td><asp:DropDownList ID="ddlNomineeRel" runat="server"/>  
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator16" runat="server" Display="Dynamic" ControlToValidate="ddlNomineeRel"
                                                  InitialValue="-Select-" ErrorMessage="Nominee Relation Is Required..!" ValidationGroup="Blnk_chk" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Customer ID Proof</td>
                                            <td><asp:DropDownList ID="ddlCustomerIdProof" runat="server"/> 
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator18" runat="server" Display="Dynamic" ControlToValidate="ddlCustomerIdProof"
                                                  InitialValue="-Select-" ErrorMessage="Customer Id Proof Is Required..!" ValidationGroup="Blnk_chk" />
                              
                                            </td>
                                            <td>ID Proof No.</td>
                                            <td>
                                            <obout:OboutTextBox ID="txtUniqueProofNos" runat="server" MaxLength="16" TextMode="SingleLine"></obout:OboutTextBox>
                                             <asp:RequiredFieldValidator ID="Requiredfieldvalidator19" runat="server" Display="Dynamic" ControlToValidate="txtUniqueProofNos"
                                                   ErrorMessage="Unique Proof Nos Is Required..!" ValidationGroup="Blnk_chk" />
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtUniqueProofNos" Display="Dynamic"
                                            ErrorMessage="Enter valid document number" OnServerValidate="Fn_Validate_Id_Proof" />
                                            </td>
                                        </tr>

                                         <tr><td>Purchase Date</td>
                                            <td>
                                            DD
                                            <asp:DropDownList ID="ddlPDD" runat="server" TextMode="Number" style="width:45px!important" AutoPostBack="true"></asp:DropDownList>
                                            
                                            MM
                                            <asp:DropDownList ID="ddlPMM" runat="server" TextMode="Number" style="width:45px!important" AutoPostBack="true"></asp:DropDownList>
                                           
                                            YYYY
                                            <asp:DropDownList ID="ddlPYYYY" runat="server" style="width:80px!important" AutoPostBack="true"></asp:DropDownList>
                                             
                                        <asp:RequiredFieldValidator ID="Requiredfieldvalidator20" runat="server" Display="Dynamic" ControlToValidate="ddlPDD"
                                           InitialValue="00" ErrorMessage="Purchase Date Year Is Required..!" ValidationGroup="Blnk_chk" />
                                        <asp:RequiredFieldValidator ID="Requiredfieldvalidator21" runat="server" Display="Dynamic" ControlToValidate="ddlPMM"
                                          InitialValue="00" ErrorMessage="Purchase Date Month Is Required..!" ValidationGroup="Blnk_chk" />
                                        <asp:RequiredFieldValidator ID="Requiredfieldvalidator22" runat="server" Display="Dynamic" ControlToValidate="ddlPYYYY"
                                          InitialValue="0000"  ErrorMessage="Purchase Date Year Is Required..!" ValidationGroup="Blnk_chk" />

                                        <asp:CustomValidator ID="CVPurchaseDate" runat="server" ControlToValidate="ddlPYYYY" display="Dynamic"
                                            ErrorMessage="" OnServerValidate="Fn_Validate_PurchaseDate" />
                                            </td>
                                            <td>Invoice No.</td>
                                             <td>
                                                <obout:OboutTextBox ID="txtInvoiceNos" runat="server" MinLength="3" MaxLength="35" TextMode="SingleLine" ></obout:OboutTextBox>
                                                  <asp:RegularExpressionValidator runat="server" ControlToValidate="txtInvoiceNos" ForeColor="Red" 
                                                      SetFocusOnError="true" Display="Dynamic" ErrorMessage="Invalid Invoice Number..!"
                                                      ID="RegularExpressionValidator5" ValidationExpression="^[\sa-zA-Z0-9]*$" ValidationGroup="Blnk_chk" />
                                            </td>
                                        </tr>

                                         <tr><td>Address Line1</td>
                                            <td>
                                            <obout:OboutTextBox ID="txtAddr1" runat="server" MaxLength="50" TextMode="SingleLine"></obout:OboutTextBox>
                                            <asp:RequiredFieldValidator ID="Requiredfieldvalidator23" runat="server" Display="Dynamic" ControlToValidate="txtAddr1"
                                            SetFocusOnError="true" ErrorMessage="Address Is Required..!" ValidationGroup="Blnk_chk" />

                                            </td>
                                            <td>Address Line2</td>
                                             <td>
                                            <obout:OboutTextBox ID="txtAddr2" runat="server" MaxLength="50" TextMode="SingleLine"></obout:OboutTextBox>
                                            </td>
                                        </tr>

                                          <tr><td>Landmark (If Any)</td>
                                            <td>
                                            <obout:OboutTextBox ID="txtAddr3" runat="server" MaxLength="50" TextMode="SingleLine"></obout:OboutTextBox>
                                            </td>
                                            <td>Pincode</td>
                                             <td>
                                            <asp:TextBox ID="txtPincode" runat="server" MaxLength="6" TextMode="Number" />
                                            <asp:RequiredFieldValidator ID="Requiredfieldvalidator24" runat="server" Display="Dynamic"
                                            ControlToValidate="txtPincode" ErrorMessage="Pincode Is Required..!" ValidationGroup="Blnk_chk" />
                                             <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPincode" ForeColor="Red" 
                                              SetFocusOnError="true" Display="Dynamic" ErrorMessage="Invalid Pincode Number..!"
                                              ID="RegularExpressionValidator6" ValidationExpression="^[1-9][0-9]{5}$" ValidationGroup="Blnk_chk" />                                 
                                            </td>
                                            
                                        </tr>
                                          <tr>
                                            <td>City</td>
                                            <td>
                                              <asp:TextBox ID="txtCity" runat="server" MaxLength="25" TextMode="SingleLine" />
                                            </td>
                                             <td>State</td>
                                             <td>
                                            <asp:TextBox ID="txtState" runat="server" MaxLength="25" TextMode="SingleLine" />
                                             </td>
                                          </tr>

                                        <tr>
                                            <td class="">Mobile No.(+91)
                                            </td>
                                            <td>
                                                <obout:OboutTextBox ID="txtMobileNos" runat="server" MaxLength="10" TextMode="Number"></obout:OboutTextBox>
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator3" runat="server" Display="Dynamic" ControlToValidate="txtMobileNos" ErrorMessage="Mobile Nos Required..!" ValidationGroup="Blnk_chk" />
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" Display="Dynamic" ValidationGroup="Blnk_chk"
                                                  SetFocusOnError="true" ErrorMessage="Please enter valid Mobile No." ValidationExpression="^(0/91)?[7-9][0-9]{9}" ControlToValidate="txtMobileNos" >
                                                </asp:RegularExpressionValidator>
                                                 <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="txtMobileNos" display="Dynamic"
                                            ErrorMessage="" OnServerValidate="Fn_Validate_MobileNo" />
                                            </td>
                                            <td class="">Email ID
                                            </td>
                                            <td>
                                                <obout:OboutTextBox ID="txtEmailId" runat="server" MaxLength="100"></obout:OboutTextBox>
                                                <asp:RegularExpressionValidator ID="emailValidator" runat="server" Display="Dynamic" ValidationGroup="Blnk_chk"
                                                 SetFocusOnError="true" ErrorMessage="Please, enter valid e-mail address." ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$"
                                                    ControlToValidate="txtEmailId">
                                                </asp:RegularExpressionValidator>
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator1" runat="server" Display="Dynamic" ControlToValidate="txtEmailId" ErrorMessage="Please, enter an e-mail!" ValidationGroup="Blnk_chk" />
                                            </td>
                                        </tr>
                                         <tr>
                                             <td colspan="4">
                                                <obout:OboutCheckBox ID="chkbConfirm" runat="server" Checked="false" OnCheckedChanged="chkbConfirm_CheckedChanged" AutoPostBack="true" />
                                                 I confirm above information is true & to the best of my knowledge.
                                               <%-- <asp:RequiredFieldValidator ID="Requiredfieldvalidator11" runat="server"  Display="Dynamic"
                                                 ControlToValidate="chkbConfirm" ErrorMessage="Please, Check the box!" ValidationGroup="Blnk_chk" />--%>
                                             </td>
                                         </tr>
                                        <tr>
                                             
                                             <td colspan="4">
                                                 <obout:OboutCheckBox ID="chkbAgree" runat="server" Checked="false" OnCheckedChanged="chkbAgree_CheckedChanged" AutoPostBack="true">
                                                 </obout:OboutCheckBox>
                                                I agree to the Terms & Conditions.
                                                <%-- <asp:RequiredFieldValidator ID="Requiredfieldvalidator12" runat="server" Display="Dynamic" ControlToValidate="chkbAgree" ErrorMessage="Please, Check the box!" ValidationGroup="Blnk_chk" />--%>
    
                                             </td>
                                            </tr>
                                        <tr>
                                            <td colspan="4">Important Note</td>
                                        </tr>
                                         <tr>
                                            <td colspan="4">1. In case of multiple registration of devices bought under ownership of single Individual, Kotak General Insurance Company's liability under the policy will be restricted to Policy with highest SI for the settlement of claim & not the total sum insured covered under all Policy.</td>
                                        </tr>
                                         <tr>
                                            <td colspan="4">2. In case of Multiple SIM, Primary IMEI No. need to be entered above.</td>
                                        </tr>
                                         <tr>
                                            <td colspan="4">3. Registration of Policy is only for Individual Customer & not for the customer where Invoice is in the name of Company.</td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">4. All fields on this page are mandatory.</td>
                                        </tr>                           
                                    </table>
                                    
                                </asp:Panel>
                            </div>
               <div class="section colm colm12">

                <script type="text/javascript">
                    /*Region TIMER */

                 </script>

                <div  id="captchaPanel" runat="server" visible="false">
                    <div style="margin-bottom:20px;float:left">
                        <asp:Image ID="imgCaptcha" runat="server" src="Captcha.aspx" Height="55px" Width="186px" /> 
                    </div>                                  
                    <div style="margin-left:10px!important;margin-bottom:10px">
                        <asp:TextBox ID="txtCaptcha"  style="margin-left:20px" runat="server"></asp:TextBox>
                    </div>
                    <div style="text-align:center;">
                       <asp:Label runat="server" ID="lblSuccessMsg"></asp:Label>
                    </div>
                </div>

                <div class="otpPanel align-left text-center" id="otpPanel" visible="false" runat="server">          
                    <asp:Label runat="server" id="lblTimer" Visible="false" />
                    <asp:Label ID="lblCounter" runat="server" ClientIDMode="Static" Text="" Visible="false" />
                   <asp:HiddenField ID="hdnCounter" Visible="true" runat="server" ClientIDMode="Static" Value="0" />
        
                    <asp:HiddenField ID="hdnOTPSentCount" runat="server" ClientIDMode="Static" Value="0" />
                            <asp:TextBox ID="txtTimer" ClientIDMode="Static" Style="display: none" ReadOnly="true" runat="server" Text="" CssClass="timercountTxt"></asp:TextBox>  
                            <!-- TIMER STARTS HERE -->
                            <br />
                            <div class="timer" id="otptimer" style="display:none">
                                <span class="secspan">sec</span>
                            </div>
                            <!-- TIMER ENDS HERE -->
                            <p>Please enter the One Time Password (OTP) sent on your mobile number</p>
                            <div class="inputBox" style="margin-top:20px">
                                <asp:TextBox ID="txtOtpNumber" runat="server" CssClass="txtOtpNumber" Text="" MaxLength="6" style="height:25px" /><br />
                                <asp:CustomValidator ID="cvtxtOtpNumber" runat="server" ValidationGroup="otp" Display="Dynamic" 
                                ErrorMessage="Please provide valid OTP number" ClientValidationFunction="fnValidateOTPNumber" OnServerValidate="OnServerValidatecvtxtOtpNumber" />
                            </div>
                            <div class="otpButton align-center">
                                <asp:Button ID="Button1" runat="server" Visible="false" CssClass="btn btn-info" style="background:#87CEEB !important;font-size:1.9rem;border-radius:8px;height:40px" ClientIDMode="Static" Text="TEST BUTTON" ValidationGroup="test" AutoPostBack="True" OnClick="Button1_Click" />
                                <asp:Button ID="btnMobileVerify" runat="server" CssClass="btn btn-info" style="background:#87CEEB !important;font-size:1.9rem;border-radius:8px;height:40px" ClientIDMode="Static" Text="Verify OTP" ValidationGroup="otp" OnClick="onClickbtnMobileVerify" AutoPostBack="True" />
                                <asp:Button ID="btnMobileReSend" runat="server" CssClass="btn btn-info" style="background:#87CEEB !important;font-size:1.9rem;border-radius:8px;height:40px" ClientIDMode="Static" Text="Resend OTP" ValidationGroup="summary" OnClick="onClickbtnMobileReSend" AutoPostBack="True" />
                            <asp:HiddenField ID="hdnIsVerifyBtnClicked" runat="server" Value="0" />
                            </div>                                          
                </div>
              
                <div style="margin-top:20px;text-align:center">
           <%--     <obout:OboutButton ID="btnSave" runat="server" Visible="false" Enabled="false" ClientIDMode="Static" Text="Submit Information" OnClick="btnSave_Click" ValidationGroup="Blnk_chk" CssClass="btn btn-info" />                               
                    <obout:OboutButton ID="btnValidate" runat="server" Text="Validate/Search" OnClick="btnValidate_Click" ValidationGroup="IMEI_chk" AutoPostback ="True" Height="40px" />
                    <obout:OboutButton ID="OboutReset" runat="server" Text="Reset" Visible="true"  OnClick="OboutReset_Click" CssClass="btn" />                                
                    <obout:OboutButton ID="btnExit" runat="server" Text="Exit" OnClick="btnExit_Click" Visible="false" CssClass="btn" />
                    <obout:OboutButton ID="btnRegister" runat="server" Text="Register Policy"  ClientIDMode="Static" OnClick="btnRegister_Click" Visible="false" ValidationGroup="Blnk_chk" CssClass="btn btn-info" />--%>
                    <asp:Button ID="btnSave" runat="server" Visible="false" Enabled="false" ClientIDMode="Static" Text="Submit Information" OnClick="btnSave_Click" ValidationGroup="Blnk_chk" CssClass="btn2" />                               
                    <asp:Button ID="btnValidate" runat="server" Text="Validate/Search" OnClick="btnValidate_Click" ValidationGroup="IMEI_chk" AutoPostback ="True" CssClass="btn2" />
                    <asp:Button ID="OboutReset" runat="server" Text="Reset" Visible="true"  OnClick="OboutReset_Click" CssClass="btn2" />                                
                    <asp:Button ID="btnRegister" runat="server" Text="Register Policy"  ClientIDMode="Static" OnClick="btnRegister_Click" Visible="false" ValidationGroup="Blnk_chk" CssClass="btn2" />
                </div>
                <div>
                    <asp:Label ID="lblstatus" runat="server" />
                    <asp:HiddenField ID="HiddenField1" runat="server" /> 
                    <asp:HiddenField ID="HdFldSave" runat="server" />
                 </div>
               </div>        
               </div>
              </div>
            </div>
            </div>
         
        </ContentTemplate>
        <Triggers>
           <%-- <asp:PostBackTrigger ControlID="chkbConfirm" />
            <asp:PostBackTrigger ControlID="chkbAgree" />--%>
           <%-- <asp:PostBackTrigger ControlID="btnExit" />--%>
           <asp:AsyncPostBackTrigger ControlID="OboutReset" />
        </Triggers>
    </asp:UpdatePanel>
  <%--   <asp:Timer runat="server" id="UpdateTimer" Enabled="false" interval="179" ontick="UpdateTimer_Tick" /> 
    <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional" ChildrenAsTriggers="false" >
        <ContentTemplate>
        </ContentTemplate> 
        <Triggers>
        <asp:AsyncPostBackTrigger controlid="UpdateTimer" eventname="Tick" />
        </Triggers>
    </asp:UpdatePanel> --%>       
<%--</asp:Content>--%>
   </form>
