namespace NotinoHomework.Models;

public class ConvertResult
{
    private readonly Stream _convertedFile;


    public ConvertResult(Stream convertedFile, string fileName)
    {
        _convertedFile = convertedFile;
        FileName = fileName;
    }

    public Stream ConvertedFile
    {
        get
        {
            _convertedFile.Position = 0;
            return _convertedFile;
        }
    }

    public string FileName { get; }
}