using System;

namespace Remax.Web.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class MeetingRoomDetailsModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int NumberOfPeople { get; set; }

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
