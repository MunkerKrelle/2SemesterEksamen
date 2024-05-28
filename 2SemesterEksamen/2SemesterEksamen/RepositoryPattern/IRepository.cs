using Npgsql;
using System.Collections.Generic;
using System;
using System.Linq;
using _2SemesterEksamen;
using ComponentPattern;

namespace RepositoryPattern
{

    public interface IRepository
    {
        void DropTables();
        void CreateTables();

        void Insert();

        bool TradeWeapon(Weapon weapon);

        public Tuple<string, int, int> ReturnValues(string weaponName);

        public Tuple<string, int, int> ReturnValuesWithID(int weaponID);


        List<BestiaryInfo> ShowBestiaryInfo();
    }
}