install json.net:

    get_from svn("https://json.svn.codeplex.com/svn/trunk")
    build_with msbuild, buildfile("src/Newtonsoft.Json.sln"), FrameworkVersion35	

    build_root_dir "."
    shared_library "src/lib"

package.category = "Web"
package.description = "A .NET serialization for converting objects to JSON and vice versa"
package.homepage = "http://james.newtonking.com/pages/json-net.aspx/"
package.name = "JSON.NET"
package.notes = ""
