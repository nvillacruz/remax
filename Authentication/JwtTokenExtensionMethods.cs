using Dna;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Remax.Web.Server
{
    /// <summary>
    /// Extension methods for working with Jwt bearer tokens
    /// </summary>
    public static class JwtTokenExtensionMethods
    {
        /// <summary>
        /// Generates a Jwt bearer token containing the users username
        /// </summary>
        /// <param name="user">The users details</param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static string GenerateJwtToken(this ApplicationUser user, List<string> roles)
        {

            // Set our tokens claims
            var claims = new List<Claim>
            {
                // Unique ID for this token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),

                // The username using the Identity name so it fills out the HttpContext.User.Identity.Name value
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),

                // Add user Id so that UserManager.GetUserAsync can find the user based on Id
                new Claim(ClaimTypes.NameIdentifier, user.Id),

                //Add Employee ID
                new Claim(ClaimTypes.PrimarySid, user.Employee.Id),


                // Add user Email , to retrieve, just use HttpContext.User.FindFirstValue(ClaimTypes.Email)
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Employee.FirstName),
                new Claim(ClaimTypes.Surname, user.Employee.LastName),

            };

            //Add Roles to the claim
            roles.ForEach(x => claims.Add(new Claim(ClaimTypes.Role, x)));
            
            // Create the credentials used to generate the token
            var credentials = new SigningCredentials(
                // Get the secret key from configuration
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Framework.Construction.Configuration["Jwt:SecretKey"])),
                // Use HS256 algorithm
                SecurityAlgorithms.HmacSha256);

            // Generate the Jwt Token
            var token = new JwtSecurityToken(
                issuer: Framework.Construction.Configuration["Jwt:Issuer"],
                audience: Framework.Construction.Configuration["Jwt:Audience"],
                claims: claims,
                signingCredentials: credentials,
                // Expire if not used for 5 minutes
                expires: DateTime.Now.AddMinutes(5)
                );

            // Return the generated token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
