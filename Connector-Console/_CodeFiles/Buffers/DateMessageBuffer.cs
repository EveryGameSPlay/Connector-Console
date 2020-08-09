using System;
using System.Text;

namespace Connector.Buffers
{
    public class DateMessageBuffer: MessageBuffer
    {
        public DateMessageBuffer() : base()
        {
        }

        public DateMessageBuffer(string id) : base(id)
        {
        }

        public override void Add(string newMessage)
        {
            var builder = new StringBuilder(8);

            var hour = DateTime.Now.Hour;
            var minute = DateTime.Now.Minute;

            builder.Append('[');
            builder.Append(hour.ToString());
            builder.Append(':');
            builder.Append(minute.ToString());
            builder.Append(']');
            builder.Append(' ');
            builder.Append(newMessage);
            
            base.Add(builder.ToString());
        }
    }
}