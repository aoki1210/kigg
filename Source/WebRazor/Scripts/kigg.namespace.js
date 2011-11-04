var kigg = kigg || {};
kigg.namespace = function (nsString) {
    var parts = nsString.split('.'),
        partsLength = parts.length,
        parent = kigg,
        i;

    // strip redundant leading global
    if (parts[0] === "kigg") {
        parts = parts.slice(1);
    }

    for (i = 0; i < partsLength; i += 1) {
        // create a property if it doesn't exist
        if (typeof parent[parts[i]] === "undefined") {
            parent[parts[i]] = {};
        }
        parent = parent[parts[i]];
    }
    return parent;
};
