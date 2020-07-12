
using System;

using Praticis.Framework.Layers.Domain.Abstractions;

namespace ProjectName.DomainName1.Domain.Models
{
    public class Address : BaseModel
    {
        public Guid CustomerId { get; private set; }
        public string Street { get; private set; }
        public string Number { get; set; }
        public virtual Customer Customer { get; private set; }
    }
}
