using Amazon.CDK;
using Amazon.CDK.AWS.SNS.Subscriptions;
using Amazon.CDK.AWS.SNS;
using Constructs;
using System.Collections.Generic;

namespace Tyts.BLambda.Shell.Provision;

public class ComponentStack : Stack
{
    internal ComponentStack(Construct scope, string id, ComponentStackProps props = null) : base(scope, id, props)
    {
        //// UI
        ////
        //var ui = new UiStack(scope, $"{props.Name}-ui", props);

        // COGNITO
        //
        var cognito = new CognitoConstruct(this, props);
    }
}
