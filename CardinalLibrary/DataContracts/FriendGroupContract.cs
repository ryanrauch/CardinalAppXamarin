using System.Collections.Generic;

namespace CardinalLibrary.DataContracts
{
    public class FriendGroupContract
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string Description { get; set; }
        public IEnumerable<FriendGroupUserContract> Users { get; set; }
    }
}
