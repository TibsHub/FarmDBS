using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FarmDBS
{
    class DBConnection
    {
        #region Settings
        // attributes

        // RB Connection String
        private string connectionString;

        // RB declaring SQL object to store the connection to the database
        public SqlConnection connectionToDB;

        // RB declaring database adapter object to open table of the database
        private SqlDataAdapter dataAdapter;

        // RB create dataSet object
        DataSet dataSet = new DataSet();

        // RB Employee and Customer Data Set
        DataSet EmployeeDataSet = new DataSet();
        DataSet CustomerDataSet = new DataSet();

        // RB Data Set for Reports
        DataSet employeeReportsDataSetContainer = new DataSet();
        DataSet employeeReportsDataSetTreatment = new DataSet();
        DataSet employeeReportsDataSetFertiliserStock = new DataSet();
        DataSet employeeReportsDataSetCropStock = new DataSet();
        DataSet managerReportsDataSetCrop = new DataSet();
        DataSet managerReportsDataSetHarvest = new DataSet();
        DataSet managerReportsDataSetHarvestLabour = new DataSet();
        DataSet managerReportsDataSetHarvestVehicles = new DataSet();

        // RK Data Set
        DataSet CropDataSet = new DataSet();

        // TT Data Set
        DataSet treatmentDataSet = new DataSet();

        // MM Data Set
        DataSet containerDataSet = new DataSet();
        DataSet vehicleDataSet = new DataSet();
        DataSet harvestDataSet = new DataSet();
        DataSet employeeHarvestDataSet = new DataSet();
        DataSet containerHarvestDataSet = new DataSet();
        DataSet vehicleHarvestDataSet = new DataSet();
        DataSet cropHarvestDataSet = new DataSet();

        // RB properties
        public string ConnectionString { get; set; }

        // RB Constructor
        public DBConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // RB Dataset Method
        public DataSet getDataSet(string sqlStatement, DataSet dataSet)
        {
            dataAdapter = new SqlDataAdapter(sqlStatement, connectionToDB);
            dataAdapter.Fill(dataSet);
            return dataSet;
        }

        // RB open the connection to the database
        public void openConnection()
        {
            // RB create the connection to the databse
            connectionToDB = new SqlConnection(connectionString);

            //RB open the connection
            connectionToDB.Open();
        }

        // RB close the connection to the databse
        public void closeConnection()
        {
            connectionToDB.Close();
        }
        #endregion Settings

        //>> RB

        #region Employee Methods

        #region Insert Data into Employee Table Method
        /// <summary>
        /// Inserting row into employee table
        /// </summary>
        /// <param name="employee"></param>
        public void InsertRowIntoEmployeeTable(Employee employee)
        {
            // RB create and initialise a sql command
            SqlCommand InsertEmployeCommand = new SqlCommand();
            InsertEmployeCommand.CommandType = CommandType.Text;

            // RB set the sql statement
            InsertEmployeCommand.CommandText = SQLQuery.insertRowIntoEmployee;

            InsertEmployeCommand.Connection = connectionToDB;

            // RB Set parameters for the SQL Statement    
            InsertEmployeCommand.Parameters.AddWithValue("@firstName", employee.FirstName);
            InsertEmployeCommand.Parameters.AddWithValue("@lastName", employee.LastName);
            InsertEmployeCommand.Parameters.AddWithValue("@address", employee.Address);
            InsertEmployeCommand.Parameters.AddWithValue("@privilegeLevel", employee.PrivilegeLevel);
            InsertEmployeCommand.Parameters.AddWithValue("@jobRole", employee.JobRole);
            InsertEmployeCommand.Parameters.AddWithValue("@hourlyRate", employee.HourlyRate);

            // RB open the connection to the DB   
            try
            {
                // RB open connection to the DB
                connectionToDB.Open();

                // RB execute SQL query to insert new Employee
                InsertEmployeCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                // RB close connection to the DB
                connectionToDB.Close();
            }
        }
        #endregion Insert Data into Employee Table Method

        #region Delete Employee record Method
        /// <summary>
        /// Deletes Employee Record
        /// </summary>
        /// <param name="defaultDGV"></param>
        public void DeleteEmployeeRow(DataGridView defaultDGV)
        {
            try
            {
                // RB Try to convert sellected value into integer
                int ID = int.Parse(defaultDGV.SelectedRows[0].Cells["ID"].Value.ToString());

                // RB initialise SQL Command
                SqlCommand deleteEmployeeCommand = new SqlCommand();
                deleteEmployeeCommand.CommandType = System.Data.CommandType.Text;

                // RB Assign SQL Query
                deleteEmployeeCommand.CommandText = SQLQuery.deleteEmployeeRow;
                deleteEmployeeCommand.Connection = connectionToDB;

                // RB set the parameters of the sql statement
                deleteEmployeeCommand.Parameters.AddWithValue("@personID", ID);

                // RB Open the connection to the DB and Execute SQL Query
                try
                {
                    // RB open connection to DB
                    connectionToDB.Open();

                    // RB execute SQL Query
                    deleteEmployeeCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    // RB throw messege error if fails above try statement
                    e.ToString();
                }
                finally
                {
                    // RB Close connection to DB
                    connectionToDB.Close();
                }

                // RB Message if REcord has been deleted from DB
                MessageBox.Show(defaultDGV.SelectedRows[0].Cells["Name"].Value.ToString() + " "
                                + defaultDGV.SelectedRows[0].Cells["Last Name"].Value.ToString() +
                                " has been deleted", "Record Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                // RB Warning Message if the raw was not selected
                MessageBox.Show("You did not select the row", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion Delete Employee record Method

        #region Display All Employees Method
        /// <summary>
        /// Display All Employees
        /// </summary>
        /// <param name="defaultDGV"></param>
        public void DisplayAllEmployees(DataGridView defaultDGV)
        {
            // RB Clear Employee Data Gid View - Check if not empty
            EmployeeDataSet.Clear();
            defaultDGV.DataSource = null;
            defaultDGV.Rows.Clear(); ;

            // RB open the connection to DB
            openConnection();

            // RB assign SQL DB query to the dataset
            EmployeeDataSet = getDataSet(SQLQuery.selectAllEmployees, EmployeeDataSet);
            DataTable table = EmployeeDataSet.Tables[0];

            // RB display data in grid view
            defaultDGV.DataSource = table;

            // RB close the connection to DB
            closeConnection();
        }
        #endregion Display All Employees Method

        #region Update Employee Record Method
        /// <summary>
        /// Update Employee Record
        /// </summary>
        /// <param name="def"></param>
        /// <param name="employee"></param>
        public void UpdateRowIntoEmployeeTable(DataGridView def, Employee employee)
        {
            // RB create and initialise a sql command
            SqlCommand UpdateCommand = new SqlCommand();
            UpdateCommand.CommandType = CommandType.Text;

            // RB set the sql statement
            UpdateCommand.CommandText = SQLQuery.updateEmployee;
            UpdateCommand.Connection = connectionToDB;

            int ID = int.Parse(def.SelectedRows[0].Cells["ID"].Value.ToString());

            // RB Set parameters for the SQL Statement    
            UpdateCommand.Parameters.AddWithValue("@firstName", employee.FirstName);
            UpdateCommand.Parameters.AddWithValue("@lastName", employee.LastName);
            UpdateCommand.Parameters.AddWithValue("@address", employee.Address);
            UpdateCommand.Parameters.AddWithValue("@privilegeLevel", employee.PrivilegeLevel);
            UpdateCommand.Parameters.AddWithValue("@jobRole", employee.JobRole);
            UpdateCommand.Parameters.AddWithValue("@hourlyRate", employee.HourlyRate);
            UpdateCommand.Parameters.AddWithValue("@personID", ID);

            try
            {
                // RB open connection to the DB
                connectionToDB.Open();

                // RB execute SQL query to insert new Employee
                UpdateCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                // RB Throw Exception Error if fails to execute query
                e.ToString();
            }
            finally
            {
                // RB close connection to the DB
                connectionToDB.Close();
            }
        }
        #endregion Update Employee Record Method

        #endregion Employee Methods

        #region Customer Methods

        #region Insert Data into Customer Table Method
        /// <summary>
        /// Inserting row into Customer table
        /// </summary>
        /// <param name="employee"></param>
        public void InsertRowIntoCustomerTable(Customer customer)
        {
            // RB create and initialise a sql command
            SqlCommand InsertCustomerCommand = new SqlCommand();
            InsertCustomerCommand.CommandType = CommandType.Text;

            // RB set the sql statement
            InsertCustomerCommand.CommandText = SQLQuery.insertRowIntoCustomer;
            InsertCustomerCommand.Connection = connectionToDB;

            // Set parameters for the SQL Statement    
            InsertCustomerCommand.Parameters.AddWithValue("@firstName", customer.FirstName);
            InsertCustomerCommand.Parameters.AddWithValue("@lastName", customer.LastName);
            InsertCustomerCommand.Parameters.AddWithValue("@address", customer.Address);

            // open the connection to the DB   
            try
            {
                // open connection to the DB
                connectionToDB.Open();

                // execute SQL query to insert new Customer
                InsertCustomerCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                // close connection to the DB
                connectionToDB.Close();
            }
        }
        #endregion Insert Data into Employee Table Method

        #region Display All Customers Method
        /// <summary>
        /// Displays All Customers in the Database - Method
        /// </summary>
        /// <param name="defaultDGV"></param>
        public void DisplayAllCustomers(DataGridView defaultDGV)
        {
            // RB Clear Employee Data Gid View - Check if not empty
            CustomerDataSet.Clear();
            defaultDGV.DataSource = null;
            defaultDGV.Rows.Clear(); ;

            // RB open the connection to DB
            openConnection();

            // RB assign SQL DB query to the dataset
            CustomerDataSet = getDataSet(SQLQuery.selectAllCustomers, CustomerDataSet);
            DataTable table = CustomerDataSet.Tables[0];

            //display data in grid view
            defaultDGV.DataSource = table;

            //close the connection to DB
            closeConnection();
        }
        #endregion Display All Customers Method

        #region Delete Customer Method
        /// <summary>
        /// Deletes Customer Record
        /// </summary>
        /// <param name="defaultDGV"></param>
        public void DeleteCustomer(DataGridView defaultDGV)
        {
            try
            {
                int ID = int.Parse(defaultDGV.SelectedRows[0].Cells["ID"].Value.ToString());

                // RB initialise SQL command
                SqlCommand deleteCustomerCommand = new SqlCommand();
                deleteCustomerCommand.CommandType = System.Data.CommandType.Text;

                // RB assign SQL Query to sql command
                deleteCustomerCommand.CommandText = SQLQuery.deleteCustomer;
                deleteCustomerCommand.Connection = connectionToDB;

                // RB set the parameters of the sql statement
                deleteCustomerCommand.Parameters.AddWithValue("@customerID", ID);

                // RB try to open the connection to the DB and Execute SQL Query
                try
                {
                    // RB open connection to DB
                    connectionToDB.Open();

                    // RB execute SQL Query
                    deleteCustomerCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    // RB Throw exception error message if try block fails
                    e.ToString();
                }
                finally
                {
                    // RB close connection to DB
                    connectionToDB.Close();
                }

                // RB Display message to the user that record hes been deleted
                MessageBox.Show(defaultDGV.SelectedRows[0].Cells["Name"].Value.ToString() + " "
                                + defaultDGV.SelectedRows[0].Cells["Last Name"].Value.ToString() +
                                " has been deleted", "Record Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                // RB display warning message that row has not been selected
                MessageBox.Show("You did not select the row", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        #endregion Delete Customer Method

        #region Update Customer Record Method
        /// <summary>
        /// Update Customer Record
        /// </summary>
        /// <param name="def"></param>
        /// <param name="customer"></param>
        public void UpdateRowIntoCustomerTable(DataGridView def, Customer customer)
        {
            // RB create and initialise a sql command
            SqlCommand UpdateCommand = new SqlCommand();
            UpdateCommand.CommandType = CommandType.Text;

            // RB set the sql statement
            UpdateCommand.CommandText = SQLQuery.updateCustomer;
            UpdateCommand.Connection = connectionToDB;

            int ID = int.Parse(def.SelectedRows[0].Cells["ID"].Value.ToString());

            // RB Set parameters for the SQL Statement    
            UpdateCommand.Parameters.AddWithValue("@firstName", customer.FirstName);
            UpdateCommand.Parameters.AddWithValue("@lastName", customer.LastName);
            UpdateCommand.Parameters.AddWithValue("@address", customer.Address);
            UpdateCommand.Parameters.AddWithValue("@customerID", ID);

            // RB try to open the connection to the DB   
            try
            {
                // RB open connection to the DB
                connectionToDB.Open();

                // RB execute SQL query to insert new Employee
                UpdateCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                // RB throw exception if try block fails
                e.ToString();
            }
            finally
            {
                // RB close connection to the DB
                connectionToDB.Close();
            }
        }
        #endregion Update Customer Record Method

        #endregion Custormer Methods

        #region Clear data set
        /// <summary>
        /// Method that clears data set and grid view. takes 2 parameters
        /// </summary>
        /// <param name="DGV"></param>
        /// <param name="dataSet"></param>
        public void ClearDataSet(DataGridView DGV, DataSet dataSet)
        {
            dataSet.Clear();
            DGV.DataSource = null;
            DGV.Rows.Clear();
        }
        #endregion Clear data set

        #region Employee Reports
        #region Employee container availability report method
        /// <summary>
        /// Method that calls Employee Container Availability Report
        /// </summary>
        /// <param name="employeeReportsDGV"></param>
        public void employeeContainerReport(DataGridView employeeReportsDGV)
        {
            // RB Clear Data Set
            ClearDataSet(employeeReportsDGV, employeeReportsDataSetContainer);

            //RB open the connection to DB
            openConnection();

            //RB assign SQL DB query to the dataset
            employeeReportsDataSetContainer = getDataSet(SQLQuery.employeeContainersAvailability, employeeReportsDataSetContainer);
            DataTable table = employeeReportsDataSetContainer.Tables[0];

            //RB display data in grid view
            employeeReportsDGV.DataSource = table;

            // RB close the connection to DB
            closeConnection();
        }
        #endregion Employee container availability report method

        #region Employee Treatment Report
        /// <summary>
        /// Method that calls Employee Treatment Report
        /// </summary>
        /// <param name="employeeReportsDGV"></param>
        public void employeeTreatmentReport(DataGridView employeeReportsDGV)
        {
            // RB Clear Data Set
            ClearDataSet(employeeReportsDGV, employeeReportsDataSetTreatment);

            // RB open the connection to DB
            openConnection();

            // RB assign SQL DB query to the dataset
            employeeReportsDataSetTreatment = getDataSet(SQLQuery.employeeTreatmentReport, employeeReportsDataSetTreatment);
            DataTable table = employeeReportsDataSetTreatment.Tables[0];

            // RB display data in grid view
            employeeReportsDGV.DataSource = table;

            // RB close the connection to DB
            closeConnection();
        }
        #endregion Employee Treatment Report

        #endregion Employee Reports

        #region Manager Reports

        #region Manager Report Method - Crops in cultivation
        /// <summary>
        /// Method that calls Manager report - Crops in cultivation
        /// </summary>
        /// <param name="managerReportsDGV"></param>
        public void managerRCropInCultivation(DataGridView managerReportsDGV)
        {
            // RB clear datagrid view
            ClearDataSet(managerReportsDGV, managerReportsDataSetCrop);

            // RB open the connection to DB
            openConnection();

            managerReportsDataSetCrop = getDataSet(SQLQuery.mangerCropsInCultivation, managerReportsDataSetCrop);
            DataTable table = managerReportsDataSetCrop.Tables[0];

            // RB display data in grid view
            managerReportsDGV.DataSource = table;

            // RB close the connection to DB
            closeConnection();
        }
        #endregion Manager Report Method - Crops in cultivation

        #region Manager Report Method - Harvest Timetable
        /// <summary>
        /// Method that calls Manager report - Harvest Timetable
        /// </summary>
        /// <param name="managerReportsDGV"></param>
        public void managerRHarvestTimeTable(DataGridView managerReportsDGV)
        {
            // RB clear datagrid view
            ClearDataSet(managerReportsDGV, managerReportsDataSetHarvest);

            // RB open the connection to DB
            openConnection();

            // RB assign SQL DB query to the dataset
            managerReportsDataSetHarvest = getDataSet(SQLQuery.mgrSelectAllHarvests, managerReportsDataSetHarvest);
            DataTable table = managerReportsDataSetHarvest.Tables[0];

            //RB display data in grid view
            managerReportsDGV.DataSource = table;

            //RB close the connection to DB
            closeConnection();
        }
        #endregion Manager Report Method - Harvest Timetable

        #region Method that calls Manager report - Labour for Harvest
        /// <summary>
        /// Method that calls Manager report - Labour for Harvest
        /// </summary>
        /// <param name="managerReportsDGV"></param>
        public void managerRHarvestLabour(DataGridView managerReportsDGV)
        {
            // RB clear datagrid view
            ClearDataSet(managerReportsDGV, managerReportsDataSetHarvestLabour);

            // RB open the connection to DB
            openConnection();

            // RB assign SQL DB query to the dataset
            managerReportsDataSetHarvestLabour = getDataSet(SQLQuery.mgrHarvestLabour, managerReportsDataSetHarvestLabour);
            DataTable table = managerReportsDataSetHarvestLabour.Tables[0];

            // RB display data in grid view
            managerReportsDGV.DataSource = table;

            // RB close the connection to DB
            closeConnection();
        }
        #endregion Method that calls Manager report - Labour for Harvest

        #region Manager Report Method - Vehicles needed for Harvest
        /// <summary>
        /// Method that calls Manager report - Vehicles needed for Harvest
        /// </summary>
        /// <param name="managerReportsDGV"></param>
        public void managerRHarvestVehicles(DataGridView managerReportsDGV)
        {
            // RB clear datagrid view
            ClearDataSet(managerReportsDGV, managerReportsDataSetHarvestVehicles);

            // RB open the connection to DB
            openConnection();

            // RB assign SQL DB query to the dataset
            managerReportsDataSetHarvestVehicles = getDataSet(SQLQuery.mgrVehiclesforHarvest, managerReportsDataSetHarvestVehicles);
            DataTable table = managerReportsDataSetHarvestVehicles.Tables[0];

            // RB display data in grid view
            managerReportsDGV.DataSource = table;

            // RB close the connection to DB
            closeConnection();
        }
        #endregion Manager Report Method - Vehicles needed for Harvest

        #region Manager Report Method - Fertiliser Stock Level
        /// <summary>
        /// Method that calls Manager report - Fertiliser Stock Level
        /// </summary>
        /// <param name="managerReportsDGV"></param>
        public void managerRFertiliserStock(DataGridView managerReportsDGV)
        {
            // RB clear datagrid view
            ClearDataSet(managerReportsDGV, employeeReportsDataSetFertiliserStock);

            // RB open the connection to DB
            openConnection();

            // RB assign SQL DB query to the dataset
            employeeReportsDataSetFertiliserStock = getDataSet(SQLQuery.displayFertiliserStock, employeeReportsDataSetFertiliserStock);
            DataTable table = employeeReportsDataSetFertiliserStock.Tables[0];

            // RB display data in grid view
            managerReportsDGV.DataSource = table;

            // RB close the connection to DB
            closeConnection();
        }
        #endregion Manager Report Method- Fertiliser Stock Level

        #region Manager Report Method - Crop Stock Level
        /// <summary>
        /// Method that calls Manager Report - Crop Stock Level
        /// </summary>
        /// <param name="managerReportsDGV"></param>
        public void managerRCropStock(DataGridView managerReportsDGV)
        {
            // RB clear datagrid view
            ClearDataSet(managerReportsDGV, employeeReportsDataSetCropStock);

            // RB open the connection to DB
            openConnection();

            // RB assign SQL DB query to the dataset
            employeeReportsDataSetCropStock = getDataSet(SQLQuery.displayCropStock, employeeReportsDataSetCropStock);
            DataTable table = employeeReportsDataSetCropStock.Tables[0];

            // RB display data in grid view
            managerReportsDGV.DataSource = table;

            // RB close the connection to DB
            closeConnection();
        }
        #endregion Manager Report Method- Fertaliser Stock Level

        #endregion Manager Reports

        //<< RB

        //>> RK

        #region Crop

        #region Display All Crop
        /// <summary>
        /// Display All Crop
        /// </summary>
        /// <param name="defaultDGV"></param>
        public void DisplayAllCrop(DataGridView defaultDGV)
        {
            
            //Clear Crop Data Gid View - Check if not empty
            CropDataSet.Clear();
            defaultDGV.DataSource = null;
            defaultDGV.Rows.Clear(); ;

            //open the connection to DB
            openConnection();

            //assign SQL DB query to the dataset
            CropDataSet = getDataSet(SQLQuery.getCropData, CropDataSet);
            DataTable table = CropDataSet.Tables[0];

            //display data in grid view
            defaultDGV.DataSource = table;

            //close the connection to DB
            closeConnection();
        }
        #endregion Display All Crop
      
        #region Add Crop
        public void addRowIntoCrop(Crop crop)
        {
            // create and initialise a sql command
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            // set the sql statement that will be executed
            sqlCommand.CommandText = SQLQuery.insertCropData;
            sqlCommand.Connection = connectionToDB;

            // set the parameters of the sql statement
            sqlCommand.Parameters.AddWithValue("@cropName", crop.CropName);
            sqlCommand.Parameters.AddWithValue("@cropType", crop.CropType);
            sqlCommand.Parameters.AddWithValue("@cropAmount", crop.CropAmount);
            sqlCommand.Parameters.AddWithValue("@cropInCultivation", crop.CropInCultivation);
            sqlCommand.Parameters.AddWithValue("@cropStorageTempMin", crop.CropStorageTempMin);
            sqlCommand.Parameters.AddWithValue("@cropStorageTempMax", crop.CropStorageTempMax);
            sqlCommand.Parameters.AddWithValue("@cropStorageType", crop.CropStorageType);

            // open the connection to the DB   
            try
            {
                // open connection to the DB
                connectionToDB.Open();

                // execute SQL query to insert new Customer
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                // close connection to the DB
                connectionToDB.Close();
            }
        }
                   
        #endregion Add Crop

        #region Update Crop
        public void updateCrop(Crop crop)
        {
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;

            
            sqlCommand.CommandText = SQLQuery.updateCropData;
            sqlCommand.Connection = connectionToDB;

            
            sqlCommand.Parameters.AddWithValue("@cropName", crop.CropName);
            sqlCommand.Parameters.AddWithValue("@cropType", crop.CropType);
            sqlCommand.Parameters.AddWithValue("@cropAmount", crop.CropAmount);
            sqlCommand.Parameters.AddWithValue("@cropInCultivation", crop.CropInCultivation);
            sqlCommand.Parameters.AddWithValue("@cropStorageTempMin", crop.CropStorageTempMin);
            sqlCommand.Parameters.AddWithValue("@cropStorageTempMax", crop.CropStorageTempMax);
            sqlCommand.Parameters.AddWithValue("@cropStorageType", crop.CropStorageType);

          
            try
            {
                connectionToDB.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Crop details has not been changed");
            }
            finally
            {
                if (connectionToDB != null)
                    connectionToDB.Close();
            }

        }
        #endregion Update Crop
       
        #region Delete Crop
        //delete Crop Method
        public void deleteCrop(DataGridView defaultDGV)
        {
        
            string name = defaultDGV.SelectedRows[0].Cells["Name"].Value.ToString();
            // initialise SQL command
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;

            // assign SQL Query to sql command
            sqlCommand.CommandText = SQLQuery.deleteCropData;
            sqlCommand.Connection = connectionToDB;

            // set the parameters of the sql statement
            sqlCommand.Parameters.AddWithValue("@cropName", name);


            try
            {
                //  open connection to DB
                connectionToDB.Open();

                //execute SQL Query
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
         
                e.ToString();
            }
            finally
            {
                connectionToDB.Close();
            }

            MessageBox.Show("Crop has been deleted");
        }
        #endregion Delete Crop
        #endregion Crop

        //<< RK

        //>> MM

        #region Containers

        #region Insert row into Conatiner Method
        /// <summary>
        ///  MM inserts a row in the table Containers
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public int insertRowContainer(Container container)
        {
            // MM creating and initialising an sql commmand
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;

            // MM setting the statement that will be executed
            sqlCommand.CommandText = SQLQuery.insertContainer;
            sqlCommand.Connection = connectionToDB;

            // MM setting the parameters of the sql statement
            sqlCommand.Parameters.AddWithValue("@containerType", container.ContainerType);
            sqlCommand.Parameters.AddWithValue("@availability", container.ContainerAvailability);


            // MM open the connection and execute the command containing the sql statement
            try
            {
                connectionToDB.Open();
                int noOfRows = sqlCommand.ExecuteNonQuery();

                //return the no of rows inserted
                return noOfRows;
            }
            catch
            {
                //return an error code
                return errNoRowInserted;
            }
            finally
            {
                //close the connection
                if (connectionToDB != null)
                    connectionToDB.Close();
            }

        }

        // MM error messages
        public static string errorInsertInTableStr = "Data was not added into the table!";

        // MM error codes
        public static int errNoRowInserted = 0;
        #endregion Insert row into Conatiner Method


        /// <summary>
        ///  MM load Container Tab, Clear and load data to DataGridView
        /// </summary>
        /// <param name="DGV"></param>
        public void loadContainer(DataGridView DGV)
        {
            // MM Clear DataGridView in Container Tab
            if (containerDataSet != null) //check if not empty
            {
                containerDataSet.Clear();
                DGV.DataSource = null;
                DGV.Rows.Clear();
            }

            //MM open the connection to DB
            openConnection();

            // MM assign SQL DB query to the dataset
            containerDataSet = getDataSet(SQLQuery.selectAllContainers, containerDataSet);
            DataTable table = containerDataSet.Tables[0];

            // MM display DataGridView
            DGV.DataSource = table;

            // MM close the connection to DB
            closeConnection();
            DGV.ClearSelection();
        }

        /// <summary>
        ///  MM deletes container from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int removeContainerDB(int id)
        {
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;

            // MM setting the statement that will be executed
            sqlCommand.CommandText = SQLQuery.removeContainer;
            sqlCommand.Connection = connectionToDB;

            // MM set the parameters of the sql statement
            sqlCommand.Parameters.AddWithValue("@containerId", id);

            // MM open the connection and execute the command containing the sql statement
            try
            {
                connectionToDB.Open();
                int noOfRows = sqlCommand.ExecuteNonQuery();

                // MM return the number of rows deleted
                return noOfRows;
            }
            catch
            {
                // MM return an error code
                return errNoRowInserted;
            }
            finally
            {
                // MM close the connection
                if (connectionToDB != null)
                    connectionToDB.Close();
            }
        }

        /// <summary>
        ///  MM update container
        /// </summary>
        /// <param name="container"></param>
        public void updateContainer(Container container)
        {
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;

            // MM setting the statement that will be executed
            sqlCommand.CommandText = SQLQuery.updateContainer;
            sqlCommand.Connection = connectionToDB;

            // MM set the parameters of the sql statement
            sqlCommand.Parameters.AddWithValue("@containerId", container.ContainerId);
            sqlCommand.Parameters.AddWithValue("@containerType", container.ContainerType);
            sqlCommand.Parameters.AddWithValue("@availability", container.ContainerAvailability);

            // MM open the connection and execute the command containing the sql statement
            try
            {
                connectionToDB.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Container details not changed");
            }
            finally
            {
                // MM close the connection
                if (connectionToDB != null)
                    connectionToDB.Close();
            }

        }
        /// <summary>
        ///  MM Counting rows amount of containers
        /// </summary>
        /// <returns>int number of rows in container table</returns>
        public int totalContainers()
        {
            try
            {
                int count = 0;
                using (SqlCommand cmdCount = new SqlCommand(SQLQuery.getTotalContainers, connectionToDB))
                {
                    connectionToDB.Open();
                    count = (int)cmdCount.ExecuteScalar();
                }

                return count;
            }
            catch
            {
                return 0;
            }
            finally
            {
                if (connectionToDB != null)
                    connectionToDB.Close();
            }
        }
        /// <summary>
        ///  MM counting amount of containers in use
        /// </summary>
        /// <returns>int number of rows in container table where container is in use</returns>
        public int containersInUse()
        {
            try
            {
                int count = 0;

                using (SqlCommand cmdCount = new SqlCommand(SQLQuery.getContainersInUse, connectionToDB))
                {
                    connectionToDB.Open();
                    count = (int)cmdCount.ExecuteScalar();
                }

                return count;
            }
            catch
            {
                return 0;
            }

            finally
            {
                if (connectionToDB != null)
                    connectionToDB.Close();
            }
        }

        #endregion Containers

        #region Harvest

        /// <summary>
        ///  MM load Harvest Tab, Clear and load data to DataGidViews
        /// </summary>
        /// <param name="harvest"></param>
        /// <param name="employee"></param>
        /// <param name="container"></param>
        /// <param name="crop"></param>
        /// <param name="vehicle"></param>
        public void loadHarvest(DataGridView harvest, DataGridView employee, DataGridView container, DataGridView crop, DataGridView vehicle)
        {
            // MM Clear DataGridView in Harvest Tab - Harvest Timetable
            if (harvestDataSet != null) //check if not empty
            {
                harvestDataSet.Clear();
                harvest.DataSource = null;
                harvest.Rows.Clear();
            }

            // MM open the connection to DB
            openConnection();

            // MM assign SQL DB query to the dataset
            harvestDataSet = getDataSet(SQLQuery.selectAllHarvests, harvestDataSet);
            DataTable table = harvestDataSet.Tables[0];

            // MM display DataGridView view
            harvest.DataSource = table;

            // MM close the connection to DB
            closeConnection();

            // MM Clear DataGridView in Harvest Tab - container
            if (containerHarvestDataSet != null) //check if not empty
            {
                containerHarvestDataSet.Clear();
                container.DataSource = null;
                container.Rows.Clear();
            }

            // MM open the connection to DB
            openConnection();

            // MM assign SQL DB query to the dataset
            containerHarvestDataSet = getDataSet(SQLQuery.selectHarvestContainers, containerHarvestDataSet);
            DataTable tableContainer = containerHarvestDataSet.Tables[0];

            // MM display DataGridView view
            container.DataSource = tableContainer;

            // MM close the connection to DB
            closeConnection();

            // MM Clear DataGridView in Harvest Tab - employee
            if (employeeHarvestDataSet != null) //check if not empty
            {
                employeeHarvestDataSet.Clear();
                employee.DataSource = null;
                employee.Rows.Clear();
            }

            // MM open the connection to DB
            openConnection();

            // MM assign SQL DB query to the dataset
            employeeHarvestDataSet = getDataSet(SQLQuery.selectHarvestEmployees, employeeHarvestDataSet);
            DataTable tableEmployee = employeeHarvestDataSet.Tables[0];

            // MM display DataGridView
            employee.DataSource = tableEmployee;

            // MM close the connection to DB
            closeConnection();

            // MM Clear DataGridView in Harvest Tab - crop
            if (cropHarvestDataSet != null) //check if not empty
            {
                cropHarvestDataSet.Clear();
                crop.DataSource = null;
                crop.Rows.Clear();
            }

            // MM open the connection to DB
            openConnection();

            // MM assign SQL DB query to the dataset
            cropHarvestDataSet = getDataSet(SQLQuery.selectHarvestCrop, cropHarvestDataSet);
            DataTable tableCrop = cropHarvestDataSet.Tables[0];

            // MM display DataGridView
            crop.DataSource = tableCrop;

            // MM close the connection to DB
            closeConnection();

            // MM Clear DataGridView in Harvest Tab - vehicle
            if (vehicleHarvestDataSet != null) //check if not empty
            {
                vehicleHarvestDataSet.Clear();
                vehicle.DataSource = null;
                vehicle.Rows.Clear();
            }

            // MM open the connection to DB
            openConnection();

            // MM assign SQL DB query to the dataset
            vehicleHarvestDataSet = getDataSet(SQLQuery.selectHarvestVehicle, vehicleHarvestDataSet);
            DataTable tableVehicle = vehicleHarvestDataSet.Tables[0];

            // MM display DataGridView
            vehicle.DataSource = tableVehicle;

            // MM close the connection to DB
            closeConnection();
        }
        /// <summary>
        ///  MM insert a row in the table Harvest
        /// </summary>
        /// <param name="harvest"></param>
        /// <returns></returns>
        public int insertRowHarvest(Harvest harvest)
        {
            // MM creating and initialising an sql commmand
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;

            // MM setting the statement that will be executed
            sqlCommand.CommandText = SQLQuery.insertHarvest;
            sqlCommand.Connection = connectionToDB;

            // MM setting the parameters of the sql statement
            sqlCommand.Parameters.AddWithValue("@harvestStart", harvest.HarvestStartDate);
            sqlCommand.Parameters.AddWithValue("@harvestEnd", harvest.HarvestEndDate);
            sqlCommand.Parameters.AddWithValue("@harvestMethod", harvest.HarvestMethod);
            sqlCommand.Parameters.AddWithValue("@harvestCrop", harvest.HarvestCrop);
            sqlCommand.Parameters.AddWithValue("@harvestVehicle", harvest.HarvestVehicle);
            sqlCommand.Parameters.AddWithValue("@harvestEmployeeID", harvest.HarvestEmployeeID);
            sqlCommand.Parameters.AddWithValue("@harvestContainer", harvest.HarvestContainer);

            // MM open the connection and execute the command containing the sql statement
            try
            {
                connectionToDB.Open();
                int noOfRows = sqlCommand.ExecuteNonQuery();

                // MM return the no of rows inserted(is 1 if executed correctly)
                return noOfRows;
            }
            catch
            {
                // MM return an error code
                return errNoRowInserted;
            }
            finally
            {
                // MM close the connection
                if (connectionToDB != null)
                    connectionToDB.Close();
            }

        }

        /// <summary>
        ///  MM delete harvest from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int removeHarvestDB(int id)
        {
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;

            // MM setting the statement that will be executed
            sqlCommand.CommandText = SQLQuery.removeHarvest;
            sqlCommand.Connection = connectionToDB;

            // MM set the parameters of the sql statement
            sqlCommand.Parameters.AddWithValue("@harvestID", id);

            // MM open the connection and execute the command containing the sql statement
            try
            {
                connectionToDB.Open();
                int noOfRows = sqlCommand.ExecuteNonQuery();

                // MM return the number of rows deleted
                return noOfRows;
            }
            catch
            {
                // MM return an error code
                return errNoRowInserted;
            }
            finally
            {
                // MM close the connection
                if (connectionToDB != null)
                    connectionToDB.Close();
            }
        }

        /// <summary>
        ///  MM update vehicle availability
        ///  Changes status to In Use when whehicle added to harvest
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="avail"></param>
        public void updateVehicleAvailability(string reg, string avail)
        {
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;

            // MM setting the statement that will be executed
            sqlCommand.CommandText = SQLQuery.updateVehicleAvailability;
            sqlCommand.Connection = connectionToDB;

            // MM set the parameters of the sql statement
            sqlCommand.Parameters.AddWithValue("@regNumber", reg);
            sqlCommand.Parameters.AddWithValue("@availability", avail);


            // MM open the connection and execute the command containing the sql statement
            try
            {
                connectionToDB.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Vehicle availability not changed");
            }
            finally
            {
                // MM close the connection
                if (connectionToDB != null)
                    connectionToDB.Close();
            }

        }

        /// <summary>
        ///  MM update container availability
        ///  Changes status t In Use when container added to harvest
        /// </summary>
        /// <param name="id"></param>
        /// <param name="avail"></param>
        public void updateContainerAvailability(int id, string avail)
        {
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;

            // MM setting the statement that will be executed
            sqlCommand.CommandText = SQLQuery.updateContainerAvailability;
            sqlCommand.Connection = connectionToDB;

            // MM set the parameters of the sql statement
            sqlCommand.Parameters.AddWithValue("@containerId", id);
            sqlCommand.Parameters.AddWithValue("@availability", avail);

            // MM open the connection and execute the command containing the sql statement
            try
            {
                connectionToDB.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Container availability not changed");
            }
            finally
            {
                // MM close the connection
                if (connectionToDB != null)
                    connectionToDB.Close();
            }

        }


        #endregion Harvest

        #region Vehicle

        /// <summary>
        /// Load Vehicle, clear and load data to DataGridView
        /// </summary>
        /// <param name="DGV"></param>
        public void loadVehicle(DataGridView DGV)
        {            // MM Clear DataGridView in Vehicle Tab
            if (vehicleDataSet != null) // MM check if not empty
            {
                vehicleDataSet.Clear();
                DGV.DataSource = null;
                DGV.Rows.Clear();
            }

            // MM open the connection to DB
            openConnection();

            // MM assign SQL DB query to the dataset
            vehicleDataSet = getDataSet(SQLQuery.selectAllVehicles, vehicleDataSet);
            DataTable table = vehicleDataSet.Tables[0];

            // MM display DataGridView view
            DGV.DataSource = table;

            // MM close the connection to DB
            closeConnection();
        }

        /// <summary>
        /// Insert row in a Vehicle table
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public int insertRowVehicle(Vehicle vehicle)
        {
            // MM creating and initialising an sql commmand
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;

            // MM setting the statement that will be executed
            sqlCommand.CommandText = SQLQuery.insertVehicle;
            sqlCommand.Connection = connectionToDB;

            // MM setting the parameters of the sql statement
            sqlCommand.Parameters.AddWithValue("@regNumber", vehicle.RegNumber);
            sqlCommand.Parameters.AddWithValue("@vehicleType", vehicle.VehicleType);
            sqlCommand.Parameters.AddWithValue("@availability", vehicle.VehicleAvailability);


            // MM open the connection and execute the command containing the sql statement
            try
            {
                connectionToDB.Open();
                int noOfRows = sqlCommand.ExecuteNonQuery();

                // MM return the no of rows inserted
                return noOfRows;
            }
            catch
            {
                // MM return an error code
                return errNoRowInserted;
            }
            finally
            {
                // MM close the connection
                if (connectionToDB != null)
                    connectionToDB.Close();
            }

        }

        /// <summary>
        ///  MM delete vehicle from the database
        /// </summary>
        /// <param name="reg"></param>
        /// <returns></returns>
        public int removeVehicleDB(string reg)
        {
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;

            // MM setting the statement that will be executed
            sqlCommand.CommandText = SQLQuery.removeVehicle;
            sqlCommand.Connection = connectionToDB;

            // MM set the parameters of the sql statement
            sqlCommand.Parameters.AddWithValue("@regNumber", reg);

            // MM open the connection and execute the command containing the sql statement
            try
            {
                connectionToDB.Open();
                int noOfRows = sqlCommand.ExecuteNonQuery();

                // MM return the number of rows deleted
                return noOfRows;
            }
            catch
            {
                // MM return an error code
                return errNoRowInserted;
            }
            finally
            {
                // MM close the connection
                if (connectionToDB != null)
                    connectionToDB.Close();
            }
        }


        /// <summary>
        ///  MM update vehicle, update of type and availability in database
        /// </summary>
        /// <param name="vehicle"></param>
        public void updateVehicle(Vehicle vehicle)
        {
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;

            // MM setting the statement that will be executed
            sqlCommand.CommandText = SQLQuery.updateVehicle;
            sqlCommand.Connection = connectionToDB;

            // MM set the parameters of the sql statement
            sqlCommand.Parameters.AddWithValue("@regNumber", vehicle.RegNumber);
            sqlCommand.Parameters.AddWithValue("@vehicleType", vehicle.VehicleType);
            sqlCommand.Parameters.AddWithValue("@availability", vehicle.VehicleAvailability);


            // MM open the connection and execute the command containing the sql statement
            try
            {
                connectionToDB.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Vehicle details not changed");
            }
            finally
            {
                // MM close the connection
                if (connectionToDB != null)
                    connectionToDB.Close();
            }

        }

        /// <summary>
        ///  MM counting total of the vehicles
        /// </summary>
        /// <returns>int number of rows in Vehicle table</returns>
        public int totalVehicles()
        {
            try
            {
                int countV = 0;
                using (SqlCommand cmdCount = new SqlCommand(SQLQuery.getTotalVehicle, connectionToDB))
                {
                    connectionToDB.Open();
                    countV = (int)cmdCount.ExecuteScalar();
                }

                return countV;
            }
            catch
            {
                return 0;
            }
            finally
            {
                if (connectionToDB != null)
                    connectionToDB.Close();
            }
        }

        /// <summary>
        ///  MM counting vehicles in use
        /// </summary>
        /// <returns>int number of rows in Vehice table where In Use</returns>
        public int vehiclesInUse()
        {
            try
            {
                int countV = 0;

                using (SqlCommand cmdCount = new SqlCommand(SQLQuery.getVehicleInUse, connectionToDB))
                {
                    connectionToDB.Open();
                    countV = (int)cmdCount.ExecuteScalar();
                }

                return countV;
            }
            catch
            {
                return 0;
            }

            finally
            {
                if (connectionToDB != null)
                    connectionToDB.Close();
            }
        }

        #endregion Vehicle

        //<< MM

        //<< TK

        #region Sow
        //will contain adding removing updating Sow

        #endregion

        #region Fertalisers
        //will contain adding removing updating Fertalisers

        #endregion Fertalisers

        #region Treatment
        /// <summary>
        /// show all treatment method
        /// </summary>
        public void showAllTreatments(DataGridView dataGridView)
        {
            // TT Clear data grid view in treatment tab
            if (treatmentDataSet != null) 
            {
                treatmentDataSet.Clear();
                dataGridView.DataSource = null;
                dataGridView.Rows.Clear();
            }

            //TT open the connection to database
            openConnection();

            // TT assign SQL query to the dataset
            treatmentDataSet = getDataSet(SQLQuery.selectAllTreatments, treatmentDataSet);
            DataTable table = treatmentDataSet.Tables[0];

            // TT display data grid view
            dataGridView.DataSource = table;

            // TT close the connection to the data base
            closeConnection();
            dataGridView.ClearSelection();
        }

        public int addNewTreatment(Treatment treatment)
        {
            // TT creating and initialising an sql commmand
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;

            // TT setting the SQL statement
            sqlCommand.CommandText = SQLQuery.insertRowTreatment;
            sqlCommand.Connection = connectionToDB;

            // TT setting the parameters of the statement
            sqlCommand.Parameters.AddWithValue("@startDate", treatment.TreatmentStartDate);
            sqlCommand.Parameters.AddWithValue("@endDate", treatment.TreatmentEndDate);
            sqlCommand.Parameters.AddWithValue("@cropName", treatment.TreatmentCropName);
            sqlCommand.Parameters.AddWithValue("@employeeID", treatment.TreatmentEmployeeID);
            sqlCommand.Parameters.AddWithValue("@fertiliserType", treatment.TreatmentFertiliserType);

            // TT open the connection and execute the command
            try
            {
                connectionToDB.Open();
                int noOfRows = sqlCommand.ExecuteNonQuery();
                return noOfRows;
            }
            catch
            {
                //return an error code
                return errorNoRowInserted;
            }
            finally
            {
                //close the connection
                if (connectionToDB != null)
                    connectionToDB.Close();
            }

        }

        /// <summary>
        /// delete treatment method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int deleteTreatment(int ID)
        {
            // TT creating and initialising an sql commmand
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;

            // TT setting the SQL statement
            sqlCommand.CommandText = SQLQuery.deleteTreatment;
            sqlCommand.Connection = connectionToDB;

            // TT setting the parameters of the statement
            sqlCommand.Parameters.AddWithValue("@treatmentID", ID);

            // TT open the connection and execute the command
            try
            {
                connectionToDB.Open();
                int noOfRows = sqlCommand.ExecuteNonQuery();

                return noOfRows;
            }
            catch
            {
                //return an error code
                return errNoRowInserted;
            }
            finally
            {
                //close the connection
                if (connectionToDB != null)
                    connectionToDB.Close();
            }
        }

        /// <summary>
        /// TT Update treatment method
        /// </summary>
        /// <param name="treatment"></param>
        public void updateTreatment( int ID, Treatment treatment)
        {
            // TT creating and initialising an sql commmand
            System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;

            // TT setting the SQL statement
            sqlCommand.CommandText = SQLQuery.updateTreatment;
            sqlCommand.Connection = connectionToDB;

            // TT setting the parameters of the statement
            sqlCommand.Parameters.AddWithValue("@treatmentID", ID);
            sqlCommand.Parameters.AddWithValue("@startDate", treatment.TreatmentStartDate);
            sqlCommand.Parameters.AddWithValue("@endDate", treatment.TreatmentEndDate);
            sqlCommand.Parameters.AddWithValue("@cropName", treatment.TreatmentCropName);
            sqlCommand.Parameters.AddWithValue("@employeeID", treatment.TreatmentEmployeeID);
            sqlCommand.Parameters.AddWithValue("@fertiliserType", treatment.TreatmentFertiliserType);

            // TT open the connection and execute the command
            try
            {
                connectionToDB.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Update Fail!");
            }
            finally
            {
                // close the connection
                if (connectionToDB != null)
                    connectionToDB.Close();
            }

        }

        public static string errorInsertTreatment = "Data not added to the table!";
        public static int errorNoRowInserted = 0;

        #endregion

        //>> TK
    }
}
