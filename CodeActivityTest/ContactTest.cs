using CodeActivity;
using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace CodeActivityTest
{
    [TestClass]
    public class ContactTest
    {

        [TestMethod]
        [Obsolete]
        public void NotFound()
        {
            var fakeContext = new XrmFakedContext();
            //var service = fakeContext.GetOrganizationService();

            var inputs = new Dictionary<string, object> { { "InArg1", "+380746377352" }, { "FieldName1", "Fax" },
               { "InArg2", "+380746377352" }, { "FieldName2", "Phone" } };

            var result = fakeContext.ExecuteCodeActivity<Contact>(inputs);

            var expected = new Dictionary<string, object> { { "ResultStatus", 2 }, { "ResultLink", string.Empty } };

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [Obsolete]
        public void CorrectValues()
        {
            var fakeContext = new XrmFakedContext();
            var service = fakeContext.GetOrganizationService();

            var contactEntity = new Entity("contact");
            contactEntity.Attributes.Add("Fax", "+380746377352");
            contactEntity.Attributes.Add("Phone", "+380746377352");

            Guid contactId = service.Create(contactEntity);

            var inputs = new Dictionary<string, object> { { "InArg1", "+380746377352" }, { "FieldName1", "Fax" },
               { "InArg2", "+380746377352" }, { "FieldName2", "Phone" } };

            var result = fakeContext.ExecuteCodeActivity<Contact>(inputs);

            var expected = new Dictionary<string, object> { { "ResultStatus", 1 }, { "ResultLink", contactEntity.LogicalName } };

            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        [Obsolete]
        public void CorrectValuesStatus3()
        {
            var fakeContext = new XrmFakedContext();
            var service = fakeContext.GetOrganizationService();

            var contactEntity1 = new Entity("contact");
            contactEntity1.Attributes.Add("Fax", "+380746377352");
            contactEntity1.Attributes.Add("Phone", "+380746377352");

            var contactEntity2 = new Entity("contact");
            contactEntity2.Attributes.Add("Fax", "+380746377352");
            contactEntity2.Attributes.Add("Phone", "+380746377352");

            Guid contact1Id = service.Create(contactEntity1);
            Guid contact2Id = service.Create(contactEntity2);

            var inputs = new Dictionary<string, object> { { "InArg1", "+380746377352" }, { "FieldName1", "Fax" },
               { "InArg2", "+380746377352" }, { "FieldName2", "Phone" } };

            var result = fakeContext.ExecuteCodeActivity<Contact>(inputs);

            var expected = new Dictionary<string, object> { { "ResultStatus", 1 }, { "ResultLink", contactEntity1.LogicalName + "; " + contactEntity2.LogicalName } };

            Assert.AreEqual(expected, result);
        }
    }
}
