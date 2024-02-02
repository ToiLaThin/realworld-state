using Microsoft.EntityFrameworkCore;
using Realworld.Api.Models;

namespace Realworld.Api.Data
{
  public class TagRepository : ITagRepository
  {
    private readonly ConduitContext _context;
    public TagRepository(ConduitContext context)
    {
      _context = context;
    }
    public async Task<List<Tag>> GetTagsAsync()
    {
      return await _context.Tags.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Tag>> UpsertTagsAsync(IEnumerable<string> tags)
    {
      foreach (var tag in tags) {
        var existingTag = await _context.Tags.AsNoTracking().FirstOrDefaultAsync(t => t.Id == tag);
        if (existingTag != null) {
          continue;
        }
        await _context.Tags.AddAsync(new Tag(tag));
      }
      return _context.Tags.AsEnumerable();
    }
  }
}