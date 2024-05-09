using ComponentPattern;
using Npgsql;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace RepositoryPattern
{
    public class UserRegistrationWithPattern
    {
        private readonly IRepository repository;
        private NpgsqlDataSource dataSource;
        private string connectionString = "Host=localhost;Username=postgres;Password=100899;Database=postgres";

        private string charName, weaponName;
        private int health, scrapAmount, damage, price, scrapDropped;
        private float speed;
        private bool buy, sell, enemyKilled;
        public UserRegistrationWithPattern(IRepository repository)
        {
            this.repository = repository;
        }

        public void RunLoop()
        {
            dataSource = NpgsqlDataSource.Create(connectionString);

            DropTables();
            CreateTables();
        }

        private void CreateTables()
        {
            NpgsqlCommand cmdCreatePlayerTable = dataSource.CreateCommand(@"
                CREATE TABLE IF NOT EXISTS player (
                    name VARCHAR(255) PRIMARY KEY,
                    health INT NOT NULL,
                    speed FLOAT NOT NULL,
                    scrap_amount int
                );");

            NpgsqlCommand cmdCreateInventoryTable = dataSource.CreateCommand(@"
                CREATE TABLE IF NOT EXISTS inventory (
                    item_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                    weapon_name VARCHAR(255),
                    damage INT NOT NULL
                );");

            NpgsqlCommand cmdCreateWeaponTable = dataSource.CreateCommand(@"
                CREATE TABLE IF NOT EXISTS weapon (
                    weapon_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                    name VARCHAR(255) NOT NULL UNIQUE,
                    damage INT NOT NULL,
                    price INT NOT NULL
                );");

            NpgsqlCommand cmdCreateEnemyTable = dataSource.CreateCommand(@"
                CREATE TABLE IF NOT EXISTS enemy (
                    enemy_id INT GENERATED ALWAS AS IDENTITY PRIMARY KEY,
                    health INT NOT NULL,
                    damage INT NOT NULL,
                    speed FLOAT NOT NULL,
                    scrap_dropped INT NOT NULL
                );");

            NpgsqlCommand cmdCreateArmsDealerTable = dataSource.CreateCommand(@"
                CREATE TABLE IF NOT EXISTS arms_dealer (
                    name VARCHAR(255) PRIMARY KEY
                );");

            NpgsqlCommand cmdCreateHasTable = dataSource.CreateCommand(@"
                CREATE TABLE IF NOT EXISTS has (
                    name VARCHAR(255) REFERENCES player(name),
                    weapon_ID INT REFERENCES weapon(weapon_id)
                );");

            NpgsqlCommand cmdCreateTradesTable = dataSource.CreateCommand(@"
                CREATE TABLE IF NOT EXISTS trades (
                    name VARCHAR(255) REFERENCES arms_dealer(name),
                    weapon_ID INT REFERENCES weapon(weapon_id)
                );");

            cmdCreatePlayerTable.ExecuteNonQuery();
            cmdCreateWeaponTable.ExecuteNonQuery();
            cmdCreateEnemyTable.ExecuteNonQuery();
            cmdCreateArmsDealerTable.ExecuteNonQuery();
            cmdCreateInventoryTable.ExecuteNonQuery();
            cmdCreateHasTable.ExecuteNonQuery();
            cmdCreateTradesTable.ExecuteNonQuery();
        }

        private void DropTables()
        {
            try
            {
                NpgsqlCommand cmdDropTables = dataSource.CreateCommand(@"
            DROP TABLE has;
            DROP TABLE trades;
            DROP TABLE player;
            DROP TABLE enemy;
            DROP TABLE arms_dealer;
            DROP TABLE weapon;
            DROP TABLE inventory;
            ");
                cmdDropTables.ExecuteNonQuery();
            }

            catch (Exception)
            {
            }
        }

        private void Insert()
        //VALUES SKAL VÆRE PLAYER/ENEMY.X
        {
            NpgsqlCommand cmdInsertPlayerValues = dataSource.CreateCommand($@"
        INSERT INTO player (name, health, speed, scrap_amount)

        VALUES('{charName}', {health}, {speed}, {scrapAmount})
        ");

            NpgsqlCommand cmdEnemyPlayerValues = dataSource.CreateCommand($@"
        INSERT INTO player (health, damage, speed, scrap_dropped)

        VALUES('{health}', {damage}, {speed}, {scrapDropped})
        ");

            NpgsqlCommand cmdInsertWeaponValues = dataSource.CreateCommand($@"
        INSERT INTO player (name, damage, price)

        VALUES('Wrench', 2, 10),
              ('Steel Bat', 5, 20),
              ('Katana', 10, 50),
              ('Lightsaber', 25, 100)
        ");

            cmdInsertPlayerValues.ExecuteNonQuery();
            cmdEnemyPlayerValues.ExecuteNonQuery();
            cmdInsertWeaponValues.ExecuteNonQuery();
        }

        private void TradeWeapon()
        {
            if (buy)
            {
                NpgsqlCommand cmdBuyWeapon = dataSource.CreateCommand($@"
        INSERT INTO inventory (weapon_name, damage)

        VALUES('{weaponName}', {damage})
        ");

                NpgsqlCommand cmdDeleteFromTable = dataSource.CreateCommand($@"
        DELETE FROM weapon
        WHERE name = '{weaponName}'
        ");

                NpgsqlCommand cmdUpdateScrapAmount = dataSource.CreateCommand($@"
        UPDATE player
        SET scrap_amount = scrap_amount - {price}
        ");

                cmdBuyWeapon.ExecuteNonQuery();
                cmdDeleteFromTable.ExecuteNonQuery();
                cmdUpdateScrapAmount.ExecuteNonQuery();
            }
            else if (sell)
            {
                NpgsqlCommand cmdSellWeapon = dataSource.CreateCommand($@"
        INSERT INTO weapon (name, damage, price)

        VALUES('{weaponName}', {damage}, {price})
        ");

                NpgsqlCommand cmdDeleteFromTable = dataSource.CreateCommand($@"
        DELETE FROM inventory
        WHERE weapon_name = '{weaponName}'
        ");

                NpgsqlCommand cmdUpdateScrapAmount = dataSource.CreateCommand($@"
        UPDATE player
        SET scrap_amount = scrap_amount + {price}
        ");

                cmdSellWeapon.ExecuteNonQuery();
                cmdDeleteFromTable.ExecuteNonQuery();
                cmdUpdateScrapAmount.ExecuteNonQuery();
            }
        }

        private void CollectScrap()
        {
            if (enemyKilled)
            {
                NpgsqlCommand cmdCollecScrap = dataSource.CreateCommand($@"
            UPDATE player
            SET scrap_amount = scrap_amount + {scrapDropped}
            ");

                cmdCollecScrap.ExecuteNonQuery();
            }
        }

        //private void TradeWeaponTest()
        //{
        //    Console.WriteLine("Buy or sell?");
        //    answer = Console.ReadLine();

        //    //BUY
        //    if (answer.ToLower() == "buy")
        //    {
        //        Console.WriteLine("Which weapon do you want to buy?");
        //        weaponName = Console.ReadLine();


        //        //BUY KATANA
        //        if (weaponName == "katana")
        //        {
        //            NpgsqlCommand cmdBuyWeapon = dataSource.CreateCommand($@"
        //INSERT INTO inventory (weapon_name, damage)

        //VALUES('{katana.Name}', {katana.Damage})
        //");
        //            NpgsqlCommand cmdDeleteFromTable = dataSource.CreateCommand($@"
        //DELETE FROM weapon
        //WHERE name = '{katana.Name}'
        //");

        //            NpgsqlCommand cmdUpdateScrapAmount = dataSource.CreateCommand($@"
        //UPDATE player
        //SET scrap_amount = scrap_amount - {katana.Price}
        //");

        //            cmdBuyWeapon.ExecuteNonQuery();
        //            cmdDeleteFromTable.ExecuteNonQuery();
        //            cmdUpdateScrapAmount.ExecuteNonQuery();

        //            Console.WriteLine("Katana has been bought");
        //        }

        //        //BUY WRENCH
        //        else if (weaponName == "wrench")
        //        {
        //            NpgsqlCommand cmdBuyWeapon = dataSource.CreateCommand($@"
        //INSERT INTO inventory (weapon_name, damage)

        //VALUES('{wrench.Name}', {wrench.Damage})
        //");
        //            NpgsqlCommand cmdDeleteFromTable = dataSource.CreateCommand($@"
        //DELETE FROM weapon
        //WHERE name = '{wrench.Name}'
        //");

        //            NpgsqlCommand cmdUpdateScrapAmount = dataSource.CreateCommand($@"
        //UPDATE player
        //SET scrap_amount = scrap_amount - {wrench.Price}
        //");

        //            cmdBuyWeapon.ExecuteNonQuery();
        //            cmdDeleteFromTable.ExecuteNonQuery();
        //            cmdUpdateScrapAmount.ExecuteNonQuery();

        //            Console.WriteLine("Wrench has been bought");
        //        }

        //    }

        //    //SELL
        //    else if (answer.ToLower() == "sell")
        //    {
        //        Console.WriteLine("Which weapon do you want to sell?");
        //        weaponName = Console.ReadLine();


        //        //SELL KATANA
        //        if (weaponName == "katana")
        //        {
        //            NpgsqlCommand cmdSellWeapon = dataSource.CreateCommand($@"
        //INSERT INTO weapon (name, damage, price)

        //VALUES('{katana.Name}', {katana.Damage}, {katana.Price})
        //");
        //            NpgsqlCommand cmdDeleteFromTable = dataSource.CreateCommand($@"
        //DELETE FROM inventory
        //WHERE weapon_name = '{katana.Name}'
        //");

        //            NpgsqlCommand cmdUpdateScrapAmount = dataSource.CreateCommand($@"
        //UPDATE player
        //SET scrap_amount = scrap_amount + {katana.Price}
        //");

        //            cmdSellWeapon.ExecuteNonQuery();
        //            cmdDeleteFromTable.ExecuteNonQuery();
        //            cmdUpdateScrapAmount.ExecuteNonQuery();

        //            Console.WriteLine("Katana has been sold");
        //        }

        //        //SELL WRENCH
        //        else if (weaponName == "wrench")
        //        {
        //            NpgsqlCommand cmdSellWeapon = dataSource.CreateCommand($@"
        //INSERT INTO weapon (name, damage)

        //VALUES('{wrench.Name}', {wrench.Damage})
        //");
        //            NpgsqlCommand cmdDeleteFromTable = dataSource.CreateCommand($@"
        //DELETE FROM inventory
        //WHERE weapon_name = '{wrench.Name}'
        //");

        //            NpgsqlCommand cmdUpdateScrapAmount = dataSource.CreateCommand($@"
        //UPDATE player
        //SET scrap_amount = scrap_amount + {wrench.Price}
        //");

        //            cmdSellWeapon.ExecuteNonQuery();
        //            cmdDeleteFromTable.ExecuteNonQuery();
        //            cmdUpdateScrapAmount.ExecuteNonQuery();

        //            Console.WriteLine("Wrench has been sold");
        //        }
        //    }
        //}

        //VIRKER IKKE
        private void SortTables()
        {
            NpgsqlCommand cmdSortInventoryTable = dataSource.CreateCommand($@"
        SELECT * 

        FROM inventory

        ORDER BY damage ASC
        ");

            NpgsqlCommand cmdSortWeaponTable = dataSource.CreateCommand($@"
        SELECT * 

        FROM weapon

        ORDER BY price ASC
        ");

            cmdSortInventoryTable.ExecuteNonQuery();
            cmdSortWeaponTable.ExecuteNonQuery();

            Console.WriteLine("You've been sorted mate");
        }
        //private void InsertTest()
        ////VALUES SKAL VÆRE PLAYER/ENEMY/WEAPON.X
        //{
        //    NpgsqlCommand cmdInsertPlayerValues = dataSource.CreateCommand($@"
        //INSERT INTO player (name, health, speed, scrap_amount)

        //VALUES('{lars.Name}', {lars.Health}, {lars.Speed}, {lars.ScrapAmount})
        //");

        //    NpgsqlCommand cmdEnemyEnemyValues = dataSource.CreateCommand($@"
        //INSERT INTO enemy (health, damage, speed, scrap_dropped)

        //VALUES('{enemy.Health}', {enemy.Damage}, {enemy.Speed}, {enemy.ScrapDropped})
        //");

        //    NpgsqlCommand cmdInsertWeaponValues = dataSource.CreateCommand($@"
        //INSERT INTO weapon (name, damage, price)

        //VALUES('{wrench.Name}', {wrench.Damage}, {wrench.Price}),
        //      ('Steel Bat', 5, 20),
        //      ('{katana.Name}', {katana.Damage}, {katana.Price}),
        //      ('Lightsaber', 25, 100)
        //");

        //    cmdInsertPlayerValues.ExecuteNonQuery();
        //    cmdEnemyEnemyValues.ExecuteNonQuery();
        //    cmdInsertWeaponValues.ExecuteNonQuery();

        //    Console.WriteLine("Values Inserted. Press enter to exit");
        //    Console.ReadKey();
        //}

    }
}
