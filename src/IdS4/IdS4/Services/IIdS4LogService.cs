using IdS4.Logs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdS4.Services
{
    public interface IIdS4LogService
    {
        void Info(string msg);

        void Debug(string msg);

        void Error(Exception e);

        void Fatal(Exception e);

        void Fatal(string msg);

        void Warn(string msg);

        Task<List<Log>> SearchAsync(LogLevel? level = null, (DateTimeOffset? Begin, DateTimeOffset? End)? range = null);
    }
}
