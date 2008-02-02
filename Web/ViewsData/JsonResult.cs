namespace Kigg
{
    using System.IO;
    using System.Text;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;

    [DataContract()]
    public class JsonResult
    {
        [DataMember()]
        public bool isSuccessful
        {
            get;
            set;
        }

        [DataMember()]
        public string errorMessage
        {
            get;
            set;
        }

        public string ToJson()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                DataContractJsonSerializer s = new DataContractJsonSerializer(this.GetType());
                s.WriteObject(ms, this);

                ms.Seek(0, SeekOrigin.Begin);

                return Encoding.Default.GetString(ms.ToArray());
            }
        }
    }
}