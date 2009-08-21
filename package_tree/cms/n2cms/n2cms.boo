install n2cms:
    get_from svn("http://n2cms.googlecode.com/svn/trunk/")
    build_with msbuild, buildfile("build/n2.proj"), FrameworkVersion35

    switches:
        parameters "/p:DefineConstants=NH2_1"

    with:
        tasks Deploy

    build_root_dir "output"
    shared_library "lib"

dependencies:
    depend "nhibernate"            >> "2.1" >> "NHibernate
    depend "nhibernate"            >> "2.1" >> "NHibernate.ByteCode.Castle"
    depend "nhibernate"            >> "2.1" >> "Iesi.Collections"
    depend "nhibernate.linq"       >> "2.1" >> "NHibernate.Linq"
    depend "nhibernate.jetdriver"  >> "2.1" >> "NHibernate.JetDriver"
    depend "nhibernate.caches"              >> "NHibernate.Caches.SysCache2"
    depend "castle.windsor"                 >> "castle.core"
    depend "castle.windsor"                 >> "Castle.DynamicProxy2"
    depend "castle.windsor"                 >> "castle.microKernel"
    depend "castle.windsor"                 >> "castle.windsor"
    depend "rhino"                          >> "Rhino.Mocks"
    depend "mvccontrib"                     >> "MvcContrib"
    depend @log4net                >> "1.2.10"  >> "log4net"

package.category = "CMS"
package.description = "N2 is a lightweight CMS framework to help you build great web sites that anyone can update"
package.forum  = "http://www.codeplex.com/n2/Thread/List.aspx"
package.homepage = "http://n2cms.com/"
package.name = "N2CMS"
package.notes = ""
package.version = "1.0.0.0"
