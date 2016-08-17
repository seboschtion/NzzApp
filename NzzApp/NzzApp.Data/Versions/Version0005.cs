using System;
using Sebastian.Toolkit.SQLite;

namespace NzzApp.Data.Versions
{
    public class Version0005 : DatabaseVersion
    {
        public override Version DbVersion => new Version(0, 0, 0, 5);

        protected override void Upgrade(SQLiteTransaction transaction)
        {
            transaction.Execute(@"ALTER TABLE AppSettings ADD COLUMN ""LastLiveTileTaskExecutionDate"" INTEGER NOT NULL DEFAULT 0");
        }
    }
}
