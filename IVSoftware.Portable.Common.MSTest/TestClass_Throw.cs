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
                builderThrow.Add(e.Message);
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

            this.ThrowHard<InvalidOperationException>();


            actual = string.Join(Environment.NewLine, builderThrow);
            actual.ToClipboardExpected();
            { }
            expected = @" 
InvalidOperationException";

            Assert.AreEqual(
                expected.NormalizeResult(),
                actual.NormalizeResult(),
                "Expecting logged throw messages."
            );
        }
    }
}
