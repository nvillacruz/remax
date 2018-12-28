using System;

namespace Remax.Web.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class EventDetailsModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string EventTitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EventDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int MaxPersons { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset StartDate { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset EndDate { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public decimal NumberOfHours { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal RegistrationFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal Price { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ConfirmButtonLink { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string ToEmail { get; set; }

    }
}
