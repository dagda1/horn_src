install log4net:
    get_from svn("http://svn.apache.org/repos/asf/logging/log4net/trunk/")
    build_with nant, buildfile("log4net.build"), FrameworkVersion35

    switches:
        parameters "mandatory=false"

    generate_strong_key

    build_root_dir "bin"
    #build_root_dir "bin/net/2.0/release"

package.category = "Loggers"
package.description = "log4net is a tool to help the programmer output log statements to a variety of output targets"
package.forum = "http://mail-archives.apache.org/mod_mbox/logging-log4net-user/"
package.homepage = "http://logging.apache.org/log4net/index.html"
package.name = "Log4Net"
package.notes = "The Trunk of Log4Net is Broken. Use Package <log4net-1.2.10>"
package.version = "1.0.0.0"
