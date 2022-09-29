<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmHDCClaimEdit.aspx.cs" Inherits="PrjPASS.FrmHDCClaimEdit" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<%@ Register Assembly="obout_SuperForm" Namespace="Obout.SuperForm" TagPrefix="obout" %>

<%@ Register Assembly="obout_Window_NET" Namespace="OboutInc.Window" TagPrefix="owd" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MstCntFormContent">
    <style type="text/css">
        .wide {
            width: 100%;
            min-width: 100%;
        }
    </style>



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
        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetAutoComplete);
        });


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


            $("#datepickerDischargeDate").click(function () {
                $("[id$=txtDateofDischarge]").datepicker("show");
            });

            $("[id$=txtDateofDischarge]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                maxDate: '0',
                yearRange: "2000:" + new Date().getFullYear().toString()
            });

            $("#datepickerDateofDeath").click(function () {
                $("[id$=txtDateofDeath]").datepicker("show");
            });

            $("[id$=txtDateofDeath]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                maxDate: '0',
                yearRange: "2000:" + new Date().getFullYear().toString()
            });

            $("#datepickerInvestigationDate").click(function () {
                $("[id$=txtInvestigationDate]").datepicker("show");
            });

            $("[id$=txtInvestigationDate]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                maxDate: '0',
                yearRange: "2000:" + new Date().getFullYear().toString()
            });

            $("#datepickerReportSubmitDate").click(function () {
                $("[id$=txtFinalReportSubmitDate]").datepicker("show");
            });

            $("[id$=txtFinalReportSubmitDate]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                minDate: '01/01/2000',
                maxDate: '0',
                yearRange: "2000:" + new Date().getFullYear().toString()
            });
        }


        function ClearDishargeDate() {
            document.getElementById("<%=txtDateofDischarge.ClientID%>").value = '';
        }

        function clearDateofDeath() {
            document.getElementById("<%=txtDateofDeath.ClientID%>").value = '';
        }

        function ClearInvestigationDate(){
            document.getElementById("<%=txtInvestigationDate.ClientID%>").value = '';
        }


        function ClearReportSubmitDate() {
            document.getElementById("<%=txtFinalReportSubmitDate.ClientID%>").value = '';
        }



    </script>

    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>HDC Claims Edit Details</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                    
                        <div class="frm-row">
                            <div class="section colm colm12">
                                <asp:Panel ID="Panel1" runat="server" Height="550px" BorderColor="#3399ff" BorderWidth="2">

                                    <div style="position: absolute; top: 12%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="lblClaimNumber" runat="server" Text="Claim Number" Width="100%"></asp:Label>
                                    </div>
                                    <div style="position: absolute; top: 12%; left: 18%; width: 25%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtClaimNumberClaim" Style="text-transform: uppercase" Width="80%" runat="server" MaxLength="30"></obout:OboutTextBox>
                                    </div>

                                    <div style="position: absolute; top: 12%; left: 45%; width: 20%; margin-top: 10px; margin-left: 10px">

                                        <obout:OboutButton ID="btnSearchClaimNumber" Width="25%" runat="server" Text="Search" OnClick="btnSearchClaimNumber_Click"></obout:OboutButton>

                                    </div>

                                    <div id="dv1" runat="server" style="position: absolute; top: 12%; left: 55%; width: 28%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label4" runat="server" Text="Customer Name" Width="100%"></asp:Label>
                                    </div>
                                    <div id="dv2" runat="server" style="position: absolute; top: 12%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtCustomerNameclaim" Style="text-transform: uppercase" Width="100%" runat="server" ReadOnly="true"></obout:OboutTextBox>
                                    </div>

                                    <div id="dv3" runat="server" style="position: absolute; top: 17%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label5" runat="server" Text="Certificate No." Width="100%"></asp:Label>
                                    </div>
                                    <div id="dv4" runat="server" style="position: absolute; top: 17%; left: 18%; width: 30%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtCertificateNumber" Style="text-transform: uppercase" Width="67%" runat="server" MaxLength="30" ReadOnly="true"></obout:OboutTextBox>
                                    </div>
                                    <div id="dv5" runat="server" style="position: absolute; top: 17%; left: 55%; width: 28%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label6" runat="server" Text="Customer Mobile Number<span style='color:Red'>*</span>" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv6" style="position: absolute; top: 17%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtCustomerMobileClaim" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="10"></obout:OboutTextBox>
                                    </div>
                                    <div runat="server" id="dv7" style="position: absolute; top: 22%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label7" runat="server" Text="Date of Discharge<span style='color:Red'>*</span>" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv8" style="position: absolute; top: 22%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtDateofDischarge" Style="text-transform: uppercase" Width="100%" runat="server"></obout:OboutTextBox>
                                    </div>
                                    <div runat="server" id="dv9" style="position: absolute; top: 22%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                                       <img src="images/calendar.png" alt="" id="datepickerDischargeDate" />
                                        <%-- <obout:Calendar ID="clnDateOfDischarge" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtDateofDischarge" DatePickerImagePath="Calendar/styles/date_picker1.gif" AllowDeselect="true"></obout:Calendar>--%>
                                        <%-- <asp:ImageButton Src="images/DeleteBtn.gif" Id="imgClear" runat="server" OnClick="imgClear_Click" />--%>
                                        <a href="javascript:ClearDishargeDate()" runat="server" onclick="ClearDate" id="hlClearDischargeDate">Clear</a>
                                    </div>


                                    <div runat="server" id="dv10" style="position: absolute; top: 22%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    </div>


                                    <div runat="server" id="dv11" style="position: absolute; top: 22%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label8" runat="server" Text="ICD Chapter Level 1<span style='color:Red'>*</span>" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv12" style="position: absolute; top: 22%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutDropDownList ID="drpIDCch1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpIDCch1_SelectedIndexChanged" Width="100%">
                                        </obout:OboutDropDownList>
                                    </div>
                                    <div runat="server" id="dv13" style="position: absolute; top: 27%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label9" runat="server" Text="ICD Chapter Level 2<span style='color:Red'>*</span>" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv14" style="position: absolute; top: 27%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutDropDownList ID="drpIDCch2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpIDCch2_SelectedIndexChanged" Width="100%">
                                        </obout:OboutDropDownList>
                                    </div>
                                    <div runat="server" id="dv15" style="position: absolute; top: 27%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label10" runat="server" Text="ICD Chapter Level 3" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv16" style="position: absolute; top: 27%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutDropDownList ID="drpIDCch3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpIDCch3_SelectedIndexChanged" Width="100%">
                                        </obout:OboutDropDownList>
                                    </div>


                                    <div runat="server" id="dv17" style="position: absolute; top: 32%; left: 55%; width: 12%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label11" runat="server" Text="ICD Chapter Level 4" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv18" style="position: absolute; top: 32%; left: 71%; width: 35%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutDropDownList ID="drpIDCch4" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpIDCch4_SelectedIndexChanged" Width="57%">
                                        </obout:OboutDropDownList>
                                    </div>

                                    <div runat="server" id="dv19" style="position: absolute; top: 32%; left: 1%; width: 28%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="lblLineOfBusiness" runat="server" Text="Line of Business (LOB)" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv20" style="position: absolute; top: 32%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtLineOfBusiness" Style="text-transform: uppercase" Width="100%" runat="server" Text="Health" ReadOnly="true"></obout:OboutTextBox>
                                    </div>

                                    <div runat="server" id="dv21" style="position: absolute; top: 37%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label13" runat="server" Text="Claimed Amount<span style='color:Red'>*</span>" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv22" style="position: absolute; top: 37%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtClaimedAmount" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="10"></obout:OboutTextBox>
                                    </div>
                                    <div runat="server" id="dv23" style="position: absolute; top: 37%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    </div>

                                    <div runat="server" id="dv24" style="position: absolute; top: 37%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label14" runat="server" Text="Expense Amount<span style='color:Red'>*</span>" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv25" style="position: absolute; top: 37%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtExpenseAmount" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="10"></obout:OboutTextBox>
                                    </div>

                                    <div runat="server" id="dv26" style="position: absolute; top: 42%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label15" runat="server" Text="Remarks" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv27" style="position: absolute; top: 42%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtRemarkClaim" Style="text-transform: uppercase" Width="100%" runat="server" TextMode="MultiLine" Rows="5" AutoPostBack="false" Columns="5" CssClass="wide"></obout:OboutTextBox>
                                    </div>



                                    <div runat="server" id="dv28" style="position: absolute; top: 42%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    </div>

                                    <div runat="server" id="dv29" style="position: absolute; top: 42%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label12" runat="server" Text="Disease Description" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv30" style="position: absolute; top: 42%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtDisDesc" Style="text-transform: uppercase" Width="100%" runat="server" TextMode="MultiLine" Rows="5" AutoPostBack="false" Columns="5" CssClass="wide"></obout:OboutTextBox>
                                    </div>




                                    <div runat="server" id="dv31" style="position: absolute; top: 57%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label3" runat="server" Text="Date of Death" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv32" style="position: absolute; top: 57%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtDateofDeath" Style="text-transform: uppercase" Width="100%" runat="server"></obout:OboutTextBox>
                                    </div>

                                    <div runat="server" id="dv33" style="position: absolute; top: 57%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <%--<obout:Calendar ID="Calendar1" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtDateofDeath" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>--%>
                                        <img src="images/calendar.png" alt="" id="datepickerDateofDeath" />
                                        <a href="javascript:clearDateofDeath()">Clear</a>
                                    </div>

                                    <div runat="server" id="dv34" style="position: absolute; top: 57%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    </div>

                                    <div runat="server" id="dv35" style="position: absolute; top: 57%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label16" runat="server" Text="ICU Days<span style='color:Red'>*</span>" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv36" style="position: absolute; top: 57%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtICUDays" Style="text-transform: uppercase" Width="100%" runat="server"></obout:OboutTextBox>
                                    </div>


                                    <div runat="server" id="dv37" style="position: absolute; top: 62%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label17" runat="server" Text="Non ICU days<span style='color:Red'>*</span>" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv38" style="position: absolute; top: 62%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtNonICUDays" Style="text-transform: uppercase" Width="100%" runat="server"></obout:OboutTextBox>
                                    </div>



                                    <div runat="server" id="dv39" style="position: absolute; top: 62%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    </div>

                                    <div runat="server" id="dv40" style="position: absolute; top: 62%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label18" runat="server" Text="Hospital Name<span style='color:Red'>*</span>" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv41" style="position: absolute; top: 62%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtHospitalName" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="100"></obout:OboutTextBox>
                                    </div>



                                    <div runat="server" id="dv42" style="position: absolute; top: 67%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label19" runat="server" Text="Hospital Address" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv43" style="position: absolute; top: 67%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtHospitalAddress" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="500"></obout:OboutTextBox>
                                    </div>



                                    <div runat="server" id="dv44" style="position: absolute; top: 67%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    </div>

                                    <div runat="server" id="dv45" style="position: absolute; top: 67%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label20" runat="server" Text="Hospital Pin Code" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv46" style="position: absolute; top: 67%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtHospitalPinCode" Style="text-transform: uppercase" Width="100%" runat="server"></obout:OboutTextBox>
                                        <asp:Label ID="lblPincodeLocality" visible="false" runat="server"></asp:Label>
                                        <asp:Button ID="btnGetPincodeDetails" runat="server" OnClick="btnGetPincodeDetails_Click" />
                                    </div>




                                    <div runat="server" id="dv47" style="position: absolute; top: 72%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label2" runat="server" Text="Hospital City" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv48" style="position: absolute; top: 72%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtHospitalCity" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="15"></obout:OboutTextBox>
                                    </div>

                                    <div runat="server" id="dv49" style="position: absolute; top: 72%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    </div>

                                    <div runat="server" id="dv50" style="position: absolute; top: 72%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label21" runat="server" Text="Hospital State" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv51" style="position: absolute; top: 72%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtHospitalState" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="30"></obout:OboutTextBox>
                                    </div>






                                    <div runat="server" id="dv52" style="position: absolute; top: 77%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label22" runat="server" Text="Investigator Name" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv53" style="position: absolute; top: 77%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtInvestigatorName" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="50"></obout:OboutTextBox>
                                    </div>

                                    <div runat="server" id="dv54" style="position: absolute; top: 77%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                                    </div>

                                    <div runat="server" id="dv55" style="position: absolute; top: 77%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label23" runat="server" Text="Investigator Address" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv56" style="position: absolute; top: 77%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtInvestigatorAddress" Style="text-transform: uppercase" Width="100%" runat="server" MaxLength="100"></obout:OboutTextBox>
                                    </div>







                                    <div runat="server" id="dv57" style="position: absolute; top: 82%; left: 1%; width: 12%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label24" runat="server" Text="Investigation Date" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv58" style="position: absolute; top: 82%; left: 18%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtInvestigationDate" Style="text-transform: uppercase" Width="100%" runat="server"></obout:OboutTextBox>
                                    </div>

                                    <div runat="server" id="dv59" style="position: absolute; top: 82%; left: 38%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <%--<obout:Calendar ID="Calendar2" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtInvestigationDate" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>--%>
                                        <img src="images/calendar.png" alt="" id="datepickerInvestigationDate" />
                                        <a href="javascript:ClearInvestigationDate()">Clear</a>
                                    </div>

                                    <div runat="server" id="dv60" style="position: absolute; top: 82%; left: 55%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="Label25" runat="server" Text="Investigation Final <br>Report Submission Date" Width="100%"></asp:Label>
                                    </div>
                                    <div runat="server" id="dv61" style="position: absolute; top: 82%; left: 71%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutTextBox ID="txtFinalReportSubmitDate" Style="text-transform: uppercase" Width="100%" runat="server"></obout:OboutTextBox>
                                    </div>
                                    <div runat="server" id="dv62" style="position: absolute; top: 82%; left: 91%; width: 20%; margin-top: 10px; margin-left: 10px">
                                        <%--<obout:Calendar ID="Calendar3" StyleFolder="Calendar/styles/orbitz" Columns="1" runat="server" DateFormat="dd/MM/yyyy" DatePickerMode="true" TextBoxId="txtFinalReportSubmitDate" DatePickerImagePath="Calendar/styles/date_picker1.gif"></obout:Calendar>--%>
                                        <img src="images/calendar.png" alt="" id="datepickerReportSubmitDate" />
                                        <a href="javascript:ClearReportSubmitDate()">Clear</a>
                                    </div>





                                    <div runat="server" class="section colm colm12" style="position: absolute; top: 86%">
                                        <asp:Panel ID="Panel3" runat="server" Height="60px">
                                            <div style="position: absolute; top: 40%; left: 35%; width: 10%; margin-top: 10px; margin-left: 10px">
                                                <obout:OboutButton ID="btnUpdateClaimDetails" runat="server" Text="Update" Width="100%" OnClick="btnUpdateClaimDetails_Click" />
                                            </div>
                                            <div style="position: absolute; top: 40%; left: 55%; width: 10%; margin-top: 10px; margin-left: 10px">
                                                <obout:OboutButton ID="OboutButton2" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click"></obout:OboutButton>
                                            </div>
                                            <div style="position: absolute; top: 58%; left: 35%; width: 8%; margin-top: 10px; margin-left: 10px">
                                                <asp:Label ID="Label1" runat="server" />
                                            </div>
                                        </asp:Panel>
                                    </div>

                                </asp:Panel>
                            </div>
                        </div>
            </div>
        </div>



    </div>

    <asp:HiddenField runat="server" ID="hdnPinCodeLocality" />
    <asp:HiddenField runat="server" ID="hfPolicyStartDate" />
    <asp:HiddenField runat="server" ID="hdnPinCode" />
    <asp:HiddenField runat="server" ID="Grid1ExcelDeletedIds" />
    <asp:HiddenField runat="server" ID="Grid1ExcelData" />
    <asp:HiddenField runat="server" ID="hftxtRemark" />
    <asp:HiddenField runat="server" ID="hftxtDiseasDesc" />
    <script src="Grid/resources/custom_scripts/excel-style/excel-style.js"></script>
</asp:Content>

