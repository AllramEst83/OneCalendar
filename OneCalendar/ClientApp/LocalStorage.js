

var LocalStorage = {
    KeyStart: "__.__One__.__Calender__.__User__.__Token_",
    Set: function (key, input) {      

        if (typeof(Storage) !== "undefined") {
            localStorage[key] = JSON.stringify(input);
            console.log("%c LocalStorage is set.Token saved with key: " + key + ", " + input, 'background: #222;color:#bada55');
        } else {
            console.log("Sorry! No Web Storage support..", ' color: red');
        }

    },
    KeyStartsWith: function (keyStart) {
             
        for (var i = 0; i < window.localStorage.length; i++) {

            var key = window.localStorage.key(i);

            var kesyStartsWith = key.slice(0, 41);
            if (kesyStartsWith === keyStart) {
                
                var results = {
                    token: JSON.parse(window.localStorage.getItem(key))
                };

                console.log(`%c Found key that starts with: ${this.KeyStart}, Full keyname is: ${key}, value is: ${results[0]}`, "background: #222; color:#bada55");

                return results;
            }   
        }
        console.log("%c Found no key", 'background: #222;color:#bada55');
        return "0";
    },
    Get: function (key) {

        var stored = localStorage[key] !== "undefined" ? localStorage[key] : "0";

        if (stored !== "0") {

            console.log("%c LocalStorage content is found and returned", 'background: #222;color:#bada55');

            return JSON.parse(token);
        }
        return false;
    },
    DeleteLocalStorageWithKey: function () {

        console.log("%c LocalStorage deleted with key.", 'background: #222;color:#bada55');

        window.localStorage.removeItem(key);
    }
};