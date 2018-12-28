using Dna;
using System;
using System.Threading.Tasks;

namespace Remax.Web.Server
{
    /// <summary>
    /// Handles sending emails specific to the Yhotel
    /// </summary>
    public static class YHotelEmailSender
    {
        /// <summary>
        /// Sends a verification email to the specified user
        /// </summary>
        /// <param name="displayName">The users display name (typically first name)</param>
        /// <param name="email">The users email to be verified</param>
        /// <param name="verificationUrl">The URL the user needs to click to verify their email</param>
        /// <returns></returns>
        public static async Task<SendEmailResponse> SendUserVerificationEmailAsync(string displayName, string email, string verificationUrl)
        {
            return await DI.EmailTemplateSender.SendGeneralEmailAsync(new SendEmailDetails
            {
                IsHTML = true,
                FromEmail = Framework.Construction.Configuration["EmailSettings:SendEmailFromEmail"],
                FromName = Framework.Construction.Configuration["EmailSettings:SendEmailFromName"],
                ToEmail = email,
                ToName = displayName,
                Subject = "Y Bizz Account - Email Verification"
            },
            "Account Verification",
            $"Hi {displayName ?? "stranger"},",
            "Thank you for creating an account with us.<br/> " +
            "You are only one step away before you can avail all our services. <br/>" +
            "Click the button below to confirm your email",
            "Verify Email",
            verificationUrl
            );
        }

        /// <summary>
        /// Sends a forgot password link to the specified user
        /// </summary>
        /// <param name="displayName">The users display name (typically first name)</param>
        /// <param name="email">The users email to be verified</param>
        /// <param name="verificationUrl">The URL the user needs to click to go for the page of Resetting Password</param>
        /// <returns></returns>
        public static async Task<SendEmailResponse> SendUserForgotPasswordLinkAsync(string displayName, string email, string verificationUrl)
        {
            var a = await DI.EmailTemplateSender.SendGeneralEmailAsync(new SendEmailDetails
            {
                IsHTML = true,
                FromEmail = Framework.Construction.Configuration["EmailSettings:SendEmailFromEmail"],
                FromName = Framework.Construction.Configuration["EmailSettings:SendEmailFromName"],
                ToEmail = email,
                ToName = displayName,
                Subject = "Forgot Password Link"
            },
                "Reset Password",
                $"Hi {displayName ?? "stranger"},",
                "It looks like you are trying to reset your password",
                "Go to Reset Password Page",
                verificationUrl
            );

            return a;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="email"></param>
        /// <param name="verificationUrl"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public static async Task<SendEmailResponse> SendBookingDetailsForConfirmationAsync(string displayName, string email, string verificationUrl, BookingConfirmationDetailsModel details)
        {
            var a = await DI.EmailTemplateSender.SendGeneralEmailAsync(new SendEmailDetails
            {
                IsHTML = true,
                FromEmail = Framework.Construction.Configuration["EmailSettings:SendEmailFromEmail"],
                FromName = Framework.Construction.Configuration["EmailSettings:SendEmailFromName"],
                ToEmail = email,
                ToName = displayName,
                Subject = "Y-Bizz Booking Confirmation"
            },
                "Booking Confirmation",
                $"Hi {displayName ?? "stranger"},",
                $"You have been booked a {details.Name} on {details.BookingDate}. Please confirm the book",
                "Confirm",
                verificationUrl
            );

            return a;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        public static async Task<SendEmailResponse> SendCoworkingMembershipForConfirmationAsync(CoworkingMembershipDetailsModel details)
        {
            var a = await DI.EmailTemplateSender.SendCoworkingMembershipConfirmationAsync(new SendEmailDetails
            {
                IsHTML = true,
                FromEmail = Framework.Construction.Configuration["EmailSettings:SendEmailFromEmail"],
                FromName = Framework.Construction.Configuration["EmailSettings:SendEmailFromName"],
                ToEmail = details.ToEmail,
                ToName = details.DisplayName,
                Subject = "Co-working Space Membership Application"
            },
                $"Co-working Space (Membership) Details",
                $"Hi {details.DisplayName ?? "stranger"},",

                $"Thank you for applying a membership with us. Below is the summary " +
                $"of your applied membership and your payment details:<br><br>" +
                $"<b> Membership Details:</b><br>" +
                $"Package type: {details.Name} <br>" +
                $"Number of people in the membership: {details.MaxPersons} person(s)" +
                $"<br><br>" +
                $"<b>Payment Details:</b><br>" +
                $"Amount: {details.Price}<br>" +
                $"Payment Frequency: Monthly<br><br>",

                "If you have further questions or inquiries, you can contact us through" +
                "our contact numbers or through our email.<br>" +
                "Hoping to see you soon! <br><br>",

                "Confirm Application",

                details.ConfirmButtonLink
            );

            return a;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        public static async Task<SendEmailResponse> SendCoworkingWalkinsForConfirmationAsync(CoworkingWalkinDetailsModel details)
        {
            var a = await DI.EmailTemplateSender.SendCoworkingMembershipConfirmationAsync(new SendEmailDetails
            {
                IsHTML = true,
                FromEmail = Framework.Construction.Configuration["EmailSettings:SendEmailFromEmail"],
                FromName = Framework.Construction.Configuration["EmailSettings:SendEmailFromName"],
                ToEmail = details.ToEmail,
                ToName = details.DisplayName,
                Subject = "Co-working Space Walkin Booking"
            },
                $"Co-working Space (WalkiIn) Details",
                $"Hi {details.DisplayName ?? "stranger"},",

                $"Thank you for booking a co-working space with us. Below is the summary " +
                $"of your booked co-working space and your payment details:<br><br>" +
                $"<b>Co-working Space Details:</b><br>" +
                $"Package type: {details.Name} <br>" +
                $"Number of hours: {details.MaxHours} hour(s) <br>" +
                $"Date and Time of booked co-working : { TimeZoneInfo.ConvertTimeFromUtc( details.StartDate.UtcDateTime, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"))} to " +
                $"{TimeZoneInfo.ConvertTimeFromUtc(details.EndDate.UtcDateTime, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"))}" +
                $"<br><br>" +
                $"<b>Payment Details:</b><br>" +
                $"Amount: Php {details.Price.ToString("0.##")}<br><br>" ,

