install ncommon:
	description "ncommon framework."	
	
	get_from svn("https://ncommon.svn.codeplex.com/svn/trunk/")
	
	build_with msbuild, buildfile("NCommon Everything.sln"), FrameworkVersion35
		
	switches:
		parameters "build.warnaserrors=false","common.testrunner.enabled=false","sign=true"
		
	shared_library "Libs"
	build_root_dir "."		
	
dependencies:   
	depend @log4net              >> "1.2.10" >>  "log4net"
	depend "castle.tools"        >> "Castle.DynamicProxy"
	depend "castle.tools"        >> "Castle.DynamicProxy2"
	depend "nhibernate"	     >> "2.1" >> "NHibernate"
	depend "nhibernate"	     >> "2.1" >> "LinFu.DynamicProxy"     
	depend "nhibernate"	     >> "2.1" >> "NHibernate.ByteCode.Castle"
	depend "nhibernate"          >> "2.1" >> "Iesi.Collections"
	depend "nhibernate"	     >> "2.1" >> "NHibernate.ByteCode.LinFu"
	depend "nhibernate.linq"     >> "2.1" >> "nhibernate.linq"
	depend "fluentnhibernate"    >> "FluentNHibernate"
