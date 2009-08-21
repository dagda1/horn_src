install nhibernate.validator:
    description "NHibernate Validator."
    get_from svn("https://nhcontrib.svn.sourceforge.net/svnroot/nhcontrib/trunk/src/NHibernate.Validator/")
    build_with nant, buildfile("default.build"), FrameworkVersion35	

    switches:
        parameters "skip.tests=true"

    with:
        tasks clean,build

    generate_strong_key

    shared_library "lib"
    build_root_dir "build"

dependencies:
    depend "castle.tools"          >> "Castle.Core"
    depend "castle.tools           >> "Castle.DynamicProxy2"
    depend "nhibernate"   >> "2.1" >> "NHibernate"
    depend "nhibernate"   >> "2.1" >> "Iesi.Collections"
    depend "nhibernate"   >> "2.1" >> "NHibernate.ByteCode.Castle"

package.category = "ORM"
package.description = "NHibernate Validator."
package.forum = "http://groups.google.co.uk/group/nhusers?hl=en"
package.homepage = "http://www.ohloh.net/p/NHibernateContrib"
package.name = "NHibernate Validator"
package.notes = ""
package.version = "1.0.0.0"
