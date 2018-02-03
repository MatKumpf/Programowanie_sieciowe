using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SMTPLibrary
{
    public class SMTP
    {
        private const string CRLF = "\r\n";
        private static TcpClient _client;

        public static SendResult SendMail(string smtpAddress, int port, string username, string password, string from, string to, string subject, string data)
        {
            try
            {
                _client = new TcpClient(smtpAddress, port);

                if (!_client.Connected)
                {
                    return new SendResult(false, "Wrong Smtp address or port.");
                }
            }
            catch
            {
                return new SendResult(false, "Wrong Smtp address or port.");
            }

            using (var stream = _client.GetStream())
            {
                var resultText = Read(stream);
                if (!resultText.Contains("220"))
                {
                    return new SendResult(false, "Connection problem. Try again later.");
                }

                Send(Command.AUTH, stream);

                resultText = Read(stream);

                if (!resultText.Contains("VXNlcm5hbWU6"))
                {
                    return new SendResult(false, "Connection problem. Try again later.");
                }

                Send(Command.USERNAME, stream, username);
                resultText = Read(stream);

                if (!resultText.Contains("UGFzc3dvcmQ6"))
                {
                    return new SendResult(false, "Wrong username or password.");
                }

                Send(Command.PASSWORD, stream, password);
                resultText = Read(stream);

                if (!resultText.Contains("235"))
                {
                    return new SendResult(false, "Wrong username or password.");
                }

                Send(Command.MAIL, stream, from);
                resultText = Read(stream);

                if (!resultText.Contains("250"))
                {
                    return new SendResult(false, "Wrong From address.");
                }

                Send(Command.RCPT, stream, to);
                resultText = Read(stream);

                if (!resultText.Contains("250"))
                {
                    return new SendResult(false, "Wrong To address.");
                }

                Send(Command.DATA, stream);
                resultText = Read(stream);

                if (!resultText.Contains("354"))
                {
                    return new SendResult(false, "Connection problem. Try again later.");
                }

                Send(Command.DATA_PREP, stream, from, to, subject, data);
                resultText = Read(stream);
                if (!resultText.Contains("250"))
                {
                    return new SendResult(false, "Connection problem. Try again later.");
                }
            }

            _client.Close();

            return new SendResult(true, "Success.");

        }

        private static void Send(Command command, NetworkStream stream, params string[] param)
        {
            byte[] message;
            
            message = (byte[])typeof(SMTP).GetMethod(command.ToString(), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance).Invoke(null, param);

            var resultWrite = stream.WriteAsync(message, 0, message.Length);
            resultWrite.Wait(2000);
        }

        private static string Read(NetworkStream stream)
        {
            string resultText = "";
            byte[] buf = new byte[_client.ReceiveBufferSize];

            Array.Clear(buf, 0, buf.Length);
            var result = stream.ReadAsync(buf, 0, buf.Length);
            result.Wait(5000);

            if(!result.IsCompleted)
            {
                return resultText;
            }

            resultText += Encoding.ASCII.GetString(buf.Take(result.Result).ToArray());

            return resultText;
        }

        private static byte[] AUTH()
        {
            return Encoding.ASCII.GetBytes($"AUTH LOGIN{CRLF}");
        }

        private static byte[] USERNAME(string username)
        {
            var usernameBytes = Encoding.ASCII.GetBytes(username);
            var usernameBase64 = Convert.ToBase64String(usernameBytes) + CRLF;

            return Encoding.ASCII.GetBytes(usernameBase64);
        }
        
        private static byte[] PASSWORD(string password)
        {
            var passwordBytes = Encoding.ASCII.GetBytes(password);
            var passwordBase64 = Convert.ToBase64String(passwordBytes) + CRLF;

            return Encoding.ASCII.GetBytes(passwordBase64);
        }

        private static byte[] HELO(string domain)
        {
            return Encoding.ASCII.GetBytes($"HELO {domain}{CRLF}");
        }

        private static byte[] EHLO(string domain)
        {
            return Encoding.ASCII.GetBytes($"EHLO {domain}{CRLF}");
        }

        private static byte[] MAIL(string from)
        {
            return Encoding.ASCII.GetBytes($"MAIL FROM:<{from}>{CRLF}");
        }

        private static byte[] RCPT(string to)
        {
            return Encoding.ASCII.GetBytes($"RCPT TO:<{to}>{CRLF}");
        }

        private static byte[] DATA()
        {
            return Encoding.ASCII.GetBytes($"DATA{CRLF}");
        }

        private static byte[] DATA_PREP(string from, string to, string subject, string text)
        {
            return Encoding.UTF8.GetBytes($"Content-Type: text/html; charset=UTF-8{CRLF}From: {from}{CRLF}To: {to}{CRLF}Subject: {subject}{CRLF}{text}{CRLF}.{CRLF}");
        }

        private static byte[] QUIT()
        {
            return null;
        }

        enum Command
        {
            HELO, EHLO, MAIL, RCPT, DATA, DATA_PREP, QUIT, AUTH, USERNAME, PASSWORD
        }
    }
}
