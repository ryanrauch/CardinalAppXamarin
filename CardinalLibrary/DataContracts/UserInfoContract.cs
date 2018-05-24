using System;

namespace CardinalLibrary.DataContracts
{
    public class UserInfoContract
    {
        public String Id { get; set; }
        public String UserName { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public AccountGender Gender { get; set; }
        public AccountType AccountType { get; set; }
        public String PhoneNumber { get; set; }
        public String Email { get; set; }
        //Only used when registering new users from api controller
        public String Password { get; set; }
    }
}
