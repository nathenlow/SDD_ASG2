$(document).ready(function () {

    //--------------on click functions--------------//
    // Edit username modal
    $(".editUsername").click(function (e) {
        $("#usernameModal").modal("show");
    });

    // Edit password modal
    $(".editPassword").click(function (e) {
        $("#passwordModal").modal("show");
    });
})