using Bogus.DataSets;
using Gigya.Socialize.SDK;
using System;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace GSCSharpSDK.Tests
{
    public class UtilTests
    {
        private readonly static Lorem TextGenerator = new Lorem();
        [Fact]
        public void EnsureOldAndNewCalcSignatureWorks()
        {
            var key = Convert.ToBase64String(Encoding.UTF8.GetBytes(TextGenerator.Sentence()));
            var text = TextGenerator.Sentence();

            var newSig = SigUtils.CalcSignature(text, key);
            var oldSig = CalcSignatureOld(text, key);

            Assert.Equal(oldSig, newSig);
        }

        private static string CalcSignatureOld(string text, string key)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            byte[] keyData = Convert.FromBase64String(key);

            // Compute signature for provided challenge and private key
            // Always use HMAC-SHA1 algorithm
            using HMAC hmac = new HMACSHA1(keyData);

            byte[] macReciever = hmac.ComputeHash(data);
            return Convert.ToBase64String(macReciever);
        }
    }
}
