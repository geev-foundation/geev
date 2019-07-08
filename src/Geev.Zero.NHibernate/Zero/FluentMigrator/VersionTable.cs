using FluentMigrator.Runner.VersionTableInfo;

namespace Geev.Zero.FluentMigrator
{
    [VersionTableMetaData]
    public class VersionTable : DefaultVersionTableMetaData
    {
        public override string TableName
        {
            get
            {
                return "GeevVersionInfo";
            }
        }
    }
}
