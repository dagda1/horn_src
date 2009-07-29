install subsonic:
	description "SubSonic is A Super High-fidelity Batman Utility Belt that works up your Data Access (using Linq in 3.0), throws in some much-needed utility functions, and generally speeds along your dev cycle."
	get_from git("git://github.com/subsonic/SubSonic-3.0.git")
	build_with msbuild, buildfile("SubSonic.Linq.sln"), FrameworkVersion35	

	shared_library "lib"
	build_root_dir "Build"		
	
package.homepage = "http://subsonicproject.com/"
package.forum    = "http://stackoverflow.com/questions/tagged/subsonic"
