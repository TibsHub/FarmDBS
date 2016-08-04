using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmDBS
{
    class SQLQuery
    {
        #region RB Queries

        #region Employee and Manager Reports Queries

        // RB Display all available cointainers for EMPLOYEE
        public static string employeeContainersAvailability = "SELECT DISTINCT containerId AS [Container ID], containerType AS [Container Type], availability AS [Available]  FROM Container WHERE availability = 'Available'";

        // RB Display Treatment report for EMPLOYEE
        public static string employeeTreatmentReport = "SELECT DISTINCT treatmentID AS [Treatment ID], cropName AS [Crop Name], startDate AS [From], endDate [Until], fertiliserID AS [Fertiliser to Use], CONCAT(employee.firstName, ' ', employee.lastName) AS [Employee Name], employeeID AS [Employee ID] FROM TREATMENT JOIN employee ON treatment.employeeID = employee.personID";

        // RB Display all crops that are in cultivation for MANAGER
        public static string mangerCropsInCultivation = "SELECT DISTINCT cropName AS [Crop Name], cropType AS [Cop Type], cropInCultivation as [In Cultivation] FROM Crop WHERE cropInCultivation = 'yes'";

        // RB Display harvest timetable for MANAGER
        public static string mgrSelectAllHarvests = "SELECT DISTINCT harvestID AS [Harvest ID], harvestStart AS [Start Date], harvestEnd AS [End Date], harvestMethod AS [Method Used], harvestCrop AS [Crop to Harvest], harvestVehicle AS [Vehicle Needed], harvestContainer AS [Container ID], container.containerType AS [Container Name], harvestEmployeeID AS [Employee ID], CONCAT(employee.firstName, ' ', employee.lastName) AS [Employee Name] FROM HARVEST JOIN Employee ON harvest.harvestEmployeeID = employee.personID JOIN container ON harvest.harvestContainer = container.containerID ORDER BY harvestStart ASC";

        // RB Display harvest labour for MANAGER
        public static string mgrHarvestLabour = "SELECT DISTINCT harvestID AS [Harvest ID], CONCAT(employee.firstName, ' ', employee.lastName) AS [Employee Name], '£ '+FORMAT(employee.hourlyRate,'#,0.00') AS [Rate per Hour], (DATEDIFF(DAY, harvest.harvestStart, harvest.harvestEnd)+1) AS [Days], '£ '+FORMAT(employee.hourlyRate*8*(DATEDIFF(DAY, harvest.harvestStart, harvest.harvestEnd)+1),'#,0.00') AS [Cost] FROM HARVEST JOIN Employee ON harvest.harvestEmployeeID = employee.personID";

        // RB Display vehicles needed for harvest for MANAGER
        public static string mgrVehiclesforHarvest = "SELECT DISTINCT regNumber AS [Vehicle Reg Number], vehicleType AS [Vehicle Type], availability AS [Busy], harvest.harvestID AS [Harvest ID], harvestStart AS [From], harvestEnd AS [Until] FROM Vehicle JOIN Harvest ON vehicle.regNumber = harvest.harvestVehicle WHERE availability='In Use' ORDER BY harvestStart ASC";

        // RB Display Fertilisers Stock for Manager
        public static string displayFertiliserStock = "SELECT DISTINCT fertiliserType AS [Fertiliser Name], fertiliserStock AS [Actual In Stock], fertiliserParLevel AS [Max Amount to Store], fertiliserParLevel-fertiliserStock AS [To Order] FROM Fertiliser";
        
        // RB Display Crop Stock for Manager
        public static string displayCropStock = "SELECT DISTINCT cropName AS [Crop Name], cropType AS [Crop Type], cropAmount AS [Actual In Stock], cropParLevel AS [Max Amount to Store], cropParLevel-cropAmount AS [To Order] FROM Crop";

        #endregion Employee and Manager Reports Queries

        #region Employee Queries

        // Display all employees
        public static string selectAllEmployees = "SELECT DISTINCT personID AS [ID], firstName AS [Name], lastName AS [Last Name], address AS [Address], privilegeLevel AS [Privilege], jobRole AS [Job Title], '£ '+FORMAT(hourlyRate,'#,0.00') AS [Rate p/h] FROM Employee";

        // Insert New Employee
        public static string insertRowIntoEmployee = "INSERT INTO Employee (firstName, lastName, address, privilegeLevel, jobRole, hourlyRate) VALUES (@firstName, @lastName, @address, @privilegeLevel, @jobRole, @hourlyRate)";

        // Delete Employee Record
        public static string deleteEmployeeRow = "DELETE FROM Employee WHERE personID = @personID";

        // Update Employee Records
        public static string updateEmployee = "UPDATE Employee SET firstName = @firstName, lastName = @lastName, address = @address, privilegeLevel = @privilegeLevel, jobRole = @jobRole, hourlyRate = @hourlyRate WHERE personID = @personID";

        #endregion Employee Queries

        #region Customer Queries

        // RB Display All Customers
        public static string selectAllCustomers = "SELECT DISTINCT customerId AS [ID], firstName AS [Name], lastName AS [Last Name], Address FROM Customer";

        // RB Insert New Customer
        public static string insertRowIntoCustomer = "INSERT INTO Customer (firstName, lastName, Address) VALUES (@firstName, @lastName, @address)";

        // RB Delete Customer Record
        public static string deleteCustomer = "DELETE FROM Customer WHERE customerID = @customerID";

        // RB Update Customer Record
        public static string updateCustomer = "UPDATE Customer SET firstName = @firstName, lastName = @lastName, address = @address WHERE customerID = @customerID";

        #endregion Customer Queries

        // RB Login Query
        public static string loginQuery = "SELECT privilegeLevel, personID, firstName FROM Employee WHERE personID = @personID AND firstName = @firstName";

        #endregion RB Queries

        #region MM Queries

        #region Container Queries
        //Display all containers 
        public static string selectAllContainers = "SELECT containerId AS [ID], containerType AS [Type], availability AS [Availability] FROM Container";

        //Add new container
        public static string insertContainer = "INSERT INTO Container (containerType, availability) VALUES (@containerType, @availability)";

        //Delete container
        public static string removeContainer = "DELETE FROM Container WHERE containerId = @containerId";

        // Update container 
        public static string updateContainer = "UPDATE Container SET containerType = @containerType, availability = @availability WHERE containerId = @containerId";

        //get total number of containers
        public static string getTotalContainers = "SELECT COUNT(*) FROM Container";
        //get number of containers in use
        public static string getContainersInUse = "SELECT COUNT(*) FROM Container WHERE availability = 'In Use'";



        #endregion Container Queries

        #region Harvest Queries

        //Add new harvest
        public static string insertHarvest = "INSERT INTO Harvest (harvestStart, harvestEnd, harvestMethod, harvestCrop, harvestVehicle, harvestEmployeeID, harvestContainer) VALUES (@harvestStart, @harvestEnd, @harvestMethod, @harvestCrop, @harvestVehicle, @harvestEmployeeID, @harvestContainer)";
        
        // Update vehicle availability
        public static string updateVehicleAvailability = "UPDATE Vehicle SET availability = @availability WHERE regNumber = @regNumber";

        // Update container availability
        public static string updateContainerAvailability = "UPDATE Container SET availability = @availability WHERE containerId = @containerId";

        //Delete harvest
        public static string removeHarvest = "DELETE FROM Harvest WHERE harvestID = @harvestID";

        //Display all harvests 
        public static string selectAllHarvests = "SELECT harvestID AS [ID], harvestCrop AS [Crop], harvestStart AS [Start], harvestEnd AS [End] FROM Harvest";

        //Display Harvest Employees
        public static string selectHarvestEmployees = "SELECT personID AS [ID], firstName AS [First Name], lastName AS [Last Name] FROM Employee";

        //Display Harvest Crop
        public static string selectHarvestCrop = "SELECT cropName AS [Crop Name], cropType AS [Type]  FROM Crop";

        //Display Harvest Vehicle
        public static string selectHarvestVehicle = "SELECT regNumber AS [Reg Number], vehicleType [Type] FROM Vehicle WHERE availability = 'Available'";

        //Display Harvest containers
        public static string selectHarvestContainers = "SELECT containerId AS [ID], containerType AS [Type] FROM Container WHERE availability = 'Available'";

        #endregion Harvest Queries

        #region Vehicle Queries

        ////Display all vehicles 
        public static string selectAllVehicles = "SELECT regNumber AS [Reg Number], vehicleType AS [Type], availability AS [Availability] FROM Vehicle";

        ////Add new vehicle
        public static string insertVehicle = "INSERT INTO Vehicle (regNumber, vehicleType, availability) VALUES (@regNumber, @vehicleType, @availability)";

        ////Delete vehicle
        public static string removeVehicle = "DELETE FROM Vehicle WHERE regNumber = @regNumber";

        // Update vehicle 
        public static string updateVehicle = "UPDATE Vehicle SET vehicleType = @vehicleType, availability = @availability WHERE regNumber = @regNumber";

        //get total number of vehicle
        public static string getTotalVehicle = "SELECT COUNT(*) FROM Vehicle";

        //get number of vehicle in use
        public static string getVehicleInUse = "SELECT COUNT(*) FROM Vehicle WHERE availability = 'In Use'";

        #endregion Vehicle Queries

        #endregion MM Queries

        #region RK Queries
      
        #region Crop Queries
        //Display all available data about Crop
        public static string getCropData = "SELECT cropName AS [Name], cropType AS [Type], cropAmount AS [Stock Level], cropInCultivation AS [In Cultivation], cropStorageTempMin AS [Storage Temp Min], cropStorageTempMax AS [Storage Temp Max], cropStorageType AS [Storage Type] FROM Crop";

        // Add new row with selected data into Crop 
        public static string insertCropData = "INSERT INTO Crop (cropName, cropType, cropAmount, cropInCultivation, cropStorageTempMin, cropStorageTempMax, cropStorageType) VALUES (@cropType, @cropName, @cropAmount, @cropInCultivation, @cropStorageTempMin, @cropStorageTempMax, @cropStorageType)";
        
        // Update Crop
        public static string updateCropData = "UPDATE Crop SET cropName = @cropName, cropType = @cropType, cropAmount = @cropAmount, cropInCultivation = @cropInCultivation, cropStorageTempMin = @cropStorageTempMin,cropStorageTempMax = @cropStorageTempMax, cropStorageType = @cropStorageType";
        
        //Delete Crop Data
        public static string deleteCropData = "DELETE FROM Crop WHERE cropName = @cropName";
        #endregion Crop Queries

        #endregion RK Queries

        #region TT Queries

        //Display all Treatments 
        public static string selectAllTreatments = "SELECT treatmentID AS [ID], startDate AS [Start], endDate AS [End], cropName AS [Crop Name], employeeID AS [Employee ID], fertiliserType AS [Fertiliser] FROM Treatment";

        //Add new treatment
        public static string insertRowTreatment = "INSERT INTO Treatment (startDate, endDate, cropName, employeeID, fertiliserType) VALUES (@startDate, @endDate, @cropName, @employeeID, @fertiliserType)";

        //Delete treatment
        public static string deleteTreatment = "DELETE FROM Treatment WHERE treatmentID = @treatmentID";

        // Update treatment 
        public static string updateTreatment = "UPDATE Treatment SET startDate = @startDate, endDate = @endDate, cropName = @cropName, employeeID = @employeeID, fertiliserType = @fertiliserType WHERE treatmentID = @treatmentID";

        #endregion TTQueries
    }
}
