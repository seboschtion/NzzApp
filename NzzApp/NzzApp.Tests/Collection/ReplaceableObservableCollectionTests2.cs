using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Implementation.Articles;
using Sebastian.Toolkit.Util.Collections;

namespace NzzApp.Tests.Collection
{
    [TestClass]
    public class ReplaceableObservableCollectionTests2
    {
        private readonly ArticleEqualityComparer _equalityComparer = new ArticleEqualityComparer();
        private ReplaceableObservableCollection<ViewOptimizedArticle> _collection;
        private ReplaceableObservableCollection<ViewOptimizedArticle> _sortedCollection;

        private readonly IArticle[] _articles =
        {
            new Article("Abc") {PublishedOn = DateTime.Now.AddSeconds(-1)},
            new Article("Def") {PublishedOn = DateTime.Now.AddSeconds(-2)},
            new Article("Ghi") {PublishedOn = DateTime.Now.AddSeconds(-3)},
            new Article("Jkl") {PublishedOn = DateTime.Now.AddSeconds(-4)},
            new Article("Mno") {PublishedOn = DateTime.Now.AddSeconds(-5)},
            new Article("Pqr") {PublishedOn = DateTime.Now.AddSeconds(-6)}
        };

        [TestInitialize]
        public void Setup()
        {
            _collection = new ReplaceableObservableCollection<ViewOptimizedArticle>(_equalityComparer);
            _sortedCollection = new ReplaceableObservableCollection<ViewOptimizedArticle>(_equalityComparer);

            _collection.Add(new ViewOptimizedArticle() {Article = _articles[0]});
            _collection.Add(new ViewOptimizedArticle() {Article = _articles[1]});
            _collection.Add(new ViewOptimizedArticle() {Article = _articles[2]});
            _sortedCollection.Add(new ViewOptimizedArticle() {Article = _articles[1], Sort = 1});
            _sortedCollection.Add(new ViewOptimizedArticle() {Article = _articles[2], Sort = 2});
            _sortedCollection.Add(new ViewOptimizedArticle() {Article = _articles[0], Sort = 3});
        }

        [TestMethod]
        public void TestReplaceItems1()
        {
            var newCollection = new List<ViewOptimizedArticle>();
            newCollection.Add(new ViewOptimizedArticle() { Article = _articles[3] });
            newCollection.Add(new ViewOptimizedArticle() { Article = _articles[4] });
            newCollection.Add(new ViewOptimizedArticle() { Article = _articles[5] });

            _collection.ReplaceItemsWithList(newCollection);

            Assert.IsTrue(CollectionsAreEqual(_collection, newCollection));
        }

        [TestMethod]
        public void TestReplaceItems2()
        {
            var newCollection = new List<ViewOptimizedArticle>();
            newCollection.Add(new ViewOptimizedArticle() {Article = _articles[0]});
            newCollection.Add(new ViewOptimizedArticle() {Article = _articles[4]});
            newCollection.Add(new ViewOptimizedArticle() {Article = _articles[5]});

            _collection.ReplaceItemsWithList(newCollection);

            Assert.IsTrue(CollectionsAreEqual(_collection, newCollection));
        }

        [TestMethod]
        public void TestReplaceItems3()
        {
            var newCollection = new List<ViewOptimizedArticle>();

            _collection.ReplaceItemsWithList(newCollection);

            Assert.IsTrue(_collection.Count == 0);
        }

        [TestMethod]
        public void TestReplaceItemsWithNewOrder1()
        {
            var newCollection = new List<ViewOptimizedArticle>();
            newCollection.Add(new ViewOptimizedArticle() { Article = _articles[0], Sort = 1});
            newCollection.Add(new ViewOptimizedArticle() { Article = _articles[1], Sort = 2});
            newCollection.Add(new ViewOptimizedArticle() { Article = _articles[2], Sort = 3});

            _sortedCollection.ReplaceItemsWithList(newCollection);

            Assert.IsTrue(CollectionsAreEqual(_sortedCollection, newCollection));
        }

        [TestMethod]
        public void TestReplaceItemsWithNewOrder2()
        {
            var newCollection = new List<ViewOptimizedArticle>();
            newCollection.Add(new ViewOptimizedArticle() { Article = _articles[1], Sort = 1 });
            newCollection.Add(new ViewOptimizedArticle() { Article = _articles[2], Sort = 2 });
            newCollection.Add(new ViewOptimizedArticle() { Article = _articles[5], Sort = 10 });

            _sortedCollection.ReplaceItemsWithList(newCollection);

            Assert.IsTrue(CollectionsAreEqual(_sortedCollection, newCollection));
        }

