SSI Technical Assessment - EDI 837 Viewer

This project is a C Windows Forms application I built for the SSI Technical Assessment. The application reads EDI 837 files, extracts claim information, and displays it in a table. It also calculates the total claim charges and allows users to save and view previously imported files using SQL Server.

Features
- Upload and read EDI 837 files
- Extract CLM01 through CLM09 from each claim
- Split CLM05 into its three components
- Display claim information in a DataGridView
- Calculate the total charges from all claims
- Save imported files and claims to SQL Server
- View previously imported files and their claims
- Unit tests for the claim parser

Technologies Used
- C / .NET 10
- Windows Forms
- SQL Server
- Microsoft.Data.SqlClient
- MSTest
- Visual Studio

How to Run
1. Open the solution in Visual Studio.
2. Make sure SQL Server is installed and running.
3. Open DatabaseSetup.sql in SQL Server Management Studio (SSMS) and execute it to create the database and tables.
4. Open ClaimRepository.cs and update the connection string to match your SQL Server instance if needed.
5. Set SSI.EdiViewer as the startup project.
6. Build and run the application.

How to Use

Importing a File
1. Click the **Upload File** button.
2. Select an EDI 837 file.
3. The application will read the file and display the claims.
4. The total claim charges will be displayed, and the file information will be saved to SQL Server.

Viewing Previous Files
1. Click the **View History** button.
2. Previously imported files will appear in the history table.
3. Double-click a file to view its saved claims.

Project Files
- Claim.cs - Stores the information for a claim.
- ClaimParser.cs - Reads EDI files and extracts claim data.
- ClaimRepository.cs - Handles saving and retrieving data from SQL Server.
- ImportedFile.cs - Stores information about previously imported files.
- EdiViewerUI.cs - Handles the Windows Forms interface.
- DatabaseSetup.sql - Creates the database and tables.
- SSI.EdiViewer.Tests - Contains unit tests for the claim parser.

Note:
- SQL Server needs to be set up before importing or viewing saved files. The included SQL script can be used to create the required database and tables.
