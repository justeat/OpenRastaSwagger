using System;

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
