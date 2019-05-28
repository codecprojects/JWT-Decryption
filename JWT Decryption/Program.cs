using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace JWT_Decryption
{
    class Program
    {
        static void Main(string[] args)
        {

        }
        // JWS - JSON Web signarture
        //        {
        //"alg":"HS256"
        //}
        //{
        //"iss":"DAFM",
        //"exp":1557747358,
        //"cid":22127
        //}

        /* Which is signed with the following test key: Fdh9u8rINxfivbrianbbVT1u232VQBZYKx1HGAGPt2I */

        /* To produce signed Base64 value: eyJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJEQUZNIiwiZXhwIjoxNTU3NzQ3MzU4LCJjaWQiOjIyMTI3fQ.pGhHG38KChajrqZ3eLdPkufmYRUR2OqiF0z_9XLlSVc */

        // We then wrap this JWS as payload in a JWE:
        //{
        //"alg":"dir",
        //"enc":"A256GCM",
        //"cty":"jwt"
        //}
        //{
        //eyJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJEQUZNIiwiZXhwIjoxNTU3NzQ3MzU4LCJjaWQiOjIyMTI3fQ.pGhHG38KChajrqZ3eLdPkufmYRUR2OqiF0z_9XLlSVc
        //    }

        //        Which is encrypted with the following test key:
        //Bdh9u8rINxfivbrianbbVT1u232VQBZYKx1HGAGPt2I

/* To produce encrypted Base64 value: 
 * eyJhbGciOiJkaXIiLCJlbmMiOiJBMjU2R0NNIiwiY3R5Ijoiand0In0
 * ..46h3grwnT9YzIsWl.3/*Gk6ZqVyqrmVPG50B3lNBGfwXJOJJHrb8hmIyEMK5DfUSoikm9
 * _G_87_WuEY0SPJfpq5Lr1rx7HJ3D1cHHIrlanH68F5MKSbPE_w_bEu6dG2QniwcsH8QaYTH0vNnuwkAxOA
 * _bck_OR4D0FpMepvRzUMZLLkzHYWBWvV.J8AwlEqTM0JlbfQZVeFabw */
    }
}
