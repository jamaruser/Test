namespace NotinoHomework.Abstraction;

using Models;

/// <summary>
/// Add new implementation to Converter service
/// </summary>
public interface IFormatConverter
{
    Document ConvertFrom(string input);
    string ConvertTo(Document document);
}