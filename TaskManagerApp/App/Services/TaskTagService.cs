using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class TaskTagService
    {
        private static readonly (string Name, string Color)[] DefaultTags =
        {
            ("Work", "#6258E8"),
            ("Personal", "#0D9488")
        };

        private readonly ITaskTagRepo _tagRepo;

        public TaskTagService(ITaskTagRepo tagRepo) => _tagRepo = tagRepo;

        public Task<List<TaskTag>> GetForUserAsync(string userId) => _tagRepo.GetForUserAsync(userId);

        public async Task<List<TaskTag>> GetOrCreateDefaultsAsync(string userId)
        {
            var tags = await _tagRepo.GetForUserAsync(userId);

            if (tags.Count == 0)
            {
                foreach (var defaultTag in DefaultTags)
                {
                    var tag = new TaskTag
                    {
                        Name = defaultTag.Name,
                        Color = defaultTag.Color,
                        UserId = userId
                    };

                    await _tagRepo.AddAsync(tag);
                    tags.Add(tag);
                }
            }

            return tags.OrderBy(tag => tag.Name).ToList();
        }

        public async Task<TaskTag> CreateAsync(string userId, string name, string color)
        {
            name = NormalizeName(name);
            color = NormalizeColor(color);

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
                Color = color,
                UserId = userId
            };

            await _tagRepo.AddAsync(newTag);
            return newTag;
        }

        public async Task UpdateAsync(string userId, int tagId, string name, string color)
        {
            name = NormalizeName(name);
            color = NormalizeColor(color);

            var tags = await _tagRepo.GetForUserAsync(userId);
            var tag = tags.FirstOrDefault(item => item.Id == tagId);

            if (tag is null)
            {
                throw new InvalidOperationException("Tag not found.");
            }

            if (tags.Any(item => item.Id != tagId && string.Equals(
                item.Name,
                name,
                StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("You already have a tag with that name.");
            }

            tag.Name = name;
            tag.Color = color;
            await _tagRepo.UpdateAsync(tag);
        }

        public async Task DeleteAsync(string userId, int tagId)
        {
            var tags = await _tagRepo.GetForUserAsync(userId);
            var tag = tags.FirstOrDefault(item => item.Id == tagId);

            if (tag is null)
            {
                throw new InvalidOperationException("Tag not found.");
            }

            if (tags.Count == 1)
            {
                throw new InvalidOperationException("You must keep at least one tag.");
            }

            await _tagRepo.DeleteAsync(tag);
        }

        private static string NormalizeName(string? name)
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

            return name;
        }

        private static string NormalizeColor(string? color)
        {
            if (color is null ||
                color.Length != 7 ||
                color[0] != '#' ||
                !color.Skip(1).All(Uri.IsHexDigit))
            {
                throw new ArgumentException("Choose a valid tag color.", nameof(color));
            }

            return color.ToUpperInvariant();
        }
    }
}