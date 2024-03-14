function userData() {
    let ud = {
        "email": $("#userMail").val(),
        "password": $("#userPassword").val()
    };
    return ud;
}

function deneme() {
    if ($("#userMail").val() !== '' && $("#userPassword").val() !== '') {
    } else {
        alert("Tüm alanları doldurmanız lazım");
    }
}

$(document).ready(function () {
    console.log("ÇAĞRILIYOR");
    $("#logUser").click(function () {
        deneme();
        $.ajax({
            url: './Login',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(userData()),
            success: function (result) {
                console.log(result); 

                if (result.isSuccess) {
                    alert("Başarıyla giriş yapıldı");
                } else {
                    alert(result.message);
                }
            },
            error: function (xhr, status, error) {
                alert("Girdiğiniz şifre veya email yanlış");
                var errorMessage = xhr.status + ': ' + xhr.statusText;
                console.error('Hata oluştu: ', errorMessage);
            }
        });
    });
});
