function userData() {
    let ud = {
        "name": $("#userName").val(),
        "mail": $("#usermail").val(),
        "surName": $("#userSurName").val(),
        "password": $("#userPassword").val(),
        "confirmPassword": $("#confirmPassword").val()
    };
    return ud;
}

function deneme() {
    if ($("#userName").val() !== '' && $("#userSurName").val() !== '' && $("#usermail").val() !== '' && $("#userPassword").val() !== '' && $("#confirmPassword").val() !== '') {

        if ($("#confirmPassword").val() == $("#userPassword").val()) {
        } else {
            alert("Şifreler birbirine eşit olmalı");
        }
    } else {
        alert("Tüm alanları doldurmanız lazım");
    }
}

$(document).ready(function () {
    console.log("ÇAĞRILIYOR");
    $("#regUser").click(function () {
        deneme();
        $.ajax({
            url: '/Register/Index',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(userData()),
            success: function (result) {
                alert("Başarıyla kayıt gerçekleşmiştir.");
            },
            error: function (xhr, status, error) {
                var errorMessage = xhr.status + ': ' + xhr.statusText;
                console.error('Hata oluştu: ', errorMessage);
            }
        });
    });
});
