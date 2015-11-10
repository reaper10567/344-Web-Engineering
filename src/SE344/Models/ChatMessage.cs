using System;

namespace SE344.Models
{
    public class ChatMessage
    {
        public long Id { get; set; }

        public string Message { get; set; }

        public DateTime SentAt { get; set; }

        public string SenderId { get; set; }

        /// <summary>
        /// Navigation property to get the sender
        /// </summary>
        public virtual ApplicationUser Sender { get; set; }
    }
}
