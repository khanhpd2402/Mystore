document.addEventListener('DOMContentLoaded', function () {
    var editUserNameBtn = document.getElementById('editUserNameBtn');
    var changePasswordBtn = document.getElementById('changePasswordBtn');
    var changpassDiv = document.getElementById('changpassDiv');
    var cancelEditUserNameBtn = document.getElementById('cancelEditUserNameBtn');
    var submitlEditUserNameBtn = document.getElementById('submitlEditUserNameBtn');
    var submitChangePassBtn = document.getElementById('submitChangePassBtn');
    var cancelChangePassBtn = document.getElementById('cancelChangePassBtn');
    var userNameInput = document.getElementById('username');
    var roleInput = document.getElementById('role');

    editUserNameBtn.addEventListener('click', function () {
        userNameInput.disabled = false; // Bỏ vô hiệu hóa cho ô input username
        editUserNameBtn.hidden = true;
        changePasswordBtn.hidden = true;
        cancelEditUserNameBtn.hidden = false;
        submitlEditUserNameBtn.hidden = false;
    });
    submitlEditUserNameBtn.addEventListener('click', function () {
        // Remove disabled attribute before form submission
        userNameInput.disabled = false;
    });

    cancelEditUserNameBtn.addEventListener('click', function () {
        userNameInput.disabled = true; // Bỏ vô hiệu hóa cho ô input username
        editUserNameBtn.hidden = false;
        changePasswordBtn.hidden = false;
        cancelEditUserNameBtn.hidden = true;
        submitlEditUserNameBtn.hidden = true;
    })
    changePasswordBtn.addEventListener('click', function () {
        changpassDiv.hidden = false;
        editUserNameBtn.hidden = true;
        changePasswordBtn.hidden = true;
        cancelChangePassBtn.hidden = false;
        submitChangePassBtn.hidden = false;
    });
    cancelChangePassBtn.addEventListener('click', function () {
        changpassDiv.hidden = true;
        editUserNameBtn.hidden = false;
        changePasswordBtn.hidden = false;
        cancelChangePassBtn.hidden = true;
        submitChangePassBtn.hidden = true;
    });
});
