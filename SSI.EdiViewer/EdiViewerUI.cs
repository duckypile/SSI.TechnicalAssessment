
using Microsoft.Data.SqlClient;

namespace SSI.EdiViewer
{
    public partial class EdiViewerUI : Form
    {
        public EdiViewerUI()
        {
            InitializeComponent();

            // connect the history button and grid events
            btnViewHistory.Click += btnViewHistory_Click;
            dgvImportedFiles.CellDoubleClick += dgvImportedFiles_CellDoubleClick;

            // make the history grid easier to use
            dgvImportedFiles.ReadOnly = true;
            dgvImportedFiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvImportedFiles.MultiSelect = false;
            dgvImportedFiles.AutoGenerateColumns = true;
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "EDI 837 Files (*.837)|*.837|All Files (*.*)|*.*";
            dialog.Title = "Select an EDI 837 File";

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                // create the parser
                ClaimParser parser = new ClaimParser();

                // parse every clm segment in the selected file
                List<Claim> claims = parser.ParseClaimsFromFile(dialog.FileName);

                // don't save files that have no claims
                if (claims.Count == 0)
                {
                    throw new FormatException("No CLM segments found in the selected file.");
                }

                // calculate the total charges
                decimal totalCharges = parser.CalculateTotalCharges(claims);

                // create the repository
                ClaimRepository repository = new ClaimRepository();

                // save the file and its claims to sql server
                repository.SaveImport(
                    Path.GetFileName(dialog.FileName),
                    totalCharges,
                    claims
                );

                // display the claims
                dgvClaims.DataSource = null;
                dgvClaims.DataSource = claims;

                // display the total charges
                lblTotalCharges.Text =
                    "Total Claim Charge: " + totalCharges.ToString("C2");

                // display the selected filename
                lblFileName.Text = Path.GetFileName(dialog.FileName);

                // show a message after saving
                MessageBox.Show(
                    "EDI file saved successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (FormatException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Invalid EDI File",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch (IOException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "File Read Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnViewHistory_Click(object sender, EventArgs e)
        {
            try
            {
                // get all previously imported files
                ClaimRepository repository = new ClaimRepository();
                List<ImportedFile> files = repository.GetImportedFiles();

                // display the files in the history grid
                dgvImportedFiles.DataSource = null;
                dgvImportedFiles.DataSource = files;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void dgvImportedFiles_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            // ignore clicks on the column headers
            if (e.RowIndex < 0)
            {
                return;
            }

            // get the file the user double-clicked
            ImportedFile? selectedFile =
                dgvImportedFiles.Rows[e.RowIndex].DataBoundItem as ImportedFile;

            if (selectedFile == null)
            {
                return;
            }

            try
            {
                ClaimRepository repository = new ClaimRepository();

                // get the claims saved under this file
                List<Claim> claims =
                    repository.GetClaimsByFileID(selectedFile.FileID);

                // show the saved claims
                dgvClaims.DataSource = null;
                dgvClaims.DataSource = claims;

                // display the saved filename and total
                lblFileName.Text = selectedFile.FileName;

                lblTotalCharges.Text =
                    "Total Claim Charge: " +
                    selectedFile.TotalCharges.ToString("C2");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
