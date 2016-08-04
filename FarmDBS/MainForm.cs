using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace FarmDBS
{
    public partial class MainForm : Form
    {
        //attributes
        string connectionSTR;
        
        // DB connection object
        DBConnection dbCon;   

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {            
            // get the connectionString from the Settings set up earlier 
            connectionSTR = Properties.Settings.Default.DatabaseConnection;
            
            // RB DBConnection object
            dbCon = new DBConnection(connectionSTR);

            // MM harvest tab load
            dbCon.loadHarvest(harvestTimetableDGV, harvestEmployeeDGV, harvestContainerDGV, harvestCropDGV, harvestVehicleDGV);

            // MM container tab load
            dbCon.loadContainer(containerDGV);
            loadContainerProgress();

            // MM vehicle tab load
            dbCon.loadVehicle(vehicleDGV);
            loadVehicleProgress();

            // RB set all tabs as inactive until user authentication is confirmed
            tabPageEmployee.Enabled = false;
            tabPageCustomer.Enabled = false;
            tabPageCrop.Enabled = false;
            tabPageFertiliser.Enabled = false;
            tabPageContainers.Enabled = false;
            tabPageHarvest.Enabled = false;
            tabPageSow.Enabled = false;
            tabPageTreatment.Enabled = false;
            tabPageVehicle.Enabled = false;
            tabEmployeeReports.Enabled = false;
            tabManagerReports.Enabled = false;

            // RB set focus on login text field
            loginNameTXTBOX.Select();
        }

        // RB >>

        #region Employee Tab

        #region Display All Employees Button
        // RB Display All Employees from DB Method
        private void displayAllEmployeesBTN_Click(object sender, EventArgs e)
        {
            dbCon.DisplayAllEmployees(employeeDGV);
        }
        #endregion Display All Employees Button

        #region Insert New Employee Button
        private void InsertEmployeeBTN_Click(object sender, EventArgs e)
        {
            saveEmployeeTextFields();
            dbCon.DisplayAllEmployees(employeeDGV);
        }
        #endregion Insert New Employee Button

        #region Delete Employee Button
        private void employeeDeleteBTN_Click(object sender, EventArgs e)
        {
            dbCon.DeleteEmployeeRow(employeeDGV);
            dbCon.DisplayAllEmployees(employeeDGV);
            ClearEmployeeTextBoxes();
        }
        #endregion Delete Employee Button

        #region Update Employee Button
        private void EmployeeUpdateBTN_Click(object sender, EventArgs e)
        {
            updateEmployeeTextFields();
            dbCon.DisplayAllEmployees(employeeDGV);
        }
        #endregion Update Employee Button

        #region Clear Employee Text Box Button
        private void ClearEmployeeTXTBoxesBTN_Click(object sender, EventArgs e)
        {
            ClearEmployeeTextBoxes();
        }
        #endregion Clear Employee Button

        #region Employee DGV Mouse Click Event
        private void employeeDGV_MouseClick(object sender, MouseEventArgs e)
        {
            // Assign text Boxes to the cells from DB - if no records found display Error Message
            try
            {
                employeeIDTextBox.Text = employeeDGV.SelectedRows[0].Cells[0].Value.ToString();
                employeeFirstNameTextBox.Text = employeeDGV.SelectedRows[0].Cells[1].Value.ToString();
                employeeLastNameTextBox.Text = employeeDGV.SelectedRows[0].Cells[2].Value.ToString();
                employeeAddressTextBox.Text = employeeDGV.SelectedRows[0].Cells[3].Value.ToString();
                employeePrivilegeLevelTextBox.Text = employeeDGV.SelectedRows[0].Cells[4].Value.ToString();
                employeeJobRoleTextBox.Text = employeeDGV.SelectedRows[0].Cells[5].Value.ToString();
                employeeHourlyRateTextBox.Text = employeeDGV.SelectedRows[0].Cells[6].Value.ToString();
                employeeFirstNameTextBox.Select();
            }
            catch
            {
                MessageBox.Show("No Records in Database", "Database Empty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        #endregion Employee DGV Mouse Click Event

        #region Saves Text Fields & Insert Employee Records into DB Method
        /// <summary>
        /// Create object of new employee and inserts data into database
        /// </summary>
        private void saveEmployeeTextFields()
        {
            // Check if Name is not empty
            if (employeeFirstNameTextBox.Text == "" || employeeLastNameTextBox.Text == "")
            {
                MessageBox.Show("Name cannot be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    // RB If User wont type numeric values they will be set to 0 as default
                    if (employeePrivilegeLevelTextBox.Text == "" || employeeHourlyRateTextBox.Text == "")
                    {
                        employeePrivilegeLevelTextBox.Text = "0";
                        employeeHourlyRateTextBox.Text = "0";
                    }
                    else
                    {
                        // RB try to convert user input into numeric values
                        int privilege = int.Parse(employeePrivilegeLevelTextBox.Text);
                        double ratePHour = double.Parse(employeeHourlyRateTextBox.Text);
                        // RB Create employee object
                        Employee employee = new Employee(0, employeeFirstNameTextBox.Text, employeeLastNameTextBox.Text, employeeAddressTextBox.Text, privilege, employeeJobRoleTextBox.Text, ratePHour);

                        // RB Use Sql query to store new employee into database 
                        dbCon.InsertRowIntoEmployeeTable(employee);
                        MessageBox.Show(employeeFirstNameTextBox.Text + " " + employeeLastNameTextBox.Text + " has been added to the Database", "New Employee Added");
                        // RB Clears Employee text boxes
                        ClearEmployeeTextBoxes();
                    }
                }
                catch (Exception)
                {
                    // RB Message Will be displayed in non numeric values are anetered as privilege level or hourly rate
                    MessageBox.Show("Numeric Values Only!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion Saves Text Fields & Insert Employee Records into DB Method

        #region Update Employee Records Method
        private void updateEmployeeTextFields()
        {
            // RB Check if Name is not empty
            if (employeeFirstNameTextBox.Text == "" || employeeLastNameTextBox.Text == "")
            {
                MessageBox.Show("Name cannot be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    // RB If User wont type numeric values they will be set to 0 as default
                    if (employeePrivilegeLevelTextBox.Text == "" || employeeHourlyRateTextBox.Text == "")
                    {
                        employeePrivilegeLevelTextBox.Text = "0";
                        employeeHourlyRateTextBox.Text = "0";
                    }
                    else
                    {
                        // RB try to convert user input into numeric values
                        int priviledge = int.Parse(employeePrivilegeLevelTextBox.Text);
                        double ratePHour = double.Parse(employeeHourlyRateTextBox.Text);
                        int employeeID = int.Parse(employeeIDTextBox.Text);

                        // RB Create ne employee object
                        Employee employee = new Employee(employeeID, employeeFirstNameTextBox.Text, employeeLastNameTextBox.Text, employeeAddressTextBox.Text, priviledge, employeeJobRoleTextBox.Text, ratePHour);

                        // RB Use Sql query to store new employee into database 
                        dbCon.UpdateRowIntoEmployeeTable(employeeDGV, employee);
                        MessageBox.Show(employeeFirstNameTextBox.Text + " " + employeeLastNameTextBox.Text + " has been updated in the Database");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Numeric Values Only!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion Update Employee Record Method

        #region Clear Employee Text Boxes Method
        public void ClearEmployeeTextBoxes()
        {
            // RB sets all text boxes value to empty string
            employeeIDTextBox.Text = "";
            employeeFirstNameTextBox.Text = "";
            employeeLastNameTextBox.Text = "";
            employeeAddressTextBox.Text = "";
            employeePrivilegeLevelTextBox.Text = "";
            employeeJobRoleTextBox.Text = "";
            employeeHourlyRateTextBox.Text = "";

            // RB Set focus to Customer Name Field
            employeeFirstNameTextBox.Select();
        }
        #endregion Clear Employee Text Boxes Method

        #endregion Employee Tab

        #region Customer Tab

        #region Display All Customers Button
        private void DisplayAllCustomersBTN_Click(object sender, EventArgs e)
        {
            dbCon.DisplayAllCustomers(customerDGV);
        }
        #endregion Display All Customers Button

        #region Add new Customer Button
        private void InsertNewCustomerBTN_Click(object sender, EventArgs e)
        {
            saveCustomerTextFields();
            dbCon.DisplayAllCustomers(customerDGV);
        }
        #endregion Add new Customer Button

        #region Delete Customer Button
        private void DeleteCustomerBTN_Click(object sender, EventArgs e)
        {
            dbCon.DeleteCustomer(employeeDGV);
            dbCon.DisplayAllCustomers(employeeDGV);
            ClearCustomerTextBoxes();
        }
        #endregion Delete Customer Button

        #region Update Customer Records Button
        private void UpdateCustomerBTN_Click(object sender, EventArgs e)
        {
            updateCustomerTextFields();
            dbCon.DisplayAllCustomers(customerDGV);
        }
        #endregion Update Customer Records Button

        #region Clear Customer Text Boxes Button
        private void ClearCustomerTextBoxesBTN_Click(object sender, EventArgs e)
        {
            ClearCustomerTextBoxes();
        }
        #endregion Clear Customer Text Boxes Button
        
        #region Customer DGV Mouse Click Event
        private void customerDGV_MouseClick(object sender, MouseEventArgs e)
        {
            // RB Assign text Boxes to the cells in DB - if no records found display Error Message
            try
            {
                CustomerIDTextBox.Text = customerDGV.SelectedRows[0].Cells[0].Value.ToString();
                CustomerNameTextBox.Text = customerDGV.SelectedRows[0].Cells[1].Value.ToString();
                CustomerLastNameTextBox.Text = customerDGV.SelectedRows[0].Cells[2].Value.ToString();
                CustomerAddressTextBox.Text = customerDGV.SelectedRows[0].Cells[3].Value.ToString();
                employeeFirstNameTextBox.Select();
            }
            catch
            {
                MessageBox.Show("No Records in Database", "Database Empty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        #endregion Customer DGV Mouse Click Event

        #region Saves Text Fields & Insert Customer Records into DB Method
        /// <summary>
        /// Create object of new Customer and inserts data into database
        /// </summary>
        private void saveCustomerTextFields()
        {

            // RB Check if Name is not empty
            if (CustomerNameTextBox.Text == "" || CustomerLastNameTextBox.Text == "")
            {
                MessageBox.Show("Name cannot be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                { 
                    // RB Create Customer object
                    Customer customer = new Customer(0, CustomerNameTextBox.Text, CustomerLastNameTextBox.Text, CustomerAddressTextBox.Text);
                    
                    // RB Use Sql query to store new employee into database 
                    dbCon.InsertRowIntoCustomerTable(customer);

                    MessageBox.Show(CustomerNameTextBox.Text + " " + CustomerLastNameTextBox.Text + " has been added to the Database", "New Record Added");
                    ClearCustomerTextBoxes();
                    
                }
                catch (Exception)
                {
                    MessageBox.Show("Something Went Wrong", "Ooops", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion Saves Text Fields & Insert Customer Records into DB Method

        #region Clear Customer Text Boxes Method
        public void ClearCustomerTextBoxes()
        {
            // RB sets all text boxes value to empty string
            CustomerIDTextBox.Text = "";
            CustomerNameTextBox.Text = "";
            CustomerLastNameTextBox.Text = "";
            CustomerAddressTextBox.Text = "";

            // RB Set focus to Customer Name Field
            CustomerNameTextBox.Select();
        }
        #endregion Clear Customer Text Boxes Method

        #region Update Customer Records Method
        private void updateCustomerTextFields()
        {
            // RB Check if Name is not empty
            if (CustomerNameTextBox.Text == "" || CustomerLastNameTextBox.Text == "")
            {
                MessageBox.Show("Name cannot be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    // RB Create ne employee object
                    Customer customer = new Customer(0, CustomerNameTextBox.Text, CustomerLastNameTextBox.Text, CustomerAddressTextBox.Text);
                    
                    // RB Use Sql query to store new employee into database 
                    dbCon.UpdateRowIntoCustomerTable(customerDGV, customer);
                    MessageBox.Show(CustomerNameTextBox.Text + " " + CustomerLastNameTextBox.Text + " has been updated in the Database");
                    ClearCustomerTextBoxes();
                }
                catch (Exception)
                {
                    MessageBox.Show("Something went wrong!", "Ooops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion Update Customer Record Method
              
        #endregion Customer Tab

        #region Login Control
        private void loginBTN_Click(object sender, EventArgs e)
        {
            Login();
        }
        #endregion Login Control

        #region Login
        private void Login()
        {
            // RB open connection to DB
            dbCon.openConnection();

            // RB Create SQL Command
            SqlCommand login = new SqlCommand();
            login.CommandType = CommandType.Text;

            // RB assign login query
            login.CommandText = SQLQuery.loginQuery;
            login.Connection = dbCon.connectionToDB;

            // RB assign login text boxes to variables
            string loginName = loginNameTXTBOX.Text;
            string loginID = loginEmployeeID.Text;

            // RB Set parameters for the SQL Statement   
            login.Parameters.AddWithValue("@personID", loginID);
            login.Parameters.AddWithValue("@firstName", loginName);

            // RB try to get user privilege level using Execute Scalar
            try
            {
                string privilege = login.ExecuteScalar().ToString();

                // RB create SQL Data Reader object
                SqlDataReader reader = login.ExecuteReader();

                // RB if records exist in DB and privilege level is 9 (managerial)
                if (reader.HasRows == true && privilege == "9")
                {
                    // RB display succesful message to user
                    MessageBox.Show("Welcome " + loginName + "! " + "\n\n" + "You are now logged in with MANAGERIAL Credentials", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // RB enable all tabs
                    tabPageEmployee.Enabled = true;
                    tabPageCustomer.Enabled = true;
                    tabPageCrop.Enabled = true;
                    tabPageFertiliser.Enabled = true;
                    tabPageContainers.Enabled = true;
                    tabPageHarvest.Enabled = true;
                    tabPageSow.Enabled = true;
                    tabPageTreatment.Enabled = true;
                    tabPageVehicle.Enabled = true;
                    tabEmployeeReports.Enabled = true;
                    tabManagerReports.Enabled = true;
                }
                // RB if records exist in DB and privilege level is non managerial (different that 9)
                else if (reader.HasRows == true && privilege != "9")
                {
                    // RB display succesful message to user
                    MessageBox.Show("Welcome " + loginName + "! " + "\n\n" + "You are now logged in with Employee Credentials", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // RB disable tabs
                    tabPageEmployee.Enabled = false;
                    tabPageCustomer.Enabled = false;
                    tabPageCrop.Enabled = false;
                    tabPageFertiliser.Enabled = false;
                    tabPageContainers.Enabled = false;
                    tabPageHarvest.Enabled = false;
                    tabPageSow.Enabled = false;
                    tabPageTreatment.Enabled = false;
                    tabPageVehicle.Enabled = false;
                    tabManagerReports.Enabled = false;

                    // RB enable employee reports tab
                    tabEmployeeReports.Enabled = true;
                }
            }
            // RB catch block if login details are incorect
            catch
            {
                // RB display error message to user
                MessageBox.Show("User Authentication FAILED!", "Wrong Credentials", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                // RB close connection to DB
                dbCon.closeConnection();

                // RB clear login text boxes
                loginNameTXTBOX.Text = "";
                loginEmployeeID.Text = "";

                // RB display message to the who is logged in
                afterLoginLBL1.Text = "You are now logged in as : ";
                afterLoginLBL2.Text = loginName.ToString();
            }
        }
        #endregion Login

        #region Employee and Manager Reports Buttons

        // RB Button - Display Available Cointainers - EMPLOYEE
        private void empContAVL_BTN_Click(object sender, EventArgs e)
        {
            dbCon.employeeContainerReport(employeeReportsDGV);
        }

        // RB Button - Display Treatment Report - EMPLOYEE
        private void emp_treatmentReport_BTN_Click(object sender, EventArgs e)
        {
            dbCon.employeeTreatmentReport(employeeReportsDGV);
        }
        // RB Button - Display Crop in Cultivation - MANAGER
        private void mgr_CropINCult_BTN_Click(object sender, EventArgs e)
        {
            dbCon.managerRCropInCultivation(managerReportsDGV);
        }

        // RB Button - Display harvest timetable - MANAGER
        private void mgr_harvestTT_BTN_Click(object sender, EventArgs e)
        {
            dbCon.managerRHarvestTimeTable(managerReportsDGV);
        }

        // RB Button - Display Labour Report - MANAGER
        private void mgr_lbr_forHarvest_BTN_Click(object sender, EventArgs e)
        {
            dbCon.managerRHarvestLabour(managerReportsDGV);
        }

        // RB Button - Display Vehicles needed for harvest - MANAGER
        private void mgr_vehForHarvest_BTN_Click(object sender, EventArgs e)
        {
            dbCon.managerRHarvestVehicles(managerReportsDGV);
        }

        // RB Button - Display Fertiliser Stock Level - MANAGER
        private void mgr_FertStockLVL_BTN_Click(object sender, EventArgs e)
        {
            dbCon.managerRFertiliserStock(managerReportsDGV);
        }

        // RB Button - Display Crop Stock Level - MANAGER
        private void mgr_cropStockLVL_BTN_Click(object sender, EventArgs e)
        {
            dbCon.managerRCropStock(managerReportsDGV);
        }
        #endregion Employee and Manager Reports Buttons

        // RB <<

        // MM >>
        #region Container

        #region Add New Container BTN

        private void addContainerBTN_Click_1(object sender, EventArgs e)
        {
            // MM cheack if type of container provided by the user
            if (containerTypeTextBox.Text == string.Empty)
            {
                MessageBox.Show("Please input container type");
            }
            else
            {
                // MM check if radioButton In Use is selected otherwise will be set to Available
                string availability;
                if (containerInUseRB.Checked == true)
                {
                    availability = "In Use";
                }
                else
                {
                    availability = "Available";
                }
                // MM create Container instance with user selected parameters
                string type = containerTypeTextBox.Text;
                Container container = new Container(type, availability);

                // MM adding new instance to database
                int rows = dbCon.insertRowContainer(container);
                if (rows == DBConnection.errNoRowInserted)
                    MessageBox.Show(DBConnection.errorInsertInTableStr);
                else
                {
                    // MM reload whole Container tab
                    dbCon.loadContainer(containerDGV);
                    clearContainer();
                    loadContainerProgress();
                }
            }

        }


        #endregion Add New Container BTN

        #region Delete Container
        private void removeContainerBTN_Click_1(object sender, EventArgs e)
        {
            // MM check if row is selected in DataGridView
            DataGridViewSelectedRowCollection selectedRows = containerDGV.SelectedRows;
            if (selectedRows.Count > 0)
            {
                // MM removing container from database at selected index
                int id = int.Parse(containerDGV.SelectedRows[0].Cells["ID"].Value.ToString());
                int rows = dbCon.removeContainerDB(id);

                // MM reloading whole container tab
                dbCon.loadContainer(containerDGV);
                clearContainer();
                loadContainerProgress();
            }
            else
            {
                MessageBox.Show("Please select the row you want to delete");
            }
        }
        #endregion Delete Container

        #region Update Container

        private void containerDGV_MouseClick(object sender, MouseEventArgs e)
        {
            // MM filling the texboxes from selected DataGridViewItem parameters 
            containerIDtextBox.Text = containerDGV.SelectedRows[0].Cells[0].Value.ToString();
            containerTypeTextBox.Text = containerDGV.SelectedRows[0].Cells[1].Value.ToString();
            string availability = containerDGV.SelectedRows[0].Cells[2].Value.ToString();
            if (availability == "Available")
            {
                containerAvailableRB.Checked = true;
            }
            else
            {
                containerInUseRB.Checked = true;
            }
        }

        private void updateContanerBTN_Click(object sender, EventArgs e)
        {
            // MM creating new Container instance for update with user parameters
            int id = int.Parse(containerDGV.SelectedRows[0].Cells[0].Value.ToString());
            string type = containerTypeTextBox.Text;
            string availability;
            if (containerInUseRB.Checked)
            {
                availability = "In Use";
            }
            else
            {
                availability = "Available";
            }
            Container container = new Container(id, type, availability);
            // MM Updating database
            dbCon.updateContainer(container);

            // MM Reload whole container tab
            dbCon.loadContainer(containerDGV);
            clearContainer();
            loadContainerProgress();
        }

        #endregion Update Container

        #region Clear Container tab
        /// <summary>
        /// MM Unselecting radio buttons and clearing textboxes 
        /// </summary>
        public void clearContainer()
        {
            containerIDtextBox.Text = string.Empty;
            containerTypeTextBox.Text = string.Empty;
            containerInUseRB.Checked = false;
            containerAvailableRB.Checked = false;
        }

        private void clearContainerBTN_Click(object sender, EventArgs e)
        {
            clearContainer();
        }

        private void containerInUseBar_Click(object sender, EventArgs e)
        {
            loadContainerProgress();
        }

        #endregion Clear Container tab

        #region Container Progress Bar
        /// <summary>
        /// MM Reloads container In Use pogress bar
        ///    Always add this after container added, removed or updated
        /// </summary>
        public void loadContainerProgress()
        {
            // MM count rows in the database
            int total = dbCon.totalContainers();
            int inUse = dbCon.containersInUse();
            
            // MM assign results to progrees bar
            containerInUseBar.Maximum = total;
            containerInUseBar.Minimum = 0;
            containerInUseBar.Value = inUse;
            contInUseValueLBL.Text = inUse.ToString();
            contTotalValueLBL.Text = total.ToString();
        }

        #endregion Container Progress Bar


        #endregion Container

        #region Vehicle

        #region Add Vehicle
        private void addVehicleBTN_Click(object sender, EventArgs e)
        {
            // MM check if textbox for reg number enabled or empty
            if (vehicleRegTextBox.Enabled == false | vehicleRegTextBox.Text == string.Empty)
            {
                vehicleRegTextBox.Enabled = true;
                vehicleRegTextBox.Text = string.Empty;
                MessageBox.Show("Please input registration number");
            }
            else
            {
                string reg = vehicleRegTextBox.Text;
                string type = vehicleTypeTextBox.Text;
                string availability;
                if (vehicleInUseRB.Checked == true)
                {
                    availability = "In Use";
                }
                else
                {
                    availability = "Available";
                }

                // MM create Vehicle instance with user parameters
                Vehicle vehicle = new Vehicle(reg, type, availability);
                // MM inserting new Vehicle Into database
                int rows = dbCon.insertRowVehicle(vehicle);
                if (rows == DBConnection.errNoRowInserted)
                    MessageBox.Show(DBConnection.errorInsertInTableStr);
                else
                {
                    // MM Reload whole vehicle tab
                    dbCon.loadVehicle(vehicleDGV);
                    loadVehicleProgress();
                    clearVehicle();
                }
            }

        }
        #endregion Add Vehicle

        #region Delete Vehicle
        private void removeVehicleBTN_Click(object sender, EventArgs e)
        {
            // MM check if row seected in DataGridView
            DataGridViewSelectedRowCollection selectedRows = vehicleDGV.SelectedRows;
            if (selectedRows.Count > 0)
            {
                string reg = vehicleDGV.SelectedRows[0].Cells["Reg Number"].Value.ToString();
                // MM deleting row fom vehicle table at selected index
                int rows = dbCon.removeVehicleDB(reg);

                // MM Reload whole Vehicle tab
                dbCon.loadVehicle(vehicleDGV);
                loadVehicleProgress();
                clearVehicle();
            }
            else
            {
                MessageBox.Show("Please select vehicle you want to delete");
            }
        }
        #endregion Delete Vehicle

        #region Clear Vehicle Tab

        /// <summary>
        /// Clear Vehicle textboxes and radio buttons
        /// Use after adding, removing or updating vehicles
        /// </summary>
        public void clearVehicle()
        {
            vehicleRegTextBox.Text = string.Empty;
            vehicleTypeTextBox.Text = string.Empty;
            vehicleInUseRB.Checked = false;
            vehicleAvailableRB.Checked = false;
            vehicleRegTextBox.Enabled = true;
            dbCon.loadVehicle(vehicleDGV);
        }

        private void clearVehicleBTN_Click(object sender, EventArgs e)
        {
            clearVehicle();
        }

        #endregion Clear Vehicle Tab

        #region Update Vehicle

        // MM bring values from selected object to textboxes and radio buttons
        private void vehicleDGV_MouseClick(object sender, MouseEventArgs e)
        {
            // MM assign selection valuses to textboxes
            try
            {
                vehicleRegTextBox.Text = vehicleDGV.SelectedRows[0].Cells[0].Value.ToString();
                vehicleTypeTextBox.Text = vehicleDGV.SelectedRows[0].Cells[1].Value.ToString();
            }
            catch
            {
                //Loading textBoxesFailed 
            }
            string availability = vehicleDGV.SelectedRows[0].Cells[2].Value.ToString();
            if (availability == "Available")
            {
                vehicleAvailableRB.Checked = true;
            }
            else
            {
                vehicleInUseRB.Checked = true;
            }

            // MM disable reg number text box to avoid changing and used to find vehicle in a database
            vehicleRegTextBox.Enabled = false;
        }

        private void vehicleUpdateBTN_Click(object sender, EventArgs e)
        {
            string reg = vehicleDGV.SelectedRows[0].Cells[0].Value.ToString();
            string type = vehicleTypeTextBox.Text;
            string availability;
            if (vehicleInUseRB.Checked)
            {
                availability = "In Use";
            }
            else
            {
                availability = "Available";
            }
            // MM creating new Vehicle instance for an update with user changed parameters
            Vehicle vehicle = new Vehicle(reg, type, availability);
            // MM Updating a database with new object
            dbCon.updateVehicle(vehicle);

            // MM reload whole Vehicle tab
            dbCon.loadVehicle(vehicleDGV);
            loadVehicleProgress();
            clearVehicle();
        }


        #endregion Update Vehicle

        #region Vehicle progress bar
        /// <summary>
        /// MM Reloads values to the Vehicle progress bar
        ///    Always use that after adding, removin or updating Vehicle
        /// </summary>
        public void loadVehicleProgress()
        {
            // MM count rows in vehicle table
            int totalV = dbCon.totalVehicles();
            int inUseV = dbCon.vehiclesInUse();

            // MM assign to the bar
            vehicleInUseBar.Maximum = totalV;
            vehicleInUseBar.Minimum = 0;
            vehicleInUseBar.Value = inUseV;
            vehInUseValueLBL.Text = inUseV.ToString();
            vehTotalValueLBL.Text = totalV.ToString();
        }

        private void vehicleInUseBar_Click(object sender, EventArgs e)
        {
            loadVehicleProgress();
        }

        #endregion Vehicle progress bar

        #endregion Vehicle

        #region Harvest

        #region Harvest mouse hoover and DataGridViews selections

        private void harvestCropDGV_MouseEnter(object sender, EventArgs e)
        {
            harvestCropTextBox.Font = new Font(harvestCropTextBox.Font, FontStyle.Bold);
        }

        private void harvestCropDGV_MouseLeave(object sender, EventArgs e)
        {
            harvestCropTextBox.Font = new Font(harvestCropTextBox.Font, FontStyle.Regular);
        }

        private void harvestEmployeeDGV_MouseEnter(object sender, EventArgs e)
        {
            harvestEmployeeTextBox.Font = new Font(harvestEmployeeTextBox.Font, FontStyle.Bold);
        }

        private void harvestEmployeeDGV_MouseLeave(object sender, EventArgs e)
        {
            harvestEmployeeTextBox.Font = new Font(harvestEmployeeTextBox.Font, FontStyle.Regular);
        }

        private void harvestVehicleDGV_MouseEnter(object sender, EventArgs e)
        {
            harvestVehicleTextBox.Font = new Font(harvestVehicleTextBox.Font, FontStyle.Bold);
        }

        private void harvestVehicleDGV_MouseLeave(object sender, EventArgs e)
        {
            harvestVehicleTextBox.Font = new Font(harvestVehicleTextBox.Font, FontStyle.Regular);
        }

        private void harvestContainerDGV_MouseEnter(object sender, EventArgs e)
        {
            harvestContainerTextBox.Font = new Font(harvestContainerTextBox.Font, FontStyle.Bold);
        }

        private void harvestContainerDGV_MouseLeave(object sender, EventArgs e)
        {
            harvestContainerTextBox.Font = new Font(harvestContainerTextBox.Font, FontStyle.Regular);
        }

        private void harvestVehicleDGV_MouseClick(object sender, MouseEventArgs e)
        {
            harvestVehicleTextBox.Text = harvestVehicleDGV.SelectedRows[0].Cells[0].Value.ToString();
        }

        private void harvestCropDGV_MouseClick(object sender, MouseEventArgs e)
        {
            harvestCropTextBox.Text = harvestCropDGV.SelectedRows[0].Cells[0].Value.ToString();
        }

        private void harvestEmployeeDGV_MouseClick(object sender, MouseEventArgs e)
        {
            harvestEmployeeTextBox.Text = harvestEmployeeDGV.SelectedRows[0].Cells[0].Value.ToString();
        }

        private void harvestContainerDGV_MouseClick(object sender, MouseEventArgs e)
        {
            harvestContainerTextBox.Text = harvestContainerDGV.SelectedRows[0].Cells[0].Value.ToString();
        }

        #endregion Harvest mouse hoover and DataGridViews selections

        #region Add Harvest

        private void addHarvestBTN_Click(object sender, EventArgs e)
        {
            // MM checking if items selected in four DataGridViews,
            DataGridViewSelectedRowCollection selectedHarvestCrop = harvestCropDGV.SelectedRows;
            DataGridViewSelectedRowCollection selectedHarvestEmployee = harvestEmployeeDGV.SelectedRows;
            DataGridViewSelectedRowCollection selectedHarvestVehicle = harvestVehicleDGV.SelectedRows;
            DataGridViewSelectedRowCollection selectedHarvestContainer = harvestContainerDGV.SelectedRows;
            if (selectedHarvestCrop.Count > 0 && selectedHarvestEmployee.Count > 0 && selectedHarvestVehicle.Count > 0 && selectedHarvestContainer.Count > 0)
            {
                // MM assigning values from four DataGridViews to the vaiables for Harvest instance 
                string hCrop = harvestCropDGV.SelectedRows[0].Cells["Crop Name"].Value.ToString();
                int hEmployeeID = Convert.ToInt16(harvestEmployeeDGV.SelectedRows[0].Cells["ID"].Value.ToString());
                string hVehicle = harvestVehicleDGV.SelectedRows[0].Cells["Reg Number"].Value.ToString();
                int hContainer = Convert.ToInt16(harvestContainerDGV.SelectedRows[0].Cells["ID"].Value.ToString());

                // MM date picking --- No validation
                string hStart = harvestStartPicker.Value.ToString("yyyy-MM-dd");
                string hEnd = harvestEndPicker.Value.ToString("yyyy-MM-dd");

                // MM check if harvesting Method choosed
                if (harvestMethodComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please select harvesting method");
                }
                else
                {
                    string hMethod = harvestMethodComboBox.Text;

                    // MM create instance of Harvest with selected paraeters
                    Harvest harvest = new Harvest(hStart, hEnd, hMethod, hCrop, hVehicle, hEmployeeID, hContainer);

                    // MM add new harvest t othe database
                    int rows = dbCon.insertRowHarvest(harvest);
                    if (rows == DBConnection.errNoRowInserted)
                        MessageBox.Show(DBConnection.errorInsertInTableStr);
                    else
                    {
                        // MM update vehicle and container availability and refresh DataGridviews
                        string availability = "In Use";
                        string reg = harvestVehicleDGV.SelectedRows[0].Cells["Reg Number"].Value.ToString();
                        int id = int.Parse(harvestContainerDGV.SelectedRows[0].Cells["ID"].Value.ToString());
                        dbCon.updateVehicleAvailability(reg, availability);
                        dbCon.updateContainerAvailability(id, availability);
                        dbCon.loadHarvest(harvestTimetableDGV, harvestEmployeeDGV, harvestContainerDGV, harvestCropDGV, harvestVehicleDGV);
                        dbCon.loadContainer(containerDGV);
                        dbCon.loadVehicle(vehicleDGV);
                        loadContainerProgress();
                        loadVehicleProgress();
                        clearHarvest();

                    }
                }
            }
            else
            {
                MessageBox.Show("Please select the Crop, Employee, Vehicle and Container from the list");
            }
        }

        #endregion Add Harvest

        #region Delete Harvest
        private void deleteHarvestBTN_Click(object sender, EventArgs e)
        {
            // MM check if row selected in DataGridView
            DataGridViewSelectedRowCollection selectedHarvest = harvestTimetableDGV.SelectedRows;
            if (selectedHarvest.Count > 0)
            {
                int id = int.Parse(harvestTimetableDGV.SelectedRows[0].Cells["ID"].Value.ToString());
                // MM Delete harvest at selected index
                int rows = dbCon.removeHarvestDB(id);
                dbCon.loadHarvest(harvestTimetableDGV, harvestEmployeeDGV, harvestContainerDGV, harvestCropDGV, harvestVehicleDGV);
            }
            else
            {
                MessageBox.Show("Please select the harvest you want to delete");
            }
        }
        #endregion Delete Harvest

        #region Clear Harvest

        /// <summary>
        /// Clear Harvest textboxes and comboBox
        /// Use this after Harvest is added 
        /// </summary>
        public void clearHarvest()
        {
            harvestCropTextBox.Text = string.Empty;
            harvestEmployeeTextBox.Text = string.Empty;
            harvestVehicleTextBox.Text = string.Empty;
            harvestContainerTextBox.Text = string.Empty;
            harvestMethodComboBox.Text = string.Empty;
        }

        private void harvestClearFormBTN_Click(object sender, EventArgs e)
        {
            clearHarvest();
        }
        #endregion Clear Harvest

        #endregion Harvest

        // MM <<

        #region Menu Strip
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
           Close();
        }
        #endregion Menu Strip

        // RK >>

        #region Crop

        #region DGV Mouse Click

        private void CropDGV_MClick(object sender, MouseEventArgs e)
        {
            // After clicking on selected Crop in DGV view, selected Crop is displayed in assigned Text Boxes
            {
                try
                {
                    cropNameTextBox.Text = cropDGV.SelectedRows[0].Cells[0].Value.ToString();
                    cropTypeTextBox.Text = cropDGV.SelectedRows[0].Cells[1].Value.ToString();
                    cropAmountTextBox.Text = cropDGV.SelectedRows[0].Cells[2].Value.ToString();
                    cropInCultivationTextBox.Text = cropDGV.SelectedRows[0].Cells[3].Value.ToString();
                    cropStorageTmintextBox.Text = cropDGV.SelectedRows[0].Cells[4].Value.ToString();
                    cropStorageTmaxtextBox.Text = cropDGV.SelectedRows[0].Cells[5].Value.ToString();
                    cropStorageTypetextBox.Text = cropDGV.SelectedRows[0].Cells[6].Value.ToString();
                    cropNameTextBox.Select();
                }
                 // If no records are in database, message is shown
                catch
                {
                    MessageBox.Show("No Records in Database", "Database Empty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
        }
        
        #endregion DGV Mouse Click

        #region Display All Crop

        // Display Button for Crops, display all Crops
        private void cropDisplayAllbtn_Click(object sender, EventArgs e)
        {
            dbCon.DisplayAllCrop(cropDGV);
        }
        #endregion Display All Crop

        #region Add Crop


        // Add Crop Data

        private void addCropDataBTN(object sender, EventArgs e)
        {
            try
            {
                // Convert User Input
                float amount = float.Parse(cropAmountTextBox.Text);
                float tempMin = float.Parse(cropStorageTmintextBox.Text);
                float tempMax = float.Parse(cropStorageTmaxtextBox.Text);

                //create new crop object
               // Crop crop = new Crop(cropNameTextBox.Text, cropTypeTextBox.Text, amount, cropInCultivationTextBox.Text, tempMin, tempMax, cropStorageTypetextBox.Text);
                //update database with new object
                Crop crop = new Crop(cropNameTextBox.Text, cropTypeTextBox.Text, amount, cropInCultivationTextBox.Text, tempMin, tempMax, cropStorageTypetextBox.Text);
                dbCon.addRowIntoCrop(crop);
                //rreload data grid view
                dbCon.DisplayAllCrop(cropDGV);
                // Clear Text Boxex
                ClearCropTextBox();
                            }
            catch
            {
                MessageBox.Show("No data Entered");
            }
            
           
        }
        #endregion Add Crop

        #region Clear Crop Box

        // Clear all assigned tex boxes
        private void ClearCropListClick(object sender, EventArgs e)
        {
            ClearCropTextBox();
        }
        // Method to clear Crop Tex Boxes
        public void ClearCropTextBox()
        {
            cropNameTextBox.Text = "";
            cropTypeTextBox.Text = "";
            cropAmountTextBox.Text = "";
            cropInCultivationTextBox.Text = "";
            cropStorageTmintextBox.Text = "";
            cropStorageTmaxtextBox.Text = "";
            cropStorageTypetextBox.Text = "";
        }
        #endregion Clear Crop Tex Box

        #region Update Crop
        // method to update crop
        private void updateCropBTN(object sender, EventArgs e)
        {
            try
            {
                // convert user input
                int amount = int.Parse(cropAmountTextBox.Text);
                int tempMin = int.Parse(cropStorageTmintextBox.Text);
                int tempMax = int.Parse(cropStorageTmaxtextBox.Text);

                //create new crop object
                Crop crop = new Crop(cropNameTextBox.Text, cropTypeTextBox.Text, amount, cropInCultivationTextBox.Text, tempMin, tempMax, cropStorageTypetextBox.Text);
                // update database 
                dbCon.updateCrop(crop);
                // reload data grid view
                dbCon.DisplayAllCrop(cropDGV);
                // Clear Text Boxex
                ClearCropTextBox();
            }
            catch 
            {
                MessageBox.Show("No Data Entered");
            }
        }
        #endregion Update Crop
       
        #region Delete Crop
        // Delete selected Crops
        private void deleteCropBTN(object sender, EventArgs e)
        {

            dbCon.deleteCrop(cropDGV);
            dbCon.DisplayAllCrop(cropDGV);
            ClearCropTextBox();
        }

        #endregion Delete Crop


        #endregion Crop

        // RK <<

        // TT >>

        #region Treatment

        //TT show all treatments button
        private void TreatmentShowAll_BTN_Click(object sender, EventArgs e)
        {
            dbCon.showAllTreatments(treatmentDGV);
        }

        //TT add new treatment button
        private void addNewTratmentBTN_Click(object sender, EventArgs e)
        {
            string treatmentStart = TreatmentStartDateTextBox.Text; //yyyy-MM-dd
            string treatmentEnd = TreatmentEndDateTextBox.Text;
            string treatmentCropName = TreatmentCropNameTextBox.Text;
            int treatmentEmployeeID = Convert.ToInt16(employeeTreatmentTextBox.Text);
            string  treatmentFertiliserType = FertiliserToUseTextBox.Text;

            Treatment treatment = new Treatment(treatmentStart, treatmentEnd, treatmentCropName, treatmentEmployeeID, treatmentFertiliserType);
            int rows = dbCon.addNewTreatment(treatment);
            if (rows == DBConnection.errorNoRowInserted)
                MessageBox.Show(DBConnection.errorInsertTreatment);
            else
            {
                dbCon.showAllTreatments(treatmentDGV);
            }
        }

        private void DeleteTratmentBTN_Click(object sender, EventArgs e)
        {
            // TT check for selection
            DataGridViewSelectedRowCollection selectedRows = treatmentDGV.SelectedRows;
            if (selectedRows.Count > 0)
            {
                // TT removing row from the database
                int ID = Convert.ToInt16(treatmentDGV.SelectedRows[0].Cells["ID"].Value.ToString());
                int rows = dbCon.deleteTreatment(ID);
                dbCon.showAllTreatments(treatmentDGV);
            }
            else
            {
                MessageBox.Show("No treatment selected!");
            }
        }

        // TT selection to textboxes
        private void treatmentDGV_MouseClick(object sender, MouseEventArgs e)
        {
            TreatmentIDTextBox.Text = treatmentDGV.SelectedRows[0].Cells[0].Value.ToString();
            TreatmentStartDateTextBox.Text = treatmentDGV.SelectedRows[0].Cells[1].Value.ToString();
            TreatmentEndDateTextBox.Text = treatmentDGV.SelectedRows[0].Cells[2].Value.ToString();
            TreatmentCropNameTextBox.Text = treatmentDGV.SelectedRows[0].Cells[3].Value.ToString();
            employeeTreatmentTextBox.Text = treatmentDGV.SelectedRows[0].Cells[4].Value.ToString();
            FertiliserToUseTextBox.Text = treatmentDGV.SelectedRows[0].Cells[5].Value.ToString();
        }

        // TT update treatment button
        private void UpdateTratmentBTN_Click(object sender, EventArgs e)
        {
            //id for update
            int ID = Convert.ToInt16(treatmentDGV.SelectedRows[0].Cells[0].Value.ToString());
            
            // creating object treatment
            string tStart = TreatmentStartDateTextBox.Text;
            string tEnd = TreatmentEndDateTextBox.Text; 
            string tCrop = TreatmentCropNameTextBox.Text;
            int tEmployee = Convert.ToInt16(employeeTreatmentTextBox.Text);
            string tFertiliser = FertiliserToUseTextBox.Text;

            Treatment treatment = new Treatment(tStart, tEnd, tCrop, tEmployee, tFertiliser);

           //testing  MessageBox.Show(ID.ToString() + tStart + tEnd + tCrop + tEmployee.ToString() + tFertiliser);

            //updating database
            dbCon.updateTreatment(ID, treatment);

            dbCon.showAllTreatments(treatmentDGV);
        }

        #endregion Treatment






    }
}
