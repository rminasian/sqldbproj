using System;

namespace rminasian.SqlDbProj
{
    static class GuidList
    {
        public const string guidSqlDbProjPkgString = "60bfa34c-211e-489d-8826-13b4ddbe5368";
        public const string guidSqlDbProjCmdSetString = "312b22c4-99d9-4d21-805c-7697f1814c29";
        public const string guidSqlDbProjCmdProjectFactoryString = "bd783696-3b58-4851-bb16-55c7f1d3c995";

        public static readonly Guid guidSqlDbProjCmdSet = new Guid(guidSqlDbProjCmdSetString);
        public static readonly Guid guidSqlDbProjCmdProjectFactory = new Guid(guidSqlDbProjCmdProjectFactoryString);
    };
}