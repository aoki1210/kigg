namespace Kigg
{
    using System.Text;
    using System.Security.Cryptography;

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
                byte[] hash = null;

                using (MD5 md5 = MD5.Create())
                {
                    byte[] data = Encoding.Default.GetBytes(Email);

                    hash = md5.ComputeHash(data);
                }

                StringBuilder result = new StringBuilder(hash.Length);

                for (int i = 0; i < hash.Length; i++)
                {
                    result.Append(hash[i].ToString("x2"));
                }

                return result.ToString();
            }
        }
    }
}