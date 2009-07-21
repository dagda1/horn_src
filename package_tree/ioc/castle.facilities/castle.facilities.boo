install castle.facilities:
	description "A castle facility augments the container with new functionality."
		
	include:  
		repository(castle, part("SharedLibs"), to("SharedLibs"))
		repository(castle, part("Facilities"), to("Facilities"))
		repository(castle, part("common.xml"), to("common.xml"))
		repository(castle, part("common-project.xml"), to("common-project.xml"))
		repository(castle, part("CastleKey.snk"), to("CastleKey.snk"))

	build_with nant, buildfile("Facilities/facilities.build"), FrameworkVersion35
		
	switches:
		parameters "sign=true","common.testrunner.enabled=false", "common.silverlight=false"
		
	shared_library "SharedLibs"
	build_root_dir "build"		
	   
dependencies:
	depend "castle.windsor"      >> "Castle.Core"
	depend "castle.windsor"      >> "Castle.DynamicProxy2"
	depend "castle.windsor"      >> "Castle.MicroKernel"
	depend "castle.windsor"      >> "Castle.Windsor"
	depend "castle.activerecord" >> "Castle.ActiveRecord"
	depend "castle.services"     >> "Castle.Services.Transaction"
	depend "castle.services"     >> "Castle.Services.Logging.Log4netIntegration"
	depend "castle.services"     >> "Castle.Services.Logging.NLogIntegration"
	depend "nhibernate"		     >> "2.1" >> "NHibernate"       
	depend "nhibernate"          >> "2.1" >> "Iesi.Collections" 
	
package.homepage = "http://www.castleproject.org/"
package.forum    = "http://groups.google.co.uk/group/castle-project-users?hl=en"    