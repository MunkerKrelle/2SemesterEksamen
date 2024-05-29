using ComponentPattern;
using RepositoryPattern;

namespace TestProjectV6
{
    [TestClass]
    public class UnitTest1
    {
        static readonly IRepository? repository;
        Database database = new Database();
        Database repDatabase = new Database(repository);

        [TestMethod]
        public void TestNoReturnMethods()
        {
            database.CreateDatabase();
            database.ShowBestiary();
            database.ShowBestiaryInfo();
        }

        [TestMethod]
        public void TestTradeWeaponWithEnoughScraps()
        {
            //Arange
            bool expectedScrapAmount = true;
            Weapon weapon = new Weapon(new GameObject(), "Bat", 3, 50);

            bool actualScrapAmount = database.TradeWeapon(weapon);

            Assert.AreEqual(expectedScrapAmount, actualScrapAmount);
        }

        [TestMethod]
        public void UnitTestTradeWeaponWithoutEnoughScraps()
        {
            bool expectedScrapAmount = false;
            Weapon weapon = new Weapon(new GameObject(), "Bat", 3, 2000);

            bool actualScrapAmount = database.TradeWeapon(weapon);

            Assert.AreEqual(expectedScrapAmount, actualScrapAmount);
        }

        [TestMethod]
        public void TestReturnValues()
        {
            string weaponName = "Butterflyknife";

            Tuple<string, int, int> expectedTuple = new Tuple<string, int, int>("Butterflyknife", 2, 10);

            var actualTuple = database.ReturnValues(weaponName);

            Assert.AreEqual(expectedTuple, actualTuple);
        }

        [TestMethod]
        public void TestReturnValuesID()
        {
            int ID = 3;

            Tuple<string, int, int> expectedTuple = new Tuple<string, int, int>("Katana", 10, 50);

            var actualTuple = database.ReturnValuesWithID(ID);

            Assert.AreEqual(expectedTuple, actualTuple);
        }

        [TestMethod]
        public void TestAddToInventory()
        {
            string expectedString = null;
            var actualstring =  database.AddToInventory();

            Assert.AreEqual(expectedString, actualstring);
        }

        [TestMethod]
        public void TestUpdateScraps()
        {
            int expectedInt = 950;
            var actualInt = database.UpdateScraps();

            Assert.AreEqual(expectedInt, actualInt);
        }
    }
}