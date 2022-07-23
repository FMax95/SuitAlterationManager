namespace SuitAlterationManager.Infrastructure.Email.Messages
{
	public class RecoveryPassword : EmailMessage
	{
    public RecoveryPassword (IEmailService emailService) : base(emailService){
      this.Template = EmailTemplates.PasswordRecovery;
      this.Subject = "SuitAlterationManager - Reset Password";
    }
	}
}
