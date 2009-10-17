install masstransit:
    get_from svn("http://masstransit.googlecode.com/svn/trunk/")
    build_with nant, buildfile("BuildScripts/masstransit.build"), FrameworkVersion35	

    shared_library "libs"
    build_root_dir "bin"

dependencies:
    depend "castle.tools"              >> "Castle.Core"
    depend "castle.tools"              >> "Castle.DynamicProxy2"
    depend "castle.windsor"            >> "Castle.MicroKernel"
    depend "castle.windsor"            >> "Castle.Windsor"
    depend "fluentnhibernate"          >> "FluentNHibernate"
    depend "nhibernate"       >> "2.1" >> "NHibernate"
    depend "nhibernate"       >> "2.1" >> "NHibernate.ByteCode.Castle"
    depend "nhibernate"       >> "2.1" >> "Iesi.Collections"
    depend "nhibernate.linq"           >> "NHibernate.Linq"

package.category = "ESB"
package.description = "MassTransit is lean service bus implementation for building loosely coupled applications using the .NET framework"
package.forum  = "http://groups.google.com/group/masstransit-discuss"
package.homepage = "http://code.google.com/p/masstransit/"
package.name = "MassTransit"
package.notes = ""
