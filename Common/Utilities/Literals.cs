using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public class Literals
    {
        #region Culturas

        public static string CultureEn = "en-EN";
        public static string CultureEs = "es-ES";

        #endregion

        #region Controllers

        public static string UsersController = "Users";
        public static string PostsController = "Posts";
        public static string CommentsController = "Comments";
        public static string MessagesController = "Messages";

        #endregion

        #region Roles

        public static string Role_Admin = "Admin";
        public static string Role_User = "User";
        public static string Role_Teacher = "Teacher";
        public static string Role_Student = "Student";

        #endregion

        #region Claims de los usuarios

        public static string Claim_Tag = "Tag";
        public static string Claim_LanguageId = "LanguageId";
        public static string Claim_Role = "Role";
        public static string Claim_UserId = "UserId";
        public static string Claim_Email = "Email";
        public static string Claim_PhoneNumber = "PhoneNumber";
        public static string Claim_Avatar = "Avatar";
        public static string Claim_Country = "Country";

        #endregion

        #region Datos email

        public static string Email_Name = "FireBreath";
        public static string Email_Address = "noreply.firebreath@gmail.com";
        public static string Email_Auth = "levp dwqb qacd vhle";
        public static string Email_Service = "smtp.gmail.com";
        public static int Email_Port = 587;

        #endregion

        #region Enlaces

        public static string Link_Recover = "http://localhost:4200/recover/";
        public static string Link_Review = "http://localhost:4200/link/";

        #endregion

        #region Redis

        public static string Redis_Users_Following = "following:";
        public static string Redis_New_Posts = "newposts:";
        public static string Redis_New_Post_Signal = "posts.new";
        #endregion
    }
}
