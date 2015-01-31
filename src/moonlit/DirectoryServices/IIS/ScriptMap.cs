using System;
using System.Collections.Generic;
using System.Text;

namespace Moonlit.DirectoryServices
{
    public partial class IisDirectoryEntry
    {
        public class ScriptMap
        {
            private string m_fileExName;
            private string m_assemblyName;
            private HttpAction m_actions;

            public HttpAction Actions
            {
                get
                {
                    return this.m_actions;
                }
                set
                {
                    this.m_actions = value;
                }
            }

            public string FileExName
            {
                get
                {
                    return this.m_fileExName;
                }
                set
                {
                    this.m_fileExName = value;
                }
            }

            public string AssemblyName
            {
                get
                {
                    return this.m_assemblyName;
                }
                set
                {
                    this.m_assemblyName = value;
                }
            }
            public override string ToString()
            {
                System.Text.StringBuilder builder = new StringBuilder();
                builder.Append(this.FileExName);
                builder.Append(",");
                builder.Append(this.AssemblyName);
                builder.Append(",");
                builder.Append("5");
                Type t = typeof(HttpAction);
                HttpAction[] actions = (HttpAction[])Enum.GetValues(t);

                foreach (HttpAction action in actions)
                {
                    if ((int)(this.Actions & action) != 0)
                    {
                        builder.Append(",");
                        builder.Append(action.ToString().ToUpper());
                    }
                }
                return builder.ToString();
            }
        }
    }
}
