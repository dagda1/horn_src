#region license
// Copyright (c) 2005 - 2007 Ayende Rahien (ayende@ayende.com)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Ayende Rahien nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

namespace Rhino.Commons
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Data;
	using System.IO;
	using System.Xml;
	using global::NHibernate;
	using global::NHibernate.Dialect;
	using global::NHibernate.Engine;
	using global::NHibernate.Criterion;
	using global::NHibernate.SqlCommand;
	using global::NHibernate.SqlTypes;
	using global::NHibernate.Type;
	using global::NHibernate.UserTypes;
	using global::NHibernate.Util;
    using global::NHibernate.Persister.Entity;

	public class XmlIn : AbstractCriterion
	{
		private readonly AbstractCriterion expr;
		private readonly string propertyName;
		private readonly object[] values;
        private readonly int maximumNumberOfParametersToNotUseXml = 100;

		public static AbstractCriterion Create(string property, IEnumerable values)
		{
			return new XmlIn(property, values);
		}


        /// <summary>
        /// Creates the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="values">The values.</param>
        /// <param name="maximumNumberOfParametersToNotUseXml">The maximum number of paramters allowed before the XmlIn creates an xml string.</param>
        /// <returns></returns>
        public static AbstractCriterion Create(string property, IEnumerable values, int maximumNumberOfParametersToNotUseXml)
        {
            return new XmlIn(property, values, maximumNumberOfParametersToNotUseXml);
        }

        public XmlIn(string propertyName, IEnumerable values, int maximumNumberOfParametersToNotUseXml)
        : this(propertyName, values)
        {
            this.maximumNumberOfParametersToNotUseXml = maximumNumberOfParametersToNotUseXml;
        }

	    public XmlIn(string propertyName, IEnumerable values)
		{
			this.propertyName = propertyName;
			ArrayList arrayList = new ArrayList();
			foreach (object o in values)
			{
				arrayList.Add(o);
			}
			this.values = arrayList.ToArray();
			expr = Expression.In(propertyName, arrayList);
		}

		public override string ToString()
		{
			return propertyName + " big in (" + StringHelper.ToString(values) + ')';
		}

		public override SqlString ToSqlString(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string,IFilter> enabledFilters)
		{
			//we only need this for SQL Server, and or large amount of values
            if ((criteriaQuery.Factory.Dialect is MsSql2005Dialect) == false || values.Length < maximumNumberOfParametersToNotUseXml)
			{
				return expr.ToSqlString(criteria, criteriaQuery, enabledFilters);
			}
			IType type = criteriaQuery.GetTypeUsingProjection(criteria, propertyName);
			if (type.IsCollectionType)
			{
				throw new QueryException("Cannot use collections with InExpression");
			}

			if (values.Length == 0)
			{
				// "something in ()" is always false
				return new SqlString("1=0");
			}

			SqlStringBuilder result = new SqlStringBuilder();
			string[] columnNames = criteriaQuery.GetColumnsUsingProjection(criteria, propertyName);

			// Generate SqlString of the form:
			// columnName1 in (xml query) and columnName2 in (xml query) and ...
			criteriaQuery.AddUsedTypedValues(this.GetTypedValues(criteria, criteriaQuery));			

			for (int columnIndex = 0; columnIndex < columnNames.Length; columnIndex++)
			{
				string columnName = columnNames[columnIndex];

				if (columnIndex > 0)
				{
					result.Add(" and ");
				}
				SqlType sqlType = type.SqlTypes(criteriaQuery.Factory)[columnIndex];
				result
					.Add(columnName)
					.Add(" in (")
					.Add("SELECT ParamValues.Val.value('.','")
					.Add(criteriaQuery.Factory.Dialect.GetTypeName(sqlType))
					.Add("') FROM ")
					.AddParameter()
					.Add(".nodes('/items/val') as ParamValues(Val)")
					.Add(")");
			}

			return result.ToSqlString();
		}

		public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			//we only need this for SQL Server, and or large amount of values
            if ((criteriaQuery.Factory.Dialect is MsSql2005Dialect) == false || values.Length < maximumNumberOfParametersToNotUseXml)
			{
				return expr.GetTypedValues(criteria, criteriaQuery);
			}

			IEntityPersister persister = null;
			IType type = criteriaQuery.GetTypeUsingProjection(criteria, propertyName);
			
			if (type.IsEntityType)
			{
				persister = criteriaQuery.Factory.GetEntityPersister(type.ReturnedClass.FullName);
			}
			StringWriter sw = new StringWriter();
			XmlWriter writer = XmlWriter.Create(sw);
			writer.WriteStartElement("items");
			foreach (object value in values)
			{
				if (value == null)
					continue;
				object valToWrite;
				if (persister != null)
					valToWrite = persister.GetIdentifier(value, EntityMode.Poco);
				else
					valToWrite = value;
				writer.WriteElementString("val", valToWrite.ToString());
			}
			writer.WriteEndElement();
			writer.WriteEndDocument();
			writer.Flush();
			string xmlString = sw.GetStringBuilder().ToString();

			return new TypedValue[] { 
				new TypedValue(new CustomType(typeof(XmlType), 
				new Dictionary<string, string>()), xmlString, EntityMode.Poco), };
		}

		private class XmlType : IUserType
		{
			private static readonly SqlType[] sqlTypes = new SqlType[] { new SqlType(DbType.Xml) };
			private readonly Type returnedType = typeof(string);
			private readonly bool isMutable = false;

			public SqlType[] SqlTypes
			{
				get { return sqlTypes; }
			}

			public Type ReturnedType
			{
				get { return returnedType; }
			}

			public new bool Equals(object x, object y)
			{
				return Object.Equals(x, y);
			}

			public int GetHashCode(object x)
			{
				return x.GetHashCode();
			}

			public object NullSafeGet(IDataReader rs, string[] names, object owner)
			{
				return null;
			}

			public void NullSafeSet(IDbCommand cmd, object value, int index)
			{
				IDataParameter parameter = (IDataParameter)cmd.Parameters[index];
				parameter.Value = value;
			}

			public object DeepCopy(object value)
			{
				return value;
			}

			public bool IsMutable
			{
				get { return isMutable; }
			}

			public object Replace(object original, object target, object owner)
			{
				return original;
			}

			public object Assemble(object cached, object owner)
			{
				return cached;
			}

			public object Disassemble(object value)
			{
				return value;
			}
		}

		public override IProjection[] GetProjections()
		{
			return null;
		}
	}
}
