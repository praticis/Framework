
using Xunit;

using Praticis.Framework.Layers.Domain.Abstractions;
using Praticis.Framework.Tests.Layers.Domain.Abstractions.Fakes;
using System;

namespace Praticis.Framework.Tests.Layers.Domain.Abstractions.UnitTests
{
    public class BaseModelTests
    {
        [Fact]
        public void BaseModel_Created_Successfully()
        {
            var model = new DefaultModel();

            Assert.NotEqual(default, model.Id);
            Assert.Contains(model.Id.ToString(), model.ToString());
            Assert.Contains(typeof(DefaultModel).Name, model.ToString());
            Assert.True(model is IModel);
        }

        [Fact]
        public void BaseModel_When_Has_Two_Model_With_Same_Id_Then_Are_Equals()
        {
            var modelA = new CompareModel();
            var modelB = new CompareModel();

            Assert.NotEqual(modelA.Id, modelB.Id);

            modelB.ChangeId(modelA.Id);

            Assert.Equal(modelA.Id, modelB.Id);
            Assert.True(modelA.Equals(modelB));
            Assert.Equal(modelA.GetHashCode(), modelB.GetHashCode());
            Assert.True(modelA == modelB);
            Assert.False(modelA != modelB);
        }

        [Fact]
        public void BaseModel_When_Has_Two_Model_With_Different_Id_Then_Are_Not_Equals()
        {
            var modelA = new CompareModel();
            var modelB = new CompareModel();

            Assert.NotEqual(modelA.Id, modelB.Id);
            Assert.False(modelA.Equals(modelB));
            Assert.NotEqual(modelA.GetHashCode(), modelB.GetHashCode());
            Assert.False(modelA == modelB);
            Assert.True(modelA != modelB);
        }
    }
}