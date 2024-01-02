import {
    baseLayerLuminance,
    StandardLuminance,
    SwatchRGB,
    //PaletteRGB,
    accentBaseColor,
    neutralBaseColor,
} from "/js/web-components.min.js";

class Luminance {

    constructor() {
        var prefersColorSchemeChangeHandler = function (isDark) { };
        const mediaIsDark = window.matchMedia('(prefers-color-scheme: dark)');

        //on mode-switch event from OS
        mediaIsDark.addEventListener('change', e => {            
            if (typeof prefersColorSchemeChangeHandler === 'function') {
                prefersColorSchemeChangeHandler(e.matches);
            }
        });

        this.init = () => {
            var theme;
            try {
                const gezipedData = atob(globalThis.localStorage.getItem("theme"));
                const gzipedDataArray = Uint8Array.from(gezipedData, c => c.charCodeAt(0));
                const ungzipedData = globalThis.pako.ungzip(gzipedDataArray);
                const json = new TextDecoder().decode(ungzipedData);

                theme = JSON.parse(json);
            }
            catch {
                theme = {
                    DarkMode: mediaIsDark.matches
                };
            }

            this.setBaseLayerLuminance(theme.DarkMode);
        };

        this.setBaseLayerLuminance = (isDark) => {
            //StandardLuminance.DarkMode
            baseLayerLuminance.setValueFor(
                document.body,
                isDark === true ? StandardLuminance.DarkMode : StandardLuminance.LightMode
            );

            //accentBaseColor.setValueFor(
            //    document.body,
            //    SwatchRGB.create(0, 120, 212)
            //);

            //neutralBaseColor.setValueFor(
            //    document.body,
            //    SwatchRGB.create(128, 128, 128)
            //);
        }

        //Observable.getNotifier(this).subscribe(subscriber, 'fillColor');
		//Observable.getNotifier(this).subscribe(subscriber, 'baseLayerLuminance');

        // color format '#ffffff'
        // SwatchRGB.from(parseColorHexRGB(color))
        // import { parseColorHexRGB } from '@microsoft/fast-colors';
        // SwatchRGB.from(parseColorHexRGB(color))

        this.setAccentBaseColor = (swatchRGB) => accentBaseColor.setValueFor(
            document.body,
            swatchRGB           
        );

        // color format '#ffffff'
        this.setNeutralBaseColor = (swatchRGB) => neutralBaseColor.setValueFor(
            document.body,
            swatchRGB
        );

        this.onPrefersColorSchemeChange = (handler) => {
            if (typeof handler === 'function') {
                prefersColorSchemeChangeHandler = handler;
            }
        }
    }
}

const theme = new Luminance();
export { theme };