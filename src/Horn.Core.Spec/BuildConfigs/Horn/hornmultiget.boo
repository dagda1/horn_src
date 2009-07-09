install horn:
	description "A .NET build and dependency manager"
	
	export:
	  svn("http://scotaltdotnet.googlecode.com/svn/trunk/src/one", to("one"))
	  svn("http://scotaltdotnet.googlecode.com/svn/trunk/src/two")
	
	prebuild:  
	  cmd "dir"
	  cmd "@echo \"hello\""
	
	build_with msbuild, buildfile("src/horn.sln"), FrameworkVersion35
	build_root_dir "Output"
	shared_library "."
	
                
dependencies:
	depend @log4net >> "lib"
	depend @castle  >> "castle.core"
	depend @castle  >> "Castle.DynamicProxy2"
	depend @castle  >> "castle.microKernel"
	depend @castle  >> "castle.windsor"
	
package.homepage = "http://code.google.com/p/scotaltdotnet/"
package.forum    = "http://groups.google.co.uk/group/horn-development?hl=en"
package.contrib  = false