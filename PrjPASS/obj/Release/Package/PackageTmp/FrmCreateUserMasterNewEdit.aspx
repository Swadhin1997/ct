<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmCreateUserMasterNewEdit.aspx.cs" Inherits="ProjectPASS.FrmCreateUserMasterNewEdit" %>

<%@ Register Assembly="obout_ComboBox" Namespace="Obout.ComboBox" TagPrefix="obout" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        #table1 td {
            border: 1px solid rgb(199, 198, 198);
            padding: 2px;
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
            font-size: 14px;
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


    <script type="text/javascript">
        $(function () {
            SetAutoComplete();
        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetAutoComplete);
            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ChangeDORLabel);
           
        });

        function SetAutoComplete() {

            $("[id$=txtIntermediaryCode]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/FrmCreateUserMasterNewEdit.aspx/GetIntermediaryCode") %>',
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                var strItems = item.split("~");
                                return {
                                    label: item,
                                    val: strItems[0]
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
                select: function (e, i) {
                    var strItems2 = i.item.label.split("~");
                    $("[id$=txtIntermediaryCode]").val(strItems2[0]);
                    $("[id$=hfIntermediaryCode]").val(i.item.val);
                    $("[id*=btnGetIntermediaryCode]").click();
                },
                minLength: 3,
                autoFocus: true
            });




        }
    </script>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MstCntFormContent">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div class="smart-wrap">
                <div class="smart-forms smart-container wrap-4">
                    <div class="form-header header-blue">
                        <h4><i class="fa fa-sign-in"></i>PASS - User Management(Create / Edit User)</h4>
                        <div id="divLogo" class="LogoCSS">
                            <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                        </div>
                    </div>
                    <div class="form-body theme-blue">
                        <div class="frm-row">
                            <div class="section colm colm12">
                                <asp:Panel ID="Panel1" runat="server" BorderColor="DarkGray" BorderWidth="1">

                                    <table id="table1" style="width: 100%;" cellspacing="0" cellpadding="2">
                                        <tr>
                                            <td class="tdbkg">Login Id
                                            </td>
                                            <td>
                                                <obout:OboutTextBox ID="txtUserId" Style="text-transform: uppercase" runat="server" MaxLength="10"></obout:OboutTextBox>
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator2" runat="server" Display="Dynamic" ControlToValidate="txtUserId" ErrorMessage="UserId Required..!" ValidationGroup="Blnk_chk" />
                                            </td>
                                            <td class="tdbkg">User Name
                                            </td>
                                            <td>
                                                <obout:OboutTextBox ID="txtUsrEmpDesc" Style="text-transform: uppercase" runat="server" MaxLength="100"></obout:OboutTextBox>
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator4" runat="server" Display="Dynamic" ControlToValidate="txtUsrEmpDesc" ErrorMessage="UserName Required..!" ValidationGroup="Blnk_chk" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="tdbkg">User Password
                                            </td>
                                            <td>
                                                <obout:OboutTextBox ID="txtUsrLoginPass" runat="server" MaxLength="100" TextMode="Password"></obout:OboutTextBox>
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator3" runat="server" Display="Dynamic" ControlToValidate="txtUsrLoginPass" ErrorMessage="Password Required..!" ValidationGroup="Blnk_chk" />
                                            </td>
                                            <td class="tdbkg">Email ID
                                            </td>
                                            <td>
                                                <obout:OboutTextBox ID="txtemail" runat="server" MaxLength="100"></obout:OboutTextBox>
                                                <asp:RegularExpressionValidator ID="emailValidator" runat="server" Display="Dynamic" ValidationGroup="Blnk_chk"
                                                    ErrorMessage="Please, enter valid e-mail address." ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$"
                                                    ControlToValidate="txtemail">
                                                </asp:RegularExpressionValidator>
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator1" runat="server" Display="Dynamic" ControlToValidate="txtemail" ErrorMessage="Please, enter an e-mail!" ValidationGroup="Blnk_chk" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdbkg">Intermediary Code</td>
                                            <td>
                                                <obout:OboutTextBox ID="txtIntermediaryCode" runat="server" MaxLength="100"></obout:OboutTextBox>
                                                <asp:HiddenField ID="hfIntermediaryCode" runat="server" Value="" />
                                                <asp:Button ID="btnGetIntermediaryCode" runat="server" OnClick="btnGetIntermediaryCode_Click" ValidationGroup="none" />
                                            </td>
                                            <td class="tdbkg">Intermediary Branch</td>
                                            <td>
                                                <obout:OboutTextBox ID="txtIntermediaryBranch" runat="server" MaxLength="100"></obout:OboutTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdbkg">Department</td>
                                            <td>
                                                <asp:DropDownList ID="drpDept" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpDept_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="tdbkg">Role</td>
                                            <td>
                                                <asp:DropDownList ID="drpRole" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdbkg">Is External User</td>
                                            <td>
                                                <obout:OboutCheckBox ID="chkIsExternalUser" runat="server" Checked="false"></obout:OboutCheckBox>
                                            </td>
                                            <td class="tdbkg">Market Movement Discount (0 to
                                                <asp:Label ID="lblMarketMovementDeviation" runat="server"></asp:Label>)</td>
                                            <td>
                                                <obout:OboutTextBox ID="txtMinMarketMovementDeviation" runat="server" MaxLength="3"></obout:OboutTextBox>
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator5" runat="server" Display="Dynamic" ControlToValidate="txtMinMarketMovementDeviation" ErrorMessage="Market Movement Deviation Required..!" ValidationGroup="Blnk_chk" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="tdbkg">Is allow login From Mobile </td>
                                            <td>
                                                <obout:OboutCheckBox ID="OboutChkMobileLogin" runat="server"  Checked="false" ></obout:OboutCheckBox> 
                                                <span id="mobileTip" style="font-size:9px;font-weight:bold">Only EPOS and BPOS users</span>
                                            </td>
                                            <td class="tdbkg">Is allow login From PASS </td>
                                            <td>
                                                <obout:OboutCheckBox ID="OboutChkChotuPASSLogin" runat="server" Checked="true"></obout:OboutCheckBox>
                                            </td>
                                        </tr>


                                        <tr>
                                            <td class="tdbkg">Type Of User </td>
                                            <td>
                                                <asp:DropDownList ID="drpUserType" runat="server">
                                                    <asp:ListItem Text="KGI" Value="KGI"></asp:ListItem>
                                                    <asp:ListItem Text="EPOS" Value="EPOS"></asp:ListItem>
                                                    <asp:ListItem Text="BPOS" Value="BPOS"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td class="tdbkg">Is allow EPOS Quote View </td>
                                            <td>
                                                <obout:OboutCheckBox ID="OboutChkEPOSQuoteView" runat="server" Checked="false"></obout:OboutCheckBox>
                                            </td>
                                        </tr>



                                        <tr>
                                            <td class="tdbkg">Regional Dept Head EmailId </td>
                                            <td>
                                                <obout:OboutTextBox ID="ObouttxtRegDeptHeadEmail" runat="server"></obout:OboutTextBox>
                                            </td>
                                            <td class="tdbkg">Is Account Locked </td>
                                            <td>
                                                <obout:OboutCheckBox ID="OboutChkLocked" runat="server" Checked="false"></obout:OboutCheckBox>
                                            </td>
                                        </tr>


                                        <tr>
                                            <td class="tdbkg">Is Active </td>
                                            <td>
                                                <obout:OboutCheckBox ID="OboutChkActive" runat="server" Checked="true"></obout:OboutCheckBox>
                                            </td>
                                            <td></td>
                                            <td></td>
                                        </tr>

                                    </table>
                                </asp:Panel>
                            </div>
                            <div class="section colm colm12">
                                <asp:RegularExpressionValidator ID="Regex4" runat="server" ControlToValidate="txtUsrLoginPass" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}" ErrorMessage="Password must contain: Minimum 8 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character" ForeColor="Red" ValidationGroup="Blnk_chk" />
                                <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="LightGray" BorderWidth="1">
                                    <div style="position: absolute; top: 40%; left: 35%; width: 10%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutButton ID="btnSave" runat="server" Text="Add" Width="100%" OnClick="btnSave_Click" ValidationGroup="Blnk_chk" />
                                        <asp:HiddenField ID="HdFldSave" runat="server" />
                                    </div>
                                    <div style="position: absolute; top: 40%; left: 50%; width: 10%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutButton ID="OboutReset" Width="100%" runat="server" Text="Reset"  OnClick="OboutReset_Click"></obout:OboutButton>
                                    </div>

                                    <div style="position: absolute; top: 40%; left: 65%; width: 10%; margin-top: 10px; margin-left: 10px">
                                        <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click"></obout:OboutButton>
                                    </div>

                                    <div style="position: absolute; top: 58%; left: 85%; width: 8%; margin-top: 10px; margin-left: 10px">
                                        <asp:Label ID="lblstatus" runat="server" />
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="section colm colm12">
                                <asp:Panel ID="Panel3" runat="server">
                                    <div style="display: none">
                                        <obout:Grid runat="server" ID="gvSubDetails" CallbackMode="true" Serialize="true"
                                            EnableTypeValidation="false" AutoGenerateColumns="false" OnUpdateCommand="gvSubDetails_RowUpdating"
                                            FolderStyle="Grid/styles/grand_gray">
                                            <ScrollingSettings ScrollWidth="100%" NumberOfFixedColumns="3" FixedColumnsPosition="Left" />
                                            <Columns>
                                                <obout:Column ID="Column1" DataField="vUserLoginId" ReadOnly="true" HeaderText="User Id" Width="100" runat="server" />
                                                <obout:Column ID="Column2" DataField="vUserLoginDesc" HeaderText="User Name" Width="150" runat="server" />
                                                <obout:Column ID="Column3" DataField="vUserPassword" HeaderText="User Password" Width="150" runat="server" />
                                                <obout:Column ID="Column6" DataField="vIntermediaryCode" HeaderText="Intermediary Code" Width="150" runat="server" />
                                                <obout:Column ID="Column7" DataField="vIntermediaryBranch" HeaderText="Intermediary Branch" Width="160" runat="server" />
                                                <obout:Column ID="Column4" DataField="bIsActivate" HeaderText="User Active" Visible="true" Width="160" runat="server">
                                                    <TemplateSettings TemplateId="bIsActiveTemplate" EditTemplateId="EditbIsActiveTemplate" />
                                                </obout:Column>
                                                <obout:Column ID="Column5" HeaderText="EDIT" Width="100" AllowEdit="true" AllowDelete="true" runat="server" />
                                            </Columns>
                                            <Templates>
                                                <obout:GridTemplate runat="server" ID="bIsActiveTemplate">
                                                    <Template>
                                                        <%# Container.DataItem["bIsActivate"].ToString() %>
                                                    </Template>
                                                </obout:GridTemplate>
                                                <obout:GridTemplate runat="server" ID="EditbIsActiveTemplate" ControlID="drpbIsActive" ControlPropertyName="value">
                                                    <Template>
                                                        <obout:ComboBox ID="drpbIsActive" runat="server">
                                                            <obout:ComboBoxItem Text="Activate" Value="Y" />
                                                            <obout:ComboBoxItem Text="Deactivate" Value="N" />
                                                        </obout:ComboBox>
                                                    </Template>
                                                </obout:GridTemplate>
                                            </Templates>
                                        </obout:Grid>
                                    </div>
                                    <asp:GridView ID="UserGridView" Visible="false" runat="server" AutoGenerateColumns="false" ItemType="ProjectPASS.UserDetails"
                                        SelectMethod="UserGridView_GetData" AutoGenerateEditButton="true" UpdateMethod="UserGridView_UpdateItem"
                                        DataKeyNames="vUserLoginId" CssClass="mydatagrid" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header"
                                        RowStyle-CssClass="rows" AllowPaging="true" OnPageIndexChanging="UserGridView_PageIndexChanging" PageSize="15">
                                        <Columns>
                                            <asp:BoundField DataField="vUserLoginId" ReadOnly="true" HeaderText="Login Id" HeaderStyle-Font-Size="Small" />
                                            <asp:BoundField DataField="vUserLoginDesc" HeaderText="User Name" HeaderStyle-Font-Size="Small" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="vUserEmailId" HeaderText="Email Id" HeaderStyle-Font-Size="Small" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="vIntermediaryCode" HeaderText="Intermediary Code" HeaderStyle-Font-Size="Small" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="vIntermediaryBranch" HeaderText="Intermediary Branch " HeaderStyle-Font-Size="Small" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:DynamicField DataField="bIsActivate" HeaderText="Is Active" HeaderStyle-Font-Size="Small" />
                                        </Columns>

                                    </asp:GridView>



                                    <%-- 2nd Gridview gvUSerdata Added for developement --%>

                                    <table id="tSearch" style="width: 100%;" cellspacing="0" cellpadding="2">
                                        <tr>
                                            <td class="tdbkg">Search Login ID</td>
                                            <td>
                                                <obout:OboutTextBox ID="txtSearch" runat="server" MaxLength="20"></obout:OboutTextBox>
                                                <obout:OboutButton ID="OboutBtnSearch" runat="server" Text="Search" OnClick="OboutBtnSearch_Click" />
                                            </td>
                                        </tr>
                                    </table>


                                    <br />
                                    <asp:GridView ID="UserDetailsGridView" runat="server" AutoGenerateColumns="false" ItemType="ProjectPASS.UserDetails"
                                        SelectMethod="UserDetailsGridView_GetData" DataKeyNames="vUserLoginId" CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                        HeaderStyle-CssClass="header" RowStyle-CssClass="rows" AllowPaging="true" OnSelectedIndexChanged="UserDetailsGridView_SelectedIndexChanged" >

                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton Text="Select" ID="lnkSelect" runat="server" CommandName="Select" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="vUserLoginId" ReadOnly="true" HeaderText="Login Id" HeaderStyle-Font-Size="Small" />
                                            <asp:BoundField DataField="vUserLoginDesc" HeaderText="User Name" HeaderStyle-Font-Size="Small" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="vUserEmailId" HeaderText="Email Id" HeaderStyle-Font-Size="Small" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="vIntermediaryCode" HeaderText="Intermediary Code" HeaderStyle-Font-Size="Small" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:BoundField DataField="vIntermediaryBranch" HeaderText="Intermediary Branch " HeaderStyle-Font-Size="Small" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" />
                                            <asp:DynamicField DataField="bIsActivate" HeaderText="Is Active" HeaderStyle-Font-Size="Small" />
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExit" />
        </Triggers>
    </asp:UpdatePanel>


</asp:Content>
