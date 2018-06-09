using PetRego.AppHost;
using PetRego.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using System.Net;

namespace PetRego.UnitTests.MappingTests
{
    [TestClass]
    public class HttpStatusCodeMapperTests
    {
        [TestInitialize]
        public void Setup()
        {
            AutomapperConfig.Configure();
        }



        [TestMethod]
        public void Can_Map_BadRequet()
        {
            var result = Result.BadRequest;
            var statusCode = Mapper.Map<HttpStatusCode>(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, statusCode);
        }

        [TestMethod]
        public void Can_Map_Created()
        {
            var result = Result.Created;
            var statusCode = Mapper.Map<HttpStatusCode>(result);
            Assert.AreEqual(HttpStatusCode.Created, statusCode);
        }

        [TestMethod]
        public void Can_Map_Deleted()
        {
            var result = Result.Deleted;
            var statusCode = Mapper.Map<HttpStatusCode>(result);
            Assert.AreEqual(HttpStatusCode.Accepted, statusCode);
        }

        [TestMethod]
        public void Can_Map_InternalError()
        {
            var result = Result.InternalError;
            var statusCode = Mapper.Map<HttpStatusCode>(result);
            Assert.AreEqual(HttpStatusCode.InternalServerError, statusCode);
        }

        [TestMethod]
        public void Can_Map_NotFound()
        {
            var result = Result.NotFound;
            var statusCode = Mapper.Map<HttpStatusCode>(result);
            Assert.AreEqual(HttpStatusCode.NotFound, statusCode);
        }

        [TestMethod]
        public void Can_Map_Success()
        {
            var result = Result.Success;
            var statusCode = Mapper.Map<HttpStatusCode>(result);
            Assert.AreEqual(HttpStatusCode.OK, statusCode);
        }

        [TestMethod]
        public void Can_Map_Unsupported()
        {
            var result = Result.Unsupported;
            var statusCode = Mapper.Map<HttpStatusCode>(result);
            Assert.AreEqual(HttpStatusCode.UnsupportedMediaType, statusCode);
        }

        [TestMethod]
        public void Can_Map_Updated()
        {
            var result = Result.Updated;
            var statusCode = Mapper.Map<HttpStatusCode>(result);
            Assert.AreEqual(HttpStatusCode.Accepted, statusCode);
        }




    }
}
