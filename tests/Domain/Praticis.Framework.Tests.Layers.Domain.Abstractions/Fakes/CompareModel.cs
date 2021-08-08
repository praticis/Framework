
using System;

using Praticis.Framework.Layers.Domain.Abstractions;

namespace Praticis.Framework.Tests.Layers.Domain.Abstractions.Fakes
{
    public class CompareModel : BaseModel
    {
        public void ChangeId(Guid id)
            => this.Id = id;
    }
}