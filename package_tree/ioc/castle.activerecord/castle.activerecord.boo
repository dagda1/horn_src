install castle.activerecord:
	description "The Castle ActiveRecord project is an implementation of the ActiveRecord pattern for .NET."
		
	include:
		repository(castle, part("SharedLibs"), to("SharedLibs"))
		repository(castle, part("ActiveRecord"), to("ActiveRecord"))
		repository(castle, part("common.xml"), to("common.xml"))
		repository(castle, part("common-project.xml"), to("common-project.xml"))
		repository(castle, part("CastleKey.snk"), to("CastleKey.snk"))
		
	build_with nant, buildfile("ActiveRecord/activerecord.build"), FrameworkVersion35
	
	switches:
		parameters "sign=true","common.testrunner.enabled=false", "common.silverlight=false"
		
	shared_library "SharedLibs"
	build_root_dir "build"
	
dependencies:
	depend "castle.tools"        >> "Castle.Core"
	depend "castle.tools"        >> "Castle.DynamicProxy2"
	depend "castle.components"   >> "Castle.Components.Validator"
	#depend "nhibernate.search"   >> "NHibernate.Search"       
	depend "nhibernate"		     >> "2.1" >> "NHibernate"
	depend "nhibernate"		     >> "2.1" >> "NHibernate.ByteCode.Castle"
	depend "nhibernate"          >> "2.1" >> "Iesi.Collections" 	
	
package.homepage = "http://www.castleproject.org/"
package.forum    = "http://groups.google.co.uk/group/castle-project-users?hl=en"  