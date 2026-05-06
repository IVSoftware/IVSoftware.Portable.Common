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
            
            foreach (var asmName in typeof(TestClass_TypeCache)
            .Assembly
            .GetReferencedAssemblies())
            {
                try
                {
                    AppDomain.CurrentDomain.Load(asmName);
                }
                catch
                {
                    // ignore load failures (not all are loadable)
                }
            }

            var types = TypeCache.Values.Where(_=>_.FullName?.StartsWith("IVSoftware") == true || _.FullName?.StartsWith("Newtonsoft") == true);

            string normalizeTypes() => 
                string.Join(
                    Environment.NewLine,
                    types
                        .Select(t => t.FullName)
                        .Where(x => x is not null)
                        .OrderBy(x => x)
                );
            
            actual = normalizeTypes();

            actual.ToClipboardExpected();
            { }
            expected = @" 
IVSoftware.Portable.Common.Attributes.CanonicalAttribute
IVSoftware.Portable.Common.Attributes.CarefulAttribute
IVSoftware.Portable.Common.Attributes.ClaimAttribute
IVSoftware.Portable.Common.Attributes.IndexerAttribute
IVSoftware.Portable.Common.Attributes.NotFlagsAttribute
IVSoftware.Portable.Common.Attributes.PolicyAttribute
IVSoftware.Portable.Common.Attributes.PolicyEnforcementAttribute
IVSoftware.Portable.Common.Attributes.ProbationaryAttribute
IVSoftware.Portable.Common.Attributes.PublishedContractAttribute
IVSoftware.Portable.Common.Attributes.ScaffoldingAttribute
IVSoftware.Portable.Common.Attributes.UnsupportedAttribute
IVSoftware.Portable.Common.Common
IVSoftware.Portable.Common.Exceptions.Advisory
IVSoftware.Portable.Common.Exceptions.Throw
IVSoftware.Portable.Common.Exceptions.ThrowableStatus
IVSoftware.Portable.Common.Exceptions.ThrowExtensions
IVSoftware.Portable.Common.Exceptions.ThrowOrAdvise
IVSoftware.Portable.Common.Exceptions.ThrowToStringFormat
IVSoftware.Portable.Common.Exceptions.Win32Message
IVSoftware.Portable.Common.Extensions
IVSoftware.Portable.Common.ITypeCache
IVSoftware.Portable.Common.MSTest.TestClass_Extensions
IVSoftware.Portable.Common.MSTest.TestClass_Throw
IVSoftware.Portable.Common.MSTest.TestClass_TypeCache
IVSoftware.Portable.Common.TypeCacheExtensions
IVSoftware.Portable.Common.TypeCacheMatchMode
IVSoftware.Portable.Disposable.AutoObservableCollection`1
IVSoftware.Portable.Disposable.BeginUsingEventArgs
IVSoftware.Portable.Disposable.BeginUsingEventHandler
IVSoftware.Portable.Disposable.Clients
IVSoftware.Portable.Disposable.CollectionChangedBatchEventHandler
IVSoftware.Portable.Disposable.CountChangedAction
IVSoftware.Portable.Disposable.CountChangedEventArgs
IVSoftware.Portable.Disposable.CountChangedEventHandler
IVSoftware.Portable.Disposable.DHostExtensions
IVSoftware.Portable.Disposable.DisposableHost
IVSoftware.Portable.Disposable.DisposableHost+DisposableToken
IVSoftware.Portable.Disposable.FinalDisposeEventArgs
IVSoftware.Portable.Disposable.FinalDisposeEventHandler
IVSoftware.Portable.Disposable.IVisibleIndex
IVSoftware.Portable.Disposable.NotifyCollectionChangedBatchEventArgs
IVSoftware.Portable.Disposable.NotifyCollectionResetEventArgs
IVSoftware.Portable.Disposable.ObservableMoveCollection`1
IVSoftware.Portable.Disposable.TokenDisposedEventArgs
IVSoftware.WinOS.MSTest.Extensions.Extensions"
            ;
            Assert.AreEqual(
                expected.NormalizeResult(),
                actual.NormalizeResult(),
                "Expecting json serialization to match."
            );

            "Newtonsoft".AppendNamespaceToCache();

            actual = normalizeTypes();
            actual.ToClipboardExpected();
            { }
            expected = @" 
