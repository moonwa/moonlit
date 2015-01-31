using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.Text;

namespace Moonlit.Wcf.FaultExtensions
{
    [DataContract(Namespace = "http://www.artech.com/")]
    public class ServiceExceptionDetail : ExceptionDetail
    {
        public const string FaultSubCodeNamespace = "http://www.artech.com/exceptionhandling/";
        public const string FaultSubCodeName = "ServiceError";
        public const string FaultAction = "http://www.artech.com/fault";

        [DataMember]
        public string AssemblyQualifiedName { get; private set; }

        [DataMember]
        public string ExceptionObject { get; set; }
        [DataMember]
        public new ServiceExceptionDetail InnerException { get; private set; }

        public ServiceExceptionDetail(Exception ex)
            : base(ex)
        {
            this.AssemblyQualifiedName = ex.GetType().AssemblyQualifiedName;
            var attr = ex.GetType().GetCustomAttributes(typeof(SerializableAttribute), false);
            if (attr.Length > 0)
            {
                BinaryFormatter serializer = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                serializer.Serialize(ms, ex);
                ExceptionObject = Convert.ToBase64String(ms.ToArray());
            }
            if (null != ex.InnerException)
            {
                this.InnerException = new ServiceExceptionDetail(ex.InnerException);
            }
        }
        public object DeserializeException()
        {
            if (ExceptionObject == null)
                return null;

            var type = System.Type.GetType(AssemblyQualifiedName);
            if (type == null)
            {
                throw new Exception("Exception " + AssemblyQualifiedName + " not register in client");
            }
            var attr = type.GetCustomAttributes(typeof(SerializableAttribute), false);
            if (attr.Length == 0)
            {
                Exception ex = (Exception)Activator.CreateInstance(type, this.Message);
                return ex;
            }
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(Convert.FromBase64String(ExceptionObject));
            ms.Position = 0;
            return serializer.Deserialize(ms);

        }
        public override string ToString()
        {
            return this.Message;
        }
    }
}
