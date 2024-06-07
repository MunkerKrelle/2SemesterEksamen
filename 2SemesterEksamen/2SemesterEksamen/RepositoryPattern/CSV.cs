using _2SemesterEksamen;
using ComponentPattern;
using CsvHelper;
using Npgsql;
using Npgsql.Internal;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

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
            var player = new PlayerDB
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

            var weapon = new List<WeaponDB> {
                new WeaponDB {weapon_id = 1, name = "Butterflyknife", damage = 2, price = 10},
                new WeaponDB {weapon_id = 2, name = "Bat", damage = 5, price = 20},
                new WeaponDB {weapon_id = 3, name = "Katana", damage = 10, price = 50},
                new WeaponDB {weapon_id = 4, name = "Chainsword", damage = 25, price = 100},
                new WeaponDB {weapon_id = 5, name = "Hammer", damage = 8, price = 25},
                new WeaponDB {weapon_id = 6, name = "Crimsonblade", damage = 50, price = 300},
                new WeaponDB {weapon_id = 7, name = "Nunchaku", damage = 30, price = 150},
                new WeaponDB {weapon_id = 8, name = "Annihilator", damage = 100, price = 500},
            };

            using (var writer = new StreamWriter(@"CSVFiles\weapon.csv"))

            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(weapon);
            }
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
        }

        /// <summary>
        /// Retunere dataen fra våben tabellen alt efter hvilket navn det har
        /// </summary>
        /// <param name="weaponName">Hvilket våben navn databasen skal lede efter</param>
        /// <returns>våbnes data</returns>
        public WeaponDB ReturnValues(string weaponName)
        {
            WeaponDB result = new WeaponDB();
            using (var streamReader = new StreamReader(@"CSVFiles\weapon.csv"))
            {
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    var records = csvReader.GetRecords<WeaponDB>().ToList();
                    int i = 0;
                    foreach (var weapon in records)
                    {
                        i++; 
                        if (weaponName == weapon.name)
                        {
                            result = weapon;
                        }
                    }
                }
            }

            using (var writer = new StreamWriter(@"CSVFiles\inventory.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecord(result);
            }

            return result;
        }

        /// <summary>
        /// Retunere dataen fra våben tabellen alt efter hvilket ID det har
        /// </summary>
        /// <param name="weaponID">Våbnes ID databasen skal lede efter</param>
        /// <returns>Våbnes data</returns>
        public WeaponDB ReturnValuesWithID(int weaponID)
        {
            WeaponDB result;
            using (var streamReader = new StreamReader(@"CSVFiles\weapon.csv"))
            {
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    var records = csvReader.GetRecords<WeaponDB>().ToList();
                    result = records[weaponID];
                }
            }

            using (var writer = new StreamWriter(@"CSVFiles\inventory.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecord(result);
            }

            return result;
        }

        /// <summary>
        /// Info på alle monstre der er i databasen
        /// </summary>
        /// <returns></returns>
        public List<BestiaryInfo> ShowBestiaryInfo()
        {
            //string name, health, damage, strengths, weaknesses, scrap_dropped, defeated;
            //dataSource = NpgsqlDataSource.Create(connectionString);
            //NpgsqlCommand cmdShowBestiary = dataSource.CreateCommand($"SELECT name, health, damage, strengths, weaknesses, scrap_dropped, defeated FROM bestiary");

            //NpgsqlDataReader reader = cmdShowBestiary.ExecuteReader();
            List<BestiaryInfo> beastInfo = new List<BestiaryInfo>();

            //while (reader.Read())
            //{
            //    name = reader.GetValue(0).ToString();
            //    health = reader.GetValue(1).ToString();
            //    damage = reader.GetValue(2).ToString();
            //    strengths = reader.GetValue(3).ToString();
            //    weaknesses = reader.GetValue(4).ToString();
            //    scrap_dropped = reader.GetValue(5).ToString();
            //    defeated = reader.GetValue(6).ToString();

            //    BestiaryInfo info = new BestiaryInfo();

            //    info.name = name;
            //    info.health = int.Parse(health);
            //    info.damage = int.Parse(damage);
            //    info.strengths = strengths;
            //    info.weaknesses = weaknesses;
            //    info.scrap_dropped = int.Parse(scrap_dropped);
            //    info.defeated = int.Parse(defeated);

            //    beastInfo.Add(info);
            //}

            //reader.Close();

            return beastInfo;
        }

        /// <summary>
        /// Checker om spilleren har nok scraps til at købe våbnet 
        /// </summary>
        /// <param name="weapon">Hvilket våben spilleren prøver at købe</param>
        /// <returns>Retunere om spilleren har nok scraps til at købe våbnet eller ej</returns>
        public bool TradeWeapon(Weapon weapon)
        {
            //    dataSource = NpgsqlDataSource.Create(connectionString);

            //    NpgsqlCommand cmdGetScraps = dataSource.CreateCommand($@"
            //    SELECT scrap_amount FROM player WHERE (name = 'TestPlayer')");

            //    NpgsqlDataReader reader = cmdGetScraps.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        scrapAmount = (int)reader.GetValue(0);

            //    }
            //    reader.Close();

            //    if (scrapAmount > weapon.Price)
            //    {

            //        NpgsqlCommand cmdBuyWeapon = dataSource.CreateCommand($@"
            //INSERT INTO inventory (weapon_name, damage, price)

            //VALUES('{weapon.Name}', '{weapon.Damage}', '{weapon.Price}')
            //");

            //        NpgsqlCommand cmdUpdateScrapAmount = dataSource.CreateCommand($@"
            //UPDATE player
            //SET scrap_amount = scrap_amount - {weapon.Price}
            //");

            //        cmdBuyWeapon.ExecuteNonQuery();
            //        cmdUpdateScrapAmount.ExecuteNonQuery();
            //        playerItemsUpdated = true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
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
            //NpgsqlCommand cmd = dataSource.CreateCommand($"SELECT name, health, damage, speed, strengths, weaknesses, scrap_dropped, defeated FROM bestiary");
            //NpgsqlDataReader reader = cmd.ExecuteReader();

            //while (reader.Read())
            //{
            //    Console.WriteLine($"Name: {reader.GetValue(0)}, Health: {reader.GetValue(1)}, Damage: {reader.GetValue(2)}, " +
            //        $"Speed: {reader.GetValue(3)}, Strengths: {reader.GetValue(4)}, Weaknesses: {reader.GetValue(5)}, " +
            //        $"Scrap Dropped: {reader.GetValue(6)}, Defeated: {reader.GetValue(7)}");
            //}
        }

        /// <summary>
        /// Returnere et våben navn, som kan oprettes 
        /// </summary>
        /// <returns>Returnere et våben navn, som kan oprettes</returns>
        public string AddToInventory()
        {
            //dataSource = NpgsqlDataSource.Create(connectionString);
            //NpgsqlCommand cmdCreateWeapons = dataSource.CreateCommand($"SELECT weapon_name FROM inventory");
            //NpgsqlDataReader reader = cmdCreateWeapons.ExecuteReader();
            //while (reader.Read())
            //{
            //    weaponName = reader.GetString(0);
            //}
            //reader.Close();
            return weaponName;
        }

        /// <summary>
        /// Opdatere hvor mange scraps spilleren har efter de har købt noget
        /// </summary>
        /// <returns>Retunere den nye mængde af scraps spilleren har</returns>
        public int UpdateScraps()
        {
            //dataSource = NpgsqlDataSource.Create(connectionString);
            //NpgsqlCommand cmdUpdateScraps = dataSource.CreateCommand($@"SELECT scrap_amount FROM player WHERE (name = 'TestPlayer')");
            //NpgsqlDataReader reader = cmdUpdateScraps.ExecuteReader();
            //while (reader.Read())
            //{
            //    scrapAmount = (int)reader.GetValue(0);
            //}
            return scrapAmount;
        }
    }
}
