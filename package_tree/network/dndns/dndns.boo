install dndns:
    get_from svn("https://dndns.svn.codeplex.com/svn")
    build_with msbuild, buildfile("SourceCode/DnDns/DnDns.sln"), FrameworkVersion35		

    build_root_dir "build"

package.category = "Network"
package.description = "A DNS protocol library"
package.forum = "http://dndns.codeplex.com/Thread/List.aspx"
package.homepage = "http://dndns.codeplex.com/"
package.name = "DnDns"
package.notes = ""
package.version = "1.0.0.0"