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
            //HACK: Need a better Expression type for pass a list of strings into a method
            var stringExpression = new StringLiteralExpression(string.Format("{0}|{1}",
                                                    binaryExpression.Left.ToString().Trim('\''),
                                                    binaryExpression.Right.ToString().Trim('\'')));

            dependencies.Items.Add(stringExpression);
        }
    }
}