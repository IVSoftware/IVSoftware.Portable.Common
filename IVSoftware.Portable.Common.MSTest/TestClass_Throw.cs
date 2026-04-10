using IVSoftware.Portable.Common.Exceptions;
using IVSoftware.Portable.Disposable;
using IVSoftware.WinOS.MSTest.Extensions;

namespace IVSoftware.Portable.Common.MSTest
{
    [TestClass]
    public sealed class TestClass_Throw
    {
        [TestMethod]
        public void Test_Throw()
        {
            string actual, expected;

            #region L o c a l F x
            var builderThrow = new List<string>();
            void localOnBeginThrowOrAdvise(object? sender, Throw e)
            {
                builderThrow.Add($"Handled={e.Handled} {e.Message}");
                e.Handled = true;
            }
            #endregion L o c a l F x
            using var local = this.WithOnDispose(
                onInit: (sender, e) =>
                {
                    Throw.BeginThrowOrAdvise += localOnBeginThrowOrAdvise;
                },
                onDispose: (sender, e) =>
                {
                    Throw.BeginThrowOrAdvise -= localOnBeginThrowOrAdvise;
                });

            // This format checks the success of handling it within the local handler.
            Assert.IsTrue(this.ThrowHard<InvalidOperationException>().Handled);

            actual = string.Join(Environment.NewLine, builderThrow); builderThrow.Clear();
            actual.ToClipboardExpected();
            { }
            expected = @" 
Handled=False InvalidOperationException"
            ;

            Assert.AreEqual(
                expected.NormalizeResult(),
                actual.NormalizeResult(),
                "Expecting NOT HANDLED."
            );

            // This format checks the success of handling it within the local handler.
            Assert.IsTrue(
                this.ThrowFramework<NotSupportedException>()
                .Handled);

            actual = string.Join(Environment.NewLine, builderThrow); builderThrow.Clear();
            actual.ToClipboardExpected();
            { }
            expected = @" 
Handled=False NotSupportedException"
            ;

            Assert.AreEqual(
                expected.NormalizeResult(),
                actual.NormalizeResult(),
                "Expecting NOT HANDLED."
            );

            this.ThrowFramework<NotSupportedException>(@throw: false);

            actual = string.Join(Environment.NewLine, builderThrow); builderThrow.Clear();
            actual.ToClipboardExpected();
            { }
            expected = @" 
Handled=True NotSupportedException"
            ;

            Assert.AreEqual(
                expected.NormalizeResult(),
                actual.NormalizeResult(),
                "Expecting HANDLED."
            );

            this.ThrowSoft<NotSupportedException>(@throw: false);

            actual = string.Join(Environment.NewLine, builderThrow); builderThrow.Clear();
            actual.ToClipboardExpected();
            { }
            expected = @" 
Handled=True NotSupportedException"
            ;

            Assert.AreEqual(
                expected.NormalizeResult(),
                actual.NormalizeResult(),
                "Expecting HANDLED."
            );

            this.RethrowHard(new NullReferenceException());

            actual = string.Join(Environment.NewLine, builderThrow); builderThrow.Clear();
            actual.ToClipboardExpected();
            { }
            expected = @" 
Handled=False Object reference not set to an instance of an object."
            ;

            Assert.AreEqual(
                expected.NormalizeResult(),
                actual.NormalizeResult(),
                "Expecting NOT HANDLED."
            );

            this.RethrowSoft(new NullReferenceException());

            actual = string.Join(Environment.NewLine, builderThrow); builderThrow.Clear();
            actual.ToClipboardExpected();
            { }
            expected = @" 
Handled=True Object reference not set to an instance of an object."
            ;

            Assert.AreEqual(
                expected.NormalizeResult(),
                actual.NormalizeResult(),
                "Expecting HANDLED."
            );
        }
    }
}
