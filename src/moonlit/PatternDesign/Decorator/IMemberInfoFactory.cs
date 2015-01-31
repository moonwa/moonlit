using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.CodeDom;

namespace Moonlit.PatternDesign
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMemberInfoFactory
    {
        /// <summary>
        /// Creates the member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="inner">The inner.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns></returns>
        CodeTypeMember CreateMember(MemberInfo member, CodeFieldReferenceExpression inner, MemberAttributes attributes);
    }
}
