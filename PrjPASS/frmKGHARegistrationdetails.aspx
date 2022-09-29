<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmKGHARegistrationdetails.aspx.cs" Inherits="PrjPASS.frmKGHARegistrationdetails" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kotak Group Health Assure</title>

    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="description" content="Bootstrap Admin App + jQuery" />
    <meta name="keywords" content="app, responsive, jquery, bootstrap, dashboard, admin" />
    <!-- =============== VENDOR STYLES ===============-->
    <!-- FONT AWESOME-->
    <link rel="stylesheet" href="css/newcssjs/fontawesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="css/newcssjs/bootstrap.css" id="bscss" />
    <!-- =============== APP STYLES ===============-->
    <link rel="stylesheet" href="css/newcssjs/app_stp.css" id="maincss" />
    <link href="css/newcssjs/jquery-ui.css" rel="stylesheet" />
</head>


<body style="background-color: white;">

    <script src="css/newcssjs/js/jquery.js"></script>
    <script src="css/newcssjs/js/jquery-ui.js"></script>
    <script src="css/newcssjs/js/circular-countdown.js"></script>


    <style>
        .secspan {
            position: absolute;
            left: 42%;
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


        .error {
            font-size: 1.3rem;
            color: red;
            float: left;
            clear: both;
            padding-top: 5px;
        }

        /*.btn {
            border-radius: 8px;
            color: #bbaf99;
            float: left;
            position: relative;
            font-size: 1.6rem;
            padding: 15px 30px;
            margin: 10px 0;
        }*/

        .btn.btn-blue {
            background: #013976 !important;
            color: #fff !important;
            transition: all 0.2s ease-in;
            -webkit-transition: all 0.2s ease-in;
            -moz-transition: all 0.2s ease-in;
        }

        .link-red {
            color: #ec1c24 !important;
        }

        .popUp {
            background: #f0fcf8;
            display: none;
            width: 100%;
            position: absolute;
            top: 0;
            left: 0;
            z-index: 99;
            padding-top: 40px;
            height: 100%;
        }

            .popUp .close-icon {
                background: url(Images/close-icon.png) no-repeat;
                width: 24px;
                height: 24px;
                position: absolute;
                right: 10px;
                top: 10px;
            }

        .container {
            width: 100%;
            max-width: 1350px;
            margin: 0 auto;
            padding: 0 15px;
            position: relative;
            height: 100%;
        }
            /* line 15, ../scss/imports/_main.scss */
            .container.container2 {
                padding: 0px;
            }

        .row-grid {
            float: left;
            width: 100%;
            padding: 25px 0px;
            text-align: left;
            position: relative;
        }

        /*.mob-text-center {
    left: 0!important;
    translate: transform(0, 0) !important;
    -moz-translate: transform(0, 0) !important;
    -webkit-translate: transform(0, 0) !important;
    -o-translate: transform(0, 0) !important;
    -ms-translate: transform(0, 0) !important; }*/

        .link-red {
            color: #ec1c24 !important;
        }

        .link.align-center.underline.callPopup {
            clear: both;
        }

        .banner-content {
            position: relative;
            width: 100%;
            float: left;
            color: #2d2d2d;
            transition: all 0.5s;
            -webkit-transition: all 0.5s;
            padding: 0;
            z-index: 0;
        }
            /* line 415, ../scss/imports/_main.scss */
            .banner-content h2 {
                text-align: center;
                margin-bottom: 10px;
                font-family: Lato-Regular;
                font-size: 2.5em;
                clear: both;
            }

        .txtclass {
            /*font-family: Tahoma,Arial, Helvetica, sans-serif;
            font-size: 16px;
            color: #000000;
            font-weight: normal;
            text-decoration: none;
            border: 1px solid #888888;
            border-radius: 4px;*/
            height: 25px;
            padding: 2px 7px;
            font-size: 13px;
            box-shadow: 0 0 0 #000 !important;
            line-height: 1.52857143;
            color: #3a3f51;
            background-color: #fff;
            background-image: none;
            border: 1px solid #dde6e9;
            border-radius: 4px;
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
        }

        .BRMinfoStyle {
            position: relative;
            top: 20px;
        }

        .InnerTextBox {
            position: relative;
            top: -4px;
        }

        .text-xs-left {
            text-align: left;
        }

        .text-xs-right {
            text-align: right;
        }

        .text-xs-center {
            text-align: center;
        }

        .text-xs-justify {
            text-align: justify;
        }

        @media (min-width: 768px) {
            .text-sm-left {
                text-align: left;
            }

            .text-sm-right {
                text-align: right;
            }

            .text-sm-center {
                text-align: center;
            }

            .text-sm-justify {
                text-align: justify;
            }
        }

        @media (min-width: 992px) {
            .text-md-left {
                text-align: left;
            }

            .text-md-right {
                text-align: right;
            }

            .text-md-center {
                text-align: center;
            }

            .text-md-justify {
                text-align: justify;
            }
        }

        @media (min-width: 1200px) {
            .text-lg-left {
                text-align: left;
            }

            .text-lg-right {
                text-align: right;
            }

            .text-lg-center {
                text-align: center;
            }

            .text-lg-justify {
                text-align: justify;
            }
        }

        @media (min-width: 768px) {
            .clsdownloadbrochure {
                margin-right: -248px;
            }
        }

        /* Media Query for Mobile Devices */
        @media (max-width: 480px) {
            .clsdownloadbrochure {
                margin-right: 13px !important;
            }
        }

        /* Media Query for low resolution  Tablets, Ipads */
        @media (min-width: 481px) and (max-width: 767px) {
            .clsdownloadbrochure {
                margin-right: -248px !important;
            }
        }

        /* Media Query for Tablets Ipads portrait mode */
        @media (min-width: 768px) {
            .clsdownloadbrochure {
                margin-right: -248px !important;
            }
        }

        /* Media Query for Laptops and Desktops */
        /*@media (min-width: 1025px) and (max-width: 1280px){
           .nav navbar-nav navbar-right clsdownloadbrochure {
               margin-right: -248px!important;
            }
        }*/

        /* Media Query for Large screens */
        @media (min-width: 1281px) {
            .clsdownloadbrochure {
                margin-right: -248px;
            }
        }
    </style>


    <form id="form1" runat="server">

        <asp:HiddenField ID="hdnAccountNumber" runat="server" Value="" />
        <asp:HiddenField ID="hdnIsKLTEmployee" runat="server" Value="" />
        <asp:HiddenField ID="hiddenMemberAge" runat="server" Value="" />
        <asp:HiddenField ID="hiddenMemberDOB" runat="server" Value="" />
        <asp:HiddenField ID="HiddenGender" runat="server" Value="" />
        <asp:HiddenField ID="hiddenMemberName" runat="server" Value="" />
        <asp:HiddenField ID="hiddenNomineeName" runat="server" Value="" />
        <asp:HiddenField ID="hiddenNomineeDob" runat="server" Value="" />
        <asp:HiddenField ID="hiddenNomineeRelationship" runat="server" Value="" />
        <asp:HiddenField ID="hiddenProposalNo" runat="server" Value="" />
        <asp:HiddenField ID="hiddenRelation" runat="server" Value="" />
        <asp:HiddenField ID="hiddenempcode" runat="server" Value="" />
        <asp:HiddenField ID="Hiddenoccupation" runat="server" Value="" />
        <asp:HiddenField ID="Hiddenmartialstatus" runat="server" Value="" />
        <asp:HiddenField ID="hidddenamount" runat="server" Value="" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnOTPSentCount" runat="server" ClientIDMode="Static" Value="0" />
        <asp:HiddenField ID="hdnOTPValue" runat="server" Value="" />
        <asp:HiddenField ID="hdnIsEmployeeRecordPresent" runat="server" Value="0" />

        <asp:UpdatePanel ID="UpdatePanel_Detail1" runat="server" UpdateMode="Conditional"
            ClientIDMode="Static">
            <ContentTemplate>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <script src="css/newcssjs/js/bootstrap.js"></script>
                <script>
                    // Rechecking
                    $(function () {
                        // callpopup();

                        $("#dialog1").dialog({
                            autoOpen: false
                        });

                        $("#opener").click(function () {
                            $("#dialog1").dialog('open');
                        });

                        debugger;
                        $("#inlineradioselfspousechildren").prop("checked", true);
                        $("#divPlanD").css("display", "block");

                        $("#<%=lblSelectedPremium.ClientID%>").text($("#<%=lblTotalAmountPayableplan4.ClientID%>").text());

                        $("#btnMakePayment").prop("disabled", true);
                        $('input[type=radio][name="radiofloaterPlan"]').on('change', function () {
                            var plan = this.value;
                            if (plan == "PlanA") {
                                $("#divPlanA").css("display", "block");
                                $("#divPlanB").css("display", "none");
                                $("#divPlanC").css("display", "none");
                                $("#divPlanD").css("display", "none");
                                var lblTotalAmountPayable1 = $("#<%=lblTotalAmountPayableplan1.ClientID%>").text();
                                $("#<%=lblSelectedPremium.ClientID%>").text(lblTotalAmountPayable1);

                            }
                            else if (plan == "PlanB") {
                                $("#divPlanA").css("display", "none");
                                $("#divPlanB").css("display", "block");
                                $("#divPlanC").css("display", "none");
                                $("#divPlanD").css("display", "none");
                                var lblTotalAmountPayable2 = $("#<%=lblTotalAmountPayableplan2.ClientID%>").text();
                                $("#<%=lblSelectedPremium.ClientID%>").text(lblTotalAmountPayable2);
                            }
                            else if (plan == "PlanC") {
                                $("#divPlanA").css("display", "none");
                                $("#divPlanB").css("display", "none");
                                $("#divPlanC").css("display", "block");
                                $("#divPlanD").css("display", "none");
                                var lblTotalAmountPayable3 = $("#<%=lblTotalAmountPayableplan3.ClientID%>").text();
                                $("#<%=lblSelectedPremium.ClientID%>").text(lblTotalAmountPayable3);
                            }
                            else if (plan == "PlanD") {
                                $("#divPlanA").css("display", "none");
                                $("#divPlanB").css("display", "none");
                                $("#divPlanC").css("display", "none");
                                $("#divPlanD").css("display", "block");
                                var lblTotalAmountPayable4 = $("#<%=lblTotalAmountPayableplan4.ClientID%>").text();
                                $("#<%=lblSelectedPremium.ClientID%>").text(lblTotalAmountPayable4);
                            }
                        });
                        $("#<%=hidddenamount.ClientID%>").val($("#<%=lblSelectedPremium.ClientID%>").text());
                    });

                    $("#dialog").dialog({ autoOpen: false, modal: true, buttons: { "Ok": function () { $(this).dialog("close"); } } });


                </script>
                <script type="text/javascript">
                    $(document).ready(function () {


                        $(function () {

                            var hdnIsEmployeeRecordPresent = $("#hdnIsEmployeeRecordPresent").val();
                            if (hdnIsEmployeeRecordPresent == "0") {
                                $("#sectionThankYou").hide();
                                $("#sectionError").hide();
                                $("#sectionFailure").hide();
                                $("#sectionRecordNotFound").show();
                                $("#sectionMain").hide();
                                $("#lblthnkyou").text('');
                            }
                            else if (hdnIsEmployeeRecordPresent == "2") {
                                $("#sectionThankYou").show();
                                $("#sectionError").hide();
                                $("#sectionFailure").hide();
                                $("#sectionRecordNotFound").hide();
                                $("#sectionMain").hide();

                                $("#lblthnkyou").text('Oops! Payment Failed');
                                $("#lblsuccessmessage").text('');
                                $("#lblsuccessmessage").append("<b> Unfortunately, an error has occurred and your payment cannot be processed. Please connect with your Key Accounts Manager to reactivate the payment link.</b>");
                            }
                            else if (hdnIsEmployeeRecordPresent == "3") {
                                $("#sectionThankYou").show();
                                $("#lblsuccessmessage").text('');
                                $("#lblthnkyou").text('Thank You');
                                $("#lblsuccessmessage").append("<b>Your payment is successful<br/>Congratulations on your <strong>Kotak Group Health Assure policy.</strong> Kindly note down your proposal number " + $("#hiddenProposalNo").val() + ". <br/> Your policy issuance is in progress and policy documents will be emailed to you shortly.</b>");
                                $("#sectionError").hide();
                                $("#sectionFailure").hide();
                                $("#sectionRecordNotFound").hide();
                                $("#sectionMain").hide();
                            }
                            else if (hdnIsEmployeeRecordPresent == "4") {
                                $("#sectionThankYou").hide();
                                $("#sectionError").hide();
                                $("#sectionFailure").show();
                                $("#lblfailmessage").text("");

                                $("#lblthnkyou").text('');
                                $("#lblfailmessage").append("<b> Oops! It looks like something went wrong and the payment wasn't processed.</br>Please note down following details for further communication</br></br>proposal number :<strong>" + $("#hiddenProposalNo").val() + "</strong></b>");
                                $("#sectionRecordNotFound").hide();
                                $("#sectionMain").hide();
                            }

                            var crndt = new Date();
                            var year = crndt.getFullYear();
                            crndt.setFullYear(year);

                            var crndt1 = new Date();
                            var year1 = crndt1.getFullYear();
                            crndt1.setFullYear(year1);
                            function calculateage(datebirth) {

                                debugger;
                                var from = datebirth.split("/");

                                var birthdate = from[2] + "-" + from[0] + "-" + from[1];


                                // This is the difference in milliseconds
                                var ageInMilliseconds = new Date() - new Date(birthdate);
                                return Math.floor(ageInMilliseconds / 31557600000);

                            }
                            $("input[type='text']").each(function () {
                                var element = $(this);
                                var id = $(this).attr('id');
                                var name = $(this).attr('name');

                                if (id.indexOf('NomineeDOB') >= 0) {

                                    $(document).on('focus', "#" + id, function () {
                                        $(this).datepicker({
                                            changeMonth: true,
                                            changeYear: true,
                                            showButtonPanel: true,
                                            dateFormat: 'mm/dd/yy',
                                            yearRange: '1920:' + year1 + '',
                                            defaultDate: crndt,
                                            maxDate: crndt

                                        });
                                    });

                                }
                                if (id.indexOf('dob') >= 0) {

                                    $(document).on('focus', "#" + id, function () {
                                        $(this).datepicker({
                                            changeMonth: true,
                                            changeYear: true,
                                            showButtonPanel: true,
                                            dateFormat: 'mm/dd/yy',
                                            yearRange: '1920:' + year1 + '',
                                            defaultDate: crndt,
                                            maxDate: crndt
                                        });
                                    });
                                }

                                //if (name.indexOf('memberdob') >= 0) {

                                //    $(document).on('change', "#" + id, function () {
                                //        var datebirth = $(this).val();
                                //        alert(calculateage(datebirth));
                                //        $("#txtspouseage2").val(calculateage(datebirth));
                                //    });
                                //}

                            });


                            //Data Bind

                            var occupation = $('#Hiddenoccupation').val();
                            var gender = $('#HiddenGender').val();
                            var martialstatus = $('#Hiddenmartialstatus').val();
                            var nomineerelation = $('#hiddenNomineeRelationship').val();
                            var memeberrelation = $('#hiddenRelation').val();

                            var Plan = ["A", "B", "C", "D"];

                            for (var i = 0; i < Plan.length; i++) {
                                var planid = Plan[i];
                                debugger;
                                $('#divTabContentplan' + planid).children('.tab-pane').each(function () {
                                    var innerDivId = $(this).attr('id');
                                    var innerDivName = $(this).attr('name');
                                    var innerDivMemberId = $(this).attr('customAttribut_MemberId');

                                    $("#" + innerDivId).find("input[type='text']").each(function () {
                                        var element = $(this);
                                        var id = $(this).attr('id');
                                        var name = $(this).attr('name');
                                        if (innerDivName == "Self") {


                                            if (name.indexOf('txtmembername') >= 0) {
                                                $("#" + id).val($('#hiddenMemberName').val());
                                            }
                                            else if (name.indexOf('txtmemberdob') >= 0) {
                                                $("#" + id).val($('#hiddenMemberDOB').val());
                                            }
                                            else if (name.indexOf('txtmemberage') >= 0) {
                                                $("#" + id).val($('#hiddenMemberAge').val());
                                            }



                                        }
                                        if (name.indexOf('NomineeName') >= 0) {
                                            $("#" + id).val($('#hiddenNomineeName').val());
                                        }
                                        else if (name.indexOf('NomineeDOB') >= 0) {
                                            $("#" + id).val($('#hiddenNomineeDob').val());
                                        }
                                    });

                                    $("#" + innerDivId).find("select.form-control").each(function () {
                                        if (innerDivName == "Self") {
                                            var element = $(this);
                                            var id = $(this).attr('id');
                                            var name = $(this).attr('name');
                                            if (name.indexOf('ddoccupation') >= 0) {
                                                $('select[name=' + name + ']').val(occupation);

                                            }
                                            else if (name.indexOf('ddmartialstatus') >= 0) {
                                                $('select[name=' + name + ']').val(martialstatus);

                                            }
                                            else if (name.indexOf('ddmembergender') >= 0) {
                                                $('select[name=' + name + ']').val(gender);

                                            }
                                            else if (name.indexOf('NomineeRelationship') >= 0 && nomineerelation != "") {
                                                $('select[name=' + name + ']').val(nomineerelation);

                                            }
                                            else if (name.indexOf('ddmemberrelation') >= 0) {
                                                $('select[name=' + name + ']').val(memeberrelation);

                                            }

                                        }
                                        else if (innerDivName == "Spouse") {
                                            var element = $(this);
                                            var id = $(this).attr('id');
                                            var name = $(this).attr('name');
                                            //if (id.indexOf('spouseNomineeRelationship') >= 0 ) {
                                            //    $('select[name=' + name + ']').val("Select");

                                            //}
                                            if (gender == "M") {
                                                if (name.indexOf('ddmembergender') >= 0) {

                                                    $('select[name=' + name + ']').val('F');

                                                }
                                                else if (name.indexOf('ddmemberrelation') >= 0) {

                                                    $('select[name=' + name + ']').val('Husband');

                                                }
                                            } else {
                                                if (name.indexOf('ddmembergender') >= 0) {

                                                    $('select[name=' + name + ']').val('M');

                                                }
                                                else if (name.indexOf('ddmemberrelation') >= 0) {

                                                    $('select[name=' + name + ']').val('Wife');

                                                }
                                            }

                                        }
                                        else if (innerDivName == "1st Child") {
                                            var element = $(this);
                                            var id = $(this).attr('id');
                                            var name = $(this).attr('name');
                                            // if (id.indexOf('childNomineeRelationship') >= 0 ) {
                                            //    $('select[name=' + name + ']').val("Select");

                                            //}
                                            if (gender == "M") {
                                                if (name.indexOf('ddmemberrelation') >= 0) {

                                                    $('select[name=' + name + ']').val('Father');

                                                }
                                            }
                                            else if (gender == "F") {
                                                if (name.indexOf('ddmemberrelation') >= 0) {

                                                    $('select[name=' + name + ']').val('Mother');

                                                }
                                            }
                                            // if (id.indexOf('childNomineeRelationship') >= 0 ) {
                                            //    $('select[name=' + name + ']').val("Select");

                                            //}
                                        }
                                        else if (innerDivName == "2nd Child") {
                                            var element = $(this);
                                            var id = $(this).attr('id');
                                            var name = $(this).attr('name');
                                            // if (id.indexOf('childNomineeRelationship') >= 0 ) {
                                            //    $('select[name=' + name + ']').val("Select");

                                            //}
                                            if (gender == "M") {

                                                if (name.indexOf('ddmemberrelation') >= 0) {

                                                    $('select[name=' + name + ']').val('Father');

                                                }
                                            }
                                            else if (gender == "F") {
                                                if (name.indexOf('ddmemberrelation') >= 0) {

                                                    $('select[name=' + name + ']').val('Mother');

                                                }
                                            }
                                            // if (id.indexOf('childNomineeRelationship') >= 0 ) {
                                            //    $('select[name=' + name + ']').val("Select");

                                            //}

                                        }
                                    });



                                });
                            }

                            var plan = $("input[name='radiofloaterPlan']:checked").val();


                            $("input[type='text']").each(function () {
                                var element = $(this);
                                var id = $(this).attr('id');
                                var name = $(this).attr('name');





                                if (id.indexOf('txtspousedob2') >= 0) {

                                    $(document).on('change', "#" + id, function () {


                                        DOBvalidation($(this).val(), name, id, "txtspouseage2", "Spouse");


                                    });
                                }
                                if (id.indexOf('txtspousedob3') >= 0) {

                                    $(document).on('change', "#" + id, function () {


                                        DOBvalidation($(this).val(), name, id, "txtspouseage3", "Spouse");


                                    });
                                }
                                if (id.indexOf('txtchilddob3') >= 0) {

                                    $(document).on('change', "#" + id, function () {


                                        DOBchildvalidation($(this).val(), name, id, "txtxhildage3", "Child");


                                    });
                                }
                                if (id.indexOf('txtspousedob4') >= 0) {

                                    $(document).on('change', "#" + id, function () {


                                        DOBvalidation($(this).val(), name, id, "txtspouseage4", "Spouse");


                                    });
                                }
                                if (id.indexOf('txtchilddob4') >= 0) {

                                    $(document).on('change', "#" + id, function () {


                                        DOBchildvalidation($(this).val(), name, id, "txtchildage4", "Child");


                                    });
                                }
                                if (id.indexOf('txtchild2dob4') >= 0) {

                                    $(document).on('change', "#" + id, function () {


                                        DOBchildvalidation($(this).val(), name, id, "txtchild2age4", "2nd Child");


                                    });
                                }



                            });

                            function DOBchildvalidation(memberdob, name, id, ageid, Member) {
                                var datebirth = memberdob;
                                var age = calculateage(datebirth);
                                if (age > 25) {
                                    alert(Member + ' Age is should be less than 25 yrs');
                                    $("#" + id).val("");
                                    if (name.indexOf('memberage') >= 0) {
                                        $("#" + ageid).val("");
                                    }
                                    return false;
                                }
                                else {


                                    $("#" + ageid).val(age);

                                }
                            }
                            function DOBvalidation(memberdob, name, id, ageid, Member) {
                                var datebirth = memberdob;
                                var age = calculateage(datebirth);
                                if (age < 18) {

                                    alert(Member + ' Age is should be greater than 18 yrs');
                                    $("#" + id).val("");
                                    if (name.indexOf('memberage') >= 0) {
                                        $("#" + ageid).val("");
                                    }
                                    return false;
                                }
                                else if (age > 55) {
                                    alert(Member + ' Age is should be less than 55 yrs');
                                    $("#" + id).val("");
                                    if (name.indexOf('memberage') >= 0) {
                                        $("#" + ageid).val("");
                                    }
                                    return false;
                                }
                                else {


                                    $("#" + ageid).val(age);

                                }
                            }
                            function declarebtn() {
                                var IsAgree = $('#termchkbox').is(':checked');

                                if (IsAgree) {
                                    $("#btnMakePayment").prop("disabled", false);
                                }
                                else {
                                    $("#btnMakePayment").prop("disabled", true);
                                }
                            }

                            $('#termchkbox').change(function () {
                                declarebtn();

                            });

                            $('#btnMakePayment').click(function () {
                                return validation();
                            });

                            $('#btnMobileReSend').click(function () {

                                return validation();
                            });

                            $('#btnMobileVerify').click(function () {
                                debugger;
                                var txtOtpNumber = $("#txtOtpNumber").val();

                                var ProposalNo = $("#hiddenProposalNo").val();
                                var EmployeeCode = $("#hiddenempcode").val();
                                var MembersDetails = {};
                                var objProposerPrimaryDetails = {};

                                var dataMembersDetails = [];
                                var MemberName = '';
                                var MemberDOB = '';
                                var MemberAge = '';
                                var MemberGender = '';
                                var MemberOccupation = '';
                                var MartialStatus = '';
                                var MemberRelationship = '';
                                var NomineeName = '';
                                var NomineeDOB = '';
                                var NomineeReslationShip = '';
                                var Relationship = '';

                                var selftitle = $('select[id="cboTitle"]').val();
                                var selfName = $("#txtmembername").val();
                                var Mobileno = $("#txtmembermobileno").val();
                                var selfAddressLine1 = $("#txtmemberaddress1").val();
                                var selfAddressLine2 = $("#txtmemberaddress2").val();
                                var selfCity = $("#txtmembercity").val();
                                var selfState = $("#txtmemberstate").val();
                                var selfPincode = $("#txtmemberpincode").val();

                                var selfNomineeName = $("#txtmemberNomineeName").val() != ('' || 'undefined') ? '' : $("#txtmemberNomineeName").val();
                                var selfNomineeRelationship = $('select[id="ddnomineerelationship"]').val() != ('' || 'undefined') ? '' : $('select[id="ddnomineerelationship"]').val();
                                var selfNomineeDOB = $("#txtmemberNomineeDOB").val() != ('' || 'undefined') ? '' : $("#txtmemberNomineeDOB").val();
                                var selfProposalNo = $("#hiddenProposalNo").val();
                                var selfMemberEmail = $("#txtmemberemailid").val();

                                $('#divTabContent1').children('.tab-pane').each(function () {
                                    debugger;
                                    var innerDivId = $(this).attr('id');
                                    var innerDivName = $(this).attr('name');
                                    var innerDivMemberId = $(this).attr('customAttribut_MemberId');

                                    $("#" + innerDivId).find("input[type='text']").each(function () {
                                        var element = $(this);
                                        var id = $(this).attr('id');
                                        var name = $(this).attr('name');


                                        if (id.indexOf('txtmembermobileno') >= 0) {
                                            Mobileno = element.val();
                                        }
                                        else if (id.indexOf('txtmemberaddress1') >= 0) {
                                            selfAddressLine1 = element.val();
                                        }
                                        else if (id.indexOf('txtmemberaddress2') >= 0) {
                                            selfAddressLine2 = element.val();
                                        }
                                        else if (id.indexOf('txtmembercity') >= 0) {
                                            selfCity = element.val();

                                        }
                                        else if (id.indexOf('txtmemberstate') >= 0) {
                                            selfState = element.val();
                                        }
                                        else if (id.indexOf('txtmemberpincode') >= 0) {

                                            selfPincode = element.val();
                                        }
                                        else if (id.indexOf('txtmemberNomineeName') >= 0) {

                                            selfNomineeName = element.val();


                                        }
                                        else if (id.indexOf('txtmemberNomineeDOB') >= 0) {

                                            selfNomineeDOB = element.val();


                                        }

                                        else if (id.indexOf('txtmemberemailid') >= 0) {

                                            selfMemberEmail = element.val();


                                        }



                                    });

                                    $("#" + innerDivId).find("select.form-control").each(function () {
                                        var element = $(this);
                                        var id = $(this).attr('id');
                                        var name = $(this).attr('name');
                                        if (id.indexOf('ddnomineerelationship') >= 0) {

                                            selfNomineeRelationship = element.val();

                                        }
                                    });
                                });
                                var plan = $("input[name='radiofloaterPlan']:checked").val();

                                if (plan == "PlanA") {

                                    $('#divTabContentplanA').children('.tab-pane').each(function () {

                                        var innerDivId = $(this).attr('id');
                                        var innerDivName = $(this).attr('name');
                                        var innerDivMemberId = $(this).attr('customAttribut_MemberId');

                                        $("#" + innerDivId).find("input[type='text']").each(function () {
                                            var element = $(this);
                                            var id = $(this).attr('id');
                                            var name = $(this).attr('name');
                                            if (name.indexOf('txtmembername') >= 0) {
                                                MemberName = element.val();
                                            }

                                            else if (name.indexOf('txtmemberdob') >= 0) {
                                                MemberDOB = element.val();
                                            }


                                            else if (name.indexOf('txtmemberage') >= 0) {

                                                MemberAge = element.val();

                                            }


                                            else if (name.indexOf('NomineeName') >= 0) {

                                                NomineeName = element.val();

                                            }



                                            else if (id.indexOf('NomineeDOB') >= 0) {

                                                NomineeDOB = element.val();

                                            }



                                        });

                                        $("#" + innerDivId).find("select.form-control").each(function () {
                                            var element = $(this);
                                            var id = $(this).attr('id');
                                            var name = $(this).attr('name');
                                            if (name.indexOf('doccupation') >= 0) {

                                                MemberOccupation = element.val();

                                            }
                                            else if (name.indexOf('ddmembergender') >= 0) {

                                                MemberGender = element.val();

                                            }
                                            else if (name.indexOf('ddmartialstatus') >= 0) {

                                                MartialStatus = element.val();

                                            }
                                            else if (name.indexOf('ddmembergender') >= 0) {

                                                MemberGender = element.val();

                                            }
                                            else if (name.indexOf('NomineeRelationship') >= 0) {

                                                NomineeReslationShip = element.val();

                                            }
                                            else if (name.indexOf('ddmemberrelation') >= 0) {

                                                MemberRelationship = element.val();
                                            }
                                        });

                                        MembersDetails = { ProposalNo: ProposalNo, MemberName: MemberName, MemberDOB: MemberDOB, MemberAge: MemberAge, MemberGender: MemberGender, MemberOccupation: MemberOccupation, MartialStatus: MartialStatus, MemberRelationship: MemberRelationship, NomineeName: NomineeName, NomineeDOB: NomineeDOB, NomineeReslationShip: NomineeReslationShip ,Relationship:innerDivName};

                                        dataMembersDetails.push(MembersDetails);
                                    });

                                }

                                else if (plan == "PlanB") {

                                    $('#divTabContentplanB').children('.tab-pane').each(function () {

                                        var innerDivId = $(this).attr('id');
                                        var innerDivName = $(this).attr('name');
                                        var innerDivMemberId = $(this).attr('customAttribut_MemberId');

                                        $("#" + innerDivId).find("input[type='text']").each(function () {
                                            var element = $(this);
                                            var id = $(this).attr('id');
                                            var name = $(this).attr('name');
                                            if (name.indexOf('txtmembername') >= 0) {
                                                MemberName = element.val();
                                            }

                                            else if (name.indexOf('txtmemberdob') >= 0) {
                                                MemberDOB = element.val();
                                            }


                                            else if (name.indexOf('txtmemberage') >= 0) {

                                                MemberAge = element.val();

                                            }


                                            else if (name.indexOf('NomineeName') >= 0) {

                                                NomineeName = element.val();

                                            }



                                            else if (id.indexOf('NomineeDOB') >= 0) {

                                                NomineeDOB = element.val();

                                            }



                                        });
                                        $("#" + innerDivId).find("select.form-control").each(function () {
                                            var element = $(this);
                                            var id = $(this).attr('id');
                                            var name = $(this).attr('name');
                                            if (name.indexOf('doccupation') >= 0) {

                                                MemberOccupation = element.val();

                                            }
                                            else if (name.indexOf('ddmembergender') >= 0) {

                                                MemberGender = element.val();

                                            }
                                            else if (name.indexOf('ddmartialstatus') >= 0) {

                                                MartialStatus = element.val();

                                            }
                                            else if (name.indexOf('ddmembergender') >= 0) {

                                                MemberGender = element.val();

                                            }
                                            else if (name.indexOf('NomineeRelationship') >= 0) {

                                                NomineeReslationShip = element.val();

                                            }
                                            else if (name.indexOf('ddmemberrelation') >= 0) {

                                                MemberRelationship = element.val();
                                            }
                                        });

                                        MembersDetails = { ProposalNo: ProposalNo, MemberName: MemberName, MemberDOB: MemberDOB, MemberAge: MemberAge, MemberGender: MemberGender, MemberOccupation: MemberOccupation, MartialStatus: MartialStatus, MemberRelationship: MemberRelationship, NomineeName: NomineeName, NomineeDOB: NomineeDOB, NomineeReslationShip: NomineeReslationShip,Relationship:innerDivName };

                                        dataMembersDetails.push(MembersDetails);
                                    });

                                }
                                else if (plan == "PlanC") {

                                    $('#divTabContentplanC').children('.tab-pane').each(function () {

                                        var innerDivId = $(this).attr('id');
                                        var innerDivName = $(this).attr('name');
                                        var innerDivMemberId = $(this).attr('customAttribut_MemberId');

                                        $("#" + innerDivId).find("input[type='text']").each(function () {
                                            var element = $(this);
                                            var id = $(this).attr('id');
                                            var name = $(this).attr('name');
                                            if (name.indexOf('txtmembername') >= 0) {
                                                MemberName = element.val();
                                            }

                                            else if (name.indexOf('txtmemberdob') >= 0) {
                                                MemberDOB = element.val();
                                            }


                                            else if (name.indexOf('txtmemberage') >= 0) {

                                                MemberAge = element.val();

                                            }


                                            else if (name.indexOf('NomineeName') >= 0) {

                                                NomineeName = element.val();

                                            }



                                            else if (id.indexOf('NomineeDOB') >= 0) {

                                                NomineeDOB = element.val();

                                            }



                                        });
                                        $("#" + innerDivId).find("select.form-control").each(function () {
                                            var element = $(this);
                                            var id = $(this).attr('id');
                                            var name = $(this).attr('name');
                                            if (name.indexOf('doccupation') >= 0) {

                                                MemberOccupation = element.val();

                                            }
                                            else if (name.indexOf('ddmembergender') >= 0) {

                                                MemberGender = element.val();

                                            }
                                            else if (name.indexOf('ddmartialstatus') >= 0) {

                                                MartialStatus = element.val();

                                            }
                                            else if (name.indexOf('ddmembergender') >= 0) {

                                                MemberGender = element.val();

                                            }
                                            else if (name.indexOf('NomineeRelationship') >= 0) {

                                                NomineeReslationShip = element.val();

                                            }
                                            else if (name.indexOf('ddmemberrelation') >= 0) {

                                                MemberRelationship = element.val();
                                            }
                                        });
                                        MembersDetails = { ProposalNo: ProposalNo, MemberName: MemberName, MemberDOB: MemberDOB, MemberAge: MemberAge, MemberGender: MemberGender, MemberOccupation: MemberOccupation, MartialStatus: MartialStatus, MemberRelationship: MemberRelationship, NomineeName: NomineeName, NomineeDOB: NomineeDOB, NomineeReslationShip: NomineeReslationShip,Relationship:innerDivName };

                                        dataMembersDetails.push(MembersDetails);

                                    });

                                }
                                else if (plan == "PlanD") {

                                    $('#divTabContentplanD').children('.tab-pane').each(function () {

                                        var innerDivId = $(this).attr('id');
                                        var innerDivName = $(this).attr('name');
                                        var innerDivMemberId = $(this).attr('customAttribut_MemberId');

                                        $("#" + innerDivId).find("input[type='text']").each(function () {
                                            var element = $(this);
                                            var id = $(this).attr('id');
                                            var name = $(this).attr('name');
                                            if (name.indexOf('txtmembername') >= 0) {
                                                MemberName = element.val();
                                            }

                                            else if (name.indexOf('txtmemberdob') >= 0) {
                                                MemberDOB = element.val();
                                            }


                                            else if (name.indexOf('txtmemberage') >= 0) {

                                                MemberAge = element.val();

                                            }


                                            else if (name.indexOf('NomineeName') >= 0) {

                                                NomineeName = element.val();

                                            }



                                            else if (id.indexOf('NomineeDOB') >= 0) {

                                                NomineeDOB = element.val();

                                            }



                                        });

                                        $("#" + innerDivId).find("select.form-control").each(function () {
                                            var element = $(this);
                                            var id = $(this).attr('id');
                                            var name = $(this).attr('name');
                                            if (name.indexOf('doccupation') >= 0) {

                                                MemberOccupation = element.val();

                                            }
                                            else if (name.indexOf('ddmembergender') >= 0) {

                                                MemberGender = element.val();

                                            }
                                            else if (name.indexOf('ddmartialstatus') >= 0) {

                                                MartialStatus = element.val();

                                            }
                                            else if (name.indexOf('ddmembergender') >= 0) {

                                                MemberGender = element.val();

                                            }
                                            else if (name.indexOf('NomineeRelationship') >= 0) {

                                                NomineeReslationShip = element.val();

                                            }
                                            else if (name.indexOf('ddmemberrelation') >= 0) {

                                                MemberRelationship = element.val();
                                            }
                                        });

                                        MembersDetails = { ProposalNo: ProposalNo, MemberName: MemberName, MemberDOB: MemberDOB, MemberAge: MemberAge, MemberGender: MemberGender, MemberOccupation: MemberOccupation, MartialStatus: MartialStatus, MemberRelationship: MemberRelationship, NomineeName: NomineeName, NomineeDOB: NomineeDOB, NomineeReslationShip: NomineeReslationShip ,Relationship:innerDivName};

                                        dataMembersDetails.push(MembersDetails);
                                    });

                                }





                                objProposerPrimaryDetails = {
                                    Title: selftitle,
                                    MemberName: selfName,
                                    SelectedPlan: plan,
                                    FinalOneTimePasswordEnteredByEmployee: txtOtpNumber,
                                    Mobileno: Mobileno,
                                    AddressLine1: selfAddressLine1,
                                    AddressLine2: selfAddressLine2,
                                    City: selfCity,
                                    State: selfState,
                                    Pincode: selfPincode,
                                    SelectedPremium: $("#lblSelectedPremium").text(),
                                    NomineeName: selfNomineeName,
                                    NomineeRelationship: selfNomineeRelationship,
                                    NomineeDOB: selfNomineeDOB,
                                    ProposalNo: selfProposalNo,
                                    MemberEmail: selfMemberEmail


                                };
                                var jsonDataPRIMARYOBJECT = JSON.stringify(objProposerPrimaryDetails);
                                //console.log("Primary Object " + jsonDataPRIMARYOBJECT);

                                var dataOTP = { OTPNumber: txtOtpNumber, EmployeeCode: EmployeeCode, ProposalNo: ProposalNo };

                                $.ajax({
                                    type: "POST",
                                    url: "frmKGHARegistrationdetails.aspx/ValidateOTP",
                                    data: JSON.stringify(dataOTP),
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (response) {

                                        if (response.d == "success") {
                                            SubmitEmployeePrimaryData(objProposerPrimaryDetails, dataMembersDetails);
                                        }
                                        else {
                                            alert('OTP Does Not Match, Please Enter OTP Sent On Your Email And Mobile Number');
                                            return false;
                                        }
                                    },
                                    error: function (requestObject, error, errorThrown) {
                                        alert(error);
                                    }
                                });


                            });

                            function validation() {

                                debugger;

                                var Istermcheck = $('#termchkbox').is(':checked');



                                var isValid = true;




                                var membermobileno = $("#txtmembermobileno").val();
                                if (membermobileno.length < 10) {
                                    isValid = false;
                                    alert('Please Enter Valid 10 Digit Mobile Number');
                                    return false;
                                }
                                var memberaddress1 = $("#txtmemberaddress1").val().trim();
                                if (memberaddress1 == "") {
                                    isValid = false;
                                    alert('Please Enter Address 1 Field');
                                    return false;
                                }
                                var memberaddress2 = $("#txtmemberaddress2").val().trim();
                                if (memberaddress2 == "") {
                                    isValid = false;
                                    alert('Please Enter Address 2 Field');
                                    return false;
                                }
                                var membercity = $("#txtmembercity").val().trim();
                                if (membercity == "") {
                                    isValid = false;
                                    alert('Please Enter City Field');
                                    return false;
                                }
                                var memberstate = $("#txtmemberstate").val().trim();
                                if (memberstate == "") {
                                    isValid = false;
                                    alert('Please Enter State Field');
                                    return false;
                                }
                                var txtmemberpincode = $("#txtmemberpincode").val().trim();
                                if (txtmemberpincode.length < 6) {
                                    isValid = false;
                                    alert('Please Enter Valid Pincode Field');
                                    return false;
                                }
                                $('#divTabContent1').children('.tab-pane').each(function () {

                                    var innerDivId = $(this).attr('id');
                                    var innerDivName = $(this).attr('name');
                                    var innerDivMemberId = $(this).attr('customAttribut_MemberId');

                                    $("#" + innerDivId).find("input[type='text']").each(function () {
                                        var element = $(this);
                                        var id = $(this).attr('id');
                                        var name = $(this).attr('name');


                                        if (id.indexOf('txtmembermobileno') == "") {
                                            if (element.val() === "") {
                                                isValid = false;
                                                alert("Please Enter Valid Mobile No. for " + innerDivName);
                                                return false;
                                            }
                                        }
                                        else if (id.indexOf('txtmemberaddress1') == "") {
                                            if (element.val() === "") {
                                                isValid = false;
                                                alert("Please Enter Address Line 1 for " + innerDivName);
                                                return false;
                                            }
                                        }
                                        else if (id.indexOf('txtmemberaddress2') == "") {
                                            if (element.val() === "") {
                                                isValid = false;
                                                alert("Please Enter Address Line 2 for " + innerDivName);
                                                return false;
                                            }
                                        }
                                        else if (id.indexOf('txtmembercity') == "") {
                                            if (element.val() === "") {
                                                isValid = false;
                                                alert("Please Enter City for " + innerDivName);
                                                return false;
                                            }
                                        }
                                        else if (id.indexOf('txtmemberstate') == "") {
                                            if (element.val() === "") {
                                                isValid = false;
                                                alert("Please Enter State for " + innerDivName);
                                                return false;
                                            }
                                        }
                                        else if (id.indexOf('txtmemberpincode') == "") {
                                            if (element.val() === "") {
                                                isValid = false;
                                                alert("Please Enter Pincode for " + innerDivName);
                                                return false;
                                            }
                                        }
                                    });

                                    if (isValid == false) {
                                        return false;
                                    }
                                });

                                var plan = $("input[name='radiofloaterPlan']:checked").val();
                                if (plan == "PlanA") {

                                    $('#divTabContentplanA').children('.tab-pane').each(function () {

                                        var innerDivId = $(this).attr('id');
                                        var innerDivName = $(this).attr('name');
                                        var innerDivMemberId = $(this).attr('customAttribut_MemberId');

                                        $("#" + innerDivId).find("input[type='text']").each(function () {
                                            var element = $(this);
                                            var id = $(this).attr('id');
                                            var name = $(this).attr('name');


                                            if (id.indexOf('txtmemberNomineeName') == "") {
                                                if (element.val() === "") {
                                                    isValid = false;
                                                    alert("Please Enter Nominee Name for " + innerDivName);
                                                    return false;
                                                }
                                            }
                                            else if (id.indexOf('txtmemberNomineeDOB') == "") {
                                                if (element.val() === "") {
                                                    isValid = false;
                                                    alert("Please Enter Nominee DOB for " + innerDivName);
                                                    return false;
                                                }
                                            }
                                            else if (id.indexOf('ddemberNomineeRelationship') == "") {
                                                if (element.val() === "") {
                                                    isValid = false;
                                                    alert("Please Select Nominee Relationship for " + innerDivName);
                                                    return false;
                                                }
                                            }

                                        });

                                        $("#" + innerDivId).find("select.form-control").each(function () {
                                            var element = $(this);
                                            var id = $(this).attr('id');
                                            var name = $(this).attr('name');
                                            if (name.indexOf('ddselfoccupation') >= 0) {

                                                if (element.val() === "Select") {
                                                    isValid = false;
                                                    alert("Please Select Self occupation for " + innerDivName);
                                                    return false;
                                                }

                                            }
                                            else if (name.indexOf('ddmartialstatus') >= 0) {

                                                if (element.val() === "Select") {
                                                    isValid = false;
                                                    alert("Please Select Self Martial Status for " + innerDivName);
                                                    return false;
                                                }

                                            }
                                            else if (name.indexOf('NomineeRelationship') >= 0) {

                                                if (element.val() === "Select") {
                                                    isValid = false;
                                                    alert("Please Select Nominee Relationship " + innerDivName);
                                                    return false;
                                                }

                                            }

                                        });

                                        if (isValid == false) {
                                            return false;
                                        }
                                    });
                                }
                                else if (plan == "PlanB") {

                                    $('#divTabContentplanB').children('.tab-pane').each(function () {
                                        debugger;
                                        var innerDivId = $(this).attr('id');
                                        var innerDivName = $(this).attr('name');
                                        var innerDivMemberId = $(this).attr('customAttribut_MemberId');
                                        $("#" + innerDivId).find("input[type='text'],select.form-control").each(function () {
                                            debugger;
                                            var element = $(this);
                                            var id = $(this).attr('id');
                                            var name = $(this).attr('name');



                                            if (name.indexOf('txtmembername') >= 0) {
                                                if (element.val() === "") {
                                                    isValid = false;
                                                    alert("Please Enter Member Name for " + innerDivName);
                                                    return false;
                                                }
                                            }
                                            else if (name.indexOf('txtmemberdob') >= 0) {
                                                if (element.val() === "") {
                                                    isValid = false;
                                                    alert("Please Enter Member DOB for " + innerDivName);
                                                    return false;
                                                }
                                            }
                                            else if (name.indexOf('txtmemberage') >= 0) {
                                                if (element.val() === "") {
                                                    isValid = false;
                                                    alert("Please Enter Age for " + innerDivName);
                                                    return false;
                                                }
                                            }

                                            else if (name.indexOf('NomineeName') >= 0) {
                                                if (element.val() === "") {
                                                    isValid = false;
                                                    alert("Please Enter Nominee Name for " + innerDivName);
                                                    return false;
                                                }
                                            }
                                            else if (name.indexOf('NomineeDOB') >= 0) {
                                                if (element.val() === "") {
                                                    isValid = false;
                                                    alert("Please Enter Nominee DOB " + innerDivName);
                                                    return false;
                                                }
                                            }

                                            else if (name.indexOf('ddoccupation') >= 0) {

                                                if (element.val() === "Select") {
                                                    isValid = false;
                                                    alert("Please Select occupation for " + innerDivName);
                                                    return false;
                                                }
                                            }
                                            else if (name.indexOf('ddmartialstatus') >= 0) {

                                                if (element.val() === "Select") {
                                                    isValid = false;
                                                    alert("Please Select Martial Status " + innerDivName);
                                                    return false;
                                                }
                                            }
                                            else if (name.indexOf('NomineeRelationship') >= 0) {

                                                if (element.val() === "Select") {
                                                    isValid = false;
                                                    alert("Please Select Nominee Relationship " + innerDivName);
                                                    return false;
                                                }
                                            }


                                        });





                                        if (isValid == false) {
                                            return false;
                                        }
                                    });


                                }
                                else if (plan == "PlanC") {

                                    $('#divTabContentplanC').children('.tab-pane').each(function () {
                                        var innerDivId = $(this).attr('id');
                                        var innerDivName = $(this).attr('name');
                                        var innerDivMemberId = $(this).attr('customAttribut_MemberId');



                                        $("#" + innerDivId).find("input[type='text'],select.form-control").each(function () {
                                            debugger;
                                            var element = $(this);
                                            var id = $(this).attr('id');
                                            var name = $(this).attr('name');



                                            if (name.indexOf('txtmembername') >= 0) {
                                                if (element.val() === "") {
                                                    isValid = false;
                                                    alert("Please Enter Member Name for " + innerDivName);
                                                    return false;
                                                }
                                            }
                                            else if (name.indexOf('txtmemberdob') >= 0) {
                                                if (element.val() === "") {
                                                    isValid = false;
                                                    alert("Please Enter Member DOB for " + innerDivName);
                                                    return false;
                                                }
                                            }
                                            else if (name.indexOf('txtmemberage') >= 0) {
                                                if (element.val() === "") {
                                                    isValid = false;
                                                    alert("Please Enter Age for " + innerDivName);
                                                    return false;
                                                }
                                            }

                                            else if (name.indexOf('NomineeName') >= 0) {
                                                if (element.val() === "") {
                                                    isValid = false;
                                                    alert("Please Enter Nominee Name for " + innerDivName);
                                                    return false;
                                                }
                                            }
                                            else if (name.indexOf('NomineeDOB') >= 0) {
                                                if (element.val() === "") {
                                                    isValid = false;
                                                    alert("Please Enter Nominee DOB " + innerDivName);
                                                    return false;
                                                }
                                            }

                                            else if (name.indexOf('ddoccupation') >= 0) {

                                                if (element.val() === "Select") {
                                                    isValid = false;
                                                    alert("Please Select occupation for " + innerDivName);
                                                    return false;
                                                }
                                            }
                                            else if (name.indexOf('ddmartialstatus') >= 0) {

                                                if (element.val() === "Select") {
                                                    isValid = false;
                                                    alert("Please Select Martial Status " + innerDivName);
                                                    return false;
                                                }
                                            }
                                            else if (name.indexOf('NomineeRelationship') >= 0) {

                                                if (element.val() === "Select") {
                                                    isValid = false;
                                                    alert("Please Select Nominee Relationship " + innerDivName);
                                                    return false;
                                                }
                                            }

                                        });


                                        if (isValid == false) {
                                            return false;
                                        }
                                    });

                                }
                                else if (plan == "PlanD") {

                                    $('#divTabContentplanD').children('.tab-pane').each(function () {

                                        var innerDivId = $(this).attr('id');
                                        var innerDivName = $(this).attr('name');
                                        var innerDivMemberId = $(this).attr('customAttribut_MemberId');

                                        $("#" + innerDivId).find("input[type='text'],select.form-control").each(function () {
                                            debugger;
                                            var element = $(this);
                                            var id = $(this).attr('id');
                                            var name = $(this).attr('name');




                                            if (name.indexOf('txtmembername') >= 0) {
                                                if (element.val() === "") {
                                                    isValid = false;
                                                    alert("Please Enter Member Name for " + innerDivName);
                                                    return false;
                                                }
                                            }
                                            else if (name.indexOf('txtmemberdob') >= 0) {
                                                if (element.val() === "") {
                                                    isValid = false;
                                                    alert("Please Enter Member DOB for " + innerDivName);
                                                    return false;
                                                }
                                            }
                                            else if (name.indexOf('txtmemberage') >= 0) {
                                                if (element.val() === "") {
                                                    isValid = false;
                                                    alert("Please Enter Age for " + innerDivName);
                                                    return false;
                                                }
                                            }

                                            else if (name.indexOf('NomineeName') >= 0) {
                                                if (element.val() === "") {
                                                    isValid = false;
                                                    alert("Please Enter Nominee Name for " + innerDivName);
                                                    return false;
                                                }
                                            }
                                            else if (name.indexOf('NomineeDOB') >= 0) {
                                                if (element.val() === "") {
                                                    isValid = false;
                                                    alert("Please Enter Nominee DOB " + innerDivName);
                                                    return false;
                                                }
                                            }

                                            else if (name.indexOf('ddoccupation') >= 0) {

                                                if (element.val() === "Select") {
                                                    isValid = false;
                                                    alert("Please Select occupation for " + innerDivName);
                                                    return false;
                                                }
                                            }
                                            else if (name.indexOf('ddmartialstatus') >= 0) {

                                                if (element.val() === "Select") {
                                                    isValid = false;
                                                    alert("Please Select Martial Status " + innerDivName);
                                                    return false;
                                                }
                                            }
                                            else if (name.indexOf('NomineeRelationship') >= 0) {

                                                if (element.val() === "Select") {
                                                    isValid = false;
                                                    alert("Please Select Nominee Relationship " + innerDivName);
                                                    return false;
                                                }
                                            }


                                        });


                                        if (isValid == false) {
                                            return false;
                                        }
                                    });
                                }

                                if (Istermcheck == false && isValid == true) {

                                    isValid = false;
                                    alert('Please check I agree to the terms and conditions');
                                    return false;
                                }

                                if (isValid == true) {
                                    ShowOTP();
                                }

                                //return isValid;
                                return false;
                            }

                            function SubmitEmployeePrimaryData(objProposerPrimaryDetails, dataMembersDetails) {

                                //alert(JSON.stringify(objEmployeePrimaryDetails));

                                $.ajax({
                                    type: "POST",
                                    url: "frmKGHARegistrationdetails.aspx/SaveEmployeePrimaryDetails",
                                    data: '{objProposerPrimaryDetails: ' + JSON.stringify(objProposerPrimaryDetails) + '}',
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (response) {
                                        if (response.d == "success") {
                                            SubmitMemberData(dataMembersDetails);
                                        }
                                        else {
                                            $("#sectionThankYou").hide();
                                            $("#sectionError").show();
                                            $("#sectionMain").hide();
                                            $("#sectionRecordNotFound").hide();
                                            $("#sectionFailure").hide();


                                        }
                                    },
                                    error: function (response) {
                                        alert(JSON.stringify(response));
                                    }
                                });
                            }

                            function SubmitMemberData(dataMembersDetails) {

                                $.ajax({
                                    type: "POST",
                                    url: "frmKGHARegistrationdetails.aspx/SaveMemberDetails",
                                    data: '{listMemberDetails: ' + JSON.stringify(dataMembersDetails) + '}',
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (response) {
                                        if (response.d == "success") {
                                            $("#btnConfirmPayment").click();
                                            //$("#sectionThankYou").show();
                                            //$("#sectionError").hide();
                                            //$("#sectionMain").hide();

                                        }
                                        else {
                                            $("#sectionThankYou").hide();
                                            $("#sectionError").show();
                                            $("#sectionMain").hide();
                                            $("#sectionRecordNotFound").hide();
                                            $("#sectionFailure").hide();
                                        }
                                    },
                                    error: function (response) {
                                        alert(JSON.stringify(response));
                                    }
                                });
                            }

                            function ShowOTP() {
                                debugger;
                                var EmpEmailId = $("#lblEmpEmailId").text();
                                var EmpName = $("#lblEmployeeName").text();
                                var MobNumber = $("#txtmembermobileno").val();
                                var EmpCode = $("#hiddenempcode").val();
                                var ProposalNo = $("#hiddenProposalNo").val();

                                var phoneno = /^\d{10}$/;
                                if (MobNumber.match(phoneno)) {
                                    ShopOTPTimer();
                                    var OTPData = { MobileNumber: MobNumber, EmailId: EmpEmailId, EmployeeCode: EmpCode, EmployeeName: EmpName, ProposalNo: ProposalNo };

                                    $.ajax({
                                        type: "POST",
                                        url: "frmKGHARegistrationdetails.aspx/GenerateOTPNew",
                                        data: JSON.stringify(OTPData),
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        success: function (response) {
                                            if (response.d == "success") {
                                                // ShopOTPTimer();
                                            }
                                            else {
                                                $("#sectionThankYou").hide();
                                                $("#sectionError").show();
                                                $("#sectionMain").hide();
                                                $("#sectionRecordNotFound").hide();
                                                $("#sectionFailure").hide();
                                            }
                                        },
                                        error: function (response) {
                                            alert(JSON.stringify(response));
                                        }
                                    });
                                }

                                else {
                                    alert("Please enter 10 digit mobile number.");
                                }
                            }


                            function ShopOTPTimer() {

                                $("#otpPanel").show();
                                $("#divDeclarofGoodHealth").hide();
                                $("#txtOtpNumber").val('');

                                $('.timer').circularCountDown({
                                    delayToFadeIn: 50,
                                    size: 70,
                                    fontColor: '#696969',
                                    colorCircle: 'transparent',
                                    background: '#ffa58c',
                                    reverseLoading: true,
                                    duration: {
                                        seconds: parseInt(180)
                                    },
                                    beforeStart: function () {
                                        $("#txtTimer").hide();
                                        $('#btnMakePayment').hide();
                                        $('#btnMobileReSend').attr('disabled', true);
                                        $('#btnMobileVerify').attr('disabled', false);
                                        $('#btnMobileVerify').removeAttr('disabled');
                                        $('#btnMobileVerify').removeClass('disabled');
                                    },
                                    end: function (countdown) {
                                        $("#txtTimer").show();
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




                            $('input[type=radio][name=radioDclrofgdhlth]').change(function () {
                                var IsYesChecked = $('#inlineradioYes').is(':checked');
                                if (IsYesChecked) {
                                    $("#btnMakePayment").show();
                                    //  $("#lblMessageifNoSelected").hide();
                                }
                                else {
                                    $("#btnMakePayment").hide();
                                    // $("#lblMessageifNoSelected").show();
                                }
                            });

                        });
                    });

                    var specialKeys = new Array();
                    specialKeys.push(8); //Backspace
                    function IsNumeric(e) {
                        var keyCode = e.which ? e.which : e.keyCode
                        var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
                        return ret;
                    }
                </script>
                <!-- Modal -->
                <div id="DecModalforhealth" class="modal fade" role="dialog" style="z-index: 9999">
                    <div class="modal-dialog">

                        <!-- Modal content-->
                        <div class="modal-content">
                            <%-- <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">Terms and Conditions/ Declarations:</h4>
                            </div>--%>
                            <div class="modal-body">

                                <p>
                                    <strong><b>
                                        <h4>Declaration of Good Health</h4>
                                    </b></strong>
                                </p>
                                <p>
                                    I hereby declare that to the best of my knowledge, I am of good and sound health at present. I / We (Persons to be covered mentioned in section II: Insured details) have not suffered in past from any major disease / disorder /ailment / deformity / surgery or neither awaiting any treatment medical or surgical nor attending any follow up for any disease / condition / ailment / injury / addiction.                               
                                </p>
                                <p>
                                    <strong><b>
                                        <h4>Declarations / Terms and Conditions:</h4>
                                    </b></strong>
                                </p>
                                <p>
                                    <strong><b>
                                        <h4>Standard Declarations:   </h4>
                                    </b></strong>

                                </p>

                                <p>
                                    I hereby declare, on my behalf and on behalf of all persons proposed to be insured, that the above statements, answers and/or particulars given by me are true and complete in all respects to the best of my knowledge and that I am authorized to propose on behalf of these other persons.<br />
                                    I understand that the information provided by me will form the basis of the insurance policy, is subject to the Board approved underwriting policy of the insurer and that the policy will come into force only after full payment of the premium chargeable.<br />
                                    I further declare that I will notify in writing any change occurring in the occupation or general health of the life to be insured/proposer after the proposal has been submitted but before communication of the risk acceptance by the company.                                   
                                    <br />
                                    I declare that I consent to the company seeking medical information from any doctor or hospital who/which at any time has attended on the person to be insured/proposer or from any past or present employer concerning anything which affects the physical or mental health of the person to be insured/proposer and seeking information from any insurer to whom an application for insurance on the person to be insured /proposer has been made for the purpose of underwriting the proposal and/or claim settlement.                                  
                                    <br />
                                    I authorize the company to share information pertaining to my proposal including the medical records of the insured/proposer for the sole purpose of underwriting the proposal and/or claims settlement and with any Governmental and/or Regulatory authority.”
                                </p>
                                <p>
                                    <strong><b>
                                        <h4>Terms and Conditions:  </h4>
                                    </b></strong>
                                </p>
                                <ul>
                                    <li>This insurance coverage is subject to the terms, conditions and exclusions of Kotak Group Health Assure Policy issued to Kotak Mahindra Bank Limited covering their customers and based on this enrollment and payment of premium.</li>
                                    <li>In case of any claim made under the Policy, no premium shall be refunded on cancellation of Insurance. 
                                    </li>
                                    <li>The insurance coverage shall commence from the date of receipt of premium by Kotak Mahindra General Insurance Company Limited</li>
                                    <li>The policy shall be issued basis the details provided by you. The policy may become voidable at the option of Insurer, in event of any untrue or incorrect statement, misrepresentation, non-description or non-disclosure in any material facts submitted.</li>
                                </ul>
                                <p>
                                    <strong><b>
                                        <h4>Go Green/ Go Paperless:</h4>
                                    </b></strong>
                                </p>
                                <p>
                                    I would like to protect and contribute in conserving the environment and help save paper by authorizing Kotak Mahindra General Insurance Company Limited to send all my policy and service related communication in soft copy to the email id as mentioned in the application form.
                                </p>
                                <p>
                                    <strong><b>
                                        <h4>Pls note:</h4>
                                    </b></strong>
                                </p>
                                <ul>
                                    <li>Issuance of policy is subject to receipt of premium, in case the premium is not received, the policy shall be void ab-initio.</li>
                                    <li>The policy is issued basis the details provided by you. In case of any untrue or incorrect statement, misrepresentation, non-description or non-disclosure, the policy is liable to be cancelled.
                                    </li>
                                    <li>The policy is subject to the underwriting guidelines of the Company.
                                    </li>

                                    <li>In case of refund of premium, the amount shall be remitted to the same account/number from where the premium is paid.</li>

                                    <li>Online premium payment should be made by the policyholder himself. No third-party payment should be made using this mode of payment</li>
                                </ul>
                                <p>
                                    Toll Free: 1800 266 4545 Email: <u>care@kotak.com</u> Website: <a href="https://www.kotakgeneral.com/" target="_blank">www.kotakgeneral.com</a><br />
                                    Registered Office: 27 BKC, C 27, G Block, Bandra Kurla Complex, Bandra (East), Mumbai - 400051. Maharashtra, India. CIN: U66000MH2014PLC260291. IRDAI Regn. No 152.<br />
                                    For more details on risk factors, terms, conditions, coverages and exclusions, please read the sales brochure carefully before concluding a sale<br />
                                    Tax benefits under Section 8- of the Income Tax are subject to change in the applicable tax laws. Please consult your financial advisor for more details.<br />
                                    Kotak Group Health Assure (UIN - KOTHLGP22168V012122)<br />
                                </p>
                                <p>
                                    <b>
                                        <h4><strong>STATUTORY WARNING</strong></h4>
                                    </b>
                                    <b>
                                        <h4><strong>PROHIBITION OF REBATES</strong>(Under Section 41 of Insurance Act 1938 as amended)</h4>
                                    </b>

                                </p>
                                <p>
                                    1) No person shall allow or offer to allow, either directly or indirectly as an inducement to any person to take out or renew or continue an insurance in respect of any kind of risk relating to lives or property, in India, any rebate of the whole or part of the commission payable or any rebate of the premium shown on the Policy, nor shall any person taking out or renewing or continuing a Policy accept any rebate, except such rebate as may be allowed in accordance with the published prospectuses or tables of the Insurer.<br />
                                    2) Any person making default in complying with the provisions of this section shall be punishable with fine, which may extend to Ten Lakhs Rupees.
                                </p>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="wrapper">
                    <!-- top navbar-->
                    <header class="topnavbar-wrapper" id="Mainheader">
                        <!-- START Top Navbar-->
                        <nav role="navigation" class="navbar topnavbar">
                            <!-- START navbar header-->
                            <div class="navbar-header">
                                <a href="#/" class="navbar-brand">
                                    <div class="brand-logo">
                                        <img src="Images/logo1.png" alt="App Logo" class="img-responsive" />
                                    </div>
                                    <div class="brand-logo-collapsed">
                                        <img src="Images/logo1.png" alt="App Logo" class="img-responsive" />
                                    </div>
                                </a>
                            </div>
                            <!-- END navbar header-->

                            <!-- START Nav wrapper-->
                            <div class="nav-wrapper">

                                <!-- START Right Navbar-->
                                <ul class="nav navbar-nav navbar-right">



                                    <li>
                                        <a href="mailto://care@kotak.com" class="email"><em class="fa fa-envelope"></em>&nbsp;care@kotak.com</a>
                                    </li>

                                    <li>
                                        <a href="tel:1800 266 4545" class="tollfree"><em class="fa fa-phone"></em>&nbsp;1800 266 4545</a>
                                    </li>

                                </ul>
                                <br />
                                <br />
                                <ul class="nav navbar-nav navbar-right clsdownloadbrochure">

                                    <li>
                                        <a style="font-size: 19px;" href="https://kgipass.kotakgeneralinsurance.com/KGIPASS/downloads/KGHA_Brouchure.pdf" target="_blank"><em class="fa fa-download"></em>&nbsp;<strong>Product Brochure</strong></a>
                                    </li>



                                </ul>
                                <!-- END Right Navbar-->
                            </div>

                            <div class="nav-wrapper">
                                <div class="row">
                                    <div class="col-lg-12 col-xs-12" style="text-align: center;"><span class="h2 text-bold">Kotak Group Health Assure</span></div>
                                </div>
                                <%--<ul class="nav navbar-nav navbar-right">
                                    <li>
                                        <a class="tollfree">To arrange a call back:&nbsp;022-XXXXXXX</a>
                                    </li>
                                </ul>--%>
                            </div>
                            <!-- END Nav wrapper-->

                        </nav>
                        <!-- END Top Navbar-->
                    </header>


                    <section id="sectionMain" runat="server">
                        <div class="content-wrapper">
                            <div class="container container-md">
                                <div class="row mb-lg">
                                    <div class="col-lg-12" style="text-align: center;">

                                        <div>
                                            <span class="h3 text-bold">Welcome</span>
                                            <asp:Label ID="lblEmployeeName" runat="server" Text="" CssClass="h3 text-bold"></asp:Label>
                                            <span class="h3 text-bold">(</span><asp:Label ID="lblVerifiedCRN" runat="server" Text="" CssClass="h3 text-bold"></asp:Label><span class="h3 text-bold">)</span>

                                            Email: <b>
                                                <asp:Label ID="lblEmpEmailId" runat="server" Text=""></asp:Label></b> | Mobile: <b>
                                                    <asp:Label ID="lblPhone" runat="server" Text=""></asp:Label></b>
                                        </div>


                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-12">
                                        <!-- START panel-->
                                        <div id="panelDemo14" class="panel panel-default" style="border-top-width: 2px;">
                                            <div class="panel-heading text-center" style="background-color: #ff902b; color: white; font-weight: bold;">Verify &amp; Fill Required Details Below</div>
                                            <div class="panel-body">
                                                <div role="tabpanel">
                                                    <!-- Nav tabs-->
                                                    <ul role="tablist" class="nav nav-tabs">

                                                        <asp:Literal ID="LiteralTabNavigation" runat="server" Text=""></asp:Literal>
                                                    </ul>
                                                    <!-- Tab panes-->
                                                    <div class="tab-content" id="divTabContent1">
                                                        <asp:Literal ID="LiteralTabContent" runat="server" Text=""></asp:Literal>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- END panel-->
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-12">
                                        <!-- START panel-->
                                        <div id="panelDemo15" class="panel panel-default" style="border-top-width: 2px;">
                                            <div class="panel-heading text-center" style="background-color: #ff902b; color: white; font-weight: bold;">Select the Floater Option</div>
                                            <div class="panel-body">
                                                <div role="tabpanel">

                                                    <!-- Tab panes-->
                                                    <div class="tab-content" id="divTabContent2">
                                                        <div id="1" role="tabpanel" class="tab-pane active">
                                                            <div class="row" id="row1">
                                                                <div class="col-md-6">
                                                                    <div class="row">
                                                                        <div class="col-sm-10 col-xs-10">
                                                                            <dl style="margin-bottom: 5px">
                                                                                <dt>Cover yourself for just Rs 1,999/-</dt>

                                                                            </dl>
                                                                        </div>
                                                                        <div class="col-sm-2 col-xs-2">
                                                                            <dl style="margin-bottom: 5px">
                                                                                <dt>
                                                                                    <label class="radio-inline c-radio">
                                                                                        <input id="inlineradioself" type="radio" name="radiofloaterPlan" value="PlanA" />
                                                                                        <span class="fa fa-circle"></span>
                                                                                    </label>
                                                                                </dt>

                                                                            </dl>
                                                                        </div>
                                                                        <div class="col-sm-10 col-xs-10">
                                                                            <dl style="margin-bottom: 5px">
                                                                                <dt>Cover yourself and your Spouse for just Rs 2,199/-</dt>

                                                                            </dl>
                                                                        </div>
                                                                        <div class="col-sm-2 col-xs-2">
                                                                            <dl style="margin-bottom: 5px">
                                                                                <dt>
                                                                                    <label class="radio-inline c-radio">
                                                                                        <input id="inlineradioselfspouse" type="radio" name="radiofloaterPlan" value="PlanB" />
                                                                                        <span class="fa fa-circle"></span>
                                                                                    </label>
                                                                                </dt>

                                                                            </dl>
                                                                        </div>
                                                                        <div class="col-sm-10 col-xs-10">
                                                                            <dl style="margin-bottom: 5px">
                                                                                <dt>Cover yourself, your Spouse and your Child for just Rs 2,399/-</dt>

                                                                            </dl>
                                                                        </div>
                                                                        <div class="col-sm-2 col-xs-2">
                                                                            <dl style="margin-bottom: 5px">
                                                                                <dt>
                                                                                    <label class="radio-inline c-radio">
                                                                                        <input id="inlineradioselfspousechild" type="radio" name="radiofloaterPlan" value="PlanC" />
                                                                                        <span class="fa fa-circle"></span>
                                                                                    </label>
                                                                                </dt>

                                                                            </dl>
                                                                        </div>
                                                                        <div class="col-sm-10 col-xs-10">
                                                                            <dl style="margin-bottom: 5px">
                                                                                <dt>Cover yourself, your Spouse and your Two Children for just Rs 2,599/-</dt>

                                                                            </dl>
                                                                        </div>
                                                                        <div class="col-sm-2 col-xs-2">
                                                                            <dl style="margin-bottom: 5px">
                                                                                <dt>
                                                                                    <label class="radio-inline c-radio">
                                                                                        <input id="inlineradioselfspousechildren" type="radio" name="radiofloaterPlan" value="PlanD" />
                                                                                        <span class="fa fa-circle"></span>
                                                                                    </label>
                                                                                </dt>

                                                                            </dl>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- END panel-->
                                    </div>
                                </div>

                                <div class="row" id="divPlanA" style="display: none;">
                                    <div class="col-lg-12">
                                        <!-- START panel-->
                                        <div id="panelDemo16" class="panel panel-default" style="border-top-width: 2px;">
                                            <div class="panel-heading text-center" style="background-color: #5d5784; color: white; font-weight: bold;">Cover yourself for just Rs 1,999/-</div>
                                            <div class="panel-body">
                                                <div role="tabpanel">
                                                    <div class="row">
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Sum Insured  </dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>1500000  </dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Tenure</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>1 Year</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left ">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Net Premium</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>1694</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>GST @ 18%</dt>

                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>305</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Total Amount Payable</dt>

                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>
                                                                    <asp:Label ID="lblTotalAmountPayableplan1" runat="server" Text="1999"></asp:Label></dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Deductible</dt>

                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>
                                                                    <asp:Label ID="Label4" runat="server" Text="200000"></asp:Label></dt>
                                                            </dl>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <!-- Nav tabs-->
                                                    <ul role="tablist" class="nav nav-tabs">
                                                        <li role="presentation" class="Active"><a href="proposertab" aria-controls="1" role="tab" data-toggle="tab" aria-expanded="false">Self</a></li>
                                                    </ul>
                                                    <!-- Tab panes-->
                                                    <div class="tab-content" id="divTabContentplanA">
                                                        <div role="tabpanel" class="tab-pane active" id="proposertab" name="Self">
                                                            <div class="row" id="row2">

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Name</dt>
                                                                        <dd>
                                                                            <input id="txtmembername1" name="txtmembername" readonly="true" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="50" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtmemberdob1" name="txtmemberdob" disabled="disabled" type="text" readonly="true" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Age</dt>
                                                                        <dd>
                                                                            <input id="txtmemberage1" name="txtmemberage" readonly="true" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Gender</dt>
                                                                        <dd>
                                                                            <select id="ddmembergender1" name="ddmembergender" class="form-control" disabled="disabled" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="M">Male</option>
                                                                                <option value="F">Female</option>

                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Occupaton</dt>
                                                                        <dd>
                                                                            <select id="ddselfoccupation1" name="ddoccupation" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Business">Business</option>
                                                                                <option value="Salaried">Salaried</option>
                                                                                <option value="Professional">Professional</option>
                                                                                <option value="Student">Student</option>
                                                                                <option value="Housewife">Housewife</option>
                                                                                <option value="Retired">Retired</option>
                                                                                <option value="Others">Others</option>
                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Martial Status</dt>
                                                                        <dd>
                                                                            <select id="ddselfmartialstatus1" name="ddmartialstatus" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Single">Single</option>
                                                                                <option value="Married">Married</option>
                                                                                <option value="Divorced">Divorced</option>
                                                                                <option value="Widowed">Widowed</option>
                                                                                <option value="Others">Others</option>

                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Member Relationship with the Proposer</dt>
                                                                        <dd>
                                                                            <select id="ddselfrelation1" name="ddmemberrelation" disabled="disabled" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">

                                                                                <option value="Self">Self</option>
                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Name</dt>
                                                                        <dd>
                                                                            <input id="txtmemberNomineeName1" name="txtmemberNomineeName" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtmemberNomineeDOB1" name="txtmemberNomineeDOB" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Relationship with Proposer</dt>
                                                                        <dd>
                                                                            <select id="cboselfNomineeRelationship1" name="cboNomineeRelationship1" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected="selected">Select</option>
                                                                                <option value="Father">Father</option>
                                                                                <option value="Mother">Mother</option>
                                                                                <option value="Sister">Sister</option>
                                                                                <option value="Son">Son</option>
                                                                                <option value="Daughter">Daughter</option>
                                                                                <option value="Spouse">Spouse</option>
                                                                                <option value="Brother">Brother</option>
                                                                                <option value="Father-In-Law">Father-In-Law</option>
                                                                                <option value="Mother-In-Law">Mother-In-Law</option>
                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- END panel-->
                                    </div>
                                </div>
                                <div class="row" id="divPlanB" style="display: none;">
                                    <div class="col-lg-12">
                                        <!-- START panel-->
                                        <div id="panelDemo17" class="panel panel-default" style="border-top-width: 2px;">
                                            <div class="panel-heading text-center" style="background-color: #e5104f; color: white; font-weight: bold;">Cover yourself and your Spouse for just Rs 2,199/-</div>
                                            <div class="panel-body">
                                                <div role="tabpanel">
                                                    <div class="row">
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Sum Insured  </dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>1500000  </dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Tenure</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>1 Year</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left ">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Net Premium</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>1864</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>GST @ 18%</dt>

                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>305</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Total Amount Payable</dt>

                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>
                                                                    <asp:Label ID="lblTotalAmountPayableplan2" runat="server" Text="2199"></asp:Label></dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Deductible</dt>

                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>
                                                                    <asp:Label ID="Label3" runat="server" Text="200000"></asp:Label></dt>
                                                            </dl>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <!-- Nav tabs-->
                                                    <ul role="tablist" class="nav nav-tabs">
                                                        <li role="presentation" class="Active"><a href="#divselfmemberdetails" aria-controls="1" role="tab" data-toggle="tab" aria-expanded="false">Self</a></li>
                                                        <li role="presentation"><a href="#divspousedetails" aria-controls="1" role="tab" data-toggle="tab" aria-expanded="false">Spouse</a></li>

                                                    </ul>
                                                    <!-- Tab panes-->
                                                    <div class="tab-content" id="divTabContentplanB">
                                                        <div role="tabpanel" class="tab-pane active" id="divselfmemberdetails" name="Self">
                                                            <div class="row">

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Name</dt>
                                                                        <dd>
                                                                            <input id="txtselfname2" name="txtmembername1" type="text" class="form-control" readonly="true" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="50" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtselfdob2" name="txtmemberdob1" type="text" disabled="disabled" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Age</dt>
                                                                        <dd>
                                                                            <input id="txtselfage2" name="txtmemberage1" readonly="true" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Gender</dt>
                                                                        <dd>
                                                                            <select id="ddmembergender2" name="ddmembergender1" class="form-control" disabled="disabled" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="M">Male</option>
                                                                                <option value="F">Female</option>

                                                                            </select></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Occupaton</dt>
                                                                        <dd>
                                                                            <select id="ddselfoccupation2" name="ddoccupation1" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Business">Business</option>
                                                                                <option value="Salaried">Salaried</option>
                                                                                <option value="Professional">Professional</option>
                                                                                <option value="Student">Student</option>
                                                                                <option value="Housewife">Housewife</option>
                                                                                <option value="Retired">Retired</option>
                                                                                <option value="Others">Others</option>
                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Martial Status</dt>
                                                                        <dd>

                                                                            <select id="ddselfmartialstatus2" name="ddmartialstatus1" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Single">Single</option>
                                                                                <option value="Married">Married</option>
                                                                                <option value="Divorced">Divorced</option>
                                                                                <option value="Widowed">Widowed</option>
                                                                                <option value="Others">Others</option>

                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Member Relationship with the Proposer
                                                                        </dt>
                                                                        <dd>
                                                                            <select id="ddselfrelation2" name="ddmemberrelation1" disabled="disabled" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">

                                                                                <option value="Self">Self</option>
                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Name</dt>
                                                                        <dd>
                                                                            <input id="txtselfNomineeName2" name="txtNomineeName1" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtselfNomineeDOB2" name="txtNomineeDOB1" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Relationship with Proposer</dt>
                                                                        <dd>

                                                                            <select id="cboselfNomineeRelationship2" name="cboNomineeRelationship1" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Father">Father</option>
                                                                                <option value="Mother">Mother</option>
                                                                                <option value="Sister">Sister</option>
                                                                                <option value="Son">Son</option>
                                                                                <option value="Daughter">Daughter</option>
                                                                                <option value="Spouse">Spouse</option>
                                                                                <option value="Brother">Brother</option>
                                                                                <option value="Father-In-Law">Father-In-Law</option>
                                                                                <option value="Mother-In-Law">Mother-In-Law</option>
                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div role="tabpanel" class="tab-pane" id="divspousedetails" name="Spouse">
                                                            <div class="row">

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Name</dt>
                                                                        <dd>
                                                                            <input id="txtspousename2" name="txtmembername2" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="50" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtspousedob2" name="txtmemberdob2" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Age</dt>
                                                                        <dd>
                                                                            <input id="txtspouseage2" name="txtmemberage2" readonly="true" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Gender</dt>
                                                                        <dd>
                                                                            <select id="ddspousegender2" name="ddmembergender2" class="form-control" disabled="disabled" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="M">Male</option>
                                                                                <option value="F">Female</option>

                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Occupaton</dt>
                                                                        <dd>
                                                                            <select id="ddspouseoccupation2" name="ddoccupation2" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Business">Business</option>
                                                                                <option value="Salaried">Salaried</option>
                                                                                <option value="Professional">Professional</option>
                                                                                <option value="Student">Student</option>
                                                                                <option value="Housewife">Housewife</option>
                                                                                <option value="Retired">Retired</option>
                                                                                <option value="Others">Others</option>
                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Martial Status</dt>
                                                                        <dd>
                                                                            <select id="ddspousemartialstatus2" name="ddmartialstatus2" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Single">Single</option>
                                                                                <option value="Married">Married</option>
                                                                                <option value="Divorced">Divorced</option>
                                                                                <option value="Widowed">Widowed</option>
                                                                                <option value="Others">Others</option>

                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Member Relationship with the Proposer
                                                                        </dt>
                                                                        <dd>
                                                                            <select id="ddspouserelation2" name="ddmemberrelation2" disabled="disabled" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Husband">Husband</option>
                                                                                <option value="Wife">Wife</option>
                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>


                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Name</dt>
                                                                        <dd>
                                                                            <input id="txtspouseNomineeName2" name="txtNomineeName2" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtspouseNomineeDOB2" name="txtNomineeDOB2" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Relationship with Proposer</dt>
                                                                        <dd>

                                                                            <select id="cbospouseNomineeRelationship2" name="cboNomineeRelationship1" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Father">Father</option>
                                                                                <option value="Mother">Mother</option>
                                                                                <option value="Sister">Sister</option>
                                                                                <option value="Son">Son</option>
                                                                                <option value="Daughter">Daughter</option>
                                                                                <option value="Spouse">Spouse</option>
                                                                                <option value="Brother">Brother</option>
                                                                                <option value="Father-In-Law">Father-In-Law</option>
                                                                                <option value="Mother-In-Law">Mother-In-Law</option>
                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                        <!-- END panel-->
                                    </div>
                                </div>
                                <div class="row" id="divPlanC" style="display: none;">
                                    <div class="col-lg-12">
                                        <!-- START panel-->
                                        <div id="panelDemo18" class="panel panel-default" style="border-top-width: 2px;">
                                            <div class="panel-heading text-center" style="background-color: #8a94f1; color: white; font-weight: bold;">Cover yourself, your Spouse and your Child for just Rs 2,399/-</div>
                                            <div class="panel-body">
                                                <div role="tabpanel">
                                                    <div class="row">
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Sum Insured  </dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>1500000  </dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Tenure</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>1 Year</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left ">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Net Premium</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>2033</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>GST @ 18%</dt>

                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>366</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Total Amount Payable</dt>

                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>
                                                                    <asp:Label ID="lblTotalAmountPayableplan3" runat="server" Text="2399"></asp:Label></dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Deductible</dt>

                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>
                                                                    <asp:Label ID="Label2" runat="server" Text="200000"></asp:Label></dt>
                                                            </dl>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <!-- Nav tabs-->
                                                    <ul role="tablist" class="nav nav-tabs">
                                                        <li role="presentation" class="Active"><a href="#divselfmemberdetails3" aria-controls="1" role="tab" data-toggle="tab" aria-expanded="false">Self</a></li>
                                                        <li role="presentation"><a href="#divspousedetails3" aria-controls="1" role="tab" data-toggle="tab" aria-expanded="false">Spouse</a></li>
                                                        <li role="presentation"><a href="#divchilddetails3" aria-controls="1" role="tab" data-toggle="tab" aria-expanded="false">Child</a></li>

                                                    </ul>
                                                    <!-- Tab panes-->
                                                    <div class="tab-content" id="divTabContentplanC">
                                                        <div role="tabpanel" class="tab-pane active" id="divselfmemberdetails3" name="Self">
                                                            <div class="row">

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Name</dt>
                                                                        <dd>
                                                                            <input id="txtselfname3" name="txtmembername1" type="text" class="form-control" readonly="true" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="50" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtselfdob3" name="txtmemberdob1" type="text" disabled="disabled" readonly="true" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Age</dt>
                                                                        <dd>
                                                                            <input id="txtselfage3" name="txtmemberage1" readonly="true" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Gender</dt>
                                                                        <dd>
                                                                            <select id="ddselfgender3" name="ddmembergender1" class="form-control" disabled="disabled" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="M">Male</option>
                                                                                <option value="F">Female</option>

                                                                            </select></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Occupaton</dt>
                                                                        <dd>
                                                                            <select id="ddselfoccupation3" name="ddoccupation1" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Business">Business</option>
                                                                                <option value="Salaried">Salaried</option>
                                                                                <option value="Professional">Professional</option>
                                                                                <option value="Student">Student</option>
                                                                                <option value="Housewife">Housewife</option>
                                                                                <option value="Retired">Retired</option>
                                                                                <option value="Others">Others</option>
                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Martial Status</dt>
                                                                        <dd>
                                                                            <select id="ddselfmartialstatus3" name="ddmartialstatus1" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Single">Single</option>
                                                                                <option value="Married">Married</option>
                                                                                <option value="Divorced">Divorced</option>
                                                                                <option value="Widowed">Widowed</option>
                                                                                <option value="Others">Others</option>

                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Member Relationship with the Proposer
                                                                        </dt>
                                                                        <dd>
                                                                            <select id="ddtselfrelation3" name="ddmemberrelation1" disabled="disabled" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Self" selected="selected">Self</option>

                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>


                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Name</dt>
                                                                        <dd>
                                                                            <input id="txtselfNomineeName3" name="txtNomineeName1" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtselfNomineeDOB3" name="txtNomineeDOB1" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Relationship with Proposer</dt>
                                                                        <dd>

                                                                            <select id="cboselfNomineeRelationship3" name="cboNomineeRelationship1" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Father">Father</option>
                                                                                <option value="Mother">Mother</option>
                                                                                <option value="Sister">Sister</option>
                                                                                <option value="Son">Son</option>
                                                                                <option value="Daughter">Daughter</option>
                                                                                <option value="Spouse">Spouse</option>
                                                                                <option value="Brother">Brother</option>
                                                                                <option value="Father-In-Law">Father-In-Law</option>
                                                                                <option value="Mother-In-Law">Mother-In-Law</option>
                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div role="tabpanel" class="tab-pane" id="divspousedetails3" name="Spouse">
                                                            <div class="row">

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Name</dt>
                                                                        <dd>
                                                                            <input id="txtspousename3" name="txtmembername2" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="50" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtspousedob3" name="txtmemberdob2" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Age</dt>
                                                                        <dd>
                                                                            <input id="txtspouseage3" name="txtmemberage2" readonly="true" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Gender</dt>
                                                                        <dd>
                                                                            <select id="ddspousegender3" name="ddmembergender2" disabled="disabled" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="M">Male</option>
                                                                                <option value="F">Female</option>

                                                                            </select></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Occupaton</dt>
                                                                        <dd>
                                                                            <select id="ddspousoccuption3" name="ddoccupation2" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Business">Business</option>
                                                                                <option value="Salaried">Salaried</option>
                                                                                <option value="Professional">Professional</option>
                                                                                <option value="Student">Student</option>
                                                                                <option value="Housewife">Housewife</option>
                                                                                <option value="Retired">Retired</option>
                                                                                <option value="Others">Others</option>
                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Martial Status</dt>
                                                                        <dd>
                                                                            <select id="ddspousemartialstatus3" name="ddmartialstatus2" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Single">Single</option>
                                                                                <option value="Married">Married</option>
                                                                                <option value="Divorced">Divorced</option>
                                                                                <option value="Widowed">Widowed</option>
                                                                                <option value="Others">Others</option>

                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Member Relationship with the Proposer
                                                                        </dt>
                                                                        <dd>
                                                                            <select id="ddspouserelation3" name="ddmemberrelation2" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Wife">Wife</option>
                                                                                <option value="Husband">Husband</option>
                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>


                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Name</dt>
                                                                        <dd>
                                                                            <input id="txtspouseNomineeName3" name="txtNomineeName2" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtspouseNomineeDOB3" name="txtNomineeDOB2" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Relationship with Proposer</dt>
                                                                        <dd>

                                                                            <select id="cbospouseNomineeRelationship3" name="cboNomineeRelationship1" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Father">Father</option>
                                                                                <option value="Mother">Mother</option>
                                                                                <option value="Sister">Sister</option>
                                                                                <option value="Son">Son</option>
                                                                                <option value="Daughter">Daughter</option>
                                                                                <option value="Spouse">Spouse</option>
                                                                                <option value="Brother">Brother</option>
                                                                                <option value="Father-In-Law">Father-In-Law</option>
                                                                                <option value="Mother-In-Law">Mother-In-Law</option>
                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>

                                                            </div>
                                                        </div>
                                                        <div role="tabpanel" class="tab-pane" id="divchilddetails3" name="1st Child">
                                                            <div class="row">

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Name</dt>
                                                                        <dd>
                                                                            <input id="txtchildname3" name="txtmembername3" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="50" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtchilddob3" name="txtmemberdob3" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Age</dt>
                                                                        <dd>
                                                                            <input id="txtxhildage3" name="txtmemberage3" readonly="true" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>

                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Gender</dt>
                                                                        <dd>
                                                                            <select id="ddchildgender3" name="ddmembergender3" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="M">Male</option>
                                                                                <option value="F">Female</option>

                                                                            </select></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Occupaton</dt>
                                                                        <dd>
                                                                            <select id="ddchildoccupation3" name="ddoccupation3" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Business">Business</option>
                                                                                <option value="Salaried">Salaried</option>
                                                                                <option value="Professional">Professional</option>
                                                                                <option value="Student">Student</option>
                                                                                <option value="Housewife">Housewife</option>
                                                                                <option value="Retired">Retired</option>
                                                                                <option value="Others">Others</option>
                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Martial Status</dt>
                                                                        <dd>
                                                                            <select id="ddchildmartialstatus3" name="ddmartialstatus3" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Single">Single</option>
                                                                                <option value="Married">Married</option>
                                                                                <option value="Divorced">Divorced</option>
                                                                                <option value="Widowed">Widowed</option>
                                                                                <option value="Others">Others</option>

                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Member Relationship with the Proposer
                                                                        </dt>
                                                                        <dd>

                                                                            <select id="ddchildrelation3" name="ddmemberrelation3" disabled="disabled" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Father">Father</option>
                                                                                <option value="Mother">Mother</option>

                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Name</dt>
                                                                        <dd>
                                                                            <input id="txtchildNomineeName3" name="txtNomineeName3" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtchildNomineeDOB3" name="txtNomineeDOB3" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Relationship with Proposer</dt>
                                                                        <dd>

                                                                            <select id="cbochildNomineeRelationship3" name="cboNomineeRelationship1" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Father">Father</option>
                                                                                <option value="Mother">Mother</option>
                                                                                <option value="Sister">Sister</option>
                                                                                <option value="Son">Son</option>
                                                                                <option value="Daughter">Daughter</option>
                                                                                <option value="Spouse">Spouse</option>
                                                                                <option value="Brother">Brother</option>
                                                                                <option value="Father-In-Law">Father-In-Law</option>
                                                                                <option value="Mother-In-Law">Mother-In-Law</option>
                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                        <!-- END panel-->
                                    </div>
                                </div>
                                <div class="row" id="divPlanD" style="display: none;">
                                    <div class="col-lg-12">
                                        <!-- START panel-->
                                        <div id="panelDemo19" class="panel panel-default" style="border-top-width: 2px;">
                                            <div class="panel-heading text-center" style="background-color: #b3a41a; color: white; font-weight: bold;">Cover yourself, your Spouse and your Two Children for just Rs 2,599/-</div>
                                            <div class="panel-body">
                                                <div role="tabpanel">
                                                    <div class="row">
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Sum Insured  </dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>1500000  </dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Tenure</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>1 Year</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left ">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Net Premium</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>2203</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>GST @ 18%</dt>

                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>396</dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Total Amount Payable</dt>

                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>
                                                                    <asp:Label ID="lblTotalAmountPayableplan4" runat="server" Text="2599"></asp:Label></dt>
                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>Deductible</dt>

                                                            </dl>
                                                        </div>
                                                        <div class="col-sm-2 col-xs-6 text-left">
                                                            <dl style="margin-bottom: 5px">
                                                                <dt>
                                                                    <asp:Label ID="Label1" runat="server" Text="200000"></asp:Label></dt>
                                                            </dl>
                                                        </div>

                                                    </div>
                                                    <br />
                                                    <!-- Nav tabs-->
                                                    <ul role="tablist" class="nav nav-tabs">
                                                        <li role="presentation" class="Active"><a href="#divselfmemberdetails4" aria-controls="1" role="tab" data-toggle="tab" aria-expanded="false">Self</a></li>
                                                        <li role="presentation"><a href="#divspousedetails4" aria-controls="1" role="tab" data-toggle="tab" aria-expanded="false">Spouse</a></li>
                                                        <li role="presentation"><a href="#divchilddetails4" aria-controls="1" role="tab" data-toggle="tab" aria-expanded="false">Child</a></li>
                                                        <li role="presentation"><a href="#divchild2details4" aria-controls="1" role="tab" data-toggle="tab" aria-expanded="false">2nd Child</a></li>

                                                    </ul>
                                                    <!-- Tab panes-->
                                                    <div class="tab-content" id="divTabContentplanD">
                                                        <div role="tabpanel" class="tab-pane active" id="divselfmemberdetails4" name="Self">
                                                            <div class="row">

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Name</dt>
                                                                        <dd>
                                                                            <input id="txtselfname4" name="txtmembername1" readonly="true" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="50" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtselfdob4" name="txtmemberdob1" disabled="disabled" readonly="true" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Age</dt>
                                                                        <dd>
                                                                            <input id="txtselfage4" name="txtmemberage1" readonly="true" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Gender</dt>
                                                                        <dd>

                                                                            <select id="ddselfgender4" name="ddmembergender1" disabled="disabled" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="M">Male</option>
                                                                                <option value="F">Female</option>

                                                                            </select></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Occupaton</dt>
                                                                        <dd>
                                                                            <select id="ddselfoccupation4" name="ddoccupation1" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Business">Business</option>
                                                                                <option value="Salaried">Salaried</option>
                                                                                <option value="Professional">Professional</option>
                                                                                <option value="Student">Student</option>
                                                                                <option value="Housewife">Housewife</option>
                                                                                <option value="Retired">Retired</option>
                                                                                <option value="Others">Others</option>
                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Martial Status</dt>
                                                                        <dd>
                                                                            <select id="ddselfmartialstatus4" name="ddmartialstatus1" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Single">Single</option>
                                                                                <option value="Married">Married</option>
                                                                                <option value="Divorced">Divorced</option>
                                                                                <option value="Widowed">Widowed</option>
                                                                                <option value="Others">Others</option>

                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Member Relationship with the Proposer
                                                                        </dt>
                                                                        <dd>
                                                                            <select id="ddselfrealtion4" name="ddmemberrelation1" class="form-control" disabled="disabled" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">

                                                                                <option value="Self">Self</option>


                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>


                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Name</dt>
                                                                        <dd>
                                                                            <input id="txtselfNomineeName4" name="txtselfNomineeName4" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtselfNomineeDOB4" name="txtNomineeDOB1" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Relationship with Proposer</dt>
                                                                        <dd>

                                                                            <select id="cboselfNomineeRelationship4" name="cboNomineeRelationship1" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Father">Father</option>
                                                                                <option value="Mother">Mother</option>
                                                                                <option value="Sister">Sister</option>
                                                                                <option value="Son">Son</option>
                                                                                <option value="Daughter">Daughter</option>
                                                                                <option value="Spouse">Spouse</option>
                                                                                <option value="Brother">Brother</option>
                                                                                <option value="Father-In-Law">Father-In-Law</option>
                                                                                <option value="Mother-In-Law">Mother-In-Law</option>
                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div role="tabpanel" class="tab-pane" id="divspousedetails4" name="Spouse">
                                                            <div class="row">

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Name</dt>
                                                                        <dd>
                                                                            <input id="txtspousename4" name="txtmembername2" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="50" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtspousedob4" name="txtmemberdob2" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Age</dt>
                                                                        <dd>
                                                                            <input id="txtspouseage4" name="txtmemberage2" readonly="true" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Gender</dt>
                                                                        <dd>
                                                                            <select id="ddspousegender4" name="ddmembergender2" class="form-control" disabled="disabled" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="M">Male</option>
                                                                                <option value="F">Female</option>

                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Occupaton</dt>
                                                                        <dd>
                                                                            <select id="ddspouseoccupdation4" name="ddoccupation2" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Business">Business</option>
                                                                                <option value="Salaried">Salaried</option>
                                                                                <option value="Professional">Professional</option>
                                                                                <option value="Student">Student</option>
                                                                                <option value="Housewife">Housewife</option>
                                                                                <option value="Retired">Retired</option>
                                                                                <option value="Others">Others</option>
                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Martial Status</dt>
                                                                        <dd>
                                                                            <select id="ddspousemartialstatus4" name="ddmartialstatus1" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Single">Single</option>
                                                                                <option value="Married">Married</option>
                                                                                <option value="Divorced">Divorced</option>
                                                                                <option value="Widowed">Widowed</option>
                                                                                <option value="Others">Others</option>

                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Member Relationship with the Proposer
                                                                        </dt>
                                                                        <dd>
                                                                            <select id="ddspouserelation4" name="ddmemberrelation2" class="form-control" disabled="disabled" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Husband">Husband</option>
                                                                                <option value="Wife">Wife</option>


                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>


                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Name</dt>
                                                                        <dd>
                                                                            <input id="txtspouseNomineeName4" name="txtNomineeName2" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtspouseNomineeDOB4" name="txtNomineeDOB2" value="" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Relationship with Proposer</dt>
                                                                        <dd>


                                                                            <select id="cbospouseNomineeRelationship4" name="cboNomineeRelationship1" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Father">Father</option>
                                                                                <option value="Mother">Mother</option>
                                                                                <option value="Sister">Sister</option>
                                                                                <option value="Son">Son</option>
                                                                                <option value="Daughter">Daughter</option>
                                                                                <option value="Spouse">Spouse</option>
                                                                                <option value="Brother">Brother</option>
                                                                                <option value="Father-In-Law">Father-In-Law</option>
                                                                                <option value="Mother-In-Law">Mother-In-Law</option>
                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>

                                                            </div>
                                                        </div>
                                                        <div role="tabpanel" class="tab-pane" id="divchilddetails4" name="1st Child">
                                                            <div class="row">

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Name</dt>
                                                                        <dd>
                                                                            <input id="txtchildname4" name="txtmembername3" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="50" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtchilddob4" name="txtmemberdob3" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Age</dt>
                                                                        <dd>
                                                                            <input id="txtchildage4" name="txtmemberage3" readonly="true" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Gender</dt>
                                                                        <dd>
                                                                            <select id="ddchildgender4" name="ddmembergender3" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="M">Male</option>
                                                                                <option value="F">Female</option>

                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Occupaton</dt>
                                                                        <dd>
                                                                            <select id="ddchildoccupation4" name="ddoccupation3" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Business">Business</option>
                                                                                <option value="Salaried">Salaried</option>
                                                                                <option value="Professional">Professional</option>
                                                                                <option value="Student">Student</option>
                                                                                <option value="Housewife">Housewife</option>
                                                                                <option value="Retired">Retired</option>
                                                                                <option value="Others">Others</option>
                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Martial Status</dt>
                                                                        <dd>
                                                                            <select id="ddchildmartialstatus4" name="ddmartialstatus3" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Single">Single</option>
                                                                                <option value="Married">Married</option>
                                                                                <option value="Divorced">Divorced</option>
                                                                                <option value="Widowed">Widowed</option>
                                                                                <option value="Others">Others</option>

                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Member Relationship with the Proposer
                                                                        </dt>
                                                                        <dd>

                                                                            <select id="ddchildrelation4" name="ddmemberrelation3" disabled="disabled" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Father">Father</option>
                                                                                <option value="Mother">Mother</option>

                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>



                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Name</dt>
                                                                        <dd>
                                                                            <input id="txtchildNomineeName4" name="txtNomineeName4" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtchildNomineeDOB4" name="txtNomineeDOB4" value="" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Relationship with Proposer</dt>
                                                                        <dd>

                                                                            <select id="cbochildNomineeRelationship4" name="cboNomineeRelationship4" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Father">Father</option>
                                                                                <option value="Mother">Mother</option>
                                                                                <option value="Sister">Sister</option>
                                                                                <option value="Son">Son</option>
                                                                                <option value="Daughter">Daughter</option>
                                                                                <option value="Spouse">Spouse</option>
                                                                                <option value="Brother">Brother</option>
                                                                                <option value="Father-In-Law">Father-In-Law</option>
                                                                                <option value="Mother-In-Law">Mother-In-Law</option>
                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>

                                                            </div>
                                                        </div>

                                                        <div role="tabpanel" class="tab-pane" id="divchild2details4" name="2nd Child">
                                                            <div class="row">

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Name</dt>
                                                                        <dd>
                                                                            <input id="txtchild2name4" name="txtmembername4" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="50" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtchild2dob4" name="txtmemberdob4" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Age</dt>
                                                                        <dd>
                                                                            <input id="txtchild2age4" name="txtmemberage4" readonly="true" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" maxlength="15" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-1">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Gender</dt>
                                                                        <dd>
                                                                            <select id="ddchild2gender4" name="ddmembergender4" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="M">Male</option>
                                                                                <option value="F">Female</option>

                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Occupaton</dt>
                                                                        <dd>
                                                                            <select id="ddchild2occupation4" name="ddoccupation4" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Business">Business</option>
                                                                                <option value="Salaried">Salaried</option>
                                                                                <option value="Professional">Professional</option>
                                                                                <option value="Student">Student</option>
                                                                                <option value="Housewife">Housewife</option>
                                                                                <option value="Retired">Retired</option>
                                                                                <option value="Others">Others</option>
                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Martial Status</dt>
                                                                        <dd>
                                                                            <select id="ddchild2martialstatus4" name="ddmartialstatus4" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Single">Single</option>
                                                                                <option value="Married">Married</option>
                                                                                <option value="Divorced">Divorced</option>
                                                                                <option value="Widowed">Widowed</option>
                                                                                <option value="Others">Others</option>

                                                                            </select>

                                                                        </dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Member Relationship with the Proposer</dt>
                                                                        <dd>

                                                                            <select id="ddchild2relation4" name="ddmemberrelation4" disabled="disabled" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Father">Father</option>
                                                                                <option value="Mother">Mother</option>

                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Name</dt>
                                                                        <dd>
                                                                            <input id="txtchild2NomineeName4" name="txtNomineeName4" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>

                                                                <div class="col-sm-2">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Date of Birth</dt>
                                                                        <dd>
                                                                            <input id="txtchild2NomineeDOB4" name="txtNomineeDOB4" value="" type="text" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;" /></dd>
                                                                    </dl>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <dl style="margin-bottom: 5px">
                                                                        <dt>Nominee Relationship with Proposer</dt>
                                                                        <dd>

                                                                            <select id="cbochildNominee2Relationship4" name="cboNomineeRelationship4" class="form-control" style="width: 100%; height: 25px; padding: 2px 7px; font-size: 13px;">
                                                                                <option value="Select" selected>Select</option>
                                                                                <option value="Father">Father</option>
                                                                                <option value="Mother">Mother</option>
                                                                                <option value="Sister">Sister</option>
                                                                                <option value="Son">Son</option>
                                                                                <option value="Daughter">Daughter</option>
                                                                                <option value="Spouse">Spouse</option>
                                                                                <option value="Brother">Brother</option>
                                                                                <option value="Father-In-Law">Father-In-Law</option>
                                                                                <option value="Mother-In-Law">Mother-In-Law</option>
                                                                            </select>
                                                                        </dd>
                                                                    </dl>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- END panel-->
                                    </div>
                                </div>

                            </div>
                        </div>



                        <h3 class="mv-lg pv-lg text-dark text-center">
                            <asp:Label ID="lblSelectedPremiumMessage" runat="server">Total Amount Payable - Rs</asp:Label>
                            <asp:Label ID="lblSelectedPremium" runat="server" Text="0.00"></asp:Label><span style="font-size: 16px;">(Inclusive of Taxes)</span>
                            <br />

                            <br />
                        </h3>


                        <center>
                
             
                    <div class="form-group" id="divDeclarofGoodHealth">
                        

                         <div class="row">
                        <div class="col-md-2"></div>
                         <div class="col-md-5 col-xs-8 text-xs-left text-md-right">
                            <label style="padding-left:20px;text-decoration:underline;"><a class="link-red" data-toggle="modal" data-target="#DecModalforhealth" href="#">Declaration Of Good Health & Terms and Conditions</a></label>
                                </div>
                                 <div class="col-md-1 col-xs-4">   <label>
                                    <input type="checkbox" id="termchkbox" value="yes" />
                                </label></div>
                        <div class="col-md-2"></div>
                        </div>
                       </div>

<%--                    <span id="lblMessageifNoSelected" style="color:red;display:none;font-weight:bold;font-size:15px;">Thank you for your interest. Please call us on 1800 266 4545 to buy offline.</span>--%>
               

                    
                                   


                <asp:Button ID="btnMakePayment" runat="server" CssClass="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px;position: relative;top: 7px"  ClientIDMode="Static" Text="Confirm & Pay" />
          <asp:Button ID="btnConfirmPayment" runat="server" Style="display: none;" CssClass="btn btn-success" ClientIDMode="Static" Text="SUBMIT" OnClick="btnConfirmPayment_click" />
                 <div class="otpPanel align-center text-center" id="otpPanel" style="display:none;">
                                  
                   
                     <asp:TextBox ID="txtTimer" ClientIDMode="Static" Style="display: none" ReadOnly="true" runat="server" Text="0" CssClass="timercountTxt"></asp:TextBox>                        
                                        <!-- TIMER STARTS HERE -->
                    <br />
                                            <div class="timer" id="divTimer">
                                                <span class="secspan">sec</span>
                                            </div>    
                                            <!-- TIMER ENDS HERE -->
                                            <p>Please Enter One Time Password (OTP) Sent On Your Mobile Number And Email</p>
                                            <div class="block-center text-center">
                                                <p>
                                                <asp:TextBox ID="txtOtpNumber" runat="server"  style="border-radius: 7px;border: 1px solid #dde6e9;padding:2px 7px;font-size: 13px;" Text="0" MaxLength="6" />
                                                    </p>
                                            </div><br />
                    
                                            <div class="otpButton align-center">
                                                <button type="button" id="btnMobileVerify" class="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px">Verify OTP</button>
                                                <asp:Button ID="btnMobileReSend" runat="server" CssClass="btn btn-info" style="background:#80631f !important;font-size:1.9rem;border-radius:8px" ClientIDMode="Static" Text="Resend OTP" />
                                            </div>
                                        </div>
                
                
            </center>


                    </section>

                    <section id="sectionThankYou" runat="server" style="display: none; min-height: 500px;">
                        <div class="content-wrapper">
                            <div class="container container-md">

                                <div class="abs-center" style="position: unset!important">
                                    <div class="text-center mv-lg">
                                        <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">
                                            <asp:Label ID="lblthnkyou" runat="server" Text="-"></asp:Label>
                                        </div>
                                        <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                            <asp:Label ID="lblsuccessmessage" runat="server" Text="-"></asp:Label>
                                            <br />
                                        </p>

                                        <hr />
                                        If you need to talk to us, we are all ears. Call us on 1800 266 4545, or shoot us an email at <a href="mailto:care@kotak.com" style="color: #003974;" target="_blank">care@kotak.com</a>.

                        <hr />

                                    </div>
                                </div>

                            </div>
                        </div>
                    </section>
                    <section id="sectionFailure" runat="server" style="display: none; min-height: 500px;">
                        <div class="content-wrapper">
                            <div class="container container-md">

                                <div class="abs-center" style="position: unset!important">
                                    <div class="text-center mv-lg">
                                        <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">
                                            Payment Failed!!!
                                        </div>
                                        <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                            <asp:Label ID="lblfailmessage" runat="server" Text="-"></asp:Label>
                                            <br />
                                        </p>

                                        <hr />
                                        For any further assistance, kindly call us on 1800 266 4545 or write to us at care@kotak.com
                        <hr />

                                    </div>
                                </div>

                            </div>
                        </div>
                    </section>
                    <section id="sectionError" runat="server" style="display: none; min-height: 500px;">
                        <div class="content-wrapper">
                            <div class="container container-md">

                                <div class="abs-center" style="position: unset!important">
                                    <div class="text-center mv-lg">
                                        <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">
                                            Sorry, Error Occurred
                                        </div>
                                        <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                            <asp:Label ID="lblerrmessage" runat="server" Text="-"></asp:Label>
                                            <br />
                                        </p>

                                        <hr />
                                        For any further assistance, kindly call us on 1800 266 4545 or write to us at care@kotak.com
                                      <hr />

                                    </div>
                                </div>

                            </div>
                        </div>
                    </section>


                    <section id="sectionRecordNotFound" runat="server" style="display: none; min-height: 500px; padding: 50px">
                        <div class="content-wrapper">
                            <div class="container container-md">

                                <div class="abs-center" style="position: unset!important">
                                    <div class="text-center mv-lg">
                                        <div class="text-bold text-lg mb-lg" style="font-size: 5rem; font-family: Source Sans Pro, sans-serif; color: #ab9d7e; padding-bottom: 10px">
                                            Sorry, nothing to display
                                        </div>
                                        <p class="lead m0" style="font-size: 1.8rem; font-family: Source Sans Pro, sans-serif">
                                            Either you have already submitted the request or your record not present in the system
                            <br />
                                        </p>

                                        <hr />
                                        For any further assistance, kindly call us on 1 800 266 4545 or write to us at care@kotak.com
                        <hr />

                                    </div>
                                </div>

                            </div>
                        </div>
                    </section>
                    <br />
                    <!-- Page footer-->
                    <footer style="background-color: white; z-index: 113; margin-bottom: -44px;" id="footerend">
                        <span style="font-size: 14px; text-align: center; padding: 10px; float: left; text-align: left;"><b>Kotak Group Health Assure UIN: KOTHLGP22168V012122.</b> The advertisement contains only an indication of cover offered. For more details on risk factors, terms, conditions and exclusions, please read the sales brochure carefully before concluding a sale. Trade logo displayed above belongs to Kotak Mahindra Bank Ltd. and is used by Kotak General Insurance Company Limited under license. Kotak Mahindra General Insurance Company Ltd. CIN: U66000MH2014PLC260291. IRDAI Reg. No.152. Registered Office Address: 27 BKC, C 27, G Block, Bandra Kurla Complex, Bandra East, Mumbai – 400051. Maharashtra. India. 
                        </span>


                    </footer>




                </div>


            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnConfirmPayment" />
            </Triggers>
        </asp:UpdatePanel>


        <!-- BOOTSTRAP-->


    </form>







</body>
</html>


