//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace WhatsAppMessageSender
//{
//    class Program
//    {
//        // WhatsApp Business API credentials - you'll need to replace these with your actual values
//        private const string WHATSAPP_API_URL = "https://graph.facebook.com/v18.0/";
//        private const string PHONE_NUMBER_ID = "YOUR_PHONE_NUMBER_ID"; // Replace with your WhatsApp Business phone number ID
//        private const string ACCESS_TOKEN = "YOUR_ACCESS_TOKEN"; // Replace with your access token

//        static async Task Main(string[] args)
//        {
//            Console.WriteLine("WhatsApp Message Sender");
//            Console.WriteLine("======================\n");

//            try
//            {
//                // Get recipient's phone number
//                Console.Write("Enter recipient's phone number (with country code, no spaces or symbols, e.g., 14155552671): ");
//                string recipientNumber = Console.ReadLine();

//                // Get message text
//                Console.Write("Enter your message: ");
//                string messageText = Console.ReadLine();

//                // Send the message
//                await SendWhatsAppMessage(recipientNumber, messageText);

//                Console.WriteLine("\nMessage sent successfully!");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"\nError: {ex.Message}");
//                if (ex.InnerException != null)
//                {
//                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
//                }
//            }

//            Console.WriteLine("\nPress any key to exit...");
//            Console.ReadKey();
//        }

//        /// <summary>
//        /// Sends a WhatsApp message using the WhatsApp Business API
//        /// </summary>
//        /// <param name="recipientNumber">The recipient's phone number with country code (e.g., 14155552671)</param>
//        /// <param name="messageText">The message to send</param>
//        static async Task SendWhatsAppMessage(string recipientNumber, string messageText)
//        {
//            // Validate inputs
//            if (string.IsNullOrWhiteSpace(recipientNumber))
//                throw new ArgumentException("Recipient phone number cannot be empty");

//            if (string.IsNullOrWhiteSpace(messageText))
//                throw new ArgumentException("Message text cannot be empty");

//            // Create HTTP client
//            using (HttpClient client = new HttpClient())
//            {
//                // Set up the request
//                client.BaseAddress = new Uri(WHATSAPP_API_URL);
//                client.DefaultRequestHeaders.Accept.Clear();
//                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

//                // Create the request payload
//                var payload = new
//                {
//                    messaging_product = "whatsapp",
//                    recipient_type = "individual",
//                    to = recipientNumber,
//                    type = "text",
//                    text = new
//                    {
//                        preview_url = false,
//                        body = messageText
//                    }
//                };

//                // Convert payload to JSON
//                string jsonPayload = JsonSerializer.Serialize(payload);
//                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

//                // Send the request
//                Console.WriteLine("Sending message...");

//                // The endpoint URL structure: {WHATSAPP_API_URL}/{PHONE_NUMBER_ID}/messages
//                var response = await client.PostAsync($"{PHONE_NUMBER_ID}/messages", content);

//                // Check if request was successful
//                if (!response.IsSuccessStatusCode)
//                {
//                    string errorResponse = await response.Content.ReadAsStringAsync();
//                    throw new Exception($"API request failed with status code {response.StatusCode}. Response: {errorResponse}");
//                }

//                // Parse and display response (optional)
//                string responseBody = await response.Content.ReadAsStringAsync();
//                Console.WriteLine($"API Response: {responseBody}");
//            }
//        }
//    }

//    // Optional: Add this class if you want to parse the response
//    class WhatsAppApiResponse
//    {
//        public string messaging_product { get; set; }
//        public List<MessageContact> contacts { get; set; }
//        public List<MessageInfo> messages { get; set; }
//    }

//    class MessageContact
//    {
//        public string input { get; set; }
//        public string wa_id { get; set; }
//    }

//    class MessageInfo
//    {
//        public string id { get; set; }
//    }
//}

// Install the C# / .NET helper library from twilio.com/docs/csharp/install

using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Threading.Tasks;
using System.Collections.Generic;

class Program
{
    public static async Task Main(string[] args)
    {
        // Find your Account SID and Auth Token at twilio.com/console
        // and set the environment variables. See http://twil.io/secure
        string accountSid = Environment.GetEnvironmentVariable("ACbcfb9adbd2c415515ac4a518422e884e");
        string authToken = Environment.GetEnvironmentVariable("63857d3dbddb3e64014ce4488aba02b6");

        TwilioClient.Init(accountSid, authToken);

        var message = await MessageResource.CreateAsync(
            from: new Twilio.Types.PhoneNumber("whatsapp:+212642981375"),
            to: new Twilio.Types.PhoneNumber("whatsapp:+212767321977"),
            body: "Hello from Brahim Elkhattab!" );

        Console.WriteLine(message.Sid);
    }
}







//using System;
//using Twilio;
//using Twilio.Rest.Api.V2010.Account;
//using Twilio.Types;

//class Program
//{
//    static void Main(string[] args)
//    {
//        // Replace with your Twilio account details

//        const string accountSid = "ACbcfb9adbd2c415515ac4a518422e884e";
//        const string authToken = "63857d3dbddb3e64014ce4488aba02b6";

//        TwilioClient.Init(accountSid, authToken);

//        var message = MessageResource.Create(
//            from: new PhoneNumber("whatsapp:+212642981375"), // Twilio Sandbox WhatsApp number
//            to: new PhoneNumber("whatsapp:+212767321977"),     // Your friend's WhatsApp number
//            body: "Hello from Brahim Elkhattab!"
//        );

//        Console.WriteLine($"Message sent! SID: {message.Sid}");
//    }
//}

























