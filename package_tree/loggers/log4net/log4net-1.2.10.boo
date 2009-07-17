install log4net:
	description "log4net is a tool to help the programmer output log statements to a variety of output targets"
	get_from svn("http://svn.apache.org/repos/asf/logging/log4net/tags/log4net-1.2.10/")
	
	build_with nant, buildfile("log4net.build"), FrameworkVersion2
	
	switches:
		parameters "mandatory=false"
		
	generate_strong_key
		
	build_root_dir "bin"
	#build_root_dir "bin/net/2.0/release"
	
package.homepage = "http://logging.apache.org/log4net/index.html"
package.forum    = "http://mail-archives.apache.org/mod_mbox/logging-log4net-user/"

	
