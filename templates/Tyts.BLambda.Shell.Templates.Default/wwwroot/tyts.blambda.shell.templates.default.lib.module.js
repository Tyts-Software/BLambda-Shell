// just an example of JavaScript initializers
// https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/startup?view=aspnetcore-8.0#javascript-initializers

import { blambda } from "/_content/Tyts.BLambda.Blazor.Wasm/js/bl.m.min.js";

var beforeStartCalled = false;
var afterStartedCalled = false;

export function beforeWebAssemblyStart(options, extensions) {
    if (beforeStartCalled) return;

    beforeStartCalled = true;
}

export function afterWebAssemblyStarted(blazor) {
    if (afterStartedCalled) return;


    afterStartedCalled = true;
}