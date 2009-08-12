install dndns:
	description "A DNS protocol library"
	get_from svn("https://dndns.svn.codeplex.com/svn")
	
	build_with msbuild, buildfile("SourceCode/DnDns/DnDns.sln"), FrameworkVersion35		
		
	build_root_dir "build"
	
package.homepage = "http://dndns.codeplex.com/"
package.forum    = "http://dndns.codeplex.com/Thread/List.aspx"