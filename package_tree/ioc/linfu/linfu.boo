install linfu:
	description "The LinFu Framework is a set of libraries that extend the CLR."
	get_from svn("http://linfu.googlecode.com/svn/trunk/")
	
	build_with nant, buildfile("LinFu.build"), FrameworkVersion35
	
	with:
		tasks clean, compile, dist
		
	build_root_dir "build"
	
package.homepage = "http://code.google.com/p/linfu/"
package.forum    = "http://groups.google.com/group/linfuframework"
