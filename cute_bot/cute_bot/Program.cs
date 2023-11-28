using Microsoft.VisualBasic;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var botClient = new TelegramBotClient("6761999039:AAFqrY7ICkL1jtbZ8DkqEIWlt28uADUQKdw");

using CancellationTokenSource cts = new();

ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() 
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    if (messageText == "/check")
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "the bot is working stably",
            cancellationToken: cancellationToken);
    }

    if (messageText == "/start")
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "hi!",
            cancellationToken: cancellationToken);
    }

    if (messageText == "/mem")
    {
        await botClient.SendPhotoAsync(
            chatId: chatId,
            photo: InputFile.FromUri("https://images.upbeatnews.com/posts/6177/YYOTyznPDLf6aqlvrtfT1kloj8yu0G5qZNlwNfMl.jpeg"),
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken);
    }
    if (messageText == "/mem1")
    {
        await botClient.SendVideoAsync(
        chatId: chatId,
        video: InputFile.FromUri("https://raw.githubusercontent.com/TelegramBots/book/master/src/docs/video-countdown.mp4"),
        thumbnail: InputFile.FromUri("https://raw.githubusercontent.com/TelegramBots/book/master/src/2/docs/thumb-clock.jpg"),
        supportsStreaming: true,
        cancellationToken: cancellationToken);
    }

    if (messageText == "/mem2")
    {
        Message message1 = await botClient.SendStickerAsync(
    chatId: chatId,
    sticker: InputFile.FromUri("https://chpic.su/_data/stickers/a/animeepp/animeepp_041.webp"),
    cancellationToken: cancellationToken);

    }
    if (messageText == "/mem3")
    {
        await botClient.SendPollAsync(
        chatId: chatId,
        question: "choose your fighter",
        options: new[]
        {
        "с++",
        "c#",
        "ph",
        "В††"
        },
        cancellationToken: cancellationToken);
    }


}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}