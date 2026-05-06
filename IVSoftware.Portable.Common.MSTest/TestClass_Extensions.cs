
using IVSoftware.WinOS.MSTest.Extensions;

namespace IVSoftware.Portable.Common.MSTest;

[TestClass]
public class TestClass_Extensions
{
    [TestMethod]
    public void Test_ToFriendClass()
    {
        string actual, expected;

        actual = typeof(Newtonsoft.Json.Formatting).ToStrongNamedFriendAssembly();
        actual.ToClipboardExpected();
        { }
        expected = @" 
Policy Violation: Cannot make a friend class designation for an instance of Type."
        ;

        Assert.AreEqual(
            expected.NormalizeResult(),
            actual.NormalizeResult(),
            "Expecting reported inadvertent use of `typeof`."
        );

        actual = Newtonsoft.Json.Formatting.Indented.ToStrongNamedFriendAssembly();
        actual.ToClipboardExpected();
        { }
        expected = @" 
[assembly: InternalsVisibleTo(""Newtonsoft.Json, PublicKey=0024000004800000940000000602000000240000525341310004000001000100f561df277c6c0b497d629032b410cdcf286e537c054724f7ffa0164345f62b3e642029d7a80cc351918955328c4adc8a048823ef90b0cf38ea7db0d729caf2b633c3babe08b0310198c1081995c19029bc675193744eab9d7345b8a67258ec17d112cebdbbb2a281487dceeafb9d83aa930f32103fbe1d2911425bc5744002c7"")]"
        ;

        Assert.AreEqual(
            expected.NormalizeResult(),
            actual.NormalizeResult(),
            "Expecting reported inadvertent use of `typeof`."
        );
    }
}
