// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let regDate = $("td#regDate").text();
regDate = regDate.substring(0,10);
$("td#regDate").text(regDate);

let dob = $("td#dob").text();
dob = dob.substring(0,10);
$("td#dob").text(dob);
