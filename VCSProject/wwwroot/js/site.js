// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var empId = $("#EmployeeId").attr('value');

function validateDOB() {
    var dob = document.getElementById("dob").value;
    var name = document.getElementById("name").value;
    var dobError = document.getElementById("dobError");
    var nameError = document.getElementById("nameError");
    var email = document.getElementById("email").value;
    var emailError = document.getElementById("emailError").value;

    if (!name) {
        document.getElementById("name").focus = "red";
        nameError.textContent = "Please enter name.";
        return false;
    }

    if (!dob) {
        document.getElementById("dob").focus = "red";
        dobError.textContent = "Please enter your date of birth.";
        return false;
    }

    if (!email) {
        document.getElementById("email").focus = "red";
        emailError.textContent = "Please enter email.";
        return false;
    }



    var today = new Date();
    today.setHours(0, 0, 0, 0); 

    var dobDate = new Date(dob);

    if (dobDate >= today) {
        dobError.textContent = "Date of birth must be less than the current date.";
        return false;
    }

    dobError.textContent = "";
    return true;
}






toastr.options = {
    "closeButton": true,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "timeOut": "3000",
    "extendedTimeOut": "1000" 
};


$('#employeeForm').submit(function (event) {
    event.preventDefault();

    var formData = new FormData(this);


    $.ajax({
        url: '/Employee/Create',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
           
            toastr.success('Saved Successfully', 'Success');

           
            setTimeout(function () {
                window.location.href = '/Employee/';
            }, 2000); 
        },
        error: function (xhr, status, error) {
            toastr.error('An error occurred: ' + error, 'Error');
        }
    });
});

$('#updateForm').submit(function (event) {
    event.preventDefault();

    var formData = new FormData(this);

    $.ajax({
        url: '/Employee/Edit/' + empId,
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
   
            toastr.success('Updated Successfully', 'Success');

            setTimeout(function () {
                window.location.href = '/Employee/';
            }, 2000); 
        },
        error: function (xhr, status, error) {
            toastr.error('An error occurred: ' + error, 'Error');
        }
    });
});

$('#deleteForm').submit(function (event) {
    event.preventDefault();

    var formData = new FormData(this);
    $.ajax({
        url: '/Employee/Delete/' + empId,
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
           
            toastr.success('Deleted Successfully', 'Success');

          
            setTimeout(function () {
                window.location.href = '/Employee/';
            }, 2000); 
        },
        error: function (xhr, status, error) {
            toastr.error('An error occurred: ' + error, 'Error');
        }
    });
});






