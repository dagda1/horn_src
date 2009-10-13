install fluentnhibernate:
    get_from svn("http://fluent-nhibernate.googlecode.com/svn/trunk/")
    build_with rake, buildfile("RakeFile"), FrameworkVersion35

    shared_library "tools/NHibernate"
    build_root_dir "build"

dependencies: 
    depend "castle.tools"               >> "Castle.Core"
    depend "castle.tools"               >> "Castle.DynamicProxy2"
    depend "castle.tools"               >> "Castle.DynamicProxy"
    depend "nhibernate.caches"          >> "NHibernate.Caches.SysCache"
    depend "nhibernate"        >> "2.1" >> "NHibernate"
    depend "nhibernate"        >> "2.1" >> "NHibernate.ByteCode.Castle"
    depend "nhibernate"        >> "2.1" >> "Iesi.Collections"

package.category = "ORM"
package.description = "Fluent, XML-less, compile safe, automated, testable mappings for NHibernate "
package.forum = "http://groups.google.com/group/fluent-nhibernate"
package.homepage = "http://fluentnhibernate.org/"
package.name = "Fluent NHibernate"
package.notes = ""
package.version = "1.0.0.0"