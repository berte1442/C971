using System;
using System.Collections.Generic;
using System.Text;

namespace C971
{
    public interface ISmsTask
    {
        bool CanSendSms { get; }
        bool CanSendSmsInBackground { get; }
        void SendSms(string recipient, string message);
        void SendSmsInBackground(string recipient, string message);
    }
}
