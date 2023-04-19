using CodeActivity;
using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;

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

            var expected = new Dictionary<string, object> { { "ResultLink", null }, { "ResultStatus", 2 } };

            Assert.AreEqual(expected.Values.ToArray()[0], result.Values.ToArray()[0]);
            Assert.AreEqual(expected.Values.ToArray()[1], result.Values.ToArray()[1]);
        }

        [TestMethod]
        [Obsolete]
        public void CorrectValues()
        {
            var fakedContext = new XrmFakedContext();
            //fakedContext.AddExecutionMock<CalculateRollupFieldRequest>(new CalculateRollupFieldResponse());
            var service = fakedContext.GetOrganizationService();

            var contactEntity = new Entity("contact");
            contactEntity.Id = Guid.NewGuid();
            contactEntity.LogicalName = "contact";
            contactEntity["phone"] = "+380746377352";
            contactEntity["fax"] = "+380746377352";

            //Guid contactId = service.Create(contactEntity);
            fakedContext.Initialize(new List<Entity>() {
                contactEntity });

            //var request = new RetrieveRequest
            //{
            //    Target = new EntityReference("contact", "phone", "+380746377352"),
            //    ColumnSet = new ColumnSet(true)
            //};

            //var result1 = service.Execute(request);
            QueryExpression query = new QueryExpression("contact");
            var en = query.EntityName;
            query.Criteria.AddCondition(new ConditionExpression("LogicalName", ConditionOperator.Equal, "contact"));
            DataCollection<Entity> contacts = service.RetrieveMultiple(query).Entities;

            var inputs = new Dictionary<string, object> { { "InArg1", "+380746377352" }, { "FieldName1", "fax" },
               { "InArg2", "+380746377352" }, { "FieldName2", "phone" } };

            //var inputs = new Dictionary<string, object> { { "InArg1", guid.ToString() }, { "FieldName1", "Id" },
            //   { "InArg2", "contact1" }, { "FieldName2", "LogicalName" } };

            //var set = inputs.Reverse();
            //inputs = set.ToDictionary(x => x.Key, x => x.Value);
            var result = fakedContext.ExecuteCodeActivity<Contact>(inputs);

            var expected = new Dictionary<string, object> { { "ResultLink", contactEntity.LogicalName }, { "ResultStatus", 1 } };

            Assert.AreEqual(expected.Values.ToArray()[0], result.Values.ToArray()[0]);
            Assert.AreEqual(expected.Values.ToArray()[1], result.Values.ToArray()[1]);
        }


        [TestMethod]
        [Obsolete]
        public void CorrectValuesStatus3()
        {
            var fakedContext = new XrmFakedContext();
            //fakedContext.AddExecutionMock<CalculateRollupFieldRequest>(new CalculateRollupFieldResponse());
            var service = fakedContext.GetOrganizationService();

            var contactEntity1 = new Entity("contact");
            contactEntity1.Id = Guid.NewGuid();
            contactEntity1.LogicalName = "contact1";
            contactEntity1["phone"] = "+380746377352";
            contactEntity1["fax"] = "+380746377352";

            var contactEntity2 = new Entity("contact2");
            contactEntity2.Id = Guid.NewGuid();
            contactEntity2.LogicalName = "contact";
            contactEntity2["phone"] = "+380746377352";
            contactEntity2["fax"] = "+380746377352";

            //Guid contactId = service.Create(contactEntity);
            fakedContext.Initialize(new List<Entity>() {
                contactEntity1, contactEntity2 });

            //var request = new RetrieveRequest
            //{
            //    Target = new EntityReference("contact", "phone", "+380746377352"),
            //    ColumnSet = new ColumnSet(true)
            //};

            //var result1 = service.Execute(request);
            QueryExpression query = new QueryExpression("contact");
            var en = query.EntityName;
            query.Criteria.AddCondition(new ConditionExpression("LogicalName", ConditionOperator.Equal, "contact"));
            DataCollection<Entity> contacts = service.RetrieveMultiple(query).Entities;

            var inputs = new Dictionary<string, object> { { "InArg1", "+380746377352" }, { "FieldName1", "fax" },
               { "InArg2", "+380746377352" }, { "FieldName2", "phone" } };

            var result = fakedContext.ExecuteCodeActivity<Contact>(inputs);

            var expected = new Dictionary<string, object> { { "ResultLink", contactEntity1.LogicalName + "; " + contactEntity2.LogicalName }, { "ResultStatus", 1 } };

            Assert.AreEqual(expected.Values.ToArray()[0], result.Values.ToArray()[0]);
            Assert.AreEqual(expected.Values.ToArray()[1], result.Values.ToArray()[1]);
        }
    }
}
