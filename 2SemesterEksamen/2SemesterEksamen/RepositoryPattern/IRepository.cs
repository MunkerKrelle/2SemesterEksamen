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

       //void SortTables();

        Tuple<string, int, int> ReturnValues(string weaponName);

        Tuple<string, int, int> ReturnValuesWithID(int weaponID);

        void ShowBestiary();

        string AddToInventory();

        int UpdateScraps();

        List<BestiaryInfo> ShowBestiaryInfo();

    }
}