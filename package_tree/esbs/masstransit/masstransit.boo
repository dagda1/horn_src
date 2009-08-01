install masstransit:
	description "lite weight ESB"
	get_from svn("http://masstransit.googlecode.com/svn/trunk/")
	
	build_with nant, buildfile("BuildScripts/masstransit.build"), FrameworkVersion35	
		
	shared_library "libs"
	build_root_dir "bin"

dependencies:
	depend "castle.tools"		 >> "Castle.Core"
	depend "castle.tools" 		 >> "Castle.DynamicProxy2"
	depend "castle.windsor" 	 >> "Castle.MicroKernel"
	depend "castle.windsor" 	 >> "Castle.Windsor"
	depend "nhibernate"		     >> "2.1" >> "NHibernate"     
	depend "nhibernate"		     >> "2.1" >> "NHibernate.ByteCode.Castle"
	depend "nhibernate"          >> "2.1" >> "Iesi.Collections"
	depend "fluentnhibernate"	 >> "FluentNhibernate"
	depend "nhibernate.linq"	 >> "Nhibernate.Linq"

package.homepage = "http://code.google.com/p/masstransit/"
package.forum    = "http://groups.google.com/group/masstransit-discuss"
