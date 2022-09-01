using Amazon.CDK;
using Amazon.CDK.AWS.Events;
using Amazon.CDK.AWS.Events.Targets;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.SecretsManager;

namespace DotnetAwsSecureCdk
{
    public class DotnetAwsSecureCdkStack : Stack
    {
        public DotnetAwsSecureCdkStack(Amazon.CDK.Construct scope, string id, IStackProps? props = null) : base(scope, id, props)
        {
            var secret = Secret.FromSecretNameV2(this, id, "ApiKey");

            var lambdaFn = new Function(this, "Singleton", new FunctionProps {
                Runtime = Runtime.PYTHON_3_9,
                Code = Code.FromInline($"def main(event, context):{System.Environment.NewLine}    print(\"Do something with the ApiKey \"){System.Environment.NewLine}"),
                Handler = "index.main",
                Timeout = Duration.Seconds(300),
            });

            lambdaFn.AddEnvironment("ApiKey", secret.SecretValue.UnsafeUnwrap());

    
            var rule = new Rule(this, "Rule", new RuleProps {
                Schedule = Schedule.Expression("cron(0 18 ? * MON-FRI *)"),
            });

            rule.AddTarget(new LambdaFunction(lambdaFn));
        }
            
    }
}
