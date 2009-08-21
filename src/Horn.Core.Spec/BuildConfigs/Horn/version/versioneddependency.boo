install castle.tools:
	description "Dynamic Proxy Generator for the CLR."
	
	prebuild:
		cmd "xcopy /s /y \"../Patch\" ."
		
	include:  
		repository(castle, part("Core"), to("Core"))
		repository(castle, part("ActiveRecord"), to("ActiveRecord"))
		repository(castle, part("Facilities"), to("Facilities"))
		repository(castle, part("Services"), to("Services"))
		repository(castle, part("common.xml"), to("common.xml"))
		repository(castle, part("common-project.xml"), to("common-project.xml"))
		repository(castle, part("CastleKey.snk"), to("CastleKey.snk"))

	build_with nant, buildfile("Facilities/facilities.build"), FrameworkVersion35
		
	switches:
		parameters "sign=true","common.testrunner.enabled=false", "common.silverlight=false"
		
	shared_library "SharedLibs/net/2.0"
	build_root_dir "build"
	   
dependencies:
	depend "castle.tools"   >> "Castle.DynamicProxy"
	depend "castle.tools"   >> "Castle.DynamicProxy2"
	depend "castle.windsor" >> "Castle.MicroKernel"
	depend "castle.windsor" >> "Castle.MicroKernel"
	depend "nhibernate">> "2.1" >> "NHibernate"       
	depend "nhibernate"     >> "2.1" >> "Iesi.Collections" 
	
package.homepage = "http://www.castleproject.org/"
package.forum    = "http://groups.google.co.uk/group/castle-project-users?hl=en"    