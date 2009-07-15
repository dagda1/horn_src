using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Steps;

namespace Horn.Core.Dsl
{
    public class RightShiftToMethodCompilerStep : AbstractTransformerCompilerStep
    {
        public override void OnBlockExpression(BlockExpression node)
        {
            var dependencies = new ArrayLiteralExpression();

            foreach (Statement statement in node.Body.Statements)
            {
                var expressionStatement = (ExpressionStatement)statement;
                var expression = (MethodInvocationExpression)expressionStatement.Expression;

                OnMethodInvocationExpression(dependencies, expression);
            }

            if (dependencies.Items.Count == 0)
                return;

            var referenceExpression = new ReferenceExpression("AddDependencies");
            var replacementMethod = new MethodInvocationExpression(referenceExpression, dependencies);

            ReplaceCurrentNode(replacementMethod);
        }

        public override void Run()
        {
            Visit(CompileUnit);
        }



        protected virtual void OnMethodInvocationExpression(ArrayLiteralExpression dependencies, MethodInvocationExpression expression)
        {
            foreach (var arg in expression.Arguments)
            {
                var binaryExpression = arg as BinaryExpression;
                if (binaryExpression == null || binaryExpression.Operator != BinaryOperatorType.ShiftRight)
                    continue;

                AddDependency(dependencies, binaryExpression);
            }
        }

        protected virtual void AddDependency(ArrayLiteralExpression dependencies, BinaryExpression binaryExpression)
        {
            StringLiteralExpression dependency;

            //HACK: replace with proper AST method invocation
            if (binaryExpression.Left is StringLiteralExpression)
            {
                dependency = new StringLiteralExpression(string.Format("{0}|{1}",
                                                    binaryExpression.Left.ToString().Trim('\''),
                                                    binaryExpression.Right.ToString().Trim('\'')));                
            }
            else if(binaryExpression.Left is BinaryExpression)
            {

                var left = (BinaryExpression) binaryExpression.Left;

                var package = left.Left.ToString().Trim('\'');
                var version = left.Right.ToString().Trim('\'');
                var dll = binaryExpression.Right.ToString().Trim('\'');

                dependency = new StringLiteralExpression(string.Format("{0}|{1}|{2}",
                                                    package,
                                                    dll,
                                                    version));
            }
            else
                throw new ArgumentOutOfRangeException(string.Format("Unkonwn Expression type {0} passed to RightShiftToMethodCompilerStep.AddDependency", binaryExpression.Left.GetType().Name));


            dependencies.Items.Add(dependency);
        }
    }
}