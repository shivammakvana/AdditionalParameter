using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace YourNamespace
{
    public partial class AdditionalParameter : System.Web.UI.Page
    {
        string connStr = "Server=localhost;Port=3306;Database=demo_db;Uid=root;Pwd=Shivam7024@;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowGroupPanel();
                SetFormMode(true);
                BindParameterTypeDropdown();
                BindGroupDropdown();
            }
        }

        protected void linkbtn1_Click(object sender, EventArgs e) => ShowGroupPanel();
        protected void linkbtn2_Click(object sender, EventArgs e) => ShowParameterPanel();

        private void ShowGroupPanel()
        {
            pnlGroup.Visible = true;
            pnlParameter.Visible = false;
            linkbtn1.Enabled = false;
            linkbtn2.Enabled = true;
            BindGroupGrid();
        }

        private void ShowParameterPanel()
        {
            pnlGroup.Visible = false;
            pnlParameter.Visible = true;
            linkbtn1.Enabled = true;
            linkbtn2.Enabled = false;
            BindParameterGrid();
        }

        private void BindGroupGrid()
        
      {
            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    string q = @"SELECT SAFGID AS GroupID, group_name AS GroupName, group_order AS GroupOrder
                                 FROM scholar_additional_field_group ORDER BY group_order";
                    var da = new MySqlDataAdapter(q, conn);
                    var dt = new DataTable();
                    da.Fill(dt);
                    gvGroup.DataSource = dt;
                    gvGroup.DataBind();
                }
            }
            catch (Exception ex)
                        {
                ShowError("Error loading groups: " + ex.Message);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
            
                if (string.IsNullOrWhiteSpace(txtGroupName.Text))
                {
                    ShowError("Group Name is required."); return;
                }
                if (!int.TryParse(txtGroupOrder.Text, out int groupOrder) || groupOrder <= 0)
                {
                    ShowError("Group Order must be a positive number."); return;
                }
             

                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string q = "INSERT INTO scholar_additional_field_group (group_name, group_order) VALUES (@name, @order)";
                    using (var cmd = new MySqlCommand(q, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", txtGroupName.Text.Trim());
                        cmd.Parameters.AddWithValue("@order", groupOrder);
                        cmd.ExecuteNonQuery();
                    }
                }

                BindGroupGrid();
                ClearGroupFields();
                ShowSuccess("Group added successfully.");
                ShowError("Duplicate group name/order is not allowed.");
            }
            catch (Exception ex)
            {
                ShowError("Error adding group: " + ex.Message);
            }
        }

        protected void gvGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvGroup.Rows[rowIndex];
                ViewState["EditGroupID"] = gvGroup.DataKeys[rowIndex].Value;
                txtGroupName.Text = row.Cells[0].Text;
                txtGroupOrder.Text = row.Cells[1].Text;
                SetFormMode(false);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["EditGroupID"] == null) return;

                if (!int.TryParse(txtGroupOrder.Text, out int groupOrder) || groupOrder <= 0)
                {
                    ShowError("Group Order must be a positive number."); return;
                }

                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string q = @"UPDATE scholar_additional_field_group
                                 SET group_name=@name, group_order=@order WHERE SAFGID=@id";
                    using (var cmd = new MySqlCommand(q, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", txtGroupName.Text.Trim());
                        cmd.Parameters.AddWithValue("@order", groupOrder);
                        cmd.Parameters.AddWithValue("@id", ViewState["EditGroupID"]);
                        cmd.ExecuteNonQuery();
                    }
                }

                BindGroupGrid();
                ClearGroupFields();
                ShowSuccess("Group updated successfully.");
            }
            catch (Exception ex)
            {
                ShowError("Error updating group: " + ex.Message);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["EditGroupID"] == null) return;

                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string q = "DELETE FROM scholar_additional_field_group WHERE SAFGID=@id";
                    using (var cmd = new MySqlCommand(q, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", ViewState["EditGroupID"]);
                        cmd.ExecuteNonQuery();
                    }
                }

                BindGroupGrid();
                ClearGroupFields();
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Group deleted successfully.";
            }
            catch (Exception ex)
            {
                ShowError("Error deleting group: " + ex.Message);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearGroupFields();
            lblMessage.Text = "";
        }

        private void ClearGroupFields()
        {
            txtGroupName.Text = "";
            txtGroupOrder.Text = "";
            ViewState["EditGroupID"] = null;
            SetFormMode(true);
        }

        private void SetFormMode(bool isNew)
        {
            btnSubmit.Visible = isNew;
            btnDelete.Visible = !isNew;
            btnUpdate.Visible = !isNew;
        }

        private void ShowError(string msg)
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = msg;
        }

        private void ShowSuccess(string msg)
        {
            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = msg;
        }


            // Second panel start
        private void BindParameterTypeDropdown()
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string q = "SELECT SPTID, para_type FROM scholar_para_type";
                using (var cmd = new MySqlCommand(q, conn))
                {
                    ddlParaType.DataSource = cmd.ExecuteReader();
                    ddlParaType.DataTextField = "para_type";
                    ddlParaType.DataValueField = "SPTID";
                    ddlParaType.DataBind();
                }
            }
        }

        private void BindGroupDropdown()
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string q = "SELECT SAFGID, group_name FROM scholar_additional_field_group ORDER BY group_order";
                using (var cmd = new MySqlCommand(q, conn))
                {
                    ddlGroup.DataSource = cmd.ExecuteReader();
                    ddlGroup.DataTextField = "group_name";
                    ddlGroup.DataValueField = "SAFGID";
                    ddlGroup.DataBind();
                }
            }
            ddlGroup.Items.Insert(0, new ListItem("-- Select Group --", "0"));
        }

        private void BindParameterGrid()
        {
            using (var conn = new MySqlConnection(connStr))
            {
                string q = @" SELECT a.SAPID AS ParameterID, a.para_name AS ParameterName, p.para_type AS ParameterType, a.is_compulsory AS IsCompulsory, a.default_value AS DefaultValue,
                 a.parent_hide AS ParentHide,a.greeting_text AS GreetingText,a.order_id AS `Order`,a.SAFGID AS GroupID,g.group_name AS GroupName FROM scholar_additional_parameters a JOIN scholar_para_type p ON a.SPTID = p.SPTID 
                 JOIN scholar_additional_field_group g ON a.SAFGID = g.SAFGID ORDER BY a.order_id";

                var da = new MySqlDataAdapter(q, conn);
                var dt = new DataTable();
                da.Fill(dt);
                gvParameter.DataSource = dt;
                gvParameter.DataBind();
            }
        }


        protected void ddlParaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleOptionsUI(ddlParaType.SelectedItem.Text);
        }

        private void ToggleOptionsUI(string typeText)
        {
            bool show = (typeText == "DropDownList" || typeText == "CheckBox");
            lstDdOption.Visible = show;
            txtDdOption.Visible = show;
            btnAddOption.Visible = show;
            btnUpdateOption.Visible = show;
            btnDeleteOption.Visible = show;
            btnClearOption.Visible = show;
        }

        protected void btnAddOption_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtDdOption.Text))
            {
                lstDdOption.Items.Add(new ListItem(txtDdOption.Text.Trim()));
                txtDdOption.Text = "";
            }
        }

        protected void btnUpdateOption_Click(object sender, EventArgs e)
        {
            if (lstDdOption.SelectedIndex >= 0 && !string.IsNullOrWhiteSpace(txtDdOption.Text))
            {
                lstDdOption.Items[lstDdOption.SelectedIndex].Text = txtDdOption.Text.Trim();
                txtDdOption.Text = "";
            }
            else
            {
                ShowError("Select an option and enter new text to update.");
            }
        }

        protected void btnDeleteOption_Click(object sender, EventArgs e)
        {
            if (lstDdOption.SelectedIndex >= 0)
                lstDdOption.Items.RemoveAt(lstDdOption.SelectedIndex);
        }

        protected void btnClearOption_Click(object sender, EventArgs e)
        {
            lstDdOption.Items.Clear();
        }

        protected void btnParamSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction())
                    {
                        string insertParamQuery = @"INSERT INTO scholar_additional_parameters(para_name, SPTID, is_compulsory, default_value, parent_hide, order_id, SAFGID, greeting_text)
                            VALUES (@para_name, @SPTID, @is_compulsory, @default_value, @parent_hide, @order_id, @SAFGID, @greeting_text); SELECT LAST_INSERT_ID();";

                        using (var cmd = new MySqlCommand(insertParamQuery, conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@para_name", txtAddParaName.Text.Trim());
                            cmd.Parameters.AddWithValue("@SPTID", ddlParaType.SelectedValue);
                            cmd.Parameters.AddWithValue("@is_compulsory", cbCompulsory.Checked ? 1 : 0);
                            cmd.Parameters.AddWithValue("@default_value", txtDefValue.Text.Trim());
                            cmd.Parameters.AddWithValue("@parent_hide", cbHide.Checked ? 1 : 0);
                            cmd.Parameters.AddWithValue("@order_id", txtOrder.Text);
                            cmd.Parameters.AddWithValue("@SAFGID", ddlGroup.SelectedValue);
                            cmd.Parameters.AddWithValue("@greeting_text", txtGreeting.Text.Trim());

                            long sapid = Convert.ToInt64(cmd.ExecuteScalar());

                            string selectedType = ddlParaType.SelectedItem.Text;
                            if (selectedType == "DropDownList" || selectedType == "CheckBox")
                            {
                                InsertOptions(conn, tx, sapid);
                            }

                            tx.Commit();
                        }
                    }
                }

                BindParameterGrid();
                ClearParameterFields();
                ShowSuccess("Parameter added successfully.");
            }
            catch (Exception ex)
            {
                ShowError("Error adding parameter: " + ex.Message);
            }
        }

        private void InsertOptions(MySqlConnection conn, MySqlTransaction tx, long sapid)
        {
            string insertOptionQuery = @"INSERT INTO scholar_additional_param_option (SAPID, option_value)
                                         VALUES (@SAPID, @option_value)";
            foreach (ListItem item in lstDdOption.Items)
            {
                using (var optionCmd = new MySqlCommand(insertOptionQuery, conn, tx))
                {
                    optionCmd.Parameters.AddWithValue("@SAPID", sapid);
                    optionCmd.Parameters.AddWithValue("@option_value", item.Text.Trim());
                    optionCmd.ExecuteNonQuery();
                }
            }
        }

        protected void gvParameter_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                var dataKey = gvParameter.DataKeys[rowIndex];
                long sapid = Convert.ToInt64(dataKey.Values["ParameterID"]);
                ViewState["EditParameterID"] = sapid;

                LoadParameterIntoForm(sapid);

                btnParamSubmit.Visible = false;
                btnParamUpdate.Visible = true;
                btnParamDelete.Visible = true;
            }
        }

        private void LoadParameterIntoForm(long sapid)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string q = @"SELECT a.para_name, a.SPTID, a.is_compulsory, a.default_value, a.parent_hide,a.order_id, a.SAFGID, a.greeting_text, p.para_type FROM scholar_additional_parameters a
                    JOIN scholar_para_type p ON a.SPTID = p.SPTID WHERE a.SAPID = @id";

                using (var cmd = new MySqlCommand(q, conn))
                {
                    cmd.Parameters.AddWithValue("@id", sapid);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            txtAddParaName.Text = rdr["para_name"]?.ToString();
                            ddlParaType.SelectedValue = rdr["SPTID"].ToString();
                            cbCompulsory.Checked = Convert.ToInt32(rdr["is_compulsory"]) == 1;
                            txtDefValue.Text = rdr["default_value"]?.ToString();
                            cbHide.Checked = Convert.ToInt32(rdr["parent_hide"]) == 1;
                            txtOrder.Text = rdr["order_id"] == DBNull.Value ? "" : rdr["order_id"].ToString();
                            ddlGroup.SelectedValue = rdr["SAFGID"].ToString();
                            txtGreeting.Text = rdr["greeting_text"]?.ToString();

                            string typeText = rdr["para_type"].ToString();
                            ToggleOptionsUI(typeText);
                        }
                    }
                }

                lstDdOption.Items.Clear();
                string optQ = @"SELECT option_value FROM scholar_additional_param_option WHERE SAPID=@id ORDER BY SAPOID";
                using (var optCmd = new MySqlCommand(optQ, conn))
                {
                    optCmd.Parameters.AddWithValue("@id", sapid);
                    using (var optRdr = optCmd.ExecuteReader())
                    {
                        while (optRdr.Read())
                        {
                            lstDdOption.Items.Add(new ListItem(optRdr["option_value"].ToString()));
                        }
                    }
                }
            }
        }

        protected void btnParamUpdate_Click(object sender, EventArgs e)
        {
            if (ViewState["EditParameterID"] == null) return;

            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction())
                    {
                        string q = @"UPDATE scholar_additional_parameters SET para_name=@para_name, SPTID=@SPTID, is_compulsory=@is_compulsory,default_value=@default_value, parent_hide=@parent_hide, 
                                order_id=@order_id, SAFGID=@SAFGID, greeting_text=@greeting_text WHERE SAPID=@id";

                        using (var cmd = new MySqlCommand(q, conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@para_name", txtAddParaName.Text.Trim());
                            cmd.Parameters.AddWithValue("@SPTID", ddlParaType.SelectedValue);
                            cmd.Parameters.AddWithValue("@is_compulsory", cbCompulsory.Checked ? 1 : 0);
                            cmd.Parameters.AddWithValue("@default_value", txtDefValue.Text.Trim());
                            cmd.Parameters.AddWithValue("@parent_hide", cbHide.Checked ? 1 : 0);
                            cmd.Parameters.AddWithValue("@order_id",txtOrder.Text);
                            cmd.Parameters.AddWithValue("@SAFGID", ddlGroup.SelectedValue);
                            cmd.Parameters.AddWithValue("@greeting_text", txtGreeting.Text.Trim());
                            cmd.Parameters.AddWithValue("@id", ViewState["EditParameterID"]);
                            cmd.ExecuteNonQuery();
                        }

                        string selectedType = ddlParaType.SelectedItem.Text;
                        long sapid = Convert.ToInt64(ViewState["EditParameterID"]);

                        using (var del = new MySqlCommand(
                            "DELETE FROM scholar_additional_param_option WHERE SAPID=@id", conn, tx))
                        {
                            del.Parameters.AddWithValue("@id", sapid);
                            del.ExecuteNonQuery();
                        }

                        if (selectedType == "DropDownList" || selectedType == "CheckBox")
                        {
                            InsertOptions(conn, tx, sapid);
                        }

                        tx.Commit();
                    }
                }

                BindParameterGrid();
                ClearParameterFields();
                ShowSuccess("Parameter updated successfully.");
            }

            catch (Exception ex)
            {
                ShowError("Error updating parameter: " + ex.Message);
            }
        }

        protected void btnParamDelete_Click(object sender, EventArgs e)
        {
            if (ViewState["EditParameterID"] == null) return;

            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction())
                    {
                        long sapid = Convert.ToInt64(ViewState["EditParameterID"]);

                        using (var delVal = new MySqlCommand(
                            "DELETE FROM scholar_additional_values WHERE SAPID=@id", conn, tx))
                        {
                            delVal.Parameters.AddWithValue("@id", sapid);
                            delVal.ExecuteNonQuery();
                        }

                        using (var delOpt = new MySqlCommand(
                            "DELETE FROM scholar_additional_param_option WHERE SAPID=@id", conn, tx))
                        {
                            delOpt.Parameters.AddWithValue("@id", sapid);
                            delOpt.ExecuteNonQuery();
                        }

                        using (var delParam = new MySqlCommand(
                            "DELETE FROM scholar_additional_parameters WHERE SAPID=@id", conn, tx))
                        {
                            delParam.Parameters.AddWithValue("@id", sapid);
                            delParam.ExecuteNonQuery();
                        }

                        tx.Commit();
                    }
                }

                BindParameterGrid();
                ClearParameterFields();
                ShowSuccess("Parameter deleted successfully.");
            }
            catch (Exception ex)
            {
                ShowError("Error deleting parameter: " + ex.Message);
            }
        }


        protected void btnParamClear_Click(object sender, EventArgs e)
        {
            ClearParameterFields();
        }

        private void ClearParameterFields()
        {
            txtAddParaName.Text = "";
            ddlParaType.SelectedIndex = 0;
            txtOrder.Text = "";
            cbCompulsory.Checked = false;
            txtDefValue.Text = "";
            cbHide.Checked = false;
            ddlGroup.SelectedIndex = 0;
            txtGreeting.Text = "";
            lstDdOption.Items.Clear();

            ViewState["EditParameterID"] = null;
            btnParamSubmit.Visible = true;
            btnParamUpdate.Visible = false;
            btnParamDelete.Visible = false;
            lblMessage.Text = string.Empty;
            ToggleOptionsUI(ddlParaType.SelectedItem.Text);
        }

        protected void lnkShowGreeting_Click(object sender, EventArgs e)
        {
            txtGreeting.Visible = !txtGreeting.Visible;
        }
    }
}
