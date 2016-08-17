using System;
using Sebastian.Toolkit.SQLite;

namespace NzzApp.Data.Versions
{
    public class Version0006 : DatabaseVersion
    {
        public override Version DbVersion => new Version(0, 0, 0, 6);

        protected override void Upgrade(SQLiteTransaction transaction)
        {
            transaction.Execute(@"ALTER TABLE AppSettings ADD COLUMN ""DisableLiveTileTask"" INTEGER NOT NULL DEFAULT 1");
        }
    }
}
