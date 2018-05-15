using System;
using System.Collections.Generic;
using System.Text;

namespace CardinalLibrary.DataContracts
{
    public class CurrentLayerContract
    {
        public String UserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public String LayersDelimited { get; set; }
    }
}
