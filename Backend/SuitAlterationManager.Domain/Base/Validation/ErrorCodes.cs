namespace SuitAlterationManager.Domain.Base.Validation
{
    public class ErrorCodes
    {
        public const string ElementNotFound = "ElementNotFound";
        public const string EmailDuplicated = "EmailDuplicated";
        public const string EmailNotValid = "EmailNotValid";
        public const string EntityDoesNotExist = "EntityDoesNotExist";
        public const string ErrorOccured = "ErrorOccured";  
        public const string InvalidData = "InvalidData";
        public const string InvalidFile = "InvalidFile";
        public const string InvalidEmailOrPassword = "InvalidEmailOrPassword";
        public const string InvalidEnum = "InvalidEnum";
        public const string InvalidResetToken = "InvalidResetToken";
        public const string InvalidToken = "InvalidToken";
        public const string InvalidUser = "InvalidUser";
        public const string PermissionDenied = "PermissionDenied";
        public const string TokenIsRequired = "TokenIsRequired";
        public const string TokenNotFound = "TokenNotFound";
        public const string UserAlreadyVerified = "UserAlreadyVerified";
        public const string UserDoesNotExist = "UserDoesNotExist";
        public const string UserGroupDoesNotExist = "UserGroupDoesNotExist";
        public const string UserGroupDuplicated = "GroupDuplicated";
        public const string UserLoginDoesNotExist = "UserLoginDoesNotExist";
        public const string UserMustVerify = "UserMustVerify";
        public const string UserWithoutGroups = "UserWithoutGroups";
    }
}