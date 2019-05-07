
var CalenderObject = CalenderObject || {};
var FormObject = FormObject || {};
var LocalStorage = LocalStorage || {};

$(document).ready(function () {


    console.log("ready!");

    CalenderObject.CheckForUserTokenAndReValidate();
    CalenderObject.InitiateCalender();
    CalenderObject.GetEvents();
    FormObject.LogInBinding();
    CalenderObject.GetAllGroups();
    FormObject.AddUser();
    CalenderObject.AddOrUpdateCalanderEvent();
    CalenderObject.DeleteEvent();
    //LocalStorage.DeleteStorageOnBrowserClose();

});