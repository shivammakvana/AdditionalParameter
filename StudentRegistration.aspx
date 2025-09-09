<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentRegistration.aspx.cs" Inherits="YourNamespace.StudentRegistration" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Registration</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/js/bootstrap.bundle.min.js"></script>
    <style>
        body {
            font-family: 'Segoe UI', sans-serif;
            font-size: 0.875rem;
            background-color: #f8f9fa;
        }
        .section-heading {
            background-color: #007bff;
            color: white;
            padding: 5px;
            margin-bottom: 10px;
            text-align: center;
        }
        .section-warning {
            background-color: #f39c12;
            color: white;
            padding: 5px;
            margin-bottom: 10px;
            text-align: center;
        }
        .section-warning2 {
            background-color: #8e44ad;
            color: white;
            padding: 5px;
            margin-bottom: 10px;
            text-align: center;
        }
        .section-warning3 {
            background-color: Green;
            color: white;
            padding: 5px;
            margin-bottom: 10px;
            text-align: center;
        }
        .form-label {
            margin-top: 5px;
            margin-bottom: 2px;
            font-weight: 500;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid py-3">

            <!-- Student Info -->
            <div class="row">
                <div class="col-md-3 border-end">
                    <div class="section-heading">Details of Student</div>

                    <div class="mb-3">
                        <asp:Label CssClass="form-label" Text="First Name:" runat="server" />
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <asp:Label CssClass="form-label" Text="Middle Name:" runat="server" />
                        <asp:TextBox ID="txtMiddleName" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <asp:Label CssClass="form-label" Text="Last Name:" runat="server" />
                        <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <asp:Label CssClass="form-label" Text="Date of Birth:" runat="server" />
                        <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control form-control-sm" placeholder="dd-mm-yyyy" TextMode="Date"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <asp:Label CssClass="form-label" Text="Image (Optional):" runat="server" />
                        <asp:FileUpload ID="fuImage" runat="server" CssClass="form-control form-control-sm" />
                    </div>
                </div>

                <!-- Nationality -->
                <div class="col-md-3 border-end">
                    <div class="section-heading">Nationality</div>

                    <div class="mb-3">
                        <asp:Label CssClass="form-label" Text="Nationality:" runat="server" />
                        <asp:TextBox ID="txtNationality" runat="server" CssClass="form-control form-control-sm" Text="Indian"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <asp:Label CssClass="form-label" Text="Religion:" runat="server" />
                        <asp:DropDownList ID="ddlReligion" runat="server" CssClass="form-select form-select-sm">
                            
                        </asp:DropDownList>
                    </div>

                    <div class="mb-3">
                        <asp:Label CssClass="form-label" Text="Caste Category:" runat="server" />
                        <asp:DropDownList ID="ddlCasteCategory" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                    </div>

                    <div class="mb-3">
                        <asp:Label CssClass="form-label" Text="Caste:" runat="server" />
                        <asp:DropDownList ID="ddlCaste" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                    </div>

                    <div class="mb-3">
                        <asp:Label CssClass="form-label" Text="Aadhar Number:" runat="server" />
                        <asp:TextBox ID="txtAadhar" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <asp:Label CssClass="form-label" Text="Gender:" runat="server" />
                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select form-select-sm">
                            
                        </asp:DropDownList>
                    </div>
                </div>

                <!-- Class -->
                <div class="col-md-3 border-end">
                    <div class="section-heading">Class</div>

                    <div class="mb-3">
                        <asp:Label CssClass="form-label" Text="Adm No.:" runat="server" />
                        <asp:TextBox ID="txtAdmNo" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <asp:Label CssClass="form-label" Text="Class:" runat="server" />
                        <asp:DropDownList ID="ddlClass" runat="server" CssClass="form-select form-select-sm">
                   
                        </asp:DropDownList>
                    </div>

                    <div class="mb-3">
                        <asp:Label CssClass="form-label" Text="Section:" runat="server" />
                        <asp:DropDownList ID="ddlSection" runat="server" CssClass="form-select form-select-sm">
                             <asp:ListItem Value="">-- Select Section --</asp:ListItem>
                            <asp:ListItem Value="A">A</asp:ListItem>
                            <asp:ListItem Value="B">B</asp:ListItem>
                            <asp:ListItem Value="C">C</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="mb-3">
                        <asp:Label CssClass="form-label" Text="Roll No.:" runat="server" />
                        <asp:TextBox ID="txtRollNo" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <asp:Label CssClass="form-label" Text="Year of Term:" runat="server" />
                        <asp:TextBox ID="txtYearTerm" runat="server" CssClass="form-control form-control-sm" ></asp:TextBox>
                    </div>
                </div>

                <!-- Image Preview -->
                <div class="col-md-3">
                    <div class="section-heading">Image</div>
                    <img src="~/images/no-image.png" alt="Image Not Available" class="img-thumbnail mt-4" />
                </div>
            </div>


        <div class="row mt-4">
            <!-- Guardian -->
            <div class="col-md-4 border-end">
                <div class="section-warning">Guardian</div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Father Name:" runat="server" />
                    <asp:TextBox ID="txtFatherName" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Mother Name:" runat="server" />
                    <asp:TextBox ID="txtMotherName" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Guardian Name:" runat="server" />
                    <asp:TextBox ID="txtGuardianName" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Relation:" runat="server" />
                    <asp:TextBox ID="txtRelation" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>
            </div>

            <!-- Occupation -->
            <div class="col-md-4 border-end">
                <div class="section-warning">Occupation</div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Occupation:" runat="server" />
                    <asp:TextBox ID="txtOccupation" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Address:" runat="server" />
                    <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Email:" runat="server" />
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>
            </div>

            <!-- Bus -->
            <div class="col-md-4">
                <div class="section-warning">Bus</div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Bus Route:" runat="server" />
                    <asp:DropDownList ID="ddlBusRoute" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                </div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Bus Stop:" runat="server" />
                    <asp:DropDownList ID="ddlBusStop" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                </div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Contact No:" runat="server" />
                    <asp:TextBox ID="txtContactNo" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Mobile:" runat="server" />
                    <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>
            </div>
        </div>

        <div class="row mt-4">
            <!-- Miscellaneous -->
            <div class="col-md-4 border-end">
                <div class="section-warning2">Miscellaneous</div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Date of Admission:" runat="server" />
                        <asp:TextBox ID="txtDateAddmission" runat="server" CssClass="form-control form-control-sm" placeholder="dd-mm-yyyy" TextMode="Date"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Admission Session YYYY:" runat="server" />
                    <asp:TextBox ID="txtAdmissionSession" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Class IX Roll No.:" runat="server" />
                    <asp:TextBox ID="txtClassIX" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Class X Roll No.:" runat="server" />
                    <asp:TextBox ID="txtClassX" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Class XI Roll No.:" runat="server" />
                    <asp:TextBox ID="txtClassXI" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Class XII Roll No.:" runat="server" />
                    <asp:TextBox ID="txtClassXII" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>

                <div class="form-check">
                    <asp:CheckBox ID="chkHostel" runat="server" CssClass="form-check-input" />
                    <asp:Label CssClass="form-check-label" Text="Require Hostel" runat="server" />
                </div>
            </div>

            <!-- Speciality -->
            <div class="col-md-4 border-end">
                <div class="section-warning2">Speciality</div>

                <div class="form-check">
                    <asp:CheckBox ID="chkRTE" runat="server" CssClass="form-check-input" />
                    <asp:Label CssClass="form-check-label" Text="RTE" runat="server" />
                </div>

                <div class="form-check">
                    <asp:CheckBox ID="chkMinority" runat="server" CssClass="form-check-input" />
                    <asp:Label CssClass="form-check-label" Text="Minority" runat="server" />
                </div>

                <div class="form-check">
                    <asp:CheckBox ID="chkBPL" runat="server" CssClass="form-check-input" />
                    <asp:Label CssClass="form-check-label" Text="BPL Category" runat="server" />
                </div>

                <div class="form-check">
                    <asp:CheckBox ID="chkDisabled" runat="server" CssClass="form-check-input" />
                    <asp:Label CssClass="form-check-label" Text="Disabled" runat="server" />
                </div>
            </div>

            <!-- School Last Attended -->
            <div class="col-md-4">
                <div class="section-warning2">School Last Attended</div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="School Name:" runat="server" />
                    <asp:TextBox ID="txtSchoolName" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Transfer Certificate Received:" runat="server" />
                    <asp:DropDownList ID="ddlTransferCert" runat="server" CssClass="form-select form-select-sm">
                        <asp:ListItem>No</asp:ListItem>
                        <asp:ListItem>Yes</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Migration Certificate Received:" runat="server" />
                    <asp:DropDownList ID="ddlMigrationCert" runat="server" CssClass="form-select form-select-sm">
                        <asp:ListItem>No</asp:ListItem>
                        <asp:ListItem>Yes</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Birth Certificate Received:" runat="server" />
                    <asp:DropDownList ID="ddlBirthCert" runat="server" CssClass="form-select form-select-sm">
                        <asp:ListItem>No</asp:ListItem>
                        <asp:ListItem>Yes</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Marksheet Certificate Received:" runat="server" />
                    <asp:DropDownList ID="ddlMarksheetCert" runat="server" CssClass="form-select form-select-sm">
                        <asp:ListItem>No</asp:ListItem>
                        <asp:ListItem>Yes</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="mb-3">
                    <asp:Label CssClass="form-label" Text="Caste Certificate Received:" runat="server" />
                    <asp:DropDownList ID="ddlCasteCert" runat="server" CssClass="form-select form-select-sm">
                        <asp:ListItem>No</asp:ListItem>
                        <asp:ListItem>Yes</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
                    <!-- Health Status -->
        <div class="row mt-4">
            <div class="col-md-12">
                <div class="section-warning3">Health Status</div>

                <div class="row">
                    <div class="col-md-2">
                        <asp:Label CssClass="form-label" Text="Height (cms):" runat="server" />
                        <asp:TextBox ID="txtHeight" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>

                    <div class="col-md-2">
                        <asp:Label CssClass="form-label" Text="Weight (kgs):" runat="server" />
                        <asp:TextBox ID="txtWeight" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>

                    <div class="col-md-2">
                        <asp:Label CssClass="form-label" Text="Vision (L):" runat="server" />
                        <asp:TextBox ID="txtVisionL" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>

                    <div class="col-md-2">
                        <asp:Label CssClass="form-label" Text="Vision (R):" runat="server" />
                        <asp:TextBox ID="txtVisionR" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>

                    <div class="col-md-2">
                        <asp:Label CssClass="form-label" Text="Blood Group:" runat="server" />
                        <asp:TextBox ID="txtBloodGroup" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>

                    <div class="col-md-2">
                        <asp:Label CssClass="form-label" Text="Dental Hygiene:" runat="server" />
                        <asp:TextBox ID="txtDentalHygiene" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
                </div>
                                    <div class="section-heading">Additional Parameter</div>
                <asp:PlaceHolder ID="paraform" runat="server"></asp:PlaceHolder>

                   <div class="form-group">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" Style="background-color: #28a745; color: white; padding: 8px 15px; border: none; border-radius: 5px; margin-right: 8px;" />        
                       <asp:Button ID="btnDelete" runat="server" Style="background-color: #dc3545; color: white; padding: 8px 15px; border: none; border-radius: 5px; margin-right: 8px;" Text="Delete" />
        </div>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>

              </form>
</body>
</html>

