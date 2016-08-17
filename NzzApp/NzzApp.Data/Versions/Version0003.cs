using System;
using Sebastian.Toolkit.SQLite;

namespace NzzApp.Data.Versions
{
    internal class Version0003 : DatabaseVersion
    {
        public override Version DbVersion => new Version(0, 0, 0, 3);

        protected override void Upgrade(SQLiteTransaction transaction)
        {
            transaction.Execute(@"ALTER TABLE AppSettings ADD COLUMN ""BreakingLiveTileEnabled"" INTEGER NOT NULL DEFAULT 1");
        }
    }
}
