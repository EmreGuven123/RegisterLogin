$(document).ready(function () {
    $(".OpenBlock").click(function () {
        let id = $(this).data("id");
        console.log("Tıklanan butonun id değeri: " + id);
        if (confirm("Blokeyi kaldırmak istediğinize emin misiniz")) {
            $.ajax({
                url: './AdminPanel',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ "id": id }),
                success: function (result) {
                    console.log(result);
                },
                error: function (xhr, status, error) {
                    console.error('AJAX Hatası: ', status, error);
                }
            });
        } else {
            return false;
        }

    });
    $(".deleteUser").click(function () {
        let id = $(this).data("id");
        if (confirm("Silmek istediğinize emin misiniz")) {
            $.ajax({
                url: './Delete',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ "id": id }),
                success: function (result) {
                    window.location.reload();
                }, error: function (xhr, status, error) {
                    console.error('AJAX Hatası: ', status, error);
                }
            });
        } else {
            return false;
        }
    });
});
