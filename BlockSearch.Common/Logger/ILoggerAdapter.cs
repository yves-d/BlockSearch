﻿namespace BlockSearch.Common.Logger
{
    public interface ILoggerAdapter<T>
    {
        void LogInformation(string message, params object[] args);
        void LogError(string message, params object[] args);
    }
}
