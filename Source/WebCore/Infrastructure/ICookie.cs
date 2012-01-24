namespace Kigg.Web
{
    using System.Collections.Specialized;
    
    public interface ICookie
    {
        T GetValue<T>(string name);

        T GetValue<T>(string name, bool expireOnceRead);

        NameValueCollection GetValues(string name, bool expireOnceRead = false);

        void SetValue<T>(string name, T value);

        void SetValue<T>(string name, T value, float expireDurationInMinutes);

        void SetValue<T>(string name, T value, bool httpOnly);

        void SetValue<T>(string name, T value, float expireDurationInMinutes, bool httpOnly);

        void SetValue(string name, NameValueCollection values, float expireDurationInMinutes = 15, bool httpOnly = false);
        
    }
}