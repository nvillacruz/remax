using Dna;
using System.IO;

namespace Remax.Web.Server

{
    /// <summary>
    /// 
    /// </summary>
    public static class ConfigurationHelpers
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetEventsDefaultPath() {
            var path = Framework.Construction.Configuration["Server:UserImages:EventsPath"];
            return Directory.Exists(path)? path: Directory.GetCurrentDirectory() + Framework.Construction.Configuration["Server:UserImages:EventsPathIfNotExists"];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetCoworkingDefaultPath() {
            var path = Framework.Construction.Configuration["Server:UserImages:CoworkingPath"];
            return Directory.Exists(path) ? path : Directory.GetCurrentDirectory() + Framework.Construction.Configuration["Server:UserImages:CoworkingPathIfNotExists"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetUserImageDefaultPath()
        {
            var path = Framework.Construction.Configuration["Server:UserImages:UserImagePath"];
            return Directory.Exists(path) ? path : Directory.GetCurrentDirectory() + Framework.Construction.Configuration["Server:UserImages:USerImagePathIfNotExists"];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GenerateDefaultEventApiPath() => GenerateDefaultEventApiPath("default_events.jpg");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GenerateDefaultEventApiPath(string filename) => !string.IsNullOrEmpty(filename) ? Framework.Construction.Configuration["Server:Host"] + $"/api/download/events?filename={filename}": GenerateDefaultEventApiPath();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GenerateDefaultCoworkingApiPath() => GenerateDefaultCoworkingApiPath("default_coworking.jpg");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GenerateDefaultCoworkingApiPath(string filename) => !string.IsNullOrEmpty(filename)? Framework.Construction.Configuration["Server:Host"] + $"/api/download/coworking?filename={filename}":  GenerateDefaultCoworkingApiPath();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GenerateDefaultUserImagePath() => GenerateDefaultUserImagePath("default-user.png");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GenerateDefaultUserImagePath(string filename) => !string.IsNullOrEmpty(filename) ? Framework.Construction.Configuration["Server:Host"] + $"/api/download/events?filename={filename}" : GenerateDefaultUserImagePath();

    }
}
