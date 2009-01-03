namespace Kigg
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;

    [DataContract]
    public class JsonResult
    {
        [DataMember]
        public bool isSuccessful
        {
            get;
            set;
        }

        [DataMember]
        public string errorMessage
        {
            get;
            set;
        }

        public string ToJson()
        {
            using (var ms = new MemoryStream())
            {
                var s = new DataContractJsonSerializer(GetType());
                s.WriteObject(ms, this);

                ms.Seek(0, SeekOrigin.Begin);

                return Encoding.Default.GetString(ms.ToArray());
            }
        }
    }
}