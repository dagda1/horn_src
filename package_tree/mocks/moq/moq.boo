install moq:
    get_from svn("http://moq.googlecode.com/svn/trunk/")
    build_with msbuild, buildfile("Moq.sln"), FrameworkVersion35

    with:
        tasks full

    build_root_dir "."
    shared_library "Lib"

package.category = "Mocks"
package.description = "The simplest mocking library for .NET 3.5 and Silverlight with deep C# 3.0 integration."
package.forum = "http://groups.google.com/group/moqdisc"
package.homepage = "http://code.google.com/p/moq/"
package.name = "Moq"
package.notes = ""
