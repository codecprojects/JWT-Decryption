using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace JWT_Decryption
{
    class Program
    {
        static void Main(string[] args)
        {
            //DisplayMenu(); // Display options to the User

            // Details from client summarized
            // 1. JWS - JSON consisting of header & payload
            //           {                         // header
            //                "alg":"HS256"
            //           }
            //          {                         // payload
            //                "iss":"DAFM",
            //                "exp":1557747358,
            //                "cid":22127
            //            }
            // 2. Signed TestKey - Fdh9u8rINxfivbrianbbVT1u232VQBZYKx1HGAGPt2I
            // 3. Produced Signed base64 value - Not Encrypted
            //    eyJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJEQUZNIiwiZXhwIjoxNTU3NzQ3MzU4LCJjaWQiOjIyMTI3fQ.pGhHG38KChajrqZ3eLdPkufmYRUR2OqiF0z_9XLlSVc
            // 4. Wrap the JWS as a payload in a JWE
            //    We then wrap this JWS as payload in a JWE:
            //            {
            //                "alg":"dir",
            //                "enc":"A256GCM",
            //                "cty":"jwt"
            //            }
            //{
            //    eyJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJEQUZNIiwiZXhwIjoxNTU3NzQ3MzU4LCJjaWQiOjIyMTI3fQ.pGhHG38KChajrqZ3eLdPkufmYRUR2OqiF0z_9XLlSVc
            //            }
            // 5. Encrypt TestKey - Bdh9u8rINxfivbrianbbVT1u232VQBZYKx1HGAGPt2I
            // 6. Produced base64 value - eyJhbGciOiJkaXIiLCJlbmMiOiJBMjU2R0NNIiwiY3R5Ijoiand0In0..46h3grwnT9YzIsWl.3Gk6ZqVyqrmVPG50B3lNBGfwXJOJJHrb8hmIyEMK5DfUSoikm9_G_87_WuEY0SPJfpq5Lr1rx7HJ3D1cHHIrlanH68F5MKSbPE_w_bEu6dG2QniwcsH8QaYTH0vNnuwkAxOA_bck_OR4D0FpMepvRzUMZLLkzHYWBWvV.J8AwlEqTM0JlbfQZVeFabw


           
        }

        public static void ClientDetails()
        {
            // As discussed, and example of our JWE generation follows.
            //We are initially preparing JWS:

            //{                         // header
            //    "alg":"HS256"
            //            }
            //{                         // payload
            //    "iss":"DAFM",
            //                "exp":1557747358,
            //                "cid":22127
            //            }

            // Which is signed with the following test key:
            // Fdh9u8rINxfivbrianbbVT1u232VQBZYKx1HGAGPt2I

            //To produce 'SIGNED' Base64 value:
            //eyJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJEQUZNIiwiZXhwIjoxNTU3NzQ3MzU4LCJjaWQiOjIyMTI3fQ.pGhHG38KChajrqZ3eLdPkufmYRUR2OqiF0z_9XLlSVc

            //We then wrap this JWS as payload in a JWE:
            //            {
            //    "alg":"dir",
            //                "enc":"A256GCM",
            //                "cty":"jwt"
            //            }
            //{
            //    eyJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJEQUZNIiwiZXhwIjoxNTU3NzQ3MzU4LCJjaWQiOjIyMTI3fQ.pGhHG38KChajrqZ3eLdPkufmYRUR2OqiF0z_9XLlSVc
            //            }

            // Which is encrypted with the following test key:
            // Bdh9u8rINxfivbrianbbVT1u232VQBZYKx1HGAGPt2I

            //  To produce encrypted Base64 value:
            //  eyJhbGciOiJkaXIiLCJlbmMiOiJBMjU2R0NNIiwiY3R5Ijoiand0In0..46h3grwnT9YzIsWl.3Gk6ZqVyqrmVPG50B3lNBGfwXJOJJHrb8hmIyEMK5DfUSoikm9_G_87_WuEY0SPJfpq5Lr1rx7HJ3D1cHHIrlanH68F5MKSbPE_w_bEu6dG2QniwcsH8QaYTH0vNnuwkAxOA_bck_OR4D0FpMepvRzUMZLLkzHYWBWvV.J8AwlEqTM0JlbfQZVeFabw

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
            Console.WriteLine("4 - Create and Read Token");
            Console.WriteLine("-----------------------------------------------\n");
            int options = int.Parse(Console.ReadLine());
            Console.WriteLine("-----------------------------------------------\n");

            switch (options)
            {
                case 1:
                    DecryptJWT();
                    break;
                case 2:
                    EncryptJWT();
                    break;
                case 4:
                    CreateReadToken();
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

        public static void CreateReadToken()
        {
            // Define const Key this should be private secret key stored in some safe place
            string privateKey = "Fdh9u8rINxfivbrianbbVT1u232VQBZYKx1HGAGPt2I";

            // Create Security keyusing private key above:
            // not that latest version of JWT using Microsoft namespace instead of System
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));

            // Also note that securityKey length should be >256b
            // so you have to make sure that your private key has a proper length
            SigningCredentials credentials = new SigningCredentials
                              (securityKey, SecurityAlgorithms.HmacSha256Signature);

            //  Finally create a Token
            JwtHeader header = new JwtHeader(credentials);

            //Some PayLoad that contain information about the  customer
            JwtPayload payload = new JwtPayload
           {
               { "exp ", "1557747358"},
               { "cid", "22127"},
               { "iss","DAFM"}
           };
            JwtSecurityToken secToken = new JwtSecurityToken(header, payload);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            // Token to String so you can use it in your client
            string tokenString = handler.WriteToken(secToken);

            Console.WriteLine(tokenString);
            Console.WriteLine("Consume Token");

            // And finally when you receive token from client
            // you can either validate it or try to read
            JwtSecurityToken token = handler.ReadJwtToken(tokenString);

            Console.WriteLine(token.Payload.First().Value);
            Console.ReadLine();
        }
        // Encrypt JWT Token
        public static void EncryptJWT()
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
        public static void DecryptJWT()
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
