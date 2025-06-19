using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Shared.ImportExport
{
    [XmlRoot("Events")]
    public class EventList
    {
        [XmlElement("Event")]
        public List<EventXmlDto> Events { get; set; } = new();
    }
}
