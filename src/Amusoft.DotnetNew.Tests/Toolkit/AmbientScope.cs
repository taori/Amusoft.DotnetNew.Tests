using System;
using System.Collections.Generic;
using System.Threading;

namespace Amusoft.DotnetNew.Tests.Toolkit;

/// <summary>
/// Ambient scope utility class
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class AmbientScope<T> : IDisposable
	where T: AmbientScope<T>
{
	private static readonly AsyncLocal<Stack<T>> Scopes = new()
	{
		Value = new Stack<T>()
	};

	// ReSharper disable once InconsistentNaming
	private static readonly AsyncLocal<T?> _current = new ();
	
	/// <summary>
	/// Current ambient instance
	/// </summary>
	public static T? Current => _current.Value;

	private readonly AsyncLocal<T?> _parentScope = new();
	
	/// <summary>
	/// Parent scope of current scope
	/// </summary>
	public T? ParentScope => _parentScope.Value;

	/// <summary>
	/// Parent scopes of current scope
	/// </summary>
	public IEnumerable<T> ParentScopes
	{
		get
		{
			var current = this;
			while (current != null)
			{
				if (current.ParentScope == null || current._parentScope.Value == null)
					break;
				
				yield return current._parentScope.Value;
				current = current._parentScope.Value;
			}
		}
	}

	/// <summary>
	/// Constructor
	/// </summary>
	protected AmbientScope()
	{
		if (_current.Value != null)
		{
			_parentScope.Value = _current.Value;
		}

		if (Scopes.Value == null)
			Scopes.Value = new Stack<T>();

		if (this is T c)
			Scopes.Value.Push(c);
		ChangeCurrentScope(this as T);
	}

	private void ChangeCurrentScope(T? scope)
	{
		if (_current.Value == null)
		{
			if (scope is not null)
			{
				_current.Value = scope;
			}
			else
			{
				_current.Value = null;
			}
		}
		else
		{
			_current.Value = scope;
		}
	}

	private bool _disposed;
	/// <summary>
	/// Dispose method
	/// </summary>
	public void Dispose()
	{
		if(_disposed)
			return;

		_disposed = true;
		
		if (Scopes.Value!.TryPop(out var scope))
		{
			ChangeCurrentScope(scope.ParentScope);
		}
		else
		{
			ChangeCurrentScope(null);
		}
	}
}