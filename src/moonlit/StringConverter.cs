using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moonlit.Diagnostics;
using System.Xml.Serialization;
using System.IO;

namespace Moonlit
{
    /// <summary>
    /// 字符串与其它类型之间的转换
    /// </summary>
    public class StringConverter
    {
        static Dictionary<Type, Func<string, object>> _Converters = new Dictionary<Type, Func<string, object>>();
        /// <summary>
        /// 注册转换函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="converter"></param>
        public static void Register<T>( Func<string, object> converter )
        {
            Register( typeof( T ), converter );
        }
        /// <summary>
        /// 注册转换函数
        /// </summary>
        /// <param name="t"></param>
        /// <param name="converter"></param>
        public static void Register( Type t, Func<string, object> converter )
        {
            _Converters.Add( t, converter );
        }
        static StringConverter( )
        {
            Register<Int16>( ( e ) => {
                return Convert.ToInt16( e );
            } );
            Register<Int32>( ( e ) => {
                return Convert.ToInt32( e );
            } );
            Register<Int64>( ( e ) => {
                return Convert.ToInt64( e );
            } );

            Register<Single>( ( e ) => {
                return Convert.ToSingle( e );
            } );

            Register<Double>( ( e ) => {
                return Convert.ToDouble( e );
            } );
            Register<Decimal>( ( e ) => {
                return Convert.ToDecimal( e );
            } );
            Register<string>( ( e ) => {
                return e;
            } );
            Register<TimeSpan>( ( e ) => {
                return TimeSpan.Parse( e );
            } );
            Register<Version>( ( e ) => {
                try
                {
                    return new Version( e );
                }
                catch
                {
                    return new Version( 0, 0 );
                }
            } );
        }
        /// <summary>
        /// 将对象转换为字符串
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string ConvertFrom( object arg )
        {
            if ( arg == null )
                return string.Empty;

            if ( _Converters.ContainsKey( arg.GetType() ) )
            {
                return arg.ToString();
            }
            XmlSerializer serializer = new XmlSerializer( arg.GetType() );
            StringWriter writer = new StringWriter();
            serializer.Serialize( writer, arg );
            return writer.ToString();

        }
        /// <summary>
        /// 将字符串转换为相应的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static T ConvertTo<T>( string arg )
        {
            if ( arg == string.Empty )
                return default( T );

            return (T) ConvertTo( typeof( T ), arg );
        }
        /// <summary>
        /// 将字符串转换为相应的对象
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="arg">The arg.</param>
        /// <returns></returns>
        public static object ConvertTo( Type type, string arg )
        {
            if ( type != typeof( string ) )
            {
                if (string.IsNullOrEmpty(arg))
                {
                    throw new ArgumentNullException("arg");
                }
            }
            if ( _Converters.ContainsKey( type ) )
            {
                return _Converters[type]( arg );
            }
            XmlSerializer serializer = new XmlSerializer( type );
            StringReader reader = new StringReader( arg );
            return serializer.Deserialize( reader );
        }

    }
}
