using System;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Driver.Builders;

namespace MausWorks.MongoDB
{
    public class EntitySet<T> : IQueryable<T> where T : class
    {
		private Lazy<MongoCollection<T>> _collection;

		public MongoCollection<T> Collection { get { return _collection.Value; } }

		public EntitySet(MongoDatabase db, string collectionName)
		{
			_collection = new Lazy<MongoCollection<T>>(() => db.GetCollection<T>(collectionName));
		}

        #region -- More wrapper functions --

        public int InsertMany(IEnumerable<T> documents)
        {
            return Collection.InsertBatch(documents).Count(cn => cn.Ok);
        }

        public bool Insert(T document)
        {
            return Collection.Insert(document).Ok;
        }

        public bool Delete(IMongoQuery query)
        {
            return Collection.Remove(query).Ok;
        }

        #endregion

        #region -- IQueryable<T> wrapper functions --

        private IQueryable<T> _queryableCollection;
		private IQueryable<T> QueryableCollection
		{
			get
			{
				if (_queryableCollection == null)
				{
					_queryableCollection = Collection.AsQueryable();
				}

				return _queryableCollection;
			}
		}


		public Expression Expression
		{
			get
			{
				return QueryableCollection.Expression;
			}
		}

		public Type ElementType
		{
			get
			{
				return QueryableCollection.ElementType;
            }
		}

		public IQueryProvider Provider
		{
			get
			{
				return QueryableCollection.Provider;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return QueryableCollection.GetEnumerator();
        }

		IEnumerator IEnumerable.GetEnumerator()
		{
			return QueryableCollection.GetEnumerator();
		}

		#endregion
	}
}