        [TestMethod]
        public void TestReplaceItemsWithNewOrder3()
        {
            var newCollection = new List<ViewOptimizedArticle>();
            newCollection.Add(new ViewOptimizedArticle() {Article = _articles[0], Sort = 1});
            newCollection.Add(new ViewOptimizedArticle() {Article = _articles[1], Sort = 2});
            newCollection.Add(new ViewOptimizedArticle() {Article = _articles[2], Sort = 3});
            newCollection.Add(new ViewOptimizedArticle() {Article = _articles[5], Sort = 1});

            _sortedCollection.ReplaceItemsWithList(newCollection);

            newCollection = new List<ViewOptimizedArticle>();
            newCollection.Add(new ViewOptimizedArticle() {Article = _articles[0], Sort = 1});
            newCollection.Add(new ViewOptimizedArticle() {Article = _articles[5], Sort = 1});
            newCollection.Add(new ViewOptimizedArticle() {Article = _articles[1], Sort = 2});
            newCollection.Add(new ViewOptimizedArticle() {Article = _articles[2], Sort = 3});

            Assert.IsTrue(CollectionsAreEqual(_sortedCollection, newCollection));
        }

        [TestMethod]
        public void TestReplaceItemsWithNewOrder4()
        {
            var newCollection = new List<ViewOptimizedArticle>();
            newCollection.Add(new ViewOptimizedArticle() {Article = _articles[0], Sort = 1});

            _sortedCollection.ReplaceItemsWithList(newCollection);

            Assert.IsTrue(CollectionsAreEqual(_sortedCollection, newCollection));
        }

        [TestMethod]
        public void TestReplaceItemsWithNewOrder5()
        {
            var newCollection = new List<ViewOptimizedArticle>();
            newCollection.Add(new ViewOptimizedArticle() { Article = _articles[4], Sort = 1 });
            newCollection.Add(new ViewOptimizedArticle() { Article = _articles[0], Sort = 2 });
            newCollection.Add(new ViewOptimizedArticle() { Article = _articles[3], Sort = 3 });

            _sortedCollection.ReplaceItemsWithList(newCollection);

            Assert.IsTrue(CollectionsAreEqual(_sortedCollection, newCollection));
        }

        [TestMethod]
        public void TestReplaceItemsWithNewOrder6()
        {
            var collection1 = new ReplaceableObservableCollection<ViewOptimizedArticle>(_equalityComparer);
            collection1.Add(new ViewOptimizedArticle() { Article = _articles[0], Sort = 1 });
            collection1.Add(new ViewOptimizedArticle() { Article = _articles[1], Sort = 2 });
            collection1.Add(new ViewOptimizedArticle() { Article = _articles[2], Sort = 3 });

            var collection2 = new ReplaceableObservableCollection<ViewOptimizedArticle>(_equalityComparer);
            collection2.Add(new ViewOptimizedArticle() { Article = _articles[0], Sort = 2 });
            collection2.Add(new ViewOptimizedArticle() { Article = _articles[1], Sort = 1 });
            collection2.Add(new ViewOptimizedArticle() { Article = _articles[2], Sort = 3 });

            var result = new ReplaceableObservableCollection<ViewOptimizedArticle>(_equalityComparer);
            result.Add(new ViewOptimizedArticle() { Article = _articles[1], Sort = 1 });
            result.Add(new ViewOptimizedArticle() { Article = _articles[0], Sort = 2 });
            result.Add(new ViewOptimizedArticle() { Article = _articles[2], Sort = 3 });

            collection1.ReplaceItemsWithList(collection2);

            Assert.IsTrue(CollectionsAreEqual(collection1, result));
        }

        private bool CollectionsAreEqual(IReadOnlyList<ViewOptimizedArticle> coll1, IReadOnlyList<ViewOptimizedArticle> coll2)
        {
            if (coll1.Count != coll2.Count)
            {
                return false;
            }

            for (int i = 0; i < coll1.Count; i++)
            {
                if (!_equalityComparer.Equals(coll1[i], coll2[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
