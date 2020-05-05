using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;

namespace Site.Utils
{
    public static class PresentationMessage
    {
        public static void ShowMessegeQueue(SnackbarMessageQueue messageQueue, string message)
        {
            Task.Factory.StartNew(() => messageQueue.Enqueue(message));
        }
    }
}
