using System;
using System.Collections.Generic;
using System.Text;

namespace ResPay.Core.Utilities
{
    public class StatusMessage
    {
        public static string OK = "OK";
        public static string INVALID_QUANTITY = "Quantity less than 1";
        public static string NOTFOUND = "Record not found";
        public static string UNATHORIZED = "Record not found";
        public static string BADREQUEST = "Record not found";
        public static string INVALID_USER = "Not a vaid user";
        public static string CREATED = "Record created successfully";
        public static string INVALID_USERNAME_ANDOR_PASSWORD = "Invalid username and/or password";
        public static string CONFIRMATION_REQUIRED = "Your account is yet to be confirmed";
        public static string PASSWORD_MISMATCH = "The password does not match";
        public static string USER_EXIST = "Username already exists";
        public static string NUMBER_EXIST = "User with this number already exists";
        public static string EMAIL_CONFIRMED = "Your email successfully confirmed";
        public static string INVALID_TOKEN = "Invalid token";
        public static string DB_ERROR = "Failed to save";
        public static string SERVER_ERROR = "Internal server error";
        public static string INVALID_INPUT = "Incorrect input value";
        public static string NO_EXTENSION = "No extension";
        public static string INVALID_EXTENSION = "Incorrect file extension";
        public static string NO_FILE = "No file uploaded";
        public static string RECORD_EXISTS = "Record already exist";
    }
}
