install nhibernate.burrow:
	description "NHibernate.Burrow is a light weight middleware developed to support .Net applications using NHibernate."
	
	get_from svn("https://nhcontrib.svn.sourceforge.net/svnroot/nhcontrib/trunk/src/NHibernate.Burrow/")
	build_with nant, buildfile("default.build"), FrameworkVersion2 

	switches:
		parameters "skip.tests=true"
		
	with:
		tasks clean,build

	shared_library "lib"
	build_root_dir "build"		
	
dependencies:
	depend "castle.tools" >> "Castle.Core"
	depend "castle.tools" >> "Castle.DynamicProxy2"
	depend "nhibernate"   >> "NHibernate"       
	depend "nhibernate"   >> "Iesi.Collections" 
	depend "nhibernate"   >> "NHibernate.ByteCode.Castle"
	depend @log4net >>  "1.2.10" >>  "log4net"	

package.homepage = "http://www.ohloh.net/p/NHibernateContrib"
package.forum    = "http://groups.google.co.uk/group/nhusers?hl=en"
