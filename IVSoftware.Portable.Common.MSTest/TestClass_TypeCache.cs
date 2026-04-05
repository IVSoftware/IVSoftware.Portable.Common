using IVSoftware.WinOS.MSTest.Extensions;
using Newtonsoft.Json;
using static IVSoftware.Portable.Common.Common;

namespace IVSoftware.Portable.Common.MSTest
{
    [TestClass]
    public class TestClass_TypeCache
    {

        [TestMethod]
        public void Test_TypeCache()
        {
            string actual, expected;

            var types = TypeCache[string.Empty];

            actual = JsonConvert.SerializeObject(types, Formatting.Indented);
            actual.ToClipboardExpected();
            { }
            expected = @" 
[
  ""Internal.Console, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"",
  ""Internal.Console+Error, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"",
  ""IVSoftware.Portable.Common.MSTest.TestClass_Throw, IVSoftware.Portable.Common.MSTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.MSTest.TestClass_TypeCache, IVSoftware.Portable.Common.MSTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Extensions, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.ITypeCache, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.TypeCacheMatchMode, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Common, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.TypeCacheExtensions, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.CanonicalAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.CarefulAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.ProbationaryAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.ScaffoldingAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.UnsupportedAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.IndexerAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.PolicyAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.PolicyEnforcementAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.NotFlagsAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.ClaimAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Exceptions.Advisory, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Exceptions.ThrowOrAdvise, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Exceptions.ThrowableStatus, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Exceptions.ThrowToStringFormat, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Exceptions.Throw, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Exceptions.ThrowExtensions, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.WinOS.MSTest.Extensions.Extensions, IVSoftware.WinOS.MSTest.Extensions, Version=1.0.8.0, Culture=neutral, PublicKeyToken=61187cbbe9faa94a""
]"
            ;
            Assert.AreEqual(
                expected.NormalizeResult(),
                actual.NormalizeResult(),
                "Expecting json serialization to match."
            );

            "Newtonsoft".AppendNamespaceToCache();

            { }

            actual = JsonConvert.SerializeObject(types, Formatting.Indented);
            actual.ToClipboardExpected();
            { }
            expected = @" 
[
  ""Internal.Console, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"",
  ""Internal.Console+Error, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"",
  ""IVSoftware.Portable.Common.MSTest.TestClass_Throw, IVSoftware.Portable.Common.MSTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.MSTest.TestClass_TypeCache, IVSoftware.Portable.Common.MSTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Extensions, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.ITypeCache, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.TypeCacheMatchMode, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Common, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.TypeCacheExtensions, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.CanonicalAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.CarefulAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.ProbationaryAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.ScaffoldingAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.UnsupportedAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.IndexerAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.PolicyAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.PolicyEnforcementAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.NotFlagsAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Attributes.ClaimAttribute, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Exceptions.Advisory, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Exceptions.ThrowOrAdvise, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Exceptions.ThrowableStatus, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Exceptions.ThrowToStringFormat, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Exceptions.Throw, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.Portable.Common.Exceptions.ThrowExtensions, IVSoftware.Portable.Common, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null"",
  ""IVSoftware.WinOS.MSTest.Extensions.Extensions, IVSoftware.WinOS.MSTest.Extensions, Version=1.0.8.0, Culture=neutral, PublicKeyToken=61187cbbe9faa94a""
]"
            ;

            Assert.AreEqual(
                expected.NormalizeResult(),
                actual.NormalizeResult(),
                "Expecting namespace appended."
            );
        }
    }
}
