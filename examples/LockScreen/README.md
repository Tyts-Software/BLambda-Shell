# BLambda CognitoAuth with LockScreen

AWS Cognito OIDC Authentication example.
Here is All pages under protection so we use LockScreen as lending.

## Check points
- package reference `Tyts.BLambda.Blazor.Wasm.Auth.Cognito`
- `appsettings.json` in wwwroot
- `blambda.AddCognito()` in Program.cs
- `bl.auth.cognito.min.js` in index.html
- `Tyts.BLambda.Blazor.Wasm.Auth.Cognito` assembly in `Router -> AdditionalAssemblies` in App.razor

- `AddDefaultTemplate -> UseLockScreen = true` in Program.cs
- `@attribute [Authorize]` to _Imports.razor
- `"PostLogoutRedirectUri": "https://[your-host]/authentication/logged-out"`


## Prerequisites
In order to run it you should do AWS provisioning 
1. Create AWS Account 
2. Setup CDK 
3. Run Tyts.BLambda.Shell.Provision

## Expected
![Demo](screenshort0.jpg?raw=true)

![Demo](screenshort1.jpg?raw=true)