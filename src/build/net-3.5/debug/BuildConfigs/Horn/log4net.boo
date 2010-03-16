install horn:
	description "log4net is a tool to help the programmer output log statements to a variety of output targets. log4net"
	get_from svn("http://svn.apache.org/repos/asf/logging/log4net/trunk/")
	build_with msbuild, buildfile("src/log4net.vs2005.sln"), FrameworkVersion35
