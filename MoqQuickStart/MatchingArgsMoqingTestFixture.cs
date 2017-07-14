using Moq;
using NUnit.Framework;
using System.Text.RegularExpressions;
using Testable;

namespace MoqQuickStart
{
    [TestFixture]
    public class MatchingArgsMoqingTestFixture
    {
        private Mock<IFoo> mock;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            mock = new Mock<IFoo>();
            //  any value
            mock.Setup(foo => foo.DoSomething(It.IsAny<string>()))
                .Returns(true);
            //  matching Func<int>, lazy evaluated
            mock.Setup(foo => foo.Add(It.Is<int>(i => i % 2 == 0)))
                .Returns(true);
            mock.Setup(foo => foo.Add(It.Is<int>(i => i % 2 == 1)))
                .Returns(false);
            //  matching ranges
            mock.Setup(foo => foo.Subtract(It.IsInRange<int>(0, 10, Range.Inclusive)))
                .Returns(true);
            mock.Setup(foo => foo.Subtract(It.IsInRange(11, int.MaxValue, Range.Inclusive)))
                .Returns(false);
            mock.Setup(foo => foo.Subtract(It.IsInRange(int.MinValue, -1, Range.Inclusive)))
                .Returns(false);
            //  matching regex
            mock.Setup(foo => foo.DoSomethingStringy(It.IsAny<string>()))
                .Returns("bar");
            mock.Setup(foo => foo.DoSomethingStringy(It.IsRegex("^[a-d]+$", RegexOptions.IgnoreCase)))
                .Returns("foo");
        }

        [Test]
        public void MatchAnyValue()
        {
            Assert.IsTrue(mock.Object.DoSomething("any value"));
            Assert.IsTrue(mock.Object.DoSomething("Any Other Value"));
            Assert.IsTrue(mock.Object.DoSomething("Any value at all"));
        }

        [Test]
        public void MatchFunc()
        {
            Assert.IsFalse(mock.Object.Add(5));
            Assert.IsTrue(mock.Object.Add(6));
        }

        [Test]
        public void Ranges()
        {
            Assert.IsTrue(mock.Object.Subtract(5));
            Assert.IsTrue(mock.Object.Subtract(6));
            Assert.IsTrue(mock.Object.Subtract(10));
            Assert.IsFalse(mock.Object.Subtract(11));
            Assert.IsFalse(mock.Object.Subtract(-10));
        }

        [Test]
        public void Regex()
        {
            Assert.AreEqual("foo", mock.Object.DoSomethingStringy("cbdbadda"));
            Assert.AreNotEqual("foo", mock.Object.DoSomethingStringy("hdda"));
        }
    }
}
