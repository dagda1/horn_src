install ninject:    
	get_from svn("http://ninject.googlecode.com/svn/trunk")    
	build_with nant, buildfile("Ninject.build"), FrameworkVersion35     

	switches:        
		parameters "skip.tests=true"     

	with:        
		tasks release,clean,all     

	shared_library "lib"    
	build_root_dir "bin/net-3.5/release" 

dependencies:    
	depend "castle"   >> "Castle.Core"    
	depend "castle"   >> "Castle.DynamicProxy2"    
	depend "castle"   >> "Castle.MicroKernel"    
	depend "castle"   >> "Castle.Windsor"	
	depend "castle"   >> "Castle.Mikrokernel"	
	depend "castle"   >> "Castle.MonoRail.Framework"    
	depend "linfu"    >> "LinFu.DynamicProxy" 

package.category = "IoC"
package.description = "Ninject: Lightning-fast dependency injection for .NET"
package.forum = "http://groups.google.com/group/ninject"
package.homepage = "http://ninject.org/"
package.name = "Ninject"
package.notes = ""
package.version = "1.0.0.0"
