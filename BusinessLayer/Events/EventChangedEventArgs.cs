using BusinessLayer.DTOs;

namespace BusinessLayer.Events
{
    public class EventChangedEventArgs : EventArgs
    {
        public EventDto Event { get; }
    
        public EventChangedEventArgs(EventDto @event)
        {
            Event = @event;
           
        }
    }

 
}