                "If you have further questions or inquiries, you can contact us through" +
                "our contact numbers or through our email.<br>" +
                "Hoping to see you soon! <br><br>",

                "Confirm booking",

                details.ConfirmButtonLink
            );

            return a;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        public static async Task<SendEmailResponse> SendMeetingRoomForConfirmationAsync(MeetingRoomDetailsModel details)
        {
            var a = await DI.EmailTemplateSender.SendCoworkingMembershipConfirmationAsync(new SendEmailDetails
            {
                IsHTML = true,
                FromEmail = Framework.Construction.Configuration["EmailSettings:SendEmailFromEmail"],
                FromName = Framework.Construction.Configuration["EmailSettings:SendEmailFromName"],
                ToEmail = details.ToEmail,
                ToName = details.DisplayName,
                Subject = "Meeting Room Booking"
            },
                $"Meeting Room Details",
                $"Hi {details.DisplayName ?? "stranger"},",

                $"Thank you for booking a meeting room  with us. Below is the summary " +
                $"of your booked meeting room and your payment details:<br><br>" +
                $"<b>Meeting Room Details:</b><br>" +
                $"Number of people in the meeting: {details.NumberOfPeople} person(s) <br>" +
                $"Number of hours: {details.NumberOfHours} hour(s) <br>" +
                $"Date and Time of booked meeting room: { TimeZoneInfo.ConvertTimeFromUtc(details.StartDate.UtcDateTime, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"))} to " +
                $"{TimeZoneInfo.ConvertTimeFromUtc(details.EndDate.UtcDateTime, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"))}" +
                $"<br><br>" +
                $"<b>Payment Details:</b><br>" +
                $"Amount: Php {details.Price.ToString("0.##")}<br><br>",

                "If you have further questions or inquiries, you can contact us through" +
                "our contact numbers or through our email.<br>" +
                "Hoping to see you soon! <br><br>",

                "Confirm booking",

                details.ConfirmButtonLink
            );

            return a;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        public static async Task<SendEmailResponse> SendEventDetailsForConfirmationAsync(EventDetailsModel details)
        {
            var a = await DI.EmailTemplateSender.SendCoworkingMembershipConfirmationAsync(new SendEmailDetails
            {
                IsHTML = true,
                FromEmail = Framework.Construction.Configuration["EmailSettings:SendEmailFromEmail"],
                FromName = Framework.Construction.Configuration["EmailSettings:SendEmailFromName"],
                ToEmail = details.ToEmail,
                ToName = details.DisplayName,
                Subject = "Event Space Booking"
            },
                $"Event Space Details",
                $"Hi {details.DisplayName ?? "stranger"},",

                $"Thank you for booking an event space with us. Below is the summary " +
                $"of your booked event and your payment details:<br><br>" +
                $"<b>Event Details:</b><br>" +
                $"Event Title: {details.EventTitle}<br>" +
                $"Event Type: {details.EventType}  <br>" +
                $"Date and Time of Event: { TimeZoneInfo.ConvertTimeFromUtc(details.StartDate.UtcDateTime, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"))} to " +
                $"{TimeZoneInfo.ConvertTimeFromUtc(details.EndDate.UtcDateTime, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"))}" +
                $"<br>" +
                $"Maximum No. of attendees: {details.MaxPersons} person(s) <br>" +
                $"Registration Fee: Php {details.RegistrationFee.ToString("0.##")} <br><br>" +
                $"<b>Payment Details:</b><br>" +
                $"Amount: Php {details.Price.ToString("0.##")}<br><br>",

                "If you have further questions or inquiries, you can contact us through" +
                "our contact numbers or through our email.<br>" +
                "Hoping to see you soon! <br><br>",

                "Confirm Booking",

                details.ConfirmButtonLink
            );

            return a;
        }


    }
}
