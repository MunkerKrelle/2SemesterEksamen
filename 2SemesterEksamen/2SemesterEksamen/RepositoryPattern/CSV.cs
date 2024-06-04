using _2SemesterEksamen;
using ComponentPattern;
using CsvHelper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using (var reader = new StreamReader(""))
using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
{
    var records = csv.GetRecord<Weapon>();
}


namespace RepositoryPattern
{
    /// <summary>
    /// Database for sql funktioner
    /// </summary>
    public class CSV : IRepository
    {
        private readonly IRepository repository;
        public static bool playerItemsUpdated = false;

        //Nogle af disse er ikke blevet brugt endnu
        private string charName, weaponName;
        private int health, scrapAmount, damage, price, scrapDropped, defeated;
        private float speed;
        private bool buy, sell, enemyKilled;

        public CSV()
        {
        }

        public CSV(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Reseter og indlæser alt dataen og tabellerne
        /// </summary>
        public void CreateDatabase()
        {
            DropTables();
            CreateTables();
            Insert();
        }

        /// <summary>
        /// Opretter alle tabellerne for spillet
        /// </summary>
        public void CreateTables()
        {
            var player = new Player { };
            using (var writer = new StreamWriter(@"CSVFiles\player.csv"))

            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecord(player);
            }

            var inventory = new Inventory { };
            using (var writer = new StreamWriter(@"CSVFiles\inventory.csv"))

            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecord(inventory);
            }

            var weapon = new Weapon { };
            using (var writer = new StreamWriter(@"CSVFiles\weapon.csv"))

            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecord(weapon);
            }

