using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

		public void Register<T>(string resourceKey) where T : Exception
		{
			if(_map.ContainsKey(typeof(T)))
				throw new KeyIsTakenException();

			_map.Add(typeof(T), resourceKey);
		}

		public void Unregister<T>() where T : Exception
		{
			_map.Remove(typeof(T));
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
