// learned decimal.TryParse works different for Euro countries decimal is ',' rather than '.'
using System.Globalization;
namespace SSI.EdiViewer
{
    public class ClaimParser
    {
        public Claim ParseClaim(string clmSegment)
        {
            Claim claim = new Claim();
            clmSegment = clmSegment.TrimEnd('~');
            string[] clmSplit = clmSegment.Split('*');

            //checks segment header is == CLM
            if (clmSplit[0] != "CLM")
            {
                throw new FormatException("Invalid segment type!");
            }

            // checks if the CLM segment has atleast 10 elements (atleast to fill CLM01 to CLM09)
            if (clmSplit.Length < 10)
            {
                throw new FormatException("Missing elements from CLM segment!");
            }


            if (clmSplit.Length <= 1 || string.IsNullOrWhiteSpace(clmSplit[1]))
            {
                throw new FormatException("CLM01 is missing!");
            }

            // stores valid CLM01 amount
            claim.CLM01 = clmSplit[1];

            // checking if CLM02 exists
            if (clmSplit.Length <= 2)
            {
                throw new FormatException("CLM02 is missing!");
            }

            // checking if CLM02 is a valid decimal
            if (!decimal.TryParse(clmSplit[2], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal value))
            {
                throw new FormatException("CLM02 contains an invalid charge amount!");
            }

            // stores valid CLM02 amount
            claim.CLM02 = value;

            // not sure on input validation, so allow empty string
            claim.CLM03 = clmSplit[3];
            claim.CLM04 = clmSplit[4];

            if (clmSplit.Length <= 5)
            {
                throw new FormatException("CLM05 is Missing!");
            }
                string[] claimComponent = clmSplit[5].Split(':');
                if (claimComponent.Length != 3)
                {
                    throw new FormatException("Wrong number of Components");
                }

                if (string.IsNullOrWhiteSpace(claimComponent[0]) || string.IsNullOrWhiteSpace(claimComponent[1]) || string.IsNullOrWhiteSpace(claimComponent[2]))
                {
                    throw new FormatException("Missing Component");
                }
                claim.CLM05Component1 = claimComponent[0];
                claim.CLM05Component2 = claimComponent[1];
                claim.CLM05Component3 = claimComponent[2];

            // same idea CLM03-04
            claim.CLM06 = clmSplit[6];
            claim.CLM07 = clmSplit[7];
            claim.CLM08 = clmSplit[8];
            claim.CLM09 = clmSplit[9];


            return claim;
        }


        public List<Claim> ParseClaimsFromFile (string filePath)
        {
            List<Claim> claims = new List<Claim>();
            // converts entire file to 1 string
            string entireFileString = File.ReadAllText(filePath);

            // since each segment has a '~' end, .split('~'), and check if it contains "CLM"
            string[] segmentedFileStrings = entireFileString.Split('~');

            foreach(string segmentedFileString in segmentedFileStrings)
            {
                string cleanSegment = segmentedFileString.Trim();

                if (cleanSegment.StartsWith("CLM*"))
                {
                    // call ParseClaim to parse segment
                    // store in Claim object
                    // add to list of parsed claims from file
                    Claim parsedClaim = ParseClaim(cleanSegment);
                    claims.Add(parsedClaim);
                }
            }

            return claims;
        }

        public decimal CalculateTotalCharges(List<Claim> claims)
        {
            decimal total = 0;
            foreach (Claim claim in claims)
            {
                total += claim.CLM02;
            }
            return total;
        }

    }
}
