using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeatBox.Services
{
    public static class AppMessages
    {
        public const string EmailAlreadyRegistered = "This email is already registered";
        public const string UsernameAlreadyRegistered = "This username is already registered";
        public const string InvalidLogin = "Not valid Email or Password";
        public const string PasswordError = "Invalid password format";
        public const string RegisterSuccess = "Registration completed successfully!";
        public const string LoginSuccess = "Welcome back!";
        public const string CreateAdminRole = "Admin Role Created successfully";
        public const string CreateRole = "Role Created successfully";
        public const string DeleteRole = "Role Deleted successfully";
        public const string AddToRole = "Added To The Role successfully";
        public const string RemoveFromRole = "Removed from the Role successfully";
        public const string DeleteUser = "User Deleted successfully";
        public const string FailedDeleteUser = "Failed to delete user. Please check if user is exist.";
        public const string OperationFaild = "Operation Faild";

    }
}