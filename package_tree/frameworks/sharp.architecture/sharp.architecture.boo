install sharp.architecture:
    get_from svn("http://sharp-architecture.googlecode.com/svn/trunk")
    build_with nant, buildfile("SharpArch.patch.build"), FrameworkVersion35
    with:
        tasks compileandcopytobuild

    shared_library "bin"
    build_root_dir "build"

dependencies:
    depend "castle.tools"                  >> "Castle.Core"
    depend "castle.tools"                  >> "Castle.DynamicProxy2"
    depend "castle.windsor"                >> "Castle.MicroKernel"
    depend "castle.windsor"                >> "Castle.Windsor"
    depend "castle.services"               >> "Castle.Services.Logging.Log4netIntegration"
    depend "castle.components"             >> "Castle.Components.Validator"
    depend "fluentnhibernate"              >> "FluentNHibernate"
    depend "nhibernate"           >> "2.1" >> "NHibernate"     
    depend "nhibernate"           >> "2.1" >> "NHibernate.ByteCode.Castle"
    depend "nhibernate"           >> "2.1" >> "Iesi.Collections"
    depend "nhibernate.linq"               >> "NHibernate.Linq"
    depend "nhibernate.validator"          >> "NHibernate.Validator"
    depend "mvccontrib"                    >> "MvcContrib"
    depend "mvccontrib"                    >> "MvcContrib.FluentHtml"
    depend "mvccontrib"                    >> "MvcContrib.ModelAttributes"
    depend "mvccontrib"                    >> "MvcContrib.TestHelper"
    depend "mvccontrib"                    >> "MvcContrib.Castle"

package.category = "Frameworks"
package.description = "This is a solid architectural foundation for rapidly building maintainable web applications leveraging the ASP.NET MVC framework with NHibernate."
package.forum = "http://groups.google.com/group/sharp-architecture"
package.homepage = "http://code.google.com/p/sharp-architecture/"
package.name = "Sharp Architecture"
package.notes = ""
package.version = "1.0.0.0"