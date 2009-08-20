install boo:
	description "A wrist friendly language fro the CLI"
	get_from svn("http://svn.codehaus.org/boo/boo/trunk/")
	
	build_with nant, buildfile("default.build"), FrameworkVersion35
	
	with:
		tasks rebuild
		
	switches:
		parameters "nosign=false"		
		
	build_root_dir "build"
	
package.homepage = "http://boo.codehaus.org/"
package.forum    = "http://groups.google.com/group/boolang"

	
