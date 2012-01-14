using System.Collections.Generic;
using System.Linq;
using AD.Shared.Collections;
using AD.Shared.Extensions;
using NUnit.Framework;
using SharpTestsEx;

namespace AD.Shared.Tests
{
    [TestFixture]
    public class PagedListFixture
    {
        private const int ITEMS_COUNT = 200;
        private const int ITEMS_PER_PAGE = 20;

        private IEnumerable<int> _source;
        private PagedList<int> _firstPage;
        private PagedList<int> _fifthPage;
        private PagedList<int> _lastPage;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _source = Enumerable.Range(1, ITEMS_COUNT);
            _firstPage = _source.ToPagedList(0, ITEMS_PER_PAGE, ITEMS_COUNT);
            _fifthPage = _source.Skip(4 * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToPagedList(4, ITEMS_PER_PAGE, ITEMS_COUNT);
            _lastPage = _source.Skip(9 * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToPagedList(9, ITEMS_PER_PAGE, ITEMS_COUNT);
        }

        [Test]
        public void FirstPageShouldBeTheFirstOne()
        {
            _firstPage.IsFirstPage.Should().Be.True();
        }

        [Test]
        public void FirstPageShouldNotBeTheLastOne()
        {
            _firstPage.IsLastPage.Should().Be.False();
        }

        [Test]
        public void TheFirstValueOfTheFirstPageShouldBeOne()
        {
            _firstPage[0].Should().Be.EqualTo(1);
        }

        [Test]
        public void FifthPageShouldNotBeTheFirstOne()
        {
            _fifthPage.IsFirstPage.Should().Be.False();
        }

        [Test]
        public void FifthPageShouldNotBeTheLastOne()
        {
            _fifthPage.IsLastPage.Should().Be.False();
        }

        [Test]
        public void TheFirstValueOfTheFistPageShouldBe81()
        {
            _fifthPage[0].Should().Be.EqualTo(81);
        }

        [Test]
        public void LastPageShouldNotBeTheFirstOne()
        {
            _lastPage.IsFirstPage.Should().Be.False();
        }

        [Test]
        public void LastPageShouldBeTheLastOne()
        {
            _lastPage.IsLastPage.Should().Be.True();
        }

        [Test]
        public void TheFistValueOfTheLastPageShouldBe181()
        {
            _lastPage[0].Should().Be.EqualTo(181);
        }

        [Test]
        public void AllPagesShouldHaveTheSameTotalItemsCount()
        {
            _firstPage.TotalItemCount.Should().Be.EqualTo(ITEMS_COUNT);
            _fifthPage.TotalItemCount.Should().Be.EqualTo(ITEMS_COUNT);
            _lastPage.TotalItemCount.Should().Be.EqualTo(ITEMS_COUNT);
        }

        [Test]
        public void AllPagesShouldHaveTheSamePageItemsCount()
        {
            _firstPage.PageSize.Should().Be.EqualTo(ITEMS_PER_PAGE);
            _fifthPage.PageSize.Should().Be.EqualTo(ITEMS_PER_PAGE);
            _lastPage.PageSize.Should().Be.EqualTo(ITEMS_PER_PAGE);
        }
    }
}
