﻿namespace ErrorOr;

/// <summary>
/// A discriminated union of errors or a value.
/// </summary>
public record struct ErrorOr<TValue> : IErrorOr
{
    private readonly TValue? _value = default;
    private readonly List<Error>? _errors = null;

    /// <summary>
    /// Gets a value indicating whether the state is error.
    /// </summary>
    public bool IsError { get; }

    /// <summary>
    /// Gets the list of errors.
    /// </summary>
    public List<Error> Errors
    {
        get
        {
            if (!IsError)
            {
                throw new InvalidOperationException("Errors can be retrieved only when the result is an error.");
            }

            return _errors!;
        }
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from a list of errors.
    /// </summary>
    public static ErrorOr<TValue> From(List<Error> errors)
    {
        return errors;
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    public TValue Value
    {
        get
        {
            if (IsError)
            {
                throw new InvalidOperationException("Value can be retrieved only when the result is not an error.");
            }

            return _value!;
        }
    }

    /// <summary>
    /// Gets the first error.
    /// </summary>
    public Error FirstError
    {
        get
        {
            if (!IsError)
            {
                throw new InvalidOperationException();
            }

            return _errors![0];
        }
    }

    private ErrorOr(Error error)
    {
        _errors = new List<Error> { error };
        IsError = true;
    }

    private ErrorOr(List<Error> errors)
    {
        _errors = errors;
        IsError = true;
    }

    private ErrorOr(TValue value)
    {
        _value = value;
        IsError = false;
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from a value.
    /// </summary>
    public static implicit operator ErrorOr<TValue>(TValue value)
    {
        return new ErrorOr<TValue>(value);
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from an error.
    /// </summary>
    public static implicit operator ErrorOr<TValue>(Error error)
    {
        return new ErrorOr<TValue>(error);
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from a list of errors.
    /// </summary>
    public static implicit operator ErrorOr<TValue>(List<Error> errors)
    {
        return new ErrorOr<TValue>(errors);
    }

    /// <summary>
    /// Creates an <see cref="ErrorOr{TValue}"/> from a list of errors.
    /// </summary>
    public static implicit operator ErrorOr<TValue>(Error[] errors)
    {
        return new ErrorOr<TValue>(errors.ToList());
    }

    public void Switch(Action<TValue> onValue, Action<List<Error>> onError)
    {
        if (IsError)
        {
            onError(Errors);
            return;
        }

        onValue(Value);
    }

    public void SwitchFirst(Action<TValue> onValue, Action<Error> onFirstError)
    {
        if (IsError)
        {
            onFirstError(FirstError);
            return;
        }

        onValue(Value);
    }

    public TResult Match<TResult>(Func<TValue, TResult> onValue, Func<List<Error>, TResult> onError)
    {
        if (IsError)
        {
            return onError(Errors);
        }

        return onValue(Value);
    }

    public TResult MatchFirst<TResult>(Func<TValue, TResult> onValue, Func<Error, TResult> onFirstError)
    {
        if (IsError)
        {
            return onFirstError(FirstError);
        }

        return onValue(Value);
    }
}
