using Amazon.CDK;

namespace DotnetAwsSecureCdk
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new DotnetAwsSecureCdkStack(app, "DotnetAwsSecureCdkStack", new StackProps
            {   
                Env = new Amazon.CDK.Environment
                {
                    Account = "544610684157",
                    Region = "us-west-2",
                }
            });
            app.Synth();
        }
    }
}
