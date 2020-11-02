namespace InstantMessenger.Identity.Api.Features.SignUp
{
    public static class MailTemplates
    {
        public static class AccountVerification
        {
            public static readonly string Subject = "Account verification";
            public static readonly string Body = @"Please use link below in order to verify your account.
<a href='{0}'>Verify account</a>";
        }
    }
}