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
        //dot not use as no tracking, because we need to add new tag, assign it to the new article in CreateArticleAsync service  
        // as no tracking can cause error when we add new tag, because it's not tracked      
        var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == tag);
        if (existingTag != null) {
          continue;
        }
        await _context.Tags.AddAsync(new Tag(tag));
      }
      await _context.SaveChangesAsync(); //so it can return the new added tags
      //should return only request added tags only, not all tags
      return _context.Tags.AsEnumerable();
    }
  }
}