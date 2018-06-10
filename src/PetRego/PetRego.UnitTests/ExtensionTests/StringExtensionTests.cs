using PetRego.AppHost;
using PetRego.Common;
using PetRego.Common.Extensions;
using PetRego.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;

namespace PetRego.UnitTests.ExtensionTests
{
    [TestClass]
    public class StringExtensionTests
    {
        
        [TestMethod]
        public void Can_Get_PetType_Cat()
        {
            var source = "Cat";
            var target = source.ToEnum<PetType>();
            Assert.AreEqual(PetType.Cat, target);
        }

        [TestMethod]
        public void Can_Get_PetType_Chicken()
        {
            var source = "Chicken";
            var target = source.ToEnum<PetType>();
            Assert.AreEqual(PetType.Chicken, target);
        }

        [TestMethod]
        public void Can_Get_PetType_Dog()
        {
            var source = "Dog";
            var target = source.ToEnum<PetType>();
            Assert.AreEqual(PetType.Dog, target);
        }

        [TestMethod]
        public void Can_Get_PetType_Snake()
        {
            var source = "Snake";
            var target = source.ToEnum<PetType>();
            Assert.AreEqual(PetType.Snake, target);
        }

        [TestMethod]
        public void Can_Get_PetType_Undefined()
        {
            var source = "Undefined";
            var target = source.ToEnum<PetType>();
            Assert.AreEqual(PetType.Undefined, target);
        }

        [TestMethod]
        public void Can_Get_PetType_GarbageInput()
        {
            var source = "sdjfkhdsjkfjhdsjkjkefjksdfhfjsdhfdfkjsjk";
            var target = source.ToEnum<PetType>();
            Assert.AreEqual(PetType.Undefined, target);
        }




    }
}
