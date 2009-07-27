install moq:
	description "A mocking framework"
	get_from svn("http://moq.googlecode.com/svn/trunk/")
	
	build_with msbuild, buildfile("Moq.sln"), FrameworkVersion35
	
	with:
		tasks full			
		
	build_root_dir "."
	shared_library "Lib"
	
package.homepage = "http://code.google.com/p/moq/"
package.forum    = "http://groups.google.com/group/moqdisc"

	
