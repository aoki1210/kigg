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

                using (MD5 md5 = MD5.Create())
                {
                    byte[] data = Encoding.Default.GetBytes(Email);

                    hash = md5.ComputeHash(data);
                }

                StringBuilder result = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    result.Append(hash[i].ToString("x2", System.Globalization.CultureInfo.InvariantCulture));
                }

                return result.ToString();
            }
        }
    }
}