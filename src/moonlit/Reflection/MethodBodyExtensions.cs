using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace System.Reflection
{
    public static class MethodBodyExtensions
    {
        public static bool IsTiny(this MethodBody mbd)
        {
            if(mbd.MaxStackSize>8)
                return false;//
            //if(mbd.LocalSignatureMetadataToken != 0)
            //    return false;
            if(mbd.LocalVariables.Count>0)
                return false;
            if(mbd.ExceptionHandlingClauses.Count>0)
                return false;
            if(mbd.GetILAsByteArray().Length>63)
                return false;
            return true;
        }

        public static bool IsSEHTiny(this MethodBody mb)
        {
            int n = mb.ExceptionHandlingClauses.Count;
            int datasize = n * 12 + 4;
            if (datasize > 255)
                return false;

            foreach(ExceptionHandlingClause ehc in mb.ExceptionHandlingClauses)
            {
                if (ehc.HandlerLength > 255)
                    return false;
                if (ehc.TryLength > 255)
                    return false;
                if (ehc.TryOffset > 65535)
                    return false;
                if (ehc.HandlerOffset > 65535)
                    return false;
            }
            return true;
        }
        public static void WriteHeader(this MethodBody mb, BinaryWriter writer)
        {
            int codesize = mb.GetILAsByteArray().Length;
            if(mb.IsTiny())
            {
                byte bt = 2;
                bt = (byte)(bt + codesize * 4);
                writer.Write(bt);
                return;
            }
            //fat mode here
            byte fg = 3;//fat flag
            if (mb.LocalVariables.Count > 0 && mb.InitLocals)
                fg |= 0x10;
            if (mb.ExceptionHandlingClauses.Count > 0)
                fg |= 0x8;
            writer.Write(fg);// byte 1            
            writer.Write((byte)0x30);//byte 2
            writer.Write((ushort)mb.MaxStackSize);// byte 3, 4
            writer.Write(codesize);//byte 5-8
            writer.Write(mb.LocalSignatureMetadataToken);//byte 9-12
        }
        public static void WriteILCode(MethodBody mb, BinaryWriter writer)
        {
            int codesize = mb.GetILAsByteArray().Length;
            writer.Write(mb.GetILAsByteArray());

            // 对齐 4 bytes
            int ig = codesize & 3;
            if (ig == 0)
                return;
            if (mb.ExceptionHandlingClauses.Count == 0)
                return;//无SEH;
            ig = 4 - ig;
            for(int i=0; i<ig;i++)
            {
                writer.Write((byte)0);
            }
        }
        public static void WriteTinySEHHeader(MethodBody mb, BinaryWriter bw)
        {
            int n = mb.ExceptionHandlingClauses.Count;
            int datasize = n * 12 + 4;
            bw.Write((byte)1);
            bw.Write((byte)datasize);
            bw.Write((byte)0);
            bw.Write((byte)0);
        }
        public static void WriteFatSEHHeader(MethodBody mb, BinaryWriter bw)
        {
            int n = mb.ExceptionHandlingClauses.Count;
            int datasize = n * 24 + 4;
            datasize = datasize * 0x100 + 0x41;
            bw.Write(datasize);
        }

        public static void WriteSeHTinyRow(ExceptionHandlingClause ehc, BinaryWriter bw)
        {
            ushort flag = 0;
           
            if (ehc.Flags == ExceptionHandlingClauseOptions.Filter)
                flag += 1;
            if (ehc.Flags == ExceptionHandlingClauseOptions.Fault)
                flag += 4;
            if (ehc.Flags == ExceptionHandlingClauseOptions.Finally)
                flag += 2;
            bw.Write(flag);

            bw.Write((ushort)ehc.TryOffset);
            bw.Write((byte)ehc.TryLength);

            bw.Write((ushort)ehc.HandlerOffset);
            bw.Write((byte)ehc.HandlerLength);
            object obj = new object();
            if (ehc.Flags == ExceptionHandlingClauseOptions.Clause /*|| ehc.CatchType != obj.GetType()*/)
                bw.Write(GetTypeToken(ehc.CatchType));
            else
                bw.Write(ehc.FilterOffset);

        }

        public static void WriteSeHFatRow(ExceptionHandlingClause ehc, BinaryWriter bw)
        {
            int flag = 0;
           
            if (ehc.Flags == ExceptionHandlingClauseOptions.Filter)
                flag += 1;
            if (ehc.Flags == ExceptionHandlingClauseOptions.Fault)
                flag += 4;
            if (ehc.Flags == ExceptionHandlingClauseOptions.Finally)
                flag += 2;
            bw.Write(flag);//
            
            bw.Write(ehc.TryOffset);
            bw.Write(ehc.TryLength);

            bw.Write(ehc.HandlerOffset);
            bw.Write(ehc.HandlerLength);
            object obj = new object();
            if (ehc.Flags == ExceptionHandlingClauseOptions.Clause /*|| ehc.CatchType != obj.GetType()*/)
                bw.Write(GetTypeToken(ehc.CatchType));
            else
                bw.Write(ehc.FilterOffset);
            

        }

        public static void WriteSEH(MethodBody mb, BinaryWriter bw)
        {
            if (mb.ExceptionHandlingClauses.Count == 0)
                return;
            bool bTiny = IsSEHTiny(mb);
            if (bTiny)
                WriteTinySEHHeader(mb, bw);
            else
                WriteFatSEHHeader(mb, bw);
            foreach (ExceptionHandlingClause ehc in mb.ExceptionHandlingClauses)
            {
                if (bTiny)
                    WriteSeHTinyRow(ehc, bw);
                else
                    WriteSeHFatRow(ehc, bw);
            }
        }

       
        public static void Dump()
        {
            Class1 cls = new Class1();
            cls.DoIt();
        }
        public Class1()
        {
            //nil
            int i = 0;
            try
            {
                string s = "";
                if (s == "")
                    i = 2;

            }
            catch(Exception ex)
            {
                Console.WriteLine("err"+ex.ToString());
            }
        }

        protected void DoIt()
        {
            Assembly ass = Assembly.GetEntryAssembly();
            DumpAssembly(ass,@"D:\4.0.1.0\dumped.exe");
           
        }

        /// <summary>
        /// Dump程序集的 IL字节代码到指定目录；
        /// </summary>
        /// <param name="ass"></param>
        /// <param name="path"></param>
        private void DumpAssembly(Assembly ass,string path)
        {
            //////////////////////////////////////////////////////////////////////////
            if(!testdd.com.WrapperClass.MetaInit(ass.Location))
            {
                MessageBox.Show("error meta");
                return;
            }
            FileStream fs = new FileStream(path, System.IO.FileMode.Open,FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);

            Type[] tps = ass.GetTypes();
            for(int i=0; i< tps.Length; i++)
            {
                DumpType(tps[i], bw);
            }
            bw.Flush();
            bw.Close();
            bw = null;
            fs.Close();
            fs = null;
            MessageBox.Show("ok");
        }
        private void DumpType(Type tp, BinaryWriter sw)
        {
            BindingFlags bf = BindingFlags.NonPublic | BindingFlags.DeclaredOnly |
               BindingFlags.Public | BindingFlags.Static
               | BindingFlags.Instance;

            
            MemberInfo[] mbis = tp.GetMembers(bf);
            for (int i = 0; i < mbis.Length; i++)
            {
                MemberInfo mbi = mbis[i];                
                
                try
                {
                    if (mbi.MemberType == MemberTypes.Method || mbi.MemberType == MemberTypes.Constructor)
                    {
                        DumpMethod((MethodBase)mbi, sw);
                    }
                }
                catch(Exception)
                {
                   
                }

            }
           
        }

        private void DumpMethod(MethodBase mb, BinaryWriter sw)
        {
            MethodBody mbd = mb.GetMethodBody();
            if (mbd == null)
                return;
            SetOffset(sw, mb.MetadataToken);

            WriteHeader(mbd, sw);

            WriteILCode(mbd, sw);

            WriteSEH(mbd, sw);   

        }
        private static int GetTypeToken(Type tp)
        {
            if (tp.Assembly == Assembly.GetEntryAssembly())
                return tp.MetadataToken;
            Assembly ass = Assembly.GetEntryAssembly();
            uint tk = testdd.com.WrapperClass.GetTypeToken(tp);
            if(tk == 0)
            {
                MessageBox.Show("error tk");
                return 0x100005f;
            }
            return (int)tk;
        }
        private void SetOffset(BinaryWriter bw, int mbtk)
        {
            uint token = (uint)mbtk;
            uint offsetrva = testdd.com.WrapperClass.GetMehodRVA(token);
            int offsetra = (int)(offsetrva - 0x1000);
            bw.Seek(offsetra, SeekOrigin.Begin);
        }
    }
}
