using System;

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
    }
}
