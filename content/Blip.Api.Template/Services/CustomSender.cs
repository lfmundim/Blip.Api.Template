using Lime.Messaging.Resources;
using Lime.Protocol;
using System.Threading;
using System.Threading.Tasks;
using Take.Blip.Client;

namespace Blip.Api.Template.Services
{
    public class CustomSender : ISender
    {
        private readonly IBlipClient _client;
        private bool _isStarted;
        public CustomSender(MySettings settings)
        {
            var builder = new BlipClientBuilder()
               .UsingAccessKey(settings.BlipBotSettings.Identifier, settings.BlipBotSettings.AccessKey)
               .UsingRoutingRule(RoutingRule.Instance); // Deve ser Instance, caso contrário poderá receber mensagens de clientes!!

            _client = builder.Build();
            _isStarted = false;
        }

        public async Task<Command> ProcessCommandAsync(Command requestCommand, CancellationToken cancellationToken)
        {
            await CheckStarted(cancellationToken);
            return await _client.ProcessCommandAsync(requestCommand, cancellationToken);
        }

        public async Task SendCommandAsync(Command command, CancellationToken cancellationToken)
        {
            //await CheckStarted();
            await _client.SendCommandAsync(command, cancellationToken);
        }

        public async Task SendMessageAsync(Message message, CancellationToken cancellationToken)
        {
            //await CheckStarted();
            await _client.SendMessageAsync(message, cancellationToken);
        }

        public async Task SendNotificationAsync(Notification notification, CancellationToken cancellationToken)
        {
            //await CheckStarted();
            await _client.SendNotificationAsync(notification, cancellationToken);
        }

        private async Task CheckStarted(CancellationToken cancellationToken)
        {
            if (!_isStarted)
            {
                await _client.StartAsync(cancellationToken);
                _isStarted = true;
            }
        }
    }
}
