
var CalenderObject = CalenderObject || {};
var FormObject = FormObject || {};
var LocalStorage = LocalStorage || {};

$(document).ready(function () {


    console.log("ready!");

    CalenderObject.CheckForUserTokenAndReValidate();
    CalenderObject.InitiateCalender();
    CalenderObject.GetEvents(123456);
    FormObject.BindLoggaIn();
    //LocalStorage.DeleteStorageOnBrowserClose();

});