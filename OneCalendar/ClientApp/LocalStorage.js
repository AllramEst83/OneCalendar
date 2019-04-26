

var LocalStorage = {

    LocalStorageKey: "__.__One__.__Calender__.__User__.__Data",

    Set: function (key, input) {      

        if (typeof(Storage) !== "undefined") {
            localStorage[key] = JSON.stringify(input);
            console.log("%c LocalStorage is set.Token saved with key: " + key + ", " + input, 'background: #222;color:#bada55');
        } else {
            console.log("Sorry! No Web Storage support..", ' color: red');
        }

    },    
    Get: function (key) {

        var stored = localStorage[key] !== undefined ? localStorage[key] : "0";

        if (stored !== "0" ) {

            console.log("%c LocalStorage content is found and returned", 'background: #222;color:#bada55');

            return JSON.parse(stored);
        }
        return stored;
    },
    DeleteLocalStorageWithKey: function (key) {

        console.log("%c LocalStorage deleted with key.", 'background: #222;color:red');

        window.localStorage.removeItem(key);
    },
    DeleteStorageOnBrowserClose: function () {

        $(window).on("unload", function (e) {
            LocalStorage.DeleteLocalStorageWithKey(LocalStorage.LocalStorageKey);
        });
    }
};