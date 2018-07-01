using CardinalAppXamarin.ViewModels.Base;
using CardinalLibrary;
using CardinalLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CardinalAppXamarin.ViewModels
{
    public class FriendViewCellModel : ViewModelBase
    {
        public FriendViewCellModel(
            UserInfoContract userInfo, 
            DateTime timeStamp,
            FriendRequestType type,
            FriendStatus status)
        {
            UserId = userInfo.Id;
            UserName = userInfo.UserName;
            FirstName = userInfo.FirstName;
            LastName = userInfo.LastName;
            Gender = userInfo.Gender;
            PhoneNumber = userInfo.PhoneNumber;
            Email = userInfo.Email;
            TimeStamp = timeStamp;
            RequestType = type;
            Status = status;
        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AccountGender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public String Email { get; set; }

        public DateTime TimeStamp { get; set; }
        public FriendRequestType RequestType { get; set; }
        public FriendStatus Status { get; set; }

        // TODO: Create visibility members for buttons from FriendStatus
        //       And derived-text from TimeStamp
        public bool ContactSearchVisibility => Status.Equals(FriendStatus.FoundInContactSearch);
        public bool AcceptPendingVisibility => Status.Equals(FriendStatus.PendingRequest);
        public bool RequestSentVisibility => Status.Equals(FriendStatus.Initiated);
        public bool MutualFriendVisibility => Status.Equals(FriendStatus.Mutual);
        
        public string FirstAndLastName => String.Format("{0} {1}", FirstName, LastName);
        public string FriendsForXDays => String.Format("Friends for {0} days.", (int)(DateTime.Now - TimeStamp).TotalDays);
        public string ZoneDescription { get; set; }
        public string FormattedPhoneNumber
        {
            get
            {
                if(PhoneNumber.Length != 10)
                {
                    return PhoneNumber;
                }
                string areacode = PhoneNumber.Substring(0, 3);
                string mid = PhoneNumber.Substring(3, 3);
                string last = PhoneNumber.Substring(6, 4);
                return string.Format("({0}){1}-{2}", areacode, mid, last);
            }
        }
        public override Task OnAppearingAsync()
        {
            return Task.CompletedTask;
        }
    }
}
