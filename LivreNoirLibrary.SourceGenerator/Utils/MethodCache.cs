using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

public enum CoerceType
{
    None,
    Static,
    Instance,
}

internal class MethodCacheItem
{
    private readonly Dictionary<(string, string), bool> _validate_methods = [];
    private readonly Dictionary<string, int> _changed_methods = [];

    public void AddCoerce(string propertyType, string fieldType, bool isStatic) => _validate_methods.Add((propertyType, fieldType), isStatic);
    public void AddOnChanged(string fieldType, int argCount)
    {
        if (!_changed_methods.TryGetValue(fieldType, out var current) || current < argCount)
        {
            _changed_methods[fieldType] = argCount;
        }
    }

    public bool ContainsCoerce(string propertyType, string fieldType, out bool isStatic) => _validate_methods.TryGetValue((propertyType, fieldType), out isStatic);
    public bool ContainsOnChanged(string fieldType, out int argCount) => _changed_methods.TryGetValue(fieldType, out argCount);
}

internal class MethodCache : Dictionary<string, MethodCacheItem>
{
    private static readonly Regex Regex_Coerce = new(@"^Coerce(.+?)$");
    private static readonly Regex Regex_OnChanged = new(@"^On(.+?)Changed$");

    private MethodCacheItem GetItem(string propertyName)
    {
        if (!TryGetValue(propertyName, out var cache))
        {
            cache = new();
            Add(propertyName, cache);
        }
        return cache;
    }

    public void AddCoerceMethod(string propertyName, IMethodSymbol method)
    {
        if (method.Parameters.Length is 1 && !method.ReturnsVoid)
        {
            var propertyType = Utils.GetTypeFullname(method.Parameters[0].Type);
            var fieldType = Utils.GetTypeFullname(method.ReturnType);
            GetItem(propertyName).AddCoerce(propertyType, fieldType, method.IsStatic);
        }
    }

    public void AddOnChangedMethod(string propertyName, IMethodSymbol method)
    {
        var @params = method.Parameters;
        if (method.ReturnsVoid && !method.IsStatic &&
            (@params.Length is 1 || @params.Length is 2 && @params[0].Type.Equals(@params[1].Type, SymbolEqualityComparer.Default))
            )
        {
            var fieldType = Utils.GetTypeFullname(@params[0].Type);
            GetItem(propertyName).AddOnChanged(fieldType, @params.Length);
        }
    }

    public CoerceType CheckCoerce(string methodName, string propertyType, string fieldType) => 
        TryGetValue(methodName, out var cache) && cache.ContainsCoerce(propertyType, fieldType, out var isStatic) 
        ? (isStatic ? CoerceType.Static : CoerceType.Instance)
        : CoerceType.None;
    public int CheckOnChange(string methodName, string fieldType) => TryGetValue(methodName, out var cache) && cache.ContainsOnChanged(fieldType, out var argCount) ? argCount : 0;

    private static readonly Dictionary<INamedTypeSymbol, MethodCache> _method_cache = [];

    public static MethodCache Get(INamedTypeSymbol typeSymbol)
    {
        if (!_method_cache.TryGetValue(typeSymbol, out var methods))
        {
            methods = [];
            foreach (var method in typeSymbol.GetMembers().OfType<IMethodSymbol>())
            {
                var match = Regex_Coerce.Match(method.Name);
                if (match.Success)
                {
                    methods.AddCoerceMethod(match.Groups[1].Value, method);
                    continue;
                }
                match = Regex_OnChanged.Match(method.Name);
                if (match.Success)
                {
                    methods.AddOnChangedMethod(match.Groups[1].Value, method);
                    continue;
                }
            }
            _method_cache.Add(typeSymbol, methods);
        }
        return methods;
    }
}