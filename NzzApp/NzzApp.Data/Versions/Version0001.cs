using System;
using Sebastian.Toolkit.SQLite;

namespace NzzApp.Data.Versions
{
    internal class Version0001 : DatabaseVersion
    {
        public override Version DbVersion => new Version(0, 0, 0, 1);

        protected override void Upgrade(SQLiteTransaction transaction)
        {
            transaction.Execute(@"CREATE TABLE IF NOT EXISTS ""AppSettings"" (
                                 ""Id"" INTEGER PRIMARY KEY, 
                                 ""FirstAppStart"" INTEGER NOT NULL,
                                 ""SuccessfullInitialization"" INTEGER NOT NULL)");

            transaction.Execute(@"INSERT INTO AppSettings " +
                                 "VALUES (1, 1, 0)");
        }
    }
}