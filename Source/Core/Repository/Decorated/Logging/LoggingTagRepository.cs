namespace Kigg.Repository
{
    //using System;
    using System.Collections.Generic;

    using DomainObjects;
    using Infrastructure;

    public class LoggingTagRepository : DecoratedTagRepository
    {
        public LoggingTagRepository(ITagRepository innerRepository) : base(innerRepository)
        {
        }

        public override void Add(Tag entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Log.Info("Adding tag: {0}, {1}", entity.Id, entity.Name);
            base.Add(entity);
            Log.Info("Tag added: {0}, {1}", entity.Id, entity.Name);
        }

        public override void Remove(Tag entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Log.Warning("Removing tag: {0}, {1}", entity.Id, entity.Name);
            base.Remove(entity);
            Log.Warning("Tag removed: {0}, {1}", entity.Id, entity.Name);
        }

        public override Tag FindById(long id)
        {
            //Check.Argument.IsNotEmpty(id, "id");

            Log.Info("Retrieving tag with id: {0}", id);

            var result = base.FindById(id);

            if (result == null)
            {
                Log.Warning("Did not find any tag with id: {0}", id);
            }
            else
            {
                Log.Info("Tag retrieved with id: {0}", id);
            }

            return result;
        }

        public override Tag FindByUniqueName(string uniqueName)
        {
            Check.Argument.IsNotNullOrEmpty(uniqueName, "uniqueName");

            Log.Info("Retrieving tag with unique name: {0}", uniqueName);

            var result = base.FindByUniqueName(uniqueName);

            if (result == null)
            {
                Log.Warning("Did not find any tag with unique name: {0}", uniqueName);
            }
            else
            {
                Log.Info("Tag retrieved with unique name: {0}", uniqueName);
            }

            return result;
        }

        public override Tag FindByName(string name)
        {
            Check.Argument.IsNotNullOrEmpty(name, "name");

            Log.Info("Retrieving tag with name: {0}".FormatWith(name));

            var result = base.FindByName(name);

            if (result == null)
            {
                Log.Warning("Did not find any tag with name: {0}", name);
            }
            else
            {
                Log.Info("Tag retrieved with name: {0}", name);
            }

            return result;
        }

        public override IEnumerable<Tag> FindMatching(string name, int max)
        {
            Check.Argument.IsNotNullOrEmpty(name, "name");
            Check.Argument.IsNotNegativeOrZero(max, "max");

            Log.Info("Retrieving tags by name like : {0}, {1}", name, max);

            var result = base.FindMatching(name, max);

            if (result.IsNullOrEmpty())
            {
                Log.Warning("Did not find any tag by name like : {0}, {1}", name, max);
            }
            else
            {
                Log.Info("Tags retrieved by name like : {0}, {1}", name, max);
            }

            return result;
        }

        public override IEnumerable<Tag> FindByUsage(int top)
        {
            Check.Argument.IsNotNegativeOrZero(top, "top");

            Log.Info("Retrieving tags by usage : {0}", top);

            var result = base.FindByUsage(top);

            if (result.IsNullOrEmpty())
            {
                Log.Warning("Did not find any tag by Usage: {0}", top);
            }
            else
            {
                Log.Info("Tags retrieved by usage : {0}", top);
            }

            return result;
        }

        public override IEnumerable<Tag> FindAll()
        {
            Log.Info("Retrieving all tag");

            var result = base.FindAll();

            if (result.IsNullOrEmpty())
            {
                Log.Warning("Did not find any tag");
            }
            else
            {
                Log.Info("All tag retrieved");
            }

            return result;
        }
    }
}