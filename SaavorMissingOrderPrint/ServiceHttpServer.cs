using System;
using System.Net;
using System.Text;
using System.Threading;

namespace SaavorMissingOrderPrint
{
    /// <summary>
    /// ServiceHttpServer
    /// </summary>
    public class ServiceHttpServer
    {
        private HttpListener _listener;
        private Thread _serverThread;
        private bool _isRunning = false;

        public void Start(string prefix = "http://localhost:8080/")
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException("HttpListener not supported on this OS.");

            _listener = new HttpListener();
            if (!_isRunning)   // ✅ check if already running
            {
                _listener.Prefixes.Add(prefix);
                _listener.Start();
                _isRunning = true;
                _serverThread = new Thread(HandleRequests);
                _serverThread.Start();
            }
           
        }

        private void HandleRequests()
        {
            while (_isRunning)
            {
                try
                {
                    var context = _listener.GetContext();
                    ProcessRequest(context);
                }
                catch { /* ignore */ }
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            string path = context.Request.Url.AbsolutePath.ToLower();
            string responseText;
            switch (path)
            {
                case "/hello":
                    responseText = "Hello from Windows Service!";
                    break;

                case "/time":
                    responseText = $"Server time is: {DateTime.Now}";
                    break;

                default:
                    responseText = "Not Found";
                    break;
            }

            byte[] buffer = Encoding.UTF8.GetBytes(responseText);
            context.Response.ContentType = "text/plain";
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }

        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
            _serverThread.Abort();
        }
    }
}
