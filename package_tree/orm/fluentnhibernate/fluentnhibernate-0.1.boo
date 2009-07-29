install FluentNHibernate:
	description "Fluent, XML-less, compile safe, automated, testable mappings for NHibernate "	
	
	get_from svn("http://fluent-nhibernate.googlecode.com/svn/trunk")
	
	build_with rake, buildfile("RakeFile"), FrameworkVersion35

	with:
		tasks compile
		
	switches:
		parameters "build.warnaserrors=false","common.testrunner.enabled=false","sign=true"
		
	shared_library "tools/NHibernate"
	build_root_dir "build"		
	
dependencies:    
	depend "castle.tools" 		 >> "Castle.Core"
	depend "castle.tools" 		 >> "Castle.DynamicProxy2"
	depend "castle.tools" 		 >> "Castle.DynamicProxy"
	depend "nhibernate"		     >> "2.1" >> "NHibernate"     
	depend "nhibernate"		     >> "2.1" >> "NHibernate.ByteCode.Castle"
	depend "nhibernate"          >> "2.1" >> "Iesi.Collections"
	depend "nhibernate.caches"   >> "NHibernate.Caches.SysCache"

package.homepage = "http://fluentnhibernate.org/"
package.forum    = "http://groups.google.com/group/fluent-nhibernate"