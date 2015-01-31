using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonlit.CodeDom
{
    public static class CodeExprs
    {
        public static CodePropertyReferenceExpression Property(object target, params string[] properties)
        {
            CodeExpression targetExpr = ToExpression(target);
            CodePropertyReferenceExpression property = null;
            foreach (var propertyName in properties)
            {
                property = new CodePropertyReferenceExpression(targetExpr, propertyName);
                targetExpr = property;
            }
            return property;
        }
        public static CodeFieldReferenceExpression Field(object target, params string[] fields)
        {
            CodeExpression targetExpr = ToExpression(target);
            CodeFieldReferenceExpression field = null;
            foreach (var fieldName in fields)
            {
                field = new CodeFieldReferenceExpression(targetExpr, fieldName);
                targetExpr = field;
            }
            return field;
        }
        public static CodeCastExpression CastThis<T>()
        {
            return Cast<T>(This);
        }
        public static CodeCastExpression Cast<T>(object obj)
        {
            return Cast(typeof(T), obj);
        }
        public static CodeCastExpression Cast(Type type, object obj)
        {
            return new CodeCastExpression(new CodeTypeReference(type), ToExpression(obj));
        }
        public static CodeThisReferenceExpression This
        {
            get { return new CodeThisReferenceExpression(); }
        }

        public static CodeExpression Null
        {
            get { return new CodePrimitiveExpression(null); }
        }

        public static CodeMethodInvokeExpression CallTarget(object target, string methodName, params object[] parameters)
        {
            CodeMethodReferenceExpression method = new CodeMethodReferenceExpression(ToExpression(target), methodName);
            return new CodeMethodInvokeExpression(method, ToExpressions(parameters));
        }
        public static CodeMethodInvokeExpression CallTarget(object target, string methodName, CodeTypeReference[] types, params object[] parameters)
        {
            CodeMethodReferenceExpression method = new CodeMethodReferenceExpression(ToExpression(target), methodName, types);
            return new CodeMethodInvokeExpression(method, ToExpressions(parameters));
        }
        public static CodeMethodInvokeExpression Call(CodeMethodReferenceExpression method, params object[] parameters)
        {
            return new CodeMethodInvokeExpression(method, ToExpressions(parameters));
        }

        private static CodeExpression[] ToExpressions(object[] parameters)
        {
            List<CodeExpression> expressions = new List<CodeExpression>();
            foreach (var parameter in parameters)
            {
                expressions.Add(ToExpression(parameter));
            }
            return expressions.ToArray();
        }

        public static CodeExpression ToExpression(object parameter)
        {
            CodeExpression codeExpression = parameter as CodeExpression;
            if (codeExpression != null)
                return codeExpression;
            Type type = parameter as Type;
            if (type != null) return new CodeTypeReferenceExpression(new CodeTypeReference(type));
            if (parameter == null) return null;

            if (parameter.GetType().IsEnum)
            {
                return new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(parameter.GetType()), parameter.ToString());
            }
            return new CodePrimitiveExpression(parameter);
        }

        public static CodeThrowExceptionStatement Throw(Type exceptionType)
        {
            return new CodeThrowExceptionStatement(new CodeObjectCreateExpression(exceptionType));
        }

        public static CodeExpression Parameter(string paramName)
        {
            return new CodeArgumentReferenceExpression(paramName);
        }

        public static CodeTypeReferenceExpression TypeRef(Type type)
        {
            return new CodeTypeReferenceExpression(type);
        }

        public static CodeExpression Field(string fieldName)
        {
            return Field(This, fieldName);
        }

        public static CodeExpression Field(object target, string fieldName)
        {
            return new CodeFieldReferenceExpression(ToExpression(target), fieldName);
        }

        public static CodeExpression Variable(string name)
        {
            return new CodeVariableReferenceExpression(name);
        }

        public static CodeTypeOfExpression TypeOf(Type type)
        {
            return new CodeTypeOfExpression(type);
        }

        public static CodeExpression Value()
        {
            return new CodeArgumentReferenceExpression("value");
        }

        public static CodeExpression Constants(int index)
        {
            return new CodePrimitiveExpression(index);
        }

        public static CodeStatement Assign(CodeExpression left, CodeExpression right)
        {
            return new CodeAssignStatement(left, right);
        }

        public static CodeExpression CreateArray(Type itemType, int count)
        {
            return new CodeArrayCreateExpression(new CodeTypeReference(itemType.MakeArrayType()), count);
        }
    }

    public static class CodeDecls
    {
        public static string Combin(string namespacePrefix, string typeName)
        {
            namespacePrefix = (namespacePrefix ?? "").Trim();
            namespacePrefix = namespacePrefix.EndsWith(".") ? namespacePrefix : (namespacePrefix + ".");
            return namespacePrefix + typeName;
        }
        public static CodeAttributeDeclaration Attribute<T>(params object[] parameters)
        {
            return Attribute(typeof(T), parameters);
        }
        public static CodeAttributeDeclaration Attribute(Type attrType, params object[] parameters)
        {
            List<CodeAttributeArgument> args = new List<CodeAttributeArgument>();
            foreach (var parameter in parameters)
            {
                args.Add(new CodeAttributeArgument(CodeExprs.ToExpression(parameter)));
            }
            return new CodeAttributeDeclaration(new CodeTypeReference(attrType), args.ToArray());
        }

        public static CodeMemberProperty PropertyGetter(string propertyName, Type propertyType, CodeExpression getter, CodeTypeDeclaration typeDeclaration)
        {
            CodeMemberProperty property = new CodeMemberProperty();
            property.Name = propertyName;
            property.Type = new CodeTypeReference(propertyType);
            property.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            property.GetStatements.Add(new CodeMethodReturnStatement(getter));
            typeDeclaration.Members.Add(property);
            return property;
        }
        public static CodeMemberProperty AutoProperty(string propertyName, Type propertyType, CodeTypeDeclaration typeDeclaration)
        {
            CodeMemberField field = new CodeMemberField(propertyType, "_" + propertyName);
            CodeMemberProperty property = new CodeMemberProperty();

            var fieldRef = new CodeFieldReferenceExpression(CodeExprs.This, "_" + propertyName);
            var valueRef = new CodeArgumentReferenceExpression("value");

            property.Name = propertyName;
            property.Type = new CodeTypeReference(propertyType);
            property.Attributes = MemberAttributes.Public;
            property.SetStatements.Add(new CodeAssignStatement(fieldRef, valueRef));
            property.GetStatements.Add(new CodeMethodReturnStatement(fieldRef));

            typeDeclaration.Members.Add(field);
            typeDeclaration.Members.Add(property);

            return property;
        }

        public static CodeParameterDeclarationExpression Parameter(Type type, string name)
        {
            return new CodeParameterDeclarationExpression(type, name);
        }

        public static CodeVariableDeclarationStatement Variable(string typeName, string name)
        {
            return new CodeVariableDeclarationStatement(typeName, name);
        }
        public static CodeVariableDeclarationStatement Variable(Type type, string name)
        {
            return new CodeVariableDeclarationStatement(type, name);
        }
    }
}
