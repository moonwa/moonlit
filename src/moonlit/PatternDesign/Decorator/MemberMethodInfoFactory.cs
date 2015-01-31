using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.CodeDom;

namespace Moonlit.PatternDesign {
    /// <summary>
    /// 
    /// </summary>
    public class MemberMethodInfoFactory : IMemberInfoFactory {

        #region IMemberInfoFactory Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <param name="inner"></param>
        /// <param name="attrs"></param>
        /// <returns></returns>
        public CodeTypeMember CreateMember(MemberInfo member, CodeFieldReferenceExpression inner, MemberAttributes attrs) {
            Debug.Assert(member is MethodInfo);
            MethodInfo method = member as MethodInfo;
            CodeMemberMethod codeMethod = new CodeMemberMethod();

            codeMethod.Name = method.Name;
            codeMethod.ReturnType = new CodeTypeReference(method.ReturnType);
            codeMethod.Attributes = attrs;

            // try
            CodeTryCatchFinallyStatement tryCode = new CodeTryCatchFinallyStatement();

            // decleare parameters
            List<CodeArgumentReferenceExpression> codeParamiteRefrs = new List<CodeArgumentReferenceExpression>();

            foreach (ParameterInfo codeParameter in method.GetParameters()) {
                CodeParameterDeclarationExpression codeParameterDeclare = new CodeParameterDeclarationExpression(codeParameter.ParameterType, codeParameter.Name);
                codeMethod.Parameters.Add(codeParameterDeclare);
                codeParamiteRefrs.Add(new CodeArgumentReferenceExpression(codeParameter.Name));
            }

            // invoke
            CodeMethodInvokeExpression invokeMethod = new CodeMethodInvokeExpression(
                inner, method.Name, codeParamiteRefrs.ToArray());
            if (method.ReturnType.Name.ToLower() == "void") {
                tryCode.TryStatements.Add(invokeMethod);
            } else {
                CodeVariableDeclarationStatement var = new CodeVariableDeclarationStatement(method.ReturnType, "returnObject", invokeMethod);
                //CodeAssignStatement assign = new CodeAssignStatement(var, invokeMethod);
                tryCode.TryStatements.Add(var);

                CodeCommentStatement todo = new CodeCommentStatement("TODO: your code", false);
                tryCode.TryStatements.Add(todo);

                CodeVariableReferenceExpression varRef = new CodeVariableReferenceExpression("returnObject");
                CodeMethodReturnStatement codeReturn = new CodeMethodReturnStatement(varRef);
                tryCode.TryStatements.Add(codeReturn);
            }

            // catch
            CodeTypeReference codeTypeRef = new CodeTypeReference(typeof(Exception));
            CodeCatchClause catchClause = new CodeCatchClause("ex", codeTypeRef);
            catchClause.Statements.Add(new CodeThrowExceptionStatement());
            tryCode.CatchClauses.Add(catchClause);

            codeMethod.Statements.Add(tryCode);
            return codeMethod;
        }

        #endregion
    }
}
