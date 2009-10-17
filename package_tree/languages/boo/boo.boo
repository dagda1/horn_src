install boo:
    get_from svn("http://svn.codehaus.org/boo/boo/trunk/")
    build_with nant, buildfile("default.build"), FrameworkVersion35
    with:
        tasks rebuild

    switches:
        parameters "nosign=false"

    build_root_dir "build"

package.category = "Languages"
package.description = "A wrist friendly language fro the CLI"
package.forum = "http://groups.google.com/group/boolang"
package.homepage = "http://boo.codehaus.org/"
package.name = "Boo"
package.notes = ""