            var beatiary = new BestiaryInfo { };
            using (var writer = new StreamWriter(@"CSVFiles\bestiary.csv"))

            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecord(beatiary);
            }

            //NpgsqlCommand cmdCreateHasTable = dataSource.CreateCommand(@"
            //    CREATE TABLE IF NOT EXISTS has (
            //        name VARCHAR(255) REFERENCES player(name),
            //        weapon_ID INT REFERENCES weapon(weapon_id)
            //    );");

            //NpgsqlCommand cmdCreateTradesTable = dataSource.CreateCommand(@"
            //    CREATE TABLE IF NOT EXISTS trades (
            //        name VARCHAR(255) REFERENCES arms_dealer(name),
            //        weapon_ID INT REFERENCES weapon(weapon_id)
            //    );");
        }

        /// <summary>
        /// Removes the tables, so we can reset them for a new game
        /// </summary>
        public void DropTables()
        {
        }

        /// <summary>
        /// Indsæt alt data ind i de forskellige tabeller
        /// </summary>
        public void Insert()
        //VALUES SKAL VÆRE PLAYER/ENEMY.X
        {
            var player = new Player
            {
                name = "TestPlayer",
                health = 1000,
                speed = 50,
                scrap_amount = 1000
            };
            using (var writer = new StreamWriter(@"CSVFiles\player.csv"))

            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecord(player);
            }

            var weapon = new List<Weapon> {
                new Weapon {weapon_id = 1, name = "Butterflyknife", damage = 2, price = 10},
                new Weapon {weapon_id = 2, name = "Bat", damage = 5, price = 20},
                new Weapon {weapon_id = 3, name = "Katana", damage = 10, price = 50},
                new Weapon {weapon_id = 4, name = "Chainsword", damage = 25, price = 100},
                new Weapon {weapon_id = 5, name = "Hammer", damage = 8, price = 25},
                new Weapon {weapon_id = 6, name = "Crimsonblade", damage = 50, price = 300},
                new Weapon {weapon_id = 7, name = "Nunchaku", damage = 30, price = 150},
                new Weapon {weapon_id = 8, name = "Annihilator", damage = 100, price = 500},
            };

            using (var writer = new StreamWriter(@"CSVFiles\weapon.csv"))

            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(weapon);
            }

            var beatiary = new List<BestiaryInfo> {
            new BestiaryInfo {name = "Drone", health = 5, damage = 1, speed = 1, strengths = "none", weaknesses = "everything", scrap_dropped = 1, defeated = 0},
            new BestiaryInfo {name = "Android", health = 10, damage = 2, speed = 2, strengths = "none", weaknesses = "melee", scrap_dropped = 2, defeated = 0},
            new BestiaryInfo {name = "Sentinel", health = 25, damage = 5, speed = 4, strengths = "ranged", weaknesses = "melee", scrap_dropped = 5, defeated = 0},
            new BestiaryInfo {name = "Enforcer", health = 100, damage = 25, speed = 1, strengths = "close combat", weaknesses = "ranged", scrap_dropped = 20, defeated = 0},
            new BestiaryInfo {name = "Cyborg", health = 75, damage = 50, speed = 10, strengths = "bio regeneration", weaknesses = "emp grenades", scrap_dropped = 50, defeated = 0}
            };
            using (var writer = new StreamWriter(@"CSVFiles\bestiary.csv"))

            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(beatiary);
            }
        }

        /// <summary>
        /// Retunere dataen fra våben tabellen alt efter hvilket navn det har
        /// </summary>
        /// <param name="weaponName">Hvilket våben navn databasen skal lede efter</param>
        /// <returns>våbnes data</returns>
        public Tuple<string, int, int> ReturnValues(string weaponName)
        {
            using (var streamReader = new StreamReader(@"CSVFiles\weapon.csv"))
            {
                using (var csvReader = new CsvReader (streamReader, CultureInfo.InvariantCulture))
                {
                    var records = csvReader.GetRecords<dynamic>().ToList();
                }
            }
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

        /// <summary>
        /// Retunere dataen fra våben tabellen alt efter hvilket ID det har
        /// </summary>
        /// <param name="weaponID">Våbnes ID databasen skal lede efter</param>
        /// <returns>Våbnes data</returns>
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
        //public void SortTables()
        //{
        //    NpgsqlCommand cmdSortInventoryTable = dataSource.CreateCommand($@"
        //SELECT * 

        //FROM inventory

        //ORDER BY damage ASC
        //");

        //    NpgsqlCommand cmdSortWeaponTable = dataSource.CreateCommand($@"
        //SELECT * 

        //FROM weapon

        //ORDER BY price ASC
        //");

        //    cmdSortInventoryTable.ExecuteNonQuery();
        //    cmdSortWeaponTable.ExecuteNonQuery();

        //    Console.WriteLine("You've been sorted mate");
        //}

        /// <summary>
        /// VIS OVERSIGT OVER FEJNDER_______________________________________________________________________________________
        /// </summary>
        public void ShowBestiary()
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

        public bool TradeWeapon(ComponentPattern.Weapon weapon)
        {
            throw new NotImplementedException();
        }

        private class Player
        {
            public string name { get; set; }
            public int health { get; set; }
            public float speed { get; set; }
            public int scrap_amount { get; set; }
        }

        private class Inventory
        {
            public int item_id { get; set; }
            public string weapon_name { get; set; }
            public int damage { get; set; }
            public int price { get; set; }
        }

        private class Weapon
        {
            public int weapon_id { get; set; }
            public string name { get; set; }
            public int damage { get; set; }
            public int price { get; set; }
        }



        //NpgsqlCommand cmdCreateArmsDealerTable = dataSource.CreateCommand(@"
        //        CREATE TABLE IF NOT EXISTS arms_dealer (
        //            name VARCHAR(255) PRIMARY KEY
        //        );");

        //NpgsqlCommand cmdCreateHasTable = dataSource.CreateCommand(@"
        //        CREATE TABLE IF NOT EXISTS has (
        //            name VARCHAR(255) REFERENCES player(name),
        //            weapon_ID INT REFERENCES weapon(weapon_id)
        //        );");

        //NpgsqlCommand cmdCreateTradesTable = dataSource.CreateCommand(@"
        //        CREATE TABLE IF NOT EXISTS trades (
        //            name VARCHAR(255) REFERENCES arms_dealer(name),
        //            weapon_ID INT REFERENCES weapon(weapon_id)
        //        );");

    }
}
