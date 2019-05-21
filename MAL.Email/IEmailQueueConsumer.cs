using System;
using System.Collections.Generic;
using System.Text;

namespace MAL.Email
{
    public interface IEmailQueueConsumer : IDisposable
    {
        void Cancel();
    }
}
