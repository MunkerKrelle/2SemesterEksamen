using Npgsql;
using System.Collections.Generic;
using System;
using System.Linq;
using _2SemesterEksamen;
using ComponentPattern;

namespace RepositoryPattern
{
    public class User
    {
    }

    public interface IRepository
    {
        void DropTables();
        void CreateTables();

        void Insert();

        void TradeWeapon(Weapon weapon);

        void SortTables();

        public Tuple<string, int, int> ReturnValues(string weaponName);

        public Tuple<string, int, int> ReturnValuesWithID(int weaponID);


        List<BestiaryInfo> ShowBestiaryInfo();
    }
}