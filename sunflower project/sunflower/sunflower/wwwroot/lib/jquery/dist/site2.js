//$(document).ready(function () {
//    // attach click event to the search button
//    $("#productSearch").click(function () {
//        var searchtext = $('#txtData').val(); // get the search input

//        // send ajax request to the SearchResults action
//        $.ajax({
//            url: '/Home/SearchResults',  // adjust the URL if necessary
//            type: 'GET',
//            data: { Name: searchtext }, // pass the search text to the action method
//            success: function (data) {
//                // replace only the product container's content
//                $('#productContainer').html(data);
//            },
//            error: function (xhr, status, error) {
//                console.log("error: " + error);
//            }
//        });
//    });
//});$(document).ready(function () {
// Handle form submission via AJAX

