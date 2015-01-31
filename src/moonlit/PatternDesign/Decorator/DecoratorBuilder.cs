using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.CodeDom;

namespace Moonlit.PatternDesign
{
    /// <summary>
    /// 装饰者构建器
    /// </summary>
    public abstract class DecoratorBuilder
    {
        Type _targetType;
        string _innerTypeName;
        public MemberAttributes MethodAttributes{ get; set; }
        public string Namespace { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="DecoratorBuilder"/> class.
        /// </summary>
        /// <param name="targetType">为该类型创建对应的装饰者.</param>
        public DecoratorBuilder(Type targetType)
        {
            _targetType = targetType;
            _innerTypeName = "_inner" + this.GetTypeName(targetType);
            this.Namespace = "mww";
        }

        /// <summary>
        /// 构建一个代码编译单元
        /// </summary>
        /// <returns></returns>
        public CodeCompileUnit Build()
        {
            CodeCompileUnit unit = new CodeCompileUnit();
            CodeNamespace codeNs = CreateNamespace(unit);
            ImportNamespaces(codeNs);
            CodeTypeDeclaration typeDec = CreateType(codeNs);

            CreateFields(typeDec);
            CreateConstructor(typeDec);

            foreach (var member in this.GetMustInheritMembers(_targetType).OrderBy(m => m.Name))
            {
                IMemberInfoFactory factory = MemberInfoFactoryFactory.Create(member);
                if (factory != null)
                {
                    typeDec.Members.Add(factory.CreateMember(member, new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), _innerTypeName), this.MethodAttributes));
                }
            }
            return unit;
        }

        private static void ImportNamespaces(CodeNamespace codeNs)
        {
            codeNs.Imports.Add(new CodeNamespaceImport("System"));
            codeNs.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
        }

        private void CreateFields(CodeTypeDeclaration typeDec)
        {
            CodeMemberField codeField = new CodeMemberField(_targetType, _innerTypeName);
            typeDec.Members.Add(codeField);
        }

        private void CreateConstructor(CodeTypeDeclaration typeDec)
        {
            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public;
            string paramName = "inner" + this.GetTypeName(_targetType);
            constructor.Parameters.Add(new CodeParameterDeclarationExpression(this._targetType, paramName));
            CodeAssignStatement setter = new CodeAssignStatement();
            setter.Left = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), _innerTypeName);
            setter.Right = new CodeArgumentReferenceExpression(paramName);
            constructor.Statements.Add(setter);
            typeDec.Members.Add(constructor);
        }

        private CodeTypeDeclaration CreateType(CodeNamespace codeNs)
        {
            CodeTypeDeclaration typeDec = new CodeTypeDeclaration(this.GetTypeName(_targetType));
            typeDec.Attributes = MemberAttributes.Public;
            codeNs.Types.Add(typeDec);
            return typeDec;
        }

        private CodeNamespace CreateNamespace(CodeCompileUnit unit)
        {
            CodeNamespace codeNs = new CodeNamespace(this.Namespace);
            unit.Namespaces.Add(codeNs);
            return codeNs;
        }
        /// <summary>
        /// 获得类型的名称.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        protected abstract string GetTypeName(Type targetType);
        /// <summary>
        /// 获取必须要实现的成员.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        protected abstract List<MemberInfo> GetMustInheritMembers(Type targetType);
        class MemberInfoFactoryFactory
        {
            public static IMemberInfoFactory Create(MemberInfo member)
            {
                if (member is MethodInfo)
                {
                    return new MemberMethodInfoFactory();
                }
                return null;
            }
        }
    }

}
