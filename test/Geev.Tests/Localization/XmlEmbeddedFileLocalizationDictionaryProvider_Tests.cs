﻿using System.Linq;
using System.Reflection;
using Geev.Localization.Dictionaries.Xml;
using Geev.Reflection.Extensions;
using Shouldly;
using Xunit;

namespace Geev.Tests.Localization
{
    public class XmlEmbeddedFileLocalizationDictionaryProvider_Tests
    {
        private readonly XmlEmbeddedFileLocalizationDictionaryProvider _dictionaryProvider;

        public XmlEmbeddedFileLocalizationDictionaryProvider_Tests()
        {
            _dictionaryProvider = new XmlEmbeddedFileLocalizationDictionaryProvider(
                typeof(XmlEmbeddedFileLocalizationDictionaryProvider_Tests).GetAssembly(),
                "Geev.Tests.Localization.TestXmlFiles"
                );

            _dictionaryProvider.Initialize("Test");
        }

        [Fact]
        public void Should_Get_Dictionaries()
        {
            var dictionaries = _dictionaryProvider.Dictionaries.Values.ToList();
            
            dictionaries.Count.ShouldBe(2);

            var enDict = dictionaries.FirstOrDefault(d => d.CultureInfo.Name == "en");
            enDict.ShouldNotBe(null);
            enDict.ShouldBe(_dictionaryProvider.DefaultDictionary);
            enDict["hello"].ShouldBe("Hello");
            
            var trDict = dictionaries.FirstOrDefault(d => d.CultureInfo.Name == "tr");
            trDict.ShouldNotBe(null);
            trDict["hello"].ShouldBe("Merhaba");
        }
    }
}
