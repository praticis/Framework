
using System.Collections.Generic;

using Praticis.Framework.Layers.Domain.Abstractions;

namespace ProjectName.DomainName1.Domain.Models
{
    public class Customer : BaseModel
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
    }
}