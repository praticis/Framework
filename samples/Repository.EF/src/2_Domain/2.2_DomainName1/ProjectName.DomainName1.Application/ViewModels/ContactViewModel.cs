
using System;

using ProjectName.DomainName1.Application.Enums;

namespace ProjectName.DomainName1.Application.ViewModels
{
    public class ContactViewModel
    {
        public Guid ContactId { get; set; }
        public string Value { get; set; }
        public ContactType Type { get; set; }
    }
}