using NotinoHomework.Abstraction;
using NotinoHomework.Configuration;
using NotinoHomework.HttpClients;
using NotinoHomework.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IConverterService, ConverterService>();
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddTransient<IBlobStorageService, BlobStorageService>();
builder.Services.AddHttpClient<IFileClient, FileClient>();
builder.Services.AddTransient<IFileProcessor, FileProcessor>();
builder.Services.AddTransient<ISmtpClient, SmtpClientWrapper>();
builder.Services.AddTransient<IBlobContainerClient, BlobContainerWrapper>();
builder.Services.Configure<BlobStorageConfiguration>(
    builder.Configuration.GetSection(nameof(BlobStorageConfiguration)));
builder.Services.Configure<FileClientConfiguration>(
    builder.Configuration.GetSection(nameof(FileClientConfiguration)));
builder.Services.Configure<SmtpConfiguration>(
    builder.Configuration.GetSection(nameof(SmtpConfiguration)));
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection(nameof(EmailConfiguration)));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.Run();