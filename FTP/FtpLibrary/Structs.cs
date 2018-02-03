using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FtpLibrary
{
    public struct FtpObject
    {
        public string Type { get; private set; }
        public string Name { get; private set; }
        public FtpObject[] Structure { get; private set; }

        public FtpObject(string type, string name, FtpObject[] structure)
        {
            Type = type;
            Name = name;
            Structure = structure;
        }

        public FtpObject(string parse)
        {
            Regex regex = new Regex(@"^(?<dir>[\-ld])(?<permission>[\-rwx]{9})\s+(?<filecode>\d+)\s+(?<owner>\w+)\s+(?<group>\w+)\s+(?<size>\d+)\s+(?<month>\w{3})\s+(?<day>\d{1,2})\s+(?<timeyear>[\d:]{4,5})\s+(?<filename>(.*))$");
            var resultReg = regex.Match(parse);
            Type = resultReg.Groups["dir"].Value == "-" ? "F" : resultReg.Groups["dir"].Value.ToUpper();
            Name = resultReg.Groups["filename"].Value;
            Structure = null;
        }
    }

    public struct ResultWithFtpObject
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public FtpObject[] FtpObjectArray { get; private set; }

        public ResultWithFtpObject(bool success, string message, FtpObject[] ftpObjectArray)
        {
            Success = success;
            Message = message;
            FtpObjectArray = ftpObjectArray;
        }
    }

    public struct ResultCommand
    {
        public string ResultText { get; private set; }
        public int[] ResultCode { get; private set; }

        public ResultCommand(string resultText, int[] resultCode)
        {
            ResultText = resultText;
            ResultCode = resultCode;
        }
    }
}
