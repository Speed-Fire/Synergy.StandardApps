using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Synergy.StandardApps.Resources
{
	public class ExceptionTranslator
	{
		private static ExceptionTranslator _instance = null;
		public static ExceptionTranslator Instance => _instance ??= new();
		
		private Dictionary<Type, string> _map;

		private ExceptionTranslator()
		{
			_map = new();
		}

		public ExceptionTranslator Register<T>(string resourceKey) where T : Exception
		{
			if(_map.ContainsKey(typeof(T)))
				throw new KeyIsTakenException();

			_map.Add(typeof(T), resourceKey);

			return this;
		}

		public ExceptionTranslator Unregister<T>() where T : Exception
		{
			_map.Remove(typeof(T));

			return this;
		}

		public object GetValue<T>() where T : Exception
		{
			if(_map.TryGetValue(typeof(T), out var value))
			{
				var resource = Application.Current.TryFindResource(value)
					?? throw new ResourceNotFoundException();

				return resource;
			}
			else
				throw new KeyNotFoundException(typeof(T).Name);
		}

		public TValue GetValue<TKey, TValue>() where TKey : Exception
		{
			if (_map.TryGetValue(typeof(TKey), out var value))
			{
				var resource = Application.Current.TryFindResource(value)
					?? throw new ResourceNotFoundException();

				return (TValue)resource;
			}
			else
				throw new KeyNotFoundException(typeof(TKey).Name);
		}

		public bool TryGetValue<T>(out object value) where T : Exception
		{
			value = null;

			if (_map.TryGetValue(typeof(T), out var val))
			{
				value = Application.Current.TryFindResource(val)
					?? throw new ResourceNotFoundException();

				return true;
			}
			else
			{
				return false;
			}
		}

		public bool TryGetValue<TKey, TValue>(out TValue value) where TKey : Exception
		{
			value = default;

			if (_map.TryGetValue(typeof(TKey), out var val))
			{
				value = (TValue)Application.Current.TryFindResource(val)
					?? throw new ResourceNotFoundException();

				return true;
			}
			else
			{
				return false;
			}
		}
	}

	public class KeyIsTakenException : Exception
	{
		public KeyIsTakenException()
			: base("Specified type of exception is already registered!")
		{

		}
	}

	public class ResourceNotFoundException : Exception
	{
		public ResourceNotFoundException()
			: base("Specified resource name is not registered in the current application!")
		{

		}
	}
}
