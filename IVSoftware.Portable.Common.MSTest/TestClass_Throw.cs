using IVSoftware.Portable.Common.Attributes;
using IVSoftware.Portable.Common.Exceptions;
using IVSoftware.Portable.Disposable;
using IVSoftware.WinOS.MSTest.Extensions;
using System.ComponentModel;
using System.Xml;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace IVSoftware.Portable.Common.MSTest
{
    [TestClass]
    public sealed class TestClass_Throw
    {
        [Policy(typeof(InvalidOperationException))]
        private enum TestPolicy
        {
            [Description("Soft policy message")]
            [PolicyEnforcement(ThrowOrAdvise.ThrowSoft)]
            SoftCase,

            [Description("Advisory policy message")]
            [PolicyEnforcement(ThrowOrAdvise.Advisory)]
            AdvisoryCase,
        }

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

        [TestMethod]
        public void Test_ThrowPolicyException_ExposesPolicyError()
        {
            string actual, expected;

            List<string> builderThrow = new();

            void localOnBeginThrowOrAdvise(object? sender, Throw e)
            {
                builderThrow.Add(e.ToString());
                if (e.Mode == ThrowOrAdvise.ThrowHard)
                {
                    e.Handled = true;
                }
                switch (e.PolicyError)
                {
                    case null:
                        break;
                    case TestPolicy.SoftCase:
                        builderThrow.Add($"DETECTED POLICY TestPolicy.SoftCase");
                        break;
                    case TestPolicy.AdvisoryCase:
                        builderThrow.Add($"DETECTED POLICY TestPolicy.AdvisoryCase");
                        break;
                    default:
                        builderThrow.Add($"DETECTED POLICY {e.PolicyError.GetType().Name}.{e.PolicyError}");
                        break;
                }
            }

            using var local = this.WithOnDispose(
                onInit: (sender, e) =>
                {
                    Throw.BeginThrowOrAdvise += localOnBeginThrowOrAdvise;
                },
                onDispose: (sender, e) =>
                {
                    Throw.BeginThrowOrAdvise -= localOnBeginThrowOrAdvise;
                });

            subtest_SoftCase();
            subtest_AdvisoryCase();
            subtest_Arbitrary();

            #region S U B T E S T S
            void subtest_SoftCase()
            {
                Assert.IsTrue(
                    this.ThrowPolicyException(TestPolicy.SoftCase).Handled,
                    $"Expecting {nameof(Throw)} is handled."
                );

                actual = string.Join(Environment.NewLine, builderThrow); builderThrow.Clear();
                actual.ToClipboardExpected();
                { }
                expected = @" 
Id: TestPolicy.SoftCase
Soft policy message
DETECTED POLICY TestPolicy.SoftCase"
                ;

                Assert.AreEqual(
                    expected.NormalizeResult(),
                    actual.NormalizeResult(),
                    "Expecting enum form detected."
                );
            }

            void subtest_AdvisoryCase()
            {
                Assert.IsTrue(
                    this.ThrowPolicyException(TestPolicy.AdvisoryCase).Handled,
                    $"Expecting {nameof(Throw)} is handled."
                );

                actual = string.Join(Environment.NewLine, builderThrow); builderThrow.Clear();
                actual.ToClipboardExpected();
                { }
                expected = @" 
Id: TestPolicy.AdvisoryCase
Advisory policy message
DETECTED POLICY TestPolicy.AdvisoryCase"
                ;

                Assert.AreEqual(
                    expected.NormalizeResult(),
                    actual.NormalizeResult(),
                    "Expecting enum form detected."
                );
            }

            void subtest_Arbitrary()
            {
                Assert.IsTrue(
                    this.ThrowPolicyException(Formatting.Indented).Handled,
                    $"Expecting {nameof(Throw)} is handled."
                );

                actual = string.Join(Environment.NewLine, builderThrow); builderThrow.Clear();
                actual.ToClipboardExpected();
                { }
                expected = @" 
Id: Formatting.Indented
Indented
DETECTED POLICY Formatting.Indented"
                ;

                Assert.AreEqual(
                    expected.NormalizeResult(),
                    actual.NormalizeResult(),
                    "Expecting enum form detected."
                );
            }
            #endregion S U B T E S T S

            var advisory = this.ThrowPolicyException(TestPolicy.AdvisoryCase);
        }
    }
}
