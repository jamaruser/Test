namespace NotinoHomework.Abstraction;

using Models;

public interface IConverterService
{
    Task<ConvertResult> Convert(FileDto input, string target);
}