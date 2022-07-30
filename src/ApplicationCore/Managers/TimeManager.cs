using ApplicationCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Managers
{
    public interface ITimeManager
    {
        bool InTime { get; }
        System.DateTime BeginTime { get; }
        System.DateTime EndTime { get; }
    }


    public class TimeManager : ITimeManager
    {
        private System.DateTime _beginTime;
        private System.DateTime _endTime;

        public TimeManager(int begin, int end)
        {
            var now = System.DateTime.Now;
            var beginTimes = begin.ToTimes();
            _beginTime = new System.DateTime(now.Year, now.Month, now.Day, beginTimes[0], beginTimes[1], beginTimes[2]);

            var endTimes = end.ToTimes();
            _endTime = new System.DateTime(now.Year, now.Month, now.Day, endTimes[0], endTimes[1], endTimes[2]);

            if (_endTime <= _beginTime) _endTime = _endTime.AddDays(1);
        }

        public System.DateTime BeginTime => _beginTime;
        public System.DateTime EndTime => _endTime;

        public bool InTime => System.DateTime.Now >= _beginTime && System.DateTime.Now <= _endTime;

        
    }

}
