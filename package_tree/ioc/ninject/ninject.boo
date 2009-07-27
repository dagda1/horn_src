install ninject:
	description "Ninject: Lightning-fast dependency injection for .NET"
	get_from svn("http://ninject.googlecode.com/svn/trunk")
	
	build_with nant, buildfile("Ninject.build"), FrameworkVersion35
	
	switches:
		parameters "skip.tests=true"

	with:
		tasks release,clean,all
		
	shared_library "lib"
	build_root_dir "bin/net-3.5/release"

dependencies:
	depend "castle.tools"	>> "Castle.Core"
	depend "castle.tools" >> "Castle.DynamicProxy2"
	depend "castle.windsor" >> "Castle.MicroKernel"
	depend "castle.windsor" >> "Castle.Windsor"
	/*depend "castle.monorail" >> "Castle.MonoRail.Framework"*/
	depend "linfu"	>> "LinFu.DynamicProxy"

package.homepage = "http://ninject.org/"
package.forum    = "http://groups.google.com/group/ninject"
