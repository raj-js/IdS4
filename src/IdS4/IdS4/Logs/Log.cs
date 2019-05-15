using RajsLibs.Key;
using System;

namespace IdS4.Logs
{
    public class Log : IKey<long>
    {
        public long Id { get; private set; }

        public LogLevel Level { get; private set; }

        public string Msg { get; private set; }

        public DateTime CreateTime { get; private set; } 

        public Log() { }

        public Log(long id, LogLevel level, string msg, DateTime timeStamp)
        {
            Id = id;
            Level = level;
            Msg = msg ?? throw new ArgumentNullException(nameof(msg));
            CreateTime = timeStamp;
        }

        public static Log New(LogLevel level, string msg)
        {
            return new Log
            {
                Level = level,
                Msg = msg ?? throw new ArgumentNullException(nameof(msg)),
                CreateTime = DateTime.Now
            };
        }
    }
}
