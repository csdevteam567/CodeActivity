using System.Activities;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;

namespace CodeActivity
{
    public class Contact : System.Activities.CodeActivity
    {
        [Input("InArg1")]
        public InArgument<string> InArg1 { get; set; }

        [Input("FieldName1")]
        public InArgument<string> FieldName1 { get; set; }

        [Input("InArg2")]
        public InArgument<string> InArg2 { get; set; }

        [Input("FieldName2")]
        public InArgument<string> FieldName2 { get; set; }

        [Output("ResultStatus")]
        public OutArgument<int> ResultStatus { get; set; }

        [Output("ResultLink")]
        public OutArgument<string> ResultLink { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            IWorkflowContext execontext = context.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(execontext.UserId);

            QueryExpression query = new QueryExpression("contact");
            query.Criteria.AddCondition(new ConditionExpression(FieldName1.ToString(), ConditionOperator.Equal, InArg1.ToString()));
            query.Criteria.AddCondition(new ConditionExpression(FieldName2.ToString(), ConditionOperator.Equal, InArg2.ToString()));

            DataCollection<Entity>contacts = service.RetrieveMultiple(query).Entities;

            if (contacts.Count == 0)
                ResultStatus.Set(context, 2);
            else if (contacts.Count == 1)
            {
                ResultStatus.Set(context, 1);
                ResultLink.Set(context, contacts.First().LogicalName);
            }
            else
            {
                ResultStatus.Set(context, 3);
                ResultLink.Set(context, string.Join("; ", contacts.Select(c => c.LogicalName).ToList()));
            }
        }
    }
}
