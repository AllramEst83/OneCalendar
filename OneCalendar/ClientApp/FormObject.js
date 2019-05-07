
var ApiObject = ApiObject || {};


var FormObject = {

    LogInBinding: function () {

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
                        $(".loginUser").slideUp(500);

                        CalenderObject.GetEvents();

                    } else if (userData.statusCode === 400) {

                        alert(userData.description);

                    } else if (userData.statusCode === 401) {

                        alert(userData.description);
                    }

                });
            }
        });
    },
    AddUser: function () {

        $("#addUser").on('click', function (e) {
            e.preventDefault();

            var firstName = $("#firstName").val();
            var lastName = $("#lastName").val();
            var userName = $("#addUserName").val();
            var password = $("#addPassword").val();
            var groupId = $('#groupSelection option:selected').val();
            if (userName !== '' || password !== '' || groupId !== '') {
                var settings = {
                    "url": "https://localhost:44305/api/auth/signup",
                    "method": "POST",
                    mediaType: 'application/json',
                    data: JSON.stringify(
                        {
                            firstName: firstName,
                            lastName: lastName,
                            email: userName,
                            password: password,
                            role:"admin_access",
                            groupId: groupId
                        })
                };

                $.when(ApiObject.RequestWithOutAuth(settings))
                    .then(function () {

                    });
            } else {
                console.log("please add userName and password");
            }
        });
    }

};