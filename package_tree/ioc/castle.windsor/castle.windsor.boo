install castle.windsor:
	description "Windsor is an inversion of control container that aggregates the MicroKernel offering a friendly fool-proof interface and options to external configurations."
	
	prebuild:
		cmd "xcopy /s /y \"../Patch\" ."
		
	include:
		repository(castle, part("InversionOfControl"), to("InversionOfControl"))
		repository(castle, part("common.xml"), to("common.xml"))
		repository(castle, part("common-project.xml"), to("common-project.xml"))
		repository(castle, part("CastleKey.snk"), to("CastleKey.snk"))

	build_with nant, buildfile("InversionOfControl/InversionOfControl.build"), FrameworkVersion35
		
	switches:
		parameters "sign=true","common.testrunner.enabled=false", "common.silverlight=false"
		
	shared_library "SharedLibs/net/2.0"
	build_root_dir "build"
	
dependencies:
	dependency "castle.tools" >> "castle.core"
	dependency "castle.tools" >> "Castle.DynamicProxy2"
	
package.homepage = "http://www.castleproject.org/"
package.forum    = "http://groups.google.co.uk/group/castle-project-users?hl=en"  