using Npgsql;
using System.Collections.Generic;
using System;
using System.Linq;
using _2SemesterEksamen;

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

        void TradeWeapon();

        void SortTables();

        List<Tuple<string, int, int>> ReturnValues(string weaponName);

        List<BestiaryInfo> ShowBestiaryInfo();
    }
}