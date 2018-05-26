
using System.Collections.Generic;

namespace CardinalLibrary.DataContracts
{
    public class ZoneContract
    {
        public string ZoneID { get; set; }
        public string Description { get; set; }
        public string VisibleLayersDelimited { get; set; }
        public string ARGBFill { get; set; }
        public ZoneType Type { get; set; }
        public IEnumerable<ZoneShapeContract> ZoneShapes { get; set; }
    }
}
