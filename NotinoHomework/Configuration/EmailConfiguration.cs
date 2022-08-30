namespace NotinoHomework.Configuration;

public class EmailConfiguration
{
    public string From { get; init; }
}

public class SmtpConfiguration
{
    public int Port { get; init; }
    public string Host { get; init; }

    public string Password { get; init; }
    public string UserName { get; set; }
}