namespace Kigg.Domain.Entities
{
    using System;

    public class UserScore : IEntity
    {
        public virtual long Id { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual decimal Score { get; set; }

        public virtual User ScoredBy { get; set; }

        public UserAction Action
        {
            get { return (UserAction)ActionType; }

            set { ActionType = (int)value; }
        }

        protected virtual int ActionType { get; set; }
    }
}
