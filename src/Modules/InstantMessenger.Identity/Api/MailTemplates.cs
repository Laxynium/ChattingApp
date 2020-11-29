namespace InstantMessenger.Identity.Api
{
    public static class MailTemplates
    {
        public static class AccountVerification
        {
            public static readonly string Subject = "Account verification";
            public static readonly string Body = @"Please use link below in order to verify your account.
<a href='{0}'>Verify account</a>";
        }

        public static class ResetPassword
        {
            public static readonly string Subject = "Password Reset";
            public static readonly string Body = @"Please use link below in order to reset your password.
<a href='{0}'>Reset password</a>";

        }
    }
}