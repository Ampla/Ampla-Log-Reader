using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Ampla.LogReader
{
    /// <summary>
    ///		Base class for TestFixtures with provision for specializing other base classes
    /// </summary>
    /// <remarks>
    ///		UnitTest classes should specialize:
    ///			- OnFixtureSetUp()
    ///			- OnFixtureTearDown()
    ///			- OnSetUp()
    ///			- OnTearDown()
    /// </remarks>
    [TestFixture]
    public abstract class TestFixture
    {
        protected Exception FixtureException { get; private set; }

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            string typeName = GetType().FullName;
            Debug.Assert(typeName != null);
            Debug.WriteLine(new string('=', typeName.Length));
            Debug.WriteLine("[TestFixture]");
            Debug.WriteLine(typeName);
            Debug.WriteLine(new string('=', typeName.Length));

            try
            {
                OnFixtureSetUp();
            }
            catch (IgnoreException)
            {
                throw;
            }
            catch (Exception e)
            {
                FixtureException = e;
            }
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            try
            {
                OnFixtureTearDown();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        [SetUp]
        public void SetUp()
        {
            Assert.AreEqual(null, FixtureException, "[TestFixtureSetUp]");

            OnSetUp();
        }

        [TearDown]
        public void TearDown()
        {
            OnTearDown();
        }

        /// <summary>
        ///		FixtureSetup method to override in UnitTests
        /// </summary>
        protected virtual void OnFixtureSetUp()
        {
        }

        /// <summary>
        ///		FixtureTearDown method to override in UnitTests
        /// </summary>
        protected virtual void OnFixtureTearDown()
        {
        }

        /// <summary>
        ///		Setup method to override in UnitTests
        /// </summary>
        protected virtual void OnSetUp()
        {
        }

        /// <summary>
        ///		TearDown method to override in UnitTests
        /// </summary>
        protected virtual void OnTearDown()
        {
        }
    }
}