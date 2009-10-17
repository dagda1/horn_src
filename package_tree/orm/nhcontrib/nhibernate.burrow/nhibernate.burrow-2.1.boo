install nhibernate.burrow:
    get_from svn("https://nhcontrib.svn.sourceforge.net/svnroot/nhcontrib/trunk/src/NHibernate.Burrow/")
    build_with nant, buildfile("default.build"), FrameworkVersion2 

    switches:
        parameters "skip.tests=true"

    with:
        tasks clean,build

    shared_library "lib"
    build_root_dir "build"

dependencies:
    depend "castle.tools"          >> "Castle.Core"
    depend "castle.tools"          >> "Castle.DynamicProxy2"
    depend "nhibernate"   >> "2.1" >> "NHibernate"
    depend "nhibernate"   >> "2.1" >> "Iesi.Collections"
    depend "nhibernate"   >> "2.1" >> "NHibernate.ByteCode.Castle"
    depend @log4net       >> "1.2.10" >> "log4net"

package.category = "ORM"
package.description = "NHibernate.Burrow is a light weight middleware developed to support .Net applications using NHibernate"
package.forum = "http://groups.google.co.uk/group/nhusers?hl=en"
package.homepage = "http://www.ohloh.net/p/NHibernateContrib"
package.name = "NHibernate Burrrow"
package.notes = ""
