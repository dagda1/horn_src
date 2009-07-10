install horn:
	description "A .NET build and dependency manager"
	get_from svn("https://hornget.googlecode.com/svn/trunk/")
	
	build_with nant, buildfile("src/horn.build"), FrameworkVersion35
	with:
		tasks build, release, quick, rebuild
	switches:
		parameters "sign=false", "testrunner=NUnit", "common.testrunner.enabled=true", "common.testrunner.failonerror=true", "build.msbuild=true"
		
	generate_strong_key

dependencies:
	depend @log4net >> "lib"
	depend @castle  >> "castle.core"
	depend @castle  >> "Castle.DynamicProxy2"
	depend @castle  >> "castle.microKernel"
	depend @castle  >> "castle.windsor"

package.homepage = "http://code.google.com/p/scotaltdotnet/"
package.forum    = "http://groups.google.co.uk/group/horn-development?hl=en"
package.contrib  = false
