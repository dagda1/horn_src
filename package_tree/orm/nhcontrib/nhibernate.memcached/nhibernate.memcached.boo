install nhcontrib:
	description "Second level cache provider using memcached."
	get_from svn("https://nhcontrib.svn.sourceforge.net/svnroot/nhcontrib/trunk/src/NHibernate.Caches/")
	build_with nant, buildfile("MemCache/default.build"), FrameworkVersion35	
		
	switches:
		parameters "with.examples=false"
		
	shared_library "Lib"
	build_root_dir "build"		
	
dependencies:  
	depend "nhibernate" >> "2.1" >> "NHibernate"     
	depend "nhibernate"	>> "2.1" >> "NHibernate.ByteCode.Castle"
	depend "nhibernate"	>> "2.1" >> "NHibernate.ByteCode.LinFu.dll"
	depend "nhibernate" >> "2.1" >> "Iesi.Collections"

package.homepage = "http://www.ohloh.net/p/NHibernateContrib"
package.forum    = "http://groups.google.co.uk/group/nhusers?hl=en"
