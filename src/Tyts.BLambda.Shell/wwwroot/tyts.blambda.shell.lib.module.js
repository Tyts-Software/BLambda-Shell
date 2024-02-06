// just an example of JavaScript initializers
// https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/startup?view=aspnetcore-8.0#javascript-initializers

import { loader } from "/_content/Tyts.BLambda.Blazor.Wasm/js/bl.m.min.js";


export function beforeWebAssemblyStart(options, extensions) {
    //debugger;

    //todo: strange but it doesnt work like expected
    // from above doc:
    // Client-side, beforeStart receives the Blazor options (options) and any extensions (extensions) added during publishing.
    // For example, options can specify the use of a custom boot resource loader.
    // so loadBootResource is set in index.html
    options.loadBootResource = function (type, filename, defaultUri, integrity) {
        return loader.fetch(type, filename, defaultUri, integrity);
    }    
}

export function afterWebAssemblyStarted(blazor) {

    // default is false. does it optimize something?
    blazor.runtime.disableDotnet6Compatibility = true;

    if (loader.logLoaded) {
        loader.logToConsole();
    }

    // clean up loading progress indicators
    //document.documentElement.removeAttribute('style');
    //document.documentElement.style.removeProperty('--blazor-load-percentage');
    //document.documentElement.style.removeProperty('--blazor-load-percentage-text');    

    // --- .net from js
    //const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    //var exports = await getAssemblyExports("Tyts.BLambda.Shell.dll");

    //var curTheme = exports.Tyts.BLambda.Shell.Domain.Themes.ThemeService.GetCurrent();
    //console.log(curTheme);
}