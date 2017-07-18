using Moq;
using NUnit.Framework;
using Testable;

namespace MoqQuickStart
{
    [TestFixture]
    public class PropertiesTestFixture
    {
        private Mock<IFoo> mock;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            mock = new Mock<IFoo>();
            //  simple setup
            mock.Setup(foo => foo.Name).Returns("bar");
            //  auto-mocking hierarchies (a.k.a. recursive mocks)
            mock.Setup(foo => foo.Bar.Baz.Name).Returns("baz");
            //  expect an invocation to setup the value to "foo"
            mock.SetupSet(foo => foo.Name = "foo").Verifiable();
            //  setup property so that it automatically tracks its value
            //  "bar" is set as default
            mock.SetupProperty(foo => foo.NickName, "bar");
        }

        [Test]
        public void MatchName()
        {
            Assert.AreEqual("bar", mock.Object.Name);
        }

        [Test]
        public void MatchRecursive()
        {
            Assert.AreEqual("baz", mock.Object.Bar.Baz.Name);
        }

        [Test]
        public void VerifySet()
        {
            mock.Object.Name = "foo";
            mock.Verify();
        }

        [Test]
        public void VerifySetDirectly()
        {
            mock.Object.Name = "myFoo";
            mock.VerifySet(foo => foo.Name = "myFoo");
        }

        [Test]
        public void VerifyPropertyTracking()
        {
            //  Test initial value
            Assert.AreEqual("bar", mock.Object.NickName);
            //  Set value
            mock.Object.NickName = "Stranger";
            //  Check new value
            Assert.AreEqual("Stranger", mock.Object.NickName);
        }

        [Test]
        public void VerifyAll()
        {
            var localMock = new Mock<IBaz>();
            localMock.SetupAllProperties();
            //  now all mock object properties started tracking their values
            Assert.AreEqual(default(int), localMock.Object.Age);
            localMock.Object.Age = 5;
            Assert.AreEqual(5, localMock.Object.Age);
            localMock.Object.Age *= 4;
            Assert.AreEqual(20, localMock.Object.Age);
            Assert.AreEqual(default(string), localMock.Object.Name);
            localMock.Object.Name = "Tom";
            Assert.AreEqual("Tom", localMock.Object.Name);
        }
    }
}
