using System;
using System.ComponentModel;
using System.Timers;

namespace VTEX.Configuration.DynamicSection
{
    /// <summary>
    /// Specialized timer that run every minute on '00' second
    /// </summary>
    internal class OnClockMinuteTimer
    {
        private Timer timer = null;
        private int lastExecutedMinute = -1;

        /// <summary>
        /// Event called on '00' second every time
        /// </summary>
        [Category("Behavior")]
        [TimersDescription("TimerIntervalElapsed")]
        public event ElapsedEventHandler Elapsed = delegate { };

        /// <summary>
        /// Create a OnClockMinuteTimer instance initializing internal timer
        /// </summary>
        public OnClockMinuteTimer()
        {
            this.timer = new Timer();
            this.timer.Elapsed += SummarizerTimer_Elapsed;
        }

        /// <summary>
        /// Start timer
        /// </summary>
        public void Start()
        {
            this.timer.Interval = this.GetTimerInterval();
            this.timer.Start();
        }

        /// <summary>
        /// Stop timer
        /// </summary>
        public void Stop()
        {
            this.timer.Stop();
        }

        private void SummarizerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var currentMinute = DateTime.Now.Minute;

            if (currentMinute != lastExecutedMinute)
            {
                this.Stop();
                this.Elapsed(sender, e);
                lastExecutedMinute = currentMinute;
                this.Start();
            }
        }

        private double GetTimerInterval()
        {
            var dateTimeNow = DateTime.Now;
            var baseDate = dateTimeNow.AddMinutes(1);
            var nextTickDateTime = new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, baseDate.Hour, baseDate.Minute, 0);
            return (nextTickDateTime - dateTimeNow).TotalMilliseconds;
        }
    }
}