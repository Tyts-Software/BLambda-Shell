(function () {
    if (typeof globalThis === 'object') return;
    Object.defineProperty(Object.prototype, '__magic__', {
        get: function () {
            return this;
        },
        configurable: true // This makes it possible to `delete` the getter later.
    });
    __magic__.globalThis = __magic__; // lolwat
    delete Object.prototype.__magic__;
}());



export function isMobile() {
    return /android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini|mobile/i.test(navigator.userAgent);
}
