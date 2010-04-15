using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Project;

using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace rminasian.SqlDbProj {
    [Guid(GuidList.guidSqlDbProjCmdProjectFactoryString)]
    public class SqlDbProjProjectFactory : ProjectFactory {
        private SqlDbProjPackage package;
        public SqlDbProjProjectFactory(SqlDbProjPackage package)
            : base(package) {
            this.package = package;
        }

        protected override ProjectNode CreateProject() {
            var project = new SqlDbProjProjectNode(this.package);
            project.SetSite((IOleServiceProvider)((IServiceProvider)this.package).
                GetService(typeof(IOleServiceProvider)));

            return project;
        }
    }
}
