﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PrjPASS.GSTService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GSTBreakUpTaxDetails", Namespace="http://schemas.datacontract.org/2004/07/WCF_Partner_integration")]
    [System.SerializableAttribute()]
    public partial class GSTBreakUpTaxDetails : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CGSTAmountField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CGSTPercentageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CustomerGSTStateIdentifierField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrroMessageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IGSTAmountField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IGSTPercentageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IntermediaryGSTStateIdentifierField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SGSTAmountField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SGSTPercentageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TotalGSTAmountField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UGSTAmountField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UGSTPercentageField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CGSTAmount {
            get {
                return this.CGSTAmountField;
            }
            set {
                if ((object.ReferenceEquals(this.CGSTAmountField, value) != true)) {
                    this.CGSTAmountField = value;
                    this.RaisePropertyChanged("CGSTAmount");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CGSTPercentage {
            get {
                return this.CGSTPercentageField;
            }
            set {
                if ((object.ReferenceEquals(this.CGSTPercentageField, value) != true)) {
                    this.CGSTPercentageField = value;
                    this.RaisePropertyChanged("CGSTPercentage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CustomerGSTStateIdentifier {
            get {
                return this.CustomerGSTStateIdentifierField;
            }
            set {
                if ((object.ReferenceEquals(this.CustomerGSTStateIdentifierField, value) != true)) {
                    this.CustomerGSTStateIdentifierField = value;
                    this.RaisePropertyChanged("CustomerGSTStateIdentifier");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrroMessage {
            get {
                return this.ErrroMessageField;
            }
            set {
                if ((object.ReferenceEquals(this.ErrroMessageField, value) != true)) {
                    this.ErrroMessageField = value;
                    this.RaisePropertyChanged("ErrroMessage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IGSTAmount {
            get {
                return this.IGSTAmountField;
            }
            set {
                if ((object.ReferenceEquals(this.IGSTAmountField, value) != true)) {
                    this.IGSTAmountField = value;
                    this.RaisePropertyChanged("IGSTAmount");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IGSTPercentage {
            get {
                return this.IGSTPercentageField;
            }
            set {
                if ((object.ReferenceEquals(this.IGSTPercentageField, value) != true)) {
                    this.IGSTPercentageField = value;
                    this.RaisePropertyChanged("IGSTPercentage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IntermediaryGSTStateIdentifier {
            get {
                return this.IntermediaryGSTStateIdentifierField;
            }
            set {
                if ((object.ReferenceEquals(this.IntermediaryGSTStateIdentifierField, value) != true)) {
                    this.IntermediaryGSTStateIdentifierField = value;
                    this.RaisePropertyChanged("IntermediaryGSTStateIdentifier");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SGSTAmount {
            get {
                return this.SGSTAmountField;
            }
            set {
                if ((object.ReferenceEquals(this.SGSTAmountField, value) != true)) {
                    this.SGSTAmountField = value;
                    this.RaisePropertyChanged("SGSTAmount");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SGSTPercentage {
            get {
                return this.SGSTPercentageField;
            }
            set {
                if ((object.ReferenceEquals(this.SGSTPercentageField, value) != true)) {
                    this.SGSTPercentageField = value;
                    this.RaisePropertyChanged("SGSTPercentage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TotalGSTAmount {
            get {
                return this.TotalGSTAmountField;
            }
            set {
                if ((object.ReferenceEquals(this.TotalGSTAmountField, value) != true)) {
                    this.TotalGSTAmountField = value;
                    this.RaisePropertyChanged("TotalGSTAmount");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UGSTAmount {
            get {
                return this.UGSTAmountField;
            }
            set {
                if ((object.ReferenceEquals(this.UGSTAmountField, value) != true)) {
                    this.UGSTAmountField = value;
                    this.RaisePropertyChanged("UGSTAmount");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UGSTPercentage {
            get {
                return this.UGSTPercentageField;
            }
            set {
                if ((object.ReferenceEquals(this.UGSTPercentageField, value) != true)) {
                    this.UGSTPercentageField = value;
                    this.RaisePropertyChanged("UGSTPercentage");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PolicyVerifcation", Namespace="http://schemas.datacontract.org/2004/07/WCF_Partner_integration")]
    [System.SerializableAttribute()]
    public partial class PolicyVerifcation : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CRNNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PolicyNoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Response_Date_TimeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SuccessField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CRNNumber {
            get {
                return this.CRNNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.CRNNumberField, value) != true)) {
                    this.CRNNumberField = value;
                    this.RaisePropertyChanged("CRNNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Message {
            get {
                return this.MessageField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageField, value) != true)) {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PolicyNo {
            get {
                return this.PolicyNoField;
            }
            set {
                if ((object.ReferenceEquals(this.PolicyNoField, value) != true)) {
                    this.PolicyNoField = value;
                    this.RaisePropertyChanged("PolicyNo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Response_Date_Time {
            get {
                return this.Response_Date_TimeField;
            }
            set {
                if ((object.ReferenceEquals(this.Response_Date_TimeField, value) != true)) {
                    this.Response_Date_TimeField = value;
                    this.RaisePropertyChanged("Response_Date_Time");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Success {
            get {
                return this.SuccessField;
            }
            set {
                if ((object.ReferenceEquals(this.SuccessField, value) != true)) {
                    this.SuccessField = value;
                    this.RaisePropertyChanged("Success");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="GSTService.IGCCommon")]
    public interface IGCCommon {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGCCommon/GetRenewalPolicyInformation", ReplyAction="http://tempuri.org/IGCCommon/GetRenewalPolicyInformationResponse")]
        string GetRenewalPolicyInformation(
                    string userid, 
                    string password, 
                    string KGICustomerID, 
                    string PolicyNo, 
                    string RegistrationBlock1, 
                    string RegistrationBlock2, 
                    string RegistrationBlock3, 
                    string RegistrationBlock4, 
                    string EngineNo, 
                    string ChassisNo, 
                    string ProposerCRNNos, 
                    string ProposerAPACNo, 
                    string MemberID, 
                    string MemberDOB, 
                    string SourceChannelID, 
                    string AdditionalParameter1, 
                    string AdditionalParameter2, 
                    string AdditionalParameter3, 
                    string AdditionalParameter4, 
                    string AdditionalParameter5);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGCCommon/GetRenewalPolicyInformation", ReplyAction="http://tempuri.org/IGCCommon/GetRenewalPolicyInformationResponse")]
        System.Threading.Tasks.Task<string> GetRenewalPolicyInformationAsync(
                    string userid, 
                    string password, 
                    string KGICustomerID, 
                    string PolicyNo, 
                    string RegistrationBlock1, 
                    string RegistrationBlock2, 
                    string RegistrationBlock3, 
                    string RegistrationBlock4, 
                    string EngineNo, 
                    string ChassisNo, 
                    string ProposerCRNNos, 
                    string ProposerAPACNo, 
                    string MemberID, 
                    string MemberDOB, 
                    string SourceChannelID, 
                    string AdditionalParameter1, 
                    string AdditionalParameter2, 
                    string AdditionalParameter3, 
                    string AdditionalParameter4, 
                    string AdditionalParameter5);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGCCommon/GSTBreakUpTaxDetails", ReplyAction="http://tempuri.org/IGCCommon/GSTBreakUpTaxDetailsResponse")]
        PrjPASS.GSTService.GSTBreakUpTaxDetails GSTBreakUpTaxDetails(string strPropsalDate, string strTotalPremium, string strIntermediaryCode, string strIntemediaryOfficeCode, string strCustomerStateCode, string strProductCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGCCommon/GSTBreakUpTaxDetails", ReplyAction="http://tempuri.org/IGCCommon/GSTBreakUpTaxDetailsResponse")]
        System.Threading.Tasks.Task<PrjPASS.GSTService.GSTBreakUpTaxDetails> GSTBreakUpTaxDetailsAsync(string strPropsalDate, string strTotalPremium, string strIntermediaryCode, string strIntemediaryOfficeCode, string strCustomerStateCode, string strProductCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGCCommon/VerifyGCPolicy", ReplyAction="http://tempuri.org/IGCCommon/VerifyGCPolicyResponse")]
        PrjPASS.GSTService.PolicyVerifcation VerifyGCPolicy(
                    string userid, 
                    string password, 
                    string strPolicyNumber, 
                    string strDOBOfProposer, 
                    string strMemberID, 
                    string strRegistrationNumber, 
                    string strEngineNumber, 
                    string strChassisNumber, 
                    string strCustMobie, 
                    string strCustEmail, 
                    string strCustCRN, 
                    string strCustPAN, 
                    string strAdd1, 
                    string strAdd2, 
                    string strAdd3, 
                    string strAdd4, 
                    string strRequestTime);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGCCommon/VerifyGCPolicy", ReplyAction="http://tempuri.org/IGCCommon/VerifyGCPolicyResponse")]
        System.Threading.Tasks.Task<PrjPASS.GSTService.PolicyVerifcation> VerifyGCPolicyAsync(
                    string userid, 
                    string password, 
                    string strPolicyNumber, 
                    string strDOBOfProposer, 
                    string strMemberID, 
                    string strRegistrationNumber, 
                    string strEngineNumber, 
                    string strChassisNumber, 
                    string strCustMobie, 
                    string strCustEmail, 
                    string strCustCRN, 
                    string strCustPAN, 
                    string strAdd1, 
                    string strAdd2, 
                    string strAdd3, 
                    string strAdd4, 
                    string strRequestTime);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGCCommon/GetUWPolicyData", ReplyAction="http://tempuri.org/IGCCommon/GetUWPolicyDataResponse")]
        string GetUWPolicyData(string GCUser, string GCPassword, string strSource, string strCustomerCRNNo, string strCustomerEmailId, string strAdditionalParam1, string strAdditionalParam2, string strAdditionalParam3, string strAdditionalParam4);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGCCommon/GetUWPolicyData", ReplyAction="http://tempuri.org/IGCCommon/GetUWPolicyDataResponse")]
        System.Threading.Tasks.Task<string> GetUWPolicyDataAsync(string GCUser, string GCPassword, string strSource, string strCustomerCRNNo, string strCustomerEmailId, string strAdditionalParam1, string strAdditionalParam2, string strAdditionalParam3, string strAdditionalParam4);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGCCommon/UpdateCustomerCRNNumber", ReplyAction="http://tempuri.org/IGCCommon/UpdateCustomerCRNNumberResponse")]
        string UpdateCustomerCRNNumber(string GCUser, string GCPassword, string strCustomerId, string strCRNNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGCCommon/UpdateCustomerCRNNumber", ReplyAction="http://tempuri.org/IGCCommon/UpdateCustomerCRNNumberResponse")]
        System.Threading.Tasks.Task<string> UpdateCustomerCRNNumberAsync(string GCUser, string GCPassword, string strCustomerId, string strCRNNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGCCommon/IDVSyncData", ReplyAction="http://tempuri.org/IGCCommon/IDVSyncDataResponse")]
        string IDVSyncData(string strIDV);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGCCommon/IDVSyncData", ReplyAction="http://tempuri.org/IGCCommon/IDVSyncDataResponse")]
        System.Threading.Tasks.Task<string> IDVSyncDataAsync(string strIDV);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGCCommonChannel : PrjPASS.GSTService.IGCCommon, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GCCommonClient : System.ServiceModel.ClientBase<PrjPASS.GSTService.IGCCommon>, PrjPASS.GSTService.IGCCommon {
        
        public GCCommonClient() {
        }
        
        public GCCommonClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public GCCommonClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GCCommonClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GCCommonClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetRenewalPolicyInformation(
                    string userid, 
                    string password, 
                    string KGICustomerID, 
                    string PolicyNo, 
                    string RegistrationBlock1, 
                    string RegistrationBlock2, 
                    string RegistrationBlock3, 
                    string RegistrationBlock4, 
                    string EngineNo, 
                    string ChassisNo, 
                    string ProposerCRNNos, 
                    string ProposerAPACNo, 
                    string MemberID, 
                    string MemberDOB, 
                    string SourceChannelID, 
                    string AdditionalParameter1, 
                    string AdditionalParameter2, 
                    string AdditionalParameter3, 
                    string AdditionalParameter4, 
                    string AdditionalParameter5) {
            return base.Channel.GetRenewalPolicyInformation(userid, password, KGICustomerID, PolicyNo, RegistrationBlock1, RegistrationBlock2, RegistrationBlock3, RegistrationBlock4, EngineNo, ChassisNo, ProposerCRNNos, ProposerAPACNo, MemberID, MemberDOB, SourceChannelID, AdditionalParameter1, AdditionalParameter2, AdditionalParameter3, AdditionalParameter4, AdditionalParameter5);
        }
        
        public System.Threading.Tasks.Task<string> GetRenewalPolicyInformationAsync(
                    string userid, 
                    string password, 
                    string KGICustomerID, 
                    string PolicyNo, 
                    string RegistrationBlock1, 
                    string RegistrationBlock2, 
                    string RegistrationBlock3, 
                    string RegistrationBlock4, 
                    string EngineNo, 
                    string ChassisNo, 
                    string ProposerCRNNos, 
                    string ProposerAPACNo, 
                    string MemberID, 
                    string MemberDOB, 
                    string SourceChannelID, 
                    string AdditionalParameter1, 
                    string AdditionalParameter2, 
                    string AdditionalParameter3, 
                    string AdditionalParameter4, 
                    string AdditionalParameter5) {
            return base.Channel.GetRenewalPolicyInformationAsync(userid, password, KGICustomerID, PolicyNo, RegistrationBlock1, RegistrationBlock2, RegistrationBlock3, RegistrationBlock4, EngineNo, ChassisNo, ProposerCRNNos, ProposerAPACNo, MemberID, MemberDOB, SourceChannelID, AdditionalParameter1, AdditionalParameter2, AdditionalParameter3, AdditionalParameter4, AdditionalParameter5);
        }
        
        public PrjPASS.GSTService.GSTBreakUpTaxDetails GSTBreakUpTaxDetails(string strPropsalDate, string strTotalPremium, string strIntermediaryCode, string strIntemediaryOfficeCode, string strCustomerStateCode, string strProductCode) {
            return base.Channel.GSTBreakUpTaxDetails(strPropsalDate, strTotalPremium, strIntermediaryCode, strIntemediaryOfficeCode, strCustomerStateCode, strProductCode);
        }
        
        public System.Threading.Tasks.Task<PrjPASS.GSTService.GSTBreakUpTaxDetails> GSTBreakUpTaxDetailsAsync(string strPropsalDate, string strTotalPremium, string strIntermediaryCode, string strIntemediaryOfficeCode, string strCustomerStateCode, string strProductCode) {
            return base.Channel.GSTBreakUpTaxDetailsAsync(strPropsalDate, strTotalPremium, strIntermediaryCode, strIntemediaryOfficeCode, strCustomerStateCode, strProductCode);
        }
        
        public PrjPASS.GSTService.PolicyVerifcation VerifyGCPolicy(
                    string userid, 
                    string password, 
                    string strPolicyNumber, 
                    string strDOBOfProposer, 
                    string strMemberID, 
                    string strRegistrationNumber, 
                    string strEngineNumber, 
                    string strChassisNumber, 
                    string strCustMobie, 
                    string strCustEmail, 
                    string strCustCRN, 
                    string strCustPAN, 
                    string strAdd1, 
                    string strAdd2, 
                    string strAdd3, 
                    string strAdd4, 
                    string strRequestTime) {
            return base.Channel.VerifyGCPolicy(userid, password, strPolicyNumber, strDOBOfProposer, strMemberID, strRegistrationNumber, strEngineNumber, strChassisNumber, strCustMobie, strCustEmail, strCustCRN, strCustPAN, strAdd1, strAdd2, strAdd3, strAdd4, strRequestTime);
        }
        
        public System.Threading.Tasks.Task<PrjPASS.GSTService.PolicyVerifcation> VerifyGCPolicyAsync(
                    string userid, 
                    string password, 
                    string strPolicyNumber, 
                    string strDOBOfProposer, 
                    string strMemberID, 
                    string strRegistrationNumber, 
                    string strEngineNumber, 
                    string strChassisNumber, 
                    string strCustMobie, 
                    string strCustEmail, 
                    string strCustCRN, 
                    string strCustPAN, 
                    string strAdd1, 
                    string strAdd2, 
                    string strAdd3, 
                    string strAdd4, 
                    string strRequestTime) {
            return base.Channel.VerifyGCPolicyAsync(userid, password, strPolicyNumber, strDOBOfProposer, strMemberID, strRegistrationNumber, strEngineNumber, strChassisNumber, strCustMobie, strCustEmail, strCustCRN, strCustPAN, strAdd1, strAdd2, strAdd3, strAdd4, strRequestTime);
        }
        
        public string GetUWPolicyData(string GCUser, string GCPassword, string strSource, string strCustomerCRNNo, string strCustomerEmailId, string strAdditionalParam1, string strAdditionalParam2, string strAdditionalParam3, string strAdditionalParam4) {
            return base.Channel.GetUWPolicyData(GCUser, GCPassword, strSource, strCustomerCRNNo, strCustomerEmailId, strAdditionalParam1, strAdditionalParam2, strAdditionalParam3, strAdditionalParam4);
        }
        
        public System.Threading.Tasks.Task<string> GetUWPolicyDataAsync(string GCUser, string GCPassword, string strSource, string strCustomerCRNNo, string strCustomerEmailId, string strAdditionalParam1, string strAdditionalParam2, string strAdditionalParam3, string strAdditionalParam4) {
            return base.Channel.GetUWPolicyDataAsync(GCUser, GCPassword, strSource, strCustomerCRNNo, strCustomerEmailId, strAdditionalParam1, strAdditionalParam2, strAdditionalParam3, strAdditionalParam4);
        }
        
        public string UpdateCustomerCRNNumber(string GCUser, string GCPassword, string strCustomerId, string strCRNNumber) {
            return base.Channel.UpdateCustomerCRNNumber(GCUser, GCPassword, strCustomerId, strCRNNumber);
        }
        
        public System.Threading.Tasks.Task<string> UpdateCustomerCRNNumberAsync(string GCUser, string GCPassword, string strCustomerId, string strCRNNumber) {
            return base.Channel.UpdateCustomerCRNNumberAsync(GCUser, GCPassword, strCustomerId, strCRNNumber);
        }
        
        public string IDVSyncData(string strIDV) {
            return base.Channel.IDVSyncData(strIDV);
        }
        
        public System.Threading.Tasks.Task<string> IDVSyncDataAsync(string strIDV) {
            return base.Channel.IDVSyncDataAsync(strIDV);
        }
    }
}
