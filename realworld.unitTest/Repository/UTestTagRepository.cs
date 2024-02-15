using FluentAssertions;
using Realworld.Api.Models;

namespace Realworld.UnitTest.Repository
{
    public class UTestTagRepository : BaseUTestRepository
    {

        protected Realworld.Api.Data.TagRepository TagRepository { get; init; }
        public UTestTagRepository(FixtureUTestRepository fixture) : base(fixture)
        {
            this.SeedDb();
            TagRepository = new Realworld.Api.Data.TagRepository(base.ConduitContext);
        }

        public override void SeedDb()
        {
            base.SeedDb();
            bool isDbJustCreated = ConduitContext.Database.EnsureCreated();
            if (ConduitContext.Tags.Any()) return;
            ConduitContext.Tags.AddRange(base.DummyTags);
            ConduitContext.SaveChanges();
        }

        [Fact]
        public async Task WhenGetAllTags_ReturnAllTags()
        {
            var actualTags = await TagRepository.GetTagsAsync();
            actualTags.Should().BeEquivalentTo(base.DummyTags);
            actualTags.Should().BeOfType(typeof(List<Tag>));
        }

        [Fact]
        public async Task WhenUpsertTags_ReturnTags()
        {
            //Arange
            var newTagList = new List<Tag>() {
                new Tag(id: "newTag"),
                new Tag(id: "tag1")
            };
            var newTagsListStr = newTagList.Select(t => t.Id).ToList();
            //Act
            var allTagsAfterAdded = await TagRepository.UpsertTagsAsync(newTagsListStr);
            //Assert
            var addedTags = base.ConduitContext.Tags
                .Where(t => newTagsListStr.Contains(t.Id))
                .ToList();
            allTagsAfterAdded.Should().BeEquivalentTo(base.DummyTags.UnionBy(addedTags, t => t.Id));
            allTagsAfterAdded.Should().BeAssignableTo<IEnumerable<Tag>>();
            addedTags.Should().BeEquivalentTo(newTagList);
        }
    }
}