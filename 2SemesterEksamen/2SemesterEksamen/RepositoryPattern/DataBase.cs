using ComponentPattern;
using Npgsql;
using System;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace RepositoryPattern
{
    public class UserRegistrationWithPattern
    {
        private readonly IRepository repository;
        private NpgsqlDataSource dataSource;
        private string connectionString = "Host=localhost;Username=postgres;Password=100899;Database=postgres";

        private string charName, weaponName;
        private int health, scrapAmount, damage, price, scrapDropped, defeated;
        private float speed;
        private bool buy, sell, enemyKilled;

        public UserRegistrationWithPattern()
        {

        }
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

        public void CreateTables()
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
                    price INT NOT NULL
                );");

            NpgsqlCommand cmdCreateWeaponTable = dataSource.CreateCommand(@"
                CREATE TABLE IF NOT EXISTS weapon (
                    weapon_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                    name VARCHAR(255) NOT NULL UNIQUE,
                    damage INT NOT NULL,
                    price INT NOT NULL
                );");

            NpgsqlCommand cmdCreateBestiaryTable = dataSource.CreateCommand(@"
                CREATE TABLE IF NOT EXISTS bestiary (
                    enemy_id INT GENERATED ALWAS AS IDENTITY PRIMARY KEY,
                    name VARCHAR(255) NOT NULL UNIQUE,
                    health INT NOT NULL,
                    damage INT NOT NULL,
                    speed FLOAT NOT NULL,
                    strengths VARCHAR(255) NOT NULL UNIQUE,
                    weaknesses VARCHAR(255) NOT NULL UNIQUE,
                    scrap_dropped INT NOT NULL,
                    defeated INT NOT NULL
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
            cmdCreateBestiaryTable.ExecuteNonQuery();
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
            DROP TABLE bestiary;
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

        public void Insert()
        //VALUES SKAL VÆRE PLAYER/ENEMY.X
        {
            NpgsqlCommand cmdInsertPlayerValues = dataSource.CreateCommand($@"
        INSERT INTO player (name, health, speed, scrap_amount)

        VALUES('{charName}', {health}, {speed}, {scrapAmount})
        ");

            NpgsqlCommand cmdInsertWeaponValues = dataSource.CreateCommand($@"
        INSERT INTO weapon (name, damage, price)

        VALUES('Wrench', 2, 10),
              ('Steel Bat', 5, 20),
              ('Katana', 10, 50),
              ('Lightsaber', 25, 100)
        ");

            NpgsqlCommand cmdInsertBestiaryValues = dataSource.CreateCommand($@"
        INSERT INTO bestiary (name, health, damage, speed, strengths, weaknesses, scrap_dropped, defeated)

        VALUES('Drone', 5, 1, 1,'none', 'everything', 1, 0),
              ('Android', 10, 2, 2, 'none', 'melee', 2, 0),
              ('Sentinel', 25, 5, 4, 'ranged', 'melee', 5, 0),
              ('Enforcer', 100, 25, 1, 'close combat', 'ranged weapons', 20, 0),
              ('Cyborg', 75, 50, 10, 'bio regeneration', 'emp grenades', 50, 0)
        ");


            cmdInsertPlayerValues.ExecuteNonQuery();
            cmdInsertWeaponValues.ExecuteNonQuery();
            cmdInsertBestiaryValues.ExecuteNonQuery();
        }

        public List<string> ReturnValues(string weaponName)
        {
            NpgsqlCommand cmd = dataSource.CreateCommand($"SELECT name, damage, price FROM weapon" +
                                                         $"WHERE name = {weaponName}");
            NpgsqlDataReader reader = cmd.ExecuteReader();
            List<string> values = new List<string>();

            while (reader.Read())
            {
                string name = (reader.GetValue(0).ToString());
                string damage = (reader.GetValue(1).ToString());
                string price = (reader.GetValue(2).ToString());

                values.Add(name);
                values.Add(damage);
                values.Add(price);
            }

            return values;
        }

        private void TradeWeapon()
        {
            if (buy)
            {
                NpgsqlCommand cmdBuyWeapon = dataSource.CreateCommand($@"
        INSERT INTO inventory (weapon_name, damage, price)

        VALUES('{weaponName}', {damage}, {price})
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
                NpgsqlCommand cmdCollectScrap = dataSource.CreateCommand($@"
            UPDATE player
            SET scrap_amount = scrap_amount + {scrapDropped}
            ");

                cmdCollectScrap.ExecuteNonQuery();
            }
        }

        //NÅR EN FEJENDE ER BESEJRET_________________________________________________________________________________
        //private void EnemyDefeated()
        //{
        //    NpgsqlCommand cmdEnemyDefeated = dataSource.CreateCommand($@"
        //    UPDATE bestiary
        //    SET defeated = defeated + 1
        //    WHERE name = {ENEMYNAME}
        //    ");

        //    cmdEnemyDefeated.ExecuteNonQuery();
        //}


        //BYT VÅBEN MED VAULES_______________________________________________________________________________________
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

        //VIRKER IKKE SORTER_______________________________________________________________________________________
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

        //INSERT MED VALUES_______________________________________________________________________________________
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

        //VIS OVERSIGT OVER FEJNDER_______________________________________________________________________________________
        private void ShowBestiary()
        {
            NpgsqlCommand cmd = dataSource.CreateCommand($"SELECT name, health, damage, speed, strengths, weaknesses, scrap_dropped, defeated FROM bestiary");
            NpgsqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine($"Name: {reader.GetValue(0)}, Health: {reader.GetValue(1)}, Damage: {reader.GetValue(2)}, " +
                    $"Speed: {reader.GetValue(3)}, Strengths: {reader.GetValue(4)}, Weaknesses: {reader.GetValue(5)}, " +
                    $"Scrap Dropped: {reader.GetValue(6)}, Defeated: {reader.GetValue(7)}");
            }
        }
    }
}
