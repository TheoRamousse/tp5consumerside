using Mailhog;
using MqLibrary.Models;
using MqLibrary.Services;
using RabbitMQ.Client.Events;
using System.Net.Mail;

void SendEmail(MqUserObject user)
{
    // Adresse SMTP et port utilisés par MailHog (par défaut)
    string mailhogSmtpHost = "mailhog"; // Remplacez localhost par l'adresse IP ou le nom du serveur si MailHog s'exécute sur un autre serveur
    int mailhogSmtpPort = 1025; // Port SMTP par défaut de MailHog

    // Configuration du client SMTP pour se connecter à MailHog
    using (SmtpClient client = new SmtpClient(mailhogSmtpHost, mailhogSmtpPort))
    {
        // Configurer le client SMTP pour MailHog
        client.UseDefaultCredentials = false; // MailHog n'a généralement pas besoin d'authentification
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.EnableSsl = false; // MailHog n'utilise pas SSL par défaut

        try
        {
            // Créer un message e-mail
            MailMessage message = new MailMessage();
            message.From = new MailAddress("sender@example.com");
            message.To.Add(new MailAddress(user.Email));
            message.Subject = "Confirmation de votre adresse mail";
            message.Body = $"Confirmer votre adresse mail ici : https://localhost:7278/User/validate?email={user.Email}&urlValidation={user.UrlApproveProfile}";

            // Envoyer l'e-mail
            client.Send(message);
            Console.WriteLine("E-mail envoyé avec succès à MailHog.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Une erreur s'est produite lors de l'envoi de l'e-mail : {ex.Message}");
        }
    }
}

IReceiverService service = new ReceiverService("rabbitmq", "users", SendEmail);


service.Start();