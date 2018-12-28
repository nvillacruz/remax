namespace Remax.Web.Server
{
    /// <summary>
    /// 
    /// </summary>
    public static class ApiRoutes
    {
        /// <summary>
        /// 
        /// </summary>
        public const string Login = "api/login";
        /// <summary>
        /// 
        /// </summary>
        public const string LogOut = "api/logout";
        /// <summary>
        /// 
        /// </summary>
        public const string Register = "api/register";
        /// <summary>
        /// 
        /// </summary>
        public const string GetCurrentUser = "api/getcurrentuser";
        /// <summary>
        /// 
        /// </summary>
        public const string UpdateUser = "api/updateuser";
        /// <summary>
        /// 
        /// </summary>
        public const string UpdateAccountSettings = "api/updateaccountsettings";
        /// <summary>
        /// 
        /// </summary>
        public const string FaceBookLogin = "api/externalauth/facebook";
        /// <summary>
        /// 
        /// </summary>
        public const string VerifyEmail = "api/verify/email/{userId}/{*emailToken}";
        /// <summary>
        /// 
        /// </summary>
        public const string SendForgotPasswordLink = "api/forgot/password";
        /// <summary>
        /// 
        /// </summary>
        public const string ResetPassword = "api/reset/password/";
      

    }
}