IVSoftware.Portable.Common.Attributes.CanonicalAttribute
IVSoftware.Portable.Common.Attributes.CarefulAttribute
IVSoftware.Portable.Common.Attributes.ClaimAttribute
IVSoftware.Portable.Common.Attributes.IndexerAttribute
IVSoftware.Portable.Common.Attributes.NotFlagsAttribute
IVSoftware.Portable.Common.Attributes.PolicyAttribute
IVSoftware.Portable.Common.Attributes.PolicyEnforcementAttribute
IVSoftware.Portable.Common.Attributes.ProbationaryAttribute
IVSoftware.Portable.Common.Attributes.PublishedContractAttribute
IVSoftware.Portable.Common.Attributes.ScaffoldingAttribute
IVSoftware.Portable.Common.Attributes.UnsupportedAttribute
IVSoftware.Portable.Common.Common
IVSoftware.Portable.Common.Exceptions.Advisory
IVSoftware.Portable.Common.Exceptions.Throw
IVSoftware.Portable.Common.Exceptions.ThrowableStatus
IVSoftware.Portable.Common.Exceptions.ThrowExtensions
IVSoftware.Portable.Common.Exceptions.ThrowOrAdvise
IVSoftware.Portable.Common.Exceptions.ThrowToStringFormat
IVSoftware.Portable.Common.Exceptions.Win32Message
IVSoftware.Portable.Common.Extensions
IVSoftware.Portable.Common.ITypeCache
IVSoftware.Portable.Common.MSTest.TestClass_Extensions
IVSoftware.Portable.Common.MSTest.TestClass_Throw
IVSoftware.Portable.Common.MSTest.TestClass_TypeCache
IVSoftware.Portable.Common.TypeCacheExtensions
IVSoftware.Portable.Common.TypeCacheMatchMode
IVSoftware.Portable.Disposable.AutoObservableCollection`1
IVSoftware.Portable.Disposable.BeginUsingEventArgs
IVSoftware.Portable.Disposable.BeginUsingEventHandler
IVSoftware.Portable.Disposable.Clients
IVSoftware.Portable.Disposable.CollectionChangedBatchEventHandler
IVSoftware.Portable.Disposable.CountChangedAction
IVSoftware.Portable.Disposable.CountChangedEventArgs
IVSoftware.Portable.Disposable.CountChangedEventHandler
IVSoftware.Portable.Disposable.DHostExtensions
IVSoftware.Portable.Disposable.DisposableHost
IVSoftware.Portable.Disposable.DisposableHost+DisposableToken
IVSoftware.Portable.Disposable.FinalDisposeEventArgs
IVSoftware.Portable.Disposable.FinalDisposeEventHandler
IVSoftware.Portable.Disposable.IVisibleIndex
IVSoftware.Portable.Disposable.NotifyCollectionChangedBatchEventArgs
IVSoftware.Portable.Disposable.NotifyCollectionResetEventArgs
IVSoftware.Portable.Disposable.ObservableMoveCollection`1
IVSoftware.Portable.Disposable.TokenDisposedEventArgs
IVSoftware.WinOS.MSTest.Extensions.Extensions
Newtonsoft.Json.Bson.BsonObjectId
Newtonsoft.Json.Bson.BsonReader
Newtonsoft.Json.Bson.BsonWriter
Newtonsoft.Json.ConstructorHandling
Newtonsoft.Json.Converters.BinaryConverter
Newtonsoft.Json.Converters.BsonObjectIdConverter
Newtonsoft.Json.Converters.CustomCreationConverter`1
Newtonsoft.Json.Converters.DataSetConverter
Newtonsoft.Json.Converters.DataTableConverter
Newtonsoft.Json.Converters.DateTimeConverterBase
Newtonsoft.Json.Converters.DiscriminatedUnionConverter
Newtonsoft.Json.Converters.EntityKeyMemberConverter
Newtonsoft.Json.Converters.ExpandoObjectConverter
Newtonsoft.Json.Converters.IsoDateTimeConverter
Newtonsoft.Json.Converters.JavaScriptDateTimeConverter
Newtonsoft.Json.Converters.KeyValuePairConverter
Newtonsoft.Json.Converters.RegexConverter
Newtonsoft.Json.Converters.StringEnumConverter
Newtonsoft.Json.Converters.UnixDateTimeConverter
Newtonsoft.Json.Converters.VersionConverter
Newtonsoft.Json.Converters.XmlNodeConverter
Newtonsoft.Json.DateFormatHandling
Newtonsoft.Json.DateParseHandling
Newtonsoft.Json.DateTimeZoneHandling
Newtonsoft.Json.DefaultJsonNameTable
Newtonsoft.Json.DefaultValueHandling
Newtonsoft.Json.FloatFormatHandling
Newtonsoft.Json.FloatParseHandling
Newtonsoft.Json.Formatting
Newtonsoft.Json.IArrayPool`1
Newtonsoft.Json.IJsonLineInfo
Newtonsoft.Json.JsonArrayAttribute
Newtonsoft.Json.JsonConstructorAttribute
Newtonsoft.Json.JsonContainerAttribute
Newtonsoft.Json.JsonConvert
Newtonsoft.Json.JsonConverter
Newtonsoft.Json.JsonConverter`1
Newtonsoft.Json.JsonConverterAttribute
Newtonsoft.Json.JsonConverterCollection
Newtonsoft.Json.JsonDictionaryAttribute
Newtonsoft.Json.JsonException
Newtonsoft.Json.JsonExtensionDataAttribute
Newtonsoft.Json.JsonIgnoreAttribute
Newtonsoft.Json.JsonNameTable
Newtonsoft.Json.JsonObjectAttribute
Newtonsoft.Json.JsonPropertyAttribute
Newtonsoft.Json.JsonReader
Newtonsoft.Json.JsonReaderException
Newtonsoft.Json.JsonRequiredAttribute
Newtonsoft.Json.JsonSerializationException
Newtonsoft.Json.JsonSerializer
Newtonsoft.Json.JsonSerializerSettings
Newtonsoft.Json.JsonTextReader
Newtonsoft.Json.JsonTextWriter
Newtonsoft.Json.JsonToken
Newtonsoft.Json.JsonValidatingReader
Newtonsoft.Json.JsonWriter
Newtonsoft.Json.JsonWriterException
Newtonsoft.Json.Linq.CommentHandling
Newtonsoft.Json.Linq.DuplicatePropertyNameHandling
Newtonsoft.Json.Linq.Extensions
Newtonsoft.Json.Linq.IJEnumerable`1
Newtonsoft.Json.Linq.JArray
Newtonsoft.Json.Linq.JConstructor
Newtonsoft.Json.Linq.JContainer
Newtonsoft.Json.Linq.JEnumerable`1
Newtonsoft.Json.Linq.JObject
Newtonsoft.Json.Linq.JProperty
Newtonsoft.Json.Linq.JPropertyDescriptor
Newtonsoft.Json.Linq.JRaw
Newtonsoft.Json.Linq.JsonLoadSettings
Newtonsoft.Json.Linq.JsonMergeSettings
Newtonsoft.Json.Linq.JsonSelectSettings
Newtonsoft.Json.Linq.JToken
Newtonsoft.Json.Linq.JTokenEqualityComparer
Newtonsoft.Json.Linq.JTokenReader
Newtonsoft.Json.Linq.JTokenType
Newtonsoft.Json.Linq.JTokenWriter
Newtonsoft.Json.Linq.JValue
Newtonsoft.Json.Linq.LineInfoHandling
Newtonsoft.Json.Linq.MergeArrayHandling
Newtonsoft.Json.Linq.MergeNullValueHandling
Newtonsoft.Json.MemberSerialization
Newtonsoft.Json.MetadataPropertyHandling
Newtonsoft.Json.MissingMemberHandling
Newtonsoft.Json.NullValueHandling
Newtonsoft.Json.ObjectCreationHandling
Newtonsoft.Json.PreserveReferencesHandling
Newtonsoft.Json.ReferenceLoopHandling
Newtonsoft.Json.Required
Newtonsoft.Json.Schema.Extensions
Newtonsoft.Json.Schema.JsonSchema
Newtonsoft.Json.Schema.JsonSchemaException
Newtonsoft.Json.Schema.JsonSchemaGenerator
Newtonsoft.Json.Schema.JsonSchemaResolver
Newtonsoft.Json.Schema.JsonSchemaType
Newtonsoft.Json.Schema.UndefinedSchemaIdHandling
Newtonsoft.Json.Schema.ValidationEventArgs
Newtonsoft.Json.Schema.ValidationEventHandler
Newtonsoft.Json.Serialization.CamelCaseNamingStrategy
Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver
Newtonsoft.Json.Serialization.DefaultContractResolver
Newtonsoft.Json.Serialization.DefaultNamingStrategy
Newtonsoft.Json.Serialization.DefaultSerializationBinder
Newtonsoft.Json.Serialization.DiagnosticsTraceWriter
Newtonsoft.Json.Serialization.ErrorContext
Newtonsoft.Json.Serialization.ErrorEventArgs
Newtonsoft.Json.Serialization.ExpressionValueProvider
Newtonsoft.Json.Serialization.ExtensionDataGetter
Newtonsoft.Json.Serialization.ExtensionDataSetter
Newtonsoft.Json.Serialization.IAttributeProvider
Newtonsoft.Json.Serialization.IContractResolver
Newtonsoft.Json.Serialization.IReferenceResolver
Newtonsoft.Json.Serialization.ISerializationBinder
Newtonsoft.Json.Serialization.ITraceWriter
Newtonsoft.Json.Serialization.IValueProvider
Newtonsoft.Json.Serialization.JsonArrayContract
Newtonsoft.Json.Serialization.JsonContainerContract
Newtonsoft.Json.Serialization.JsonContract
Newtonsoft.Json.Serialization.JsonDictionaryContract
Newtonsoft.Json.Serialization.JsonDynamicContract
Newtonsoft.Json.Serialization.JsonISerializableContract
Newtonsoft.Json.Serialization.JsonLinqContract
Newtonsoft.Json.Serialization.JsonObjectContract
Newtonsoft.Json.Serialization.JsonPrimitiveContract
Newtonsoft.Json.Serialization.JsonProperty
Newtonsoft.Json.Serialization.JsonPropertyCollection
Newtonsoft.Json.Serialization.JsonStringContract
Newtonsoft.Json.Serialization.KebabCaseNamingStrategy
Newtonsoft.Json.Serialization.MemoryTraceWriter
Newtonsoft.Json.Serialization.NamingStrategy
Newtonsoft.Json.Serialization.ObjectConstructor`1
Newtonsoft.Json.Serialization.OnErrorAttribute
Newtonsoft.Json.Serialization.ReflectionAttributeProvider
Newtonsoft.Json.Serialization.ReflectionValueProvider
Newtonsoft.Json.Serialization.SerializationCallback
Newtonsoft.Json.Serialization.SerializationErrorCallback
Newtonsoft.Json.Serialization.SnakeCaseNamingStrategy
Newtonsoft.Json.StringEscapeHandling
Newtonsoft.Json.TypeNameAssemblyFormatHandling
Newtonsoft.Json.TypeNameHandling
Newtonsoft.Json.WriteState"
            ;
            Assert.AreEqual(
                expected.NormalizeResult(),
                actual.NormalizeResult(),
                "Expecting namespace appended."
            );
        }
    }
}
