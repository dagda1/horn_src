install rhino:
	description "Rhino tools including Rhino Mocks, Rhino ETL, Binsor etc."
	get_from svn("https://rhino-tools.svn.sourceforge.net/svnroot/rhino-tools/trunk")
	
	build_with nant, buildfile("default.build"), FrameworkVersion35
		
	switches:
		parameters "build.warnaserrors=false","common.testrunner.enabled=false","sign=true"
		
	shared_library "SharedLibs"
	build_root_dir "build"		
	
dependencies:    
	#depend @log4net >> "log4net"
	depend @boo					 >> "Boo.Lang.Extensions"
	depend @boo				     >> "Boo.Lang.Interpreter"
	depend @boo				     >> "Boo.Lang.Parser"
	depend @boo				     >> "Boo.Lang.Useful"
	depend @boo				     >> "Boo.NAnt.Tasks"  
	depend @boo				     >> "Boo.Lang.CodeDom"
	depend @boo				     >> "Boo.Lang.Compiler"	
	depend @boo				     >> "booc"
	depend @boo				     >> "Boo.Lang"
	depend "castle.tools"        >> "Castle.Core"
	depend "castle.tools"        >> "Castle.DynamicProxy"
	depend "castle.tools"		 >> "Castle.DynamicProxy2"
	depend "castle.windsor"      >> "Castle.MicroKernel"
	depend "castle.windsor"      >> "Castle.Windsor"	
	depend "castle.services"     >> "Castle.Services.Transaction"
	depend "castle.services"     >> "Castle.Services.Logging.Log4netIntegration"
	depend "castle.services"     >> "Castle.Services.Logging.NLogIntegration"
	depend "castle.components"   >> "Castle.Components.Validator"
	depend "castle.activerecord" >> "Castle.ActiveRecord"
	depend "castle.facilities"   >> "Castle.Facilities.AutomaticTransactionManagement"
	depend "nhibernate"		     >> "2.1" >> "NHibernate"     
	depend "nhibernate"		     >> "2.1" >> "NHibernate.ByteCode.Castle"
	depend "nhibernate"          >> "2.1" >> "Iesi.Collections"	
	
package.homepage = "http://ayende.com/projects/rhino-mocks.aspx"
package.forum    = "http://groups.google.co.uk/group/rhino-tools-dev"