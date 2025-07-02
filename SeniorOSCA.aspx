<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="SeniorOSCA.aspx.vb" Inherits="SeniorOSCA" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <input id="txt_refID" runat="server" hidden="hidden" />
    <input id="txt_brgycity" hidden="hidden" runat="server" />

    <div class="row">
        <div class="col-12">
            <div id="div_list" runat="server" class="divList">
                <div class="card">
                    <div class="card-header">
                        <ul class="nav nav-pills ml-auto">
                            <li class="nav-item">
                                <a class="nav-link active" data-toggle="tab" href="#tab_default">Filter by Category</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" data-toggle="tab" href="#tab_search" role="tab" aria-selected="false">
                                    Search by Keyword</a>
                            </li>
                        </ul> 
                    </div>
                    <div class="card-body">
                        <div class="tab-content" id="custom-content-below-tabContent">
                            <div class="tab-pane fade show active" id="tab_default" role="tabpanel">
                                <div class="row">
                                    <!-- Baranggay Selection -->
                                    <div class="form-group col-12 col-md-4">
                                        <label>Baranggay</label>
                                        <select id="dl_filterBrgy" class="form-control select2" onchange="generateImportTable(0);">
                                            <option value="">All</option>
                                            <!-- Options will be populated dynamically -->
                                        </select>
                                    </div>
                                    <%--slider--%>
                                    <div class="form-group col-12 col-md-4 d-flex flex-column align-items-center"
                                        style="background-color: #f8f9fa; border: 1px solid #dee2e6; border-radius: 8px;
                                         ">
                                        <label class="text-center w-100" font-weight: bold;">Age Range</label>
                                        <div id="age-range-slider" class="mt-1 ml-1"
                                            style="width: 80%; background-color: #e9ecef; border-radius: 5px; height: 5px;">
                                        </div>
                                        <input type="hidden" id="dl_filterAge" name="age_range" value="60-100">
                                        <p class="mt-1 text-center" style="color: #28a745; font-size: 1em;"><span id="age-range-display">
                                            60 - 100</span></p>
                                    </div>

                                    <!-- Sex Selection -->
                                    <div class="form-group col-12 col-md-4">        
                                        <label>Sex</label>
                                        <select id="dl_filterSex" class="form-control select2" onchange="generateImportTable(0);">
                                            <option value="">All</option>
                                            <option value="1">Female</option>
                                            <option value="2">Male</option>
                                        </select>
                                    </div>
                                </div>


                                <div class="row">
                                    <!-- Second Row: 2 Columns -->
                                    <div class="form-group col-12 col-md-6">
                                        <label>Pensioner Type</label>
                                        <select id="dl_DSWDPensioner" class="form-control select2" onchange="generateImportTable(0);">
                                            <option value="">All</option>
                                            <option value="2">Regional</option>
                                            <option value="3">Subsistence Assistance</option>
                                            <option value="4">City</option>
                                        </select>
                                    </div>

                                    <div class="form-group col-12 col-md-6">
                                        <label>Status</label>
                                        <select id="dl_filterStatus" class="form-control select2" onchange="generateImportTable(0);">
                                            <option value="Last">Last 10 Input</option>
                                            <option value="">All</option>
                                            <option value="1">Active</option>
                                            <option value="0">Deactivated</option>
                                            <option value="2">Cancelled</option>
                                        </select>
                                    </div>
                                </div>
                            </div>

                            <div class="tab-pane fade" id="tab_search" role="tabpanel">
                                <div class="form-group">
                                    <div class="input-group">
                                        <input type="text" class="form-control" id="txt_filterSearch" placeholder="Search by CPS ID/SNR ID or by Senior Citizen's fullname"
                                            onkeypress="return EnterEvent(event)" />
                                        <span class="input-group-append">
                                            <button type="button" class="btn btn-info btn-flat" onclick="generateImportTable(1);"
                                                id="btn_searchKeyword">
                                                <i class="fa fa-search"></i>
                                            </button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

                <div class="card">
                    <div class="card-header" runat="server" id="header_add" visible="false">
                        <button type="button" id="btn_addNew" runat="server" onclick="showDetails();" class="btn bg-gradient-primary">
                            <i class="ace-icon fa fa-plus-circle bigger-110"></i>
                            Add New
                        </button>

                        <!-- Button trigger modal search -->
                        <button type="button" class="btn btn-info" data-target="#modal_citizenSearch" data-toggle="modal"> Print ID 
                          <i class="fa fa-id-card"></i></button>

                    </div>

                    <div class="card-body">
                        <div class="table-responsive">
                            <table id="example1" class="table table-bordered table-striped cepTable">
                                <thead>
                                    <tr>
                                        <th>RefID</th>
                                        <th>Name</th>
                                        <th>Sex</th>
                                        <th>Birthday</th>
                                        <th>Age</th>
                                        <th>Location/Branggay</th>
                                        <th>NCSCRRN</th>
                                        <th>Pensioner Type</th>
                                        <th class="text-center">Status</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder ID="ph_list" runat="server"></asp:PlaceHolder>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>

            <div class="divDetails" hidden="hidden" id="div_details" runat="server">
                <div class="card">
                    <div class="card-header d-flex p-0">
                        <h3 class="card-title p-3" id="h3_title" runat="server">Add New</h3>



                        <ul class="nav nav-pills ml-auto p-2" runat="server" id="ul_tabs" visible="false">
                            <li class="nav-item"><a class="nav-link active" href="#tab_details" data-toggle="tab">
                                Details</a></li>
                            <li class="nav-item"><a class="nav-link" href="#tab_timeline" data-toggle="tab">Timeline</a>
                            </li>
                        </ul>
                    </div>


                    <%--TAB PANE ADD NEW--%>
                    <div class="card-body">
                        <div class="tab-content">
                            <div class="tab-pane active" id="tab_details">
                                <div class="row">
                                    <%--SEARCH BAR--%>
                                    <div class="form-group col-md-6 col-4" runat="server">
                                        <label>Citizen Profile</label>
                                        <div class="input-group">
                                            <input type="text" class="form-control appendGroup" disabled="disabled" runat="server"
                                                id="txt_ownerName" />
                                            <span class="input-group-append">
                                                <button type="button" class="btn btn-primary" onclick="ViewSearch()">
                                                    <i class="fa fa-search"></i>
                                                </button>
                                            </span>
                                        </div>
                                        <input type="text" class="form-control" hidden runat="server" id="txt_ownerID" />
                                    </div>
                                    <div class="form-group col-2 col-md-2"> 
                                        <label>Type</label>
                                        <select id="dl_type" runat="server" class="form-control select2 required">
                                            <option value="">-</option>
                                            <option value="1" selected>Trece Resident</option>
                                            <option value="2">Non Trece Resident</option>
                                        </select>
                                    </div>
                                    

                                    <input type="hidden" id="txt_updates" runat="server"/>
                                    <input type="hidden" id="txt_pictureName" runat="server" />

                                    <div class="form-group col-2 col-md-2">
                                        <label>Pentioner Type</label>
                                        <select id="dl_dswdPentioner" runat="server" class="form-control checkValue select2 required ">
                                            <option value="">-</option>
                                            <%--<option value="1">Yes</option>
                                            <option value="0">No</option>--%>
                                            <option value="2">Regional</option>
                                            <option value="3">Subsistence Assistance</option>
                                            <option value="4">City</option>
                                        </select>
                                    </div>
                                   

                                    <div class="form-group col-2 col-md-2">
                                        <label>NCSC-RRN</label>
                                        <div>
                                            <input type="text" class="form-control checkValue" runat="server" id="txt_ncscRRN" />
                                        </div>
                                    </div>

                                    <div id="div_attachment" class="hide form-group col-md-12 row" runat="server">
                                        <div class="form-group col-md-6">
                                            <label>Notarized Application Form with 2x2 Picture</label>
                                            <select id="dl_notarizedFormWithID" runat="server" class="form-control checkValue select2">
                                                <option value="">-</option>
                                                <option value="1">Yes</option>
                                                <option value="0">No</option>
                                            </select>
                                        </div>

                                        <div class="form-group col-md-6">
                                            <label>Full Body ID Pricture</label>
                                            <select id="dl_fullBodyID" runat="server" class="form-control checkValue select2">
                                                <option value="">-</option>
                                                <option value="1">Yes</option>
                                                <option value="0">No</option>
                                            </select>
                                        </div>

                                        <div class="form-group col-md-6">
                                            <label>Primary Requirements</label>
                                            <select id="dl_priReq" class="form-control checkValue select2" multiple="multiple">
                                                <option value="">-</option>
                                                <option value="1">Birth Certificate</option>
                                                <option value="2">Philsys ID</option>
                                                <option value="3">National ID</option>
                                            </select>
                                        </div>
                                        <input id="txt_priReq" hidden="hidden" runat="server" />

                                        <div class="form-group col-md-6">
                                            <label>Secondary Requirements</label>
                                            <select id="dl_secReq" class="form-control select2" multiple="multiple">
                                                <option value="">-</option>
                                                <option value="1">(1) PASSPORT</option>
                                                <option value="2">(2) PSA CERTIFICATE OF LATE REGISTRATION OF BIRTH</option>
                                                <option value="3">(3) SENIOR CITIZENS' ID</option>
                                                <option value="4">(4) ID FROM GSIS, SSS, LTO, PRC, POSTAL, COMELEC OR CERTIFICATION</option>
                                                <option value="5">(5) CERTIFICATE OF LIVE BIRTH OF THE ELDEST CHILD</option>
                                                <option value="6">(6) CERTIFICATE OF MARRIAGE FROM LC/RPSA</option>
                                                <option value="7">(7) JOINT AFFIDAVIT EXECUTED BY TWO (2) DISINTERESTED PERSONS</option>
                                                <option value="8">(8) SCHOOL RECORDS OR EMPLOYMENT RECORDS</option>
                                                <option value="9">(9) CERTIFICATE OF MEMBERSHIP FROM RETIREMENT AND PENSION INSURANCE
                                                    SYSTEMS (SSS, GSIS, PVAO, AFPMBAI, PNRBBS, ETC.)</option>
                                                <option value="10">(10) BAPTISMAL CERTIFICATE OR ANY CHURCH RECORD OF BAPTISM</option>
                                                <option value="11">(11) CERTIFICATION OR SIMILAR DOCUMENT DULY ISSUED BY THE NCIP OR
                                                    NCMF</option>
                                            </select>
                                        </div>

                                        <input id="txt_secReq" hidden="hidden" runat="server" />
                                        <input id="populateMultiple" hidden="hidden" runat="server" value="0" />
                                    </div>

                                    <div class="form-group col-12 col-md-3">
                                        <label>First Name</label>
                                        <div>
                                            <input type="text" class="form-control checkValue required" id="txt_FName" runat="server"
                                                onchange="validateCitizen1(this)" />
                                        </div>
                                    </div>

                                    <div class="form-group col-12 col-md-3">
                                        <label>Middle Name</label>
                                        <div>
                                            <input type="text" class="form-control checkValue" runat="server" id="txt_MName" />
                                        </div>
                                    </div>

                                    <div class="form-group col-12 col-md-3">
                                        <label>Last Name</label>
                                        <div>
                                            <input type="text" class="form-control checkValue required " runat="server" id="txt_LName"
                                                onchange="validateCitizen1(this)" />
                                        </div>
                                    </div>

                                    <div class="form-group col-12 col-md-3">
                                        <label>Suffix</label>
                                        <select id="dl_suffix" runat="server" class="form-control checkValue select2">
                                            <option value="">-</option>
                                            <option value="SRA">SRA</option>
                                            <option value="JRA">JRA</option>
                                            <option value="SR">SR</option>
                                            <option value="JR">JR</option>
                                            <option value="I">I</option>
                                            <option value="II">II</option>
                                            <option value="III">III</option>
                                            <option value="IV">IV</option>
                                            <option value="V">V</option>
                                            <option value="VI">VI</option>
                                            <option value="VII">VII</option>
                                        </select>
                                    </div>

                                    <div class="form-group col-12 col-md-3">
                                        <label>Sex</label>
                                        <select id="dl_sex" runat="server" class="form-control checkValue select2 required">
                                            <option value="">-</option>
                                            <option value="1">Female</option>
                                            <option value="2">Male</option>
                                        </select>
                                    </div>

                                    <div class="form-group col-12 col-md-3">
                                        <label>Civil Status</label>
                                        <select id="dl_civilStatus" runat="server" class="form-control checkValue select2 required">
                                            <option value="">-</option>
                                            <option value="Married">Married</option>
                                            <option value="Single">Single</option>
                                            <option value="Widowed">Widowed</option>
                                            <option value="Separated">Separated</option>
                                        </select>
                                    </div>

                                    <div class="form-group col-12 col-md-3">
                                        <label>Birth Date</label>
                                        <div class="input-group date lastDateNow" id="div_mainbDate" data-target-input="nearest">
                                            <input type="text" class="form-control datetimepicker-input required appendGroup"
                                                runat="server" onchange="validateCitizen1(this)" data-target="#div_mainbDate"
                                                id="txt_BDate" />
                                            <div class="input-group-append" data-target="#div_mainbDate" data-toggle="datetimepicker">
                                                <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-12 col-md-3">
                                        <label>Place of Birth</label>
                                        <div>
                                            <input type="text" class="form-control checkValue" runat="server" id="txt_BPLace" />
                                        </div>
                                    </div>

                                    <div class="form-group col-12 col-md-3">
                                        <label>Citizenship</label>
                                        <div>
                                            <input type="text" class="form-control checkValue required" runat="server" id="txt_Citizenship" />
                                        </div>
                                    </div>

                                    <div class="form-group col-12 col-md-3">
                                        <label>Height </label><label style="font-weight:400; padding-left:3px""> (cm)</label>
                                        <div>
                                            <input type="text" class="form-control checkValue decOnly" runat="server" id="txt_Height"
                                                placeholder="ex. 160" />
                                        </div>
                                    </div>

                                    <div class="form-group col-12 col-md-3">
                                        <label>Weight</label><label style="font-weight:400; padding-left:3px"> (kg)</label>
                                        <div>
                                            <input type="text" class="form-control checkValue decOnly" runat="server" id="txt_Weight"
                                                placeholder="ex. 60" />
                                        </div>
                                    </div>

                                    <div class="form-group col-12 col-md-3">
                                        <label>LGBTQIA+</label>
                                        <div class="sel2">
                                            <select id="dl_lgbtq" runat="server" class="form-control checkValue select2 required">
                                                <option value="0">-</option>
                                                <option value="1">Yes</option>
                                                <option value="0">No</option>
                                            </select>
                                        </div>
                                    </div>

                                    <div class="form-group col-12 col-md-6">
                                        <label>Contact No</label>
                                        <div>
                                            <input type="text" class="form-control checkValue" runat="server" value="" placeholder="ex. 09123456789"
                                                id="txt_contactNo" oninput="validateContactNo()"
                                                onkeydown="restrictNonNumeric(event)" maxlength="11" />
                                            <span id="error-message" style="color: red; display: none;">Please enter 11 digits only.</span>
                                        </div>
                                    </div>
                                    <input type="hidden" runat="server" id="txt_brgyID" />

                                    <div class="form-group col-12 col-md-6">
                                        <label id="lbl_mainBrgy">Location</label>
                                        <%--checkValue--%>
                                        <select id="dl_brgycity" runat="server" class="form-control checkValue select2 required"> 
                                            <option value="">-</option>
                                        </select>
                                    </div>

                                    <div class="form-group col-12">
                                        <label>Detailed Address</label>
                                        <div>
                                            <textarea class="form-control checkValue" id="txt_Address" runat="server"></textarea>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label>Remarks</label><small style="font-style: italic"> (optional)</small>
                                    <div>
                                        <textarea id="txt_remarks" maxlength="200" class="form-control checkValue" runat="server"></textarea>
                                    </div>
                                </div>
                            </div>

                            <div class="tab-pane" id="tab_timeline">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:PlaceHolder ID="ph_timeline" runat="server"></asp:PlaceHolder>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12">
                                <label>Status</label>
                                <select id="select_status" runat="server" class="form-control checkValue select2 required">
                                    <option value="">- Select Action -</option>
                                    <option value="0">Inactive</option>
                                    <%--<option value="2">Cancel</option>--%>
                                    <option value="1">Active</option>
                                </select>
                            </div>
                        </div>
                    </div>

                    <div class="card-footer text-right">
                        <button type="button" id="btn_back" class="btn btn-default pull-left" onclick="refreshPage();">
                            <i class="fa fa-list"></i> Back to List
                        </button>
                        <button type="button" class="btn btn-success" onclick="if (!validateForm('required')) return;"
                            onserverclick="Save_Click" visible="false" runat="server" id="btn_submit">
                            Submit <i class="fa fa-save"></i>
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modal_cancel">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Deactivate Account</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <div>
                            <textarea id="txt_closeReason" maxlength="200" placeholder="Reason" class="form-control cancelrequired"
                                runat="server"></textarea>
                        </div>
                    </div>
                </div>
                <div class="modal-footer justify-content-between">
                    <button type="button" class="btn btn-outline-dark" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-outline-success" onsubmit="return validateMe()"
                        onclick="if (!validateForm('cancelrequired')) return;"
                        onserverclick="Cancel_Click" runat="server">
                        Submit</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modal_reactivate">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Reactivate Account</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <div>
                            <textarea id="txt_reactivateRemarks" maxlength="200" placeholder="Reason" class="form-control reactivaterequired"
                                runat="server"></textarea>
                        </div>
                    </div>
                </div>
                <div class="modal-footer justify-content-between">
                    <button type="button" class="btn btn-outline-dark" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-outline-success" onclick="if (!validateForm('reactivaterequired')) return;"
                        onserverclick="Reactivate_Click" runat="server">
                        Submit</button>
                </div>
            </div>
        </div>
    </div>

    <%--<div class="modal fade" id="searchModal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Search Resident</h5>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <input type="text" id="searchBox" class="form-control" placeholder="Search by name..."
                        onkeyup="searchResident()">
                    <table class="table table-bordered mt-2">
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Action</th>
                            </tr>
                        </thead>
                        <tbody id="residentList">
                            <!-- Residents will be loaded here -->
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>--%>

    <div class="modal fade" id="modal_search">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="h4_search">Citizen Profile</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <input type="text" class="form-control" id="txt_keywordSearch" placeholder="Enter Name" />
                    </div>

                    <div class="form-group d-none" id="div_search">
                        <hr />
                        <div class="table-responsive">
                            <table id="tbl_search" class="table table-bordered table-striped">
                                <thead>
                                    <tr>
                                        <th>Particulars</th>
                                        <th>Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <!-- This should be inside the modal -->
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="modal-footer justify-content-between">
                    <button type="button" class="btn btn-outline-dark" data-dismiss="modal">Close</button>
                    <a href="#tab_details" data-bs-toggle="modal" data-bs-target="#modal_addProfile"> Not able to locate the profile? Add new here</a>
                </div>
            </div>
        </div>
    </div>

    <%--moodal serach senior citizen--%>
    <div class="modal fade" id="modal_citizenSearch">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Search Profile</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body pb-0">
                    <div class="row">
                        <div class="col-6">
                            <div class="form-group">
                                <input type="text" class="form-control" id="txt_keywordCitizenSearch" placeholder="Enter Name (min 3 characters)" />
                            </div>
                            <div class="form-group d-none" id="div_citizenSearch">
                                <hr />
                                <%--<div class="d-flex justify-content-center align-items-center" id="loading" style="height: 100px;
                                    display: none;">
                                    <img src="Images/loading__.gif" alt="Loading..." style="width: 70px; height: 70px;" />
                                </div>--%>
                                <div class="table-responsive">
                                    <table id="tbl_citizenSearch" class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Particulars</th>
                                                <th>Action</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="small-box">
                                <div class="inner">
                                    <div class="form-group">
                                        <label>ID to print:</label>
                                        <div class="table-responsive">
                                            <table class="table table-bordered table-striped" id="tbl_citizenItems">
                                                <thead>
                                                    <tr>
                                                        <th>Particulars</th>
                                                        <th>Actions</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <!-- Selected profiles will appear here -->
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer d-flex justify-content-between">
                    <button type="button" class="btn btn-outline-dark" data-dismiss="modal">Close</button>
                    <button type="button" id="genBtn" class="btn btn-info" disabled>
                        <i class="fas fa-external-link-alt"></i>&nbsp;Generate
                    </button>
                </div>
            </div>
        </div>
    </div>


    <script>
        $(document).ready(function () {
            // ======================== INITIALIZATION ========================
            initializeFormValidation();
            initializeSearchFunctionality();
            initializeUIComponents();
            initializeEventHandlers();
            performInitialLoad();
        });

        // ======================== FORM VALIDATION INITIALIZATION ========================
        function initializeFormValidation() {
            // Set initial values for all checkValue elements
            getCitizenInitialValues();

            // Detect changes in real-time
            $(".checkValue").on("change input", validateMe);
        }

        function getCitizenInitialValues() {
            $('.checkValue').each(function () {
                let value;
                const $element = $(this);

                if ($element.is('select')) {
                    if ($element.hasClass('select2-hidden-accessible')) {
                        // Handle Select2 elements
                        const select2Data = $element.select2('data');
                        value = select2Data && select2Data[0] ? select2Data[0].text.trim() : '';
                    } else {
                        // Handle regular select elements
                        value = $element.find('option:selected').text().trim();
                    }
                } else {
                    // Handle input/textarea elements
                    value = $element.val() ? $element.val().trim() : '';
                }

                // Store the initial value and field name
                $element.data('initialValue', value);
                $element.data('_name', $element.attr("name") || $element.attr("id"));
            });
        }

        // ======================== SEARCH FUNCTIONALITY ========================
        function initializeSearchFunctionality() {
            let searchTimeout;
            const whatButton = "Senior Citizen";

            // Citizen search with debouncing
            $('#txt_keywordCitizenSearch').on('keyup', function () {
                clearTimeout(searchTimeout);

                searchTimeout = setTimeout(() => {
                    const rawInput = $(this).val();
                    const keyword = rawInput.trim().replace(/\s+/g, ' ');

                    if (keyword.length >= 3) {
                        performCitizenSearch(keyword, whatButton);
                    } else {
                        $('#div_citizenSearch').addClass('d-none');
                        hideLoading();
                    }
                }, 300);
            });

            // Modal search functionality
            $("#txt_keywordSearch").off("input").on("input", function () {
                const keyword = this.value.trim();

                if (keyword.length >= 3) {
                    performModalSearch(keyword);
                } else {
                    clearSearchResults();
                }
            });
        }

        // ======================== UI COMPONENTS INITIALIZATION ========================
        function initializeUIComponents() {
            // Age range slider
            $("#age-range-slider").slider({
                range: true,
                min: 60,
                max: 100,
                values: [60, 100],
                slide: function (event, ui) {
                    $("#dl_filterAge").val(ui.values[0] + "-" + ui.values[1]);
                    $("#age-range-display").text(ui.values[0] + " - " + ui.values[1]);
                },
                change: function (event, ui) {
                    $("#dl_filterAge").trigger("change");
                }
            });

            // Date picker initialization
            $('.lastDateNow').datetimepicker({
                maxDate: new Date(),
                format: 'L',
                autoclose: 1
            });

            // Load dropdown options
            loadBaranggays();

            // Set initial dropdown values
            const dlType = $("#<%=dl_type.ClientID%>").val();
            if (dlType == 2) {
                $('#lbl_mainBrgy').text('City');
            }

            // Load dropdown list and then get initial values
            loadDL(1, dlType, function () {
                getCitizenInitialValues();
            });

            // Populate attachments if needed
            populateAttachment();

            // Set status dropdown
            const statusVal = $("#select_status").val();
            if (statusVal !== "") {
                $("#select_status").val(statusVal).trigger('change.select2');
            }
        }

        // ======================== EVENT HANDLERS ========================
        function initializeEventHandlers() {
            // Birth date change handler
            $('#div_mainbDate').on('change.datetimepicker', handleBirthDateChange);

            // Age filter change
            $('#dl_filterAge').on('change', handleAgeFilterChange);

            // Dropdown change handlers
            $('#dl_priReq').on('change', function () {
                $("#<%=txt_priReq.ClientID%>").val($(this).val().join(','));
        });

        $('#dl_secReq').on('change', function () {
            $("#<%=txt_secReq.ClientID%>").val($(this).val().join(','));
        });

        $("#<%=dl_brgycity.ClientID%>").on('change', function () {
            $("#<%=txt_brgycity.ClientID%>").val(this.value);
        });

        $("#<%=dl_type.ClientID%>").on('change', handleTypeChange);

        // Profile selection handlers
        $(document).on('click', '.btn-select-profile', handleProfileSelection);
        $(document).on('click', '.btn-remove-row', handleProfileRemoval);

        // Generate ID handler
        $('#genBtn').on('click', handleGenerateID);

        // Add new button handler
        $('#btn_addNew').on('click', handleAddNew);

        // Enter key handler
        $(document).on('keydown', function (e) {
            if (e.keyCode === 13 && $("#<%=txt_refID.ClientID%>").val() === '') {
                $('#btn_searchKeyword').click();
            }
        });

            // Contact number validation
            $('#txt_contactNo').on('input', validateContactNo);
        }

        // ======================== INITIAL DATA LOAD ========================
        function performInitialLoad() {
            hideLoading();

            if ($('#<%=txt_refID.ClientID%>').val() === '') {
                generateImportTable(0);
            }
        }

        // ======================== SEARCH HELPER FUNCTIONS ========================
        function performCitizenSearch(keyword, whatButton) {
            showLoading();

            $.ajax({
                type: "POST",
                url: documentName + "/ViewSearch",
                data: JSON.stringify({ keyword: keyword, whatButton: whatButton }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    const table = $('#tbl_citizenSearch').DataTable();
                    table.clear().destroy();
                    $('#tbl_citizenSearch tbody').html(response.d);
                    $('#tbl_citizenSearch').DataTable();
                    $('#div_citizenSearch').removeClass('d-none');
                },
                error: function (xhr) {
                    console.error("Error:", xhr.responseText);
                },
                complete: function () {
                    hideLoading();
                }
            });
        }

        function performModalSearch(keyword) {
            const requestData = { keyword: keyword, profileType: '' };
            console.log("🔄 Sending AJAX request:", requestData);

            showLoading();

            if ($.fn.DataTable.isDataTable("#tbl_search")) {
                $("#tbl_search").DataTable().destroy();
                $("#tbl_search tbody").empty();
            }

            $.ajax({
                type: "POST",
                url: documentName + "/ViewCitizenSearch",
                data: JSON.stringify(requestData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    console.log("✅ Response received:", response);

                    if (response.d.includes("Error:")) {
                        console.error("❌ Server Error:", response.d);
                        systemMsg(0, response.d);
                    } else {
                        $("#tbl_search tbody").html(response.d);
                        $("#div_search").removeClass("d-none");
                        $("#tbl_search").show();

                        $("#tbl_search").DataTable({
                            order: [[0, "desc"]],
                            destroy: true,
                            searching: true,
                            stateSave: false
                        });

                        console.log("✅ Citizen search table updated!");
                    }
                    hideLoading();
                },
                error: function (xhr) {
                    console.error("❌ AJAX Error:", xhr.responseText);
                    systemMsg(0, xhr.responseText);
                    hideLoading();
                }
            });
        }

        function clearSearchResults() {
            if ($.fn.DataTable.isDataTable("#tbl_search")) {
                $("#tbl_search").DataTable().destroy();
            }
            $("#tbl_search tbody").empty();
            $("#div_search").addClass("d-none");
        }

        // ======================== EVENT HANDLER FUNCTIONS ========================
        function handleBirthDateChange(event) {
            const selectedDate = event.date;

            if (selectedDate) {
                const age = calculateAge(selectedDate.toDate());
                const attachmentDiv = $("#<%=div_attachment.ClientID%>");
            const notarizedForm = $("#<%=dl_notarizedFormWithID.ClientID%>");
            const fullBodyID = $("#<%=dl_fullBodyID.ClientID%>");

                if (age >= 80 && age <= 100) {
                    attachmentDiv.removeClass('hide');
                    notarizedForm.addClass('required');
                    fullBodyID.addClass('required');
                } else if (age < 80) {
                    attachmentDiv.addClass('hide');
                    notarizedForm.removeClass('required');
                    fullBodyID.removeClass('required');
                }
            }
            validateAge();
        }

        function handleAgeFilterChange() {
            const input = $(this).val();
            const pattern = /^(6[0-9]|7[0-9]|8[0-9]|9[0-9]|100)-(\d{2,3})$/;

            if (pattern.test(input)) {
                generateImportTable(0);
            } else {
                systemMsg(0, 'Enter age range as 60-100.');
                $(this).val('');
            }
        }

        function handleTypeChange() {
            $('#lbl_mainBrgy').text(this.value == 2 ? 'City' : 'Baranggay');
            loadDL(0, this.value);
        }

        function handleProfileSelection() {
            const refId = $(this).data('refid');
            const profileName = $(this).data('profilename');
            const birthdate = $(this).data('bdate');
            const address = $(this).data('address');

            if ($(`.profileRow[data-refid="${refId}"]`).length) {
                toastr.error('This profile is already selected.', '', { positionClass: 'toast-bottom-right' });
                return;
            }

            const profileRow = `
        <tr class="profileRow" data-refid="${refId}">
            <td>
                <strong>Name:</strong> ${profileName}<br/>
                <strong>Birthdate:</strong> ${birthdate}<br/>
                <strong>Address:</strong> ${address}<br/>
            </td>
            <td class="text-center">
                <button type="button" class="btn btn-danger btn-sm btn-remove-row" data-refid="${refId}">
                    <i class="fas fa-minus"></i>
                </button>
            </td>
        </tr>`;

            $('#tbl_citizenItems tbody').prepend(profileRow);
            $('#genBtn').prop('disabled', false);
        }

        function handleProfileRemoval() {
            $(this).closest('tr').remove();
            $('#genBtn').prop('disabled', $('#tbl_citizenItems tbody tr.profileRow').length === 0);
        }

        function handleGenerateID() {
            const profileList = $('#tbl_citizenItems tbody tr.profileRow').map(function () {
                return { col1: $(this).data('refid') };
            }).get();

            showLoading();

            $.ajax({
                type: "POST",
                url: documentName + "/genID",
                data: JSON.stringify({ profileList: JSON.stringify(profileList), whatButton: "Senior Citizen" }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d) {
                        const w = window.open("", '_blank');
                        w.document.write(response.d);
                        w.document.close();
                    }
                },
                error: function (xhr) {
                    console.error("Error:", xhr.responseText);
                },
                complete: function () {
                    hideLoading();
                }
            });
        }

        function handleAddNew(e) {
            e.preventDefault();
            $('#div_details').find('input, select, textarea, button')
                .not('#btn_back, [onclick*="ViewSearch"]')
                .prop('disabled', true);

            $('#div_details').find('button[onclick*="ViewSearch"]').prop('disabled', false);
            $('#modal_search').modal('show');
        }

        // ======================== DATA LOADING FUNCTIONS ========================
        function loadBaranggays() {
            $.ajax({
                type: "POST",
                url: documentName + "/getLocations",
                data: JSON.stringify({ type: "1" }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    const $dropdown = $('#dl_filterBrgy');
                    $dropdown.empty().append('<option value="">All</option>');

                    if (response.d && response.d.length > 0) {
                        $.each(response.d, function () {
                            $dropdown.append($("<option></option>").val(this['Value']).html(this['Text']));
                        });
                    }

                    if ($dropdown.hasClass('select2-hidden-accessible')) {
                        $dropdown.trigger('change.select2');
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error loading baranggays:", error);
                    loadStaticBaranggays();
                }
            });
        }

        function loadStaticBaranggays() {
            const staticOptions = [
                { Value: "9", Text: "AGUADO" },
                { Value: "1", Text: "CABEZES" },
                { Value: "2", Text: "CABUCO" },
                { Value: "7", Text: "CONCHU" },
                { Value: "3", Text: "DE OCAMPO" },
                { Value: "10", Text: "GREGORIO" },
                { Value: "8", Text: "HUGO PEREZ" },
                { Value: "11", Text: "INOCENCIO" },
                { Value: "4", Text: "LALLANA" },
                { Value: "12", Text: "LAPIDARIO" },
                { Value: "13", Text: "LUCIANO" },
                { Value: "6", Text: "OSORIO" },
                { Value: "5", Text: "SAN AGUSTIN" }
            ];

            const $dropdown = $('#dl_filterBrgy');
            $dropdown.empty().append('<option value="">All</option>');

            $.each(staticOptions, function () {
                $dropdown.append($("<option></option>").val(this.Value).html(this.Text));
            });
        }

        // ======================== FORM VALIDATION FUNCTIONS ========================
        function validateMe() {
            let jsonChanged = [];

            $('.checkValue').each(function () {
                let newVal, initialVal;

                if ($(this).attr('test') === 'profilePicture') {
                    let splitProfile = $(this).val().split('\\');
                    newVal = splitProfile.length > 0 ? splitProfile[splitProfile.length - 1] : "";
                    initialVal = $('#<%=txt_pictureName.ClientID%>').text().trim();
            } else if ($(this).prop("multiple")) {
                if ($(this).is("select")) {
                    newVal = $(this).find("option:selected").map(function () { return $(this).text(); }).get().join(", ");
                } else {
                    newVal = $(this).val() ? $(this).val().sort().join(",") : "";
                }
                initialVal = $(this).data('initialValue') || "";
                if (Array.isArray(initialVal)) initialVal = initialVal.sort().join(",");
            } else if ($(this).is("select")) {
                newVal = $(this).find("option:selected").text();
                initialVal = $(this).data('initialValue') || "";
            } else {
                newVal = $(this).val() ? $(this).val().trim() : "";
                initialVal = $(this).data('initialValue') ? $(this).data('initialValue').trim() : "";
            }

            if (newVal !== initialVal) {
                jsonChanged.push({
                    field: $(this).attr('test') || $(this).attr('id'),
                    oldValue: initialVal,
                    newValue: newVal,
                });
            }
        });

        if ($('.required').length > 0 && $('.required').valid() && jsonChanged.length > 0) {
            $('#<%=txt_updates.ClientID%>').val(JSON.stringify(jsonChanged));
            //console.log("Updated JSON:", JSON.stringify(jsonChanged));
            return true;
        } else {
            $('#<%=txt_updates.ClientID%>').val('');
                return false;
            }
        }

        // ======================== PROFILE MANAGEMENT FUNCTIONS ========================
        function ViewSearch() {
            const profileType = '';
            const whatButton = "Senior Citizen";

            $("#h4_search").text("Search Senior Citizen Profile");
            $("#txt_keywordSearch").val("").attr("placeholder", "Enter Name");
            $("#div_search").addClass("d-none");
            $("#modal_search").modal("show");
            $("#tbl_search tbody").empty();
        }

        function SelectCitizenProfile(data) {
            try {
                if (typeof data !== "string") {
                    console.error("Invalid data type:", data);
                    return;
                }

                const citizenData = JSON.parse(data);
                console.log("Received Citizen Data:", citizenData);

                $.ajax({
                    type: "POST",
                    url: documentName + "/CheckSeniorCitizenExists",
                    data: JSON.stringify({ refID: citizenData.RefID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d.exists) {
                            if (!confirm("WARNING: This citizen is already registered in the senior citizen database. Do you want to proceed anyway?")) {
                                return;
                            }
                        }

                        // Populate form fields
                        populateFormFields(citizenData);

                        // Set read-only fields
                        setReadOnlyFields();

                         //Update initial values after populating form
                        getCitizenInitialValues();

                        $("#modal_search").modal("hide");
                    },
                    error: function (xhr, status, error) {
                        console.error("Error checking senior citizen status:", error);
                        systemMsg(0, "Error verifying citizen status. Please try again.");
                    }
                });
            } catch (err) {
                console.error("Error parsing citizen data:", err, data);
                systemMsg(0, "Invalid profile data. Please try again.");
            }
        }

        function populateFormFields(citizenData) {
            $("#<%=txt_ownerID.ClientID%>").val(citizenData.RefID || "");
        $("#<%=txt_updates.ClientID%>").val(citizenData.RefID || "");
        $("#<%=txt_ownerName.ClientID%>").val(`${citizenData.fName || ""} ${citizenData.mName || ""} ${citizenData.lName || ""} ${citizenData.suffix || ""}`.trim());
        $("#<%=txt_FName.ClientID%>").val(citizenData.fName || "");
        $("#<%=txt_LName.ClientID%>").val(citizenData.lName || "");
        $("#<%=txt_MName.ClientID%>").val(citizenData.mName || "");
        $("#<%=txt_BPLace.ClientID%>").val(citizenData.BPlace || "");
        $("#<%=txt_contactNo.ClientID%>").val(citizenData.contactNo || "");
        $("#<%=txt_Address.ClientID%>").val(citizenData.address || "");
        $("#<%=txt_ncscRRN.ClientID%>").val(citizenData.ncscrrn || "");
        $("#<%=txt_Citizenship.ClientID%>").val(citizenData.Citizenship || "");

        const birthdate = citizenData.bdate ? citizenData.bdate.split(" ")[0] : "";
        $("#<%=txt_BDate.ClientID%>").val(birthdate);

        const height = citizenData.height ? citizenData.height.replace(",", ".") : "";
        const weight = citizenData.weight ? citizenData.weight.replace(",", ".") : "";
        $("#<%=txt_Height.ClientID%>").val(height);
        $("#<%=txt_Weight.ClientID%>").val(weight);

        // Trigger change events for dropdowns
        $("#<%=dl_suffix.ClientID%>").val((citizenData.suffix || "").trim()).trigger("change");
        $("#<%=dl_sex.ClientID%>").val((citizenData.sex || "").trim()).trigger("change");
        $("#<%=dl_civilStatus.ClientID%>").val((citizenData.civilStatus || "").trim()).trigger("change");
        $("#<%=dl_dswdPentioner.ClientID%>").val((citizenData.DSWDPensioner || "").trim()).trigger("change");
        $("#<%=select_status.ClientID%>").val((citizenData.status || "").trim()).trigger("change");
        $("#<%=txt_brgycity.ClientID%>").val((citizenData.locationID || "").trim()).trigger("change");
    }

    function setReadOnlyFields() {
        const readOnlyFields = [
            "#<%=  txt_FName.ClientID %>",
            "#<%= txt_LName.ClientID %>",
            "#<%= txt_MName.ClientID %>",
            "#<%= txt_BDate.ClientID %>"
        ];

        readOnlyFields.forEach(field => {
            $(field).prop("readOnly", true);
        });
    }

    function populateAttachment() {
        if ($("#<%=populateMultiple.ClientID%>").val() === '1') {
            const primaryValue = $("#<%=txt_priReq.ClientID%>").val();
            const secondaryValue = $("#<%=txt_secReq.ClientID%>").val();

            if (primaryValue) {
                const primaryReqs = primaryValue.split(',');
                $('#dl_priReq').val(primaryReqs).trigger('change');
            }

            if (secondaryValue) {
                const secondaryReqs = secondaryValue.split(',');
                $('#dl_secReq').val(secondaryReqs).trigger('change');
            }
        }
    }

    // ======================== UTILITY FUNCTIONS ========================
    function calculateAge(birthDate) {
        const today = new Date();
        return today.getFullYear() - birthDate.getFullYear();
    }

    function validateAge() {
        const input = $('#<%=txt_BDate.ClientID%>').val();
        if (input) {
            const birthDate = moment(input, 'MM/DD/YYYY');
            const now = moment();
            const age = now.diff(birthDate, 'years');

            if (age < 60) {
                systemMsg(0, 'The age must be at least 60 years old.');
                $('#<%=txt_BDate.ClientID%>').val('');
            }
        }
    }

    function generateImportTable(mode) {
        const status = $('#dl_filterStatus').val();
        const brgy = $('#dl_filterBrgy').val();
        const age = $('#dl_filterAge').val();
        const DSWDPensioner = $('#dl_DSWDPensioner').val();
        const keyword = $('#txt_filterSearch').val();
        const sex = $('#dl_filterSex').val();

        if ($('#<%=txt_MName.ClientID%>').val() === '') {
            $.ajax({
                type: "POST",
                url: documentName + "/LoadList",
                data: JSON.stringify({ mode, status, age, brgy, DSWDPensioner, sex, keyword }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    if (r.d.includes('Error:')) {
                        systemMsg(0, r.d);
                    } else {
                        const table = $('#example1').DataTable();
                        table.clear().destroy();
                        $("#example1").append(r.d);
                        $('#example1').DataTable({
                            stateSave: true,
                            order: [[0, "desc"]],
                            responsive: true,
                            lengthChange: true,
                            autoWidth: false,
                            buttons: ["copy", "excel", "print"]
                        }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');
                    }
                },
                error: function (request, status, error) {
                    systemMsg(0, request.responseText);
                }
            });
        }
    }

    function validateCitizen1(obj) {
        const fName = $('#<%=txt_BDate.ClientID%>').val();
        const lName = $('#<%=populateMultiple.ClientID%>').val();
        const bDate = $('#<%=txt_priReq.ClientID%>').val();

        if (fName && lName && bDate) {
            $.ajax({
                type: "POST",
                url: documentName + "/ValidateProfile",
                data: JSON.stringify({ fName, lName, bDate }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    const results = r.d;

                    if (results.Error !== undefined) {
                        systemMsg(0, results.Error);
                    } else if (results.Output === "1") {
                        systemMsg(0, "Entry record already exists.");
                        if (obj.value === undefined) {
                            $('#<%=txt_secReq.ClientID%>').val("").focus();
                        } else {
                            obj.value = "";
                            obj.focus();
                        }
                    }
                }
            });
        }
    }

        function loadDL(mode, val, callback) {
            const dlMainLocation = $("#<%=dl_brgycity.ClientID%>");
        dlMainLocation.empty().append('<option selected="selected" value="">Please select</option>');

        if (val) {
            $.ajax({
                type: "POST",
                url: documentName + "/getLocations",
                data: JSON.stringify({ type: val }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    dlMainLocation.empty().append('<option selected="selected" value="">Please select</option>');
                    $.each(r.d, function () {
                        dlMainLocation.append($("<option></option>").val(this['Value']).html(this['Text']));
                    });

                    if (mode === 1) {
                        $("#<%=dl_brgycity.ClientID%>").val($("#<%=txt_brgycity.ClientID%>").val()).trigger("change");
                    }
                    if (typeof callback === 'function') callback();
                }
            });
        }
    }

        function showLoading() {
            $("#loading").removeClass("d-none").show();
            $("#tbl_search").hide();
            $("#div_search").addClass("d-none");
        }

        function hideLoading() {
            $("#loading").addClass("d-none").hide();
            $("#tbl_search").show();
            $("#div_search").removeClass("d-none");
        }

        function restrictNonNumeric(event) {
            const key = event.keyCode || event.which;

            const allowedKeys = [8, 46, 9]; // backspace, delete, tab
            const arrowKeys = [37, 38, 39, 40]; // arrow keys
            const numKeys = [48, 49, 50, 51, 52, 53, 54, 55, 56, 57]; // 0-9 number keys  
            const numpadKeys = [96, 97, 98, 99, 100, 101, 102, 103, 104, 105]; // numpad 0-9

            if (allowedKeys.includes(key) || arrowKeys.includes(key) || numKeys.includes(key) || numpadKeys.includes(key)) {
                return true;
            } else {
                event.preventDefault();
                return false;
            }
        }

        function validateContactNo() {
            const inputField = document.getElementById("txt_contactNo");
            const errorMessage = document.getElementById("error-message");

            if (!inputField) return;

            const contactNo = inputField.value.replace(/\D/g, '');
            inputField.value = contactNo;

            if (errorMessage) {
                errorMessage.style.display = contactNo.length === 11 ? "none" : "inline";
            }
        }
    </script>



    
</asp:Content>

