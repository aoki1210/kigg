namespace Kigg.EF.DomainObjects
{
    using System.Collections.Generic;
    
    using Kigg.DomainObjects;
    
    public partial class User : IUser
    {
        Roles IUser.Role
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public decimal CurrentScore
        {
            get { throw new System.NotImplementedException(); }
        }

        public void ChangeEmail(string email)
        {
            throw new System.NotImplementedException();
        }

        public void ChangePassword(string oldPassword, string newPassword)
        {
            throw new System.NotImplementedException();
        }

        public string ResetPassword()
        {
            throw new System.NotImplementedException();
        }

        public void Lock()
        {
            throw new System.NotImplementedException();
        }

        public void Unlock()
        {
            throw new System.NotImplementedException();
        }

        public decimal GetScoreBetween(System.DateTime startTimestamp, System.DateTime endTimestamp)
        {
            throw new System.NotImplementedException();
        }

        public void IncreaseScoreBy(decimal score, UserAction reason)
        {
            throw new System.NotImplementedException();
        }

        public void DecreaseScoreBy(decimal score, UserAction reason)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<ITag> Tags
        {
            get { throw new System.NotImplementedException(); }
        }

        public int TagCount
        {
            get { throw new System.NotImplementedException(); }
        }

        public void AddTag(ITag tag)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveTag(ITag tag)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAllTags()
        {
            throw new System.NotImplementedException();
        }

        public bool ContainsTag(ITag tag)
        {
            throw new System.NotImplementedException();
        }
    }
}
