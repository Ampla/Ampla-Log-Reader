using NUnit.Framework;

namespace Ampla.LogReader.EventLogs
{
    [TestFixture]
    public class SecurityResolverUnitTests : TestFixture
    {
        [Test]
        public void NtAuthoritySystem()
        {
            AssertName("S-1-5-18", @"NT AUTHORITY\SYSTEM");
        }

        private static void AssertName(string sid, string expected)
        {
            ISecurityResolver resolver = new SecurityResolver();
            string name = resolver.GetName(sid);

            Assert.That(name, Is.EqualTo(expected));
        }

        [Test]
        public void UnknownUser()
        {
            const string unknown = "S-1-5-21-3765203034-2564593448-2833180799-1004";
            AssertName(unknown, unknown);
        }
        
        [Test]
        public void EmptyString()
        {
            AssertName("", "");
        }

        [Test]
        public void NullString()
        {
            AssertName(null, "");
        }

        [Test]
        public void StringSid()
        {
            const string name = @"NT AUTHORITY\SYSTEM";
            AssertName(name, name);
        }
         
    }
}