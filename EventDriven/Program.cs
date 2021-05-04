using System;
using System.Timers;

namespace EventDriven
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Application started at" + DateTime.Now);

            var eventSource = new EventSource();
            var subscriber1 = new Subscriber(eventSource);        

            eventSource.Start();
            Console.WriteLine("Press any key to quit");
            Console.ReadLine();
        }
    }

    internal class EventSource
    {
        private Timer timer;

        internal event EventHandler<EventSourceEventTypeEventArgs> RaiseEvent;
        internal EventSource()
        {
            timer = new Timer(1000);
        }

        internal void Start()
        {
            timer.Elapsed += OnTimerElapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (RaiseEvent != null)
            {
                OnRaiseEvent(new EventSourceEventTypeEventArgs { EventClass = EventDriven.EventClass.Warning , Message = $"Hallo here is a waring sent at {DateTime.Now}" });
            }
        }

        protected virtual void OnRaiseEvent(EventSourceEventTypeEventArgs eventSourceEventTypeEventArgs)
        {
            EventHandler<EventSourceEventTypeEventArgs> raiseEvent = RaiseEvent;
            if (raiseEvent != null)
            {
                raiseEvent(this, eventSourceEventTypeEventArgs);
            }
        }
    }

    internal class Subscriber
    {
        public Subscriber(EventSource eSource)
        {
            eSource.RaiseEvent += HandleEvent;
        }

        private void HandleEvent (object sender, EventSourceEventTypeEventArgs args)
        {
            Console.WriteLine($"Received event at: {DateTime.Now}, Message class is {args.EventClass}, Message is {args.Message}");
        }
    }

    internal class EventSourceEventTypeEventArgs : EventArgs
    {
        internal string EventClass { get; set; }
        internal string Message { get; set; }
    }

    internal class EventClass
    {
        internal const string Warning = "Warning";
        internal const string Info = "Info";
    }
}
