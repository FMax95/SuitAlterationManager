using System;
using System.Text.Json;

namespace SuitAlterationManager.Api.Client
{  
    [Serializable]
    public partial struct Envelope
    {
        public bool IsSuccess { get; }
        public object Error { get; }
        internal Envelope(object error,bool isSuccess)
        {
            Error = error;
            IsSuccess = isSuccess;
        }
    }
    
    [Serializable]
    public struct Envelope<T>
    {
        public bool IsSuccess { get; }
        public object Error { get; }
        public T Result { get; }
        
        internal Envelope(T value, object error, bool isSuccess)
        {
            Result = value;
            Error = error;
            IsSuccess = isSuccess;
        }
        
        public static implicit operator Envelope<T>(T value)
        {
            if (value is Envelope<T> env)
                return env;
            return Envelope.Success(value);
        }

        public static implicit operator Envelope(Envelope<T> env)
        {
            return env.IsSuccess ? Envelope.Success(true) : Envelope.Failure(env.Error);
        }

    }
    
    public partial struct Envelope
    {
        public static Envelope Success(bool isSuccess) => new Envelope(default, isSuccess);
        public static Envelope Failure(object error) => new Envelope(error, false);
        public static Envelope<T> Success<T>(T result) => new Envelope<T>(result, null,true);
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}