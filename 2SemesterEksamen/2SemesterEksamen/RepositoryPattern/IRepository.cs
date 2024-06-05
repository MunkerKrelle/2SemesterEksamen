using Npgsql;
using System.Collections.Generic;
using System;
using System.Linq;
using _2SemesterEksamen;
using ComponentPattern;

namespace RepositoryPattern
{
    /// <summary>
    /// Interface for repository systemer, hvis vi ville bruge forskellige servere 
    /// </summary>
    public interface IRepository
    {
        public static IRepository currentRepository;

        void DropTables();
        void CreateTables();

        void Insert();

        bool TradeWeapon(Weapon weapon);

        //void SortTables();

        WeaponDB ReturnValues(string weaponName);

        WeaponDB ReturnValuesWithID(int weaponID);

        void ShowBestiary();

        string AddToInventory();

        int UpdateScraps();

        List<BestiaryInfo> ShowBestiaryInfo();

    }
    public class PlayerDB
    {
        public string name { get; set; }
        public int health { get; set; }
        public float speed { get; set; }
        public int scrap_amount { get; set; }
    }

    public class InventoryDB
    {
        public int item_id { get; set; }
        public string weapon_name { get; set; }
        public int damage { get; set; }
        public int price { get; set; }
    }

    public class WeaponDB
    {
        public int weapon_id { get; set; }
        public string name { get; set; }
        public int damage { get; set; }
        public int price { get; set; }
    }
}
