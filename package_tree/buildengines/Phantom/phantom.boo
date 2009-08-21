install phantom:
    get_from git("git://github.com/JeremySkinner/Phantom.git")
    build_with phantom, buildfile("build.boo"), FrameworkVersion35	

    with:
        tasks compile,deploy

    shared_library "lib"
    build_root_dir "build"

package.category = "Build Engines"
package.description = "A .NET build system written in C# and Boo"
package.forum = "http://github.com/JeremySkinner/Phantom/"
package.homepage = "http://github.com/JeremySkinner/Phantom/tree/master"
package.name = "Phantom"
package.notes = ""
package.version = "1.0.0.0"
