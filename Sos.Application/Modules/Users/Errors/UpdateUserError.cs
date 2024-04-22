using Sos.Domain.Core.Commons.Bases;

namespace Sos.Application.Modules.Users.Errors
{
    /// <summary>
    /// Contains the update user errors.
    /// </summary>
    public static class UpdateUserError
    {
        public static Error FirstNameIsRequired =>
            new
            (
                "CreateUserError.FirstNameIsRequired",
                "The first name is required."
            );

        public static Error FirstNameIsTooShort =>
            new
            (
                "CreateUserError.FirstNameIsTooShort",
                "The first name is too short."
            );

        public static Error FirstNameIsTooLong =>
            new
            (
                "CreateUserError.FirstNameIsTooLong",
                "The first name is too long."
            );

        public static Error LastNameIsRequired =>
            new
            (
                "CreateUserError.LastNameIsRequired",
                "The last name is required."
            );

        public static Error LastNameIsTooShort =>
            new
            (
                "CreateUserError.LastNameIsTooShort",
                "The last name is too short."
            );

        public static Error LastNameIsTooLong =>
            new
            (
                "CreateUserError.LastNameIsTooLong",
                "The last name is too long."
            );

        public static Error ContactPhoneIsRequired =>
            new
            (
                "CreateUserError.ContactPhoneIsRequired",
                "The contact phone is required."
            );
    }
}
