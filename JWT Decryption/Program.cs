using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
          //  DisplayMenu(); // Display options to the User
          
            //Assume the input is in a control called txtJwtIn,
            Console.WriteLine("Enter JWT");
            string jwtInput = Console.ReadLine();
            string jwtOutput = "";

            //and the output will be placed in a control called txtJwtOut
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            // string jwtInput = txtJwtIn.Text;

            //Check if readable token (string is in a JWT format)
            bool readableToken = jwtHandler.CanReadToken(jwtInput);

            if (readableToken != true)
            {
                Console.WriteLine("The token doesn't seem to be in a proper JWT format."); ;
            }
            if (readableToken == true)
            {
                JwtSecurityToken token = jwtHandler.ReadJwtToken(jwtInput);

                //Extract the headers of the JWT
                JwtHeader headers = token.Header;
                string jwtHeader = "{";

                foreach (KeyValuePair<string, object> h in headers)
                {
                    jwtHeader += '"' + h.Key + "\":\"" + h.Value + "\",";
                }
                jwtHeader += "}";
                Console.WriteLine("Header:\r\n" + JToken.Parse(jwtHeader).ToString(Formatting.Indented));

                //Extract the payload of the JWT
                IEnumerable<Claim> claims = token.Claims;
                string jwtPayload = "{";
                foreach (Claim c in claims)
                {
                    jwtPayload += '"' + c.Type + "\":\"" + c.Value + "\",";
                }
                jwtPayload += "}";
                jwtOutput += "\r\nPayload:\r\n" + JToken.Parse(jwtPayload).ToString(Formatting.Indented);
                Console.WriteLine(jwtOutput.ToString());
                Console.ReadLine();
            }

            #region Region: Details from client summarized
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
            #endregion

            #region Region: Client Requirements
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
            #endregion
        }
        public static void DisplayMenu()
        {
            try
            {
                Console.WriteLine("Hello!".PadLeft(20));
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Welcome to the JSON Web Token Tool".PadLeft(15));
            Console.WriteLine("-----------------------------------------------\n");
            Console.WriteLine("Please choose one of the following options\n".PadLeft(5));
            Console.WriteLine("1 - Decryption\n");
            Console.WriteLine("2 - Encryption\n");
            Console.WriteLine("3 - Create and Read Token\n");
            Console.WriteLine("4 - Exit\n");
            Console.WriteLine("-----------------------------------------------\n");
            int options = int.Parse(Console.ReadLine());
            Console.WriteLine("-----------------------------------------------\n");

           
                switch (options)
                {
                    default:
                        Console.WriteLine("YOU HAVE NOT SELECTED A NUMBER FROM THE MENU!\n" +
                            "\nThe number you have selected is: " + options + "\n \nSelect a number from 1-4");
                        Console.ReadLine();
                        break;
                    case 1:
                        DecryptJWT();
                        break;
                    case 2:
                        EncryptJWT();
                        break;
                    case 3:
                        CreateReadToken();
                        break;
                    case 4:
                        Exit();
                        break;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n"+ e.Message);
                Console.WriteLine("\nEnter a number from 1-4\n");
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
            // Define const Key this should be private secret key stored in some safe place, preferably config settings on server
            string privateKey = "Fdh9u8rINxfivbrianbbVT1u232VQBZYKx1HGAGPt2I";

            // Create Security key  using private key above:
            // not that latest version of JWT using Microsoft namespace instead of System
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));

            // Also note that securityKey length should be >256b
            // So you have to make sure that your private key has a proper length
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

            Console.WriteLine("Enter you token string to decrypt it :");
            // Token to String so you can use it in your client
            string tokenString = Console.ReadLine();// handler.WriteToken(secToken);

            Console.WriteLine("Token string (not ecnrypted) is : {0}\n", tokenString);
            Console.WriteLine("Consume Token");

            // And finally when you receive token from client
            // you can either validate it or try to read
            JwtSecurityToken token = handler.ReadJwtToken(tokenString);
            Console.WriteLine("JWT Security token (encrypted) is {0}", token);

            // Console.WriteLine(token.Payload.First().Value);
            Console.WriteLine("The values extracted from the payload are: ");
            foreach (var t in token.Payload)
            {
                Console.WriteLine(t);
            }
            Console.ReadLine();
        }
        public static void EncryptJWT()
        {
            Console.WriteLine("ENCRYPTING TOKEN");

            const string sec = "ProEMLh5e_qnzdNUQrqdHPgp";
            const string sec1 = "ProEMLh5e_qnzdNU";
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));
            SymmetricSecurityKey securityKey1 = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec1));

            SigningCredentials signingCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>()
{
    new Claim("sub", "test"),
};
            EncryptingCredentials ep = new EncryptingCredentials(
                            securityKey1,
                            SecurityAlgorithms.Aes128KW,
                            SecurityAlgorithms.Aes128CbcHmacSha256);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            JwtSecurityToken jwtSecurityToken = handler.CreateJwtSecurityToken(
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
            JwtSecurityToken jwt = new JwtSecurityToken(tokenString);

            Console.Write("The claims that were encoded were: ");
            claims.ForEach(Console.WriteLine);
            Console.WriteLine("\nDecoded JWT is: {0} \n", jwt);
            Console.WriteLine("Encoded token string is: {0}", tokenString);
            Console.ReadLine();

        }
        public static void DecryptJWT()
        {
            Console.WriteLine("DECRYPTING TOKEN");

            const string sec = "Fdh9u8rINxfivbrianbbVT1u232VQBZYKx1HGAGPt2I"; // issuer signing key
            const string sec1 = "Bdh9u8rINxfivbrianbbVT1u232VQBZYKx1HGAGPt2I";                         // token decryption key
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));
            SymmetricSecurityKey securityKey1 = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec1));

            // This is the input JWT which we want to validate.
            string tokenString = "eyJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJEQUZNIiwiZXhwIjoxNTU3NzQ3MzU4LCJjaWQiOjIyMTI3fQ.pGhHG38KChajrqZ3eLdPkufmYRUR2OqiF0z_9XLlSVc";

            // If we retrieve the token without decrypting the claims, we won't get any claims
            // DO not use this jwt variable

            //JwtSecurityToken jwt = new JwtSecurityToken(tokenString);

            // Verification
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
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
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            handler.ValidateToken(tokenString, tokenValidationParameters, out validatedToken);
            JwtSecurityToken jwt = new JwtSecurityToken(tokenString);


            // Display token to User
            //Console.WriteLine("The Decrypted token's values is : {0}", jwt );
        }
    }
}
