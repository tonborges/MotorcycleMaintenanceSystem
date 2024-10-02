using System.Text.Json.Serialization;
using FluentValidation.Results;

namespace Shared.Results;

public class ServiceResult
{
    [JsonIgnore] public bool IsSuccess { get; private set; }
    [JsonIgnore] public bool IsFailure { get; private set; }
    public IEnumerable<Notification>? Notifications { get; set; }
    public Guid? Id { get; set; }

    public enum StatusResult
    {
        Ok,
        Error,
        Warning,
        Information
    }

    public ServiceResult()
    {
        IsFailure = false;
        IsSuccess = true;
    }

    public ServiceResult(bool success, IEnumerable<string>? messages)
    {
        IsFailure = !success;
        IsSuccess = success;
        Notifications = messages?
            .Select(s => new Notification(s, null));
    }

    public ServiceResult(bool success, IEnumerable<Notification>? notifications)
    {
        IsFailure = !success;
        IsSuccess = success;
        Notifications = notifications;
    }

    public ServiceResult(IEnumerable<string>? messages)
    {
        IsFailure = true;
        IsSuccess = false;
        Notifications = messages?
            .Select(s => new Notification(s, null));
    }
    
    public virtual ServiceResult SetNotification(string message, string? property = null)
    {
        Notifications =
        [
            new(message, property)
        ];
        return this;
    }

    public virtual ServiceResult SetNotifications(IEnumerable<Notification> notifications)
    {
        Notifications = notifications;
        return this;
    }

    public virtual ServiceResult SetId(Guid? id)
    {
        Id = id;
        return this;
    }

    public static ServiceResult Ok() => new(true, messages: null);
    public static ServiceResult Ok(List<string>? messages) => new(true, messages);
    public static ServiceResult Fail() => new(false, messages: null);
    public static ServiceResult Fail(List<string>? messages) => new(false, messages);
    public static ServiceResult Fail(List<ValidationFailure>? messages) => new(false, messages?.Select(s => new Notification(s.ErrorMessage, s.ErrorCode)));


    public static implicit operator ServiceResult(string messages) => new(new List<string> { messages });

    public TResponse Match<TResponse>(Func<ServiceResult, TResponse> onSuccess, Func<ServiceResult, TResponse> onFailure)
        => IsSuccess ? onSuccess(this) : onFailure(this);
}

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; set; }

    public override ServiceResult<T> SetNotifications(IEnumerable<Notification>? notifications)
    {
        Notifications = notifications;
        return this;
    }

    public ServiceResult<T> SetData(T? data)
    {
        Data = data;
        return this;
    }

    public override ServiceResult SetId(Guid? id)
    {
        Id = id;
        return this;
    }

    public ServiceResult(bool success, T? data, List<string>? messages)
        : base(success, messages)
        => Data = data;

    public ServiceResult(T? data)
        : base(true, messages: null)
        => Data = data;

    public ServiceResult(List<string>? messages)
        : base(false, messages)
    {
    }

    public static ServiceResult<T> Ok(T? data) => new(true, data, null);
    public static ServiceResult<T> Ok(T? data, List<string>? messages) => new(true, data, messages);
    public new static ServiceResult<T> Fail(List<string>? messages) => new(false, data: default, messages);
    public static ServiceResult<T> Fail(T? data, List<string>? messages) => new(false, data: data, messages);

    public TResponse Match<TResponse>(Func<ServiceResult<T>, TResponse> onSuccess, Func<ServiceResult<T>, TResponse> onFailure)
        => IsSuccess ? onSuccess(this) : onFailure(this);

    //happy path
    public static implicit operator ServiceResult<T>(T value) => new(value);

    //error path
    public static implicit operator ServiceResult<T>(List<string>? messages) => new(messages);
    public static implicit operator ServiceResult<T>(string messages) => new(new List<string> { messages });
}