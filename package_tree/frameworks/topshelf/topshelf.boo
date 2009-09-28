install topshelf:
    get_from svn("http://topshelf.googlecode.com/svn/trunk")
    build_with batch, buildfile("build.bat"), FrameworkVersion35
    with:
        tasks compileandcopytobuild

    shared_library "lib"
    build_root_dir "build_output"

dependencies:
    depend "magnum"                  >> "Magnum"

package.category = "Frameworks"
package.description = "A simple micro-framework for building and testing winservice type code"
package.forum = "http://groups.google.com/group/masstransit"
package.homepage = "http://code.google.com/p/masstransit/"
package.name = "Topshelf"
package.notes = ""
package.version = "1.0.0.0"
