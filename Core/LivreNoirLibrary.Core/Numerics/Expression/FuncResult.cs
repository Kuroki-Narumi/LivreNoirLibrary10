using System;

namespace LivreNoirLibrary.Numerics
{
    public readonly struct FuncResult<T>(T value, Exception? exception)
    {
        public readonly T Value = value;
        public readonly Exception? Exception = exception;

        public bool IsSuccessful => Exception is null;
        public bool HasException => Exception is not null;

        public static implicit operator FuncResult<T>(T value) => new(value, null);
        public static implicit operator FuncResult<T>(Exception exception) => new(default!, exception);
    }
}
