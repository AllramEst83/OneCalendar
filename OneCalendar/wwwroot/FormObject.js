
var ApiObject = ApiObject || {};


var FormObject = {

    LogInBinding: function () {

        $("#loggaInKnapp").on("click", function (e) {
            e.preventDefault();

            var userName = $("#userNameInput").val().trim();
            var password = $("#passwordInput").val().trim();

            if (userName === '' || password === '') {
                CalenderObject.UserMessages.Show("Felmeddelande", "V&#228;nligen ange anv&#228;ndarnamn eller l&#246senord", "panel-danger");
                CalenderObject.UserMessages.Hide(6000);
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

                        var existingUserData = LocalStorage.Get(LocalStorage.KeyToUserData);

                        if (existingUserData !== "0") {

                            LocalStorage.DeleteLocalStorageWithKey(LocalStorage.KeyToUserData);

                        }

                        LocalStorage.Set(LocalStorage.KeyToUserData, userCookie);

                        $("#loginUserTitlePane").empty();
                        $("#loginUserTitlePane").append(userData.userName);
                        $(".loginUser").slideUp(500);
                        $(".loginPanel").removeClass("panel-default");
                        $(".loginPanel").addClass("panel-success");

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
            if (userName !== '' && password !== '' && groupId !== '' && firstName !== '' && lastName !== '') {
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
                            role: "admin_access",
                            groupId: groupId
                        })
                };

                $.when(ApiObject.RequestWithOutAuth(settings))
                    .then(function (data, textStatus) {
                        //var userData = JSON.parse(data);
                        if (textStatus === "success") {
                            if (data.statusCode === 200) {
                                CalenderObject.UserMessages.Show("Meddelande", data.description, "panel-info");
                            } else if (data.statusCode === 400) {
                                CalenderObject.UserMessages.Show("Felmeddelande", data.description, "panel-danger");
                            }else if (data.statusCode === 409) {
                                CalenderObject.UserMessages.Show("Felmeddelande", data.description, "panel-danger");
                            } else if (data.statusCode === 404) {
                                CalenderObject.UserMessages.Show("Felmeddelande", data.description, "panel-danger");                      
                            }

                            CalenderObject.UserMessages.Hide(6000);
                        }
                    });
            } else {
                console.log("please add userName and password");
                CalenderObject.UserMessages.Show("Felmeddelande", "V&#228;nligen ange anv&#228;ndarnamn, l&#246;senord, f&#246;rnamn och efternamn", "panel-danger");  
                CalenderObject.UserMessages.Hide(6000);
            }
        });
    },
    Logout: function () {
        $("#logoutButton").on('click', function (e) {
            e.preventDefault();

            console.log("%c Logga ut!", 'background: #222; color:#bada55');

            $('#calender').fullCalendar('removeEvents');
            LocalStorage.DeleteLocalStorageWithKey(LocalStorage.KeyToUserData);

            $(".loginPanel").removeClass("panel-success");
            $(".loginPanel").addClass("panel-default");
            $("#loginUserTitlePane").empty();
            $("#loginUserTitlePane").append("Logga in");
            $("#userNameInput,#passwordInput").val("");
            $(".loginUser").slideDown(500);
            //location.reload(true);
        });
    }

};