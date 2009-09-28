install magnum:
    get_from svn("http://magnum.googlecode.com/svn/trunk")
    build_with batch, buildfile("build.bat"), FrameworkVersion35

    shared_library "bin"
    build_root_dir "build"


package.category = "Frameworks"
package.description = "For the larger than average developer"
package.forum = "http://groups.google.com/group/masstransit"
package.homepage = "http://code.google.com/p/masstransit/"
package.name = "Magnum"
package.notes = ""
package.version = "1.0.0.0"
