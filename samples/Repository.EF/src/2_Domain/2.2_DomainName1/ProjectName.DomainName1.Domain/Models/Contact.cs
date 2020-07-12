
using System;

using Praticis.Framework.Layers.Domain.Abstractions;

using ProjectName.DomainName1.Domain.Enums;

namespace ProjectName.DomainName1.Domain.Models
{
    public class Contact : BaseModel
    {
        public Guid CustomerId { get; set; }
        public string Value { get; set; }
        public ContactType Type { get; set; }
        public Customer Customer { get; set; }
    }
}