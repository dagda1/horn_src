install horn:
	description "A .NET build and dependency manager"
	get_from svn("http://scotaltdotnet.googlecode.com/svn/trunk/")
	build_with msbuild, buildfile("src/horn.sln"), FrameworkVersion35	
	
	build_root_dir "."
	shared_library "lib"	

dependencies:
	depend @log4net >>  "log4net"	
	depend @castle  >>  "castle.core"
	depend @castle  >>  "Castle.DynamicProxy2"
	depend @castle  >>  "castle.microKernel"
	depend @castle  >>  "castle.windsor"

package.homepage = "http://code.google.com/p/scotaltdotnet/"
package.forum    = "http://groups.google.co.uk/group/horn-development?hl=en"	
