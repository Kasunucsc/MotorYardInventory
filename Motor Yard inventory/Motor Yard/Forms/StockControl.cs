﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace Motor_Yard
{
    public partial class Stock_Control : Form
    {

       
     
        
        public Stock_Control(int index)
        {
            InitializeComponent();
            
            if (index == 1) 
            {
                tabControl1.SelectedTab = tabPageAddNewStock;
                tabPageClearStock.Enabled = false;
                tabPageDeleteStock.Enabled = false;
                tabPageStockStatus.Enabled = false;
                tabPageUpdateStock.Enabled = false;
            }

            else if (index == 2) 
            {
                tabControl1.SelectedTab = tabPageUpdateStock;
                generateComboItems_Brand_updt();
                tabPageAddNewStock.Enabled = false;
                tabPageDeleteStock.Enabled = false;
                tabPageStockStatus.Enabled = false;
                tabPageClearStock.Enabled = false;
            }

            else if (index == 3)
            {
                tabControl1.SelectedTab = tabPageDeleteStock;
                generateComboItems_Brand_dlt();
                tabPageAddNewStock.Enabled = false;
                tabPageClearStock.Enabled = false;
                tabPageStockStatus.Enabled = false;
                tabPageUpdateStock.Enabled = false;
            }

            else if (index == 4)
            {
                tabControl1.SelectedTab = tabPageClearStock;
                generateComboItems_Brand_clr();
                tabPageAddNewStock.Enabled = false;
                tabPageDeleteStock.Enabled = false;
                tabPageStockStatus.Enabled = false;
                tabPageUpdateStock.Enabled = false;
            }

            else if (index == 5)
            {
                tabControl1.SelectedTab = tabPageStockStatus;
                tabPageAddNewStock.Enabled = false;
                tabPageDeleteStock.Enabled = false;
                tabPageClearStock.Enabled = false;
                tabPageUpdateStock.Enabled = false;
            }

            textBoxBrandId_AddStock.Enabled = false;
            textBoxCatId_AddStock.Enabled = false;
            textBoxFuelId_AddStock.Enabled = false;
            textBoxModelId_AddStock.Enabled = false;
            textBoxPartId_AddStock.Enabled = false;
            textBoxEngineId_AddStock.Enabled = false;
            textBoxYearId_AddStock.Enabled = false;

        }

        string brand_id;
        string model_id;
        string fuel_id;
        string engine_id;
        string year;
        string year_id;
        string cat_id;
        string part_id;
        long quantity_in;
        long unit_price;
        string brand_name;
        string model_name;
        string fuel_type;
        string engine_capacity;
        string cat_name;
        string part_name;

       /* public void fillCombo() {

            MySqlDataReader newdr;
            MySqlConnection con = new MySqlConnection("Server=localhost;DATABASE=motoryard_inventory;UID=root;");
            DatabaseConnections db = new DatabaseConnections();
            newdr = db.generateBrands();
            con.Open();
            
           con.Close();
        
        }*/

        private void pictureBoxClearButton_Click(object sender, EventArgs e)
        {
            string itemCode = textBox_ItemCode_ClearStock.Text;
            string repeatitemCode = textBox_RepeatItemCode_ClearStock.Text;
            string description = textBoxDescription_ClearStock.Text;
            string date = dateTimePicker_ClearItem.Value.ToString();

            if (itemCode == repeatitemCode && (itemCode!="" || repeatitemCode!=""))
            {

                

                DatabaseConnections db = new DatabaseConnections();
                long QuantityHand = db.CheckQuantity(itemCode);

                DialogResult confirm = MessageBox.Show("ItemCode : " + itemCode + "\nQuantity on Hand : " + QuantityHand, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes && QuantityHand > 0)
                {
                    DatabaseConnections db1 = new DatabaseConnections();
                    db1.Clearstock(itemCode);
                    db1.Delete_Clear_Details(itemCode, description, date, "Clear");
                    textBox_RepeatItemCode_ClearStock.Text = null;
                    textBox_ItemCode_ClearStock.Text = null ;
                    textBoxDescription_ClearStock.Text = null;
                }

                else if(confirm == DialogResult.Yes && QuantityHand == 0)

                {
                    textBox_ItemCode_ClearStock.Text = null;
                    textBox_RepeatItemCode_ClearStock.Text =null;
                    textBoxDescription_ClearStock.Text = null;
                    MessageBox.Show("Item is Not In the Database\n         or \nItem Quantity is  0.\n\n\nCheck Item Code Again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {
                    textBox_ItemCode_ClearStock.Text = null;
                    textBox_RepeatItemCode_ClearStock.Text =null;
                    textBoxDescription_ClearStock.Text = null;
                }
            }

            else
            {
                MessageBox.Show("Can't keep Empty Fields", "Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
        }

        private void pictureBoxDeleteButton_Click(object sender, EventArgs e)
        {
            String itemCode = textBox_ItemCode_DeleteStock.Text;
            String repeatitemCode = textBox_RepeatItemCode_DeleteStock.Text;
            string description = textBoxDescription_DeleteItem.Text;
            string date = dateTimePicker_DeleteItem.Value.ToString();

            if (itemCode == repeatitemCode && (itemCode != "" || repeatitemCode != ""))
            {

                DatabaseConnections db = new DatabaseConnections();
                long QuantityHand = db.CheckQuantity(itemCode);

                if (QuantityHand != -1)
                {
                DialogResult result1 = MessageBox.Show("ItemCode : " + itemCode + "\n Item Name : " + db.getItemDetails_String(itemCode), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result1 == DialogResult.Yes && QuantityHand == 0)
                {
                    DatabaseConnections db1 = new DatabaseConnections();
                    textBox_ItemCode_DeleteStock.Text = null;
                    textBox_RepeatItemCode_DeleteStock.Text = null;
                    textBoxDescription_DeleteItem.Text = null;
                    db1.DeleteItem(itemCode);
                    db1.Delete_Clear_Details(itemCode, description, date, "Delete");
                }
                else if (result1 == DialogResult.Yes && QuantityHand > 0)
                {
                    DialogResult result = MessageBox.Show("Item Quantity is  " + QuantityHand + " Please clear the stock before delete the Item.", "Warnning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.OK)
                    {
                        Stock_Control stock = new Stock_Control(4);
                        stock.textBox_ItemCode_ClearStock.Text = itemCode;
                        stock.textBox_RepeatItemCode_ClearStock.Text = itemCode;
                        stock.textBox_RepeatItemCode_ClearStock.Hide();
                        stock.label_RepeatItemCode_ClearStock.Hide();
                        stock.Show();
                    }

                    else
                    {
                        this.Hide();
                        textBox_ItemCode_DeleteStock.Text = null;
                        textBox_RepeatItemCode_DeleteStock.Text = null;
                        textBoxDescription_DeleteItem.Text = null;
                    }
                }

               /* else if (result1 == DialogResult.Yes && QuantityHand == -1)
                {
                    MessageBox.Show("Invalid ItemCode", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox_ItemCode_DeleteStock.Text = null;
                    textBox_RepeatItemCode_DeleteStock.Text = null;
                    textBoxDescription_DeleteItem.Text = null;
                }*/


                else
                {
                    textBox_ItemCode_DeleteStock.Text = null;
                    textBox_RepeatItemCode_DeleteStock.Text = null;
                    textBoxDescription_DeleteItem.Text = null;

                }
                }else{
                    MessageBox.Show("Invalid ItemCode", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox_ItemCode_DeleteStock.Text = null;
                    textBox_RepeatItemCode_DeleteStock.Text = null;
                    textBoxDescription_DeleteItem.Text = null;
                
                }
            }

            else
            {
                MessageBox.Show("Can't keep Empty Fields", "Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
        }

        private void pictureBoxUpdateButton_Click(object sender, EventArgs e)
        {

            if (textBox_ItemCode_UpdateStock.Text != "" && textBox_QuantityIn_UpdateStock.Text != "")
            {
                string itemCode = textBox_ItemCode_UpdateStock.Text;
                string QuantityIn = textBox_QuantityIn_UpdateStock.Text;
                string date_time = dateTimePicker_UpdateStock.Value.Date.ToShortDateString();
                long Quan_in = Convert.ToInt64(QuantityIn);
                DatabaseConnections db = new DatabaseConnections();
                long QuantityHand = db.CheckQuantity(itemCode);
                string Qh = Convert.ToString(QuantityHand);
                textBox_QuantityOnHand_UpdateStock.Text = Qh;
                if (QuantityHand !=-1 && Quan_in > 0)
                {
                    DialogResult result1 = MessageBox.Show("Item Code : " + itemCode + "\nQuantity In : " + QuantityIn, "Verify Item Code and Quantity In", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (result1 == DialogResult.OK)
                    {
                        DatabaseConnections db2 = new DatabaseConnections();
                        db2.UpdateStock(itemCode, QuantityIn,date_time);
                        textBox_ItemCode_UpdateStock.Text = null;
                        textBox_QuantityIn_UpdateStock.Text = null;
                        textBox_QuantityOnHand_UpdateStock.Text = null;
                    }
                    if (result1 == DialogResult.Cancel)
                    {
                        textBox_ItemCode_UpdateStock.Text = null;
                        textBox_QuantityIn_UpdateStock.Text = null;
                        textBox_QuantityOnHand_UpdateStock.Text = null;
                    }
                }
                else if (Quan_in < 0 && QuantityHand != -1)
                {
                    MessageBox.Show("Invalid Qunanty In... Qunatity In Can't be less than 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {
                    DialogResult result2 = MessageBox.Show("Check Item code : " + itemCode, "Invalid Item Code", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (result2 == DialogResult.Retry)
                    {
                        textBox_ItemCode_UpdateStock.Text = null;
                        textBox_QuantityIn_UpdateStock.Text = null;
                        textBox_QuantityOnHand_UpdateStock.Text = null;
                    }

                    if (result2 == DialogResult.Cancel)
                    {
                        this.Hide();
                    }
                }
            }
            else
            {
                if (textBox_ItemCode_UpdateStock.Text != "")
                {
                    MessageBox.Show("Enter data to Quantity in data Field!", "Warnig", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                else if (textBox_QuantityIn_UpdateStock.Text != "")
                {
                    MessageBox.Show("Enter data to ItemCode data Field!", "Warnig", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Can't keep empty data Field!\n Enter ItemCode and Quantity In", "Warnig", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                
            }
            
        }

        private void pictureBoxAddButton_Click(object sender, EventArgs e)
        {
            if (textBoxBrandId_AddStock.Text != "" && textBoxModelId_AddStock.Text != "" && textBoxFuelId_AddStock.Text != "" && textBoxEngineId_AddStock.Text != "" && textBoxCatId_AddStock.Text != "" && textBoxPartId_AddStock.Text != "" && textBoxYearId_AddStock.Text != "" && textBoxQuantityIn_AddStock.Text != "" && textBoxUnitPrice_AddStock.Text != ""
                && comboBoxBrandName_AddStock.Text != "" && comboBoxModelName_AddStock.Text != "" && comboBoxFuelType_AddStock.Text != "" && comboBoxEngineCapacity_AddStock.Text != "" && comboBoxCatName_AddStock.Text != "" && comboBoxPartName_AddStock.Text != "" && comboBoxYear_AddStock.Text!="")
            {
                brand_id = textBoxBrandId_AddStock.Text;

                model_id = textBoxModelId_AddStock.Text;
                fuel_id = textBoxFuelId_AddStock.Text;
                engine_id = textBoxEngineId_AddStock.Text;
                year_id = textBoxYearId_AddStock.Text;
                cat_id = textBoxCatId_AddStock.Text;
                part_id = textBoxPartId_AddStock.Text;
                quantity_in = Convert.ToInt64(textBoxQuantityIn_AddStock.Text);
                unit_price = Convert.ToInt64(textBoxUnitPrice_AddStock.Text);
                year = comboBoxYear_AddStock.Text;
                brand_name = comboBoxBrandName_AddStock.Text;
                model_name = comboBoxModelName_AddStock.Text;
                fuel_type = comboBoxFuelType_AddStock.Text;
                engine_capacity = comboBoxEngineCapacity_AddStock.Text;
                cat_name = comboBoxCatName_AddStock.Text;
                part_name = comboBoxPartName_AddStock.Text;
                string date = dateTimePicker_AddStock.Value.ToString();


                DatabaseConnections db = new DatabaseConnections();
                db.AddNewStock(brand_id, brand_name, model_id, model_name, fuel_id, fuel_type, engine_id, engine_capacity, year_id, year, cat_id, cat_name, part_id, part_name, quantity_in, unit_price,date);

            }

            else
            {
                MessageBox.Show("Can't keep Empty Fields", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            textBoxBrandId_AddStock.Text = null;
            textBoxModelId_AddStock.Text = null;
            textBoxFuelId_AddStock.Text = null;
            textBoxEngineId_AddStock.Text = null;
            textBoxYearId_AddStock.Text = null;
            textBoxCatId_AddStock.Text = null;
            textBoxPartId_AddStock.Text = null;
            textBoxQuantityIn_AddStock.Text = null;
            textBoxUnitPrice_AddStock.Text = null;
            comboBoxBrandName_AddStock.Text = null;
            comboBoxModelName_AddStock.Text = null;
            comboBoxFuelType_AddStock.Text = null;
            comboBoxEngineCapacity_AddStock.Text = null;
            comboBoxYear_AddStock.Text = null; 
            comboBoxCatName_AddStock.Text = null;
            comboBoxPartName_AddStock.Text = null;
        }

        private void textBox_QuantityIn_UpdateStock_MouseClick(object sender, MouseEventArgs e)
        {
            string itemCode = textBox_ItemCode_UpdateStock.Text;
            DatabaseConnections db = new DatabaseConnections();
            long QuantityHand = db.CheckQuantity(itemCode);
            string Qh = Convert.ToString(QuantityHand);
            textBox_QuantityOnHand_UpdateStock.Text = Qh;
        }


        private void btn_checkstock_Click(object sender, EventArgs e)
        {
            //todo if a code search is needed impliment it here

            /*OleDbConnection con = new OleDbConnection();
            OleDbCommand com = new OleDbCommand();

            string connectionStr = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
            con.ConnectionString = @connectionStr;
            com.Connection = con;*/
            String sqlconnection = "Server=localhost;DATABASE=motoryard_inventory;UID=root;";
            MySqlConnection con = new MySqlConnection(sqlconnection);
            
            try
            {
                con.Open();
                String sql = "SELECT inventory_id,item_name,unit_price,quantity FROM Client_InventoryItem";

                MySqlDataAdapter dataadapter = new MySqlDataAdapter(sql, con);
             
                DataTable dt = new DataTable();
                dataadapter.Fill(dt);
                
                dataGridView1.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            };
        }

        private void tabPageStockStatus_Click(object sender, EventArgs e)
        {


        }     


// COMBO BOXES ADD_STOCK
        private void comboBoxBrandName_AddStock_TextChanged(object sender, EventArgs e)
        {

            
            string check = comboBoxBrandName_AddStock.Text;

            if (check != ""){  
                
                if(comboBoxModelName_AddStock.Items.Count!= 0)
                {
                comboBoxModelName_AddStock.Items.Clear();
                comboBoxModelName_AddStock.Text = "";
                }

                DatabaseConnections db = new DatabaseConnections();
                string ItemId1 = db.GetId(check, "Brand");
                textBoxBrandId_AddStock.Text = ItemId1;
               
                String[] reader=db.generateComboItems_Model_AddStock(ItemId1);
                int i = 0;
                while(reader[i]!=null){
                    comboBoxModelName_AddStock.Items.Add(reader[i]);
                    i++;
                
                }
            }

            else
            {
                textBoxBrandId_AddStock.Text = "";
            }
            

        }

        private void comboBoxModelName_AddStock_TextChanged(object sender, EventArgs e)
        {
           
            string check = comboBoxModelName_AddStock.Text;
            if (check != "")
            {
                DatabaseConnections db = new DatabaseConnections();
                string ItemId2 = db.GetId(check, "Model");
                textBoxModelId_AddStock.Text = ItemId2;
            }

            else
            {
                textBoxModelId_AddStock.Text = "";
            }
        }

        private void comboBoxFuelType_AddStock_TextChanged(object sender, EventArgs e)
        {
            string check = comboBoxFuelType_AddStock.Text;
            if (check != "")
            {
                DatabaseConnections db = new DatabaseConnections();
                string ItemId3 = db.GetId(check, "Fuel");
                textBoxFuelId_AddStock.Text = ItemId3;
            }

            else
            {
                textBoxFuelId_AddStock.Text = "";
            }
        }

        private void comboBoxEngineCapacity_AddStock_TextChanged(object sender, EventArgs e)
        {
            string check = comboBoxEngineCapacity_AddStock.Text;
            if (check != "")
            {
                DatabaseConnections db = new DatabaseConnections();
                string ItemId4 = db.GetId(check, "Engine");
                textBoxEngineId_AddStock.Text = ItemId4;
            }

            else
            {
                textBoxEngineId_AddStock.Text = "";
            }
        }

        private void comboBoxYear_AddStock_TextChanged(object sender, EventArgs e)
        {
            string check = comboBoxYear_AddStock.Text;
            if (check != "")
            {
                DatabaseConnections db = new DatabaseConnections();
                string ItemId5 = db.GetId(check, "Year");
                textBoxYearId_AddStock.Text = ItemId5;
            }

            else
            {
                textBoxYearId_AddStock.Text = "";
            }
        }


        private void comboBoxCatName_AddStock_TextChanged(object sender, EventArgs e)
        {
            string check = comboBoxCatName_AddStock.Text;
            if (check != "")
            {
                if (comboBoxPartName_AddStock.Items.Count != 0)
                {
                    comboBoxPartName_AddStock.Items.Clear();
                    comboBoxPartName_AddStock.Text = "";
                }

                DatabaseConnections db = new DatabaseConnections();
                string ItemId6 = db.GetId(check, "Category");

                textBoxCatId_AddStock.Text = ItemId6;
                String[] reader = db.generateComboItems_part_AddStock(ItemId6);
                int i = 0;
                while (reader[i] != null)
                {
                    comboBoxPartName_AddStock.Items.Add(reader[i]);
                    i++;

                }
            }

            else
            {
                textBoxCatId_AddStock.Text = "";
            }
        }

        private void comboBoxPartName_AddStock_TextChanged(object sender, EventArgs e)
        {
            string check = comboBoxPartName_AddStock.Text;
            if (check != "")
            {
                DatabaseConnections db = new DatabaseConnections();
                string ItemId7 = db.GetId(check, "SparePart");
                textBoxPartId_AddStock.Text = ItemId7;
            }

            else
            {
                textBoxPartId_AddStock.Text = "";
            }
        }

        private void buttonGetItemcode_GenarateItemcode_Update_Click(object sender, EventArgs e)
        {
            
            string brand_name = comboBoxBrandName_GenarateItemcode_Update.Text;
            string model_name = comboBoxModelName_GenarateItemcode_Update.Text;
            string fuel_type = comboBoxFuelType_GenarateItemcode_Update.Text;
            string engine_capacity = comboBoxEngineCapacity_GenarateItemcode_Update.Text;
            string year = comboBoxYear_GenarateItemcode_Update.Text;
            string cat_name = comboBoxCatName_GenarateItemcode_Update.Text;
            string part_name = comboBoxPartName_GenarateItemcode_Update.Text;

            string Inventory_ItemCode;

            if (brand_name != "" && model_id != "" && fuel_type != "" && engine_capacity != "" && year != "" && cat_name != "" && part_name != "")
            {

                DatabaseConnections db = new DatabaseConnections();
                string Brand_id = db.GetId(brand_name, "Brand");
                string Model_id = db.GetId(model_name, "Model");
                string Fuel_id = db.GetId(fuel_type, "Fuel");
                string Engine_Id = db.GetId(engine_capacity, "Engine");
                string Year_id = db.GetId(year, "Year");
                string Cat_id = db.GetId(cat_name, "Category");
                string Part_id = db.GetId(part_name, "SparePart");
                Inventory_ItemCode = Brand_id + Model_id + Fuel_id + Engine_Id + Year_id + Cat_id + Part_id;
                long QuantityHand = db.CheckQuantity(Inventory_ItemCode);
                if (QuantityHand >= 0)
                {
                    textBox_ItemCode_UpdateStock.Text = Inventory_ItemCode;

                    comboBoxBrandName_GenarateItemcode_Update.Text = null;
                    comboBoxModelName_GenarateItemcode_Update.Text = null;
                    comboBoxFuelType_GenarateItemcode_Update.Text = null;
                    comboBoxEngineCapacity_GenarateItemcode_Update.Text = null;
                    comboBoxYear_GenarateItemcode_Update.Text = null;
                    comboBoxCatName_GenarateItemcode_Update.Text = null;
                    comboBoxPartName_GenarateItemcode_Update.Text = null;
                }
                else
                {
                    MessageBox.Show("Check all the fiels. Invalid Itemcode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            else
            {
                MessageBox.Show("Can't keep empty fields", "Warinning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }

        private void buttonGetItemcode_GenarateItemcode_Delete_Click(object sender, EventArgs e)
        {
            

            string brand_name = comboBoxBrandName_GenarateItemcode_Delete.Text;
            string model_name = comboBoxModelName_GenarateItemcode_Delete.Text;
            string fuel_type = comboBoxFuelType_GenarateItemcode_Delete.Text;
            string engine_capacity = comboBoxEngineCapacity_GenarateItemcode_Delete.Text;
            string year = comboBoxYear_GenarateItemcode_Delete.Text;
            string cat_name = comboBoxCatName_GenarateItemcode_Delete.Text;
            string part_name = comboBoxPartName_GenarateItemcode_Delete.Text;

            string Inventory_ItemCode;

            
            
            if (brand_name != "" && model_id != "" && fuel_type != "" && engine_capacity != "" && year != "" && cat_name != "" && part_name != "")
            {
                DatabaseConnections db = new DatabaseConnections();

                string Brand_id = db.GetId(brand_name, "Brand");
                string Model_id = db.GetId(model_name, "Model");
                string Fuel_id = db.GetId(fuel_type, "Fuel");
                string Engine_Id = db.GetId(engine_capacity, "Engine");
                string Year_id = db.GetId(year, "Year");
                string Cat_id = db.GetId(cat_name, "Category");
                string Part_id = db.GetId(part_name, "SparePart");
                Inventory_ItemCode = Brand_id + Model_id + Fuel_id + Engine_Id + Year_id + Cat_id + Part_id;
                long QuantityHand = db.CheckQuantity(Inventory_ItemCode);
                if (QuantityHand >= 0)
                {
                    if(textBox_ItemCode_DeleteStock.Text.Length==21 && checkBox_Repeat_Delete.Checked)
                    {
                        textBox_RepeatItemCode_DeleteStock.Text = Inventory_ItemCode;
                        
                    }

                    else
                    {
                        textBox_ItemCode_DeleteStock.Text = Inventory_ItemCode;
                    }
                    
                    comboBoxBrandName_GenarateItemcode_Delete.Text = null;
                    comboBoxModelName_GenarateItemcode_Delete.Text = null;
                    comboBoxFuelType_GenarateItemcode_Delete.Text = null;
                    comboBoxEngineCapacity_GenarateItemcode_Delete.Text = null;
                    comboBoxYear_GenarateItemcode_Delete.Text = null;
                    comboBoxCatName_GenarateItemcode_Delete.Text = null;
                    comboBoxPartName_GenarateItemcode_Delete.Text = null;
                }
                else
                {
                    MessageBox.Show("Check all the fiels. Invalid Itemcode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Can't keep empty fields", "Warinning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void buttonGetItemcode_GenarateItemcode_Clear_Click(object sender, EventArgs e)
        {
            string brand_name = comboBoxBrandName_GenarateItemcode_Clear.Text;
            string model_name = comboBoxModelName_GenarateItemcode_Clear.Text;
            string fuel_type = comboBoxFuelType_GenarateItemcode_Clear.Text;
            string engine_capacity = comboBoxEngineCapacity_GenarateItemcode_Clear.Text;
            string year = comboBoxYear_GenarateItemcode_Clear.Text;
            string cat_name = comboBoxCatName_GenarateItemcode_Clear.Text;
            string part_name = comboBoxPartName_GenarateItemcode_Clear.Text;

            string Inventory_ItemCode;



            if (brand_name != "" && model_id != "" && fuel_type != "" && engine_capacity != "" && year != "" && cat_name != "" && part_name != "")
            {
                DatabaseConnections db = new DatabaseConnections();

                string Brand_id = db.GetId(brand_name, "Brand");
                string Model_id = db.GetId(model_name, "Model");
                string Fuel_id = db.GetId(fuel_type, "Fuel");
                string Engine_Id = db.GetId(engine_capacity, "Engine");
                string Year_id = db.GetId(year, "Year");
                string Cat_id = db.GetId(cat_name, "Category");
                string Part_id = db.GetId(part_name, "SparePart");
                Inventory_ItemCode = Brand_id + Model_id + Fuel_id + Engine_Id + Year_id + Cat_id + Part_id;
                long QuantityHand = db.CheckQuantity(Inventory_ItemCode);
                if (QuantityHand >= 0)
                {
                    if (textBox_ItemCode_ClearStock.Text.Length == 21 && checkBox_Repeat_Clear.Checked)
                    {
                        textBox_RepeatItemCode_ClearStock.Text = Inventory_ItemCode;

                    }

                    else
                    {
                        textBox_ItemCode_ClearStock.Text = Inventory_ItemCode;
                    }

                    comboBoxBrandName_GenarateItemcode_Clear.Text = null;
                    comboBoxModelName_GenarateItemcode_Clear.Text = null;
                    comboBoxFuelType_GenarateItemcode_Clear.Text = null;
                    comboBoxEngineCapacity_GenarateItemcode_Clear.Text = null;
                    comboBoxYear_GenarateItemcode_Clear.Text = null;
                    comboBoxCatName_GenarateItemcode_Clear.Text = null;
                    comboBoxPartName_GenarateItemcode_Clear.Text = null;
                }
                else
                {
                    MessageBox.Show("Check all the fiels. Invalid Itemcode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Can't keep empty fields", "Warinning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



//Update view
        public void generateComboItems_Brand_updt()
        {

            DatabaseConnections db = new DatabaseConnections();
            String[] brands = db.generateComboBrand();
            int i = 0;
            while (brands[i] != null)
            {
                comboBoxBrandName_GenarateItemcode_Update.Items.Add(brands[i]);
                i++;
            }



        }

        private void comboBoxBrandName_GenarateItemcode_Update_TextChanged(object sender, EventArgs e)
        {
            

            string check = comboBoxBrandName_GenarateItemcode_Update.Text;
            if (check != "")
            {
                if (comboBoxModelName_GenarateItemcode_Update.Items.Count != 0)
                {
                    comboBoxModelName_GenarateItemcode_Update.Items.Clear();
                    comboBoxModelName_GenarateItemcode_Update.Text="";
                }
                DatabaseConnections db = new DatabaseConnections();
                string ItemId = db.GetId(check, "Brand");
                String[] reader = db.generateComboItems_Model(ItemId);
                int i = 0;
                while (reader[i] != null)
                {
                    comboBoxModelName_GenarateItemcode_Update.Items.Add(reader[i]);
                    i++;

                }
                
               
            }

            
        
        }
        private void comboBoxModelName_GenarateItemcode_Update_TextChanged(object sender, EventArgs e)
        {


            string check = comboBoxModelName_GenarateItemcode_Update.Text;
            if (check != "")
            {
                if (comboBoxFuelType_GenarateItemcode_Update.Items.Count != 0)
                {
                    comboBoxFuelType_GenarateItemcode_Update.Items.Clear();
                    comboBoxFuelType_GenarateItemcode_Update.Text = "";
                }
                DatabaseConnections db = new DatabaseConnections();
                string ItemId = db.GetId(check, "Model");
                String[] reader = db.generateComboItems_Fuel(ItemId);
                int i = 0;
                while (reader[i] != null)
                {
                    comboBoxFuelType_GenarateItemcode_Update.Items.Add(reader[i]);
                    i++;

                }
                

            }

            


        }
        private void comboBoxFuelType_GenarateItemcode_Update_TextChanged(object sender, EventArgs e)
        {


            string check = comboBoxFuelType_GenarateItemcode_Update.Text;
            string check2 = comboBoxModelName_GenarateItemcode_Update.Text;
            if (check != "")
            {
                if (comboBoxEngineCapacity_GenarateItemcode_Update.Items.Count != 0)
                {
                    comboBoxEngineCapacity_GenarateItemcode_Update.Items.Clear();
                    comboBoxEngineCapacity_GenarateItemcode_Update.Text = "";
                }
                DatabaseConnections db = new DatabaseConnections();
                string ItemId = db.GetId(check, "Fuel");
                string ItemId2 = db.GetId(check2, "Model");
                String[] reader = db.generateComboItems_Engine(ItemId,ItemId2);
                int i = 0;
                while (reader[i] != null)
                {
                    comboBoxEngineCapacity_GenarateItemcode_Update.Items.Add(reader[i]);
                    i++;

                }
               

            }

            


        }
        private void comboBoxEngineCapacity_GenarateItemcode_Update_TextChanged(object sender, EventArgs e)
        {


            string check = comboBoxEngineCapacity_GenarateItemcode_Update.Text;
            string check2=comboBoxModelName_GenarateItemcode_Update.Text;
            if (check != "")
            {
                if (comboBoxYear_GenarateItemcode_Update.Items.Count != 0)
                {
                    comboBoxYear_GenarateItemcode_Update.Items.Clear();
                    comboBoxYear_GenarateItemcode_Update.Text = "";
                }
                DatabaseConnections db = new DatabaseConnections();

                string ItemId = db.GetId(check, "Engine");
                string ItemId2 = db.GetId(check2, "Model");
                String[] reader = db.generateComboItems_Year(ItemId,ItemId2);
                int i = 0;
                while (reader[i] != null)
                {
                    comboBoxYear_GenarateItemcode_Update.Items.Add(reader[i]);
                    i++;

                }
             

            }

            


        }
         private void comboBoxYear_GenarateItemcode_Update_TextChanged(object sender, EventArgs e)
          {


              string check = comboBoxYear_GenarateItemcode_Update.Text;
              string check2 = comboBoxModelName_GenarateItemcode_Update.Text;

              if (check != "")
              {
                  if (comboBoxCatName_GenarateItemcode_Update.Items.Count != 0)
                  {
                      comboBoxCatName_GenarateItemcode_Update.Items.Clear();
                      comboBoxCatName_GenarateItemcode_Update.Text = "";
                  }
                  DatabaseConnections db = new DatabaseConnections();
                  string ItemId = db.GetId(check, "Year");
                  string ItemId2 = db.GetId(check2, "Model");

                  String[] reader = db.generateComboItems_Cat(ItemId,ItemId2);
                  int i = 0;
             
                  while (reader[i] != null)
                  {
                      comboBoxCatName_GenarateItemcode_Update.Items.Add(reader[i]);
                      i++;

                  }
                 

              }

              


          }
          private void comboBoxCatName_GenarateItemcode_Update_TextChanged(object sender, EventArgs e)
         {

             string check2= comboBoxCatName_GenarateItemcode_Update.Text;
             string check = comboBoxModelName_GenarateItemcode_Update.Text;
             if (check != "")
             {
                 if (comboBoxPartName_GenarateItemcode_Update.Items.Count != 0)
                 {
                     comboBoxPartName_GenarateItemcode_Update.Items.Clear();
                     comboBoxPartName_GenarateItemcode_Update.Text = "";
                 }
                 DatabaseConnections db = new DatabaseConnections();
                 string ItemId = db.GetId(check, "Model");
                 string ItemId2 = db.GetId(check2,"Category");
                 String[] reader = db.generateComboItems_Part(ItemId2,ItemId);
                 int i = 0;
                 while (reader[i] != null)
                 {
                     comboBoxPartName_GenarateItemcode_Update.Items.Add(reader[i]);
                     i++;

                 }
               

             }

             


         }
      
        



    

        // Delete View
        public void generateComboItems_Brand_dlt()
        {

            DatabaseConnections db = new DatabaseConnections();
            String[] brands = db.generateComboBrand();
            int i = 0;
            while (brands[i] != null)
            {
                comboBoxBrandName_GenarateItemcode_Delete.Items.Add(brands[i]);
                i++;
            }



        }





        private void comboBoxBrandName_GenarateItemcode_Delete_TextChanged(object sender, EventArgs e)
        {
            

            string check = comboBoxBrandName_GenarateItemcode_Delete.Text;
            if (check != "")
            {
                if (comboBoxModelName_GenarateItemcode_Delete.Items.Count != 0)
                {
                    comboBoxModelName_GenarateItemcode_Delete.Items.Clear();
                    comboBoxModelName_GenarateItemcode_Delete.Text="";
                }
                DatabaseConnections db = new DatabaseConnections();
                string ItemId = db.GetId(check, "Brand");
                String[] reader = db.generateComboItems_Model(ItemId);
                int i = 0;
                while (reader[i] != null)
                {
                    comboBoxModelName_GenarateItemcode_Delete.Items.Add(reader[i]);
                    i++;

                }
                
               
            }

            
        
        }
        private void comboBoxModelName_GenarateItemcode_Delete_TextChanged(object sender, EventArgs e)
        {


            string check = comboBoxModelName_GenarateItemcode_Delete.Text;
            if (check != "")
            {
                if (comboBoxFuelType_GenarateItemcode_Delete.Items.Count != 0)
                {
                    comboBoxFuelType_GenarateItemcode_Delete.Items.Clear();
                    comboBoxFuelType_GenarateItemcode_Delete.Text = "";
                }
                DatabaseConnections db = new DatabaseConnections();
                string ItemId = db.GetId(check, "Model");
                String[] reader = db.generateComboItems_Fuel(ItemId);
                int i = 0;
                while (reader[i] != null)
                {
                    comboBoxFuelType_GenarateItemcode_Delete.Items.Add(reader[i]);
                    i++;

                }
                

            }

            


        }
        private void comboBoxFuelType_GenarateItemcode_Delete_TextChanged(object sender, EventArgs e)
        {


            string check = comboBoxFuelType_GenarateItemcode_Delete.Text;
            string check2 = comboBoxModelName_GenarateItemcode_Delete.Text;
            if (check != "")
            {
                if (comboBoxEngineCapacity_GenarateItemcode_Delete.Items.Count != 0)
                {
                    comboBoxEngineCapacity_GenarateItemcode_Delete.Items.Clear();
                    comboBoxEngineCapacity_GenarateItemcode_Delete.Text = "";
                }
                DatabaseConnections db = new DatabaseConnections();
                string ItemId = db.GetId(check, "Fuel");
                string ItemId2 = db.GetId(check2, "Model");
                String[] reader = db.generateComboItems_Engine(ItemId,ItemId2);
                int i = 0;
                while (reader[i] != null)
                {
                    comboBoxEngineCapacity_GenarateItemcode_Delete.Items.Add(reader[i]);
                    i++;

                }
               

            }

            


        }
        private void comboBoxEngineCapacity_GenarateItemcode_Delete_TextChanged(object sender, EventArgs e)
        {


            string check = comboBoxEngineCapacity_GenarateItemcode_Delete.Text;
            string check2=comboBoxModelName_GenarateItemcode_Delete.Text;
            if (check != "")
            {
                if (comboBoxYear_GenarateItemcode_Delete.Items.Count != 0)
                {
                    comboBoxYear_GenarateItemcode_Delete.Items.Clear();
                    comboBoxYear_GenarateItemcode_Delete.Text = "";
                }
                DatabaseConnections db = new DatabaseConnections();

                string ItemId = db.GetId(check, "Engine");
                string ItemId2 = db.GetId(check2, "Model");
                String[] reader = db.generateComboItems_Year(ItemId,ItemId2);
                int i = 0;
                while (reader[i] != null)
                {
                    comboBoxYear_GenarateItemcode_Delete.Items.Add(reader[i]);
                    i++;

                }
             

            }

            


        }
         private void comboBoxYear_GenarateItemcode_Delete_TextChanged(object sender, EventArgs e)
          {


              string check = comboBoxYear_GenarateItemcode_Delete.Text;
              string check2 = comboBoxModelName_GenarateItemcode_Delete.Text;

              if (check != "")
              {
                  if (comboBoxCatName_GenarateItemcode_Delete.Items.Count != 0)
                  {
                      comboBoxCatName_GenarateItemcode_Delete.Items.Clear();
                      comboBoxCatName_GenarateItemcode_Delete.Text = "";
                  }
                  DatabaseConnections db = new DatabaseConnections();
                  string ItemId = db.GetId(check, "Year");
                  string ItemId2 = db.GetId(check2, "Model");

                  String[] reader = db.generateComboItems_Cat(ItemId,ItemId2);
                  int i = 0;
             
                  while (reader[i] != null)
                  {
                      comboBoxCatName_GenarateItemcode_Delete.Items.Add(reader[i]);
                      i++;

                  }
                 

              }

              


          }
         private void comboBoxCatName_GenarateItemcode_Delete_TextChanged(object sender, EventArgs e)
         {

             string check2 = comboBoxCatName_GenarateItemcode_Delete.Text;
             string check = comboBoxModelName_GenarateItemcode_Delete.Text;
             if (check != "")
             {
                 if (comboBoxPartName_GenarateItemcode_Delete.Items.Count != 0)
                 {
                     comboBoxPartName_GenarateItemcode_Delete.Items.Clear();
                     comboBoxPartName_GenarateItemcode_Delete.Text = "";
                 }
                 DatabaseConnections db = new DatabaseConnections();
                 string ItemId = db.GetId(check, "Model");
                 string ItemId2 = db.GetId(check2, "Category");
                 String[] reader = db.generateComboItems_Part(ItemId2, ItemId);
                 int i = 0;
                 while (reader[i] != null)
                 {
                     comboBoxPartName_GenarateItemcode_Delete.Items.Add(reader[i]);
                     i++;

                 }


             }
         }

             


         





        
// Clear-stock view 
        public void generateComboItems_Brand_clr()
        {

            DatabaseConnections db = new DatabaseConnections();
            String[] brands = db.generateComboBrand();
            int i = 0;
            while (brands[i] != null)
            {
                comboBoxBrandName_GenarateItemcode_Clear.Items.Add(brands[i]);
                i++;
            }



        }



        private void comboBoxBrandName_GenarateItemcode_Clear_TextChanged(object sender, EventArgs e)
        {


            string check = comboBoxBrandName_GenarateItemcode_Clear.Text;
            if (check != "")
            {
                if (comboBoxModelName_GenarateItemcode_Clear.Items.Count != 0)
                {
                    comboBoxModelName_GenarateItemcode_Clear.Items.Clear();
                    comboBoxModelName_GenarateItemcode_Clear.Text = "";
                }
                DatabaseConnections db = new DatabaseConnections();
                string ItemId = db.GetId(check, "Brand");
                String[] reader = db.generateComboItems_Model(ItemId);
                int i = 0;
                while (reader[i] != null)
                {
                    comboBoxModelName_GenarateItemcode_Clear.Items.Add(reader[i]);
                    i++;

                }


            }



        }
        private void comboBoxModelName_GenarateItemcode_Clear_TextChanged(object sender, EventArgs e)
        {


            string check = comboBoxModelName_GenarateItemcode_Clear.Text;
            if (check != "")
            {
                if (comboBoxFuelType_GenarateItemcode_Clear.Items.Count != 0)
                {
                    comboBoxFuelType_GenarateItemcode_Clear.Items.Clear();
                    comboBoxFuelType_GenarateItemcode_Clear.Text = "";
                }
                DatabaseConnections db = new DatabaseConnections();
                string ItemId = db.GetId(check, "Model");
                String[] reader = db.generateComboItems_Fuel(ItemId);
                int i = 0;
                while (reader[i] != null)
                {
                    comboBoxFuelType_GenarateItemcode_Clear.Items.Add(reader[i]);
                    i++;

                }


            }




        }
        private void comboBoxFuelType_GenarateItemcode_Clear_TextChanged(object sender, EventArgs e)
        {


            string check = comboBoxFuelType_GenarateItemcode_Clear.Text;
            string check2 = comboBoxModelName_GenarateItemcode_Clear.Text;
            if (check != "")
            {
                if (comboBoxEngineCapacity_GenarateItemcode_Clear.Items.Count != 0)
                {
                    comboBoxEngineCapacity_GenarateItemcode_Clear.Items.Clear();
                    comboBoxEngineCapacity_GenarateItemcode_Clear.Text = "";
                }
                DatabaseConnections db = new DatabaseConnections();
                string ItemId = db.GetId(check, "Fuel");
                string ItemId2 = db.GetId(check2, "Model");
                String[] reader = db.generateComboItems_Engine(ItemId, ItemId2);
                int i = 0;
                while (reader[i] != null)
                {
                    comboBoxEngineCapacity_GenarateItemcode_Clear.Items.Add(reader[i]);
                    i++;

                }


            }




        }
        private void comboBoxEngineCapacity_GenarateItemcode_Clear_TextChanged(object sender, EventArgs e)
        {


            string check = comboBoxEngineCapacity_GenarateItemcode_Clear.Text;
            string check2 = comboBoxModelName_GenarateItemcode_Clear.Text;
            if (check != "")
            {
                if (comboBoxYear_GenarateItemcode_Clear.Items.Count != 0)
                {
                    comboBoxYear_GenarateItemcode_Clear.Items.Clear();
                    comboBoxYear_GenarateItemcode_Clear.Text = "";
                }
                DatabaseConnections db = new DatabaseConnections();

                string ItemId = db.GetId(check, "Engine");
                string ItemId2 = db.GetId(check2, "Model");
                String[] reader = db.generateComboItems_Year(ItemId, ItemId2);
                int i = 0;
                while (reader[i] != null)
                {
                    comboBoxYear_GenarateItemcode_Clear.Items.Add(reader[i]);
                    i++;

                }


            }




        }
        private void comboBoxYear_GenarateItemcode_Clear_TextChanged(object sender, EventArgs e)
        {


            string check = comboBoxYear_GenarateItemcode_Clear.Text;
            string check2 = comboBoxModelName_GenarateItemcode_Clear.Text;

            if (check != "")
            {
                if (comboBoxCatName_GenarateItemcode_Clear.Items.Count != 0)
                {
                    comboBoxCatName_GenarateItemcode_Clear.Items.Clear();
                    comboBoxCatName_GenarateItemcode_Clear.Text = "";
                }
                DatabaseConnections db = new DatabaseConnections();
                string ItemId = db.GetId(check, "Year");
                string ItemId2 = db.GetId(check2, "Model");

                String[] reader = db.generateComboItems_Cat(ItemId, ItemId2);
                int i = 0;

                while (reader[i] != null)
                {
                    comboBoxCatName_GenarateItemcode_Clear.Items.Add(reader[i]);
                    i++;

                }


            }




        }
        private void comboBoxCatName_GenarateItemcode_Clear_TextChanged(object sender, EventArgs e)
        {

            string check2 = comboBoxCatName_GenarateItemcode_Clear.Text;
            string check = comboBoxModelName_GenarateItemcode_Clear.Text;
            if (check != "")
            {
                if (comboBoxPartName_GenarateItemcode_Clear.Items.Count != 0)
                {
                    comboBoxPartName_GenarateItemcode_Clear.Items.Clear();
                    comboBoxPartName_GenarateItemcode_Clear.Text = "";
                }
                DatabaseConnections db = new DatabaseConnections();
                string ItemId = db.GetId(check, "Model");
                string ItemId2 = db.GetId(check2, "Category");
                String[] reader = db.generateComboItems_Part(ItemId2, ItemId);
                int i = 0;
                while (reader[i] != null)
                {
                    comboBoxPartName_GenarateItemcode_Clear.Items.Add(reader[i]);
                    i++;

                }


            }
        }
       
       
        

    }
}
