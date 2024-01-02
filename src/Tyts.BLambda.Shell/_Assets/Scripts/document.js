
const blambda = {};

blambda.document = {
    linkStyle: function (id, href) {
        var link = document.getElementById(id);
        if (link === null) {
            link = document.createElement('link');
            link.rel = 'stylesheet';
            link.type = 'text/css';
            link.href = href;
            link.id = id;
            document.head.appendChild(link);
        }
    },

    embedStyle: function (id, style, placement) {
        var link = document.getElementById(id);
        if (link === null) {
            link = document.createElement('style');
            link.textContent = style;
            link.id = id;
            if (placement === 'head') {
                document.head.appendChild(link);
            }
            if (placement === 'body') {
                document.body.appendChild(link);
            }
        }
    },

    linkScript: function (id, src) {
        var script = document.getElementById(id);
        if (script === null) {
            script = document.createElement('script');
            script.type = 'text/javascript';
            script.src = src;
            script.id = id;
            document.body.appendChild(script);
        }
    }
};

export { blambda };
