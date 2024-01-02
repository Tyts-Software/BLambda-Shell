using Amazon.CDK;
using System;

namespace Tyts.BLambda.Shell.Provision;

public sealed class ComponentStackProps : StackProps
{
    public const string APP_NAME = "shell";

    // unique prefix
    public string UPix => string.IsNullOrWhiteSpace(ComponentName) ? APP_NAME : $"{APP_NAME}-{ComponentName}";
    public string UPixName(string name) => $"{UPix}{name}";

    public string Deployment { get; set; }
    public string ComponentName { get; set; }
    public string Domain { get; set; }
    public string DomainLocal => string.IsNullOrWhiteSpace(Deployment) ? $"{UPix}.localhost" : $"{UPix}.{Deployment}.localhost";
    public string SubDomain { get; set; }
    public string SubDomainLocal => $"{SubDomain}.{DomainLocal}";
    public string LogLevel { get; set; }

    public string Name => UPix;
}


sealed class Program
{
    public static void Main(string[] args)
    {
        var app = new App();
        var props = new ComponentStackProps
        {
            Deployment = ((string)app.Node.TryGetContext("deployment") ?? "dev").ToLower(),
            ComponentName = ((string)app.Node.TryGetContext("component") ?? "").ToLower(),
            Domain = ((string)app.Node.TryGetContext("domain") ?? "").ToLower(),
            LogLevel = (string)app.Node.TryGetContext("log-level") ?? "Warning",

            Env = new Amazon.CDK.Environment
            {
                Account = System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT"),
                //?? throw new ArgumentNullException("CDK_DEFAULT_REGION");
                Region = System.Environment.GetEnvironmentVariable("CDK_DEFAULT_REGION")
                //?? throw new ArgumentNullException("CDK_DEFAULT_ACCOUNT");
            },
        };

        Console.WriteLine($"account: {props.Env.Account}");
        Console.WriteLine($"region: {props.Env.Region}");
        Console.WriteLine($"component: {props.ComponentName}");
        Console.WriteLine($"domain: {props.Domain ?? props.DomainLocal}");
        Console.WriteLine($"stack: {props.Name}");

        new ComponentStack(app, props.Name, props);


        Tags.Of(app).Add("APP", ComponentStackProps.APP_NAME);
        Tags.Of(app).Add("COMPONENT", props.Name);

        app.Synth();
    }
}
