
using System;
using System.Collections.Generic;

namespace ProjectName.DomainName1.Application.ViewModels
{
    public class CustomerViewModel
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public AddressViewModel Address { get; set; }
        public IEnumerable<ContactViewModel> Contacts { get; set; }
    }
}