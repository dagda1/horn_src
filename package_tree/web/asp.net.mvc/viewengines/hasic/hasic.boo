install hasic:
	description "A view engine for ASP.NET MVC that uses VB.NET XML literals."
	get_from svn("http://subversion.assembla.com/svn/hasic/trunk/")
	
	build_with msbuild, buildfile("src/Hasic.sln"), FrameworkVersion35
		
	build_root_dir "."
	shared_library "src/lib"
	
package.homepage = "http://www.assembla.com/wiki/show/hasic"