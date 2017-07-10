using NUnit.Framework;
using Testable;
using Moq;
using System;

namespace MoqQuickStart
{
    [TestFixture]
    class MethodsMoqingTestFixture
    {
        private Mock<IFoo> mock;
        private Bar instance;
        private const string category = nameof(MethodsMoqingTestFixture);
        private int count = 1;
        private int calls = 0;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            mock = new Mock<IFoo>();
            //  Simple setup
            mock.Setup(foo => foo.DoSomething("ping")).Returns(true);
            mock.Setup(foo => foo.DoSomething("badPing")).Returns(false);
            //  out argument
            var outStr = "ack";
            mock.Setup(foo => foo.TryParse("ping", out outStr)).Returns(true);
            //  ref argument
            instance = new Bar();
            mock.Setup(foo => foo.Submit(ref instance)).Returns(true);
            //  access invocation arguments when returning a value
            mock.Setup(foo => foo.DoSomethingStringy(It.IsAny<string>()))
                .Returns((string s) => s.ToLower());
            //  multiple parameter overload available
            mock.Setup(foo => foo.DoSomethingDoubleStringy(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string first, string second) => $"{first} {second}");
            //  throwing when invoked with specific parameters
            mock.Setup(foo => foo.DoSomething("reset"))
                .Throws<InvalidOperationException>();
            mock.Setup(foo => foo.DoSomething(""))
                .Throws(new InvalidOperationException("command"));
            //  lazy evaluating return value
            mock.Setup(foo => foo.GetCount()).Returns(() => count);
            //  return different value for each invokation
            mock.Setup(foo => foo.GetCountThing()).Returns(() => calls)
                .Callback(() => ++calls);
        }

        [Test]
        public void PingOK()
        {
            Assert.IsTrue(mock.Object.DoSomething("ping"));
        }

        [Test]
        public void PingFail()
        {
            Assert.IsFalse(mock.Object.DoSomething("badPing"));
        }

        [Test]
        public void TryParseOK()
        {
            var outString = "";
            var isOK = mock.Object.TryParse("ping", out outString);
            Assert.IsTrue(isOK);
            Assert.AreEqual("ack", outString);
        }

        [Test]
        public void SubmitBar()
        {
            Assert.IsTrue(mock.Object.Submit(ref instance));
        }

        [Test]
        public void StringyThing()
        {
            var str = mock.Object.DoSomethingStringy("Light My Fire");
            Assert.AreEqual("light my fire", str);
        }

        [Test]
        public void FooBarConcat()
        {
            var concat = mock.Object.DoSomethingDoubleStringy("foo", "bar");
            Assert.AreEqual("foo bar", concat);
        }

        [Test]
        public void ThrowExceptions()
        {
            Assert.Throws<InvalidOperationException>(
                () => mock.Object.DoSomething("reset"));
            Assert.Throws(typeof(InvalidOperationException), 
                () => mock.Object.DoSomething(""),
                "command");
        }

        [Test]
        public void LazyCount()
        {
            Assert.AreEqual(1, mock.Object.GetCount());
            count = 5;
            Assert.AreEqual(5, mock.Object.GetCount());
        }

        [Test]
        public void DiffVals()
        {
            Assert.AreEqual(0, mock.Object.GetCountThing());
            Assert.AreEqual(1, mock.Object.GetCountThing());
            Assert.AreEqual(2, mock.Object.GetCountThing());
            Assert.AreEqual(3, mock.Object.GetCountThing());
            Assert.AreEqual(4, mock.Object.GetCountThing());
        }
    }
}
