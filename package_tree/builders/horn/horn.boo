install horn:
    get_from svn("http://hornget.googlecode.com/svn/trunk/")
    build_with nant, buildfile("src/Horn.build"), FrameworkVersion35	

    build_root_dir "src/build"
    shared_library "lib"	

dependencies:
    depend @log4net >>  "1.2.10" >>  "log4net"	
    depend "castle.windsor"  >>  "castle.core"
    depend "castle.windsor"  >>  "Castle.DynamicProxy2"
    depend "castle.windsor"  >>  "castle.microKernel"
    depend "castle.windsor"  >>  "castle.windsor"

package.category = "Builders"
package.description = "A .NET build and dependency manager"
package.forum = "http://groups.google.co.uk/group/horn-development?hl=en"	
package.homepage = "http://code.google.com/p/hornget/"
package.name = "Horn"
package.notes = ""
package.version = "1.0.0.0"
