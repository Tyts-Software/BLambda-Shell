using Amazon.CDK;
using Amazon.CDK.AWS.Cognito;
using Amazon.CDK.AWS.SSM;
using Constructs;
using System.Collections.Generic;
using Tyts.BLambda.Shell.Domain.Auth;

namespace Tyts.BLambda.Shell.Provision;

internal sealed class CognitoConstruct : Construct
{
    internal static class Attr
    {
        public const string CUSTOM_EXAMPLE = "custom_ex";
    }


    private readonly UserPool userPool;

    public CognitoConstruct(Construct scope, ComponentStackProps props) : base(scope, $"{props.ComponentName}{props.Domain}Cognito")
    {
        userPool = new UserPool(this, "UserPool", new UserPoolProps
        {
            UserPoolName = $"{props.UPix}-user-pool",

            PasswordPolicy = new PasswordPolicy
            {
                MinLength = 6,
                RequireUppercase = false,
                RequireLowercase = false,
                RequireDigits = false,
                RequireSymbols = false
            },

            AccountRecovery = AccountRecovery.NONE,
            Mfa = Mfa.OFF,
            MfaSecondFactor = new MfaSecondFactor { Otp = false, Sms = false },
            EnableSmsRole = false,

            SelfSignUpEnabled = false,
            SignInCaseSensitive = false,
            SignInAliases = new SignInAliases { Username = false, Email = true, Phone = false },
            AutoVerify = new AutoVerifiedAttrs { Phone = false, Email = true },
            StandardAttributes = new StandardAttributes
            {
                Email = new StandardAttribute { Mutable = false, Required = true }
            },
            CustomAttributes = new Dictionary<string, ICustomAttribute>
            {
                [Attr.CUSTOM_EXAMPLE] = new StringAttribute(new StringAttributeProps { Mutable = true })
            },

            RemovalPolicy = RemovalPolicy.DESTROY,
        });

        //var userPoolBotClient = userPool.AddClient("BLambdaApiCognitoUserPoolClient", new UserPoolClientOptions()
        //{
        //    UserPoolClientName = "Tyts.BLambda.Api.Client", //???

        //    AccessTokenValidity = Duration.Minutes(5),
        //    //AuthSessionValidity =,
        //    IdTokenValidity = Duration.Minutes(5),
        //    RefreshTokenValidity = Duration.Hours(1),
        //    EnableTokenRevocation = false,

        //    //OAuth = ,
        //    DisableOAuth = true,

        //    AuthFlows = new AuthFlow { AdminUserPassword = true, UserPassword = false, UserSrp = false, Custom = false },
        //    PreventUserExistenceErrors = true,
        //    GenerateSecret = false,
        //    ReadAttributes = new ClientAttributes()
        //        .WithStandardAttributes(new StandardAttributesMask { Email = true })
        //        .WithCustomAttributes(Attr.CUSTOM_EXAMPLE),
        //    WriteAttributes = new ClientAttributes()
        //        .WithStandardAttributes(new StandardAttributesMask { Email = true })
        //        .WithCustomAttributes(Attr.CUSTOM_EXAMPLE),
        //});

        // Resource Server - protected APIs
        userPool.AddResourceServer("CognitoUserPoolWeatherForecastApiResourceServer", new UserPoolResourceServerOptions()
        {
            UserPoolResourceServerName = "Weather Forecast API",
            Identifier = "api://weather-forecast",
            Scopes = new[]
            {
                new ResourceServerScope(new ResourceServerScopeProps
                {
                    ScopeName = "access-by-role",
                    ScopeDescription = "User access allowed authoruzed by roles"
                })
            }
        });

        userPool.AddResourceServer("CognitoUserPoolTemperatureLogApiResourceServer", new UserPoolResourceServerOptions()
        {
            UserPoolResourceServerName = "Temperature Log API",
            Identifier = "api://temperature-log",
            Scopes = new[]
            {
                new ResourceServerScope(new ResourceServerScopeProps
                {
                    ScopeName = "access-by-role",
                    ScopeDescription = "User access allowed authoruzed by roles"
                })
            }
        });


        var shellUserPoolClient = userPool.AddClient("BLambdaShellCognitoUserPoolClient", new UserPoolClientOptions()
        {
            UserPoolClientName = "Tyts.BLambda.Shell.Client",

            //AuthSessionValidity =,
            AccessTokenValidity = Duration.Days(1),
            IdTokenValidity = Duration.Days(1),
            RefreshTokenValidity = Duration.Days(365),
            EnableTokenRevocation = true,

            DisableOAuth = false,
            OAuth = new OAuthSettings
            {
                Flows = new OAuthFlows { AuthorizationCodeGrant = true, ImplicitCodeGrant = false, ClientCredentials = false },
                Scopes = new[]
                {
                    OAuthScope.EMAIL,
                    OAuthScope.PROFILE,
                    //OAuthScope.COGNITO_ADMIN

                    // Make sure to take care of the convention for scopes: <resourceserver-identifier>//<scope-name> (notice the double slash).
                    OAuthScope.Custom("api://weather-forecast/access-by-role"),
                    OAuthScope.Custom("api://temperature-log/access-by-role")
                },
                CallbackUrls = new[]
                {
                    "https://localhost:4433/authentication/login-callback"
                },
                LogoutUrls = new[]
                {
                    "https://localhost:4433/authentication/logged-out"
                }
            },
            SupportedIdentityProviders = new[]
            {
                UserPoolClientIdentityProvider.COGNITO
            },

            AuthFlows = new AuthFlow { AdminUserPassword = false, UserPassword = false, UserSrp = false, Custom = false },

            PreventUserExistenceErrors = true,
            GenerateSecret = false,
            ReadAttributes = new ClientAttributes()
                .WithStandardAttributes(new StandardAttributesMask { Email = true })
                .WithCustomAttributes(Attr.CUSTOM_EXAMPLE),
            WriteAttributes = new ClientAttributes()
                .WithStandardAttributes(new StandardAttributesMask { Email = true })
                .WithCustomAttributes(Attr.CUSTOM_EXAMPLE),
        });

        var idpDomain = string.IsNullOrEmpty(props.Domain) ? props.UPix : props.Domain;
        userPool.AddDomain("ShellIdPDomain", new UserPoolDomainOptions
        {
            CognitoDomain = new CognitoDomainOptions { DomainPrefix = $"idp-{idpDomain}" }
        });

        new CfnUserPoolGroup(this, "CognitoUserPoolAdminGroup", new CfnUserPoolGroupProps
        {
            UserPoolId = userPool.UserPoolId,
            GroupName = Roles.ADMIN
        });

        new CfnUserPoolGroup(this, "CognitoUserPoolBossGroup", new CfnUserPoolGroupProps
        {
            UserPoolId = userPool.UserPoolId,
            GroupName = Roles.EDITOR
        });


        // SSM Params
        var userPoolMetadataAddress = new StringParameter(this, "UserPoolMetadataAddress", new StringParameterProps
        {
            ParameterName = "/Shell/JwtBearer/MetadataAddress",
            StringValue = $"{userPool.UserPoolProviderUrl}/.well-known/openid-configuration",
            Tier = ParameterTier.STANDARD
        });

        new StringParameter(this, "UserPoolId", new StringParameterProps
        {
            ParameterName = "/Shell/JwtBearer/TokenValidationParameters/ValidIssuer",
            StringValue = userPool.UserPoolId,
            Tier = ParameterTier.STANDARD
        });

        new StringParameter(this, "UserPoolClientId0", new StringParameterProps
        {
            ParameterName = "/Shell/JwtBearer/TokenValidationParameters/ValidAudiences/0",
            StringValue = shellUserPoolClient.UserPoolClientId,
            Tier = ParameterTier.STANDARD
        });


        new CfnOutput(scope, "MetadataAddress", new CfnOutputProps
        {
            Value = userPoolMetadataAddress.StringValue,
            ExportName = $"{props.UPix}MetadataAddress"
        });
        new CfnOutput(scope, "Authority", new CfnOutputProps
        {
            Value = userPool.UserPoolProviderUrl,
            ExportName = $"{props.UPix}Authority"
        });
        new CfnOutput(scope, "UserPoolId", new CfnOutputProps
        {
            Value = userPool.UserPoolId,
            ExportName = $"{props.UPix}UserPoolId"
        });
        new CfnOutput(scope, "ClientId", new CfnOutputProps
        {
            Value = shellUserPoolClient.UserPoolClientId,
            ExportName = $"{props.UPix}ClientId"
        });

        //todo: how to get IdP URL? 
        //new CfnOutput(scope, "IdPUri", new CfnOutputProps
        //{
        //    Value = userPool.,
        //    ExportName = $"{props.UPix}IdPUri"
        //});
    }

    //public void Grant(IGrantable api)
    //{
    //    userPool.Grant(api, new[]
    //    {
    //        "cognito-idp:AdminInitiateAuth",
    //        "cognito-idp:RespondToAuthChallenge",
    //        "cognito-idp:AdminGetUser",
    //        "cognito-idp:AdminCreateUser",
    //        "cognito-idp:AdminDeleteUser",
    //        "cognito-idp:ListUsers",
    //        "cognito-idp:AdminEnableUser",
    //        "cognito-idp:AdminDisableUser",
    //        "cognito-idp:AdminUpdateUserAttributes"
    //    });
    //}
}
