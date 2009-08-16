install n2cms:
	description "N2 is a lightweight CMS framework to help you build great web sites that anyone can update"
	get_from svn("http://n2cms.googlecode.com/svn/trunk/")
	build_with msbuild, buildfile("build/n2.proj"), FrameworkVersion35
	
	with:
		tasks Deploy
		
	build_root_dir "output"
	shared_library "lib"

dependencies:
	depend "rhino"  				>> "Rhino.Mocks"

package.homepage = "http://n2cms.com/"
package.forum    = "http://www.codeplex.com/n2/Thread/List.aspx"
