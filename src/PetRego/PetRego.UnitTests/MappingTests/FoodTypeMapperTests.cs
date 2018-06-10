using PetRego.AppHost;
using PetRego.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;

namespace PetRego.UnitTests.MappingTests
{
    [TestClass]
    public class FoodTypeMapperTests
    {
        [TestInitialize]
        public void Setup()
        {
            AutomapperConfig.Configure();
        }



        [TestMethod]
        public void Can_Map_Fish()
        {
            var petType = PetType.Cat;
            var foodType = Mapper.Map<FoodType>(petType);
            Assert.AreEqual(FoodType.Fish, foodType);
        }

        [TestMethod]
        public void Can_Map_Corn()
        {
            var petType = PetType.Chicken;
            var foodType = Mapper.Map<FoodType>(petType);
            Assert.AreEqual(FoodType.Corn, foodType);
        }

        [TestMethod]
        public void Can_Map_Bones()
        {
            var petType = PetType.Dog;
            var foodType = Mapper.Map<FoodType>(petType);
            Assert.AreEqual(FoodType.Bones, foodType);
        }

        [TestMethod]
        public void Can_Map_Mice()
        {
            var petType = PetType.Snake;
            var foodType = Mapper.Map<FoodType>(petType);
            Assert.AreEqual(FoodType.Mice, foodType);
        }

    }
}
