using _2SemesterEksamen;
using ComponentPattern;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Security.Cryptography;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace RepositoryPattern
{
    public class Database : IRepository
    {
        private readonly IRepository repository;
        private NpgsqlDataSource dataSource;
        private string connectionString = "Host=localhost;Username=postgres;Password=Cqw52and;Database=postgres";

        private string charName, weaponName;
        private int health, scrapAmount, damage, price, scrapDropped, defeated;
        private float speed;
        private bool buy, sell, enemyKilled;

        public Database()
        {
        }

        public Database(IRepository repository)
        {
            this.repository = repository;
        }

        public void RunLoop()
        {
            dataSource = NpgsqlDataSource.Create(connectionString);

            DropTables();
            CreateTables();
            Insert();
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

        public void Insert()
        //VALUES SKAL VÆRE PLAYER/ENEMY.X

        {
            NpgsqlCommand cmdInsertPlayerValues = dataSource.CreateCommand($@"
        INSERT INTO player (name, health, speed, scrap_amount)

        VALUES('TestPlayer', 100, 50, 0)
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

        public Tuple<string,int,int> ReturnValues(string weaponName)
        {
            dataSource = NpgsqlDataSource.Create(connectionString);
            NpgsqlCommand cmd = dataSource.CreateCommand($"SELECT name, damage, price FROM weapon " +
                                                     $"WHERE (name = '{weaponName}')");
            NpgsqlDataReader reader = cmd.ExecuteReader();
            Tuple<string, int, int> list = null;
     
            while (reader.Read())
            {
                list = (new Tuple<string, int, int>(reader.GetValue(0).ToString(), (int)reader.GetValue(1), (int)reader.GetValue(2)));

            }
            reader.Close();

            return list;
        }

        public Tuple<string, int, int> ReturnValuesWithID(int weaponID)
        {
            dataSource = NpgsqlDataSource.Create(connectionString);
            NpgsqlCommand cmd = dataSource.CreateCommand($"SELECT weapon_id, name, damage, price FROM weapon " +
                                                     $"WHERE (weapon_id = '{weaponID}')");
            NpgsqlDataReader reader = cmd.ExecuteReader();
            Tuple<string, int, int> list = null;

            while (reader.Read())
            {
                list = (new Tuple<string, int, int>(reader.GetValue(1).ToString(), (int)reader.GetValue(2), (int)reader.GetValue(3)));

            }
            reader.Close();

            return list;
        }

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


        public void TradeWeapon()
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

        //NÅR EN FEJENDE ER BESEJRET_________________________________________________________________________________
        //private void CollectScrap()
        //{
        //    if (interact)
        //    {
        //        NpgsqlCommand cmdCollectScrap = dataSource.CreateCommand($@"
        //    UPDATE player
        //    SET scrap_amount = scrap_amount + {scrapDropped}
        //    ");

        //        cmdCollectScrap.ExecuteNonQuery();
        //    }
        //}

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

        public Tuple<int, string> CreateNewShop(int weaponID)
        {
            dataSource = NpgsqlDataSource.Create(connectionString);
            NpgsqlCommand cmdFindWeapons = dataSource.CreateCommand($"SELECT weapon_id, name FROM weapon " +
                                                     $"WHERE (weapon_id = '{weaponID}')");
            NpgsqlDataReader reader = cmdFindWeapons.ExecuteReader();
            Tuple<int, string> list = null;

            while (reader.Read())
            {
                list = new Tuple <int, string>((int)reader.GetValue(0), reader.GetValue(1).ToString());
                weaponID = (int)reader.GetValue(0);
                charName = reader.GetValue(1).ToString();

            }
            reader.Close();

            NpgsqlCommand cmdSellWeapon = dataSource.CreateCommand($@"
        INSERT INTO trades (weapon_id, name)

        VALUES('{weaponID}', '{charName}')
        ");

            return list;

        }

        public void ResetShop()
        {
            NpgsqlCommand cmdDropTradesTable = dataSource.CreateCommand(@"
            DROP TABLE trades;
            ");
            cmdDropTradesTable.ExecuteNonQuery();

            NpgsqlCommand cmdCreateTradesTable = dataSource.CreateCommand(@"
                CREATE TABLE IF NOT EXISTS trades (
                    name VARCHAR(255) REFERENCES arms_dealer(name),
                    weapon_ID INT REFERENCES weapon(weapon_id)
                );");
            cmdCreateTradesTable.ExecuteNonQuery();
        }

        public void AddToInventory()
        {

        }

        public void RemoveFromInventory()
        {

        }
    }
}
