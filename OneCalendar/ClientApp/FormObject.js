
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

                        LocalStorage.Set(LocalStorage.KeyStart + userData.userName, userData.auth_Token);

                        $("#titlePane").empty();
                        $("#titlePane").append(userData.userName);
                        $(".panel-body").slideUp(500);

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