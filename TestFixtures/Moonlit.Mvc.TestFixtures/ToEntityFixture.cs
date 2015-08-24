using System;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.Mvc.TestFixtures
{
    [TestClass]
    public class ToEntityFixture
    {
        [TestMethod]
        public void ToEntityTest()
        {
            MyModel model = new MyModel(13)
            {
                FirstName = "zz",
                BirthDate = DateTime.Parse("1911-1-1"),
                IsReadonly = "changed",
            };
            var entity = new MyEntity();
            model.ToEntity(entity, new ToEntityContext());
            Assert.AreEqual("zz", entity.FirstName);
            Assert.AreEqual(DateTime.Parse("1911-1-1"), entity.DateOfBirth, "mapping to is not working");
            Assert.AreEqual(13, entity.MyEntityId);
            Assert.IsNull(entity.IsReadonly);
        }
        [TestMethod]
        public void FromEntityIsNotPostbackTest()
        {
            MyModel model = new MyModel();
            var entity = new MyEntity
            {
                DateOfBirth = DateTime.Parse("1911-1-1"),
                MyEntityId = 13,
                FirstName = "zz",
                UpdateForIsPostback = "changed",
                IsReadonly = "is readonly should be changed in any case",
            };
            model.FromEntity(entity, new FromEntityContext() { IsPostback = false });
            Assert.AreEqual("zz", model.FirstName);
            Assert.AreEqual(DateTime.Parse("1911-1-1"), model.BirthDate, "mapping to is not working");
            Assert.AreEqual(13, model.MyEntityId);
            Assert.AreEqual("changed", model.UpdateForIsPostback);
            Assert.AreEqual("is readonly should be changed in any case", model.IsReadonly);
        }
        [TestMethod]
        public void FromEntityIsPostbackTest()
        {
            MyModel model = new MyModel();
            var entity = new MyEntity
            {
                FirstName = "zz",
                UpdateForIsPostback = "changed",
                IsReadonly = "is readonly should be changed in any case",
            };
            model.FromEntity(entity, new FromEntityContext() { IsPostback = true });
            Assert.IsNull(model.FirstName);
            Assert.AreEqual("changed", model.UpdateForIsPostback);
            Assert.AreEqual("is readonly should be changed in any case", model.IsReadonly);
        }
        public class MyModel : IToEntity<MyEntity>, IFromEntity<MyEntity>
        {
            public MyModel()
            {

            }

            public int MyEntityId { get; set; }

            public MyModel(int myEntityId)
            {
                MyEntityId = myEntityId;
            }

            #region Implementation of IToEntity<MyEntity>

            public void OnToEntity(MyEntity entity, ToEntityContext context)
            {
                entity.MyEntityId = MyEntityId;
            }

            #endregion

            [Mapping]
            public string FirstName { get; set; }
            [Mapping]
            [ReadOnly(true)]
            public string IsReadonly { get; set; }

            [Mapping(To = "DateOfBirth")]
            public DateTime BirthDate { get; set; }

            #region Implementation of IFromEntity<MyEntity>

            public void OnFromEntity(MyEntity entity, FromEntityContext context)
            {
                MyEntityId = entity.MyEntityId;
            }

            #endregion
            [Mapping(OnlyNotPostback = false)]
            public string UpdateForIsPostback { get; set; }
        }
        public class MyEntity
        {
            public int MyEntityId { get; set; }
            public string FirstName { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string UpdateForIsPostback { get; set; }
            public string IsReadonly { get; set; }
        }
    }
}
