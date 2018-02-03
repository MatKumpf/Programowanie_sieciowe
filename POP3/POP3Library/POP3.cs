using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace POP3Library
{
    public static class POP3
    {
        private static TcpClient _client;
        private static string CRLF = "\r\n";

        public static ResultDownloadMessages DownloadMessages(string pop3Server, int port, string username, string password, ReceivedMail[] prevReceivedMails = null)
        {

            try
            {
                _client = new TcpClient(pop3Server, port);

                if(!_client.Connected)
                {
                    return new ResultDownloadMessages(false, "Wrong POP3 server address or port.", null);
                }
            }
            catch
            {
                return new ResultDownloadMessages(false, "Wrong POP3 server address or port.", null);
            }

            using (var stream = _client.GetStream())
            {
                var resultText = Read(stream);

                if (!resultText.Contains("+OK"))
                {
                    return new ResultDownloadMessages(false, "Connection problem. Try again later.", null);
                }

                Send(stream, Command.USER, username);

                resultText = Read(stream);

                if(!resultText.Contains("+OK"))
                {
                    return new ResultDownloadMessages(false, "Wrong username or password.", null);
                }

                Send(stream, Command.PASS, password);

                resultText = Read(stream);

                if(!resultText.Contains("+OK"))
                {
                    return new ResultDownloadMessages(false, "Wrong username or password.", null);
                }

                Dictionary<string, string> uidlResult = new Dictionary<string, string>();

                Regex regex = new Regex(@"^(?<id>\d*)\s(?<token>[\x21-\x7E]{1,70})$");

                Send(stream, Command.UIDL, "");
                resultText = Read(stream, Command.UIDL);

                var splitResultText = resultText.Split(new string[] { CRLF }, StringSplitOptions.RemoveEmptyEntries);
                
                for(int i = 0; i < splitResultText.Length; i++)
                {
                    if(i == 0)
                    {
                        if(splitResultText[i].Contains("+OK"))
                        {
                            continue;
                        }
                    }

                    var result = regex.Match(splitResultText[i]);

                    if (result.Length != 0)
                    {
                        uidlResult.Add(result.Groups["id"].Value, result.Groups["token"].Value);
                    }
                }

                ReceivedMail[] receivedMail = CreateReceivedMails(stream, uidlResult, prevReceivedMails);

                return new ResultDownloadMessages(true, "Success", receivedMail);
            }

        }

        private static ReceivedMail[] CreateReceivedMails(NetworkStream stream, Dictionary<string, string> uidl, ReceivedMail[] prevReceivedMails)
        {
            if (prevReceivedMails != null)
            {
                string[] prevReceivedTokens = TokensMail(prevReceivedMails);
                FindAndRemoveEquals(uidl, prevReceivedTokens);
            }

            List<ReceivedMail> list = new List<ReceivedMail>();

            Regex regexFrom = new Regex(@"From:\s(?<from>[\x21-\x7E].*)");
            Regex regexSubject = new Regex(@"Subject:\s(?<subject>[\x21-\x7E].*)");

            foreach(var id in uidl)
            {
                Send(stream, Command.RETR, id.Key);

                var resultText = Read(stream, Command.RETR);

                var from = regexFrom.Match(resultText).Groups["from"].Value.Replace("\r", "").Replace("\n", "");
                var subject = regexSubject.Match(resultText).Groups["subject"].Value.Replace("\r", "").Replace("\n", "");

                list.Add(new ReceivedMail(id.Value, from , subject));
            }

            return list.ToArray();
        }

        private static string[] TokensMail(ReceivedMail[] receivedMail)
        {
            List<string> tokens = new List<string>();

            foreach(var mail in receivedMail)
            {
                tokens.Add(mail.Token);
            }

            return tokens.ToArray();
        }

        private static void FindAndRemoveEquals(Dictionary<string, string> uidl, string[] prevReceivedTokens)
        {
            string[] tokens = uidl.Values.ToArray();

            foreach(var token in tokens)
            {
                if(prevReceivedTokens.Contains(token))
                {
                    uidl.Remove(uidl.First(k => k.Value == token).Key);
                }
            }
        }

        private static bool Send(NetworkStream stream, Command command, params string[] param)
        {
            byte[] message;

            message = (byte[])typeof(POP3).GetMethod(command.ToString(), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance).Invoke(null, param);

            var resultWrite = stream.WriteAsync(message, 0, message.Length);
            resultWrite.Wait(2000);

            return resultWrite.IsCompleted;

        }

        private static string Read(NetworkStream stream, Command command = default(Command))
        {
            string resultText = "";
            byte[] buf = new byte[_client.ReceiveBufferSize];

            while (true)
            {
                Array.Clear(buf, 0, buf.Length);
                var result = stream.ReadAsync(buf, 0, buf.Length);
                result.Wait(2000);

                if (!result.IsCompleted)
                {
                    return resultText;
                }

                resultText += Encoding.UTF8.GetString(buf.Take(result.Result).ToArray());

                if (command == Command.RETR || command == Command.UIDL)
                {
                    if (resultText.Contains($"{CRLF}.{CRLF}"))
                    {
                        break;
                    }
                }
                else
                {
                    if (result.Result != buf.Length)
                    {
                        break;
                    }
                }
            }

            return resultText;
        }

        private static byte[] USER(string userName)
        {
            return Encoding.ASCII.GetBytes($"USER {userName}{CRLF}");
        }

        private static byte[] PASS(string password)
        {
            return Encoding.ASCII.GetBytes($"PASS {password}{CRLF}");
        }

        private static byte[] UIDL(string id)
        {
            if (id != "")
            {
                return Encoding.ASCII.GetBytes($"UIDL {id}{CRLF}");
            }
            else
            {
                return Encoding.ASCII.GetBytes($"UIDL{CRLF}");
            }
        }

        private static byte[] RETR(string id)
        {
            return Encoding.ASCII.GetBytes($"RETR {id}{CRLF}");
        }

        private static byte[] QUIT()
        {
            return Encoding.ASCII.GetBytes($"QUIT{CRLF}");
        }

        enum Command
        {
            DEFAULT, USER, PASS, UIDL, RETR, QUIT
        }

    }
}
