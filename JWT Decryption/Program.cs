using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_Decryption
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayMenu(); // Display options to the User
        }

        public static void DisplayMenu()
        {
            Console.WriteLine("Hello!".PadLeft(20));
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Welcome to the JSON Web Token Tool".PadLeft(15));
            Console.WriteLine("-----------------------------------------------\n");
            Console.WriteLine("Please choose one of the following options\n".PadLeft(5));
            Console.WriteLine("1 - Decryption\n");
            Console.WriteLine("2 - Encryption\n");
            Console.WriteLine("3 - Exit");
            Console.WriteLine("-----------------------------------------------\n");
            int options = int.Parse(Console.ReadLine());
            Console.WriteLine("-----------------------------------------------\n");

            switch (options)
            {
                case 1:
                    DecryptJWT2();
                    break;
                case 2:
                    EncryptJWT2();
                    break;
                case 3:
                    Exit();
                    break;
            }
        }

        public static void Exit()
        {
            Console.Clear();
            Console.WriteLine("\n\n");
            Console.WriteLine("GOODBYE".PadLeft(25));
            Console.ReadLine();
        }

        public static void EncryptJWT1()
        {

        }

        public static void DecryptJWT1()
        {

        }


        // Encrypt JWT Token
        public static void EncryptJWT2()
        {
            Console.WriteLine("ENCRYPTING TOKEN");

            const string sec = "ProEMLh5e_qnzdNUQrqdHPgp";
            const string sec1 = "ProEMLh5e_qnzdNU";
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));
            var securityKey1 = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec1));

            var signingCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>()
{
    new Claim("sub", "test"),
};
            var ep = new EncryptingCredentials(
                            securityKey1,
                            SecurityAlgorithms.Aes128KW,
                            SecurityAlgorithms.Aes128CbcHmacSha256);

            var handler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = handler.CreateJwtSecurityToken(
                "issuer",
                "Audience",
                new ClaimsIdentity(claims),
                DateTime.Now,
                DateTime.Now.AddHours(1),
                DateTime.Now,
                signingCredentials,
                ep);

            string tokenString = handler.WriteToken(jwtSecurityToken);

            // If someone tries to view the JWT without validating/decrypting the token,
            // then no claims are retrieved and the token is safe guarded.
            var jwt = new JwtSecurityToken(tokenString);

            Console.Write("The claims that were encoded were: ");
            claims.ForEach(Console.WriteLine);
            Console.WriteLine("\nDecoded JWT is: {0} \n", jwt);
            Console.WriteLine("Encoded token string is: {0}", tokenString);
            Console.ReadLine();

        }

        // Validate/Decrypt JWT Token
        public static void DecryptJWT2()
        {
            Console.WriteLine("DECRYPTING TOKEN");

            const string sec = "ProEMLh5e_qnzdNUQrqdHPgp";
            const string sec1 = "ProEMLh5e_qnzdNU";
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));
            var securityKey1 = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec1));

            // This is the input JWT which we want to validate.
            string tokenString = string.Empty;

            // If we retrieve the token without decrypting the claims, we won't get any claims
            // DO not use this jwt variable

            //var jwt = new JwtSecurityToken(tokenString);
            JwtSecurityToken jwt = new JwtSecurityToken(tokenString);


            // Verification
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudiences = new string[]
                {
        "536481524875-glk7nibpj1q9c4184d4n3gittrt8q3mn.apps.googleusercontent.com"
                },
                ValidIssuers = new string[]
                {
        "https://accounts.google.com"
                },
                IssuerSigningKey = securityKey,
                // This is the decryption key
                TokenDecryptionKey = securityKey1
            };

            SecurityToken validatedToken;
            var handler = new JwtSecurityTokenHandler();

            handler.ValidateToken(tokenString, tokenValidationParameters, out validatedToken);
        }
    }
}
