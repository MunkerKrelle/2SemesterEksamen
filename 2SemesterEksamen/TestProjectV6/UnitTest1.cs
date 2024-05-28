using _2SemesterEksamen;
using ArmsDealer;
using Inventory;
using Weapon;
using DataBase;

namespace TestProjectV6
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void RunInventory()
        {
            //Arange
            DataBase dataBase = new DataBase();

            bool expectedScrapAmount = false;

            bool actualScrapAmount = dataBase.TradeWeapon();

            Assert.AreEqual(actualScrapAmount, expectedScrapAmount);
        }
    }
}