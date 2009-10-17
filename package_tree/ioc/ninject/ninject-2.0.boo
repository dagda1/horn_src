install ninject:    
	get_from git("git://github.com/enkari/ninject.git")    
	build_with nant, buildfile("Ninject.build"), FrameworkVersion35   
	
	switches:
        parameters "build.config=release", "build.platform=net-3.5", "skip.tests=true"

	shared_library "lib"    
	build_root_dir "build/net-3.5/release" 

package.category = "IoC"
package.description = "Ninject: Open source dependency injector for .NET"
package.forum = "http://groups.google.com/group/ninject"
package.homepage = "http://ninject.org/"
package.name = "Ninject"
package.notes = ""
