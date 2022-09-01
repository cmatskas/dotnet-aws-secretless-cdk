using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.RDS;
using Amazon.CDK.AWS.SecretsManager;

namespace DotnetAwsSecureCdk
{
    public class DotnetAwsSecureCdkStack : Stack
    {
        public DotnetAwsSecureCdkStack(Amazon.CDK.Construct scope, string id, IStackProps? props = null) : base(scope, id, props)
        {
            var secret = Secret.FromSecretNameV2(this, id, "RDSPassword");

            var vpc = new Vpc(this, "MyVpcSample", new VpcProps
            {
                Cidr = "10.0.0.0/16",
                MaxAzs = 2,
                SubnetConfiguration = new ISubnetConfiguration[]
                {
                    new SubnetConfiguration
                    {
                        CidrMask = 24,
                        SubnetType = SubnetType.PUBLIC,
                        Name = "MyPublicSubnet"
                    },
                    new SubnetConfiguration
                    {
                        CidrMask = 24,
                        SubnetType = SubnetType.PRIVATE_WITH_NAT,
                        Name = "MyPrivateSubnet"
                    }
                }
            });
 
            const int dbPort = 1433;
            
            var db = new DatabaseInstance(this, "DB", new DatabaseInstanceProps
            {
                Vpc = vpc,
                VpcSubnets = new SubnetSelection{ SubnetType = SubnetType.PRIVATE_WITH_NAT },
                Engine = DatabaseInstanceEngine.SqlServerEx(new SqlServerExInstanceEngineProps { Version = SqlServerEngineVersion.VER_14 }),
                InstanceType = InstanceType.Of(InstanceClass.BURSTABLE2, InstanceSize.MICRO),
                Port = dbPort,
                Credentials = Credentials.FromSecret(secret,"rds_god"),
                InstanceIdentifier = "MyDbInstance",
                BackupRetention = Duration.Seconds(0)
            });
        }
            
    }
}
