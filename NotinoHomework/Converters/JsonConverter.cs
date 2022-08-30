namespace NotinoHomework.Converters;

using Abstraction;
using Helpers;
using Models;
using Newtonsoft.Json;

public class JsonConverter : IFormatConverter
{
    public Document ConvertFrom(string input)
    {
        try
        {
            return JsonConvert.DeserializeObject<Document>(input);
        }
        catch (JsonReaderException e)
        {
            throw new ArgumentException(ValidationMessages.InputFileNotInCorrectFormat, e);
        }
    }

    public string ConvertTo(Document document)
    {
        return JsonConvert.SerializeObject(document);
    }
}