using System;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MausWorks.MongoDB
{
    public class EntitySet<T> : IQueryable<T> where T : class, new()
    {
		private Lazy<MongoCollection<T>> _collection;

		public MongoCollection<T> Collection { get { return _collection.Value; } }

		public EntitySet(MongoDatabase db, string collectionName)
		{
			_collection = new Lazy<MongoCollection<T>>(() => db.GetCollection<T>(collectionName));
		}

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