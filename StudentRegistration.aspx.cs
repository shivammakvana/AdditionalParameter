using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Org.BouncyCastle.Crypto.Agreement.Kdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static System.Collections.Specialized.BitVector32;

namespace YourNamespace
{
    public partial class StudentRegistration : System.Web.UI.Page
    {
        string connStr = "Server=192.168.10.196;Port=3306;Database=demo_db;Uid=root;Pwd=console123;";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindAdmissionNumber();
                BindReligion();
                BindCasteCategory();
                BindCaste();
                BindGender();
                BindClass();
                BindBusRoute();
                BindBusStop();

                DynamicGroupsGeneration();
            }
            else
            {
                DynamicGroupsGeneration();
            }
        }

        private void BindAdmissionNumber()
        {
            string query = @"SELECT DISTINCT scholar_branch_registration_id 
                     FROM scholar_register 
                     WHERE TRIM(scholar_branch_registration_id) <> '' 
                     ORDER BY scholar_branch_registration_id DESC";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    ddlAdmissionNumber.DataSource = reader;
                    ddlAdmissionNumber.DataTextField = "scholar_branch_registration_id";
                    ddlAdmissionNumber.DataValueField = "scholar_branch_registration_id";
                    ddlAdmissionNumber.DataBind();
                }
            }

            ddlAdmissionNumber.Items.Insert(0, new ListItem("----Select----", ""));
        }

        private void BindReligion()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "SELECT DISTINCT religion FROM scholar_register WHERE religion IS NOT NULL";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    ddlReligion.DataSource = cmd.ExecuteReader();
                    ddlReligion.DataTextField = "religion";
                    ddlReligion.DataValueField = "religion";
                    ddlReligion.DataBind();
                }
            }
            ddlReligion.Items.Insert(0, new ListItem("-- Select Religion --", ""));
        }

        private void BindCasteCategory()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "SELECT DISTINCT caste_category FROM scholar_register WHERE caste_category IS NOT NULL";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    ddlCasteCategory.DataSource = cmd.ExecuteReader();
                    ddlCasteCategory.DataTextField = "caste_category";
                    ddlCasteCategory.DataValueField = "caste_category";
                    ddlCasteCategory.DataBind();
                }
            }
            ddlCasteCategory.Items.Insert(0, new ListItem("-- Select Category --", ""));
        }

        private void BindCaste()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "SELECT DISTINCT caste FROM scholar_register WHERE caste IS NOT NULL";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    ddlCaste.DataSource = cmd.ExecuteReader();
                    ddlCaste.DataTextField = "caste";
                    ddlCaste.DataValueField = "caste";
                    ddlCaste.DataBind();
                }
            }
            ddlCaste.Items.Insert(0, new ListItem("-- Select Caste --", ""));
        }

        private void BindGender()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "SELECT DISTINCT gender FROM scholar_register WHERE gender IS NOT NULL";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    ddlGender.DataSource = cmd.ExecuteReader();
                    ddlGender.DataTextField = "gender";
                    ddlGender.DataValueField = "gender";
                    ddlGender.DataBind();
                }
            }
            ddlGender.Items.Insert(0, new ListItem("-- Select Gender --", ""));
        }

        private void BindClass()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "SELECT class_id, class_description FROM class ORDER BY class_order";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    ddlClass.DataSource = cmd.ExecuteReader();
                    ddlClass.DataTextField = "class_description";
                    ddlClass.DataValueField = "class_id";
                    ddlClass.DataBind();
                }
            }
            ddlClass.Items.Insert(0, new ListItem("-- Select Class --", ""));
        }

        private void BindBusRoute()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = @"SELECT DISTINCT br.route_id, br.route_name FROM scholar_class sc INNER JOIN bus_stop bs ON sc.bus_stop_id = bs.stop_id
                    INNER JOIN bus_route br ON bs.route_id = br.route_id WHERE sc.bus_stop_id IS NOT NULL";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    ddlBusRoute.DataSource = cmd.ExecuteReader();
                    ddlBusRoute.DataTextField = "route_name";
                    ddlBusRoute.DataValueField = "route_id";
                    ddlBusRoute.DataBind();
                }
            }
            ddlBusRoute.Items.Insert(0, new ListItem("-- Select Route --", ""));
        }

        private void BindBusStop()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = @" SELECT DISTINCT bs.stop_id, bs.stop_name FROM scholar_class sc INNER JOIN bus_stop bs ON sc.bus_stop_id = bs.stop_id
                    WHERE sc.bus_stop_id IS NOT NULL";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    ddlBusStop.DataSource = cmd.ExecuteReader();
                    ddlBusStop.DataTextField = "stop_name";
                    ddlBusStop.DataValueField = "stop_id";
                    ddlBusStop.DataBind();
                }
            }
            ddlBusStop.Items.Insert(0, new ListItem("-- Select Stop --", ""));
        }

        private void DynamicGroupsGeneration()
        {
            paraform.Controls.Clear();
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string groupQuery = "SELECT SAFGID, group_name FROM scholar_additional_field_group ORDER BY group_order";
                List<(string SAFGID, string GroupName)> groupList = new List<(string, string)>();

                using (MySqlCommand groupCmd = new MySqlCommand(groupQuery, conn))
                using (MySqlDataReader groupReader = groupCmd.ExecuteReader())
                {
                    while (groupReader.Read())
                    {
                        groupList.Add((
                            groupReader["SAFGID"].ToString(),
                            groupReader["group_name"].ToString()
                        ));
                    }
                }

                for (int i = 0; i < groupList.Count; i += 2)
                {
                    HtmlGenericControl rowDiv = new HtmlGenericControl("div");
                    rowDiv.Attributes["class"] = "row mb-4";

                    for (int j = 0; j < 2 && (i + j) < groupList.Count; j++)
                    {
                        HtmlGenericControl colDiv = new HtmlGenericControl("div");
                        colDiv.Attributes["class"] = "col-md-6 border p-3";

                        Literal groupTitle = new Literal();
                        groupTitle.Text = $"<h6 style='background-color:#0370af; color:white; padding:8px;" + $" text-align:center;'>{groupList[i + j].GroupName}</h6>";
                        colDiv.Controls.Add(groupTitle);

                        DynamicParameterGeneration(groupList[i + j].SAFGID, colDiv);

                        rowDiv.Controls.Add(colDiv);
                    }

                    paraform.Controls.Add(rowDiv);
                }
            }
        }

        private void DynamicParameterGeneration(string groupId, Control container)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string paramQuery = @"SELECT sap.SAPID, sap.para_name, spt.para_type FROM scholar_additional_parameters sap JOIN scholar_para_type spt ON sap.SPTID = spt.SPTID
                    WHERE sap.SAFGID = @groupId ORDER BY sap.order_id";

                using (MySqlCommand cmd = new MySqlCommand(paramQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@groupId", groupId);

                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            string sapid = rd["SAPID"].ToString();
                            string paraName = rd["para_name"].ToString();
                            string paraType = rd["para_type"].ToString();

                            HtmlGenericControl row = new HtmlGenericControl("div");
                            row.Attributes["class"] = "row mb-2";

                            HtmlGenericControl labelCol = new HtmlGenericControl("div");
                            labelCol.Attributes["class"] = "col-md-4 d-flex align-items-center";

                            Label lbl = new Label();
                            lbl.Text = paraName;
                            lbl.AssociatedControlID = "input_" + sapid;
                            lbl.CssClass = "form-label mb-0";
                            labelCol.Controls.Add(lbl);

                            HtmlGenericControl inputCol = new HtmlGenericControl("div");
                            inputCol.Attributes["class"] = "col-md-8";

                            Control inputControl;
                            List<ListItem> optionItems = new List<ListItem>();
                            if (paraType == "DropDownList" || paraType == "CheckBox")
                            {
                                optionItems = GetParameterOp(sapid);
                            }

                            switch (paraType)
                            {
                                case "TextArea":
                                    TextBox txtArea = new TextBox();
                                    txtArea.ID = "input_" + sapid;
                                    txtArea.CssClass = "form-control";
                                    txtArea.TextMode = TextBoxMode.MultiLine;
                                    inputControl = txtArea;
                                    break;

                                default:
                                    TextBox txt = new TextBox();
                                    txt.ID = "input_" + sapid;
                                    txt.CssClass = "form-control";
                                    inputControl = txt;
                                    break;

                                case "DropDownList":
                                    DropDownList ddl = new DropDownList();
                                    ddl.ID = "input_" + sapid;
                                    ddl.CssClass = "form-control";
                                    ddl.Items.Add(new ListItem("--Select--", ""));
                                    ddl.Items.AddRange(optionItems.ToArray());
                                    inputControl = ddl;
                                    break;

                                case "CheckBox":
                                    if (optionItems.Count > 1)
                                    {
                                        CheckBoxList chkList = new CheckBoxList();
                                        chkList.ID = "input_" + sapid;
                                        chkList.CssClass = "form-check";
                                        chkList.RepeatDirection = RepeatDirection.Vertical;
                                        chkList.Items.AddRange(optionItems.ToArray());
                                        inputControl = chkList;
                                    }
                                    else
                                    {
                                        CheckBox chk = new CheckBox();
                                        chk.ID = "input_" + sapid;
                                        inputControl = chk;
                                    }
                                    break;
                            }

                            inputCol.Controls.Add(inputControl);
                            row.Controls.Add(labelCol);
                            row.Controls.Add(inputCol);
                            container.Controls.Add(row);
                        }
                    }
                }
            }
        }

        private List<ListItem> GetParameterOp(string sapid)
        {
            List<ListItem> items = new List<ListItem>();

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string optionQuery = "SELECT option_value FROM scholar_additional_param_option WHERE SAPID = @SAPID";

                using (MySqlCommand cmd = new MySqlCommand(optionQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@SAPID", sapid);
                    using (MySqlDataReader optReader = cmd.ExecuteReader())
                    {
                        while (optReader.Read())
                        {
                            string value = optReader["option_value"].ToString();
                            items.Add(new ListItem(value, value));
                        }
                    }
                }
            }

            return items;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            long newRegId;
                            long SCID;

                            string queryRegister = @"INSERT INTO scholar_register(first_name, middle_name, last_name, date_of_birth, religion, date_of_admission,
                                                 father_name, mother_name, gaurdian_name, relation_with_student, occupation,address, phone, mobile, email_id, nationality, caste,
                                                 tc_received, cc_received, mc_received, marksheet_received, class_tenth_roll_no, class_tweelth_roll_no,
                                                 rte, gender, caste_category, minority, aadhar_no, BPL_category, disabled, yot_adm, scholar_branch_registration_id)
                                                 VALUES(@first_name, @middle_name, @last_name, @date_of_birth, @religion, @date_of_admission, @father_name, @mother_name, @gaurdian_name, @relation_with_student, @occupation,
                                                 @address, @phone, @mobile, @email_id, @nationality, @caste, @tc_received, @cc_received, @mc_received, @marksheet_received,
                                                 @class_tenth_roll_no, @class_tweelth_roll_no, @rte, @gender, @caste_category, @minority, @aadhar_no, @BPL_category, @disabled,
                                                 @yot_adm, @scholar_branch_registration_id)";

                            using (var cmd = new MySqlCommand(queryRegister, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@first_name", txtFirstName.Text.Trim());
                                cmd.Parameters.AddWithValue("@middle_name", txtMiddleName.Text.Trim());
                                cmd.Parameters.AddWithValue("@last_name", txtLastName.Text.Trim());
                                cmd.Parameters.AddWithValue("@date_of_birth", DateTime.Parse(txtDOB.Text.Trim()));
                                cmd.Parameters.AddWithValue("@religion", ddlReligion.SelectedValue);
                                cmd.Parameters.AddWithValue("@date_of_admission", DateTime.Parse(txtDateAddmission.Text.Trim()));
                                cmd.Parameters.AddWithValue("@father_name", txtFatherName.Text.Trim());
                                cmd.Parameters.AddWithValue("@mother_name", txtMotherName.Text.Trim());
                                cmd.Parameters.AddWithValue("@gaurdian_name", txtGuardianName.Text.Trim());
                                cmd.Parameters.AddWithValue("@relation_with_student", txtRelation.Text.Trim());
                                cmd.Parameters.AddWithValue("@occupation", txtOccupation.Text.Trim());
                                cmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
                                cmd.Parameters.AddWithValue("@phone", txtContactNo.Text.Trim());
                                cmd.Parameters.AddWithValue("@mobile", txtMobile.Text.Trim());
                                cmd.Parameters.AddWithValue("@email_id", txtEmail.Text.Trim());
                                cmd.Parameters.AddWithValue("@nationality", txtNationality.Text.Trim());
                                cmd.Parameters.AddWithValue("@caste", ddlCaste.SelectedValue);
                                cmd.Parameters.AddWithValue("@class_tenth_roll_no", txtClassX.Text.Trim());
                                cmd.Parameters.AddWithValue("@class_tweelth_roll_no", txtClassXII.Text.Trim());
                                cmd.Parameters.AddWithValue("@tc_received", ddlTransferCert.SelectedValue);
                                cmd.Parameters.AddWithValue("@cc_received", ddlCasteCert.SelectedValue);
                                cmd.Parameters.AddWithValue("@mc_received", ddlMigrationCert.SelectedValue);
                                cmd.Parameters.AddWithValue("@marksheet_received", ddlMarksheetCert.SelectedValue);
                                cmd.Parameters.AddWithValue("@rte", chkRTE.Checked);
                                cmd.Parameters.AddWithValue("@gender", ddlGender.SelectedValue);
                                cmd.Parameters.AddWithValue("@caste_category", ddlCasteCategory.SelectedValue);
                                cmd.Parameters.AddWithValue("@minority", chkMinority.Checked);
                                cmd.Parameters.AddWithValue("@aadhar_no", txtAadhar.Text.Trim());
                                cmd.Parameters.AddWithValue("@BPL_category", chkBPL.Checked);
                                cmd.Parameters.AddWithValue("@disabled", chkDisabled.Checked);
                                cmd.Parameters.AddWithValue("@yot_adm", int.Parse(txtYearTerm.Text.Trim()));
                                cmd.Parameters.AddWithValue("@scholar_branch_registration_id", txtAdmNo.Text.Trim());

                                cmd.ExecuteNonQuery();
                                newRegId = cmd.LastInsertedId;
                            }

                            string queryClass = @"INSERT INTO scholar_class(scholar_registration_number, class_id, admission_date,
                                                section, year_of_term, avail_hostel, height_in_cms, weight_in_kgs, scholar_roll_number)
                                                VALUES(@scholar_registration_number, @class_id, @admission_date,
                                                @section, @year_of_term, @avail_hostel, @height_in_cms, @weight_in_kgs, @scholar_roll_number)";

                            using (var cmd = new MySqlCommand(queryClass, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@scholar_registration_number", newRegId);
                                cmd.Parameters.AddWithValue("@class_id", ddlClass.SelectedValue);
                                cmd.Parameters.AddWithValue("@admission_date", DateTime.Parse(txtDateAddmission.Text.Trim()));
                                cmd.Parameters.AddWithValue("@section", ddlSection.SelectedValue);
                                cmd.Parameters.AddWithValue("@year_of_term", int.Parse(txtYearTerm.Text.Trim()));
                                cmd.Parameters.AddWithValue("@avail_hostel", chkHostel.Checked ? 1 : 0);
                                cmd.Parameters.AddWithValue("@height_in_cms", txtHeight.Text.Trim());
                                cmd.Parameters.AddWithValue("@weight_in_kgs", txtWeight.Text.Trim());
                                cmd.Parameters.AddWithValue("@scholar_roll_number", txtRollNo.Text.Trim());

                                cmd.ExecuteNonQuery();
                                SCID = cmd.LastInsertedId;

                                string paramQuery = "SELECT SAPID, SPTID FROM scholar_additional_parameters";
                                using (MySqlCommand cmd2 = new MySqlCommand(paramQuery, conn, transaction))
                                using (MySqlDataReader reader = cmd2.ExecuteReader())
                                {
                                    var parameters = new List<(string sapid, string sptid)>();
                                    while (reader.Read())
                                    {
                                        parameters.Add((reader["SAPID"].ToString(), reader["SPTID"].ToString()));
                                    }
                                    reader.Close();

                                    foreach (var (sapid, sptid) in parameters)
                                    {
                                        string controlId = "input_" + sapid;
                                        Control ctrl = paraform.FindControl(controlId);

                                        if (ctrl == null)
                                        {
                                            System.Diagnostics.Debug.WriteLine("Control not found for SAPID: " + sapid);
                                            continue;
                                        }


                                        if (ctrl is TextBox txt && !string.IsNullOrWhiteSpace(txt.Text))
                                        {
                                            using (MySqlCommand valCmd = new MySqlCommand(
                                                @"INSERT INTO scholar_additional_values (SCID, SAPID, para_value) 
                                                 VALUES (@SCID, @SAPID, @value);", conn, transaction))
                                            {
                                                valCmd.Parameters.AddWithValue("@SCID", SCID);
                                                valCmd.Parameters.AddWithValue("@SAPID", sapid);
                                                valCmd.Parameters.AddWithValue("@value", txt.Text.Trim());
                                                valCmd.ExecuteNonQuery();
                                            }
                                        }

                                        else if (ctrl is DropDownList ddl && !string.IsNullOrEmpty(ddl.SelectedValue))
                                        {
                                            long savid = GetOrCreateSAVID(SCID, sapid, conn, transaction);
                                            long sapoid = GetSAPOID(sapid, ddl.SelectedItem.Text, conn, transaction);

                                            using (MySqlCommand optCmd = new MySqlCommand(@"INSERT INTO scholar_additional_val_option (SAVID, SAPOID) VALUES (@SAVID, @SAPOID);", conn, transaction))
                                            {
                                                optCmd.Parameters.AddWithValue("@SAVID", savid);
                                                optCmd.Parameters.AddWithValue("@SAPOID", sapoid);
                                                optCmd.ExecuteNonQuery();
                                            }
                                        }
                                        else if (ctrl is CheckBox chk && chk.Checked)
                                        {
                                            long savid = GetOrCreateSAVID(SCID, sapid, conn, transaction);
                                            long sapoid = GetSAPOID(sapid, "Yes", conn, transaction);

                                            using (MySqlCommand optCmd = new MySqlCommand(@"INSERT INTO scholar_additional_val_option (SAVID, SAPOID) VALUES (@SAVID, @SAPOID);", conn, transaction))
                                            {
                                                optCmd.Parameters.AddWithValue("@SAVID", savid);
                                                optCmd.Parameters.AddWithValue("@SAPOID", sapoid);
                                                optCmd.ExecuteNonQuery();
                                            }
                                        }
                                        else if (ctrl is CheckBoxList chkList)
                                        {
                                            foreach (ListItem item in chkList.Items)
                                            {
                                                if (item.Selected)
                                                {
                                                    long savid = GetOrCreateSAVID(SCID, sapid, conn, transaction);
                                                    long sapoid = GetSAPOID(sapid, item.Text, conn, transaction);

                                                    using (MySqlCommand optCmd = new MySqlCommand(@"INSERT INTO scholar_additional_val_option (SAVID, SAPOID) VALUES (@SAVID, @SAPOID);", conn, transaction))
                                                    {
                                                        optCmd.Parameters.AddWithValue("@SAVID", savid);
                                                        optCmd.Parameters.AddWithValue("@SAPOID", sapoid);
                                                        optCmd.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                transaction.Commit();
                                Response.Write("<script>alert('Submitted Successfully');</script>");
                                ClearForm();
                                DynamicGroupsGeneration();
                            }
                        }
                        catch (MySqlException ex)
                        {
                            transaction.Rollback();
                            lblMessage.Text = "Database Error: " + ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
            }
        }

        private long GetOrCreateSAVID(long SCID, string SAPID, MySqlConnection conn, MySqlTransaction transaction)
        {
            using (var checkCmd = new MySqlCommand(@"SELECT SAVID FROM scholar_additional_values WHERE SCID = @SCID AND SAPID = @SAPID;", conn, transaction))
            {
                checkCmd.Parameters.AddWithValue("@SCID", SCID);
                checkCmd.Parameters.AddWithValue("@SAPID", SAPID);
                object result = checkCmd.ExecuteScalar();
                if (result != null)
                    return Convert.ToInt64(result);
            }

            using (var insertCmd = new MySqlCommand(@"INSERT INTO scholar_additional_values (SCID, SAPID, para_value) VALUES (@SCID, @SAPID, '');", conn, transaction))
            {
                insertCmd.Parameters.AddWithValue("@SCID", SCID);
                insertCmd.Parameters.AddWithValue("@SAPID", SAPID);
                insertCmd.ExecuteNonQuery();
                return insertCmd.LastInsertedId;
            }
        }

        private long GetSAPOID(string SAPID, string optionValue, MySqlConnection conn, MySqlTransaction transaction)
        {
            using (var cmd = new MySqlCommand(@"SELECT SAPOID FROM scholar_additional_param_option WHERE SAPID = @SAPID AND option_value = @value;", conn, transaction))
            {
                cmd.Parameters.AddWithValue("@SAPID", SAPID);
                cmd.Parameters.AddWithValue("@value", optionValue);
                object result = cmd.ExecuteScalar();
                if (result != null)
                    return Convert.ToInt64(result);
            }

            throw new Exception($"Option '{optionValue}' not found for SAPID {SAPID}");
        }
        protected void ddlAdmissionNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            string admissionNo = ddlAdmissionNumber.SelectedValue;
            if (!string.IsNullOrEmpty(admissionNo))
            {
                LoadStudentData(admissionNo);
            }
        }

        private void LoadStudentData(string admissionNo)
        {
            ClearForm();
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string queryRegister = @"SELECT * from scholar_register where scholar_branch_registration_id = @AdmissionNo";
                using (var cmd = new MySqlCommand(queryRegister, conn))
                {
                    cmd.Parameters.AddWithValue("@AdmissionNo", admissionNo);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtFirstName.Text = reader["first_name"].ToString();
                            txtMiddleName.Text = reader["middle_name"].ToString();
                            txtLastName.Text = reader["last_name"].ToString();
                            txtDOB.Text = Convert.ToDateTime(reader["date_of_birth"]).ToString("yyyy-MM-dd");
                            ddlReligion.SelectedValue = reader["religion"].ToString();
                            txtDateAddmission.Text = Convert.ToDateTime(reader["date_of_admission"]).ToString("yyyy-MM-dd");
                            txtFatherName.Text = reader["father_name"].ToString();
                            txtMotherName.Text = reader["mother_name"].ToString();
                            txtGuardianName.Text = reader["gaurdian_name"].ToString();
                            txtRelation.Text = reader["relation_with_student"].ToString();
                            txtOccupation.Text = reader["occupation"].ToString();
                            txtAddress.Text = reader["address"].ToString();
                            txtContactNo.Text = reader["phone"].ToString();
                            txtMobile.Text = reader["mobile"].ToString();
                            txtEmail.Text = reader["email_id"].ToString();
                            txtNationality.Text = reader["nationality"].ToString();
                            ddlCaste.SelectedValue = reader["caste"].ToString();
                            txtClassX.Text = reader["class_tenth_roll_no"].ToString();
                            txtClassXII.Text = reader["class_tweelth_roll_no"].ToString();
                            ddlTransferCert.SelectedValue = reader["tc_received"].ToString();
                            ddlCasteCert.SelectedValue = reader["cc_received"].ToString();
                            ddlMigrationCert.SelectedValue = reader["mc_received"].ToString();
                            ddlMarksheetCert.SelectedValue = reader["marksheet_received"].ToString();
                            chkRTE.Checked = Convert.ToBoolean(reader["rte"]);
                            ddlGender.SelectedValue = reader["gender"].ToString();
                            ddlCasteCategory.SelectedValue = reader["caste_category"].ToString();
                            chkMinority.Checked = Convert.ToBoolean(reader["minority"]);
                            txtAadhar.Text = reader["aadhar_no"].ToString();
                            chkBPL.Checked = Convert.ToBoolean(reader["BPL_category"]);
                            chkDisabled.Checked = Convert.ToBoolean(reader["disabled"]);
                            txtYearTerm.Text = reader["yot_adm"].ToString();
                            txtAdmNo.Text = reader["scholar_branch_registration_id"].ToString();

                        }
                    }
                }
                string queryClass = @"SELECT * FROM scholar_class sc join scholar_register sr ON sr.registration_number = sc.scholar_registration_number
                                    where sr.scholar_branch_registration_id = @AdmissionNo";

                using (var cmd = new MySqlCommand(queryClass, conn))
                {
                    cmd.Parameters.AddWithValue("@AdmissionNo", admissionNo);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ddlClass.SelectedValue = reader["class_id"].ToString();
                            ddlSection.SelectedValue = reader["section"].ToString();
                            txtYearTerm.Text = reader["year_of_term"].ToString();
                            chkHostel.Checked = Convert.ToBoolean(reader["avail_hostel"]);
                            txtHeight.Text = reader["height_in_cms"].ToString();
                            txtWeight.Text = reader["weight_in_kgs"].ToString();
                            txtRollNo.Text = reader["scholar_roll_number"].ToString();

                        }
                    }
                }
                DynamicGroupsGeneration();

                string queryAdditional = @"SELECT sav.SAPID, sav.para_value, sao.option_value 
                           FROM scholar_additional_values sav
                           LEFT JOIN scholar_additional_val_option svo ON sav.SAVID = svo.SAVID
                           LEFT JOIN scholar_additional_param_option sao ON svo.SAPOID = sao.SAPOID
                           INNER JOIN scholar_class sc ON sav.SCID = sc.scholar_class_id
                           INNER JOIN scholar_register sr ON sc.scholar_registration_number = sr.registration_number
                           WHERE sr.scholar_branch_registration_id = @AdmissionNo";

                using (var cmd = new MySqlCommand(queryAdditional, conn))
                {
                    cmd.Parameters.AddWithValue("@AdmissionNo", admissionNo);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string sapid = reader["SAPID"] != DBNull.Value ? reader["SAPID"].ToString() : "";
                            string value = reader["para_value"] != DBNull.Value ? reader["para_value"].ToString() : "";
                            string optionValue = reader["option_value"] != DBNull.Value ? reader["option_value"].ToString() : "";

                            string controlId = "input_" + sapid;
                            Control ctrl = paraform.FindControl(controlId);

                            if (ctrl is TextBox txt)
                            {
                                txt.Text = value;
                            }
                            else if (ctrl is DropDownList ddl && !string.IsNullOrEmpty(optionValue))
                            {
                                if (ddl.Items.FindByValue(optionValue) != null)
                                    ddl.SelectedValue = optionValue;
                            }
                            else if (ctrl is CheckBox chk && optionValue == "Yes")
                            {
                                chk.Checked = true;
                            }
                            else if (ctrl is CheckBoxList chkList)
                            {
                                foreach (ListItem item in chkList.Items)
                                {
                                    if (item.Text == optionValue)
                                        item.Selected = true;
                                }
                            }
                        }
                    }
                }


            }
        }


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        string queryUpdRegister = @"UPDATE scholar_register SET first_name = @first_name, middle_name = @middle_name, last_name = @last_name, date_of_birth = @date_of_birth, religion = @religion,
                            date_of_admission = @date_of_admission, father_name = @father_name, mother_name = @mother_name, gaurdian_name = @gaurdian_name, relation_with_student = @relation_with_student,occupation = @occupation, 
                            address = @address, phone = @phone, mobile = @mobile, email_id = @email_id, nationality = @nationality, caste = @caste,tc_received = @tc_received, 
                            cc_received = @cc_received, mc_received = @mc_received, marksheet_received = @marksheet_received, class_tenth_roll_no = @class_tenth_roll_no,class_tweelth_roll_no = @class_tweelth_roll_no,
                            rte = @rte, gender = @gender, caste_category = @caste_category, minority = @minority, aadhar_no = @aadhar_no,BPL_category = @BPL_category, disabled = @disabled, 
                            yot_adm = @yot_adm WHERE scholar_branch_registration_id = @scholar_branch_registration_id";

                        using (var cmd = new MySqlCommand(queryUpdRegister, conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@first_name", txtFirstName.Text);
                            cmd.Parameters.AddWithValue("@middle_name", txtMiddleName.Text);
                            cmd.Parameters.AddWithValue("@last_name", txtLastName.Text);
                            cmd.Parameters.AddWithValue("@date_of_birth", DateTime.Parse(txtDOB.Text));
                            cmd.Parameters.AddWithValue("@religion", ddlReligion.SelectedValue);
                            cmd.Parameters.AddWithValue("@date_of_admission", DateTime.Parse(txtDateAddmission.Text));
                            cmd.Parameters.AddWithValue("@father_name", txtFatherName.Text);
                            cmd.Parameters.AddWithValue("@mother_name", txtMotherName.Text);
                            cmd.Parameters.AddWithValue("@gaurdian_name", txtGuardianName.Text);
                            cmd.Parameters.AddWithValue("@relation_with_student", txtRelation.Text);
                            cmd.Parameters.AddWithValue("@occupation", txtOccupation.Text);
                            cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                            cmd.Parameters.AddWithValue("@phone", txtContactNo.Text);
                            cmd.Parameters.AddWithValue("@mobile", txtMobile.Text);
                            cmd.Parameters.AddWithValue("@email_id", txtEmail.Text);
                            cmd.Parameters.AddWithValue("@nationality", txtNationality.Text);
                            cmd.Parameters.AddWithValue("@caste", ddlCaste.SelectedValue);
                            cmd.Parameters.AddWithValue("@tc_received", ddlTransferCert.SelectedValue);
                            cmd.Parameters.AddWithValue("@cc_received", ddlCasteCert.SelectedValue);
                            cmd.Parameters.AddWithValue("@mc_received", ddlMigrationCert.SelectedValue);
                            cmd.Parameters.AddWithValue("@marksheet_received", ddlMarksheetCert.SelectedValue);
                            cmd.Parameters.AddWithValue("@class_tenth_roll_no", txtClassX.Text);
                            cmd.Parameters.AddWithValue("@class_tweelth_roll_no", txtClassXII.Text);
                            cmd.Parameters.AddWithValue("@rte", chkRTE.Checked);
                            cmd.Parameters.AddWithValue("@gender", ddlGender.SelectedValue);
                            cmd.Parameters.AddWithValue("@caste_category", ddlCasteCategory.SelectedValue);
                            cmd.Parameters.AddWithValue("@minority", chkMinority.Checked);
                            cmd.Parameters.AddWithValue("@aadhar_no", txtAadhar.Text);
                            cmd.Parameters.AddWithValue("@BPL_category", chkBPL.Checked);
                            cmd.Parameters.AddWithValue("@disabled", chkDisabled.Checked);
                            cmd.Parameters.AddWithValue("@yot_adm", int.Parse(txtYearTerm.Text));
                            cmd.Parameters.AddWithValue("@scholar_branch_registration_id", txtAdmNo.Text);

                            cmd.ExecuteNonQuery();
                        }
                        long SCID;
                        string queryUpdClass = @"UPDATE scholar_class SET class_id = @class_id, admission_date = @admission_date, section = @section, year_of_term = @year_of_term,
                                                avail_hostel = @avail_hostel, height_in_cms = @height_in_cms, weight_in_kgs = @weight_in_kgs,scholar_roll_number = @scholar_roll_number 
                                                WHERE scholar_registration_number = (SELECT registration_number FROM scholar_register WHERE scholar_branch_registration_id = @scholar_branch_registration_id)";
                        using (var cmd2 = new MySqlCommand(queryUpdClass, conn, tx))
                        {
                            cmd2.Parameters.AddWithValue("@class_id", ddlClass.SelectedValue);
                            cmd2.Parameters.AddWithValue("@admission_date", DateTime.Parse(txtDateAddmission.Text));
                            cmd2.Parameters.AddWithValue("@section", ddlSection.SelectedValue);
                            cmd2.Parameters.AddWithValue("@year_of_term", int.Parse(txtYearTerm.Text));
                            cmd2.Parameters.AddWithValue("@avail_hostel", chkHostel.Checked ? 1 : 0);
                            cmd2.Parameters.AddWithValue("@height_in_cms", txtHeight.Text);
                            cmd2.Parameters.AddWithValue("@weight_in_kgs", txtWeight.Text);
                            cmd2.Parameters.AddWithValue("@scholar_roll_number", txtRollNo.Text.Trim());
                            cmd2.Parameters.AddWithValue("@scholar_branch_registration_id", txtAdmNo.Text);
                           
                            SCID = cmd2.LastInsertedId;

                            try
                            {
                                cmd2.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        using (var scidCmd = new MySqlCommand(
                                    @"SELECT scholar_class_id 
                                      FROM scholar_class 
                                      WHERE scholar_registration_number = (
                                          SELECT registration_number 
                                          FROM scholar_register 
                                          WHERE scholar_branch_registration_id = @scholar_branch_registration_id
                                      );", conn, tx))
                        {
                            scidCmd.Parameters.AddWithValue("@scholar_branch_registration_id", txtAdmNo.Text);

                            var result = scidCmd.ExecuteScalar();
                            if (result == null)
                            {
                                throw new Exception("No scholar_class found for the given registration.");
                            }

                            SCID = Convert.ToInt64(result);
                        }


                        using (MySqlCommand checkScidCmd = new MySqlCommand(
                            "SELECT COUNT(*) FROM scholar_class WHERE scholar_class_id = @SCID", conn, tx))
                        {
                            checkScidCmd.Parameters.AddWithValue("@SCID", SCID);
                            var count = Convert.ToInt32(checkScidCmd.ExecuteScalar());
                            if (count == SCID)
                            {
                                throw new Exception($"Invalid SCID: {SCID}. No such scholar_class exists.");
                            }
                        }

                        using (MySqlCommand delOptCmd = new MySqlCommand(
                            @"DELETE FROM scholar_additional_val_option 
                             WHERE SAVID IN (SELECT SAVID FROM scholar_additional_values WHERE SCID = @SCID);", conn, tx))
                        {
                            delOptCmd.Parameters.AddWithValue("@SCID", SCID);
                            delOptCmd.ExecuteNonQuery();
                        }

                        using (MySqlCommand delValCmd = new MySqlCommand(
                            @"DELETE FROM scholar_additional_values WHERE SCID = @SCID;", conn, tx))
                        {
                            delValCmd.Parameters.AddWithValue("@SCID", SCID);
                            delValCmd.ExecuteNonQuery();
                        }

                        string paramQuery = "SELECT SAPID, SPTID FROM scholar_additional_parameters";

                        using (MySqlCommand cmd2 = new MySqlCommand(paramQuery, conn, tx))
                        using (MySqlDataReader reader = cmd2.ExecuteReader())
                        {
                            var parameters = new List<(string sapid, string sptid)>();
                            while (reader.Read())
                            {
                                parameters.Add((reader["SAPID"].ToString(), reader["SPTID"].ToString()));
                            }
                            reader.Close();

                            foreach (var (sapid, sptid) in parameters)
                            {
                                string controlId = "input_" + sapid;
                                Control ctrl = paraform.FindControl(controlId);

                                if (ctrl == null)
                                {
                                    System.Diagnostics.Debug.WriteLine("Control not found for SAPID: " + sapid);
                                    continue;
                                }

                                if (ctrl is TextBox txt && !string.IsNullOrWhiteSpace(txt.Text))
                                {
                                    using (MySqlCommand valCmd = new MySqlCommand(
                                        @"INSERT INTO scholar_additional_values (SCID, SAPID, para_value) 
                                        VALUES (@SCID, @SAPID, @value);", conn, tx))
                                    {
                                        valCmd.Parameters.AddWithValue("@SCID", SCID);
                                        valCmd.Parameters.AddWithValue("@SAPID", sapid);
                                        valCmd.Parameters.AddWithValue("@value", txt.Text.Trim());
                                        valCmd.ExecuteNonQuery();
                                    }
                                }

                                else if (ctrl is DropDownList ddl && !string.IsNullOrEmpty(ddl.SelectedValue))
                                {
                                    long savid = GetOrCreateSAVID(SCID, sapid, conn, tx);
                                    long sapoid = GetSAPOID(sapid, ddl.SelectedItem.Text, conn, tx);

                                    using (MySqlCommand optCmd = new MySqlCommand(
                                        @"INSERT IGNORE INTO scholar_additional_val_option (SAVID, SAPOID) 
                                         VALUES (@SAVID, @SAPOID);", conn, tx))
                                    {
                                        optCmd.Parameters.AddWithValue("@SAVID", savid);
                                        optCmd.Parameters.AddWithValue("@SAPOID", sapoid);
                                        optCmd.ExecuteNonQuery();
                                    }
                                }

                                else if (ctrl is CheckBox chk && chk.Checked)
                                {
                                    long savid = GetOrCreateSAVID(SCID, sapid, conn, tx);
                                    long sapoid = GetSAPOID(sapid, "Yes", conn, tx);

                                    using (MySqlCommand optCmd = new MySqlCommand(
                                        @"INSERT IGNORE INTO scholar_additional_val_option (SAVID, SAPOID) 
                                          VALUES (@SAVID, @SAPOID);", conn, tx))
                                    {
                                        optCmd.Parameters.AddWithValue("@SAVID", savid);
                                        optCmd.Parameters.AddWithValue("@SAPOID", sapoid);
                                        optCmd.ExecuteNonQuery();
                                    }
                                }

                                else if (ctrl is CheckBoxList chkList)
                                {
                                    foreach (ListItem item in chkList.Items)
                                    {
                                        if (item.Selected)
                                        {
                                            long savid = GetOrCreateSAVID(SCID, sapid, conn, tx);
                                            long sapoid = GetSAPOID(sapid, item.Text, conn, tx);

                                            using (MySqlCommand optCmd = new MySqlCommand(
                                                @"INSERT IGNORE INTO scholar_additional_val_option (SAVID, SAPOID) 
                                                 VALUES (@SAVID, @SAPOID);", conn, tx))
                                            {
                                                optCmd.Parameters.AddWithValue("@SAVID", savid);
                                                optCmd.Parameters.AddWithValue("@SAPOID", sapoid);
                                                optCmd.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }
                            }
                        }


                        tx.Commit();

                        Response.Write("Student details updated successfully.");
                        ClearForm();
                        DynamicGroupsGeneration();
                    }

                    catch (Exception ex)
                    {
                        tx.Rollback();
                        Console.WriteLine(ex.Message);
                        Response.Write("Error occurred: " + ex.Message);
                    }
                }
            }
        }
        private void ClearForm()
        {

            txtFirstName.Text = "";
            txtMiddleName.Text = "";
            txtLastName.Text = "";
            txtDOB.Text = "";
            txtDateAddmission.Text = "";
            txtFatherName.Text = "";
            txtMotherName.Text = "";
            txtGuardianName.Text = "";
            txtRelation.Text = "";
            txtOccupation.Text = "";
            txtAddress.Text = "";
            txtContactNo.Text = "";
            txtMobile.Text = "";
            txtEmail.Text = "";
            txtNationality.Text = "";
            txtClassX.Text = "";
            txtClassXII.Text = "";
            txtAadhar.Text = "";
            txtYearTerm.Text = "";
            txtAdmNo.Text = "";
            txtHeight.Text = "";
            txtWeight.Text = "";
            txtRollNo.Text = "";

            ddlReligion.SelectedIndex = 0;
            ddlCasteCategory.SelectedIndex = 0;
            ddlCaste.SelectedIndex = 0;
            ddlGender.SelectedIndex = 0;
            ddlClass.SelectedIndex = 0;
            ddlBusRoute.SelectedIndex = 0;
            ddlBusStop.SelectedIndex = 0;
            ddlSection.SelectedIndex = 0;
            ddlTransferCert.SelectedIndex = 0;
            ddlCasteCert.SelectedIndex = 0;
            ddlMigrationCert.SelectedIndex = 0;
            ddlMarksheetCert.SelectedIndex = 0;

            chkRTE.Checked = false;
            chkMinority.Checked = false;
            chkBPL.Checked = false;
            chkDisabled.Checked = false;
            chkHostel.Checked = false;

            paraform.Controls.Clear();

        }        
    }
}

