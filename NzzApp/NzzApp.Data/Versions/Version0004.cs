using System;
using Sebastian.Toolkit.SQLite;

namespace NzzApp.Data.Versions
{
    internal class Version0004 : DatabaseVersion
    {
        public override Version DbVersion => new Version(0, 0, 0, 4);

        protected override void Upgrade(SQLiteTransaction transaction)
        {
            transaction.Execute(@"ALTER TABLE AppSettings ADD COLUMN ""ArticleFontSize"" INTEGER NOT NULL DEFAULT 14");
        }
    }
}
