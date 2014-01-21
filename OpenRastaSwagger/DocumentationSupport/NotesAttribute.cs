using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRastaSwagger.DocumentationSupport
{
    public class NotesAttribute : Attribute
    {
        public string Notes { get; set; }

        public NotesAttribute(string notes)
        {
            Notes = notes;
        }
    }
}
