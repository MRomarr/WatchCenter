using FluentEmail.Core;

namespace WatchCenter.Infrasturcture.Services.EmailService
{
    internal class EmailService(IFluentEmail fluentEmail) : IEmailService
    {
        public async Task<bool> SendEmailAsync(string email, string subject, string body)
        {
           var result = await fluentEmail
                .To(email)
                .Subject(subject)
                .Body(body, isHtml: true)
                .SendAsync();
            return result.Successful;

        }

    }
}
