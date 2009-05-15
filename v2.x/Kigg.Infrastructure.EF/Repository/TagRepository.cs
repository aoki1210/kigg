namespace Kigg.EF.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Kigg.DomainObjects;
    using Kigg.Repository;
    using DomainObjects;

    public class TagRepository : BaseRepository<ITag, Tag>, ITagRepository
    {
        public TagRepository(IDatabase database)
            : base(database)
        {
        }

        public TagRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }

        public override void Add(ITag entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            var tag = (Tag)entity;

            if (Database.TagDataSource.Any(t => t.Name == tag.Name))
            {
                throw new ArgumentException("\"{0}\" tag already exits. Specifiy a diffrent name.".FormatWith(tag.Name), "entity");
            }

            tag.UniqueName = UniqueNameGenerator.GenerateFrom(Database.TagDataSource, tag.Name);

            Database.InsertOnSubmit(tag);
        }

        public override void Remove(ITag entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            var tag = (Tag)entity;

            tag.RemoveAllStories();
            tag.RemoveAllUsers();
            
            base.Remove(tag);
        }

#if(DEBUG)
        public virtual ITag FindById(Guid id)
#else
        public ITag FindById(Guid id)
#endif
        {
            Check.Argument.IsNotEmpty(id, "id");

            return Database.TagDataSource.FirstOrDefault(t => t.Id == id);
        }

#if(DEBUG)
        public virtual ITag FindByUniqueName(string uniqueName)
#else
        public ITag FindByUniqueName(string uniqueName)
#endif
        {
            Check.Argument.IsNotEmpty(uniqueName, "uniqueName");

            return Database.TagDataSource.FirstOrDefault(t => t.UniqueName == uniqueName);
        }

#if(DEBUG)
        public virtual ITag FindByName(string name)
#else
        public ITag FindByName(string name)
#endif

        {
            Check.Argument.IsNotEmpty(name, "name");

            return Database.TagDataSource.FirstOrDefault(t => t.Name == name);
        }

#if(DEBUG)
        public virtual ICollection<ITag> FindMatching(string name, int max)
#else
        public ICollection<ITag> FindMatching(string name, int max)
#endif
        {
            Check.Argument.IsNotEmpty(name, "name");
            Check.Argument.IsNotNegativeOrZero(max, "max");

            return Database.TagDataSource
                           .Where(t => t.Name.StartsWith(name))
                           .OrderBy(t => t.Name)
                           .Take(max)
                           .AsEnumerable()
                           .Cast<ITag>()
                           .ToList()
                           .AsReadOnly();
        }

#if(DEBUG)
        public virtual ICollection<ITag> FindByUsage(int top)
#else
        public ICollection<ITag> FindByUsage(int top)
#endif
        {
            Check.Argument.IsNotNegativeOrZero(top, "top");

            return Database.TagDataSource
                           .Where(t => t.StoriesInternal.Any())
                           .OrderByDescending(t => t.StoriesInternal.Count(st => st.ApprovedAt != null))
                           .ThenBy(t => t.Name)
                           .Take(top)
                           .AsEnumerable()
                           .Cast<ITag>()
                           .ToList()
                           .AsReadOnly();
        }

#if(DEBUG)
        public virtual ICollection<ITag> FindAll()
#else
        public ICollection<ITag> FindAll()
#endif
        {
            return Database.TagDataSource
                           .OrderBy(t => t.Name)
                           .AsEnumerable()
                           .Cast<ITag>()
                           .ToList()
                           .AsReadOnly();
        }
    }
}