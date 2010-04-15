using System;
using Microsoft.VisualStudio.Project;

namespace rminasian.SqlDbProj {
    public class SqlDbProjProjectNode : ProjectNode {
        private SqlDbProjPackage package;

        public SqlDbProjProjectNode(SqlDbProjPackage package) {
            this.package = package;
        }

        public override Guid ProjectGuid { get { return GuidList.guidSqlDbProjCmdProjectFactory; } }

        public override string ProjectType { get { return "SqlDbProjType"; } }

        public override void AddFileFromTemplate(
            string source, string target) {
            FileTemplateProcessor.UntokenFile(source, target);

            FileTemplateProcessor.Reset();
        }
    }
}