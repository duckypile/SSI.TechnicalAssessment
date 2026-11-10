
using Microsoft.Data.SqlClient;

namespace SSI.EdiViewer
{
    public class ClaimRepository
    {
        private readonly string connectionString =
    Environment.GetEnvironmentVariable("SSI_EDIVIEWER_CONNECTION_STRING")
    ?? @"Server=localhost\MSSQLSERVER01;Database=SSI_EdiViewerDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public void CheckConnection()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public void SaveImport(string fileName, decimal totalCharges, List<Claim> claims)
        {
            // use one connection for the whole import
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            // start a transaction so everything saves together
            using SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                // save the file and get its id
                string fileSql = @"
                    INSERT INTO ImportedFiles (FileName, TotalCharges)
                    OUTPUT INSERTED.FileID
                    VALUES (@FileName, @TotalCharges);
                ";

                using SqlCommand fileCommand =
                    new SqlCommand(fileSql, connection, transaction);

                fileCommand.Parameters.AddWithValue("@FileName", fileName);
                fileCommand.Parameters.AddWithValue("@TotalCharges", totalCharges);

                int fileID = Convert.ToInt32(fileCommand.ExecuteScalar());

                // query for saving each claim
                string claimSql = @"
                    INSERT INTO Claims (
                        FileID,
                        CLM01, CLM02, CLM03, CLM04,
                        CLM05Component1, CLM05Component2, CLM05Component3,
                        CLM06, CLM07, CLM08, CLM09
                    )
                    VALUES (
                        @FileID,
                        @CLM01, @CLM02, @CLM03, @CLM04,
                        @CLM05Component1, @CLM05Component2, @CLM05Component3,
                        @CLM06, @CLM07, @CLM08, @CLM09
                    );
                ";

                // go through every claim and save it
                foreach (Claim claim in claims)
                {
                    using SqlCommand claimCommand =
                        new SqlCommand(claimSql, connection, transaction);

                    // connect this claim to the imported file
                    claimCommand.Parameters.AddWithValue("@FileID", fileID);

                    // add the claim values
                    claimCommand.Parameters.AddWithValue("@CLM01", claim.CLM01);
                    claimCommand.Parameters.AddWithValue("@CLM02", claim.CLM02);
                    claimCommand.Parameters.AddWithValue("@CLM03", claim.CLM03);
                    claimCommand.Parameters.AddWithValue("@CLM04", claim.CLM04);

                    // add the three clm05 parts
                    claimCommand.Parameters.AddWithValue("@CLM05Component1", claim.CLM05Component1);
                    claimCommand.Parameters.AddWithValue("@CLM05Component2", claim.CLM05Component2);
                    claimCommand.Parameters.AddWithValue("@CLM05Component3", claim.CLM05Component3);

                    // add the remaining values
                    claimCommand.Parameters.AddWithValue("@CLM06", claim.CLM06);
                    claimCommand.Parameters.AddWithValue("@CLM07", claim.CLM07);
                    claimCommand.Parameters.AddWithValue("@CLM08", claim.CLM08);
                    claimCommand.Parameters.AddWithValue("@CLM09", claim.CLM09);

                    claimCommand.ExecuteNonQuery();
                }

                // everything worked, so save the changes
                transaction.Commit();
            }
            catch
            {
                // something failed, so undo the import
                transaction.Rollback();
                throw;
            }
        }

        public List<ImportedFile> GetImportedFiles()
        {
            // make a list for all saved files
            List<ImportedFile> files = new List<ImportedFile>();

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            // get the newest imported files first
            string sql = @"
                SELECT FileID, FileName, ImportedAt, TotalCharges
                FROM ImportedFiles
                ORDER BY ImportedAt DESC, FileID DESC;
            ";

            using SqlCommand command = new SqlCommand(sql, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                // turn the current row into an importedfile object
                ImportedFile file = new ImportedFile
                {
                    FileID = reader.GetInt32(reader.GetOrdinal("FileID")),
                    FileName = reader.GetString(reader.GetOrdinal("FileName")),
                    ImportedAt = reader.GetDateTime(reader.GetOrdinal("ImportedAt")),
                    TotalCharges = reader.GetDecimal(reader.GetOrdinal("TotalCharges"))
                };

                files.Add(file);
            }

            return files;
        }

        public List<Claim> GetClaimsByFileID(int fileID)
        {
            // make a list for the saved claims
            List<Claim> claims = new List<Claim>();

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            // get claims that belong to the selected file
            string sql = @"
                SELECT
                    CLM01, CLM02, CLM03, CLM04,
                    CLM05Component1, CLM05Component2, CLM05Component3,
                    CLM06, CLM07, CLM08, CLM09
                FROM Claims
                WHERE FileID = @FileID
                ORDER BY ClaimID;
            ";

            using SqlCommand command = new SqlCommand(sql, connection);

            // only get claims for this file id
            command.Parameters.AddWithValue("@FileID", fileID);

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                // turn the database row back into a claim object
                Claim claim = new Claim
                {
                    CLM01 = reader.GetString(reader.GetOrdinal("CLM01")),
                    CLM02 = reader.GetDecimal(reader.GetOrdinal("CLM02")),

                    // use empty strings if sql has null values
                    CLM03 = reader["CLM03"] as string ?? "",
                    CLM04 = reader["CLM04"] as string ?? "",

                    CLM05Component1 = reader.GetString(reader.GetOrdinal("CLM05Component1")),
                    CLM05Component2 = reader.GetString(reader.GetOrdinal("CLM05Component2")),
                    CLM05Component3 = reader.GetString(reader.GetOrdinal("CLM05Component3")),

                    CLM06 = reader["CLM06"] as string ?? "",
                    CLM07 = reader["CLM07"] as string ?? "",
                    CLM08 = reader["CLM08"] as string ?? "",
                    CLM09 = reader["CLM09"] as string ?? ""
                };

                claims.Add(claim);
            }

            return claims;
        }
    }
}
