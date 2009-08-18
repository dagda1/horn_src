install phantom:
	description "A .NET build system written in C# and Boo"
	get_from git("git://github.com/JeremySkinner/Phantom.git")
	
	build_with phantom, buildfile("build.boo"), FrameworkVersion35	

	with:
		tasks compile,deploy

	shared_library "lib"
	build_root_dir "build"

package.homepage = "http://github.com/JeremySkinner/Phantom/tree/master"
package.forum    = "http://github.com/JeremySkinner/Phantom/"
