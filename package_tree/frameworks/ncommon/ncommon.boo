install ncommon:
    get_from svn("https://ncommon.svn.codeplex.com/svn/trunk/")
    build_with msbuild, buildfile("NCommon Everything.sln"), FrameworkVersion35

    shared_library "Libs"
    build_root_dir "build"

dependencies:
    depend "castle.tools"              >> "Castle.DynamicProxy"
    depend "castle.tools"              >> "Castle.DynamicProxy2"
    depend "fluentnhibernate"          >> "FluentNHibernate"
    depend "nhibernate"       >> "2.1" >> "NHibernate"
    depend "nhibernate"       >> "2.1" >> "nunit.framework"
    depend "nhibernate"       >> "2.1" >> "LinFu.DynamicProxy"     
    depend "nhibernate"       >> "2.1" >> "NHibernate.ByteCode.Castle"
    depend "nhibernate"       >> "2.1" >> "Iesi.Collections"
    depend "nhibernate"       >> "2.1" >> "NHibernate.ByteCode.LinFu"
    depend "nhibernate.linq"  >> "2.1" >> "nhibernate.linq"
    depend @log4net           >> "1.2.10" >>  "log4net"

package.category = "Frameworks"
package.description = "NCommon is a library that contains implementations of commonly used design patterns when developing applications."
package.forum = "http://ncommon.codeplex.com/Thread/List.aspx"
package.homepage = "http://ncommon.codeplex.com/"
package.name = "NCommon"
package.notes = ""
package.version = "1.0.0.0"
