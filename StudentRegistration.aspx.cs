using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Org.BouncyCastle.Crypto.Agreement.Kdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace YourNamespace
{
    public partial class StudentRegistration : System.Web.UI.Page
    {
        string connStr = "Server=192.168.10.196;Port=3306;Database=demo_db;Uid=root;Pwd=console123;";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindReligion();
                BindCasteCategory();
                BindCaste();
                BindGender();
                BindClass();
                BindBusRoute();
                BindBusStop();
                DynamicGroupsGeneration();


            }
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
                            }

                            string paramQuery = "SELECT SAPID, SPTID FROM scholar_additional_parameters";
                            using (MySqlCommand cmd = new MySqlCommand(paramQuery, conn, transaction))
                            using (MySqlDataReader reader = cmd.ExecuteReader())
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
                                    if (ctrl == null) continue;

                                    if (ctrl is TextBox txt)
                                    {
                                        using (MySqlCommand valCmd = new MySqlCommand(@"INSERT INTO scholar_additional_values (SCID, SAPID, para_value) VALUES (@SCID, @SAPID, @value);", conn, transaction))
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
                DynamicGroupsGeneration();
                        }
                        catch (MySqlException ex)
                        {
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

    }
}

