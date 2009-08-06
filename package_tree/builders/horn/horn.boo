install horn:
	description "A .NET build and dependency manager"
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

package.homepage = "http://code.google.com/p/hornget/"
package.forum    = "http://groups.google.co.uk/group/horn-development?hl=en"	
