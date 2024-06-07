using _2SemesterEksamen;
using ComponentPattern;
using Npgsql;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("TestProjectV6")]

namespace RepositoryPattern
{
    /// <summary>
    /// Database for sql funktioner
    /// </summary>
    public class Database : IRepository
    {
        private readonly IRepository repository;
        public static bool playerItemsUpdated = false;
        private NpgsqlDataSource dataSource;
        private string connectionString = "Host=localhost;Username=postgres;Password=Saunire.124;Database=myDatabase";

        //Nogle af disse er ikke blevet brugt endnu
        private string weaponName;
        private int scrapAmount;


        public Database()
        {
        }

        public Database(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Reseter og indlæser alt dataen og tabellerne
        /// </summary>
        public void CreateDatabase()
        {
            dataSource = NpgsqlDataSource.Create(connectionString);

            DropTables();
            CreateTables();
            Insert();
        }

        /// <summary>
        /// Opretter alle tabellerne for spillet
        /// </summary>
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
                    damage INT NOT NULL,
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
                    enemy_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                    name VARCHAR(255) NOT NULL UNIQUE,
                    health INT NOT NULL,
                    damage INT NOT NULL,
                    speed FLOAT NOT NULL,
                    strengths VARCHAR(255) NOT NULL,
                    weaknesses VARCHAR(255) NOT NULL,
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

        /// <summary>
        /// Removes the tables, so we can reset them for a new game
        /// </summary>
        public void DropTables()
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

        /// <summary>
        /// Indsæt alt data ind i de forskellige tabeller
        /// </summary>
        public void Insert()
        //VALUES SKAL VÆRE PLAYER/ENEMY.X

        {
            NpgsqlCommand cmdInsertPlayerValues = dataSource.CreateCommand($@"
        INSERT INTO player (name, health, speed, scrap_amount)

        VALUES('TestPlayer', 100, 50, 1000)
        ");

            NpgsqlCommand cmdInsertWeaponValues = dataSource.CreateCommand($@"
        INSERT INTO weapon (name, damage, price)

        VALUES('Butterflyknife', 2, 10),
              ('Bat', 5, 20),
              ('Katana', 10, 50),
              ('Chainsword', 25, 100),
              ('Hammer', 8, 25),
              ('Crimsonblade', 50, 300),
              ('Nunchaku', 30, 150),
              ('Annihilator', 100, 500)

        ");

            NpgsqlCommand cmdInsertBestiaryValues = dataSource.CreateCommand($@"
        INSERT INTO bestiary (name, health, damage, speed, strengths, weaknesses, scrap_dropped, defeated)

        VALUES('Drone', 5, 1, 1,'none', 'everything', 1, 0),
              ('Android', 10, 2, 2, 'none', 'melee', 2, 0),
              ('Sentinel', 25, 5, 4, 'ranged', 'melee', 5, 0),
              ('Enforcer', 100, 25, 1, 'close combat', 'ranged weaponsList', 20, 0),
              ('Cyborg', 75, 50, 10, 'bio regeneration', 'emp grenades', 50, 0)
        ");

            cmdInsertPlayerValues.ExecuteNonQuery();
            cmdInsertWeaponValues.ExecuteNonQuery();
            cmdInsertBestiaryValues.ExecuteNonQuery();
        }

        /// <summary>
        /// Retunere dataen fra våben tabellen alt efter hvilket navn det har
        /// </summary>
        /// <param name="weaponName">Hvilket våben navn databasen skal lede efter</param>
        /// <returns>våbnes data</returns>
        public WeaponDB ReturnValues(string weaponName)
        {
            dataSource = NpgsqlDataSource.Create(connectionString);
            NpgsqlCommand cmd = dataSource.CreateCommand($"SELECT weapon_id, name, damage, price FROM weapon " +
                                                     $"WHERE (name = '{weaponName}')");
            NpgsqlDataReader reader = cmd.ExecuteReader();
            WeaponDB result = new WeaponDB();

            while (reader.Read())
            {
                result = new WeaponDB { weapon_id = (int)reader.GetValue(0), name = reader.GetValue(1).ToString(), damage = (int)reader.GetValue(2), price = (int)reader.GetValue(3) };

            }
            reader.Close();

            return result;
        }

        /// <summary>
        /// Retunere dataen fra våben tabellen alt efter hvilket ID det har
        /// </summary>
        /// <param name="weaponID">Våbnes ID databasen skal lede efter</param>
        /// <returns>Våbnes data</returns>
        public WeaponDB ReturnValuesWithID(int weaponID)
        {
            dataSource = NpgsqlDataSource.Create(connectionString);
            NpgsqlCommand cmd = dataSource.CreateCommand($"SELECT weapon_id, name, damage, price FROM weapon " +
                                                     $"WHERE (weapon_id = '{weaponID}')");
            NpgsqlDataReader reader = cmd.ExecuteReader();
            WeaponDB result = new WeaponDB();

            while (reader.Read())
            {
                result = new WeaponDB { weapon_id = (int)reader.GetValue(0), name = reader.GetValue(1).ToString(), damage = (int)reader.GetValue(2), price = (int)reader.GetValue(3) };

            }
            reader.Close();

            return result;
        }

        /// <summary>
        /// Info på alle monstre der er i databasen
        /// </summary>
        /// <returns></returns>
        public List<BestiaryInfo> ShowBestiaryInfo()
        {
            string name, health, damage, strengths, weaknesses, scrap_dropped, defeated;
            dataSource = NpgsqlDataSource.Create(connectionString);
            NpgsqlCommand cmdShowBestiary = dataSource.CreateCommand($"SELECT name, health, damage, strengths, weaknesses, scrap_dropped, defeated FROM bestiary");

            NpgsqlDataReader reader = cmdShowBestiary.ExecuteReader();
            List<BestiaryInfo> beastInfo = new List<BestiaryInfo>();

            while (reader.Read())
            {
                name = reader.GetValue(0).ToString();
                health = reader.GetValue(1).ToString();
                damage = reader.GetValue(2).ToString();
                strengths = reader.GetValue(3).ToString();
                weaknesses = reader.GetValue(4).ToString();
                scrap_dropped = reader.GetValue(5).ToString();
                defeated = reader.GetValue(6).ToString();

                BestiaryInfo info = new BestiaryInfo();

                info.name = name;
                info.health = int.Parse(health);
                info.damage = int.Parse(damage);
                info.strengths = strengths;
                info.weaknesses = weaknesses;
                info.scrap_dropped = int.Parse(scrap_dropped);
                info.defeated = int.Parse(defeated);

                beastInfo.Add(info);
            }

            reader.Close();

            return beastInfo;
        }

        /// <summary>
        /// Checker om spilleren har nok scraps til at købe våbnet 
        /// </summary>
        /// <param name="weapon">Hvilket våben spilleren prøver at købe</param>
        /// <returns>Retunere om spilleren har nok scraps til at købe våbnet eller ej</returns>
        public bool TradeWeapon(Weapon weapon)
        {
            dataSource = NpgsqlDataSource.Create(connectionString);

            NpgsqlCommand cmdGetScraps = dataSource.CreateCommand($@"
            SELECT scrap_amount FROM player WHERE (name = 'TestPlayer')");

            NpgsqlDataReader reader = cmdGetScraps.ExecuteReader();
            while (reader.Read())
            {
                scrapAmount = (int)reader.GetValue(0);

            }
            reader.Close();

            if (scrapAmount > weapon.Price)
            {

                NpgsqlCommand cmdBuyWeapon = dataSource.CreateCommand($@"
        INSERT INTO inventory (weapon_name, damage, price)

        VALUES('{weapon.Name}', '{weapon.Damage}', '{weapon.Price}')
        ");

                NpgsqlCommand cmdUpdateScrapAmount = dataSource.CreateCommand($@"
        UPDATE player
        SET scrap_amount = scrap_amount - {weapon.Price}
        ");

                cmdBuyWeapon.ExecuteNonQuery();
                cmdUpdateScrapAmount.ExecuteNonQuery();
                playerItemsUpdated = true;
            }
            else
            {
                return false;
            }
            return true;
        }

        //VIRKER IKKE SORTER_______________________________________________________________________________________
        public void SortTables()
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

        /// <summary>
        /// VIS OVERSIGT OVER FEJNDER_______________________________________________________________________________________
        /// </summary>
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

        /// <summary>
        /// Returnere et våben navn, som kan oprettes 
        /// </summary>
        /// <returns>Returnere et våben navn, som kan oprettes</returns>
        public string AddToInventory()
        {
            dataSource = NpgsqlDataSource.Create(connectionString);
            NpgsqlCommand cmdCreateWeapons = dataSource.CreateCommand($"SELECT weapon_name FROM inventory");
            NpgsqlDataReader reader = cmdCreateWeapons.ExecuteReader();
            while (reader.Read())
            {
                weaponName = reader.GetString(0);
            }
            reader.Close();
            return weaponName;
        }

        /// <summary>
        /// Opdatere hvor mange scraps spilleren har efter de har købt noget
        /// </summary>
        /// <returns>Retunere den nye mængde af scraps spilleren har</returns>
        public int UpdateScraps()
        {
            dataSource = NpgsqlDataSource.Create(connectionString);
            NpgsqlCommand cmdUpdateScraps = dataSource.CreateCommand($@"SELECT scrap_amount FROM player WHERE (name = 'TestPlayer')");
            NpgsqlDataReader reader = cmdUpdateScraps.ExecuteReader();
            while (reader.Read())
            {
                scrapAmount = (int)reader.GetValue(0);
            }
            return scrapAmount;
        }
    }
}
