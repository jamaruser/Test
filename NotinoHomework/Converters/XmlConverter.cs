namespace NotinoHomework.Converters;

using System.Xml.Serialization;
using Abstraction;
using Helpers;
using Models;

public class XmlConverter : IFormatConverter
{
    public Document ConvertFrom(string input)
    {
        var objectToDeserialize = new Document();
        var xmlSerializer = new XmlSerializer(objectToDeserialize.GetType());
        using var streamReader = new StringReader(input);
        try
        {
            return (Document) xmlSerializer.Deserialize(streamReader);
        }
        catch (InvalidOperationException exception)
        {
            throw new ArgumentException(ValidationMessages.InputFileNotInCorrectFormat, exception);
        }
    }

    public string ConvertTo(Document document)
    {
        var xmlSerializer = new XmlSerializer(document.GetType());
        using var textWriter = new StringWriter();
        xmlSerializer.Serialize(textWriter, document);
        return textWriter.ToString();
    }
}