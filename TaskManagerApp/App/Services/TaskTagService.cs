using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class TaskTagService
    {
        private static readonly string[] DefaultTagNames = { "Work", "Personal" };
        private readonly ITaskTagRepo _tagRepo;

        public TaskTagService(ITaskTagRepo tagRepo) => _tagRepo = tagRepo;

        public async Task<List<TaskTag>> GetOrCreateDefaultsAsync(string userId)
        {
            var tags = await _tagRepo.GetForUserAsync(userId);

            foreach (var name in DefaultTagNames)
            {
                if (tags.Any(tag => string.Equals(
                    tag.Name,
                    name,
                    StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                var tag = new TaskTag
                {
                    Name = name,
                    UserId = userId
                };

                await _tagRepo.AddAsync(tag);
                tags.Add(tag);
            }

            return tags.OrderBy(tag => tag.Name).ToList();
        }

        public async Task<TaskTag> CreateAsync(string userId, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Enter a tag name.", nameof(name));
            }

            name = name.Trim();

            if (name.Length > 50)
            {
                throw new ArgumentException("Tag names must be 50 characters or fewer.", nameof(name));
            }

            var tags = await _tagRepo.GetForUserAsync(userId);

            if (tags.Any(tag => string.Equals(
                tag.Name,
                name,
                StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("You already have a tag with that name.");
            }

            var newTag = new TaskTag
            {
                Name = name,
                UserId = userId
            };

            await _tagRepo.AddAsync(newTag);
            return newTag;
        }
    }
}