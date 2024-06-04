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
        void DropTables();
        void CreateTables();

        void Insert();

        bool TradeWeapon(Weapon weapon);

        public Tuple<string, int, int> ReturnValues(string weaponName);

        string AddToInventory();

        int UpdateScraps();

        List<BestiaryInfo> ShowBestiaryInfo();

    }
}