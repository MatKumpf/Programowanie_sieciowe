namespace POP3Library
{
    public struct ReceivedMail
    {
        public string Token { get; private set; }
        public string From { get; private set; }
        public string Subject { get; private set; }

        public ReceivedMail(string token, string from, string subject)
        {
            Token = token;
            From = from;
            Subject = subject;
        }

        public ReceivedMail Clone()
        {
            return new ReceivedMail(this.Token, this.From, this.Subject);
        }

        public override string ToString()
        {
            return $"Token: {this.Token} | From: {this.From} | Subject: {this.Subject}";
        }
    }

    public struct ResultDownloadMessages
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public ReceivedMail[] ReceivedMails { get; private set; }

        public ResultDownloadMessages(bool success, string message, ReceivedMail[] receivedMails)
        {
            Success = success;
            Message = message;
            ReceivedMails = receivedMails;
        }
    }
}