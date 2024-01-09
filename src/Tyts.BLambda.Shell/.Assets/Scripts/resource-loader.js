
let loadedCount = 0;
const resourcesToLoad = [];

const resourceRequestsLog = [];

const div = document.createElement('div');
div.setAttribute("id", "statusBar");

const sb = {
    el: document.getElementById('statusBar') ?? document.body.appendChild(div),

    update: (status) => {
        sb.el.innerText = status;
        sb.show();
    },

    show: () => sb.el.classList.remove('none'),
    hide: () => {
        sb.el.innerText = "";
        sb.el.classList.add('none');
    }
}


const loader = {

    logLoaded: new URL(document.URL).hash.indexOf('logloaded') !== -1,

    logToConsole: function () {
        if (resourceRequestsLog.length > 0) {
            resourceRequestsLog.forEach((value, index, array) => console.log(`type:${value[0]} file:${value[1]}`));
        }
        else {
            console.log(`res-loader: nothing loaded`);
        }
    },

    fetch: function (type, filename, defaultUri, integrity) {
        sb.update(`Loading: ${filename}`);

        if (this.logLoaded) {
            resourceRequestsLog.push([type, filename, defaultUri, integrity]);            
        }

        // black magic here...
        // manifest is blazor.boot.json
        if (type == "dotnetjs" || type == "manifest") return defaultUri;

        const fetchResources = fetch(defaultUri, {
            cache: 'no-cache',
            integrity: integrity,
            //headers: { 'MyCustomHeader': 'My custom value' }
        });

        resourcesToLoad.push(fetchResources);

        fetchResources.then((r) => {
            sb.update(`Loading: ${++loadedCount}/${resourcesToLoad.length}: ${filename}`);
        }).then(() => {
            if (loadedCount == resourcesToLoad.length) {
                sb.hide();
            }
        });

        return fetchResources;
    }
};


export { loader };
