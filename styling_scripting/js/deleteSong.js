function deleteSong(songId) {
    songId -= 0;
    if (songId == NaN) {
        alert("why??")
        return;
    }
    const xhttp = new XMLHttpRequest();
    xhttp.onload = function () {
        switch (this.responseText) {
            case "success":
                document.getElementById(`SongBox${songId}`).remove();
                alert("song deleted");
                break;
            case "notAnAdmin":
                alert("you aren't an admin");
                break;
            default:
                alert(this.responseText);
                break;
        }
    }
    xhttp.open("GET", `../aspx/processer.aspx?action=deleteSong&song=${songId}`);
    xhttp.send();
}