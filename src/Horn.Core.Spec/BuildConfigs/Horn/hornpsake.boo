install horn:
	description "A .NET build and dependency manager"
	get_from svn("http://hornget.googlecode.com/svn/trunk/")
	
	build_with psake, buildfile("default.ps1"), FrameworkVersion35
	
	with:
		tasks Compile	

dependencies:
	depend @log4net >> "lib"
	depend @castle  >> "castle.core"
	depend @castle  >> "Castle.DynamicProxy2"
	depend @castle  >> "castle.microKernel"
	depend @castle  >> "castle.windsor"

package.homepage = "http://code.google.com/p/scotaltdotnet/"
package.forum    = "http://groups.google.co.uk/group/horn-development?hl=en"
package.contrib  = false