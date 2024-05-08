using ComponentPattern;
using Npgsql;
using System;

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
        private bool buy, sell;
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
    }
}
