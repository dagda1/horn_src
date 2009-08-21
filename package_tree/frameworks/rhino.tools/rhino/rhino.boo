install rhino:
    get_from svn("https://rhino-tools.svn.sourceforge.net/svnroot/rhino-tools/trunk")
    build_with nant, buildfile("default.build"), FrameworkVersion35

    switches:
        parameters "build.warnaserrors=false","common.testrunner.enabled=false","sign=true"

    shared_library "SharedLibs"
    build_root_dir "build"

dependencies:
    depend "castle.tools"                 >> "Castle.Core"
    depend "castle.tools"                 >> "Castle.DynamicProxy"
    depend "castle.tools"                 >> "Castle.DynamicProxy2"
    depend "castle.windsor"               >> "Castle.MicroKernel"
    depend "castle.windsor"               >> "Castle.Windsor"
    depend "castle.services"              >> "Castle.Services.Transaction"
    depend "castle.services"              >> "Castle.Services.Logging.Log4netIntegration"
    depend "castle.services"              >> "Castle.Services.Logging.NLogIntegration"
    depend "castle.components"            >> "Castle.Components.Validator"
    depend "castle.activerecord"          >> "Castle.ActiveRecord"
    depend "castle.facilities"            >> "Castle.Facilities.AutomaticTransactionManagement"
    depend "nhibernate.caches"            >> "NHibernate.Caches.SysCache"
    depend "nhibernate"          >> "2.1" >> "NHibernate"     
    depend "nhibernate"          >> "2.1" >> "NHibernate.ByteCode.Castle"
    depend "nhibernate"          >> "2.1" >> "Iesi.Collections"

package.category = "Frameworks"
package.description = "Rhino tools including Rhino Mocks, Rhino ETL, Binsor etc."
package.homepage = "http://ayende.com/projects/rhino-mocks.aspx"
package.forum = "http://groups.google.co.uk/group/rhino-tools-dev"
package.name = "Rhino Tools"
package.notes = ""
package.version = "1.0.0.0"