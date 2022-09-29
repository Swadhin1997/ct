<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PASS.Master" CodeBehind="FrmGPAPolicyEndorsementUpdate.aspx.cs" Inherits="ProjectPASS.FrmGPAPolicyEndorsementUpdate" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<%@ Register Assembly="obout_SuperForm" Namespace="Obout.SuperForm" TagPrefix="obout" %>

<%@ Register Assembly="obout_Window_NET" Namespace="OboutInc.Window" TagPrefix="owd" %>

<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="obout" %>

<asp:Content runat="server" ContentPlaceHolderID="MstCntFormContent">
    <div class="smart-wrap">
        <div class="smart-forms smart-container wrap-4">
            <div class="form-header header-blue">
                <h4><i class="fa fa-sign-in"></i>PASS - Endorsement Update</h4>
                <div id="divLogo" class="LogoCSS">
                    <img src="./Images/logo.jpg" style="height: 70px; width: 230px" alt="">
                </div>
            </div>
            <div class="form-body theme-blue">
                <div class="frm-row">
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel1" runat="server" Height="200px" BorderColor="#3399ff" BorderWidth="2">
                                <div style="position: absolute; top: 21%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="Label6" runat="server" Text="Policy Type" Width="100%"></asp:Label>
                            </div>
                              <div style="position: absolute; top: 20%; left: 8%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutRadioButton ID="rdoGPAPolicyType" runat="server" Text="GPA POLICY" GroupName="PolicyType" Checked="True"></obout:OboutRadioButton>
                                <span style="margin-left: 60px"></span>
                                    <obout:OboutRadioButton ID="rdoHDColicyType" runat="server" Text="HDC POLICY" GroupName="PolicyType"></obout:OboutRadioButton>
                            </div>

                            <div style="position: absolute; top: 21%; left: 45%; width: 20%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblPolicyId" runat="server" Text="Policy No. / Certificate No." Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 20%; left: 58%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtPolicyId" Width="100%" runat="server" MaxLength="15"></obout:OboutTextBox>
                            </div>
                           <div style="position: absolute; top: 58%; left: 1%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblCRNNo" runat="server" Text="CRN No" Width="100%"></asp:Label>
                            </div>
                          <div style="position: absolute; top: 57%; left: 8%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCrnNo" runat="server" Width="100%" MaxLength="15"></obout:OboutTextBox>
                            </div>
                            <div style="position: absolute; top: 58%; left: 45%; width: 15%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name" Width="100%"></asp:Label>
                            </div>
                            <div style="position: absolute; top: 57%; left: 58%; width: 35%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutTextBox ID="txtCustomerName" runat="server" Width="100%" MaxLength="100"></obout:OboutTextBox>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel2" runat="server" Height="100px" BorderColor="#3399ff" BorderWidth="2">
                            <div style="position: absolute; top: 40%; left: 25%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnGetPolicy" runat="server" Text="Get Policy" Width="100%" OnClick="btnGetPolicy_Click" />
                            </div>
                            <div style="position: absolute; top: 40%; left: 45%; width: 10%; margin-top: 10px; margin-left: 10px">
                                <obout:OboutButton ID="btnExit" Width="100%" runat="server" Text="Exit" OnClick="btnExit_Click"></obout:OboutButton>
                            </div>
                            <div style="position: absolute; top: 58%; left: 65%; width: 8%; margin-top: 10px; margin-left: 10px">
                                <asp:Label ID="lblstatus" runat="server"/>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="section colm colm12">
                        <asp:Panel ID="Panel3" runat="server" Height="400px">
                            <obout:Grid runat="server" ID="gvSubDetails" CallbackMode="true" Serialize="true"
                                EnableTypeValidation="false" AutoGenerateColumns="false" OnUpdateCommand="gvSubDetails_RowUpdating"
                                FolderStyle="Grid/styles/grand_gray">
                                <ScrollingSettings ScrollWidth="100%" NumberOfFixedColumns="3" FixedColumnsPosition="Left" />
                                <Columns>
                                    <obout:Column ID="Column1" DataField="vCertificateNo" ReadOnly="true" HeaderText="Certificate No" Width="150" runat="server" />
                                    <obout:Column ID="Column2" DataField="vCrnNo" ReadOnly="true" HeaderText="CRN No" Width="100" runat="server" />
                                    <obout:Column ID="Column27" DataField="vAccountNo" ReadOnly="true" HeaderText="Account No" Width="100" runat="server" />
                                    <obout:Column ID="Column3" DataField="vCustomerName" HeaderText="Customer Name" Width="200" runat="server" />
                                    <obout:Column ID="Column4" DataField="vCustomerGender" HeaderText="Gender" Width="100" runat="server" />
                                    <obout:Column ID="Column5" DataField="dCustomerDob" HeaderText="DOB" Width="100" runat="server" DataFormatString="{0:dd/MM/yyyy}" ApplyFormatInEditMode="true" />
                                    <obout:Column ID="Column6" DataField="vProposerAddLine1" HeaderText="Proposer Add1" Width="200" runat="server" />
                                    <obout:Column ID="Column7" DataField="vProposerAddLine2" HeaderText="Proposer Add2" Width="200" runat="server" />
                                    <obout:Column ID="Column8" DataField="vProposerAddLine3" HeaderText="Proposer Add3" Width="200" runat="server" />
                                    <obout:Column ID="Column9" DataField="vProposerCity" HeaderText="City" Width="100" runat="server" />
                                    <obout:Column ID="Column10" DataField="vProposerState" HeaderText="State" Width="150" runat="server" />
                                    <obout:Column ID="Column11" DataField="vProposerPinCode" HeaderText="Pincode" Width="100" runat="server" />
                                    <obout:Column ID="Column12" DataField="vMobileNo" HeaderText="Mobile No" Width="100" runat="server" />
                                    <obout:Column ID="Column13" DataField="vEmailId" HeaderText="Email Id" Width="150" runat="server" />
                                    <obout:Column ID="Column14" DataField="vNomineeName" HeaderText="Nominee Name" Width="200" runat="server" />
                                    <obout:Column ID="Column15" DataField="vNomineeRelation" HeaderText="Relation" Width="100" runat="server" />
                                    <obout:Column ID="Column16" DataField="vNomineeAge" HeaderText="Age" Width="100" runat="server" />
                                    <obout:Column ID="Column17" DataField="vNomineeGuardian" HeaderText="Nominee Guardian" Width="200" runat="server" />
                                    <obout:Column ID="Column18" DataField="vNomineeRelWithGuardian" ReadOnly="true" HeaderText="Minor Relation" Width="200" runat="server" />
                                    <obout:Column ID="Column19" DataField="vCorporateID" HeaderText="Corporate Id" Width="150" runat="server" />
                                    <obout:Column ID="Column20" DataField="vCorporateName"  ReadOnly="true" HeaderText="Corporate Name" Width="200" runat="server" />
                                    <obout:Column ID="Column21" DataField="vEndorsementType" HeaderText="Endorse. Type" Width="150" runat="server" />
                                    <obout:Column ID="Column22" DataField="vEndorsementReason" HeaderText="Endorse. Reason" Width="200" runat="server" />
                                    <obout:Column ID="Column23" DataField="vEndorsementDesc" HeaderText="Endorse. Desc" Width="200" runat="server" />
                                    <obout:Column ID="Column24" DataField="dEndorsementEffectiveDate" HeaderText="Endorse. Effective Date" Width="200" runat="server" DataFormatString="{0:dd/MM/yyyy}" ApplyFormatInEditMode="true" />
                                    <obout:Column ID="Column25" DataField="dEndorsementIssueDate" HeaderText="Endors. Issue Date" Width="200" runat="server" DataFormatString="{0:dd/MM/yyyy}" ApplyFormatInEditMode="true" />
                                    <obout:Column ID="Column26" HeaderText="EDIT" Width="150" AllowEdit="true" AllowDelete="true" runat="server" />
                                </Columns>
                            </obout:Grid>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

