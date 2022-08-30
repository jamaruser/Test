namespace NotinoHomework.Helpers;

public static class FileHelper
{
    public static string GetExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        return extension.Replace(".", "");
    }

    public static string ChangeExtension(string fileName, string newExtension)
    {
        return Path.ChangeExtension(fileName, $".{newExtension}");
    }

    public static Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}