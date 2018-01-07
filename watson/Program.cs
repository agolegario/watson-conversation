using System;
using System.IO;
using System.Linq;
using IBM.WatsonDeveloperCloud.Conversation.v1;
using IBM.WatsonDeveloperCloud.Conversation.v1.Model;
using Microsoft.Extensions.Configuration;

namespace watson
{
    internal class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        private static void Main(string[] args = null)
        {
            Configuration = GetConfiguration();

            var userName = Configuration.GetSection("AppConfiguration")["UserName"];
            var password = Configuration.GetSection("AppConfiguration")["Password"];
            var workspaceId = Configuration.GetSection("AppConfiguration")["WorkspaceId"];
            var dateVersion = Configuration.GetSection("AppConfiguration")["DateVersion"];

            Console.WriteLine("DIGITE exit PARA SAIR");

            var conversation = new ConversationService(userName, password, dateVersion);

            var messageRequest = new MessageRequest {Input = new InputData {Text = ""}};
            var response = conversation.Message(workspaceId, messageRequest);
            Console.WriteLine("Watson >> {0}", response.Output.Text.FirstOrDefault());

            while (true)
            {
                Console.Write("Usuario >> ");
                messageRequest.Input = new InputData { Text = Console.ReadLine() };
                if(messageRequest.Input.Text == "exit") goto Finish;
                messageRequest.Context = response.Context;
                response = conversation.Message(workspaceId, messageRequest);
                Console.WriteLine("Watson >> {0}", response.Output.Text.FirstOrDefault());
            }
            Finish: Console.WriteLine("SAINDO DA CONVERSA.........");
            Console.ReadKey();
        }

        public static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            return builder.Build();
        }
    }
}