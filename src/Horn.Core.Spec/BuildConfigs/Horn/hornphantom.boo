install horn:
	description "A .NET build and dependency manager"
	get_from svn("http://hornget.googlecode.com/svn/trunk/")
	
	build_with phantom, buildfile("src/horn.boo"), FrameworkVersion35
	with:
		tasks build, release, quick, rebuild

package.homepage = "http://code.google.com/p/scotaltdotnet/"
package.forum    = "http://groups.google.co.uk/group/horn-development?hl=en"