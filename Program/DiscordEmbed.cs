using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class DiscordEmbed
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? Color { get; set; }
    public DateTime? Timestamp { get; set; }
    public DiscordEmbedFooter? Footer { get; set; }
    public DiscordEmbedAuthor? Author { get; set; }
    public List<DiscordEmbedField>? Fields { get; set; } = new List<DiscordEmbedField>();
    public DiscordEmbedImage? Image { get; set; }
    public DiscordEmbedThumbnail? Thumbnail { get; set; }
}

public class DiscordEmbedFooter
{
    public string Text { get; set; }
    public string IconUrl { get; set; }
}

public class DiscordEmbedAuthor
{
    public string? Name { get; set; }
    public string? Url { get; set; }
    public string? IconUrl { get; set; }
}

public class DiscordEmbedField
{
    public string Name { get; set; }
    public string Value { get; set; }
    public bool Inline { get; set; }
}

public class DiscordEmbedImage
{
    public string Url { get; set; }
}

public class DiscordEmbedThumbnail
{
    public string Url { get; set; }
}

public class DiscordWebhookSender
{
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly string _webhookUrl = ConfigurationManager.AppSettings["WebhookURL"];

    public async Task SendEmbedAsync(DiscordEmbed embed)
    {
        var payload = new
        {
            embeds = new[] { new
            {
                footer = embed.Footer != null ? new
                {
                    text = embed.Footer.Text,
                    icon_url = embed.Footer.IconUrl
                } : null,
                author = embed.Author != null ? new
                {
                    name = embed.Author.Name,
                    url = embed.Author.Url,
                    icon_url = embed.Author.IconUrl
                } : null,
                fields = embed.Fields?.Select(f => new
                {
                    name = f.Name,
                    value = f.Value,
                    inline = f.Inline
                }),
                image = embed.Image != null ? new
                {
                    url = embed.Image.Url
                } : null,
                thumbnail = embed.Thumbnail != null ? new
                {
                    url = embed.Thumbnail.Url
                } : null
            } }
        };

        try
        {
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            MessageBox.Show(json.ToString());

            var response = await _httpClient.PostAsync(_webhookUrl, content);
            
            response.EnsureSuccessStatusCode();
            
        } catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

    }
}