namespace Kigg
{
    using System.Security.Cryptography;
    using System.Text;

    public class UserItem
    {
        public string Name
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string GravatarID
        {
            get
            {
                if (string.IsNullOrEmpty(Email)) return string.Empty;

                byte[] hash;

                using (var md5 = MD5.Create())
                {
                    var data = Encoding.Default.GetBytes(Email);

                    hash = md5.ComputeHash(data);
                }

                var result = new StringBuilder();

                for (var i = 0; i < hash.Length; i++)
                {
                    result.Append(hash[i].ToString("x2", System.Globalization.CultureInfo.InvariantCulture));
                }

                return result.ToString();
            }
        }
    }
}