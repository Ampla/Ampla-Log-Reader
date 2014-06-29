using System.Collections.Generic;
using NUnit.Framework;

namespace Ampla.LogReader.Remoting
{
    [TestFixture]
    public class IdentitySessionUnitTests : TestFixture
    {
        [Test]
        public void Identity()
        {
            IdentitySession identitySession = new IdentitySession("Identity", "Session");
            Assert.That(identitySession.Identity, Is.EqualTo("Identity"));
        }

        [Test]
        public void Session()
        {
            IdentitySession identitySession = new IdentitySession("Identity", "Session");
            Assert.That(identitySession.Session, Is.EqualTo("Session"));
        }

        [Test]
        public void TestToString()
        {
            IdentitySession identitySession = new IdentitySession("Identity", "Session");
            Assert.That(identitySession.ToString(), Is.EqualTo("Identity (Session)"));
        }

        [Test]
        public void TestEquals()
        {
            IdentitySession identitySession1 = new IdentitySession("Identity", "Session");
            IdentitySession identitySession2 = new IdentitySession("Identity", "Session");
            IdentitySession identitySession3 = new IdentitySession("Identity1", "Session");

            Assert.That(identitySession1, Is.EqualTo(identitySession2));
            Assert.That(identitySession2, Is.EqualTo(identitySession1));
            Assert.That(identitySession1, Is.Not.EqualTo(identitySession3));
            Assert.That(identitySession2, Is.Not.EqualTo(identitySession3));
        }

        [Test]
        public void Dictionary()
        {
            IdentitySession identitySession1 = new IdentitySession("Identity", "Session");
            IdentitySession identitySession2 = new IdentitySession("Identity", "Session");
            IdentitySession identitySession3 = new IdentitySession("Identity1", "Session");
            IdentitySession nullSession3 = new IdentitySession("Identity1", null);
            Dictionary<IdentitySession, string> dictionary = new Dictionary<IdentitySession,string>();

            dictionary[identitySession1] = identitySession1.Identity;
            Assert.That(dictionary.Count, Is.EqualTo(1));

            dictionary[identitySession2] = identitySession2.Identity;
            Assert.That(dictionary.Count, Is.EqualTo(1));

            dictionary[identitySession3] = identitySession3.Identity;
            Assert.That(dictionary.Count, Is.EqualTo(2));

            dictionary[nullSession3] = nullSession3.Identity;
            Assert.That(dictionary.Count, Is.EqualTo(3));

        }

        [Test]
        public void NullValues()
        {
            IdentitySession session = new IdentitySession(null, null);
            Assert.That(session.ToString(), Is.Not.Empty);
            Assert.That(session.GetHashCode(), Is.Not.EqualTo(0));
            Assert.That(session.Equals(new IdentitySession(null, null)), Is.True);
        }
    }
}