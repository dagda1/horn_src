using System;
using Boo.Lang;
using Boo.Lang.Compiler.Ast;
using Horn.Core.BuildEngines;
using Horn.Core.Dependencies;
using Horn.Core.SCM;
using Horn.Core.Utils;
using Horn.Core.Utils.Framework;

namespace Horn.Core.Dsl
{
    public abstract class BooConfigReader
    {
        private readonly IBuildMetaData buildMetaData;

        public IBuildMetaData BuildMetaData
        {
            get { return buildMetaData; }
        }

        public virtual PackageMetaData PackageMetaData
        {
            get
            {
                return Global.package;
            }
        }

        public abstract void Prepare();

        [Meta]
        public static Expression build_with(ReferenceExpression builder, MethodInvocationExpression build, ReferenceExpression frameWorkVersion)
        {
            var targetName = builder.Name;

            return new MethodInvocationExpression(
                    new ReferenceExpression(targetName),
                    build.Arguments[0],
                    new StringLiteralExpression(frameWorkVersion.Name)
                );
        }

        [Meta]
        public static Expression data(params StringLiteralExpression[] expressions)
        {
            var arrayExpression = new ArrayLiteralExpression();

            for (var i = 0; i < expressions.Length; i++)
                arrayExpression.Items.Add(expressions[i]);

            return new MethodInvocationExpression(
                new ReferenceExpression("SetData"),
                arrayExpression
            );
        }

        [Meta]
        public static Expression dependencies(MethodInvocationExpression addDependencyMethod)
        {
            return addDependencyMethod;
        }

        [Meta]
        public static Expression get_from(MethodInvocationExpression get)
        {
            return get;
        }

        [Meta]
        public static Expression install(ReferenceExpression expression,
                                                        BlockExpression action)
        {
            var installName = new StringLiteralExpression(expression.Name);

            return new MethodInvocationExpression(
                    new ReferenceExpression("GetInstallerMeta"),
                    installName,
                    action
                );
        }

        [Meta]
        public static Expression parameters(params StringLiteralExpression[] expressions)
        {
            var arrayExpression = new ArrayLiteralExpression();

            for (var i = 0; i < expressions.Length; i++)
                arrayExpression.Items.Add(expressions[i]);

            return new MethodInvocationExpression(
                new ReferenceExpression("SetParameters"),
                arrayExpression
            );
        }

        [Meta]
        public static Expression export(BlockExpression exportUrls)
        {
            var exportList = new ArrayLiteralExpression();

            foreach (var statement in exportUrls.Body.Statements)
            {
                var expression = (MethodInvocationExpression)((ExpressionStatement)statement).Expression;

                var sourceType = expression.Target.ToString();
                var remoteUrl = ((StringLiteralExpression)expression.Arguments[0]).Value;

                MethodInvocationExpression export;

                if (expression.Arguments.Count == 1)
                {
                    export = new MethodInvocationExpression(new ReferenceExpression("ExportData"),
                                                                new StringLiteralExpression(remoteUrl),
                                                                new StringLiteralExpression(sourceType));

                    exportList.Items.Add(export);

                    continue;
                }

                var to = ((StringLiteralExpression)((MethodInvocationExpression)expression.Arguments[1]).Arguments[0]).Value;

                export = new MethodInvocationExpression(new ReferenceExpression("ExportData"),
                                                                new StringLiteralExpression(remoteUrl),
                                                                new StringLiteralExpression(sourceType),
                                                                new StringLiteralExpression(to));

                exportList.Items.Add(export);
            }

            return new MethodInvocationExpression(new ReferenceExpression("ParseExportList"), exportList);
        }

        [Meta]
        public static Expression include(BlockExpression includes)
        {
            var includeList = new ArrayLiteralExpression();

            foreach (var statement in includes.Body.Statements)
            {
                var expression = (MethodInvocationExpression)((ExpressionStatement)statement).Expression;

                var repositoryName = ((ReferenceExpression)expression.Arguments[0]).Name;
                var includePath = ((StringLiteralExpression)((MethodInvocationExpression)expression.Arguments[1]).Arguments[0]).Value;
                var exportPath = ((StringLiteralExpression)((MethodInvocationExpression)expression.Arguments[2]).Arguments[0]).Value; ;

                var repositoryInclude = new MethodInvocationExpression(new ReferenceExpression("RepositoryElement"),
                                                                       new StringLiteralExpression(repositoryName),
                                                                       new StringLiteralExpression(includePath),
                                                                       new StringLiteralExpression(exportPath));

                includeList.Items.Add(repositoryInclude);
            }

            return new MethodInvocationExpression(new ReferenceExpression("ParseIncludes"), includeList);
        }

