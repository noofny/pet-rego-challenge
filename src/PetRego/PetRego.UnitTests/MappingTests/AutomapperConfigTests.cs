using PetRego.AppHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;

namespace PetRego.UnitTests.MappingTests
{
    [TestClass]
    public class AutomapperConfigTests
    {
        [TestInitialize]
        public void Setup()
        {
            AutomapperConfig.Configure();
        }


        [TestMethod]
        public void Mapping_Config_Is_Valid()
        {
            Mapper.Configuration.AssertConfigurationIsValid();
        }

    }
}
