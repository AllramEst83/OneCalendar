
var ApiObject = ApiObject || {};

    
var FormObject = {

    BindLoggaIn: function () {

        $("#loggaInKnapp").on("click", function (e) {
            e.preventDefault();

            var userName = $("#userNameInput").val().trim();
            var password = $("#passwordInput").val().trim();

            if (userName === '' || password === '') {
                alert("please fill out form");
            } else {
                var settings = {
                    "url": "https://localhost:44305/api/auth/login",
                    "method": "POST",
                    mediaType: 'application/json',
                    data: JSON.stringify({ userName: userName, password: password })
                };
                $.when(ApiObject.RequestWithOutAuth(settings)).then(function (data, textStatus) {
                    
                    var userData = JSON.parse(data);

                    if (userData.statusCode === 200) {

                        var userCookie = { userId: userData.id, userName: userData.userName, token: userData.auth_Token };

                        var existingUserData = LocalStorage.Get(LocalStorage.LocalStorageKey);

                        if (existingUserData !== "0") {

                            LocalStorage.DeleteLocalStorageWithKey(LocalStorage.LocalStorageKey);

                        }

                        LocalStorage.Set(LocalStorage.LocalStorageKey, userCookie);

                        $("#titlePane").empty();
                        $("#titlePane").append(userData.userName);
                        $(".panel-body").slideUp(500);

                        CalenderObject.GetEvents();

                    } else if (userData.statusCode === 400) {

                        alert(userData.description);

                    } else if (userData.statusCode === 401) {

                        alert(userData.description);
                    }

                });
            }
        });
    }

};