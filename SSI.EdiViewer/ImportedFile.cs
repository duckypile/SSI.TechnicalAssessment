namespace SSI.EdiViewer
{
    public class ImportedFile
    {
        public int FileID { get; set; }
        public string FileName { get; set; } = "";
        public DateTime ImportedAt { get; set; }
        public decimal TotalCharges { get; set; }
    }
}
