using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FtpLibrary
{
    public static class FTP
    {
        private static string CRLF = "\r\n";
        private static TcpClient _client;

        public static ResultWithFtpObject ObjectsInPath(string uriFtp, string username, string password, string path = "/")
        {
            try
            {
                _client = new TcpClient(uriFtp, 21);

                if(!_client.Connected)
                {
                    return new ResultWithFtpObject(false, "Wrong Ftp address.", null);
                }
            }
            catch
            {
                return new ResultWithFtpObject(false, "Wrong Ftp address.", null);
            }

            using (var stream = _client.GetStream())
            {
                var result = Read(stream);
                if (!result.Contains("220"))
                {
                    return new ResultWithFtpObject(false, "Connection problem. Try again later.", null);
                }

                Send(stream, Command.USER, username);

                result = Read(stream);
                if (!result.Contains("331"))
                {
                    return new ResultWithFtpObject(false, "Wrong username or password.", null);
                }

                Send(stream, Command.PASS, password);

                result = Read(stream);
                if (!result.Contains("230"))
                {
                    return new ResultWithFtpObject(false, "Wrong username or password.", null);
                }

                Send(stream, Command.CWD, path);

                result = Read(stream);
                if (!result.Contains("250"))
                {
                    return new ResultWithFtpObject(false, "Wrong start path or access denied.", null);
                }

                var commandResult = SendCommandWithPassive(stream, Command.LIST);

                if(commandResult.ResultCode.Length == 2)
                {
                    if(commandResult.ResultCode[0] != 150 && commandResult.ResultCode[1] != 226)
                    {
                        return new ResultWithFtpObject(false, "Function \"LIST\" failed.", null);
                    }
                }

                var listLines = commandResult.ResultText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                List<FtpObject> ftpObjectsList = new List<FtpObject>();

                foreach(var line in listLines)
                {
                    ftpObjectsList.Add(new FtpObject(line));
                }

                return new ResultWithFtpObject(true, "Success", ftpObjectsList.ToArray());
            }
        }

        private static ResultCommand SendCommandWithPassive(NetworkStream stream, Command command)
        {
            string result;

            var newClient = PassiveCommand(stream);

            Send(stream, command);

            using (var newStream = newClient.GetStream())
            {
                result = Read(newStream);
            }

            newClient.Close();

            var resultMessage = Read(stream);

            List<int> resultCodeList = new List<int>();

            var resultMessageSplit = resultMessage.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach(var split in resultMessageSplit)
            {
                resultCodeList.Add(int.Parse(split.Substring(0, 3)));
            }

            if(resultCodeList.Count == 1)
            {
                if(resultCodeList[0] >= 100 && resultCodeList[0] < 200)
                {
                    resultMessage = Read(stream);
                    resultCodeList.Add(int.Parse(resultMessage.Substring(0,3)));
                }
            }
            
            return new ResultCommand(result, resultCodeList.ToArray());
        }

        private static TcpClient PassiveCommand(NetworkStream stream)
        {
            Send(stream, Command.PASV);

            var result = Read(stream);

            Regex regex = new Regex(@"\d*,\d*,\d*,\d*,\d*,\d*");

            var split = regex.Match(result).Value.Split(',');

            IPAddress address = new IPAddress(new byte[]
            {
                    Convert.ToByte(split[0]),
                    Convert.ToByte(split[1]),
                    Convert.ToByte(split[2]),
                    Convert.ToByte(split[3])
            });

            int newPort = Convert.ToInt32(split[4]) * 256 + Convert.ToInt32(split[5]);

            TcpClient newClient = new TcpClient(address.ToString(), newPort);

            return newClient;
        }

        private static bool Send(NetworkStream stream, Command command, params string[] param)
        {
            byte[] message;

            message = (byte[])typeof(FTP).GetMethod(command.ToString(), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance).Invoke(null, param);

            var resultWrite = stream.WriteAsync(message, 0, message.Length);
            resultWrite.Wait(2000);

            return resultWrite.IsCompleted;

        }

        private static string Read(NetworkStream stream)
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
                
                if(result.Result != buf.Length)
                {
                    break;
                }
            }

            return resultText;
        }

        private static byte[] USER(string username)
        {
            return Encoding.UTF8.GetBytes($"USER {username}{CRLF}");
        }

        private static byte[] PASS(string password)
        {
            return Encoding.UTF8.GetBytes($"PASS {password}{CRLF}");
        }

        private static byte[] LIST()
        {
            return Encoding.UTF8.GetBytes($"LIST{CRLF}");
        }

        private static byte[] PASV()
        {
            return Encoding.UTF8.GetBytes($"PASV{CRLF}");
        }

        private static byte[] CWD(string path)
        {
            return Encoding.UTF8.GetBytes($"CWD {path}{CRLF}");
        }

        enum Command
        {
            USER, PASS, LIST, PASV, CWD
        }
    }
}
