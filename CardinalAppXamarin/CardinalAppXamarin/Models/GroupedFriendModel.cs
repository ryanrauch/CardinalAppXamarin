using CardinalAppXamarin.ViewModels;
using System.Collections.ObjectModel;

namespace CardinalAppXamarin.Models
{
    public class GroupedFriendModel : ObservableCollection<FriendViewCellModel>
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }
    }
}
