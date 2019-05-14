using IdS4.Logs;
using IdS4.Repositories;
using RajsLibs.Repositories.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdS4.Services.Impls
{
    public class IdS4LogService : IIdS4LogService
    {
        private readonly ILogRepository _logRepository = null;

        public IdS4LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public void Debug(string msg)
        {
            _logRepository.Add(Log.New(LogLevel.Debug, msg));
        }

        public void Error(Exception e)
        {
            _logRepository.Add(Log.New(LogLevel.Error, e.ToString()));
        }

        public void Fatal(Exception e)
        {
            _logRepository.Add(Log.New(LogLevel.Fatal, e.ToString()));
        }

        public void Fatal(string msg)
        {
            _logRepository.Add(Log.New(LogLevel.Fatal, msg));
        }

        public void Info(string msg)
        {
            _logRepository.Add(Log.New(LogLevel.Info, msg));
        }

        public async Task<List<Log>> SearchAsync(LogLevel? level = null, (DateTimeOffset? Begin, DateTimeOffset? End)? range = null)
        {
            var query = new PageQuery<Log>.Builder()
                .Filter(level.HasValue, s => s.Level == level)
                .Filter(range.HasValue && range?.Begin.HasValue == true, s => s.TimeStamp >= range.Value.Begin)
                .Filter(range.HasValue && range?.End.HasValue == true, s => s.TimeStamp <= range.Value.End)
                .OrderBy("TimeStamp")
                .Descending()
                .Skip(0)
                .Take(50)
                .Build();

           var paging = await _logRepository.PagingAsync(query);
           return paging.ToList();
        }

        public void Warn(string msg)
        {
            _logRepository.Add(Log.New(LogLevel.Warn, msg));
        }
    }
}
