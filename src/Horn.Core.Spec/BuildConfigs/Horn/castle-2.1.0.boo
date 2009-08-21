install castle:
	description "Castle is an open source project for .net that aspires to simplify the development of enterprise and web applications."
	get_from svn("http://svn.castleproject.org:8080/svn/castle/trunk/")
	
	build_with nant, buildfile("default.build"), FrameworkVersion35
	
	with:
		tasks quick, rebuild
		
	switches:
		parameters "sign=true","build.warnaserrors=false"
		
	shared_library "SharedLibs/net/2.0"
	build_root_dir "build"
	
package.homepage = "http://www.castleproject.org/"
package.forum    = "http://groups.google.co.uk/group/castle-project-users?hl=en"  