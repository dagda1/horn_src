install linfu:
    get_from svn("http://linfu.googlecode.com/svn/trunk/")
    build_with nant, buildfile("LinFu.build"), FrameworkVersion35

    with:
        tasks clean, compile, dist

    build_root_dir "build"

package.category = "IoC"
package.description = "The LinFu Framework is a set of libraries that extend the CLR."
package.forum = "http://groups.google.com/group/linfuframework"
package.homepage = "http://code.google.com/p/linfu/"
package.name = "LinFu"
package.notes = ""
package.version = "1.0.0.0"
