using SSI.EdiViewer;
namespace SSI.EdiViewer.Tests
{
    [TestClass]
    public sealed class ClaimParserTests
    {
        private ClaimParser parser = null!;

        [TestInitialize]
        public void Setup()
        {
            parser = new ClaimParser();
        }

        private List<Claim> LoadSampleClaims()
        {
            string filePath = Path.Combine(
                AppContext.BaseDirectory,
                "Sample Professional.837"
            );

            return parser.ParseClaimsFromFile(filePath);
        }

        [TestMethod]
        public void ParseClaimsFromFile_HasTwoClaims()
        {
            List<Claim> claims = LoadSampleClaims();
            Assert.AreEqual(2, claims.Count);
        }

        [TestMethod]
        public void ParseClaimsFromFile_ReturnsCorrectClaimIdentifiers()
        {
            List<Claim> claims = LoadSampleClaims();
            Assert.AreEqual("XYZ123000456", claims[0].CLM01);
            Assert.AreEqual("XYZ123000567", claims[1].CLM01);
        }
        [TestMethod]
        public void ParseClaimsFrom_ReturnsCorrectChargeAmounts()
        {

            List<Claim> claims = LoadSampleClaims();
            Assert.AreEqual(25m, claims[0].CLM02);
            Assert.AreEqual(10m, claims[1].CLM02);

        }
        [TestMethod]
        public void ParseClaimsFromFile_ReturnsCorrectCLM05Components()
        {
            List<Claim> claims = LoadSampleClaims();
            Assert.AreEqual("01", claims[0].CLM05Component1);
            Assert.AreEqual("B", claims[0].CLM05Component2);
            Assert.AreEqual("1", claims[0].CLM05Component3);

            Assert.AreEqual("01", claims[1].CLM05Component1);
            Assert.AreEqual("B", claims[1].CLM05Component2);
            Assert.AreEqual("1", claims[1].CLM05Component3);
        }

        [TestMethod]
        public void ParseClaim_InvalidSegmentType_ThrowsException()
        {
            string testString = "NM1*ABC123*25***01:B:1*N*C*Y*Y~";
            

            Assert.ThrowsExactly<FormatException> (
                () => parser.ParseClaim(testString)
                );
        }

        [TestMethod]
        public void ParseClaim_InvalidChargeAmount_ThrowsException()
        {
            string testString = "CLM*ABC123*abc***01:B:1*N*C*Y*Y~";

            Assert.ThrowsExactly<FormatException>(
                () => parser.ParseClaim(testString)
                );
        }

        [TestMethod]
        public void ParseClaim_MissingCLM01_ThrowsException()
        {
            string testString = "CLM**25***01:B:1*N*C*Y*Y~";

            Assert.ThrowsExactly<FormatException>(
                () => parser.ParseClaim(testString)
                );
        }

        [TestMethod]
        public void ParseClaim_InvalidCLM05_ThrowsException()
        {
            string testString = "CLM*ABC123*25***01::*N*C*Y*Y~";
                
            Assert.ThrowsExactly<FormatException>(
                () => parser.ParseClaim(testString)
                );
        }

        [TestMethod]
        public void ParseClaimsFromFile_ReturnsCorrectCLM03ToCLM04()
        {
            List<Claim> claims = LoadSampleClaims();
            Assert.AreEqual("", claims[0].CLM03);
            Assert.AreEqual("", claims[0].CLM04);

            Assert.AreEqual("", claims[1].CLM03);
            Assert.AreEqual("", claims[1].CLM04);
        }

        [TestMethod]
        public void ParseClaimsFromFile_ReturnsCorrectCLM06ToCLM09()
        {
            List<Claim> claims = LoadSampleClaims();
            Assert.AreEqual("N", claims[0].CLM06);
            Assert.AreEqual("C", claims[0].CLM07);
            Assert.AreEqual("Y", claims[0].CLM08);
            Assert.AreEqual("Y", claims[0].CLM09);

            Assert.AreEqual("N", claims[1].CLM06);
            Assert.AreEqual("C", claims[1].CLM07);
            Assert.AreEqual("Y", claims[1].CLM08);
            Assert.AreEqual("Y", claims[1].CLM09);
        }

    }
}
