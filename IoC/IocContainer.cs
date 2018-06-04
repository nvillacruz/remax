﻿using Dna;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Y.Bizz.Web.Server
{
    /// <summary>
    /// A shorthand access class to get DI services with nice clean short code
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// The scoped instance of the <see cref="ApplicationDbContext"/>
        /// </summary>
        public static ApplicationDbContext ApplicationDbContext => Framework.Provider.GetService<ApplicationDbContext>();

        /// <summary>
        /// The transient instance of the <see cref="IEmailSender"/>
        /// </summary>
        public static IEmailSender EmailSender => Framework.Provider.GetService<IEmailSender>();

        /// <summary>
        /// The transient instance of the <see cref="IEmailTemplateSender"/>
        /// </summary>
        public static IEmailTemplateSender EmailTemplateSender => Framework.Provider.GetService<IEmailTemplateSender>();
    }

    ///// <summary>
    ///// The dependency injection container making use of the built in .Net Core service provider
    ///// </summary>
    //public static class IoCContainer
    //{
    //    /// <summary>
    //    /// The service provider for this application
    //    /// </summary>
    //    public static ServiceProvider Provider { get; set; }

    //    /// <summary>
    //    /// The configuration manager for the application
    //    /// </summary>
    //    public static IConfiguration Configuration { get; set; }
    //}
}
