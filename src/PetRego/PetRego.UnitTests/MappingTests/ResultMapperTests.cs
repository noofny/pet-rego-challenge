using PetRego.AppHost;
using PetRego.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;

namespace PetRego.UnitTests.MappingTests
{
    [TestClass]
    public class ResultMapperTests
    {
        [TestInitialize]
        public void Setup()
        {
            AutomapperConfig.Configure();
        }



        [TestMethod]
        public void Can_Map_Created()
        {
            var result = Nest.Result.Created;
            var statusCode = Mapper.Map<Result>(result);
            Assert.AreEqual(Result.Created, statusCode);
        }

        [TestMethod]
        public void Can_Map_Deleted()
        {
            var result = Nest.Result.Deleted;
            var statusCode = Mapper.Map<Result>(result);
            Assert.AreEqual(Result.Deleted, statusCode);
        }

        [TestMethod]
        public void Can_Map_Error()
        {
            var result = Nest.Result.Error;
            var statusCode = Mapper.Map<Result>(result);
            Assert.AreEqual(Result.InternalError, statusCode);
        }

        [TestMethod]
        public void Can_Map_NotFound()
        {
            var result = Nest.Result.NotFound;
            var statusCode = Mapper.Map<Result>(result);
            Assert.AreEqual(Result.NotFound, statusCode);
        }

        [TestMethod]
        public void Can_Map_Updated()
        {
            var result = Nest.Result.Updated;
            var statusCode = Mapper.Map<Result>(result);
            Assert.AreEqual(Result.Updated, statusCode);
        }

        [TestMethod]
        public void Can_Map_Noop()
        {
            var result = Nest.Result.Noop;
            var statusCode = Mapper.Map<Result>(result);
            Assert.AreEqual(Result.Unsupported, statusCode);
        }


    }
}
