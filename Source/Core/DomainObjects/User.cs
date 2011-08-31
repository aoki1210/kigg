namespace Kigg.DomainObjects
{
    using System;
    using System.Collections.Generic;

    public class User : IEntity, ITagContainer
    {
        public virtual long Id { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Password { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual string About { get; set; }

        public virtual string Website { get; set; }

        public virtual string Email { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual bool IsLockedOut { get; set; }

        public virtual decimal CurrentScore { get; set; }

        public virtual DateTime LastActivityAt { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        protected virtual int RoleInternal { get; set; }

        public Roles Role
        {
            get { return (Roles)RoleInternal; }
            set { RoleInternal = (int)value; }
        }

        public virtual ICollection<Tag> Tags{get; protected set;}

        public void AddTag(Tag tag)
        {
            throw new NotImplementedException();
        }

        public void RemoveTag(Tag tag)
        {
            throw new NotImplementedException();
        }

        public void RemoveAllTags()
        {
            throw new NotImplementedException();
        }

        public bool ContainsTag(Tag tag)
        {
            throw new NotImplementedException();
        }

        public void ChangeEmail(string email)
        {
            throw new NotImplementedException();
        }

        public void ChangePassword(string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public string ResetPassword()
        {
            throw new NotImplementedException();
        }

        public void Lock()
        {
            throw new NotImplementedException();
        }

        public void Unlock()
        {
            throw new NotImplementedException();
        }

        public decimal GetScoreBetween(DateTime startTimestamp, DateTime endTimestamp)
        {
            throw new NotImplementedException();
        }

        public void IncreaseScoreBy(decimal score, UserAction reason)
        {
            throw new NotImplementedException();
        }

        public void DecreaseScoreBy(decimal score, UserAction reason)
        {
            throw new NotImplementedException();
        }
    }
}