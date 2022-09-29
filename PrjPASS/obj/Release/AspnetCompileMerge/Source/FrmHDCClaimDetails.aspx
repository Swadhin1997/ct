<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmHDCClaimDetails.aspx.cs" Inherits="PrjPASS.FrmHDCClaimDetails" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<%@ Register Assembly="obout_SuperForm" Namespace="Obout.SuperForm" TagPrefix="obout" %>

<%@ Register Assembly="obout_Window_NET" Namespace="OboutInc.Window" TagPrefix="owd" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>




<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MstCntFormContent">



    <script type="text/javascript">
        window.onload = function () {
            gvSubDetails.convertToExcel(
                ['ReadOnly', 'ReadOnly', 'TextBox', 'ComboBox', 'Action'],
                '<%=Grid1ExcelData.ClientID %>',
                '<%=Grid1ExcelDeletedIds.ClientID %>'
                );
        }
        $(function () {
            SetAutoComplete();
            ApplyDatePicker();
        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetAutoComplete);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ApplyDatePicker);
        });




        function ApplyDatePicker() {

            $("[id$=txtDateOfDischarge]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                maxDate:0 ,
                yearRange: "2000:" + new Date().getFullYear().toString()
            });

            $("#datepickerDischargeDate").click(function () {
                $("[id$=txtDateOfDischarge]").datepicker("show");
            });

          
            $("[id$=txtDateofDeath]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                maxDate: 0,
                yearRange: "2000:" + new Date().getFullYear().toString()
            });

            $("#datepickerDateofDeath").click(function () {
                $("[id$=txtDateofDeath]").datepicker("show");
            });


            $("[id$=txtInvestigationDate]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                maxDate: 0,
                yearRange: "2000:" + new Date().getFullYear().toString()
            });
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ApplyDatePicker);
        });




        function ApplyDatePicker() {

            $("[id$=txtDateOfDischarge]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                yearRange: "2000:" + new Date().getFullYear().toString()
            });

            $("#datepickerDischargeDate").click(function () {
                $("[id$=txtDateOfDischarge]").datepicker("show");
            });

          
            $("[id$=txtDateofDeath]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                yearRange: "2000:" + new Date().getFullYear().toString()
            });

            $("#datepickerDateofDeath").click(function () {
                $("[id$=txtDateofDeath]").datepicker("show");
            });


            $("[id$=txtInvestigationDate]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                yearRange: "2000:" + new Date().getFullYear().toString()
            });

            $("#datepickerInvestigation").click(function () {
                $("[id$=txtInvestigationDate]").datepicker("show");
            });
            

            $("#datepickerSubmitDate").click(function () {
                $("[id$=txtFinalSubmitDate]").datepicker("show");
            });
            

            $("[id$=txtFinalSubmitDate]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                maxDate: 0,
                yearRange: "2000:" + new Date().getFullYear().toString()
            });

        }


        function clearDateofDischarge() {
            document.getElementById("<%=txtDateOfDischarge.ClientID%>").value = '';
        }


        function clearDateofDeath() {
            document.getElementById("<%=txtDateofDeath.ClientID%>").value = '';
        }

        function clearDateofInvestigation() {
            document.getElementById("<%=txtInvestigationDate.ClientID%>").value = '';
        }


        function clearSubmissionDate() {
            document.getElementById("<%=txtFinalSubmitDate.ClientID%>").value = '';
        }
        

        

        

        function SetAutoComplete() {
            $("[id$=txtHospitalPinCode]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/FrmHDCClaimDetails.aspx/GetPincode") %>',
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                var strItems = item.split("~");
                                return {
                                    label: item,
                                    val: strItems[1]
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
               select: function (event, ui) {
                   var strItems2 = ui.item.label.split("~");
                   $("[id$=txtHospitalPinCode]").val(strItems2[0]);
                   $("[id$=hdnPinCode]").val(strItems2[0]);
                   $("[id$=hdnPinCodeLocality]").val(ui.item.val);
                   $("[id*=btnGetPincodeDetails]").click();
               },
               minLength: 3,
               autoFocus: true
           });
        }


    </script>
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>HDC Claims Details</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                <asp:HiddenField ID="hfPolicyStartDate" Value="" runat="server"/>
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="460px" BorderColor="#3399ff" BorderWidth="2">

                            <div id="d58" runat="server" style="position: absolute; top: 1%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblCertificateNumber" runat="server" Text="Claim Number" Width="100%"></asp:Label>
                            </div>
                            <div id="d59" runat="server" style="position: absolute; top: 1%; left: 18%; width: 25%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCertificateNumber" Style="text-transform: uppercase" Width="50%" runat="server" MaxLength="30"></obout:OboutTextBox>
                            </div>
                            <div id="d60" runat="server" style="position: absolute; top: 1%; left: 33%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnSearchClaim" Width="50%" runat="server" Text="Search" OnClick="btnSearchClaim_Click"></obout:OboutButton>
                            </div>

                            <div id="d1" runat="server" style="position: absolute; top: 1%; left: 55%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblCustomerName" runat="server" Text="Claim Number<font style='color:red'>*</font>" Width="100%"></asp:Label>
                            </div>
                            <div id="d2" runat="server" style="position: absolute; top: 1%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtClaimNumber" Style="text-transform: uppercase" Width="100%" runat="server" ReadOnly="true" OnTextChanged="txtCustomerName_TextChanged"></obout:OboutTextBox>
                            </div>

                            <div id="d3" runat="server" style="position: absolute; top: 7%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblCustomerType" runat="server" Text="Certificate Number<font style='color:red'>*</font>" Width="173%"></asp:Label>
                            </div>
                            <div id="d4" runat="server" style="position: absolute; top: 7%; left: 18%; width: 30%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCertNumber" Style="text-transform: uppercase" Width="67%" runat="server" MaxLength="30" ReadOnly="true"></obout:OboutTextBox>
                            </div>
                            <div id="d5" runat="server" style="position: absolute; top: 7%; left: 55%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblDateofAdmission" runat="server" Text="Date of Admission<font style='color:red'>*</font>" Width="100%"></asp:Label>
                            </div>
                            <div id="d6" runat="server" style="position: absolute; top: 7%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtDateOfAdmission" runat="server" class="input" Width="100%" />
                            </div>
                            <div id="d61" runat="server" style="position: absolute; top: 7%; left: 91%; width: 20%; margin-top: 10px; margin-left: 10px">
                               
                            </div>


                            <div id="d7" runat="server" style="position: absolute; top: 13%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblDateOfDischarge" runat="server" Text="Date Of Discharge<font style='color:red'>*</font>" Width="100%"></asp:Label>
                            </div>
                            <div id="d8" runat="server" style="position: absolute; top: 13%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtDateOfDischarge" runat="server" Style="text-transform: uppercase" Width="100%"> </obout:OboutTextBox>
                            </div>
                            <div id="d64" runat="server" style="position: absolute; top: 13%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                               <%-- <obout:Calendar ID="clnDateofDischarge" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtDateOfDischarge" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>--%>
                                <img src="images/calendar.png" alt="" id="datepickerDischargeDate" />
                                <a href="javascript:clearDateofDischarge();">Clear</a> 
                            </div>

                            <div id="d9" runat="server" style="position: absolute; top: 13%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblDateofDeath" runat="server" Text="Date of Death" Width="100%"></asp:Label>
                            </div>
                            <div id="d10" runat="server" style="position: absolute; top: 13%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtDateofDeath" Style="text-transform: uppercase" Width="100%" runat="server"></obout:OboutTextBox>
                            </div>
                            <div id="d62" runat="server" style="position: absolute; top: 13%; left: 91%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <%--<obout:Calendar ID="clnDateofDeath" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtDateofDeath" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>--%>
                                <img src="images/calendar.png" alt="" id="datepickerDateofDeath" />
                                <a href="javascript:clearDateofDeath();">Clear</a> 
                            </div>


                            <div id="d11" runat="server" style="position: absolute; top: 19%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblICUDays" runat="server" Text="ICU Days<font style='color:red'>*</font>" Width="100%"></asp:Label>
                            </div>
                            <div id="d12" runat="server" style="position: absolute; top: 19%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtICUDays" Style="text-transform: uppercase" Width="100%" runat="server" text="" MaxLength="3" ></obout:OboutTextBox>
                            </div>
                            <div id="d13" runat="server" style="position: absolute; top: 19%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblNonICUDays" runat="server" Text="Non ICU days<font style='color:red'>*</font>" Width="100%"></asp:Label>
                            </div>
                            <div id="d14" runat="server" style="position: absolute; top: 19%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtNonICUDays" Style="text-transform: uppercase" Width="100%" runat="server" text="" MaxLength="3"></obout:OboutTextBox>
                            </div>

                            <div id="d15" runat="server" style="position: absolute; top: 25%; left: 55%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblHospitalAddress" runat="server" Text="Hospital Address" Width="100%"></asp:Label>
                            </div>
                            <div id="d16" runat="server" style="position: absolute; top: 25%; left: 71%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtHospitalAddress" Style="text-transform: uppercase" Width="58%" runat="server" MaxLength="500"></obout:OboutTextBox>
                            </div>
                            <div id="d17" runat="server" style="position: absolute; top: 25%; left: 1%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblHospitalName" runat="server" Text="Hospital Name<font style='color:red'>*</font>" Width="100%"></asp:Label>
                            </div>
                            <div id="d18" runat="server" style="position: absolute; top: 25%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtHospitalName" Style="text-transform: uppercase;" Width="100%" runat="server" MaxLength="100"></obout:OboutTextBox>
                            </div>

                            <div id="d19" runat="server" style="position: absolute; top: 31%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblHospitalPinCode" runat="server" Text="Hospital Pin Code" Width="100%"></asp:Label>
                            </div>
                            <div id="d20" runat="server" style="position: absolute; top: 31%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtHospitalPinCode" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="6"></obout:OboutTextBox>
                                <asp:Label Text="" ID="lblPincodeLocality" runat="server" Visible="false"></asp:Label>
                                <asp:Button ID="btnGetPincodeDetails" runat="server" OnClick="btnGetPincodeDetails_Click" />
                            </div>
                            <div id="d21" runat="server" style="position: absolute; top: 31%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                            </div>

                            <div id="d22" runat="server" style="position: absolute; top: 31%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblHospitalCity" runat="server" Text="Hospital City" Width="100%"></asp:Label>
                            </div>
                            <div id="d23" runat="server" style="position: absolute; top: 31%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtHospitalCity" Style="text-transform: uppercase" Width="100%" runat="server" ></obout:OboutTextBox>
                            </div>

                            <div id="d24" runat="server" style="position: absolute; top: 37%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblHospitalState" runat="server" Text="Hospital State" Width="100%"></asp:Label>
                            </div>
                            <div id="d25" runat="server" style="position: absolute; top: 37%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtHospitalState" Style="text-transform: uppercase" Width="100%" runat="server" ></obout:OboutTextBox>
                            </div>


                            <div id="d26" runat="server" style="position: absolute; top: 37%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblClaimSettlingOffice" runat="server" Text="Claim Settling Office<font style='color:red'>*</font>" Width="100%"></asp:Label>
                            </div>
                            <div id="d27" runat="server" style="position: absolute; top: 37%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtClaimSettlingOffice" Style="text-transform: uppercase" Width="100%" runat="server" ReadOnly="true" Text="Head Office"></obout:OboutTextBox>
                            </div>

                            <div id="d28" runat="server" style="position: absolute; top: 43%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblInvestigatorName" runat="server" Text="Investigator Name" Width="100%"></asp:Label>
                            </div>
                            <div id="d29" runat="server" style="position: absolute; top: 43%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtInvestigatorName" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="50"></obout:OboutTextBox>
                            </div>
                            <div id="d30" runat="server" style="position: absolute; top: 43%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                            </div>

                            <div id="d31" runat="server" style="position: absolute; top: 43%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblInvestigatorAddress" runat="server" Text="Investigator Address" Width="100%"></asp:Label>
                            </div>
                            <div id="d32" runat="server" style="position: absolute; top: 43%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtInvestigatorAddress" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength ="500"></obout:OboutTextBox>
                            </div>


                            <div id="d33" runat="server" style="position: absolute; top: 50%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblInvestigationDate" runat="server" Text="Investigation Date" Width="100%"></asp:Label>
                            </div>
                            <div id="d34" runat="server" style="position: absolute; top: 50%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtInvestigationDate" Style="text-transform: uppercase" Width="100%" runat="server"></obout:OboutTextBox>
                            </div>
                            <div id="d35" runat="server" style="position: absolute; top: 50%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <%--<obout:Calendar ID="clnInvestigationDate" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtInvestigationDate" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>--%>
                                <img src="images/calendar.png" alt="" id="datepickerInvestigation" />
                                 <a href="javascript:clearDateofInvestigation();">Clear</a> 
                            </div>
                            <div id="d36" runat="server" style="position: absolute; top: 50%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblFinalSubmitDate" runat="server" Text="Investigation Final</br> Report Submission Date" Width="100%"></asp:Label>
                            </div>
                            <div id="d37" runat="server" style="position: absolute; top: 50%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtFinalSubmitDate" Style="text-transform: uppercase" Width="100%" runat="server"></obout:OboutTextBox>
                            </div>
                            <div id="d65" runat="server" style="position: absolute; top: 50%; left: 91%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <%--<obout:Calendar ID="clnFinalSubmit" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtFinalSubmitDate" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>--%>
                                <img src="images/calendar.png" alt="" id="datepickerSubmitDate" />
                                <a href="javascript:clearSubmissionDate()">Clear</a> 
                            </div>


                            <div id="d38" runat="server" style="position: absolute; top: 58%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblBenefitType" runat="server" Text="Benefit Type<font style='color:red'>*</font>" Width="100%"></asp:Label>
                            </div>
                            <div id="d39" runat="server" style="position: absolute; top: 56%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutDropDownList ID="DrpBenefitType" runat="server">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                    <asp:ListItem Value="Hospital Cash"></asp:ListItem>
                                    <asp:ListItem Value="Surgical Cash">Surgical Cash</asp:ListItem>
                                    <asp:ListItem Value="Others">Others</asp:ListItem>
                                </obout:OboutDropDownList>
                            </div>
                            <div id="d40" runat="server" style="position: absolute; top: 58%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                            </div>
                            <div id="d41" runat="server" style="position: absolute; top: 58%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblExpenseAmount" runat="server" Text="Expense Amount<font style='color:red'>*</font>" Width="100%"></asp:Label>
                            </div>
                            <div id="d42" runat="server" style="position: absolute; top: 58%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtExpenseAmt" Style="text-transform: uppercase" Width="100%" runat="server" Text="" MaxLength="10"></obout:OboutTextBox>
                            </div>


                            <div id="d43" runat="server" style="position: absolute; top: 64%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblIDCCh1" runat="server" Text="ICD Chapter - Level 1<font style='color:red'>*</font>" Width="100%"></asp:Label>
                            </div>
                            <div id="d44" runat="server" style="position: absolute; top: 64%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutDropDownList ID="drpIDCch1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpIDCch1_SelectedIndexChanged" Width="100%">
                                </obout:OboutDropDownList>
                            </div>

                            <div id="d45" runat="server" style="position: absolute; top: 64%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                            </div>
                            <div id="d46" runat="server" style="position: absolute; top: 64%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblIDCCh2" runat="server" Text="ICD Chapter - Level 2<font style='color:red'>*</font>" Width="100%"></asp:Label>

                            </div>
                            <div id="d47" runat="server" style="position: absolute; top: 64%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutDropDownList ID="drpIDCch2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpIDCch2_SelectedIndexChanged" Width="100%">
                                </obout:OboutDropDownList>
                            </div>


                            <div id="d48" runat="server" style="position: absolute; top: 70%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblIDCCh3" runat="server" Text="ICD Chapter - Level 3" Width="100%"></asp:Label>
                            </div>
                            <div id="d49" runat="server" style="position: absolute; top: 70%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutDropDownList ID="drpIDCch3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpIDCch3_SelectedIndexChanged" Width="100%">
                                </obout:OboutDropDownList>
                            </div>
                            <div id="d50" runat="server" style="position: absolute; top: 70%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                            </div>
                            <div id="d51" runat="server" style="position: absolute; top: 70%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblIDCCh4" runat="server" Text="ICD Chapter - Level 4" Width="100%"></asp:Label>
                            </div>
                            <div id="d52" runat="server" style="position: absolute; top: 70%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutDropDownList ID="drpIDCch4" runat="server" Width="100%">
                                </obout:OboutDropDownList>
                            </div>


                            <div id="d53" runat="server" style="position: absolute; top: 76%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblDiseaseDescription" runat="server" Text="Disease Description" Width="100%"></asp:Label>
                            </div>
                            <div id="d54" runat="server" style="position: absolute; top: 76%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtDiseaseDescription" Style="text-transform: uppercase" Width="100%" runat="server" TextMode="MultiLine" Rows="5" Columns="5" MaxLength="4000"> </obout:OboutTextBox>
                            </div>
                            <div id="d55" runat="server" style="position: absolute; top: 76%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                            </div>
                            <div id="d56" runat="server" style="position: absolute; top: 76%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblRemark" runat="server" Text="Remarks if any" Width="100%"></asp:Label>
                            </div>
                            <div id="d57" runat="server" style="position: absolute; top: 76%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtClaimDetailRemark" Style="text-transform: uppercase" Width="100%" runat="server" TextMode="MultiLine" Rows="5" Columns="5" MaxLength="4000"> </obout:OboutTextBox>
                            </div>



                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left: 25%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnSave" runat="server" Text="Save" Width="100%" OnClick="btnSave_Click" />
                            </div>
                            <div style="position: absolute; top: 40%; left: 45%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click1"></obout:OboutButton>
                            </div>
                            <div style="position: absolute; top: 40%; left: 65%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnReset" Width="100%" runat="server" Text="Reset" OnClick="btnReset_Click"></obout:OboutButton>
                            </div>

                            <div style="position: absolute; top: 58%; left: 35%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label3" runat="server" />
                            </div>
                        </asp:Panel>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnPinCodeLocality" />
    <asp:HiddenField runat="server" ID="hdnPinCode" />
    <asp:HiddenField runat="server" ID="Grid1ExcelDeletedIds" />
    <asp:HiddenField runat="server" ID="Grid1ExcelData" />
    <script src="Grid/resources/custom_scripts/excel-style/excel-style.js"></script>
</asp:Content>

