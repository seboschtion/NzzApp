using System;
using Sebastian.Toolkit.SQLite;

namespace NzzApp.Data.Versions
{
    public class Version0007 : DatabaseVersion
    {
        public override Version DbVersion => new Version(0, 0, 0, 7);

        protected override void Upgrade(SQLiteTransaction transaction)
        {
            transaction.Execute(@"ALTER TABLE AppSettings ADD COLUMN ""ArticleFontFamily"" TEXT NOT NULL DEFAULT ""Georgia""");
        }
    }
}
