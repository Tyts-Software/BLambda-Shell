# Blazor + AWS Lambda = BLambda

Modular, serverless aproach to build Progressive Web Applications (PWA) and not only, presented by WebAssembly hosted Blazor and served by AWS.

Welcome to proof of concept journey! 

## Getting Started

```
Goal: Find out one more solution for building serverless, modular PWA.
Main assumption: Let's say it would be Blazor for UI and AWS Lambda for BL.
```

## Components
|								| URI	| Description	|
| -----							| ----- | ----			|
| BLambda.Identity				| https://id.bl.dev.lcl:4431; http://id.bl.dev.lcl:801					| OIDC Identity Provider |
| BLambda.Shall					| https://admin.bl.dev.lcl:4433; http://admin.bl.dev.lcl:803			| UI Shell, Blazor WASM |
| BLambda.Will.WeatherForecast	| https://api.bl.dev.lcl:58526; http://api.bl.dev.lcl:58529				| Demo Will Module, Web API|

## Tools

Install or update the [latest tooling](https://github.com/aws/aws-extensions-for-dotnet-cli), this lets you deploy and run Lambda functions.

`dotnet tool install -g Amazon.Lambda.Tools`
`dotnet tool update -g Amazon.Lambda.Tools`

Install or update the latest [Lambda function templates](https://github.com/aws/aws-lambda-dotnet/).

`dotnet new --install Amazon.Lambda.Templates

### [CDK](https://docs.aws.amazon.com/cdk/v2/guide/cli.html)

`cdk --version`


CLI is not compatible with the CDK library

`npm uninstall -g aws-cdk && npm install -g aws-cdk`

#### Bootstrap
Deploys the CDK Toolkit staging stack
`cdk bootstrap 1111111111/eu-central-1` 

where `1111111111` is your AWS account ID and `eu-central-1` is target region



- [Blazor component library for FluentUI](https://github.com/microsoft/fast-blazor)
- [NSwag](https://github.com/RicoSuter/NSwag)
- [Bundler & Minifier](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.ExtensibilityTools)


## Assumptions

| Assumption	| POC	|
| -----			| ----	|
| [Assumption #0](https://github.com/Tyts-Software/blambda/tree/master/poc/00-host-blazor-on-s3) | It should be Blazor (WebAssembly) app hosted on S3 |
| [Assumption #1](https://github.com/Tyts-Software/blambda/tree/master/poc/01-blazor-use-http-api-lambda) | It should be Weather forecast API Lambda |
| [Assumption #2](https://github.com/Tyts-Software/blambda/tree/master/poc/02-blazor-wasm-lazyloads-moduls) | RCL lazy loading it is not true modular solution but..|
| [Assumption #3](https://github.com/Tyts-Software/blambda/tree/master/poc/03-use-custom-oidc-identity-provider) | It is possible create and use custom OIDC identity provider|
| [Assumption #4](https://github.com/Tyts-Software/blambda/tree/master/poc/04-use-cognito-user-pool-as-odic-identity-provider) | It is possible use Cognito User Pool as OIDC identity provider|


**IMPORTANT NOTE:** *Playing around with this POCs your AWS account will create and consume AWS resources, which **will cost money**. Be sure to shut down/remove all resources once you are finished to avoid ongoing charges to your AWS account (see instructions in delete-stack.ps1).*