        [Meta]
        public static Expression prebuild(BlockExpression commands)
        {
            var cmdList = new ArrayLiteralExpression();

            foreach (Statement statement in commands.Body.Statements)
            {
                var expression = (MethodInvocationExpression)((ExpressionStatement)statement).Expression;

                cmdList.Items.Add(new StringLiteralExpression(expression.Arguments[0].ToString().Trim(new char[] { '\'' })));
            }

            return new MethodInvocationExpression(new ReferenceExpression("ParseCommands"), cmdList);
        }

        [Meta]
        public static Expression switches(Expression action)
        {
            return new MethodInvocationExpression(
                    new ReferenceExpression("AddSwitches"),
                    action
                );
        }

        [Meta]
        public static Expression tasks(params ReferenceExpression[] expressions)
        {
            var arrayExpression = new ArrayLiteralExpression();

			for (var i = 0; i < expressions.Length; i++)
				arrayExpression.Items.Add(new StringLiteralExpression(expressions[i].Name));
			
            return new MethodInvocationExpression(
                new ReferenceExpression("SetBuildTargets"),
                arrayExpression
            );
        }

        [Meta]
        public static Expression with(Expression action)
        {
            return new MethodInvocationExpression(
                    new ReferenceExpression("AssignTasks"),
                    action
                );
        }

        public void AddDependencies(string[] dependencies)
        {
            Array.ForEach(dependencies, item =>
                                     {
                                         var dependency = Dependency.Parse(item);

                                         buildMetaData.BuildEngine.Dependencies.Add(dependency);
                                     });
        }

        public void AddSwitches(Action parametersDelegate)
        {
            parametersDelegate();
        }

        public void AssignTasks(Action tasksDelegate)
        {
            tasksDelegate();
        }

        public void description(string text)
        {
            buildMetaData.Description = text;
        }

        public void GetInstallerMeta(string installName, Action installDelegate)
        {
            buildMetaData.InstallName = installName;

            installDelegate();
        }

        public void build_root_dir(string path)
        {
            buildMetaData.BuildEngine.BuildRootDirectory = path;
        }

        public void ParseCommands(string[] cmdList)
        {
            buildMetaData.PrebuildCommandList.AddRange(cmdList);
        }

        public void ParseExportList(ExportData[] exports)
        {
            foreach (var exportData in exports)
                buildMetaData.ExportList.Add(exportData.SourceControl);
        }

        public virtual void ParseIncludes(RepositoryElement[] elements)
        {
            buildMetaData.RepositoryElementList.AddRange(elements);
        }

        protected void msbuild(string buildFile, string frameWorkVersion)
        {
            var version = (FrameworkVersion)Enum.Parse(typeof(FrameworkVersion), frameWorkVersion);

            SetBuildEngine(new MSBuildBuildTool(), buildFile, version);
        }

        protected void nant(string buildFile, string frameWorkVersion)
        {
            var version = (FrameworkVersion)Enum.Parse(typeof(FrameworkVersion), frameWorkVersion);

            SetBuildEngine(new NAntBuildTool(), buildFile, version);
        }

        protected void rake(string buildFile, string frameWorkVersion)
        {
            var version = (FrameworkVersion)Enum.Parse(typeof(FrameworkVersion), frameWorkVersion);

            SetBuildEngine(new RakeBuildTool(new EnvironmentVariable()), buildFile, version);
        }

        protected void SetBuildTargets(string[] taskActions)
        {
            buildMetaData.BuildEngine.AssignTasks(taskActions);
        }

        protected void SetParameters(string[] parameters)
        {
            buildMetaData.BuildEngine.AssignParameters(parameters);
        }

        protected void svn(string url)
        {
            buildMetaData.SourceControl = SourceControl.Create<SVNSourceControl>(url);
        }

        protected void git(string url)
        {
            buildMetaData.SourceControl = SourceControl.Create<GitSourceControl>(url);
        }

		protected void hg(string url)
		{
			buildMetaData.SourceControl = SourceControl.Create<MercurialSourceControl>(url);
		}

        private void SetBuildEngine(IBuildTool tool, string buildFile, FrameworkVersion version)
        {
            buildMetaData.BuildEngine = new BuildEngine(tool, buildFile, version, IoC.Resolve<IDependencyDispatcher>());
        }

        public virtual void generate_strong_key()
        {
            buildMetaData.BuildEngine.GenerateStrongKey = true;
        }

        public void shared_library(string sharedLib)
        {
            buildMetaData.BuildEngine.SharedLibrary = sharedLib;
        }

        protected BooConfigReader()
        {
            buildMetaData = new BuildMetaData();

            Global.package.PackageInfo.Clear();
        }
    }
}