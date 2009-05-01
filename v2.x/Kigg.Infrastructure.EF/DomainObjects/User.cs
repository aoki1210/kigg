namespace Kigg.EF.DomainObjects
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    
    using Kigg.DomainObjects;
    using Infrastructure.DomainRepositoryExtensions;

    public partial class User : IUser
    {
        private EntityCollection<ITag, Tag> _userTags;
        internal IEntityCollection<ITag> UserTags
        {
            get
            {
                EnsureUserTags();
                if (!_userTags.IsLoaded)
                {
                    _userTags.Load();
                }
                return _userTags;
            }
            set
            {
                if (!(value is EntityCollection<ITag, Tag>))
                    throw new NotSupportedException("Assigned value must be of type EntityCollection<ITag, Tag>");

                _userTags = (EntityCollection<ITag, Tag>)value;
            }
        }
        
        public Roles Role
        {
            get
            {
                return (Roles) AssignedRole;
            }
            set
            {
                AssignedRole = (int) value;
            }
        }

        public decimal CurrentScore
        {
            get
            {
                return GetScoreBetween(CreatedAt, SystemTime.Now());
            }
        }

        public ICollection<ITag> Tags
        {
            get
            {
                return UserTags.OrderBy(t => t.Name).ToList().AsReadOnly();
            }
        }

        public int TagCount
        {
            get
            {
                var tags = (EntityCollection<ITag, Tag>) UserTags;
                var query = tags.CreateSourceQuery();
                return query != null ? query.Count() : 0;
            }
        }

        public void ChangeEmail(string email)
        {
            Check.Argument.IsNotInvalidEmail(email, "email");
            Check.Argument.IsNotOutOfLength(email, 256, "email");

            if (!this.IsUniqueEmail(email))
            {
                throw new InvalidOperationException("User with the same email already exists.");
            }

            Email = email.ToLowerInvariant();
            LastActivityAt = SystemTime.Now();
        }

        public void ChangePassword(string oldPassword, string newPassword)
        {
            if (this.IsOpenIDAccount())
            {
                throw new InvalidOperationException("Open ID account does not support change password. Please use your Open ID provider.");
            }

            Check.Argument.IsNotEmpty(oldPassword, "oldPassword");
            Check.Argument.IsNotEmpty(newPassword, "newPassword");
            Check.Argument.IsNotOutOfLength(newPassword, 64, "password");

            if (string.Compare(Password, oldPassword.Trim().Hash(), StringComparison.OrdinalIgnoreCase) != 0)
            {
                throw new InvalidOperationException("Old password does not match with the current password.");
            }

            Password = newPassword.Trim().Hash();
            LastActivityAt = SystemTime.Now();
        }

        public string ResetPassword()
        {
            if (this.IsOpenIDAccount())
            {
                throw new InvalidOperationException("Open ID account does not support reset password. Please use your Open ID provider to recover your lost password.");
            }

            string password = CreateRandomString(6, 8);

            Password = password.Hash();

            return password;
        }

        public void Lock()
        {
            IsLockedOut = true;
        }

        public void Unlock()
        {
            IsLockedOut = false;
        }

        public decimal GetScoreBetween(DateTime startTimestamp, DateTime endTimestamp)
        {
            Check.Argument.IsNotInFuture(startTimestamp, "startTimestamp");
            Check.Argument.IsNotInFuture(endTimestamp, "endTimestamp");

            return this.GetScore(startTimestamp, endTimestamp);
        }

        public void IncreaseScoreBy(decimal score, UserAction reason)
        {
            Check.Argument.IsNotNegativeOrZero(score, "score");

            AddScore(score, reason);
        }

        public void DecreaseScoreBy(decimal score, UserAction reason)
        {
            Check.Argument.IsNotNegativeOrZero(score, "score");

            AddScore(-score, reason);
        }

        public void AddTag(ITag tag)
        {
            Check.Argument.IsNotNull(tag, "tag");
            Check.Argument.IsNotEmpty(tag.Id, "tag.Id");
            Check.Argument.IsNotEmpty(tag.Name, "tag.Name");

            if (!ContainsTag(tag))
            {
                UserTagsInternal.Add((Tag)tag);
            }
        }

        public void RemoveTag(ITag tag)
        {
            Check.Argument.IsNotNull(tag, "tag");
            Check.Argument.IsNotEmpty(tag.Name, "tag.Name");

            //It should load all UserTags then remove the desired tag
            UserTags.Remove(UserTags.FirstOrDefault(t => t.Name == tag.Name));
        }

        public void RemoveAllTags()
        {
            //It should load all UserTags then clear the collection
            UserTags.Clear();
        }

        public bool ContainsTag(ITag tag)
        {
            Check.Argument.IsNotNull(tag, "tag");
            Check.Argument.IsNotEmpty(tag.Name, "tag.Name");

            var tagName = tag.Name;

            EnsureUserTags();
            var srcQuery = _userTags.CreateSourceQuery();

            return UserTagsInternal.Any(t => t.Name == tagName) || (srcQuery != null && srcQuery.Any(t => t.Name == tagName));
        }

        private static string CreateRandomString(int minLegth, int maxLength)
        {
            const string Characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#$&";
            var rnd = new Random();

            var length = rnd.Next(minLegth, maxLength);
            var result = new char[length];

            for (var i = 0; i < length; i++)
            {
                result[i] = Characters[rnd.Next(0, Characters.Length)];
            }

            return new string(result);
        }

        private void AddScore(decimal score, UserAction reason)
        {
            var userScore = new UserScore
                                {
                                    Timestamp = SystemTime.Now(),
                                    Score = score,
                                    Action = (int) reason,
                                };

            UserScoreInternal.Add(userScore);
        }
        private void EnsureUserTags()
        {
            if (_userTags == null)
            {
                _userTags = new EntityCollection<ITag, Tag>(UserTagsInternal);
            }
        }
    }
}
