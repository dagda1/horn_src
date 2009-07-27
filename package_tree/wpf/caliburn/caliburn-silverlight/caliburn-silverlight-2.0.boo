install caliburn:
	description "Designed to aid in the development of WPF and Silverlight applications, Caliburn implements a variety of UI patterns for solving real-world problems. Patterns that are enabled by the framework include MVC, MVP, Presentation Model (MVVM), Commands and Application Controller."
	get_from svn("https://caliburn.svn.codeplex.com/svn")
	build_with nant, buildfile("default.build"), FrameworkVersion35	
		
	switches:
		parameters "skip.tests=true"

	with:
		tasks clean,release,platformSilverlight20

	shared_library "lib/Silverlight-2.0"
	build_root_dir "bin"		
	
package.homepage = "http://caliburn.codeplex.com/"
package.forum    = "http://caliburn.codeplex.com/Thread/List.aspx"
