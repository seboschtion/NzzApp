using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Sebastian.Toolkit.Util.Collections;

namespace NzzApp.Tests.Collection
{
    [TestClass]
    public class ReplaceableObservableCollectionTests
    {
        private readonly ItemEqualityComparer _equalityComparer = new ItemEqualityComparer();
        private ReplaceableObservableCollection<Item> _collection;
        private ReplaceableObservableCollection<Item> _sortedCollection;

        [TestInitialize]
        public void Setup()
        {
            _collection = new ReplaceableObservableCollection<Item>(_equalityComparer);
            _sortedCollection = new ReplaceableObservableCollection<Item>(_equalityComparer);

            _collection.Add(new Item("Abc"));
            _collection.Add(new Item("Def"));
            _collection.Add(new Item("Ghi"));
            _sortedCollection.Add(new Item("Def", 1));
            _sortedCollection.Add(new Item("Ghi", 2));
            _sortedCollection.Add(new Item("Abc", 3));
        }

        [TestMethod]
        public void TestReplaceItems1()
        {
            var newCollection = new List<Item>();
            newCollection.Add(new Item("Jkl"));
            newCollection.Add(new Item("Mno"));
            newCollection.Add(new Item("Pqr"));

            _collection.ReplaceItemsWithList(newCollection);

            Assert.IsTrue(CollectionsAreEqual(_collection, newCollection));
        }

        [TestMethod]
        public void TestReplaceItems2()
        {
            var newCollection = new List<Item>();
            newCollection.Add(new Item("Abc"));
            newCollection.Add(new Item("Mno"));
            newCollection.Add(new Item("Pqr"));

            _collection.ReplaceItemsWithList(newCollection);

            Assert.IsTrue(CollectionsAreEqual(_collection, newCollection));
        }

        [TestMethod]
        public void TestReplaceItems3()
        {
            var newCollection = new List<Item>();

            _collection.ReplaceItemsWithList(newCollection);

            Assert.IsTrue(_collection.Count == 0);
        }

        [TestMethod]
        public void TestReplaceItemsWithNewOrder1()
        {
            var newCollection = new List<Item>();
            newCollection.Add(new Item("Abc", 1));
            newCollection.Add(new Item("Def", 2));
            newCollection.Add(new Item("Ghi", 3));

            _sortedCollection.ReplaceItemsWithList(newCollection);

            Assert.IsTrue(CollectionsAreEqual(_sortedCollection, newCollection));
        }

        [TestMethod]
        public void TestReplaceItemsWithNewOrder2()
        {
            var newCollection = new List<Item>();
            newCollection.Add(new Item("Abc", 1));
            newCollection.Add(new Item("Def", 2));
            newCollection.Add(new Item("Xyz", 10));

            _sortedCollection.ReplaceItemsWithList(newCollection);

            Assert.IsTrue(CollectionsAreEqual(_sortedCollection, newCollection));
        }

        [TestMethod]
        public void TestReplaceItemsWithNewOrder3()
        {
            var newCollection = new List<Item>();
            newCollection.Add(new Item("Abc", 1));
            newCollection.Add(new Item("Def", 2));
            newCollection.Add(new Item("Ghi", 3));
            newCollection.Add(new Item("Xyz", 1));

            _sortedCollection.ReplaceItemsWithList(newCollection);

            newCollection = new List<Item>();
            newCollection.Add(new Item("Abc", 1));
            newCollection.Add(new Item("Xyz", 1));
            newCollection.Add(new Item("Def", 2));
            newCollection.Add(new Item("Ghi", 3));

            Assert.IsTrue(CollectionsAreEqual(_sortedCollection, newCollection));
        }

        private bool CollectionsAreEqual(IReadOnlyList<Item> coll1, IReadOnlyList<Item> coll2)
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
