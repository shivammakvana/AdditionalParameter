using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace YourNamespace
{
    public partial class StudentRegistration : System.Web.UI.Page
    {
        string connStr = "Server=localhost;Port=3306;Database=demo_db;Uid=root;Pwd=Shivam7024@;";


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

    }
}
