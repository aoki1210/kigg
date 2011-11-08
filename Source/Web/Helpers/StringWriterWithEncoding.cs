namespace Kigg.Web
{
    using System.IO;
    using System.Text;

    public class StringWriterWithEncoding : StringWriter
    {
        private readonly Encoding _encoding;

        public StringWriterWithEncoding(StringBuilder sb) : this(sb, Encoding.UTF8)
        {
        }

        public StringWriterWithEncoding(StringBuilder sb, Encoding encoding) : base(sb, GlobalConstants.CurrentCulture)
        {
            Check.Argument.IsNotNull(encoding, "encoding");

            _encoding = encoding;
        }

        public override Encoding Encoding
        {
            get
            {
                return _encoding;
            }
        }
    }
}