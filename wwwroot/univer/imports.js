/**
 * Returns true if the script was loaded correctly
 * @param {any} link Link for the script tag
 * @param {any} isScript True to indicate script tag. False to indicate link tag
 * @returns (boolean)
 */
export function chargeScript(link, isScript) {

    /**
     * Adds some scripts to document.head and waits until the last one is charged up
     * Code extracted from: https://stackoverflow.com/questions/18784920/how-to-add-dom-element-script-to-head-section
     */
    var loadScript = function (src, isScript) {
        // Initialize scripts queue
        if (loadScript.scripts === undefined) {
            loadScript.scripts = [];
            loadScript.index = -1;
            loadScript.loading = false;

            loadScript.next = function () {
                if (loadScript.loading)
                    return;

                // Load the next queue item
                loadScript.loading = true;

                var item = loadScript.scripts[++loadScript.index];
                var head = document.head;
                var element = null
                if (isScript) {
                    element         = document.createElement('script');
                    element.type    = 'text/javascript';
                    element.src     = item.src;
                }
                else {
                    element         = document.createElement('link');
                    element.rel     = 'stylesheet';
                    element.href    = item.src;
                }

                // When complete, start next item in queue and resolve this item's promise
                element.onload = () => {
                    loadScript.loading = false;
                    if (loadScript.index < loadScript.scripts.length - 1)
                        loadScript.next()
                    item.resolve()
                };

                head.appendChild(element);
            };
        };

        // Adding a script to the queue
        if (src) {

            // Check if already added
            for (var i = 0; i < loadScript.scripts.length; i++) {
                if (loadScript.scripts[i].src == src)
                    return loadScript.scripts[i].promise;
            }

            // Add to the queue
            var item = { src: src };
            item.promise = new Promise(resolve => { item.resolve = resolve; });
            loadScript.scripts.push(item);
            loadScript.next();
        }

        // Return the promise of the last queue item
        return loadScript.scripts[loadScript.scripts.length - 1].promise;
    };

    var charged = loadScript(link, isScript).then(function () {
        return true
    });
    return charged
}