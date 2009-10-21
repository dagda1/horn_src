install castle.templateengine:
    get_from svn("https://svn.castleproject.org/svn/castle/Components/TemplateEngine/trunk/")
    build_with nant, buildfile("default.build"), FrameworkVersion35	

    with:
        tasks build

    shared_library "lib"
    build_root_dir "build"

dependencies:
    depend "castle.nvelocity"   >> "NVelocity"

package.category = "TemplateEngines"
package.description = "The TemplateEngine component is used to abstract template processing approaches. It is useful to provide a minimal template processing to your applications. It comes with a default implementation which relies on NVelocity."
package.forum = "http://groups.google.com/group/castle-project-users"
package.homepage = "http://www.castleproject.org/components/templateengine/index.html"
package.name = "Castle TemplateEngine Component"
package.notes = ""