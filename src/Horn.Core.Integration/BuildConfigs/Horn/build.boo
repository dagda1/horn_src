install horn:
	description "A .NET build and dependency manager"
	get_from svn("http://scotaltdotnet.googlecode.com/svn/trunk/")
	build_with msbuild, buildfile("src/horn.sln"), frameworkVersion35
	
dependencies:
	depend @log4net >> @lib	
