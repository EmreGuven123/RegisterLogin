function resetPassword() {
    let rp = {
        "mail": $("#usermail").val(),
        "Password": $("#userPassword").val(),
        "confirmPassword": $("#confirmPassword").val()
    };
    return rp;
}

function controlEdit() {
    if ($("#usermail").val() !== '' && $("#userPassword").val() !== '' && $("#confirmPassword").val() !== '') {
        if ($("#confirmPassword").val() !== $("#userPassword").val()) {
            alert("Şifreler birbirine eşit olmalı");
        }    
    } else {
        alert("Tüm alanları doldurmanız gerekli");
    }
}

$(document).ready(function () {
    $("#resetUser").click(function () {
        controlEdit()
        $.ajax({
            url: './ForgetPassword',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(resetPassword()),
            success: function (result) {
                alert("Başarıyla şifre yenileme işlemi tamamlandı");
            },
            error: function (xhr, status, error) {
                var errorMessage = xhr.status + ': ' + xhr.statusText;
                console.error('Hata oluştu: ', errorMessage);
            }
        });
    });
});
